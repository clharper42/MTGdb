using System;
using System.Collections.Generic;
using System.Text;

namespace MTGdb
{
    class LoadableFile
    {
        public string Path { get;  private set; }

        public string Name { get;  private set; }


        public LoadableFile(string path)
        {
            Path = path;
            Name = path.Substring(path.LastIndexOf('\\') + 1).Substring(0, path.Substring(path.LastIndexOf('\\') + 1).Length - 4);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
