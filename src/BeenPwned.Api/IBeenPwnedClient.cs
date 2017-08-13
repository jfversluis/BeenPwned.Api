using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BeenPwned.Api.Models;

namespace BeenPwned.Api
{
    public interface IBeenPwnedClient : IDisposable
    {
        Task<IEnumerable<Breach>> GetAllBreaches(bool truncateResponse = true, string domain = "", bool includeUnverified = false);
        Task<IEnumerable<string>> GetAllDataClasses();
        Task<IEnumerable<Breach>> GetBreachesForAccount(string account, bool truncateResponse = true, bool includeUnverified = false);
        Task<IEnumerable<Paste>> GetPastesForAccount(string account);
        Task<bool> GetPwnedPassword(string password, bool originalPasswordIsAHash = false, bool sendAsPostRequest = false);
    }
}