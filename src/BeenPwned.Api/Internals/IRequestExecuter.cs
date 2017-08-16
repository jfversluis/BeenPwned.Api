using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BeenPwned.Api.Internals
{
    internal interface IRequestExecuter : IDisposable
    {
        Task<IEnumerable<T>> GetCollectionAsync<T>(string endpoint) where T : class;
        Task<HttpResponseMessage> GetAsync(string endpointUrl);
        Task<HttpResponseMessage> PostAsync(string endpointUrl, FormUrlEncodedContent formUrlEncodedContent);
    }
}