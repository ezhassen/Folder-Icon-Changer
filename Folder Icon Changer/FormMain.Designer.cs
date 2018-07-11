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
            this.labelTargetFolder = new System.Windows.Forms.Label();
            this.gBNewIcon = new System.Windows.Forms.GroupBox();
            this.bNewShowIconGroup = new System.Windows.Forms.Button();
            this.bClearNewInfo = new System.Windows.Forms.Button();
            this.BGetIcon = new System.Windows.Forms.Button();
            this.PBNew = new System.Windows.Forms.PictureBox();
            this.CBHideFile = new System.Windows.Forms.CheckBox();
            this.CBCopyIconToFolder = new System.Windows.Forms.CheckBox();
            this.bIconFromImage = new System.Windows.Forms.Button();
            this.BBrowseIcon = new System.Windows.Forms.Button();
            this.contextMSBrowseIconFShell32 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemShell32 = new System.Windows.Forms.ToolStripMenuItem();
            this.TBCurrentIconIndex = new System.Windows.Forms.TextBox();
            this.TBCurrentIcon = new System.Windows.Forms.TextBox();
            this.LabelCurrentIcon = new System.Windows.Forms.Label();
            this.BBrowseFolder = new System.Windows.Forms.Button();
            this.BRest = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.BClose = new System.Windows.Forms.Button();
            this.BApply = new System.Windows.Forms.Button();
            this.bCurrentShowIconGroup = new System.Windows.Forms.Button();
            this.PBCurrent = new System.Windows.Forms.PictureBox();
            this.bCurrentGenBestFit = new System.Windows.Forms.Button();
            this.bTopMost = new System.Windows.Forms.CheckBox();
            this.bAbout = new System.Windows.Forms.Button();
            this.bOptions = new System.Windows.Forms.Button();
            this.bRefresh = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tBTargetFolder = new Ezz_Helper.WinForms.EditControls.TextBoxEzz();
            this.nUpDownIconIndex = new Ezz_Helper.WinForms.EditControls.NumericUpDownEx();
            this.TBNewIcon = new Ezz_Helper.WinForms.EditControls.TextBoxEzz();
            this.gBNewIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNew)).BeginInit();
            this.contextMSBrowseIconFShell32.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDownIconIndex)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTargetFolder
            // 
            this.labelTargetFolder.AutoSize = true;
            this.labelTargetFolder.BackColor = System.Drawing.Color.Transparent;
            this.labelTargetFolder.Location = new System.Drawing.Point(9, 67);
            this.labelTargetFolder.Name = "labelTargetFolder";
            this.labelTargetFolder.Size = new System.Drawing.Size(80, 13);
            this.labelTargetFolder.TabIndex = 2;
            this.labelTargetFolder.Text = "Target folder : ";
            this.labelTargetFolder.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // gBNewIcon
            // 
            this.gBNewIcon.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gBNewIcon.BackColor = System.Drawing.Color.Transparent;
            this.gBNewIcon.Controls.Add(this.bNewShowIconGroup);
            this.gBNewIcon.Controls.Add(this.bClearNewInfo);
            this.gBNewIcon.Controls.Add(this.BGetIcon);
            this.gBNewIcon.Controls.Add(this.PBNew);
            this.gBNewIcon.Controls.Add(this.nUpDownIconIndex);
            this.gBNewIcon.Controls.Add(this.CBHideFile);
            this.gBNewIcon.Controls.Add(this.CBCopyIconToFolder);
            this.gBNewIcon.Controls.Add(this.bIconFromImage);
            this.gBNewIcon.Controls.Add(this.BBrowseIcon);
            this.gBNewIcon.Controls.Add(this.TBNewIcon);
            this.gBNewIcon.Location = new System.Drawing.Point(12, 212);
            this.gBNewIcon.Name = "gBNewIcon";
            this.gBNewIcon.Size = new System.Drawing.Size(496, 151);
            this.gBNewIcon.TabIndex = 5;
            this.gBNewIcon.TabStop = false;
            this.gBNewIcon.Text = "New Icon Info";
            // 
            // bNewShowIconGroup
            // 
            this.bNewShowIconGroup.BackColor = System.Drawing.Color.Orange;
            this.bNewShowIconGroup.Enabled = false;
            this.bNewShowIconGroup.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.bNewShowIconGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.bNewShowIconGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.bNewShowIconGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bNewShowIconGroup.Location = new System.Drawing.Point(182, 48);
            this.bNewShowIconGroup.Name = "bNewShowIconGroup";
            this.bNewShowIconGroup.Size = new System.Drawing.Size(132, 23);
            this.bNewShowIconGroup.TabIndex = 24;
            this.bNewShowIconGroup.Text = "Show icon group";
            this.bNewShowIconGroup.UseVisualStyleBackColor = false;
            this.bNewShowIconGroup.Click += new System.EventHandler(this.bNewShowIconGroup_Click);
            // 
            // bClearNewInfo
            // 
            this.bClearNewInfo.BackColor = System.Drawing.Color.Orange;
            this.bClearNewInfo.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.bClearNewInfo.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.bClearNewInfo.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.bClearNewInfo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bClearNewInfo.Location = new System.Drawing.Point(6, 19);
            this.bClearNewInfo.Name = "bClearNewInfo";
            this.bClearNewInfo.Size = new System.Drawing.Size(50, 23);
            this.bClearNewInfo.TabIndex = 20;
            this.bClearNewInfo.Text = "Clear";
            this.bClearNewInfo.UseVisualStyleBackColor = false;
            this.bClearNewInfo.Click += new System.EventHandler(this.bClearNewInfo_Click);
            // 
            // BGetIcon
            // 
            this.BGetIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BGetIcon.BackColor = System.Drawing.Color.Orange;
            this.BGetIcon.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BGetIcon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BGetIcon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BGetIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BGetIcon.Location = new System.Drawing.Point(374, 15);
            this.BGetIcon.Name = "BGetIcon";
            this.BGetIcon.Size = new System.Drawing.Size(52, 44);
            this.BGetIcon.TabIndex = 19;
            this.BGetIcon.Text = "Get icon";
            this.BGetIcon.UseVisualStyleBackColor = false;
            this.BGetIcon.Click += new System.EventHandler(this.BGetNew_Click);
            // 
            // PBNew
            // 
            this.PBNew.BackColor = System.Drawing.Color.LightSteelBlue;
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
            this.CBHideFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CBHideFile.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.HideTheFileA;
            this.CBHideFile.Location = new System.Drawing.Point(188, 128);
            this.CBHideFile.Name = "CBHideFile";
            this.CBHideFile.Size = new System.Drawing.Size(301, 17);
            this.CBHideFile.TabIndex = 12;
            this.CBHideFile.Text = "Hide if the icon exists in the target folder";
            this.CBHideFile.UseVisualStyleBackColor = true;
            // 
            // CBCopyIconToFolder
            // 
            this.CBCopyIconToFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CBCopyIconToFolder.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.CopyIconT;
            this.CBCopyIconToFolder.Location = new System.Drawing.Point(188, 105);
            this.CBCopyIconToFolder.Name = "CBCopyIconToFolder";
            this.CBCopyIconToFolder.Size = new System.Drawing.Size(301, 17);
            this.CBCopyIconToFolder.TabIndex = 12;
            this.CBCopyIconToFolder.Text = "Copy icon to the folder";
            this.CBCopyIconToFolder.UseVisualStyleBackColor = true;
            // 
            // bIconFromImage
            // 
            this.bIconFromImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bIconFromImage.BackColor = System.Drawing.Color.Orange;
            this.bIconFromImage.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.bIconFromImage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.bIconFromImage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.bIconFromImage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bIconFromImage.Location = new System.Drawing.Point(374, 65);
            this.bIconFromImage.Name = "bIconFromImage";
            this.bIconFromImage.Size = new System.Drawing.Size(109, 27);
            this.bIconFromImage.TabIndex = 7;
            this.bIconFromImage.Text = "IconFromImage";
            this.bIconFromImage.UseVisualStyleBackColor = false;
            this.bIconFromImage.Click += new System.EventHandler(this.bIconFromImage_Click);
            // 
            // BBrowseIcon
            // 
            this.BBrowseIcon.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BBrowseIcon.BackColor = System.Drawing.Color.Orange;
            this.BBrowseIcon.ContextMenuStrip = this.contextMSBrowseIconFShell32;
            this.BBrowseIcon.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BBrowseIcon.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BBrowseIcon.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BBrowseIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BBrowseIcon.Location = new System.Drawing.Point(432, 15);
            this.BBrowseIcon.Name = "BBrowseIcon";
            this.BBrowseIcon.Size = new System.Drawing.Size(51, 44);
            this.BBrowseIcon.TabIndex = 7;
            this.BBrowseIcon.Text = "---";
            this.BBrowseIcon.UseVisualStyleBackColor = false;
            this.BBrowseIcon.Click += new System.EventHandler(this.BBrowseIcon_Click);
            // 
            // contextMSBrowseIconFShell32
            // 
            this.contextMSBrowseIconFShell32.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShell32});
            this.contextMSBrowseIconFShell32.Name = "contextMSBrowseIconFShell32";
            this.contextMSBrowseIconFShell32.Size = new System.Drawing.Size(213, 26);
            // 
            // toolStripMenuItemShell32
            // 
            this.toolStripMenuItemShell32.Name = "toolStripMenuItemShell32";
            this.toolStripMenuItemShell32.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItemShell32.Text = "Browse to system shell file";
            this.toolStripMenuItemShell32.Click += new System.EventHandler(this.toolStripMenuItemShell32_Click);
            // 
            // TBCurrentIconIndex
            // 
            this.TBCurrentIconIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TBCurrentIconIndex.BackColor = System.Drawing.Color.LightSteelBlue;
            this.TBCurrentIconIndex.ForeColor = System.Drawing.Color.Black;
            this.TBCurrentIconIndex.Location = new System.Drawing.Point(426, 110);
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
            this.TBCurrentIcon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.TBCurrentIcon.ForeColor = System.Drawing.Color.Black;
            this.TBCurrentIcon.Location = new System.Drawing.Point(197, 110);
            this.TBCurrentIcon.Name = "TBCurrentIcon";
            this.TBCurrentIcon.ReadOnly = true;
            this.TBCurrentIcon.Size = new System.Drawing.Size(223, 20);
            this.TBCurrentIcon.TabIndex = 14;
            this.TBCurrentIcon.WordWrap = false;
            // 
            // LabelCurrentIcon
            // 
            this.LabelCurrentIcon.AutoSize = true;
            this.LabelCurrentIcon.BackColor = System.Drawing.Color.Transparent;
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
            this.BBrowseFolder.BackColor = System.Drawing.Color.Orange;
            this.BBrowseFolder.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BBrowseFolder.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BBrowseFolder.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BBrowseFolder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BBrowseFolder.Location = new System.Drawing.Point(479, 65);
            this.BBrowseFolder.Name = "BBrowseFolder";
            this.BBrowseFolder.Size = new System.Drawing.Size(35, 26);
            this.BBrowseFolder.TabIndex = 15;
            this.BBrowseFolder.Text = "---";
            this.BBrowseFolder.UseVisualStyleBackColor = false;
            this.BBrowseFolder.Click += new System.EventHandler(this.BBrowseFolder_Click);
            // 
            // BRest
            // 
            this.BRest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BRest.BackColor = System.Drawing.Color.Orange;
            this.BRest.Enabled = false;
            this.BRest.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BRest.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BRest.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BRest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BRest.Image = global::Folder_Icon_Changer.Properties.Resources.Undo3_32P;
            this.BRest.Location = new System.Drawing.Point(12, 369);
            this.BRest.Name = "BRest";
            this.BRest.Size = new System.Drawing.Size(120, 60);
            this.BRest.TabIndex = 17;
            this.BRest.Text = "Reset to default icon";
            this.BRest.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BRest.UseVisualStyleBackColor = false;
            this.BRest.Click += new System.EventHandler(this.BRest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 438);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(520, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 18;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.AutoToolTip = true;
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.IndianRed;
            this.toolStripStatusLabel1.Margin = new System.Windows.Forms.Padding(20, 0, 0, 5);
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(22, 17);
            this.toolStripStatusLabel1.Text = "---";
            // 
            // BClose
            // 
            this.BClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BClose.BackColor = System.Drawing.Color.Orange;
            this.BClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BClose.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BClose.Image = global::Folder_Icon_Changer.Properties.Resources.Close32;
            this.BClose.Location = new System.Drawing.Point(438, 369);
            this.BClose.Name = "BClose";
            this.BClose.Size = new System.Drawing.Size(70, 60);
            this.BClose.TabIndex = 19;
            this.BClose.Text = "Close";
            this.BClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BClose.UseVisualStyleBackColor = false;
            this.BClose.Click += new System.EventHandler(this.BClose_Click);
            // 
            // BApply
            // 
            this.BApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BApply.BackColor = System.Drawing.Color.Orange;
            this.BApply.Enabled = false;
            this.BApply.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.BApply.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.BApply.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.BApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BApply.Image = global::Folder_Icon_Changer.Properties.Resources.Check32p;
            this.BApply.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BApply.Location = new System.Drawing.Point(282, 369);
            this.BApply.Name = "BApply";
            this.BApply.Size = new System.Drawing.Size(150, 60);
            this.BApply.TabIndex = 20;
            this.BApply.Text = "Apply";
            this.BApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.BApply.UseVisualStyleBackColor = false;
            this.BApply.Click += new System.EventHandler(this.BApply_Click);
            // 
            // bCurrentShowIconGroup
            // 
            this.bCurrentShowIconGroup.BackColor = System.Drawing.Color.Orange;
            this.bCurrentShowIconGroup.Enabled = false;
            this.bCurrentShowIconGroup.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.bCurrentShowIconGroup.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.bCurrentShowIconGroup.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.bCurrentShowIconGroup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCurrentShowIconGroup.Location = new System.Drawing.Point(197, 136);
            this.bCurrentShowIconGroup.Name = "bCurrentShowIconGroup";
            this.bCurrentShowIconGroup.Size = new System.Drawing.Size(132, 23);
            this.bCurrentShowIconGroup.TabIndex = 24;
            this.bCurrentShowIconGroup.Text = "Show icon group";
            this.bCurrentShowIconGroup.UseVisualStyleBackColor = false;
            this.bCurrentShowIconGroup.Click += new System.EventHandler(this.bCurrentShowIconGroup_Click);
            // 
            // PBCurrent
            // 
            this.PBCurrent.BackColor = System.Drawing.Color.LightSteelBlue;
            this.PBCurrent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PBCurrent.Location = new System.Drawing.Point(95, 100);
            this.PBCurrent.Name = "PBCurrent";
            this.PBCurrent.Size = new System.Drawing.Size(96, 96);
            this.PBCurrent.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PBCurrent.TabIndex = 11;
            this.PBCurrent.TabStop = false;
            this.PBCurrent.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            // 
            // bCurrentGenBestFit
            // 
            this.bCurrentGenBestFit.BackColor = System.Drawing.Color.Orange;
            this.bCurrentGenBestFit.Enabled = false;
            this.bCurrentGenBestFit.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.bCurrentGenBestFit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Goldenrod;
            this.bCurrentGenBestFit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Gold;
            this.bCurrentGenBestFit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bCurrentGenBestFit.Location = new System.Drawing.Point(197, 165);
            this.bCurrentGenBestFit.Name = "bCurrentGenBestFit";
            this.bCurrentGenBestFit.Size = new System.Drawing.Size(132, 31);
            this.bCurrentGenBestFit.TabIndex = 24;
            this.bCurrentGenBestFit.Text = "Generate Best Fit Icon";
            this.bCurrentGenBestFit.UseVisualStyleBackColor = false;
            this.bCurrentGenBestFit.Click += new System.EventHandler(this.bCurrentGenBestFit_Click);
            // 
            // bTopMost
            // 
            this.bTopMost.Appearance = System.Windows.Forms.Appearance.Button;
            this.bTopMost.AutoSize = true;
            this.bTopMost.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(93)))), ((int)(((byte)(164)))));
            this.bTopMost.CheckAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bTopMost.Checked = global::Folder_Icon_Changer.Properties.Settings.Default.TopMost;
            this.bTopMost.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::Folder_Icon_Changer.Properties.Settings.Default, "TopMost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.bTopMost.FlatAppearance.BorderSize = 0;
            this.bTopMost.FlatAppearance.CheckedBackColor = System.Drawing.Color.OrangeRed;
            this.bTopMost.FlatAppearance.MouseDownBackColor = System.Drawing.Color.OrangeRed;
            this.bTopMost.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bTopMost.Image = global::Folder_Icon_Changer.Properties.Resources.pin_1;
            this.bTopMost.Location = new System.Drawing.Point(470, 12);
            this.bTopMost.Name = "bTopMost";
            this.bTopMost.Size = new System.Drawing.Size(38, 38);
            this.bTopMost.TabIndex = 27;
            this.bTopMost.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.bTopMost.UseVisualStyleBackColor = false;
            this.bTopMost.CheckedChanged += new System.EventHandler(this.bTopMost_CheckedChanged);
            // 
            // bAbout
            // 
            this.bAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bAbout.BackColor = System.Drawing.Color.Transparent;
            this.bAbout.FlatAppearance.BorderSize = 0;
            this.bAbout.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.bAbout.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.bAbout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bAbout.Image = global::Folder_Icon_Changer.Properties.Resources.ico_alpha_Information_32x32;
            this.bAbout.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bAbout.Location = new System.Drawing.Point(136, 6);
            this.bAbout.Name = "bAbout";
            this.bAbout.Size = new System.Drawing.Size(53, 54);
            this.bAbout.TabIndex = 28;
            this.bAbout.Text = "About";
            this.bAbout.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bAbout.UseVisualStyleBackColor = false;
            this.bAbout.Click += new System.EventHandler(this.BAbout_Click);
            // 
            // bOptions
            // 
            this.bOptions.BackColor = System.Drawing.Color.Transparent;
            this.bOptions.FlatAppearance.BorderSize = 0;
            this.bOptions.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.bOptions.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.bOptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bOptions.Image = global::Folder_Icon_Changer.Properties.Resources.Settings;
            this.bOptions.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bOptions.Location = new System.Drawing.Point(77, 6);
            this.bOptions.Name = "bOptions";
            this.bOptions.Size = new System.Drawing.Size(53, 54);
            this.bOptions.TabIndex = 29;
            this.bOptions.Text = "Options";
            this.bOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bOptions.UseVisualStyleBackColor = false;
            this.bOptions.Click += new System.EventHandler(this.bOptions_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.BackColor = System.Drawing.Color.Transparent;
            this.bRefresh.FlatAppearance.BorderSize = 0;
            this.bRefresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(160)))), ((int)(((byte)(160)))));
            this.bRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(130)))), ((int)(((byte)(160)))));
            this.bRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bRefresh.Image = global::Folder_Icon_Changer.Properties.Resources.Refresh_32P;
            this.bRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.bRefresh.Location = new System.Drawing.Point(18, 6);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(53, 54);
            this.bRefresh.TabIndex = 30;
            this.bRefresh.Text = "Refresh";
            this.bRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.bRefresh.UseVisualStyleBackColor = false;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // tBTargetFolder
            // 
            this.tBTargetFolder.AllowToCalculate = false;
            this.tBTargetFolder.AllowToTogglesTheSign = false;
            this.tBTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tBTargetFolder.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tBTargetFolder.EditType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.EditTypes.Path;
            this.tBTargetFolder.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tBTargetFolder.ForeColor = System.Drawing.Color.Black;
            this.tBTargetFolder.FreeInputForCalculating = false;
            this.tBTargetFolder.Location = new System.Drawing.Point(95, 67);
            this.tBTargetFolder.MaxNumber = 7.92281625142643E+28D;
            this.tBTargetFolder.MinNumber = -7.92281625142643E+28D;
            this.tBTargetFolder.Name = "tBTargetFolder";
            this.tBTargetFolder.NumericType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.NumericTypes.AcceptAny;
            this.tBTargetFolder.Size = new System.Drawing.Size(378, 23);
            this.tBTargetFolder.TabIndex = 16;
            // 
            // nUpDownIconIndex
            // 
            this.nUpDownIconIndex.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nUpDownIconIndex.BackColor = System.Drawing.Color.LightSteelBlue;
            this.nUpDownIconIndex.ForeColor = System.Drawing.Color.Black;
            this.nUpDownIconIndex.Location = new System.Drawing.Point(322, 20);
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
            this.TBNewIcon.BackColor = System.Drawing.Color.LightSteelBlue;
            this.TBNewIcon.EditType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.EditTypes.Path;
            this.TBNewIcon.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TBNewIcon.ForeColor = System.Drawing.Color.Black;
            this.TBNewIcon.FreeInputForCalculating = false;
            this.TBNewIcon.Location = new System.Drawing.Point(62, 19);
            this.TBNewIcon.MaxNumber = 7.92281625142643E+28D;
            this.TBNewIcon.MinNumber = -7.92281625142643E+28D;
            this.TBNewIcon.Name = "TBNewIcon";
            this.TBNewIcon.NumericType = Ezz_Helper.WinForms.EditControls.TextBoxEzz.NumericTypes.AcceptAny;
            this.TBNewIcon.ReadOnly = true;
            this.TBNewIcon.Size = new System.Drawing.Size(254, 23);
            this.TBNewIcon.TabIndex = 16;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::Folder_Icon_Changer.Properties.Resources.BG314P;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(520, 460);
            this.ControlBox = false;
            this.Controls.Add(this.bRefresh);
            this.Controls.Add(this.bOptions);
            this.Controls.Add(this.bAbout);
            this.Controls.Add(this.bTopMost);
            this.Controls.Add(this.bCurrentGenBestFit);
            this.Controls.Add(this.bCurrentShowIconGroup);
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
            this.Controls.Add(this.labelTargetFolder);
            this.DataBindings.Add(new System.Windows.Forms.Binding("TopMost", global::Folder_Icon_Changer.Properties.Settings.Default, "TopMost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(520, 400);
            this.Name = "FormMain";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = global::Folder_Icon_Changer.Properties.Settings.Default.TopMost;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormMain_MouseDown);
            this.gBNewIcon.ResumeLayout(false);
            this.gBNewIcon.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBNew)).EndInit();
            this.contextMSBrowseIconFShell32.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PBCurrent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nUpDownIconIndex)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTargetFolder;
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
        internal System.Windows.Forms.Button BGetIcon;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Button bClearNewInfo;
        private System.Windows.Forms.Button bCurrentShowIconGroup;
        private System.Windows.Forms.Button bNewShowIconGroup;
        private System.Windows.Forms.ContextMenuStrip contextMSBrowseIconFShell32;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShell32;
        private System.Windows.Forms.Button bCurrentGenBestFit;
        private System.Windows.Forms.CheckBox bTopMost;
        private System.Windows.Forms.Button bAbout;
        private System.Windows.Forms.Button bOptions;
        private System.Windows.Forms.Button bRefresh;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

