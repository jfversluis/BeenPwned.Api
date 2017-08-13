using Newtonsoft.Json;
using System;

namespace BeenPwned.Api.Models
{
    public class Breach
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Domain")]
        public string Domain { get; set; }

        [JsonProperty("BreachDate")]
        public DateTime BreachDate { get; set; }

        [JsonProperty("AddedDate")]
        public DateTime AddedDate { get; set; }

        [JsonProperty("ModifiedDate")]
        public DateTime ModifiedDate { get; set; }

        [JsonProperty("PwnCount")]
        public int NoOfAccounts { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("DataClasses")]
        public string[] DataClasses { get; set; }

        [JsonProperty("IsVerified")]
        public bool IsVerified { get; set; }

        [JsonProperty("IsSensitive")]
        public bool IsSensitive { get; set; }

        [JsonProperty("IsRetired")]
        public bool IsRetired { get; set; }

        [JsonProperty("IsSpamList")]
        public bool IsSpamList { get; set; }

        [JsonProperty("IsActive")]
        public bool IsActive { get; set; }

        [JsonProperty("IsFabricated")]
        public bool IsFabricated { get; set; }

        [JsonProperty("LogoType")]
        public string LogoType { get; set; }
    }
}