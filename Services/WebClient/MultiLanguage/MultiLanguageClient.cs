using System;
using System.Configuration;
using Services.WebClient.MultiLanguage.Contracts;

namespace Services.WebClient.MultiLanguage
{
    public class MultiLanguageClient : WebClientWrapperBase, IMultiLanguageClient
    {
        public MultiLanguageClient() : base(ConfigurationManager.AppSettings["MultiLanguageApiUrl"])
        {
        }
        public MultiLanguageClient(string baseUrl)
        : base(baseUrl)
        {
        }
        public string GetTranslation(string resourceId)
        {
            var language = ConfigurationManager.AppSettings["AppLanguage"];
            try
            {
                var response = Execute<object>($"Initials/{language}/Phrase/{resourceId}");
                if (response is string)
                {
                    return response.ToString();
                }
                else
                {
                    return Execute<string>($"Context/{resourceId}");
                }
            }
            catch (Exception e)
            {
                //TODO Write this thing if server return 403 or some other error
                return "NOT FOUND";
            }
        }
    }
}
