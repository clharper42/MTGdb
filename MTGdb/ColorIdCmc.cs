using System;
using System.Collections.Generic;
using System.Text;

namespace MTGdb
{
    class ColorIdCmc
    {
        public string Colors { get; set; }
        public string Cmc { get; set; }

        public string Display { get; set; }

        public ColorIdCmc(string cmc,string colors)
        {
            Colors = colors;
            Cmc = cmc;
            Display = Colors + " - " + Cmc;
        }

        public override bool Equals(object obj)
        {
            var groupdata = obj as ColorIdCmc;
            return Colors.Equals(groupdata.Colors) && Cmc.Equals(groupdata.Cmc);
        }
    }
}
