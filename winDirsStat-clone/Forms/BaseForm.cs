using System;
using System.Windows.Forms;
using winDirsStat_clone.Forms.UserControls;

namespace winDirsStat_clone
{
    public partial class BaseForm : Form
    {  
        #region Constructor
        public BaseForm()
        {
            InitializeComponent();        
        }
        #endregion

        #region Events
        private void btnScan_Click(object sender, EventArgs e)
        {            
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))                                   
                    ucFolderManagement.FillTreeList(fbd.SelectedPath);                                    
            }
        }
        #endregion        
    }
}
