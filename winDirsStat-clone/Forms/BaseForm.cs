using System;
using System.Windows.Forms;
using winDirsStat_clone.Enums;
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
        void SearchDirectory()
        {            
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))                                   
                    ucFolderManagement.FillTreeList(fbd.SelectedPath);                                    
            }
        }
        #endregion

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            e.ToString();
        }

        private void optionsToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var option = e.ClickedItem.Text;
            switch (option)
            {
                case "Scan Directory":
                    SearchDirectory();
                    break;
                case "Clear Search":
                    ucFolderManagement.ClearDependencies();
                    break;
                default:
                    break;
            }
        }
    }
}
