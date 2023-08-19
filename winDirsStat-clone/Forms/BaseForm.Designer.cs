
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
            this.btnScan = new System.Windows.Forms.Button();
            this.ucFolderManagement = new winDirsStat_clone.Forms.UserControls.ucFolderManagement();
            this.SuspendLayout();
            // 
            // btnScan
            // 
            this.btnScan.Font = new System.Drawing.Font("LEMON MILK Light", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnScan.Location = new System.Drawing.Point(35, 27);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(128, 23);
            this.btnScan.TabIndex = 0;
            this.btnScan.Text = "Scan Directory";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // ucFolderManagement
            // 
            this.ucFolderManagement.Location = new System.Drawing.Point(12, 56);
            this.ucFolderManagement.Name = "ucFolderManagement";
            this.ucFolderManagement.Size = new System.Drawing.Size(1085, 376);
            this.ucFolderManagement.TabIndex = 0;
            // 
            // BaseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 444);
            this.Controls.Add(this.btnScan);
            this.Controls.Add(this.ucFolderManagement);
            this.Name = "BaseForm";
            this.Text = "winDirStat ";
            this.ResumeLayout(false);

        }

        #endregion

        private Forms.UserControls.ucFolderManagement ucFolderManagement;
        private System.Windows.Forms.Button btnScan;
    }
}

