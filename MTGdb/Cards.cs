using System.Collections.Generic;
namespace MTGdb
{
    class Cards
    {
        public string Object { get; set; }
        public List<CardSearch> Not_found { get; set; }
        public List<Card> Data { get; set; }
    }
}