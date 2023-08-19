using System.Collections.Generic;

namespace winDirsStat_clone.Data
{
    public class Directory
    {
        #region Constructors
        public Directory()
        {
            this.path = "";
        }
        public Directory(string _path)
        {
            this.path = _path;
        }
        #endregion

        #region Properties
        public string path;
        public string[] files;
        public string[] subDirectories;
        public List<string> fileExtensions;
        #endregion       
    }
}
