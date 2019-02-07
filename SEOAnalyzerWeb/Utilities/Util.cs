using Newtonsoft.Json;
using SEOAnalyzerWeb.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace SEOAnalyzerWeb.Utilities
{
    public class Util
    {
        public static async Task<List<string>> GetAllWords(string text)
        {
            return await Task.Run(() =>
            {
                var words = SplitSentenceIntoWords(text.ToLower(), 1);

                List<string> modifiedWords = new List<string>();

                foreach (var word in words)
                {
                    var stripedWords = word;

                    if (!string.IsNullOrWhiteSpace(stripedWords) &&
                        Regex.IsMatch(stripedWords, "^[a-z\u00c0-\u00f6]+$", RegexOptions.IgnoreCase) &&
                        stripedWords.Length > 1)
                    {
                        modifiedWords.Add(stripedWords);
                    }

                }

                return modifiedWords.ToList<string>();
            });

        }
        public async static Task<List<string>> FilterStopWords(List<string> Words, string stopWordsPath)
        {
            var jsonText = File.ReadAllText(stopWordsPath);
            var stopWords = JsonConvert.DeserializeObject<IList<string>>(jsonText);

            var matches = Words.Where(word => !stopWords.Contains(word));

            return matches.ToList<string>();
        }
        public static async Task<List<string>> GetAllExternalLinksFromText(string text)
        {
            return await Task.Run(() =>
            {
                List<string> listofURL = new List<string>();
                MatchCollection mc = Regex.Matches(text, FilterFormat.GetAllLinks);
                foreach (Match match in mc)
                {
                    listofURL.Add(match.Value);
                }

                return listofURL;
            });
        }
        public static IEnumerable<string> SplitSentenceIntoWords(string sentence, int wordMinLength)
        {
            var word = new StringBuilder();
            foreach (var chr in sentence)
            {
                if (Char.IsPunctuation(chr) || Char.IsSeparator(chr) || Char.IsWhiteSpace(chr))
                {
                    if (word.Length > wordMinLength)
                    {
                        yield return word.ToString();
                        word.Clear();
                    }
                }
                else
                {
                    word.Append(chr);
                }
            }

            if (word.Length > wordMinLength)
            {
                yield return word.ToString();
            }
        }
        public async static Task<Dictionary<string, int>> GroupListOfString(List<string> listofString)
        {
            return await Task.Run(() =>
            {
                return listofString.GroupBy(word => word)
                   .ToDictionary(group => group.Key, group => group.Count());
            });

        }
    }
}