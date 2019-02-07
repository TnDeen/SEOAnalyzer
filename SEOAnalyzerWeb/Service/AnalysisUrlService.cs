using HtmlAgilityPack;
using NUglify;
using SEOAnalyzerWeb.Constants;
using SEOAnalyzerWeb.Models;
using SEOAnalyzerWeb.Service.Interface;
using SEOAnalyzerWeb.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SEOAnalyzerWeb.Service
{
    public class AnalysisUrlService : IAnalysisService
    {
        public AnalysisUrlService()
        {
        }

        public async Task<Dictionary<string, int>> GetAllWordsInfo(string searchText, bool isPageFilterStopWords)
        {
            var listOfWords = new List<string>();

            var web = new HtmlWeb();
            var doc = web.Load(searchText);
            var root = doc.DocumentNode.SelectSingleNode("//body");
            var allText = Uglify.HtmlToText(root.OuterHtml);
            listOfWords = await Util.GetAllWords(allText.Code);

            if (isPageFilterStopWords)
            {
                listOfWords = await Util.FilterStopWords(listOfWords, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constant.StopWordsPath));
            }

            return await Util.GroupListOfString(listOfWords);
        }

        public async Task<List<MetaTagInfo>> GetAllMetaTagsInfo(string searchText, bool isPageFilterStopWords)
        {

            var listOfWords = new List<string>();

            var webGet = new HtmlWeb();
            var document = webGet.Load(searchText);
            var metaTags = document.DocumentNode.SelectNodes("//meta");
            var listofMetaTagInfo = new List<MetaTagInfo>();

            foreach (var tag in metaTags.ToList())
            {
                var metaTagInfo = new MetaTagInfo();

                List<string> listofURL = new List<string>();
                List<string> listofWords = new List<string>();

                string content = tag.Attributes["content"] != null ? tag.Attributes["content"].Value : "";
                string property = tag.Attributes["property"] != null ? tag.Attributes["property"].Value : "";
                string name = tag.Attributes["name"] != null ? tag.Attributes["name"].Value : "";
                string itemProp = tag.Attributes["itemprop"] != null ? tag.Attributes["itemprop"].Value : "";
                string httpEquiv = tag.Attributes["http-equiv"] != null ? tag.Attributes["http-equiv"].Value : "";

                metaTagInfo.Content = content;
                metaTagInfo.Property = property;
                metaTagInfo.Name = name;
                metaTagInfo.ItemProp = itemProp;
                metaTagInfo.HttpEquiv = httpEquiv;

                var hrefList = Regex.Replace(metaTagInfo.Content, FilterFormat.GetAllLinks, "$1");

                if (hrefList.ToString().ToUpper().Contains("HTTP") || hrefList.ToString().ToUpper().Contains("://"))
                {
                    //isURL
                    listofURL.Add(hrefList);
                }
                else
                {
                    //isWords
                    var words = await Task.Run(() => { return Util.SplitSentenceIntoWords(hrefList.ToLower(), 1); });
                    listofWords.AddRange(words);
                }

                if (isPageFilterStopWords)
                {
                    listOfWords = await Util.FilterStopWords(listOfWords, Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Constant.StopWordsPath));
                }

                metaTagInfo.TotalWordCount = listofWords.Count();
                metaTagInfo.URLInfoList = await Util.GroupListOfString(listofURL);
                metaTagInfo.WordsInfoList = await Util.GroupListOfString(listofWords);

                if (!string.IsNullOrWhiteSpace(metaTagInfo.Content))
                {
                    listofMetaTagInfo.Add(metaTagInfo);
                }
            }

            return listofMetaTagInfo;
        }
        public async Task<Dictionary<string, int>> GetAllExternalLinks(string searchText)
        {
            var web = new HtmlWeb();
            var doc = web.Load(searchText);

            var listofURL = new List<String>();
            var nodeSingle = doc.DocumentNode.SelectSingleNode("//html");
            listofURL = await Util.GetAllExternalLinksFromText(nodeSingle.OuterHtml);

            return await Util.GroupListOfString(listofURL);
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}