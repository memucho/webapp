using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.Mvc;
using TrueOrFalse.Frontend.Web.Code;
using TrueOrFalse.Web;

public class EditCategoryController : BaseController
{
    private readonly CategoryRepository _categoryRepository;
    private const string _viewPath = "~/Views/Categories/Edit/EditCategory.aspx";
    private const string _viewPathTypeControls = "~/Views/Categories/Edit/TypeControls/{0}.ascx";

    public EditCategoryController(CategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        ActionInvoker = new JavaScriptActionInvoker();
    }

    [SetMenu(MenuEntry.Categories)]
    public ViewResult Create(string name, string parent, string type)
    {
        var model = new EditCategoryModel {Name = name ?? "", PreselectedType = !String.IsNullOrEmpty(type) ? (CategoryType)Enum.Parse(typeof(CategoryType), type) : CategoryType.Standard };

        if (!String.IsNullOrEmpty(parent))
            model.ParentCategories.Add(_categoryRepository.GetById(Convert.ToInt32(parent)));

        return View(_viewPath, model);
    }

    [SetMenu(MenuEntry.Categories)]
    public ViewResult Edit(int id)
    {
        var category = _categoryRepository.GetById(id);

        if(!IsAllowedTo.ToEdit(category))
            throw new SecurityException("Not allowed to edit categoty");

        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Edit));
        
        var model = new EditCategoryModel(category){IsEditing = true};
        //model.DescendantCategories = Sl.R<CategoryRepository>().GetDescendants(category.Type, category.Type, category.Id).ToList();

        if (TempData["createCategoryMsg"] != null)
            model.Message = (SuccessMessage)TempData["createCategoryMsg"];

        return View(_viewPath, model);
    }

    [HttpPost]
    [SetMenu(MenuEntry.Categories)]
    public ViewResult Edit(int id, EditCategoryModel model, HttpPostedFileBase file)
    {
        var category = _categoryRepository.GetById(id);
        _sessionUiData.VisitedCategories.Add(new CategoryHistoryItem(category, HistoryItemType.Edit));

        if (!IsAllowedTo.ToEdit(category))
            throw new SecurityException("Not allowed to edit categoty");

        var categoryAllowed = new CategoryNameAllowed();

        model.FillReleatedCategoriesFromPostData(Request.Form);
        model.UpdateCategory(category);
        if (model.Name != category.Name && categoryAllowed.No(model, category.Type)){
            model.Message = new ErrorMessage(string.Format("Es existiert bereits ein Thema mit dem Namen <strong>'{0}'</strong>.",
                                                            categoryAllowed.ExistingCategories.First().Name));
        } else {
            _categoryRepository.Update(category);

            model.Message = new SuccessMessage("Das Thema wurde gespeichert.");
        }
        StoreImage(id);
        
        model.Init(category);
        model.IsEditing = true;
        model.DescendantCategories = Sl.R<CategoryRepository>().GetDescendants(category.Id).ToList();

        return View(_viewPath, model);
    }

    [HttpPost]
    [SetMenu(MenuEntry.Categories)]
    public ActionResult Create(EditCategoryModel model, HttpPostedFileBase file)
    {                
        model.FillReleatedCategoriesFromPostData(Request.Form);
        
        var convertResult = model.ConvertToCategory();
        if (convertResult.HasError){

            if(convertResult.TypeModel == null)
                throw new Exception("Dear developer, please assign the type model!");

            EditCategoryTypeModel.SaveToSession(convertResult.TypeModel, convertResult.Category);
            model.Message = convertResult.ErrorMessage;
            return View(_viewPath, model);
        }

        var category = convertResult.Category;
        category.Creator = _sessionUser.User;

        var categoryNameAllowed = new CategoryNameAllowed();
        if (categoryNameAllowed.No(category))
        {
            model.Message = new ErrorMessage(
                string.Format("Das Thema <strong>'{0}'</strong> existiert bereits. " +
                              "<a href=\"{1}\">Klicke hier</a>, um es zu bearbeiten.",
                              categoryNameAllowed.ExistingCategories.First().Name,
                              Links.CategoryEdit(categoryNameAllowed.ExistingCategories.First()))); //Url.Action("Edit", new { id = categoryNameAllowed.ExistingCategories.First().Id })

            return View(_viewPath, model);
        }


        _categoryRepository.Create(category);
        StoreImage(category.Id);

        EditCategoryTypeModel.RemoveRecentTypeModelFromSession();

        TempData["createCategoryMsg"] 
            = new SuccessMessage(string.Format(
                 "Das Thema <strong>'{0}'</strong> wurde angelegt.<br>" + 
                 "Du kannst das Thema jetzt bearbeiten" +
                 " oder ein <a href=\"{1}\">neues Thema anlegen</a>.", 
                category.Name,
                Links.CategoryCreate()));

        return Redirect(Links.CategoryEdit(category));
    }

    public ActionResult DetailsPartial(int? categoryId, CategoryType type, string typeModelGuid)
    {
        Category category = null;

        if (categoryId.HasValue && categoryId.Value > 0){
            category = _categoryRepository.GetById(categoryId.Value);
        }

        return View(string.Format(_viewPathTypeControls, type), new EditCategoryTypeModel(category, type));
    }

    private void StoreImage(int categoryId)
    {
        if (Request["ImageIsNew"] == "true")
        {
            if (Request["ImageSource"] == "wikimedia")
            {
                Resolve<ImageStore>().RunWikimedia<CategoryImageSettings>(
                    Request["ImageWikiFileName"], categoryId, ImageType.Category, _sessionUser.User.Id);
            }
            if (Request["ImageSource"] == "upload")
            {
                Resolve<ImageStore>().RunUploaded<CategoryImageSettings>(
                    _sessionUiData.TmpImagesStore.ByGuid(Request["ImageGuid"]), categoryId, _sessionUser.User.Id, Request["ImageLicenseOwner"]);
            }
        }
    }

    [HttpPost]
    public JsonResult GetMarkdownPreview(int categoryId, string text)
    {
        var category = Sl.CategoryRepo.GetByIdEager(categoryId);
        category.TopicMarkdown = text;

        Sl.Session.Evict(category); //prevent change tracking and updates

        return Json(MarkdownToHtml.Run(category, ControllerContext));
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void EditAggregation(int categoryId, string categoriesToExcludeIdsString, string categoriesToIncludeIdsString)
    {
        var category = _categoryRepository.GetById(categoryId);

        category.CategoriesToExcludeIdsString = categoriesToExcludeIdsString;
        category.CategoriesToIncludeIdsString = categoriesToIncludeIdsString;

        ModifyRelationsForCategory.UpdateRelationsOfTypeIncludesContentOf(category);
    }

    [HttpPost]
    [AccessOnlyAsAdmin]
    public void ResetAggregation(int categoryId)
    {
        var catRepo = Sl.CategoryRepo;

        var category = catRepo.GetById(categoryId);

        var relationsToRemove =
            category.CategoryRelations.Where(r => r.CategoryRelationType == CategoryRelationType.IncludesContentOf).ToList();

        foreach (var relation in relationsToRemove)
        {
            category.CategoryRelations.Remove(relation);
        }

        catRepo.Update(category);
    }

    public ActionResult GetEditCategoryAggregationModalContent(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        return View("~/Views/Categories/Modals/EditAggregationModal.ascx", new EditCategoryModel(category));
    }

    public string GetCategoryGraphDisplay(int categoryId)
    {
        var category = Sl.CategoryRepo.GetById(categoryId);
        return ViewRenderer.RenderPartialView("~/Views/Categories/Edit/GraphDisplay/CategoryGraph.ascx", new CategoryGraphModel(category), ControllerContext);
    }
}