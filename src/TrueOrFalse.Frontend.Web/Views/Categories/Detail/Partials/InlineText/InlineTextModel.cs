using System.Security.AccessControl;
using TrueOrFalse.Web;

public class InlineTextModel : BaseContentModule
{
    public string Content;
    public string Raw;
    public bool ShowContent;
    public InlineTextModel(string htmlContent, bool isInWishknowledge = false)
    {
        Raw = htmlContent;
        Content = MarkdownMarkdig.ToHtml(htmlContent);
        ShowContent = !_sessionUser.ShowOwnWorld || isInWishknowledge;
    }
}
