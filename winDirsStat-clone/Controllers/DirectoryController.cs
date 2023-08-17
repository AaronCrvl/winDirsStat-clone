using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winDirsStat_clone.Controllers
{
    public class DirectoryController
    {
        // List of and object that carries the file path as name
        // and a list with the file names.        
        public List<List<string>> names;

        // This should be a two step process, first list all the files and directories
        // then trigger asynchronous tasks to read each file and subdirectories
        public DirectoryController()
        {
        }

        public winDirsStat_clone.Data.Directory ReadDirectory(string path = @"C:\Users\aaron\OneDrive - WGC SISTEMAS LTDA\Área de Trabalho\Ebooks\")
        {
            List<string> ext = new List<string>();
            winDirsStat_clone.Data.Directory directory = new winDirsStat_clone.Data.Directory(path);            
            
            if (new DirectoryInfo(path).Exists)
            {
                directory.files = Task.Factory.StartNew(() => Directory.GetFiles(path)).Result;
                directory.subDirectories = Task.Factory.StartNew(() => Directory.GetDirectories(path)).Result;

                if (directory.files.Length > 0)
                    foreach (var fl in directory.files)
                    {                        
                        directory.mbSize +=
                                // MB ---------------------------------
                                // KB -------------------------
                                File.OpenRead(fl).Length / 1024 / 1000;

                        string extension = new FileInfo(fl).Extension.ToString();
                        if (!ext.Contains(extension))
                            ext.Add(extension);
                    }                

                directory.fileExtensions = ext;
            }

            return directory;
        }
    }
}
