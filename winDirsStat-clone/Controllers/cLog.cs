using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace winDirsStat_clone.Controllers
{
    public static class cLog
    {
        public static FileStream CreateLogFile()
        {
            try
            {                
                var logFilePath = $@".\Desktop\{GetLogFileName()}";
                if (!cFile.FileExists(logFilePath))
                    return File.Create(logFilePath);
                else 
                {
                    return File.OpenRead(logFilePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
         
        }

        static string GetLogFileName() => "Log_" +  DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + ".txt";        

        public static void WriteLog(string method, Exception e) 
        {            
            var stream = CreateLogFile();           
            var sw = new StreamWriter(stream);
            sw.WriteLine($"({DateTime.Now}) !_Error on {method} - Message: {e.Message} \n\t");
            sw.WriteLine($"Stack Trace : {e.StackTrace}\n\t");
            sw.WriteLine($"Inner Exception: {e.InnerException}\n\t");
            sw.WriteLine("\n\t");
        }
    }
}
