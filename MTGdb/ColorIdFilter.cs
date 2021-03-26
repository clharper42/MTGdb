using System;
using System.Collections.Generic;
using System.Text;

namespace MTGdb
{
    class ColorIdFilter
    {
        public string Colors { get; set; }
        public string Filter { get; set; }
        public string Display { get; set; }
        public ColorIdFilter(string filter, string colors)
        {
            Colors = colors;
            Filter = filter;
            Display = Colors + " - " + Filter;
        }
        public override bool Equals(object obj)
        {
            var groupdata = obj as ColorIdFilter;
            return Colors.Equals(groupdata.Colors) && Filter.Equals(groupdata.Filter);
        }
    }
}
