using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using winDirsStat_clone.Controllers;
using winDirsStat_clone.Data.Objects;

namespace winDirsStat_clone.Forms.UserControls
{
    public partial class ucFolderManagement : UserControl
    {
        #region Constructor
        public ucFolderManagement()
        {
            InitializeComponent();
            ClearDependencies();
            lblLoad.Visible = false;
        }
        #endregion

        #region Properties
        TreeView tree;
        List<ExtensionData> extensions = new List<ExtensionData>();
        #endregion        

        #region Events
        void ckBoxList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(!treeView.Nodes.Equals(tree.Nodes))
                treeView = tree;

            var checkedExtensions = new List<string>();
            checkedExtensions.Add((string)ckBoxList.SelectedItem);

            if (ckBoxList.CheckedItems.Count == 0)                                   
                FilterTreeView(checkedExtensions);            
            else
            {                
                var items = ckBoxList.CheckedItems;
                foreach (var extension in items)
                    checkedExtensions.Add(extension.ToString());

                FilterTreeView(checkedExtensions);
            }
        }
        #endregion

        #region Methods
        void ClearDependencies()
        {
            ckBoxList.Items.Clear();
            treeView.Nodes.Clear();
        }

        public async void FillTreeList(string path)
        {
            lblLoad.Visible = true;
            ClearDependencies();

            var data = DirectoryController.ReadDirectory(path);
            var node = new TreeNode(data.path);
            node.BackColor = Color.LightBlue;            
            treeView.Nodes.Add(node);

            await ReadSubdirectories(data, node);
            await CalculateFileDataQuantities(treeView.Nodes);
            treeView.ExpandAll();         
            lblLoad.Visible = false;
        }

        async Task ReadSubdirectories(Data.Directory data, TreeNode fatherNode)
        {
            try
            {
                // Read file extensions from initial directory and fill initial treeview node
                if (data.files != null)
                    data.files.ToList().ForEach((fileName) =>
                    {
                        // check if file already exist on node
                        if (!fatherNode.Nodes.ContainsKey(fatherNode.Nodes.IndexOf(new TreeNode(fileName)).ToString()))
                            fatherNode.Nodes.Add(CreateFileTreeNode(fileName.Replace(data.path, "."), fileName));
                    });

                // Read file extensions from initial directory and fill checkBox
                if (data.fileExtensions != null)
                    data.fileExtensions.ToList().ForEach((ext) =>
                    {
                        ckBoxList.Items.Add(ext);
                    });

                // Async LINQ foreach for reading subdirectories 
                data.subDirectories.ToList<string>().ForEach(async (subDirectoriesName) =>
                {
                    var dirData = DirectoryController.ReadDirectory(subDirectoriesName);
                    var subNode = new TreeNode(subDirectoriesName);

                    if (dirData.files != null)
                        dirData.files.ToList<string>().ForEach((subData) =>
                        {                            
                            if (!subNode.Nodes.ContainsKey(fatherNode.Nodes.IndexOf(new TreeNode(subData.Replace(dirData.path, ".."))).ToString()))
                                subNode.Nodes.Add(CreateFileTreeNode(subData.Replace(dirData.path, ".."), subData));
                        });

                    if (dirData.fileExtensions != null)
                        dirData.fileExtensions.ForEach((ext) =>
                        {
                            if (!ckBoxList.Items.Contains(ext))
                                ckBoxList.Items.Add(ext);
                        });

                    CreateFolderTreeNode(ref subNode, new
                    {
                        fullPath = subDirectoriesName,
                        name = subDirectoriesName.Replace(dirData.path, ".."),
                        fileCount = DirectoryController.GetDirectoryFileCount(subDirectoriesName),
                    });
                    fatherNode.Nodes.Add(subNode);

                    // Asynchronous subTasks, reading each subdirectory
                    if (dirData.subDirectories.Length > 0)
                        await ReadSubdirectories(dirData, subNode);
                });

                tree = treeView;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task CalculateFileDataQuantities(TreeNodeCollection nodeCollection)
        {
            try
            {
                foreach (TreeNode node in nodeCollection)
                    if (node.Text.Contains("."))
                    {
                        // get Treenode.Tag (object) members
                        System.Reflection.PropertyInfo pi = node.Tag.GetType().GetProperty("fileSize");
                        long fileSize = pi == null ? 0 : (long)pi.GetValue(node.Tag, null);

                        pi = node.Tag.GetType().GetProperty("fullPath");
                        String fullPath = pi == null ? string.Empty : (String)(pi.GetValue(node.Tag, null));

                        pi = node.Tag.GetType().GetProperty("extension");
                        String extension = pi == null ? string.Empty : (String)(pi.GetValue(node.Tag, null));

                        pi = node.Tag.GetType().GetProperty("exists");
                        Boolean exists = pi == null ? false : (Boolean)(pi.GetValue(node.Tag, null));

                        if (exists)
                            if (ContainsInExtensionList(extension))
                            {
                                foreach (var _ext in extensions)
                                    if (_ext.extensionName == extension)
                                    {
                                        _ext.extensionTotalSize += fileSize;
                                        _ext.fileCount += 1;
                                    }
                            }
                            else
                            {
                                var ext = new ExtensionData(extension);
                                ext.extensionTotalSize = fileSize;
                                extensions.Add(ext);
                            }
                    }
                    else
                    {
                        if (node.Nodes.Count > 1)
                            await CalculateFileDataQuantities(node.Nodes);
                    }

                FillExtensionsListBox();
            }
            catch (Exception ex)
            {
                // ignore not supported file exception and log it on the console error log file
                if (ex.GetType() == new System.NotSupportedException().GetType())
                    Console.WriteLine($"Some file is not supported. \t\n InnerException: {ex.InnerException} \t\n StackTrace: {ex.StackTrace}");
                else
                    throw ex;
            }
        }

        void FillExtensionsListBox()
        {
            if (listBox.Items.Count > 0)
                listBox.Items.Clear();

            extensions.OrderByDescending(ext => ext.extensionTotalSize).ToList().ForEach((ext) =>
            {
                listBox.Items.Add($"{ext.extensionName} - File Count: {ext.fileCount} - Total Size: {ext.extensionTotalSize} MB");
            });
        }

        TreeNode CreateFileTreeNode(string name, string path)
        {
            var fileTreeNode = new TreeNode(name);
            fileTreeNode.BackColor = Color.LightGray;
            fileTreeNode.Tag = new // Treenode.Tag stores a object with object attributes  
            {
                fullPath = path,
                fileSize = FileController.GetFileSizeInMB(path),
                extension = FileController.GetFileExtension(path),
                exists = FileController.FileExists(path),
            };            
            return fileTreeNode;
        }

        void CreateFolderTreeNode(ref TreeNode node, object obj)
        {
            node.BackColor = Color.LightBlue;
            node.Tag = obj;
        }

        void FilterTreeView(List<string> indexes, TreeNodeCollection subNode = null)
        {
            var collection = subNode == null ? treeView.Nodes : subNode;
            foreach (TreeNode node in collection)
            {
                if (node == null)
                    continue;

                // Is directory ?
                if (node.Nodes.Count > 0)
                    FilterTreeView(indexes, node.Nodes);                
                else
                {                    
                    if (node.Tag == null)
                        continue;

                    System.Reflection.PropertyInfo pi = node.Tag.GetType().GetProperty("exists");
                    Boolean exists = pi == null ? false : (Boolean)(pi.GetValue(node.Tag, null));
                    if (exists)
                    {
                        pi = node.Tag.GetType().GetProperty("extension");
                        String extension = pi == null ? "" : (String)pi.GetValue(node.Tag, null);

                        // BUGGGGGG - not filtering files in TreeNode
                        // Remove file nodes who dont have the especified extension
                        if (!indexes.ToList<string>().Contains(extension) && treeView.Nodes.IndexOf(node) != -1)
                            treeView.Nodes.RemoveAt(treeView.Nodes.IndexOf(node));
                    }
                }
            }
        }

        bool ContainsInExtensionList(string name)
        {
            foreach (var ext in extensions)
                if (ext.extensionName == name)
                    return true;

            return false;
        }
        #endregion   
    }
}
