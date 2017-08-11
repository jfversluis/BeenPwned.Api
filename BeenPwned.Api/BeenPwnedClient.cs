using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BeenPwned.Api
{
    public class BeenPwnedClient : IDisposable
    {
        private bool _isDisposing;
        private readonly HttpClient _httpClient;

        public BeenPwnedClient(string useragent, string baseApiUrl = "https://haveibeenpwned.com/api/v2")
        {
            if (string.IsNullOrWhiteSpace(useragent))
                throw new ArgumentException("For communication to the HIBP API a user-agent needs to be set.", nameof(useragent));

            if (!Uri.IsWellFormedUriString(baseApiUrl, UriKind.Absolute))
                throw new ArgumentException("The given HIBP base URL does not seem to be valid. Make sure you provide a full, valid URL.", nameof(baseApiUrl));

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseApiUrl)
            };

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(useragent));
        }

        public void Dispose()
        {
            // TODO this necessary?
            _isDisposing = true;

            if (!_isDisposing)
            {
                _httpClient?.Dispose();
            }
        }
    }
}
