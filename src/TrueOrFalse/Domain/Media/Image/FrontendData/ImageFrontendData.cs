using System.Net;
using static System.String;

public class ImageFrontendData
{ 
    public ImageMetaData ImageMetaData;
    public bool ImageMetaDataExists;
    public MainLicenseInfo MainLicenseInfo;
    public string OriginalFileLink;
    public ManualImageEvaluation ManualImageEvaluation;
    public bool ImageCanBeDisplayed;
    public bool HasAuthorizedMainLicense;
    public bool LicenseDataIncomplete;
    public LicenseImage MainLicense;
    public string Author;
    public string Description;
    public string LicenseName;
    public string LicenseLink;
    public string LicenseShortDescriptionLink;
    public string AttributionHtmlString;
    public ImageParsingNotifications ImageParsingNotifications;

    public ImageFrontendData(int typeId, ImageType imageType) : this(PrepareConstructorArguments(typeId, imageType))
    {
    }
        
    public ImageFrontendData(ImageMetaData imageMetaData)
    {
        if (imageMetaData == null)
            return;

        ImageMetaDataExists = true;
        ImageMetaData = imageMetaData;
        MainLicenseInfo = !IsNullOrEmpty(ImageMetaData.MainLicenseInfo)
            ? MainLicenseInfo.FromJson(ImageMetaData.MainLicenseInfo)
            : null;

        if (ImageMetaData.Source == ImageSource.WikiMedia)
            OriginalFileLink = LicenseParser.GetWikiDetailsPageFromSourceUrl(ImageMetaData.SourceUrl);

        ManualImageEvaluation = imageMetaData.ManualEntriesFromJson().ManualImageEvaluation;
        ImageCanBeDisplayed = ManualImageEvaluation != ManualImageEvaluation.ImageManuallyRuledOut;

        Description = !IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded)
            ? ImageMetaData.ManualEntriesFromJson().DescriptionManuallyAdded
            : ImageMetaData.DescriptionParsed;

        if (!ImageCanBeDisplayed)
        {
            AttributionHtmlString = "Das Bild kann aus lizenzrechtlichen Gründen leider zur Zeit nicht angezeigt werden. ";

            if (!IsNullOrEmpty(OriginalFileLink))
                AttributionHtmlString += $"Hier findest du die <a href='{OriginalFileLink}' target='_blank'>Originaldatei</a>.";
        }
        else //Image can be displayed
        {
            Fill_Author();

            if (IsAuthorizedLicense())
                FillFor_Authorized_License();
            else
                FillFor_NonAuthorized_License();
        }

        ImageParsingNotifications = ImageParsingNotifications.FromJson(ImageMetaData.Notifications);
    }

    private void Fill_Author()
    {
        if (!IsNullOrEmpty(MainLicenseInfo?.Author))
        {
            Author = MainLicenseInfo.Author;
        }
        else
        {
            LicenseDataIncomplete = true;
            Author = !IsNullOrEmpty(ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded)
                ? ImageMetaData.ManualEntriesFromJson().AuthorManuallyAdded
                : ImageMetaData.AuthorParsed;
        }
    }

    public static ImageFrontendData Create(Question question) => new ImageFrontendData(question.Id, ImageType.Question);
    public static ImageFrontendData Create(Set set) => new ImageFrontendData(set.Id, ImageType.QuestionSet);

    private bool IsAuthorizedLicense()
    {
        return MainLicenseInfo != null && ManualImageEvaluation == ManualImageEvaluation.ImageCheckedForCustomAttributionAndAuthorized;
    }

    private void FillFor_Authorized_License()
    {
        HasAuthorizedMainLicense = true;
        MainLicense = LicenseImageRepo.GetById(MainLicenseInfo.MainLicenseId);
        if (MainLicense != null)
        {
            LicenseName = !IsNullOrEmpty(MainLicense.LicenseShortName)
                ? MainLicense.LicenseShortName
                : MainLicense.LicenseLongName;
            LicenseLink = MainLicense.LicenseLink;
            LicenseShortDescriptionLink = MainLicense.LicenseShortDescriptionLink;
        }

        AttributionHtmlString =
            $"<span class='InfoLabel'>Bild</span>: {(!IsNullOrEmpty(Author) ? Author + ", " : "")}";

        if (ImageMetaData.Source == ImageSource.WikiMedia && !IsNullOrEmpty(OriginalFileLink))
        {
            AttributionHtmlString +=
                $"<a href='{OriginalFileLink}' target='_blank'>Wikimedia Commons</a>";
        }

        if (IsNullOrEmpty(LicenseName))
        {
            LicenseDataIncomplete = true;
        }
        else //LicenseDataIncomplete
        {
            if (!IsNullOrEmpty(LicenseLink))
            {
                AttributionHtmlString += $", Lizenz: <a href='{LicenseLink}' target='_blank'>{LicenseName}</a>";

                if (!IsNullOrEmpty(LicenseShortDescriptionLink))
                {
                    AttributionHtmlString += $" (<a href='{LicenseShortDescriptionLink}' target='_blank'>Kurzfassung</a>)";
                }
            }
            else if (MainLicense != null && MainLicense.LicenseLinkRequired == true)
            {
                LicenseDataIncomplete = true;
            }
            else
            {
                AttributionHtmlString += ", Lizenz: " + LicenseName;
            }
        }
    }

    private void FillFor_NonAuthorized_License()
    {
        AttributionHtmlString =
            $"<span class='InfoLabel'>Bild:</span> {(!IsNullOrEmpty(Author) ? Author + ", " : "")}";

        if (!IsNullOrEmpty(OriginalFileLink))
        {
            var source = "";

            if (ImageMetaData.Source == ImageSource.WikiMedia)
                source = "(Wikimedia Commons)";

            AttributionHtmlString +=
                $"<a href='{OriginalFileLink}' target='_blank'>Bildquelle und erweiterte Information {source}</a>.";
        }
        else
        {
            AttributionHtmlString +=
                "Die Lizenzangaben werden zur Zeit geprüft und aufbereitet. ";
        }
    }

    public ImageUrl GetImageUrl(int width, bool asSquare = false, bool getDummy = false, ImageType imageTypeForDummy = ImageType.Question)
    {
        IImageSettings imageSettings;
        var typeId = getDummy ? -1 : (ImageMetaDataExists ? ImageMetaData.TypeId : -1);
        var imageType = ImageMetaDataExists ? ImageMetaData.Type : imageTypeForDummy;

        switch (imageType)
        {
            case ImageType.Category:
                imageSettings = new CategoryImageSettings(typeId);
                break;
            case ImageType.User:
                imageSettings = new UserImageSettings(typeId);
                break;
            case ImageType.QuestionSet:

                if(ImageMetaDataExists && ImageMetaData.IsYoutubePreviewImage)
                    return new SetImageSettings(typeId).GetUrl(width, asSquare);

                imageSettings = new SetImageSettings(typeId);
                break;
            default:
                imageSettings = new QuestionImageSettings(typeId);
                break;
        }

        return ImageUrl.Get(imageSettings, width, asSquare, arg => ImageUrl.GetFallbackImageUrl(imageSettings, width));
    }

    public string RenderHtmlImageBasis(
        int width, 
        bool asSquare, 
        ImageType imageTypeForDummies, 
        string insertLicenseLinkAfterAncestorOfClass = "ImageContainer", 
        string additionalCssClasses = "",
        string linkToItem = "",
        bool noFollow = false)
    {
        var imageUrl = GetImageUrl(width, asSquare, false, imageTypeForDummies);

        if(additionalCssClasses != "")
            additionalCssClasses = " " + additionalCssClasses;

        if (ImageMetaDataExists && imageUrl.HasUploadedImage || ImageMetaData != null && ImageMetaData.IsYoutubePreviewImage)
        {
            if (!ImageCanBeDisplayed)
                additionalCssClasses += " JS-CantBeDisplayed";

            var dataIsYoutubeVide = "";
            if (ImageMetaData.IsYoutubePreviewImage)
                dataIsYoutubeVide = $" data-is-youtube-video='{ImageMetaData.YoutubeKey}' ";

            var altDescription = IsNullOrEmpty(this.Description) ?
                "" : 
                WebUtility.HtmlEncode(this.Description)
                    .Replace("\"", "'")
                    .Replace("„", "'")
                    .Replace("“", "'")
                    .StripHTMLTags()
                    .Truncate(120, true);

            return AddLink(
                    "<img src='" + GetImageUrl(width, asSquare, true, imageTypeForDummies).Url + "' " + //Dummy url gets replaced by javascript (look for class: LicensedImage) to avoid displaying images without license in case of no javascript
                    "class='ItemImage LicensedImage JS-InitImage" + additionalCssClasses + "' " + 
                    "data-image-id='" + ImageMetaData.Id + "' data-image-url='" + imageUrl.Url + "' " + dataIsYoutubeVide + 
                    "data-append-image-link-to='" + insertLicenseLinkAfterAncestorOfClass + "' " +
                    "alt='" + altDescription + "'/>",
                    linkToItem, noFollow);
        }
        
        return AddLink( //if no image, then display dummy picture
            "<img src='" + imageUrl.Url 
            + "' class='ItemImage JS-InitImage" + additionalCssClasses
            + "' data-append-image-link-to='" + insertLicenseLinkAfterAncestorOfClass 
            + "' alt=''/>",
            linkToItem, noFollow);
    }

    private static string AddLink(string html, string link, bool noFollow = false)
    {
        if(link == "")
            return html;

        var noFollowString = noFollow ? " rel='nofollow'" : "";
        return $"<a href='{link}'{noFollowString}>{html}</a>";

    }

    private static ImageMetaData PrepareConstructorArguments(int typeId, ImageType imageType)
    {
        return Sl.ImageMetaDataRepo.GetBy(typeId, imageType);
    }
}
