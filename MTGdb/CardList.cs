using System;
using System.Collections.Generic;
using System.Text;

namespace MTGdb
{
    class CardList
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<CardInList> TheList { get; private set; }
        public CardList(string name, string description)
        {
            Name = name;
            Description = description;
            TheList = new List<CardInList>();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
