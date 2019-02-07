using SEOAnalyzerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEOAnalyzerWeb.Service.Interface
{
    public interface IAnalysisService : IDisposable
    {
        Task<Dictionary<string, int>> GetAllWordsInfo(string searchText, bool isPageFilterStopWords);
        Task<List<MetaTagInfo>> GetAllMetaTagsInfo(string searchText, bool isPageFilterStopWords);
        Task<Dictionary<string, int>> GetAllExternalLinks(string searchText);
    }
}
