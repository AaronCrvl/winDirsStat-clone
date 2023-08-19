namespace winDirsStat_clone.Data.Objects
{
    public class ExtensionData
    {
        #region Constructors
        public ExtensionData(string name)
        {
            this.extensionName = name;
            fileCount = 1;
        }
        #endregion

        #region Properties
        public string extensionName;
        public long extensionTotalSize;
        public int fileCount;        
        #endregion

        #region Methods
        public void updateExtData(ExtensionData data)
        {
            this.extensionTotalSize = data.extensionTotalSize;
        }
        #endregion        
    }
}
