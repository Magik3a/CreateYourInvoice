using System.ComponentModel;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace Tools.Attributes
{
    public class MultiLanguageDisplayNameAttribute : DisplayNameAttribute
    {
        private static HttpClient client = new HttpClient();

        public MultiLanguageDisplayNameAttribute(string resourceId)
        : base(GetMessageFromResource(resourceId))
    { }

        private static string GetMessageFromResource(string resourceId)
        {
            var language = ConfigurationManager.AppSettings["AppLanguage"];
            var url = ConfigurationManager.AppSettings["MultiLanguageApiUrl"];
            HttpResponseMessage response = Task.Run(() => client.GetAsync($"{url}/Initials/{language}/Phrase/{resourceId}")).Result;

            if (response.IsSuccessStatusCode)
            {
                var task = Json.Decode(Task.Run(() => response.Content.ReadAsStringAsync()).Result);
                return task;
            }
            else
            {
                return response.StatusCode.ToString();
            }
        }
    }
}