using Newtonsoft.Json;
namespace MTGdb
{
    class CardSearch
    {
        [JsonProperty(PropertyName = "set")]
        public string Set { get; set; }

        [JsonProperty(PropertyName = "collector_number")]
        public string Collector_number { get; set; }

    }
}