using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace winDirsStat_clone.Controllers
{
    public class DirectoryController
    {
        #region Methods
        public static winDirsStat_clone.Data.Directory ReadDirectory(string path)
        {
            try
            {                
                var directory = new winDirsStat_clone.Data.Directory(path);
                if (new DirectoryInfo(path).Exists)
                {
                    var ext = new List<string>();
                    directory.files = Task.Factory.StartNew(() => Directory.GetFiles(path)).Result;
                    directory.subDirectories = Task.Factory.StartNew(() => Directory.GetDirectories(path)).Result;

                    if (directory.files.Length > 0)
                        directory.files.ToList().ForEach((fl) => 
                        {                        
                            string extension = new FileInfo(fl).Extension.ToString();
                            if (!ext.Contains(extension))
                                ext.Add(extension);
                        });
                    directory.fileExtensions = ext;
                }

                return directory;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static long GetDirectoryFileCount(string path)
        {
            try
            {
                if (DirectoryExists(path))
                    return new DirectoryInfo(path).GetFiles().Count();
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool DirectoryExists(string path)
        {
            return new DirectoryInfo(path).Exists;
        }
        #endregion        
    }
}
