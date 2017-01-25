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
using Services.WebClient.MultiLanguage.Contracts;
using Tools;

namespace akcet_fakturi.Helpers
{
    public static class HtmlHelperExtensions
    {
        //private static HttpClient client = new HttpClient();

        //private static MultiLanguageClient multiLanguageClient;

        public static string MultiLanguageString(this HtmlHelper htmlHelper, int phrase)
        {
            var multiLanguageClient = DependencyResolver.Current.GetService<IMultiLanguageClient>();
            return multiLanguageClient.GetTranslation(phrase.ToString());
        }

        public static IHtmlString MultiLanguage(this HtmlHelper htmlHelper, int phrase)
        {
            //var language = ConfigurationManager.AppSettings["AppLanguage"];
            //var url = ConfigurationManager.AppSettings["MultiLanguageApiUrl"];
            //HttpResponseMessage response = Task.Run(() => client.GetAsync($"{url}/Initials/{language}/Phrase/{phrase}")).Result;

            //if (response.IsSuccessStatusCode)
            //{
            //    var task = Json.Decode(Task.Run(() => response.Content.ReadAsStringAsync()).Result);
            //    if (string.IsNullOrWhiteSpace(task))
            //        return "Not translated: id=" + phrase;
            //    return task;
            //}
            //else
            //{
            //    return response.StatusCode.ToString();
            //}


           // var multiLanguageClient = DependencyResolver.Current.GetService<IMultiLanguageClient>();
           // return multiLanguageClient.GetTranslation(phrase.ToString());

            var span = htmlHelper.Raw($"<span class='multilanguage-phrase' data-multilanguage='{phrase}'>loading...</span>");
            return span;
        }

    }

}