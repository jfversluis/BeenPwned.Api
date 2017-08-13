using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace BeenPwned.Api
{
    internal static class Utilities
    {
        internal static string BuildQueryString(string url, Dictionary<string, string> keyValueDictionary)
        {
            var array = keyValueDictionary.Select(x => x.Key + "=" + WebUtility.UrlEncode(x.Value.ToString()))
                .ToArray();

            return url + "?" + string.Join("&", array);
        }

        internal static bool IsValidEmailaddress(string emailaddress)
        {
            var regex = "^(([^<>()[\\]\\\\.,;:\\s@\\\"\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"\"]+)*)|(\\\"\".+\\\"\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";

            return new Regex(regex).IsMatch(emailaddress);
        }
    }
}