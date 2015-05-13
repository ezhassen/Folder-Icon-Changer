namespace Folder_Icon_Changer
{
    partial class FormMain
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.label1 = new System.Windows.Forms.Label();
            this.gBNewIcon = new System.Windows.Forms.GroupBox();
            this.bNewShowIconGroup = new System.Windows.Forms.Button();
            this.bClearNewInfo = new System.Windows.Forms.Button();
            this.BGetNew = new System.Windows.Forms.Button();
            this.PBNew = new System.Windows.Forms.PictureBox();
            this.CBHideFile = new System.Windows.Forms.CheckBox();
            this.CBCopyIconToFolder = new System.Windows.Forms.CheckBox();
            this.bIconFromImage = new System.Windows.Forms.Button();
            this.BBrowseIcon = new System.Windows.Forms.Button();
            this.TBCurrentIconIndex = new System.Windows.Forms.TextBox();
            this.TBCurrentIcon = new System.Windows.Forms.TextBox();
            this.LabelCurrentIcon = new System.Windows.Forms.Label();
            this.BBrowseFolder = new System.Windows.Forms.Button();
            this.BRest = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BClose = new System.Windows.Forms.Button();
            this.BApply = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.BAbout = new System.Windows.Forms.Button();
            this.PBCurrent = new System.Windows.Forms.PictureBox();
            this.bCurrentShowIconGroup = new System.Windows.Forms.Button();
            this.tBTargetFolder = new Ezz_Helper.WinForms.EditControls.TextBoxEzz();
            this.nUpDownIconIndex = new Ezz_Helper.WinForms.EditControls.NumericUpDownEx();
            this.TBNewIcon = new Ezz_Helper.WinForms.EditControls.TextBoxEzz();
            this.contextMSBrowseIconFShell32 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemShell32 = new System.Windows.Forms.ToolStripMenuItem();
            this.gBNewIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNew)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDownIconIndex)).BeginInit();
            this.contextMSBrowseIconFShell32.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Target folder : ";
            this.label1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // gBNewIcon
            // 
            this.gBNewIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBNewIcon.Controls.Add(this.bNewShowIconGroup);
            this.gBNewIcon.Controls.Add(this.bClearNewInfo);
            this.gBNewIcon.Controls.Add(this.BGetNew);
            this.gBNewIcon.Controls.Add(this.PBNew);
            this.gBNewIcon.Controls.Add(this.nUpDownIconIndex);
            this.gBNewIcon.Controls.Add(this.CBHideFile);
            this.gBNewIcon.Controls.Add(this.CBCopyIconToFolder);
            this.gBNewIcon.Controls.Add(this.bIconFromImage);
            this.gBNewIcon.Controls.Add(this.BBrowseIcon);
            this.gBNewIcon.Controls.Add(this.TBNewIcon);
            this.gBNewIcon.Location = new System.Drawing.Point(15, 212);
            this.gBNewIcon.Name = "gBNewIcon";
            this.gBNewIcon.Size = new System.Drawing.Size(477, 155);
            this.gBNewIcon.TabIndex = 5;
            this.gBNewIcon.TabStop = false;
            this.gBNewIcon.Text = "New Icon Info";
            // 
            // bNewShowIconGroup
            // 
            this.bNewShowIconGroup.Enabled = false;
            this.bNewShowIconGroup.Location = new System.Drawing.Point(182, 48);
            this.bNewShowIconGroup.Name = "bNewShowIconGroup";
            this.bNewShowIconGroup.Size = new System.Drawing.Size(132, 23);
            this.bNewShowIconGroup.TabIndex = 24;
            this.bNewShowIconGroup.Text = "Show icon group";
            this.bNewShowIconGroup.UseVisualStyleBackColor = true;
            this.bNewShowIconGroup.Click += new System.EventHandler(this.bNewShowIconGroup_Click);
            // 
            // bClearNewInfo
            // 
            this.bClearNewInfo.Location = new System.Drawing.Point(6, 19);
            this.bClearNewInfo.Name = "bClearNewInfo";
            this.bClearNewInfo.Size = new System.Drawing.Size(50, 23);
            this.bClearNewInfo.TabIndex = 20;
            this.bClearNewInfo.Text = "Clear";
            this.bClearNewInfo.UseVisualStyleBackColor = true;
            this.bClearNewInfo.Click += new System.EventHandler(this.bClearNewInfo_Click);
            // 
            // BGetNew
            // 
            this.BGetNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BGetNew.Location = new System.Drawing.Point(355, 15);
            this.BGetNew.Name = "BGetNew";
            this.BGetNew.Size = new System.Drawing.Size(52, 44);
            this.BGetNew.TabIndex = 19;
            this.BGetNew.Text = "Get icon";
            this.BGetNew.UseVisualStyleBackColor = true;
            this.BGetNew.Click += new System.EventHandler(this.BGetNew_Click);
            // 
            // PBNew
            // 
            this.PBNew.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PBNew.Location = new System.Drawing.Point(83, 48);
            this.PBNew.Name = "PBNew";
            this.PBNew.Size = new System.Drawing.Size(96, 96);
            this.PBNew.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBNew.TabIndex = 18;
            this.PBNew.TabStop = false;
            this.PBNew.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // CBHideFile
            // 
            this.CBHideFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CBHideFile.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.HideTheFileA;
            this.CBHideFile.Location = new System.Drawing.Point(188, 132);
            this.CBHideFile.Name = "CBHideFile";
            this.CBHideFile.Size = new System.Drawing.Size(283, 17);
            this.CBHideFile.TabIndex = 12;
            this.CBHideFile.Text = "Hide the file (after copying OR if it in the target folder)";
            this.CBHideFile.UseVisualStyleBackColor = true;
            // 
            // CBCopyIconToFolder
            // 
            this.CBCopyIconToFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CBCopyIconToFolder.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.CopyIconT;
            this.CBCopyIconToFolder.Location = new System.Drawing.Point(188, 109);
            this.CBCopyIconToFolder.Name = "CBCopyIconToFolder";
            this.CBCopyIconToFolder.Size = new System.Drawing.Size(289, 17);
            this.CBCopyIconToFolder.TabIndex = 12;
            this.CBCopyIconToFolder.Text = "Copy icon to the folder";
            this.CBCopyIconToFolder.UseVisualStyleBackColor = true;
            // 
            // bIconFromImage
            // 
            this.bIconFromImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bIconFromImage.Location = new System.Drawing.Point(355, 65);
            this.bIconFromImage.Name = "bIconFromImage";
            this.bIconFromImage.Size = new System.Drawing.Size(109, 27);
            this.bIconFromImage.TabIndex = 7;
            this.bIconFromImage.Text = "IconFromImage";
            this.bIconFromImage.UseVisualStyleBackColor = true;
            this.bIconFromImage.Visible = false;
            // 
            // BBrowseIcon
            // 
            this.BBrowseIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBrowseIcon.ContextMenuStrip = this.contextMSBrowseIconFShell32;
            this.BBrowseIcon.Location = new System.Drawing.Point(413, 15);
            this.BBrowseIcon.Name = "BBrowseIcon";
            this.BBrowseIcon.Size = new System.Drawing.Size(51, 44);
            this.BBrowseIcon.TabIndex = 7;
            this.BBrowseIcon.Text = "---";
            this.BBrowseIcon.UseVisualStyleBackColor = true;
            this.BBrowseIcon.Click += new System.EventHandler(this.BBrowseIcon_Click);
            // 
            // TBCurrentIconIndex
            // 
            this.TBCurrentIconIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TBCurrentIconIndex.Location = new System.Drawing.Point(410, 110);
            this.TBCurrentIconIndex.Name = "TBCurrentIconIndex";
            this.TBCurrentIconIndex.ReadOnly = true;
            this.TBCurrentIconIndex.Size = new System.Drawing.Size(33, 20);
            this.TBCurrentIconIndex.TabIndex = 13;
            this.TBCurrentIconIndex.WordWrap = false;
            // 
            // TBCurrentIcon
            // 
            this.TBCurrentIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBCurrentIcon.Location = new System.Drawing.Point(197, 110);
            this.TBCurrentIcon.Name = "TBCurrentIcon";
            this.TBCurrentIcon.ReadOnly = true;
            this.TBCurrentIcon.Size = new System.Drawing.Size(207, 20);
            this.TBCurrentIcon.TabIndex = 14;
            this.TBCurrentIcon.WordWrap = false;
            // 
            // LabelCurrentIcon
            // 
            this.LabelCurrentIcon.AutoSize = true;
            this.LabelCurrentIcon.Location = new System.Drawing.Point(11, 113);
            this.LabelCurrentIcon.Name = "LabelCurrentIcon";
            this.LabelCurrentIcon.Size = new System.Drawing.Size(78, 13);
            this.LabelCurrentIcon.TabIndex = 12;
            this.LabelCurrentIcon.Text = "Current Icon : ";
            this.LabelCurrentIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // BBrowseFolder
            // 
            this.BBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBrowseFolder.Location = new System.Drawing.Point(463, 65);
            this.BBrowseFolder.Name = "BBrowseFolder";
            this.BBrowseFolder.Size = new System.Drawing.Size(35, 26);
            this.BBrowseFolder.TabIndex = 15;
            this.BBrowseFolder.Text = "---";
            this.BBrowseFolder.UseVisualStyleBackColor = true;
            this.BBrowseFolder.Click += new System.EventHandler(this.BBrowseFolder_Click);
            // 
            // BRest
            // 
            this.BRest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.BRest.Enabled = false;
            this.BRest.Location = new System.Drawing.Point(12, 373);
            this.BRest.Name = "BRest";
            this.BRest.Size = new System.Drawing.Size(119, 34);
            this.BRest.TabIndex = 17;
            this.BRest.Text = "Rest to default icon";
            this.BRest.UseVisualStyleBackColor = true;
            this.BRest.Click += new System.EventHandler(this.BRest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 416);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(504, 22);
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel1.Text = "---";
            // 
            // BClose
            // 
            this.BClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BClose.Location = new System.Drawing.Point(428, 373);
            this.BClose.Name = "BClose";
            this.BClose.Size = new System.Drawing.Size(64, 34);
            this.BClose.TabIndex = 19;
            this.BClose.Text = "Close";
            this.BClose.UseVisualStyleBackColor = true;
            this.BClose.Click += new System.EventHandler(this.BClose_Click);
            // 
            // BApply
            // 
            this.BApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.BApply.Enabled = false;
            this.BApply.Location = new System.Drawing.Point(325, 373);
            this.BApply.Name = "BApply";
            this.BApply.Size = new System.Drawing.Size(97, 34);
            this.BApply.TabIndex = 20;
            this.BApply.Text = "Apply";
            this.BApply.UseVisualStyleBackColor = true;
            this.BApply.Click += new System.EventHandler(this.BApply_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.Location = new System.Drawing.Point(95, 21);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(60, 40);
            this.bRefresh.TabIndex = 22;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.UseVisualStyleBackColor = true;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // BAbout
            // 
            this.BAbout.Image = global::Folder_Icon_Changer.Properties.Resources.ico_alpha_Information_32x32;
            this.BAbout.Location = new System.Drawing.Point(12, 12);
            this.BAbout.Name = "BAbout";
            this.BAbout.Size = new System.Drawing.Size(40, 40);
            this.BAbout.TabIndex = 23;
            this.BAbout.UseVisualStyleBackColor = true;
            this.BAbout.Click += new System.EventHandler(this.BAbout_Click);
            // 
            // PBCurrent
            // 
            this.PBCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PBCurrent.Location = new System.Drawing.Point(95, 100);
            this.PBCurrent.Name = "PBCurrent";
            this.PBCurrent.Size = new System.Drawing.Size(96, 96);
            this.PBCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBCurrent.TabIndex = 11;
            this.PBCurrent.TabStop = false;
            this.PBCurrent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // bCurrentShowIconGroup
            // 
            this.bCurrentShowIconGroup.Enabled = false;
            this.bCurrentShowIconGroup.Location = new System.Drawing.Point(197, 136);
            this.bCurrentShowIconGroup.Name = "bCurrentShowIconGroup";
            this.bCurrentShowIconGroup.Size = new System.Drawing.Size(132, 23);
            this.bCurrentShowIconGroup.TabIndex = 24;
            this.bCurrentShowIconGroup.Text = "Show icon group";
            this.bCurrentShowIconGroup.UseVisualStyleBackColor = true;
            this.bCurrentShowIconGroup.Click += new System.EventHandler(this.bCurrentShowIconGroup_Click);
            // 
            // tBTargetFolder
            // 
            this.tBTargetFolder.AllowToCalculate = false;
            this.tBTargetFolder.AllowToTogglesTheSign = false;
            this.tBTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBTargetFolder.EditType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.EditTypes.Path;
            this.tBTargetFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBTargetFolder.FreeInputForCalculating = false;
            this.tBTargetFolder.Location = new System.Drawing.Point(95, 67);
            this.tBTargetFolder.MaxNumber = 1.7976931348623157E+308D;
            this.tBTargetFolder.MinNumber = -1.7976931348623157E+308D;
            this.tBTargetFolder.Name = "tBTargetFolder";
            this.tBTargetFolder.NumericType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.NumericTypes.AcceptAny;
            this.tBTargetFolder.Size = new System.Drawing.Size(362, 23);
            this.tBTargetFolder.TabIndex = 16;
            // 
            // nUpDownIconIndex
            // 
            this.nUpDownIconIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nUpDownIconIndex.Location = new System.Drawing.Point(303, 20);
            this.nUpDownIconIndex.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nUpDownIconIndex.Name = "nUpDownIconIndex";
            this.nUpDownIconIndex.Size = new System.Drawing.Size(45, 20);
            this.nUpDownIconIndex.TabIndex = 17;
            this.nUpDownIconIndex.ValueChanged += new System.EventHandler(this.nUpDownIconIndex_ValueChanged);
            // 
            // TBNewIcon
            // 
            this.TBNewIcon.AllowToCalculate = false;
            this.TBNewIcon.AllowToTogglesTheSign = false;
            this.TBNewIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TBNewIcon.EditType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.EditTypes.Path;
            this.TBNewIcon.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBNewIcon.FreeInputForCalculating = false;
            this.TBNewIcon.Location = new System.Drawing.Point(62, 19);
            this.TBNewIcon.MaxNumber = 1.7976931348623157E+308D;
            this.TBNewIcon.MinNumber = -1.7976931348623157E+308D;
            this.TBNewIcon.Name = "TBNewIcon";
            this.TBNewIcon.NumericType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.NumericTypes.AcceptAny;
            this.TBNewIcon.ReadOnly = true;
            this.TBNewIcon.Size = new System.Drawing.Size(235, 23);
            this.TBNewIcon.TabIndex = 16;
            // 
            // contextMSBrowseIconFShell32
            // 
            this.contextMSBrowseIconFShell32.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShell32});
            this.contextMSBrowseIconFShell32.Name = "contextMSBrowseIconFShell32";
            this.contextMSBrowseIconFShell32.Size = new System.Drawing.Size(213, 48);
            // 
            // toolStripMenuItemShell32
            // 
            this.toolStripMenuItemShell32.Name = "toolStripMenuItemShell32";
            this.toolStripMenuItemShell32.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItemShell32.Text = "Browse to system shell file";
            this.toolStripMenuItemShell32.Click += new System.EventHandler(this.toolStripMenuItemShell32_Click);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 438);
            this.Controls.Add(this.bCurrentShowIconGroup);
            this.Controls.Add(this.BAbout);
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.BClose);
            this.Controls.Add(this.BApply);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BRest);
            this.Controls.Add(this.tBTargetFolder);
            this.Controls.Add(this.BBrowseFolder);
            this.Controls.Add(this.TBCurrentIconIndex);
            this.Controls.Add(this.TBCurrentIcon);
            this.Controls.Add(this.LabelCurrentIcon);
            this.Controls.Add(this.PBCurrent);
            this.Controls.Add(this.gBNewIcon);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(520, 477);
            this.Name = "FormMain";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Folder Icon Changer v{0} By ezhassen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.gBNewIcon.ResumeLayout(false);
            this.gBNewIcon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNew)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDownIconIndex)).EndInit();
            this.contextMSBrowseIconFShell32.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gBNewIcon;
        internal System.Windows.Forms.TextBox TBCurrentIconIndex;
        internal System.Windows.Forms.TextBox TBCurrentIcon;
        internal System.Windows.Forms.Label LabelCurrentIcon;
        internal System.Windows.Forms.PictureBox PBCurrent;
        internal System.Windows.Forms.Button BBrowseFolder;
        private Ezz_Helper.WinForms.EditControls.TextBoxEzz tBTargetFolder;
        internal System.Windows.Forms.Button BBrowseIcon;
        internal System.Windows.Forms.CheckBox CBCopyIconToFolder;
        internal System.Windows.Forms.Button BRest;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Ezz_Helper.WinForms.EditControls.TextBoxEzz TBNewIcon;
        private Ezz_Helper.WinForms.EditControls.NumericUpDownEx nUpDownIconIndex;
        internal System.Windows.Forms.PictureBox PBNew;
        internal System.Windows.Forms.Button BClose;
        internal System.Windows.Forms.Button BApply;
        internal System.Windows.Forms.CheckBox CBHideFile;
        internal System.Windows.Forms.Button bIconFromImage;
        internal System.Windows.Forms.Button BGetNew;
        internal System.Windows.Forms.Button bRefresh;
        internal System.Windows.Forms.Button BAbout;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button bClearNewInfo;
        private System.Windows.Forms.Button bCurrentShowIconGroup;
        private System.Windows.Forms.Button bNewShowIconGroup;
        private System.Windows.Forms.ContextMenuStrip contextMSBrowseIconFShell32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShell32;
    }
}

