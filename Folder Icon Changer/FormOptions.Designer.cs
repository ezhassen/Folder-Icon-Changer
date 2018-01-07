namespace Folder_Icon_Changer
{
    partial class FormOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormOptions));
            this.cbLangs = new System.Windows.Forms.ComboBox();
            this.gbLanguage = new System.Windows.Forms.GroupBox();
            this.bRefresh = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.gbOtherOptions = new System.Windows.Forms.GroupBox();
            this.cbShowCurrentFolderForIconFromImage = new System.Windows.Forms.CheckBox();
            this.gbLanguage.SuspendLayout();
            this.gbOtherOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbLangs
            // 
            this.cbLangs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbLangs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLangs.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbLangs.FormattingEnabled = true;
            this.cbLangs.Location = new System.Drawing.Point(72, 35);
            this.cbLangs.Name = "cbLangs";
            this.cbLangs.Size = new System.Drawing.Size(202, 24);
            this.cbLangs.TabIndex = 0;
            // 
            // gbLanguage
            // 
            this.gbLanguage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbLanguage.Controls.Add(this.bRefresh);
            this.gbLanguage.Controls.Add(this.cbLangs);
            this.gbLanguage.Location = new System.Drawing.Point(12, 12);
            this.gbLanguage.Name = "gbLanguage";
            this.gbLanguage.Size = new System.Drawing.Size(280, 93);
            this.gbLanguage.TabIndex = 1;
            this.gbLanguage.TabStop = false;
            this.gbLanguage.Text = "Language";
            // 
            // bRefresh
            // 
            this.bRefresh.Image = global::Folder_Icon_Changer.Properties.Resources.Refresh_32P;
            this.bRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bRefresh.Location = new System.Drawing.Point(5, 19);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(60, 59);
            this.bRefresh.TabIndex = 23;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bCancel
            // 
            this.bCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bCancel.Location = new System.Drawing.Point(217, 193);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 28);
            this.bCancel.TabIndex = 2;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(136, 193);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 28);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "OK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // gbOtherOptions
            // 
            this.gbOtherOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbOtherOptions.Controls.Add(this.cbShowCurrentFolderForIconFromImage);
            this.gbOtherOptions.Location = new System.Drawing.Point(12, 111);
            this.gbOtherOptions.Name = "gbOtherOptions";
            this.gbOtherOptions.Size = new System.Drawing.Size(280, 76);
            this.gbOtherOptions.TabIndex = 3;
            this.gbOtherOptions.TabStop = false;
            this.gbOtherOptions.Text = "Other Options";
            // 
            // cbShowCurrentFolderForIconFromImage
            // 
            this.cbShowCurrentFolderForIconFromImage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbShowCurrentFolderForIconFromImage.AutoEllipsis = true;
            this.cbShowCurrentFolderForIconFromImage.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.ShowCurrentFolderForIconFromImage;
            this.cbShowCurrentFolderForIconFromImage.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowCurrentFolderForIconFromImage.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Folder_Icon_Changer.Properties.Settings.Default, "ShowCurrentFolderForIconFromImage", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbShowCurrentFolderForIconFromImage.Location = new System.Drawing.Point(6, 19);
            this.cbShowCurrentFolderForIconFromImage.Name = "cbShowCurrentFolderForIconFromImage";
            this.cbShowCurrentFolderForIconFromImage.Size = new System.Drawing.Size(268, 51);
            this.cbShowCurrentFolderForIconFromImage.TabIndex = 0;
            this.cbShowCurrentFolderForIconFromImage.Text = "ShowCurrentFolderForIconFromImageShowCurrentFolderForIconFromImage";
            this.cbShowCurrentFolderForIconFromImage.UseVisualStyleBackColor = true;
            // 
            // FormOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bCancel;
            this.ClientSize = new System.Drawing.Size(304, 233);
            this.Controls.Add(this.gbOtherOptions);
            this.Controls.Add(this.bOK);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.gbLanguage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.FormOptions_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormOptions_MouseDown);
            this.gbLanguage.ResumeLayout(false);
            this.gbOtherOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbLangs;
        private System.Windows.Forms.GroupBox gbLanguage;
        private System.Windows.Forms.Button bCancel;
        private System.Windows.Forms.Button bOK;
        internal System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.GroupBox gbOtherOptions;
        private System.Windows.Forms.CheckBox cbShowCurrentFolderForIconFromImage;
    }
}