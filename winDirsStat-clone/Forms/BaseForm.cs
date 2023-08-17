using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using winDirsStat_clone.Controllers;

namespace winDirsStat_clone
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
            ClearDependencies();
            FillTreeList();
        }

        #region Events
        private void ckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {

        }
        #endregion

        #region Methods
        private void ClearDependencies()
        {
            ckBox.Items.Clear();
            treeView.Nodes.Clear();
        }

        private void FillTreeList()
        {
            var data = new DirectoryController().ReadDirectory();
            var node = new TreeNode(data.path);

            treeView.Nodes.Add(node);
            ReadSubdirectories(data, node);
        }

        private void ReadSubdirectories(Data.Directory data, TreeNode fatherNode)
        {
            // Asynchronous initial tasks
            Task.Factory.StartNew(() => {

                // Read files from initial directory
                if (data.files != null)
                    foreach (string fileName in data.files)
                        fatherNode.Nodes.Add(CreateFileTreeNode(fileName.Replace(data.path, "..")));

                // Read file extensions from initial directory
                if (data.fileExtensions != null)
                    foreach (string ext in data.fileExtensions)
                        ckBox.Items.Add(ext);

                foreach (string subDirectoriesName in data.subDirectories)
                {
                    var dirData = new DirectoryController().ReadDirectory(subDirectoriesName);
                    var subNode = new TreeNode();

                    if (dirData.files != null)
                        foreach (var subData in dirData.files)
                            subNode.Nodes.Add(CreateFileTreeNode(subData));

                    if (dirData.fileExtensions != null)
                        foreach (string ext in dirData.fileExtensions)
                            if (!ckBox.Items.Contains(ext))
                                ckBox.Items.Add(ext);

                    CreateFolderTreeNode(subDirectoriesName.Replace(fatherNode.Text, ".."), dirData, ref subNode);
                    fatherNode.Nodes.Add(subNode);
                    treeView.ExpandAll();

                    // Asynchronous subTasks, reading each subdirectory
                    Task.Factory.StartNew(() =>
                    {
                        if (dirData.subDirectories.Length > 0)
                            ReadSubdirectories(dirData, subNode);
                    });
                }
            });
        }

        private TreeNode CreateFileTreeNode(string text)
        {
            var fileTreeNode = new TreeNode(text);
            fileTreeNode.BackColor = Color.LightGray;
            return fileTreeNode;
        }
        private void CreateFolderTreeNode(string txt, Data.Directory data, ref TreeNode node)
        {
            if (data.files.Length > 0)
                node.Text = $"{txt}            - Folder Size: {data.mbSize} MB";
            else
                node.Text = $"{txt}            - No files";

            node.BackColor = Color.LightBlue;
        }
        #endregion
    }
}
