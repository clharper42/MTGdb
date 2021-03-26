using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
namespace MTGdb
{
    class Cardface
    {
        //FOR DOUBLE FACED CARDS
        [Ignore]
        public string Name { get; set; }
        [Ignore]
        public string Mana_cost { get; set; }
        [Ignore]
        public string Type_line { get; set; }
        [Ignore]
        public string Oracle_text { get; set; }
        [Ignore]
        public string Artist { get; set; }
        [Ignore]
        public List<string> Colors { get; set; }
        [Ignore]
        public string Flavor_text { get; set; }
        [Ignore]
        public string Power { get; set; }
        [Ignore]
        public string Toughness { get; set; }
        [Ignore]
        public string Loyalty { get; set; }
        [Ignore]
        public Dictionary<string, string> Image_uris { get; set; }
    }
}