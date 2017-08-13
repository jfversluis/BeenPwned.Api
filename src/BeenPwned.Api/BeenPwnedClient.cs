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
    // TODO Find a way to handle the response codes (https://haveibeenpwned.com/API/v2/#ResponseCodes) globally
    public class BeenPwnedClient : IBeenPwnedClient
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
        public async Task<IEnumerable<Breach>> GetAllBreaches(bool truncateResponse = true, string domain = "", bool includeUnverified = false)
        {
            var endpointUrl = "breaches";

            var queryValues = new Dictionary<string, string>
            {
                { "truncateResponse", truncateResponse.ToString() },
                { "includeUnverified", includeUnverified.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(domain))
                queryValues.Add("domain", domain);

            endpointUrl += Utilities.BuildQueryString(queryValues);

            return await GetResult<IEnumerable<Breach>>(endpointUrl);
        }

        // TODO add error handling
        public async Task<IEnumerable<Breach>> GetBreachesForAccount(string account, bool truncateResponse = true, bool includeUnverified = false)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            var endpointUrl = $"breachesbreachedaccount/{account}";

            var queryValues = new Dictionary<string, string>
            {
                { "truncateResponse", truncateResponse.ToString() },
                { "includeUnverified", includeUnverified.ToString() }
            };

            endpointUrl += Utilities.BuildQueryString(queryValues);

            return await GetResult<IEnumerable<Breach>>(endpointUrl);
        }

        // TODO add error handling
        public async Task<IEnumerable<Paste>> GetPastesForAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            if (!Utilities.IsValidEmailaddress(account))
                throw new ArgumentException("Account it not a (valid) emailaddress", nameof(account));

            return await GetResult<IEnumerable<Paste>>($"pasteaccount/{account}");
        }

        public async Task<IEnumerable<string>> GetAllDataClasses()
        {
            return await GetResult<IEnumerable<string>>("dataclasses");
        }
        
        public async Task<bool> GetPwnedPassword(string password, bool originalPasswordIsAHash = false,
            bool sendAsPostRequest = false)
        {
            HttpResponseMessage result;

            if (sendAsPostRequest)
            {
                var formValues =
                    new List<KeyValuePair<string, string>> {new KeyValuePair<string, string>("Password", password)};

                result =
                    await _httpClient.PostAsync($"pwnedpassword?originalPasswordIsAHash={originalPasswordIsAHash}",
                    new FormUrlEncodedContent(formValues));
            }
            else
            {
                result = await _httpClient.GetAsync($"pwnedpassword/{password}?originalPasswordIsAHash={originalPasswordIsAHash}");
            }

            switch ((int) result.StatusCode)
            {
                case 200:
                    return true;
                case 404:
                    return false;
                default:
                    throw new Exception($"Unexpected result from API. Statuscode {result.StatusCode}, message: {result.ReasonPhrase}");
            }
        }

        public void Dispose()
        {
            // TODO this necessary?
            if (!_isDisposing)
            {
                _isDisposing = true;
                _httpClient.Dispose();
            }
        }

        private async Task<T> GetResult<T>(string endpoint) where T : class
        {
            var stringResult = await _httpClient.GetStringAsync(endpoint);

            return JsonConvert.DeserializeObject<T>(stringResult);
        }
    }
}