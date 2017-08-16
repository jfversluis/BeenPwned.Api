[![Build status](https://ci.appveyor.com/api/projects/status/8l9ae2irxxhvo0dq?svg=true)](https://ci.appveyor.com/project/jfversluis/beenpwned-api) [![NuGet](https://img.shields.io/nuget/v/BeenPwned.Api.svg)](https://www.nuget.org/packages/BeenPwned.Api/)

# BeenPwned.Api
.NET Wrapper library for the [haveibeenpwned.com API](https://haveibeenpwned.com/api/)

## Platform support

|Platform|Version|
| ------------------- | :----------:|
|.NET|4.5 & 4.6|
|.NET Standard|1.3|
|PCL|Profile 111|

## Documentation
    /// <summary>
    /// A client which contains all functionality to communicate with the public haveibeenpwned.com API.
    /// More information: https://haveibeenpwned.com/API/
    /// </summary>
    public interface IBeenPwnedClient : IDisposable
    {
        /// <summary>
        /// Retrieves all breaches from the API. Including breaches marked as sensitive and retired.
        /// Full details are returned.
        /// </summary>
        /// <param name="domain">Filter breaches by domain. For example: abobe.com</param>
        /// <returns>List of all breaches currently in the API</returns>
        Task<IEnumerable<Breach>> GetAllBreaches(string domain = "");

        /// <summary>
        /// Retrieves a list of all data class available in the API.
        /// From the API description:
        /// A "data class" is an attribute of a record compromised in a breach.
        /// For example, many breaches expose data classes such as "Email addresses" and "Passwords".
        /// The values returned by this service are ordered alphabetically in a string array
        /// and will expand over time as new breaches expose previously unseen classes of data.
        /// </summary>
        /// <returns>A list of strings representing data classes</returns>
        Task<IEnumerable<string>> GetAllDataClasses();

        /// <summary>
        /// Retrieves all breaches for a certain account.
        /// The public API will NOT return breaches marked as sensitive or retired.
        /// By default, unverified breaches aren't included, however these can be included with a switch.
        /// </summary>
        /// <param name="account">Username or emailaddress to retrieve the breaches for</param>
        /// <param name="domain">Filter breaches by domain. For example: abobe.com. A breach can be included more than once, if the have been comprimised on multiple occasions</param>
        /// <param name="truncateResponse">Setting this to true will return only the names of the breaches</param>
        /// <param name="includeUnverified">Includes unverified breaches in the results</param>
        /// <returns>A list of breaches relevant to the given parameters</returns>
        Task<IEnumerable<Breach>> GetBreachesForAccount(string account, string domain = "", bool truncateResponse = true, bool includeUnverified = false);

        /// <summary>
        /// Retrieves a list of "pastes" for the given account.
        /// A "paste" is information that has been "pasted" to a publicly facing website designed to share content such as Pastebin.
        /// These services are favoured by hackers due to the ease of anonymously sharing information and they're frequently the first place a breach appears.
        /// More information: https://haveibeenpwned.com/FAQs#Pastes
        /// </summary>
        /// <param name="account">Username to search for. Has to be a (valid) emailaddress.</param>
        /// <returns>A list of pastes for the given account</returns>
        Task<IEnumerable<Paste>> GetPastesForAccount(string account);

        /// <summary>
        /// Checks if a given password is in the list of known breached passwords.
        /// </summary>
        /// <param name="password">The password to be checked</param>
        /// <param name="originalPasswordIsAHash">Specifies if the value in the "password" parameter is a SHA1 hash</param>
        /// <param name="sendAsPostRequest">Optionally the request can be sent as a POST request to prevent possible URL logging</param>
        /// <returns></returns>
        Task<bool> GetPwnedPassword(string password, bool originalPasswordIsAHash = false, bool sendAsPostRequest = false);
    }

## License
The MIT License (MIT) see [License file](LICENSE)