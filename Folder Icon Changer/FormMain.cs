using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Ezz_Helper;
using Ezz_Helper.WinForms.IconsManager;
using Ezz_Helper.Drawing.IconsManager;
using Folder_Icon_Changer.Properties;
using static Ezz_Helper.OtherH;

namespace Folder_Icon_Changer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            gBNewIcon.MouseDown += FormMain_MouseDown;
        }

        Select_Icon.LangStrings Select_IconLangs;
        public void RefreshLng()
        {
            this.Text = string.Format("{0} - {1}", string.Format(Program.mlm.GetString("strings", "MainFormTitle"), Application.ProductVersion.ToString()), Application.CompanyName);
            //Buttons
            bRefresh.Text = Program.mlm.GetString("Buttons", "Refresh");
            bOptions.Text = Program.mlm.GetString("Buttons", "Options");
            bAbout.Text = Program.mlm.GetString("Buttons", "About");
            bCurrentShowIconGroup.Text = Program.mlm.GetString("Buttons", "ShowIconGroup"); bNewShowIconGroup.Text = bCurrentShowIconGroup.Text;
            bClearNewInfo.Text = Program.mlm.GetString("Buttons", "Clear");
            BGetIcon.Text = Program.mlm.GetString("Buttons", "GetIcon");
            bIconFromImage.Text = Program.mlm.GetString("Buttons", "IconFromImage");
            BRest.Text = Program.mlm.GetString("Buttons", "ResetToDefaultIcon");
            BApply.Text = Program.mlm.GetString("Buttons", "Apply");
            BClose.Text = Program.mlm.GetString("Buttons", "Close");
            bCurrentGenBestFit.Text = Program.mlm.GetString("Buttons", "GenerateBestFit");
            //Label
            labelTargetFolder.Text = Program.mlm.GetString("Label", "TargetFolder");
            LabelCurrentIcon.Text = Program.mlm.GetString("Label", "CurrentIcon");
            gBNewIcon.Text = Program.mlm.GetString("Label", "NewIconInfo");
            CBCopyIconToFolder.Text = Program.mlm.GetString("Label", "CopyIconToFolder");
            CBHideFile.Text = Program.mlm.GetString("Label", "HideIcon");
            //
            Select_IconLangs = new Select_Icon.LangStrings
            {
                FormText = Program.mlm.GetString("Select_Icon", "FormText"),
                Abort = Program.mlm.GetString("Select_Icon", "Abort"),
                AllSupportedFormats = Program.mlm.GetString("Select_Icon", "AllSupportedFormats"),
                Back = Program.mlm.GetString("Select_Icon", "Back"),
                Cancel = Program.mlm.GetString("Select_Icon", "Cancel"),
                Count = Program.mlm.GetString("Select_Icon", "Count"),
                DrawText = Program.mlm.GetString("Select_Icon", "DrawText"),
                GetIcons = Program.mlm.GetString("Select_Icon", "GetIcons"),
                Index = Program.mlm.GetString("Select_Icon", "Index"),
                Loading = Program.mlm.GetString("Select_Icon", "Loading"),
                NoIconToShow = Program.mlm.GetString("Select_Icon", "NoIconToShow"),
                OK = Program.mlm.GetString("Select_Icon", "OK"),
                SelectedCount = Program.mlm.GetString("Select_Icon", "SelectedCount"),
                StretchedImage = Program.mlm.GetString("Select_Icon", "StretchedImage"),
                StretchedSmallImagesToo = Program.mlm.GetString("Select_Icon", "StretchedSmallImagesToo"),
                Refresh = Program.mlm.GetString("Select_Icon", "Refresh"),
                Select = Program.mlm.GetString("Select_Icon", "Select"),
                ShowIconGroup = Program.mlm.GetString("Select_Icon", "ShowIconGroup"),
                View = Program.mlm.GetString("Select_Icon", "View"),
                SaveAs = Program.mlm.GetString("Select_Icon", "SaveAs"),
                ExportIcon = Program.mlm.GetString("Select_Icon", "ExportIcon"),
                ExportImage = Program.mlm.GetString("Select_Icon", "ExportImage")
            };
        }

        #region Helper methods

        private string GetNewIconName(string IcFile)
        {
            return GetNewIconName(IcFile, 0);
        }
        private string GetNewIconName(string IcFile, int Index_)
        {
            if (Path.GetExtension(IcFile).EndsWith("ico", StringComparison.CurrentCultureIgnoreCase))
            {
                return Path.GetFileName(IcFile);
            }
            else
            {
                return Path.GetFileNameWithoutExtension(IcFile) + " #" + Index_.ToString() + ".ico";
            }
            //return "icon.ico";
        }

        //
        private void CheckNSetAllowToApply()
        {
            if (newIconInfo == null)
                BApply.Enabled = false;
            else
                BApply.Enabled = Directory.Exists(tBTargetFolder.Text) && !string.Equals(Path.GetPathRoot(tBTargetFolder.Text), tBTargetFolder.Text);
        }

        private void RestCurrentInfo()
        {
            TBCurrentIcon.Text = "";
            TBCurrentIconIndex.Text = "";
            CurrentIconInfo = null;
            if (PBCurrent.Image != null) PBCurrent.Image.Dispose();
            PBCurrent.Image = null;
        }

        private bool NeedGenBestFit()
        {
            if (CurrentIconInfo == null) return false;
            if (!CurrentIconInfo.ContainsAllIcons(BestFitIconsInfo())) return true;
            return false;
        }
        private void ctrsCurrentIEnabled(bool enabled)
        {
            bRefresh.Enabled = enabled;
            tBTargetFolder.Enabled = enabled;
            BBrowseFolder.Enabled = enabled;
            if (enabled)
            {
                CheckNSetAllowToApply();
                bCurrentShowIconGroup.Enabled = CurrentIconInfo != null;
                bCurrentGenBestFit.Enabled = NeedGenBestFit();
            }
            else
            {
                BApply.Enabled = false;
                bCurrentShowIconGroup.Enabled = false;
                bCurrentGenBestFit.Enabled = false;
            }
        }
        private IconInfo CurrentIconInfo;
        private void RefreshCurrentInfo()
        {
            this.UseWaitCursor = true;
            ctrsCurrentIEnabled(false);
            if (!Directory.Exists(tBTargetFolder.Text))
            {
                RestCurrentInfo();
                ctrsCurrentIEnabled(true);
                toolStripStatusLabel1.Text = "---";
                this.UseWaitCursor = false;
                return;
            }
            Select_Icon.SelectedIconInfo FIInfo = null;
            try
            {
                FIInfo = Ezz_Helper.Files.GetInfo.GetDirectoryInfo.GetFolderIconInfo(tBTargetFolder.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (FIInfo == null)
            {
                RestCurrentInfo();
                ctrsCurrentIEnabled(true);
                toolStripStatusLabel1.Text = "---";
                this.UseWaitCursor = false;
                return;
            }
            if (FIInfo.SourceIcon == null)
            {
                if (CurrentIconInfo != null) CurrentIconInfo.Dispose();
                CurrentIconInfo = null;
            }
            else
            {
                CurrentIconInfo = new IconInfo(FIInfo.SourceIcon);
            }
            TBCurrentIcon.Text = GetIconFileShortPathIfInFolder(tBTargetFolder.Text, FIInfo.FilePath);
            TBCurrentIconIndex.Text = FIInfo.Index.ToString();
            if (CurrentIconInfo == null)
            {
                PBCurrent.Image = null;
            }
            else
            {
                PBCurrent.Image = CurrentIconInfo.GetBestFitIcon(new Size(256, 256)).Image;
            }


            BRest.Enabled = CurrentIconInfo != null;
            //bCurrentShowIconGroup.Enabled = BRest.Enabled;
            ctrsCurrentIEnabled(true);
            toolStripStatusLabel1.Text = "---";
            this.UseWaitCursor = false;
            this.Refresh();
        }

        private void ctrsNewIEnabled(bool enabled)
        {
            bClearNewInfo.Enabled = enabled;
            nUpDownIconIndex.Enabled = enabled;
            BGetIcon.Enabled = enabled;
            BBrowseIcon.Enabled = enabled;
            bIconFromImage.Enabled = enabled;
            bNewShowIconGroup.Enabled = enabled ? newIconInfo != null : false;
        }
        private IconInfo newIconInfo;
        private void GetNewIconInfo(string FilePath, int iconIndex)
        {
            this.Cursor = Cursors.WaitCursor;
            ctrsNewIEnabled(false);
            //FilePath = GetIconFileFullPathIfInFolder(tBTargetFolder.Text, FilePath);
            GetNewIconInfo(Select_Icon.DirectSelectIconFromFile(GetIconFileFullPathIfInFolder(tBTargetFolder.Text, FilePath), iconIndex));
            //
            //if (!File.Exists(FilePath))
            //{
            //    GetNewIconInfo(null);
            //    return;
            //}
            //var ExIcon = IconExtractor.ExtractIcon(FilePath, iconIndex);
            //if (ExIcon == null)
            //{
            //    //ToDo : Can not find an icon by the index in the file
            //    GetNewIconInfo(null);
            //    return;
            //}
            //Select_Icon.SelectedIconInfo SNewIConInfo = new Select_Icon.SelectedIconInfo();
            //SNewIConInfo.FilePath = FilePath;
            //SNewIConInfo.SourceIcon = ExIcon;
            //SNewIConInfo.Index = iconIndex;
            //SNewIConInfo.iCount = IconExtractor.GetIconsCount(FilePath);
            //GetNewIconInfo(SNewIConInfo);
        }
        private void GetNewIconInfo(Select_Icon.SelectedIconInfo SNewIConInfo)
        {
            if (nUpDownIconIndex.Enabled)
            {
                ctrsNewIEnabled(false);
                this.Cursor = Cursors.WaitCursor;
            }
            //bNewShowIconGroup.Enabled = false;
            if (SNewIConInfo == null)
            {
                TBNewIcon.Clear();
                //TBNewIcon.Text = string.Empty;
                nUpDownIconIndex.Maximum = 0;
                nUpDownIconIndex.Value = 0;
                if (newIconInfo != null) newIconInfo.Dispose();
                newIconInfo = null;
                PBNew.Image = null;
                BApply.Enabled = false;
                //
                ctrsNewIEnabled(true);
                toolStripStatusLabel1.Text = "---";
                this.Cursor = Cursors.Default;
                return;
            }
            //
            TBNewIcon.Text = SNewIConInfo.FilePath;
            int IIndex = SNewIConInfo.Index == 0 ? 0 : SNewIConInfo.FilePath.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ? 0 : SNewIConInfo.Index;

            if (SNewIConInfo.SourceIcon == null)
            {
                if (newIconInfo != null) newIconInfo.Dispose();
                newIconInfo = null;
                nUpDownIconIndex.Maximum = 0;
                nUpDownIconIndex.Value = 0;
                PBNew.Image = null;
                BApply.Enabled = false;
            }
            else
            {
                newIconInfo = new IconInfo(SNewIConInfo.SourceIcon);
                newIconInfo.Index = IIndex;
                nUpDownIconIndex.Maximum = SNewIConInfo.iCount == 0 ? 0 : SNewIConInfo.iCount - 1;
                nUpDownIconIndex.Value = IIndex;
                PBNew.Image = newIconInfo.GetBestFitIcon(new Size(256, 256)).Image;
                //BApply.Enabled = Directory.Exists(tBTargetFolder.Text);
                CheckNSetAllowToApply();
            }
            //bNewShowIconGroup.Enabled = newIconInfo!=null;

            ctrsNewIEnabled(true);
            toolStripStatusLabel1.Text = "---";
            this.Cursor = Cursors.Default;
        }

        private void BrowseIcon()
        {
            BrowseIcon(string.IsNullOrEmpty(TBNewIcon.Text) ? tBTargetFolder.Text : TBNewIcon.Text, (int)nUpDownIconIndex.Value);
        }
        private void BrowseIcon(string DefTarget, int defindex = 0)
        {
            var SICon = Select_Icon.ShowD(this, DefTarget, defindex, string.IsNullOrEmpty(DefTarget) ? true : string.IsNullOrEmpty(TBNewIcon.Text), lang: Select_IconLangs);
            if (SICon.DialogResult == DialogResult.OK)
            {
                GetNewIconInfo(SICon.GetFirstItem());
            }
        }
        private void bIconFromImage_Click(object sender, EventArgs e)
        { IconFromImage(); }
        private void IconFromImage()
        {
            var fd = new OpenFileDialog();
            fd.Multiselect = false;
            fd.BuildFilter(new string[] { "*.jpg", "*.Jpeg", "*.png", "*.bmp" });
            fd.Title = Program.mlm.GetString("strings", "SelectAnyImageToBeConverted");
            if (fd.ShowDialog(this) == DialogResult.OK)
            {
                IconFromImage(fd.FileName);
            }
        }
        private void IconFromImage(string SourceImageFile)
        {
            if (!File.Exists(SourceImageFile))
            {
                MessageBox.Show(Program.mlm.GetString("strings", "FileNotExists"));
                return;
            }
            var SaveDefDir = Directory.Exists(tBTargetFolder.Text) ? tBTargetFolder.Text : Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var SaveFD = new SaveFileDialog();
            SaveFD.BuildFilter(new string[] { "*.ico" });
            SaveFD.AddExtension = true;
            SaveFD.DefaultExt = ".ico";
            SaveFD.InitialDirectory = SaveDefDir;
            //generatedsad 
            SaveFD.Title = Program.mlm.GetString("strings", "SelectToSaveNewGeneratedIcon");
            SaveFD.FileName = Program.mlm.GetString("strings", "NewIcon") + ".ico";
            SaveFD.OverwritePrompt = true;
            if (SaveFD.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    var SourceImage = new Bitmap(SourceImageFile);
                    string iconFile = "";
                    //using (var IconEd = new IconEditor(SourceImage, new OneIconInfo(Sizes.px_256x256, ImageColorsTypes.Alpha_Channel),
                    //    new OneIconInfo(Sizes.px_128x128, ImageColorsTypes.Alpha_Channel),
                    //    new OneIconInfo(Sizes.px_64x64, ImageColorsTypes.Alpha_Channel),
                    //    new OneIconInfo(Sizes.px_48x48, ImageColorsTypes.Alpha_Channel),
                    //    new OneIconInfo(Sizes.px_32x32, ImageColorsTypes.Alpha_Channel),
                    //    new OneIconInfo(Sizes.px_16x16, ImageColorsTypes.Alpha_Channel)))
                    //{
                    //    var res = IconEd.SaveTo(SaveFD.FileName, SameFileNameDecisions.Overwrite);
                    //    iconFile = res.FilePath;
                    //}
                    using (var IconEd = new IconEditor(SourceImage, GetOneIconInfoArry()))
                    {
                        var res = IconEd.SaveTo(SaveFD.FileName, SameFileNameDecisions.Overwrite);
                        iconFile = res.FilePath;
                    }
                    SourceImage.Dispose();
                    //
                    //BrowseIcon(iconFile);
                    GetNewIconInfo(iconFile, 0);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    //throw;
                }
            }
        }
        private OneIconInfo[] OneIconInfoArry;
        private OneIconInfo[] GetOneIconInfoArry()
        {
            if (OneIconInfoArry != null) return OneIconInfoArry;
            OneIconInfoArry = new OneIconInfo[] {new OneIconInfo(Sizes.px_256x256, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_128x128, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_64x64, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes._256_IndexedColors),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes._16_IndexedColors),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes._16_IndexedColors) };
            return OneIconInfoArry;
        }
        private OneIconInfo[] BestFitIconsInfo()
        {
            return new OneIconInfo[] {new OneIconInfo(Sizes.px_256x256, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_128x128, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_64x64, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_48x48, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_32x32, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_24x24, ImageColorsTypes.Alpha_Channel),
                        new OneIconInfo(Sizes.px_16x16, ImageColorsTypes.Alpha_Channel)};
        }

        private string GetIconFileFullPathIfInFolder(string Folder_, string iconFP)
        {
            if (string.IsNullOrEmpty(iconFP))
                return "";
            string FPath = iconFP;
            //.Trim
            try
            {
                if (Path.IsPathRooted(FPath))
                    return FPath;
                FPath = Path.Combine(Folder_, iconFP);
                if (!File.Exists(FPath))
                    return iconFP.Trim();
            }
            catch (Exception)
            {
                return iconFP.Trim();
            }
            return FPath.Trim();
        }
        private string GetIconFileShortPathIfInFolder(string Folder_, string iconFP)
        {
            try
            {
                if (!Path.IsPathRooted(iconFP))
                    return iconFP.Trim();
                if (Path.GetDirectoryName(iconFP).Equals(Folder_, StringComparison.CurrentCultureIgnoreCase))
                {
                    return Path.GetFileName(iconFP);
                }
                return iconFP.Trim();
            }
            catch (Exception)
            {
                return iconFP.Trim();
            }
        }

        #endregion

        private void BBrowseFolder_Click(object sender, EventArgs e)
        {
            var OFD = new FolderBrowserDialog();
            OFD.ShowNewFolderButton = true;
            OFD.SelectedPath = tBTargetFolder.Text;
            //OFD.Description =""
            if (OFD.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                //FillingFolderPath = true;
                tBTargetFolder.Text = OFD.SelectedPath;
                RefreshCurrentInfo();
                //RefreshCurrentInfo(CBOpenChangeIcon.Enabled & CBOpenChangeIcon.Checked);
                //FillingFolderPath = false;
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                ///"*.jpg", "*.Jpeg", "*.png", "*.bmp"
                var DFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                var DDD = DFiles[0];
                if (Directory.Exists(DDD) || DDD.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("exe", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("Jpeg", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("png", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("bmp", StringComparison.CurrentCultureIgnoreCase))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }
        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop)) return;
            var DFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
            var DDD = DFiles[0];
            if (Directory.Exists(DDD))
            {
                tBTargetFolder.Text = DDD;
                RefreshCurrentInfo();
            }
            else if (DDD.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ||
                DDD.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase)
                || DDD.EndsWith("exe", StringComparison.CurrentCultureIgnoreCase))
            {
                var SICon = Ezz_Helper.WinForms.IconsManager.Select_Icon.ShowD(this, DDD, 0, lang: Select_IconLangs);
                if (SICon.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GetNewIconInfo(SICon.GetFirstItem());
                }
            }
            else if (DDD.EndsWith("jpg", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("Jpeg", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("png", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("bmp", StringComparison.CurrentCultureIgnoreCase))
            {
                IconFromImage(DDD);
            }
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshCurrentInfo();
        }

        //private Select_Icon.SelectedIconInfo SNewIConInfo;
        private void BGetNew_Click(object sender, EventArgs e)
        {
            GetNewIconInfo(TBNewIcon.Text, (int)nUpDownIconIndex.Value);
        }
        private void bClearNewInfo_Click(object sender, EventArgs e)
        {
            GetNewIconInfo(null);
        }

        private void BBrowseIcon_Click(object sender, EventArgs e)
        {
            BrowseIcon();
        }
        private void toolStripMenuItemShell32_Click(object sender, EventArgs e)
        {
            BrowseIcon("");
        }

        private void BApply_Click(object sender, EventArgs e)
        {
            if (!Directory.Exists(tBTargetFolder.Text)) return;
            Exception err = null;

            Ezz_Helper.Files.GetInfo.GetDirectoryInfo.SetFolderCustomIcon(tBTargetFolder.Text, TBNewIcon.Text, (int)nUpDownIconIndex.Value,
                CopyTheIconToTargetFolder: CBCopyIconToFolder.Checked,
                HideFileIfitInFolder: CBHideFile.Checked,
                ToDoOnError: (ex) => { err = ex; });
            if (err == null)
            {
                RefreshCurrentInfo();
                GetNewIconInfo(null);
                toolStripStatusLabel1.Text = Program.mlm.GetString("strings", "Done");
            }
            else
            {
                toolStripStatusLabel1.Text = Program.mlm.GetString("strings", "ThereIsAnError");
                MessageBox.Show(err.Message);
            }
            //
        }

        private void BClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BRest_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(tBTargetFolder.Text))
            {
                Ezz_Helper.Files.GetInfo.GetDirectoryInfo.RestFolderIcon(tBTargetFolder.Text);
                RefreshCurrentInfo();
            }
        }

        private void FormMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                this.MoveFormByMouseDown();
            }
        }

        private void bCurrentShowIconGroup_Click(object sender, EventArgs e)
        {
            ShowIconGroup(CurrentIconInfo);
        }

        private void bNewShowIconGroup_Click(object sender, EventArgs e)
        {
            ShowIconGroup(newIconInfo);
        }

        private void ShowIconGroup(IconInfo _iconInfo)
        {
            if (_iconInfo == null) return;
            IconGroup.Show_D(this, _iconInfo, new IconGroup.LangStrings { ExportImage = Program.mlm.GetString("Select_Icon", "ExportImage") });
        }

        private void BAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.Show(this);
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.ApplicationExitCall)
            {
                Settings.Default.CopyIconT = CBCopyIconToFolder.Checked;
                Settings.Default.HideTheFileA = CBHideFile.Checked;
                Settings.Default.Save();
            }
        }


        private void nUpDownIconIndex_ValueChanged(object sender, EventArgs e)
        {
            if (newIconInfo != null)
            {
                if (newIconInfo.Index != (int)nUpDownIconIndex.Value)
                {
                    toolStripStatusLabel1.Text = string.Format(Program.mlm.GetString("strings", "ClickGetIconButtonToGetTheIconFirst"), BGetIcon.Text);
                    BApply.Enabled = false;
                }
                else
                {
                    toolStripStatusLabel1.Text = "---";
                    CheckNSetAllowToApply();
                }
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            RefreshLng();
        }

        private void bOptions_Click(object sender, EventArgs e)
        {
            var formOp = new FormOptions();
            if (formOp.ShowDialog(this) == DialogResult.OK)
            {
                this.RefreshLng();
            }
        }

        bool _genBestFit;
        private void bCurrentGenBestFit_Click(object sender, EventArgs e)
        {
            if (_genBestFit) return;
            var msgGenBestFit = Program.mlm.GetString("strings", "msgGenBestFit");
            if (this.ShowMsgBox(msgGenBestFit, Program.mlm, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
            // if (Program.mlm.CurrentLng.GetLngInfo_Value("RTL", "false").ToLower() == "false")Program.mlm.CurrentLng.GetLngInfo_Value("RTL", "false").ToLower() == "false"
            //{
            //    if (MessageBox.Show(this, msgGenBestFit, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) != DialogResult.Yes) return;
            //}
            //else
            //{
            //    if (MessageBox.Show(this, msgGenBestFit, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.RightAlign) != DialogResult.Yes) return;
            //}
            _genBestFit = true;
            RefreshCurrentInfo();
            bool canGen = bCurrentGenBestFit.Enabled;
            ctrsNewIEnabled(false);
            try
            {
                toolStripStatusLabel1.Text = Program.mlm.GetString("strings", "Working"); statusStrip1.Update();
                if (!canGen)
                {
                    _genBestFit = false;
                    RefreshCurrentInfo();
                    return;
                }
                var filePath = Path.Combine(tBTargetFolder.Text, "FolderIcon.ico");
                //
                var bestFitImage = CurrentIconInfo.GetBestFitIcon();
                var SourceImage = new Bitmap(bestFitImage.Image);
                using (var IconEd = new IconEditor(SourceImage, GetOneIconInfoArry()))
                {
                    var res = IconEd.SaveTo(filePath, SameFileNameDecisions.Rename);
                    filePath = res.FilePath;
                }
                CurrentIconInfo.Dispose();
                SourceImage.Dispose();
                GetNewIconInfo(filePath, 0);
                BApply_Click(sender, e);
                //RefreshCurrentInfo();
                //toolStripStatusLabel1.Text = Program.mlm.GetString("strings", "Done"); statusStrip1.Update();
            }
            catch (Exception ex)
            {
                RefreshCurrentInfo();
                toolStripStatusLabel1.Text = Program.mlm.GetString("strings", "ThereIsAnError"); statusStrip1.Update();
                MessageBox.Show(ex.ToString());
                //throw;
            }
            //ctrsNewIEnabled(true);
            _genBestFit = false;
        }

        
    }
}
