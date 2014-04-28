using System;
using System.IO;
using System.Net;
using TrueOrFalse.WikiMarkup;

namespace TrueOrFalse
{
    public class WikiImageLicenceLoader : IRegisterAsInstancePerLifetime
    {
        public WikiImageLicenceInfo Run(string fileName)
        {
            fileName = WikiApiUtils.ExtractFileNameFromUrl(fileName);

            var url = String.Format("http://commons.wikimedia.org/w/index.php?title=File:{0}&action=raw", fileName);
            var page = WikiApiUtils.GetWebpage(url);

            var parsedImageMakup = ParseImageMarkup.Run(page);
            var licenceInfo = new WikiImageLicenceInfo
            {
                AuthorName = parsedImageMakup.AuthorName,
                Description = parsedImageMakup.Description,
                Markup = page
            };

            return licenceInfo;
        }
    }
}