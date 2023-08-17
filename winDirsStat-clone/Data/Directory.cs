using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winDirsStat_clone.Data
{
    public class Directory
    {
        public Directory()
        {
            this.path = "";            
        }
        public Directory(string _path)
        {
            this.path = _path;
        }

        public string path;
        public long mbSize;
        public string[] files;
        public string[] subDirectories;
        public List<string> fileExtensions;
    }
}
