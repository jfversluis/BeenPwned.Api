using System;
using Newtonsoft.Json;

namespace BeenPwned.Api.Models
{
    public class Paste
    {
        [JsonProperty("Source")]
        public string Source { get; set; }

        [JsonProperty("Id")]
        public string Id { get; set; }
        
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Date")]
        public DateTime? Date { get; set; }

        [JsonProperty("EmailCount")]
        public int NoOfEmails { get; set; }
    }
}