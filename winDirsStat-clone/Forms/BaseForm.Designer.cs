
namespace winDirsStat_clone
{
    partial class BaseForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scanDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearSearchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucFolderManagement = new winDirsStat_clone.Forms.UserControls.ucFolderManagement();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.menuStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.menuStrip1.Font = new System.Drawing.Font("LEMON MILK Light", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Visible;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1086, 25);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scanDirectoryToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(77, 21);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.optionsToolStripMenuItem_DropDownItemClicked);
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // scanDirectoryToolStripMenuItem
            // 
            this.scanDirectoryToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearSearchToolStripMenuItem});
            this.scanDirectoryToolStripMenuItem.Name = "scanDirectoryToolStripMenuItem";
            this.scanDirectoryToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.scanDirectoryToolStripMenuItem.Text = "Scan Directory";
            // 
            // clearSearchToolStripMenuItem
            // 
            this.clearSearchToolStripMenuItem.Name = "clearSearchToolStripMenuItem";
            this.clearSearchToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.clearSearchToolStripMenuItem.Text = "Clear Search";
            // 
            // ucFolderManagement
            // 
            this.ucFolderManagement.Location = new System.Drawing.Point(9, 28);
            this.ucFolderManagement.Name = "ucFolderManagement";
            this.ucFolderManagement.Size = new System.Drawing.Size(1085, 551);
            this.ucFolderManagement.TabIndex = 0;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 591);
            this.Controls.Add(this.ucFolderManagement);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BaseForm";
            this.Text = "winDirStat ";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Forms.UserControls.ucFolderManagement ucFolderManagement;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scanDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearSearchToolStripMenuItem;
    }
}

