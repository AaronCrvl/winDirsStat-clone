using System;
using System.Windows.Forms;

namespace winDirsStat_clone
{
    static class Program
    {       
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BaseForm());
        }
    }
}
