using System.Collections.Generic;
using CsvHelper.Configuration.Attributes;
namespace MTGdb
{
    class Card
    {
        [Ignore]
        public string Object { get; set; }
        [Ignore]
        public string Id { get; set; }
        [Ignore]
        public string Oracle_id { get; set; }
        [Ignore]
        public List<int> Multiverse_ids { get; set; }
        [Ignore]
        public int Mtgo_id { get; set; }
        [Ignore]
        public int Tcgplayer_id { get; set; }
        [Ignore]
        public int Cardmarket_id { get; set; }
        [Ignore]
        public string Lang { get; set; }
        [Ignore]
        public string Released_at { get; set; }
        [Ignore]
        public string Uri { get; set; }
        [Ignore]
        public string Scryfall_uri { get; set; }
        [Ignore]
        public string Layout { get; set; }
        [Ignore]
        public bool Highres_image { get; set; }
        [Ignore]
        public Dictionary<string, string> Image_uris { get; set; }
        [Ignore]
        public string Mana_cost { get; set; }
        [Ignore]
        public string Cmc { get; set; }
        [Ignore]
        public string Type_line { get; set; }
        [Ignore]
        public string Oracle_text { get; set; }
        [Ignore]
        public List<string> Colors { get; set; }
        [Ignore]
        public List<char> Color_identity { get; set; }
        [Ignore]
        public List<string> Keywords { get; set; }
        [Ignore]
        public Dictionary<string, string> Legalities { get; set; }
        [Ignore]
        public List<string> Games { get; set; }
        [Ignore]
        public bool Reserved { get; set; }
        [Ignore]
        public bool Foil { get; set; }
        [Ignore]
        public bool Nonfoil { get; set; }
        [Ignore]
        public bool Oversized { get; set; }
        [Ignore]
        public bool Promo { get; set; }
        [Ignore]
        public bool Reprint { get; set; }
        [Ignore]
        public bool Variation { get; set; }
        [Ignore]
        public string Set_name { get; set; }
        [Ignore]
        public string Set_type { get; set; }
        [Ignore]
        public string Set_uri { get; set; }
        [Ignore]
        public string Set_search_uri { get; set; }
        [Ignore]
        public string Scryfall_set_uri { get; set; }
        [Ignore]
        public string Rulings_uri { get; set; }
        [Ignore]
        public string Prints_search_uri { get; set; }
        [Ignore]
        public bool Digital { get; set; }
        [Ignore]
        public string Rarity { get; set; }
        [Ignore]
        public string Card_back_id { get; set; }
        [Ignore]
        public string Artist { get; set; }
        [Ignore]
        public List<string> Artist_ids { get; set; }
        [Ignore]
        public string Illustration_id { get; set; }
        [Ignore]
        public string Border_color { get; set; }
        [Ignore]
        public string Frame { get; set; }
        [Ignore]
        public bool Full_art { get; set; }
        [Ignore]
        public bool Textless { get; set; }
        [Ignore]
        public bool Booster { get; set; }
        [Ignore]
        public bool Story_spotlight { get; set; }
        [Ignore]
        public int Edhrec_rank { get; set; }
        [Ignore]
        public Dictionary<string, string> Preview { get; set; }
        [Ignore]
        public Dictionary<string, string> Prices { get; set; }
        [Ignore]
        public Dictionary<string, string> Related_uris { get; set; }
        [Ignore]
        public Dictionary<string, string> Purchase_uris { get; set; }
        [Ignore]
        //FOR DOUBLE FACED CARDS
        public List<Cardface> Card_faces { get; set; }
        [Ignore]
        public string Loyalty { get; set; }
        [Ignore]
        public string Power { get; set; }
        [Ignore]
        public string Toughness { get; set; }

        public string Name { get; set; }
        //add after api call
        public string Special_name { get; set; }
        public string Set { get; set; }
        //add after api call
        public int Amount { get; set; }
        //add after api call
        public string Printing { get; set; }
        //got from api
        public string Collector_number { get; set; }
        [Ignore]
        public int Database_id { get; set; }
        [Ignore]
        public ColorIdCmc Coloridcmc { get; set; }
        [Ignore]
        public string Colorstring { get; set; }
        [Ignore]
        public double Pricetotal { get; set; }
        [Ignore]
        private static int dbid = 0;
        public Card()
        {
            Database_id = dbid;
            dbid++;
        }

        public override string ToString()
        {
            return Special_name + " " + Set + " " + Printing + " " + Amount + " " + Database_id;
        }
    }
}
