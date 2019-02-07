using HtmlAgilityPack;
using SEOAnalyzerWeb.Models;
using SEOAnalyzerWeb.Models.ViewModel;
using SEOAnalyzerWeb.Service;
using SEOAnalyzerWeb.Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SEOAnalyzerWeb.Controllers
{
    public class AnalyzerController : Controller
    {
        // GET: Analyzer
        public async Task<ActionResult> Index(Analyzer analyzer)
        {
            var model = await Analyze(analyzer);
            return View(model);
        }

        // GET: Analyzer/Create
        public ActionResult Create()
        {
            return View(new Analyzer());
        }

        // POST: Analyzer/Create
        [HttpPost]
        public ActionResult Create(Analyzer analyzer)
        {
            if (string.IsNullOrEmpty(analyzer.SearchText))
            {
                ModelState.AddModelError("", "Empty Search Text!");
                return View(analyzer);
            }

            if (analyzer.IsURL)
            {
                var valid = ValidateURL(analyzer.SearchText.Trim());
                if (!valid)
                {
                    ModelState.AddModelError("", "Invalid Url!");
                    return View(analyzer);
                }

            }

            return RedirectToAction("Index", analyzer);


        }

        private async Task<AnalyzerViewModel> Analyze(Analyzer analyzer)
        {
            var viewModel = new AnalyzerViewModel
            {
                Analyzer = analyzer
            };

            if (analyzer == null || string.IsNullOrEmpty(analyzer.SearchText))
                return viewModel;


            IAnalysisService service;
            if (analyzer.IsURL)
            {
                service = new AnalysisUrlService();
            }
            else
            {
                service = new AnalysisTextService();
            }

            if(analyzer.IsCountNumberofWords)
            {
                viewModel.AllWordsInfo = await service.GetAllWordsInfo(analyzer.SearchText, analyzer.IsPageFilterStopWords);
                
                ViewBag.WordSortParm = string.IsNullOrEmpty(analyzer.WordSortBy) ? "word_desc" : "word";
                ViewBag.CountSortParm = analyzer.WordSortBy == "count" ? "count_desc" : "count";

                if (!string.IsNullOrEmpty(analyzer.WordSortBy))
                {
                    switch(analyzer.WordSortBy)
                    {
                        case "word_desc":
                            viewModel.AllWordsInfo = viewModel.AllWordsInfo.OrderByDescending(a => a.Key).ToDictionary(a=> a.Key, a => a.Value);
                            break;
                        case "count":
                            viewModel.AllWordsInfo = viewModel.AllWordsInfo.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
                            break;
                        case "count_desc":
                            viewModel.AllWordsInfo = viewModel.AllWordsInfo.OrderByDescending(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
                            break;
                        default:
                            viewModel.AllWordsInfo = viewModel.AllWordsInfo.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);
                            break;
                    }
                    
                }
            }

            if (analyzer.IsMetaTagsInfo)
            {
                viewModel.AllMetaTagsInfo = await service.GetAllMetaTagsInfo(analyzer.SearchText, analyzer.IsPageFilterStopWords);

                ViewBag.MetaWordSortParm = string.IsNullOrEmpty(analyzer.MetaSortBy) ? "word_desc" : "word";
                ViewBag.MetaCountSortParm = analyzer.MetaSortBy == "count" ? "count_desc" : "count";

                if (!string.IsNullOrEmpty(analyzer.MetaSortBy))
                {
                    switch (analyzer.MetaSortBy)
                    {
                        case "word_desc":
                            viewModel.AllMetaTagsInfo = viewModel.AllMetaTagsInfo.OrderByDescending(a => a.Name).ToList();
                            break;
                        case "count":
                            viewModel.AllMetaTagsInfo = viewModel.AllMetaTagsInfo.OrderBy(a => a.TotalWordCount).ToList();
                            break;
                        case "count_desc":
                            viewModel.AllMetaTagsInfo = viewModel.AllMetaTagsInfo.OrderByDescending(a => a.TotalWordCount).ToList();
                            break;
                        default:
                            viewModel.AllMetaTagsInfo = viewModel.AllMetaTagsInfo.OrderBy(a => a.Name).ToList();
                            break;
                    }

                }
            }

            if (analyzer.IsGetExternalLink)
            {
                viewModel.AllExternalLinks = await service.GetAllExternalLinks(analyzer.SearchText);

                ViewBag.LinkWordSortParm = string.IsNullOrEmpty(analyzer.LinkSortBy) ? "word_desc" : "word";
                ViewBag.LinkCountSortParm = analyzer.LinkSortBy == "count" ? "count_desc" : "count";

                if (!string.IsNullOrEmpty(analyzer.LinkSortBy))
                {
                    switch (analyzer.LinkSortBy)
                    {
                        case "word_desc":
                            viewModel.AllExternalLinks = viewModel.AllExternalLinks.OrderByDescending(a => a.Key).ToDictionary(a => a.Key, a => a.Value);
                            break;
                        case "count":
                            viewModel.AllExternalLinks = viewModel.AllExternalLinks.OrderBy(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
                            break;
                        case "count_desc":
                            viewModel.AllExternalLinks = viewModel.AllExternalLinks.OrderByDescending(a => a.Value).ToDictionary(a => a.Key, a => a.Value);
                            break;
                        default:
                            viewModel.AllExternalLinks = viewModel.AllExternalLinks.OrderBy(a => a.Key).ToDictionary(a => a.Key, a => a.Value);
                            break;
                    }

                }
            }
             
            
            
            service.Dispose();
            return viewModel;
        }

        private bool ValidateURL(string searchText)
        {
            var isValidURL = false;
            try
            {
                var web = new HtmlWeb();
                var docURL = web.Load(searchText.Trim());
                if (docURL != null)
                {
                    isValidURL = true;
                }
            }
            catch (Exception ex)
            {
                //log
            }

            return isValidURL;
        }
    }
}
