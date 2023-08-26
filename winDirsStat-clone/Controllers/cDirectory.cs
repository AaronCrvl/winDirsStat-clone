using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace winDirsStat_clone.Controllers
{
    public class cDirectory
    {
        #region Methods
        public static winDirsStat_clone.Data.Directory ReadDirectory(string path)
        {
            var res = new winDirsStat_clone.Data.Directory();

            try
            {
                res =
                    Task.Factory.StartNew(() =>
                    {
                        var directory = new winDirsStat_clone.Data.Directory(path);
                        if (new DirectoryInfo(path).Exists)
                        {
                            var ext = new List<string>();
                            directory.files = Directory.GetFiles(path);
                            directory.subDirectories = Directory.GetDirectories(path);

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
                    }).Result;

                return res;
            }
            catch (Exception ex)
            {
                if (ex.GetType() != new System.IO.IOException().GetType())
                    throw ex;
                else
                {
                    //LogController.WriteLog("DirectoryController.ReadSubdirectories", ex);
                    return res;
                }
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

        public static bool DirectoryExists(string path) => new DirectoryInfo(path).Exists;
        #endregion        
    }
}
