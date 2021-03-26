using System.Collections.Generic;
using Newtonsoft.Json;
namespace MTGdb
{
    class Identifier
    {
        [JsonProperty(PropertyName = "identifiers")]
        public ICollection<CardSearch> IDs { get; set; }
    }
}
