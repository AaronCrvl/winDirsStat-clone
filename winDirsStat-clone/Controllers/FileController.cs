using System;
using System.IO;

namespace winDirsStat_clone.Controllers
{
    public class FileController
    {
        #region Methods
        public static long GetFileSizeInMB(string path)
        {
            try
            {
                if (File.Exists(path))
                    return
                        // MB -----------------------------------
                        // KB ---------------------------
                        // Bytes -----------------
                        File.OpenRead(path).Length / 1024 / 1000;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetFileExtension(string path)
        {
            try
            {
                if (File.Exists(path))
                    return new FileInfo(path).Extension;
                else
                    return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool FileExists(string path)
        {
            return new FileInfo(path).Exists;
        }
        #endregion        
    }
}
