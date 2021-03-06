﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BeenPwned.Api.Internals
{
    internal class RequestExecuter : IRequestExecuter
    {
        private readonly HttpClient _httpClient;

        public RequestExecuter(string useragent, string baseApiUrl)
        {
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

        public void Dispose()
        {
            _httpClient.Dispose();
        }

        public async Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint) where T : class
        {
            var response = await _httpClient.GetAsync(endpoint);

            switch ((int)response.StatusCode)
            {
                case 200:
                    break;
                case 400:
                    throw new BeenPwnedUnavailableException("Invalid request");
                case 403:
                    throw new BeenPwnedUnavailableException("Access denied: probably no user-agent is specified for the request.");
                case 404:
                    return Enumerable.Empty<T>();
                case 429:
                    throw new BeenPwnedUnavailableException("Too many requests: rate-limit exceeded. Please try again in a second.");
                default:
                    throw new BeenPwnedUnavailableException($"Unknown error. Statuscode {response.StatusCode}, message: {response.ReasonPhrase}");
            }

            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<IEnumerable<T>>(stringResult);
        }

        public Task<HttpResponseMessage> GetAsync(string endpointUrl)
        {
            return _httpClient.GetAsync(endpointUrl);
        }

        public Task<HttpResponseMessage> PostAsync(string endpointUrl, FormUrlEncodedContent formUrlEncodedContent)
        {
            return _httpClient.PostAsync(endpointUrl, formUrlEncodedContent);
        }
    }
}