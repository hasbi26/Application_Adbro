using Newtonsoft.Json;

namespace UserInterestsAPIService.Models;
    public class Intrsts
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("interests")]
        public string interests { get; set; }
    }

