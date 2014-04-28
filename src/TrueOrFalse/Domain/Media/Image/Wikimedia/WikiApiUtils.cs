﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TrueOrFalse
{
    public class WikiApiUtils
    {
        public static string GetWebpage(string url)
        {
            var webRequest = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            webRequest.UserAgent = "TrueOrFalseBot/1.0 (http://www.memucho.de/)";

            string resultString;
            using (var response = webRequest.GetResponse())
            {
                var stream = new StreamReader(response.GetResponseStream());
                resultString = stream.ReadToEnd();
            }

            return resultString;
        }

        public static string ExtractFileNameFromUrl(string filePath)
        {
            //remove query string
            filePath = filePath.Split('?')[0];

            if (filePath.Contains("File:"))
                return filePath.Split(new[] { "File:" }, StringSplitOptions.None)[1];

            return filePath;
        }
    }
}
