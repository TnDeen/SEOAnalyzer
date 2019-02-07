using SEOAnalyzerWeb.Constants;
using SEOAnalyzerWeb.Models;
using SEOAnalyzerWeb.Service.Interface;
using SEOAnalyzerWeb.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SEOAnalyzerWeb.Service
{
    public class AnalysisTextService : IAnalysisService
    {
        public AnalysisTextService()
        {
        }

        public async Task<Dictionary<string, int>> GetAllWordsInfo(string searchText, bool isPageFilterStopWords)
        {
            var listOfWords = new List<string>();

            listOfWords = await Util.GetAllWords(searchText);

            if (isPageFilterStopWords)
            {
                listOfWords = await Util.FilterStopWords(listOfWords, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constant.StopWordsPath));
            }
            return await Util.GroupListOfString(listOfWords);
        }

        public async Task<List<MetaTagInfo>> GetAllMetaTagsInfo(string searchText, bool isPageFilterStopWords)
        {
            return new List<MetaTagInfo>();
        }

        public async Task<Dictionary<string, int>> GetAllExternalLinks(string searchText)
        {
            var listofURL = new List<string>();
            listofURL = await Util.GetAllExternalLinksFromText(searchText);
            return await Util.GroupListOfString(listofURL);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}