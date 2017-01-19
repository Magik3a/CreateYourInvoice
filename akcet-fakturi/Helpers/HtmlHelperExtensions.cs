using System;
using System.Configuration;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;

namespace akcet_fakturi.Helpers
{
    public static class HtmlHelperExtensions
    {
        private static HttpClient client = new HttpClient();

        public static string MultiLanguage(this HtmlHelper htmlHelper, int phrase)
        {
            var language = ConfigurationManager.AppSettings["AppLanguage"];
            var url = ConfigurationManager.AppSettings["MultiLanguageApiUrl"];
            HttpResponseMessage response = Task.Run(() => client.GetAsync($"{url}/Initials/{language}/Phrase/{phrase}")).Result;

            if (response.IsSuccessStatusCode)
            {
                var task = Json.Decode(Task.Run(() => response.Content.ReadAsStringAsync()).Result);
                if (string.IsNullOrWhiteSpace(task))
                    return "Not translated: id=" + phrase;
                return task;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }

    }

}