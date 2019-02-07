using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace SEOAnalyzerWeb.Models
{
    public class Analyzer
    {
        public string WordSortBy { get; set; }
        public string MetaSortBy { get; set; }
        public string LinkSortBy { get; set; }
        [DisplayName("Text/Url")]
        public string SearchText { get; set; }
        [DisplayName("IsURL")]
        public bool IsURL { get; set; } = true;
        [DisplayName("Filter Stop-Words")]
        public bool IsPageFilterStopWords { get; set; } = true;
        [DisplayName("Count Number of Words")]
        public bool IsCountNumberofWords { get; set; } = true;
        [DisplayName("Count Meta Tags")]
        public bool IsMetaTagsInfo { get; set; } = true;
        [DisplayName("Count External Link")]
        public bool IsGetExternalLink { get; set; } = true;
    }

    public class MetaTagInfo
    {
        public string Name { get; set; }
        public string Property { get; set; }
        public string ItemProp { get; set; }
        public string HttpEquiv { get; set; }
        public string Content { get; set; }
        public Dictionary<string, int> URLInfoList { get; set; }
        public Dictionary<string, int> WordsInfoList { get; set; }
        public int TotalWordCount { get; set; }
    }
}