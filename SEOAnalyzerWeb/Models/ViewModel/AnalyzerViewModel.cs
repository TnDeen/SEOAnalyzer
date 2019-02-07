using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SEOAnalyzerWeb.Models.ViewModel
{
    public class AnalyzerViewModel
    {
        public Analyzer Analyzer { get; set; }
        public Dictionary<string, int> AllWordsInfo { get; set; } = new Dictionary<string, int>();
        public List<MetaTagInfo> AllMetaTagsInfo { get; set; } = new List<MetaTagInfo>();
        public Dictionary<string, int> AllExternalLinks { get; set; } = new Dictionary<string, int>();
    }
}