using System;
using System.Text;
using Newtonsoft.Json;

namespace Services.WebClient
{
    public abstract class WebClientWrapperBase : IDisposable
    {
        private readonly string _baseUrl;
        private Lazy<System.Net.WebClient> _lazyClient;

        protected WebClientWrapperBase(string baseUrl)
        {
            _baseUrl = baseUrl.Trim('/');
            _lazyClient = new Lazy<System.Net.WebClient>(() => new System.Net.WebClient() { Encoding = Encoding.UTF8});
        }

        protected System.Net.WebClient Client()
        {
            if (_lazyClient == null)
            {
                throw new ObjectDisposedException("WebClient has been disposed");
            }

            return _lazyClient.Value;
        }

        protected T Execute<T>(string urlSegment)
        {
            return JsonConvert.DeserializeObject<T>(Client().DownloadString(_baseUrl + '/' + urlSegment.TrimStart('/')));
        }

        ~WebClientWrapperBase()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_lazyClient != null)
            {
                if (disposing)
                {
                    if (_lazyClient.IsValueCreated)
                    {
                        _lazyClient.Value.Dispose();
                        _lazyClient = null;
                    }
                }

                // There are no unmanaged resources to release, but
                // if we add them, they need to be released here.
            }
        }
    }
}
