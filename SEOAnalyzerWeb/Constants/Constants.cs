using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEOAnalyzerWeb.Constants
{
    public class Constant
    {
        public const string StopWordsPath = "Configs/stopWords.json";
    }

    public class FilterFormat
    {
        public const string GetAllLinks = @"((http|ftp|https):\/\/[\w\-_]+(\.[\w\-_]+)+([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])?)";
    }
}