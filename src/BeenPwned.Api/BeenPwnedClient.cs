using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using BeenPwned.Api.Models;
using Newtonsoft.Json;

namespace BeenPwned.Api
{
    // TODO add results based on HTTP status codes
    public class BeenPwnedClient : IDisposable
    {
        private bool _isDisposing;
        private readonly HttpClient _httpClient;

        public BeenPwnedClient(string useragent, string baseApiUrl = "https://haveibeenpwned.com/api/v2/")
        {
            if (string.IsNullOrWhiteSpace(useragent))
                throw new ArgumentException("For communication to the HIBP API a user-agent needs to be set.", nameof(useragent));

            if (!Uri.IsWellFormedUriString(baseApiUrl, UriKind.Absolute))
                throw new ArgumentException("The given HIBP base URL does not seem to be valid. Make sure you provide a full, valid URL.", nameof(baseApiUrl));

            var handler = new HttpClientHandler();
			if (handler.SupportsAutomaticDecompression)
			{
				handler.AutomaticDecompression = DecompressionMethods.GZip |
												 DecompressionMethods.Deflate;
			}

            _httpClient = new HttpClient(handler)
            {
                BaseAddress = new Uri(baseApiUrl)
            };

            _httpClient.DefaultRequestHeaders.UserAgent.Clear();
            _httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue(new ProductHeaderValue(useragent)));
        }

        // TODO add error handling
        // TODO add truncate switch
        public async Task<IEnumerable<Breach>> GetBreaches(string domain = "")
        {
            var endpointUrl = "breaches";

            if (!string.IsNullOrWhiteSpace(domain))
                endpointUrl += $"?domain={domain}";

            return await GetResult<IEnumerable<Breach>>(endpointUrl);
        }

        // TODO add error handling
        // TODO add domain filter
        // TODO add unverified switch
        public async Task<IEnumerable<Breach>> GetBreachesForAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            return await GetResult<IEnumerable<Breach>>($"breachesbreachedaccount/{account}");
        }

        // TODO add error handling
        // TODO account needs to be an email
        public async Task<IEnumerable<Paste>> GetPastes(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            return await GetResult<IEnumerable<Paste>>($"pasteaccount/{account}");
        }

        public async Task<IEnumerable<string>> GetDataClasses()
        {
            return await GetResult<IEnumerable<string>>("dataclasses");
        }

        // TODO implement password hash switch
        public async Task<IEnumerable<string>> GetPwnedPassword(string password)
        {
            return await GetResult<IEnumerable<string>>($"pwnedpassword/{password}");
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

        private async Task<T> GetResult<T>(string endpoint) where T : class
        {
            var stringResult = await _httpClient.GetStringAsync(endpoint);

            return JsonConvert.DeserializeObject<T>(stringResult);
        }
    }
}