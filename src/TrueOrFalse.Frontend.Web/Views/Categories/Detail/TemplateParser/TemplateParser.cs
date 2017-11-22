using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Newtonsoft.Json;

public class TemplateParser
{
    public static string Run(string stringToParse, Category category, ControllerContext controllerContext)
    {
        //Matches "[[something]]" (optionally with surrounding p tag) non-greedily across multiple lines and only if not nested
        var regex = new Regex(@"(<p>)?\[\[(.*?)\]\](<\/p>)?", RegexOptions.Singleline);

        return regex.Replace(stringToParse, match =>
        {
            try
            {
                var templateJson = GetTemplateJson(
                    match.Value
                        .Replace("<p>[[", "")
                        .Replace("]]</p>", "")
                        .Replace("[[", "")
                        .Replace("]]", "")
                        .Replace("&quot;", @""""),
                    category.Id);

                var html = GetHtml(templateJson, category, controllerContext);

                if (string.IsNullOrEmpty(html))
                    throw new Exception("Es konnte kein Html erzeugt werden.");

                return html;

            }
            catch (Exception e)
            {
                Logg.r().Error($"Fehler beim Parsen der Kategorie Id={category.Id} ({e.Message} {e.StackTrace}).");
                return GetReplacementForNonparsableTemplate(match.Value, e.Message);
            }

        });
    }

    private static TemplateJson GetTemplateJson(string template, int categoryId)
    {
        var templateJson = JsonConvert.DeserializeObject<TemplateJson>(template);
        templateJson.ContainingCategoryId = categoryId;
        return templateJson;
    }

    private static string GetHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
            case "medialist":
            case "videowidget":
            case "settestsessionnostartscreen":
            case "singlesetfullwidth":
            case "singlecategoryfullwidth":
            case "categorynetwork":
            case "contentlists":
            case "educationofferlist":
            case "singleset":
            case "setlistcard":
            case "setcardminilist":
            case "singlecategory":
            case "singlequestionsquiz":
            case "spacer":
                return GetPartialHtml(templateJson, category, controllerContext);
            default:
            {
                var elementHtml = GetElementHtml(templateJson);

                if (string.IsNullOrEmpty(elementHtml))
                    throw new Exception($"Name des Templates '{templateJson.TemplateName}' ist unbekannt.");

                return elementHtml;
            }
        }
    }

    private static string GetPartialHtml(TemplateJson templateJson, Category category, ControllerContext controllerContext)
    {
        var partialModel = GetPartialModel(templateJson, category);

        return GetPartialHtml(templateJson, controllerContext, partialModel);
    }


    private static string GetPartialHtml(TemplateJson templateJson, ControllerContext controllerContext, BaseModel partialModel)
    {
        return ViewRenderer.RenderPartialView(
            "~/Views/Categories/Detail/Partials/" + templateJson.TemplateName + ".ascx",
            partialModel,
            controllerContext);
    }

    private static BaseModel GetPartialModel(TemplateJson templateJson, Category category)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "topicnavigation":
                return new TopicNavigationModel(category, templateJson.Title, templateJson.Text, templateJson.Load, templateJson.Order);
            case "medialist":
                return new MediaListModel(category, templateJson.Title, templateJson.Text);
            case "educationofferlist":
                return new EducationOfferListModel(category, templateJson.Title, templateJson.Text, templateJson.Load, templateJson.Order);
            case "videowidget":
                return new VideoWidgetModel(templateJson.SetId);
            case "settestsessionnostartscreen":
                return new SetTestSessionNoStartScreenModel(templateJson.SetId, templateJson.Title, templateJson.Text);
            case "singlesetfullwidth":
                return new SingleSetFullWidthModel(templateJson.SetId, templateJson.Title, templateJson.Text);
            case "singlecategoryfullwidth":
                return  new SingleCategoryFullWidthModel(templateJson.CategoryId, templateJson.Title, templateJson.Description);
            case "categorynetwork":
            case "contentlists":
                return new CategoryModel(category, loadKnowledgeSummary : false);
            case "singleset":
                return new SingleSetModel(Sl.R<SetRepo>().GetById(templateJson.SetId), setText: templateJson.SetText);
            case "setlistcard":
                return new SetListCardModel(
                    templateJson.SetList,
                    templateJson.Title,
                    templateJson.Description,
                    category.Id,
                    templateJson.TitleRowCount,
                    templateJson.DescriptionRowCount,
                    templateJson.SetRowCount);
            case "setcardminilist":
                return new SetCardMiniListModel(templateJson.SetList);
            case "singlecategory":
                return new SingleCategoryModel(
                    templateJson.CategoryId,
                    templateJson.Description);
            case "singlequestionsquiz":
                return new SingleQuestionsQuizModel(category, templateJson.MaxQuestions, templateJson.Title, templateJson.Text, templateJson.QuestionIds, templateJson.Order);
            case "spacer":
                return new SpacerModel(templateJson.AmountSpaces, templateJson.AddBorderTop);
            default:
                throw new Exception("Kein Model für diese Template hinterlegt.");
        }
    }

    private static string GetElementHtml(TemplateJson templateJson)
    {
        switch (templateJson.TemplateName.ToLower())
        {
            case "divstart":
                return "<div class='" + templateJson.CssClasses + "'>";
            case "divend":
                return "</div>";
            default:
                return null;
        }
    }

    /// <summary>
    /// /If template cannot be parsed show it for admins, otherwise hide it
    /// </summary>
    /// <param name="match"></param>
    /// <returns></returns>
    private static string GetReplacementForNonparsableTemplate(string match, string exceptionMessage)
    {
        return Sl.SessionUser.IsInstallationAdmin 
            ? $"<div style=\'background-color: rgba(130, 8, 22, 0.33); margin-bottom: 20px;\'>Folgendes Template konnte nicht umgewandelt werden:<div>{match}</div>Fehler: {exceptionMessage}</div>"
            : "";
    }
}