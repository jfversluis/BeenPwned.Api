using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BeenPwned.Api.Internals;
using BeenPwned.Api.Models;

namespace BeenPwned.Api
{
    public class BeenPwnedClient : IBeenPwnedClient
    {
        private readonly IRequestExecuter _requestExecuter;

        public BeenPwnedClient(string useragent, string baseApiUrl = "https://haveibeenpwned.com/api/v2/")
        {
            if (string.IsNullOrWhiteSpace(useragent))
                throw new ArgumentException("For communication to the HIBP API a user-agent needs to be set.", nameof(useragent));

            if (!Uri.IsWellFormedUriString(baseApiUrl, UriKind.Absolute))
                throw new ArgumentException("The given HIBP base URL does not seem to be valid. Make sure you provide a full, valid URL.", nameof(baseApiUrl));

            _requestExecuter = new RequestExecuter(useragent, baseApiUrl);
        }

        public Task<IEnumerable<Breach>> GetAllBreaches(string domain = "")
        {
            var queryValues = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(domain))
                queryValues.Add("domain", domain);

            var endpointUrl = Utilities.BuildQueryString("breaches", queryValues);

            return _requestExecuter.GetCollectionAsync<Breach>(endpointUrl);
        }

        public Task<IEnumerable<Breach>> GetBreachesForAccount(string account, string domain = "", bool truncateResponse = true, bool includeUnverified = false)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            var queryValues = new Dictionary<string, string>
            {
                { "truncateResponse", truncateResponse.ToString() },
                { "includeUnverified", includeUnverified.ToString() }
            };

            if (!string.IsNullOrWhiteSpace(domain))
                queryValues.Add("domain", domain);

            var endpointUrl = Utilities.BuildQueryString($"breachedaccount/{account}", queryValues);

            return _requestExecuter.GetCollectionAsync<Breach>(endpointUrl);
        }

        public Task<IEnumerable<Paste>> GetPastesForAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentException("An account name needs to be specified", nameof(account));

            if (!Utilities.IsValidEmailaddress(account))
                throw new ArgumentException("Account it not a (valid) emailaddress", nameof(account));

            return _requestExecuter.GetCollectionAsync<Paste>($"pasteaccount/{account}");
        }

        public Task<IEnumerable<string>> GetAllDataClasses()
        {
            return _requestExecuter.GetCollectionAsync<string>("dataclasses");
        }

        public async Task<bool> GetPwnedPassword(string password, bool originalPasswordIsAHash = false,
            bool sendAsPostRequest = false)
        {
            var queryValues = new Dictionary<string, string>
            {
                { "originalPasswordIsAHash", originalPasswordIsAHash.ToString() }
            };

            HttpResponseMessage result;

            if (sendAsPostRequest)
            {
                var formValues = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("Password", password)
                };

                var endpointUrl = Utilities.BuildQueryString("pwnedpassword", queryValues);

                result = await _requestExecuter.PostAsync(endpointUrl, new FormUrlEncodedContent(formValues));
            }
            else
            {
                var endpointUrl = Utilities.BuildQueryString($"pwnedpassword/{password}", queryValues);

                result = await _requestExecuter.GetAsync(endpointUrl);
            }

            switch ((int)result.StatusCode)
            {
                case 200:
                    return true;
                case 403:
                    throw new BeenPwnedUnavailableException("Access denied: probably no user-agent is specified for the request.");
                case 404:
                    return false;
                case 429:
                    throw new BeenPwnedUnavailableException("Too many requests: rate-limit exceeded. Please try again in a second.");
                default:
                    throw new BeenPwnedUnavailableException($"Unexpected result from API. Statuscode {result.StatusCode}, message: {result.ReasonPhrase}");
            }
        }

        public void Dispose()
        {
            _requestExecuter.Dispose();
        }
    }
}