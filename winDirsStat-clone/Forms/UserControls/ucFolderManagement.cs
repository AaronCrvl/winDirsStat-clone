using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        TreeView treeCopy = new TreeView();
        List<ExtensionData> extensions = new List<ExtensionData>();
        #endregion        

        #region Events
        void ckBoxList_ItemCheck(object sender, ItemCheckEventArgs e)
        {           
            treeView.Nodes.Clear();
                           
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

        void treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                // double click only open directory for now to prevent from exceptions                
                if (cDirectory.DirectoryExists(e.Node.Text)) // double click on a directory
                    Process.Start(e.Node.Text);
                else // double click on file
                {
                    System.Reflection.PropertyInfo pi = e.Node.Tag.GetType().GetProperty("fullPath");
                    String fullPath = pi == null ? string.Empty : (String)(pi.GetValue(e.Node.Tag, null));
                    Process.Start(cFile.GetFilePath(fullPath + "\\"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Methods
        public void ClearDependencies()
        {
            ckBoxList.Items.Clear();
            treeView.Nodes.Clear();
            treeCopy.Nodes.Clear();
        }

        public async void FillTreeList(string path)
        {           
            lblLoad.Visible = true;            
            ClearDependencies();
            treeView.CreateGraphics();

            var data = cDirectory.ReadDirectory(path);
            var node = new TreeNode(data.path);
            node.BackColor = Color.LightCyan;            
            treeView.Nodes.Add(node);

            await ReadSubdirectories(data, node);
            await CalculateFileDataQuantities(treeView.Nodes);
            await CopyTreeViewNodes();
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
                    var dirData = cDirectory.ReadDirectory(subDirectoriesName);
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
                        fileCount = cDirectory.GetDirectoryFileCount(subDirectoriesName),
                    });
                    fatherNode.Nodes.Add(subNode);

                    // Asynchronous subTasks, reading each subdirectory
                    if (dirData.subDirectories.Length > 0)
                        await ReadSubdirectories(dirData, subNode);
                });                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task CopyTreeViewNodes()
        {            
            foreach (TreeNode node in treeView.Nodes)
                treeCopy.Nodes.Add((TreeNode)node.Clone());
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

                        pi = node.Tag.GetType().GetProperty("FileExists");
                        Boolean FileExists = pi == null ? false : (Boolean)(pi.GetValue(node.Tag, null));

                        if (FileExists)
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
                // ignore file if it is not supported , register it on a error log file
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
            fileTreeNode.Tag = new // Treenode.Tag stores a object with node attributes  
            {
                fullPath = cFile.GetFilePath(path),
                fileSize = cFile.GetFileSizeInMB(path),
                extension = cFile.GetFileExtension(path),
                FileExists = cFile.FileExists(path),
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
            treeView.Nodes.Clear();
            if (ckBoxList.SelectedItems.Count == 0)
            {
                foreach (TreeNode node in treeCopy.Nodes)
                    treeView.Nodes.Add(node);

                return;
            }

            var collection = subNode == null ? treeCopy.Nodes : subNode;
            foreach (TreeNode node in collection)
            {
                if (node == null)
                    continue;

                // Is directory ?
                if (node.Nodes.Count > 0)
                {
                    treeView.Nodes.Add((TreeNode)node.Clone());
                    FilterTreeView(indexes, node.Nodes);
                }                 
                else
                {                                  
                    System.Reflection.PropertyInfo pi = node.Tag.GetType().GetProperty("FileExists");
                    Boolean FileExists = pi == null ? false : (Boolean)(pi.GetValue(node.Tag, null));
                    if (FileExists)
                    {
                        pi = node.Tag.GetType().GetProperty("extension");
                        String extension = pi == null ? "" : (String)pi.GetValue(node.Tag, null);                        

                        if (indexes.ToList<String>().Contains(extension) && treeView.Nodes.IndexOf(node) == -1)
                            treeView.Nodes.Add((TreeNode)node.Clone());
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
