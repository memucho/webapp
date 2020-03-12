using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Seedworks.Lib;
using TrueOrFalse.Web;

public class EditQuestionModel : BaseModel
{
    public UIMessage Message;

    public QuestionVisibility Visibility { get; set; }

    public Question Question;

    //Validation serves as backup for client side validation
    [Required(ErrorMessage="Du musst eine Frage eingeben.")]
    [DataType(DataType.MultilineText )]
    [DisplayName("Frage")]
    public string QuestionText { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Frage erweitert")]
    public string QuestionExtended { get; set; }

    [Required]
    [DisplayName("Antworttyp")]
    public string SolutionType { get; set; }

    [DataType(DataType.MultilineText)]
    [DisplayName("Ergänzungen")]
    public string Description { get; set; }

    public int LicenseId { get; set; }

    public IEnumerable<SelectListItem> LicenseDropdownList
    {
        get {
            if(_licenseDropdownList == null)

                return LicenseQuestionRepo.GetAllRegisteredLicenses()
                .Select(l => new SelectListItem
                {
                    Selected = IsEditing ? l.Id == LicenseId : l.IsDefault(),
                    Text = l.NameShort,
                    Value = l.Id.ToString()//http://stackoverflow.com/a/782030
                });

            return _licenseDropdownList;
        }
    }

    public IEnumerable<SelectListItem> _licenseDropdownList;

    public int Id = -1;

    public IList<Category> Categories = new List<Category>();
    public IList<Reference> References = new List<Reference>();
    public IList<Reference> EscapedReferences => EscapeReferenceText(References);

    public string PageTitle;

    public string FormTitle;
    public bool ShowSaveAndNewButton;
    public string ImageUrl_128;
    public string SoundUrl;
    public bool IsEditing;
    private string _pageTitle;

    public IEnumerable<SelectListItem> VisibilityData =>  new List<SelectListItem>
    {
        new SelectListItem {Text = "Alle", Value = QuestionVisibility.All.ToString()},
        new SelectListItem {Text = "Nur Ich", Value = QuestionVisibility.Owner.ToString()},
        new SelectListItem {Text = "Ich und meine Freunde", Value = QuestionVisibility.OwnerAndFriends.ToString()},
    };

    public IEnumerable<SelectListItem> AnswerTypeData => new List<SelectListItem>
    {
        new SelectListItem {Text = "Multiple Choice (eine richtige Antwort)", Value = TrueOrFalse.SolutionType.MultipleChoice_SingleSolution.ToString()},
        new SelectListItem {Text = "Multiple Choice (mehrere richtige Antworten)", Value = TrueOrFalse.SolutionType.MultipleChoice.ToString()},
        new SelectListItem {Text = "Freie Antwort (Text/Zahl/Datum)", Value = TrueOrFalse.SolutionType.Text.ToString()},
        //new SelectListItem {Text = "Sequenz (Liste)", Value = TrueOrFalse.SolutionType.Sequence.ToString()},
        new SelectListItem {Text = "Zuordnung (Liste)", Value = TrueOrFalse.SolutionType.MatchList.ToString()},
        new SelectListItem {Text = "Karteikarte", Value = TrueOrFalse.SolutionType.FlashCard.ToString()}
    };

    [DisplayName("Charakter")]
    public IEnumerable<SelectListItem> CharacterData => new List<SelectListItem>
    {
        new SelectListItem {Text = "Ernsthaft", Value = QuestionCharacter.Serious.ToString()},
        new SelectListItem {Text = "Unterhaltsam", Value = QuestionCharacter.Entertaining.ToString()}
    };
    public EditQuestionModel()
    {
        ImageUrl_128 = "";
        SoundUrl = "";
    }

    public EditQuestionModel(Question question)
    {
        Id = question.Id;
        Question = question;
        QuestionText = question.Text;
        QuestionExtended = question.TextExtended;
        SolutionType = question.SolutionType.ToString();
        Description = question.Description;
        Categories = question.Categories;
        References = question.References;
        LicenseId = question.LicenseId;
        ImageUrl_128 = QuestionImageSettings.Create(question.Id).GetUrl_500px().Url;
        SoundUrl = new GetQuestionSoundUrl().Run(question);
        Visibility = question.Visibility;
    }

    public void FillCategoriesFromPostData(NameValueCollection postData)
    {
        Categories = AutocompleteUtils.GetRelatedCategoriesFromPostData(postData);
    }

    public IList<Reference> FillReferencesFromPostData(HttpRequestBase request, Question question)
    {
        var referencesJson = request["hddReferencesJson"];
        if(String.IsNullOrEmpty(referencesJson))
            References = new List<Reference>();

        return References = ReferenceJson.LoadFromJson(referencesJson, question);
    }

    public void SetToCreateModel()
    {
        IsEditing = false;
        PageTitle = FormTitle = "Frage erstellen";
        ShowSaveAndNewButton = true;
    }

    public void SetToUpdateModel()
    {
        PageTitle = "Frage bearbeiten ("+ QuestionText.Truncate(30, "...") + ")";
        FormTitle = string.Format("Frage '{0}' bearbeiten", QuestionText.TruncateAtWord(30));
        IsEditing = true;
        ShowSaveAndNewButton = false;
    }

    public void Reset()
    {
        Id = -1;
        QuestionText = "";
        Description = "";
        Categories = new List<Category>();
    }

    private IList<Reference> EscapeReferenceText(IList<Reference> questionReferences)
    {
        foreach (var reference in questionReferences)
        {
            switch (reference.ReferenceType)
            {
                    case ReferenceType.FreeTextreference:
                        var newString = EscapeChars(reference.ReferenceText);
                    reference.ReferenceText = EscapeChars(reference.ReferenceText);
                        break;

                    case ReferenceType.UrlReference:
                        reference.AdditionalInfo = EscapeChars(reference.AdditionalInfo);
                        break;
            }
        }
        return questionReferences;
    }

    private string EscapeChars(string objectString)
    {
        return objectString.Replace(Environment.NewLine, "\\n").Replace("\r\n", "\\n").Replace("\n", "\\n").Replace("\r", "\\n")
            .Replace("\'", @"\'").Replace("\"", "\\\"").Replace(@"\t", " ");
    }
}