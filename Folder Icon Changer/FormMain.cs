using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using Ezz_Helper;
using Ezz_Helper.WinForms.IconsManager;
using Ezz_Helper.Drawing.IconsManager;
using Folder_Icon_Changer.Properties;

namespace Folder_Icon_Changer
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            gBNewIcon.MouseDown += FormMain_MouseDown;

            this.Text = string.Format("Folder Icon Changer v{0} By ezhassen", Application.ProductVersion.ToString());
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

        private void ctrsCurrentIEnabled(bool enabled)
        {
            bRefresh.Enabled = enabled;
            tBTargetFolder.Enabled = enabled;
            BBrowseFolder.Enabled = enabled;
            if (enabled)
            {
                CheckNSetAllowToApply();
                bCurrentShowIconGroup.Enabled = CurrentIconInfo != null;
            }
            else
            {
                BApply.Enabled = false;
                bCurrentShowIconGroup.Enabled = false;
            }
        }
        private IconInfo CurrentIconInfo;
        private void RefreshCurrentInfo()
        {
            this.Cursor = Cursors.WaitCursor;
            ctrsCurrentIEnabled(false);
            if (!Directory.Exists(tBTargetFolder.Text))
            {
                RestCurrentInfo();
                ctrsCurrentIEnabled(true);
                toolStripStatusLabel1.Text = "---";
                this.Cursor = Cursors.Default;
                return;
            }
            Ezz_Helper.WinForms.IconsManager.Select_Icon.SelectedIconInfo FIInfo = null;
            try
            {
                FIInfo = Ezz_Helper.Files.GetInfo.GetDirectoryInfo.GetFolderIconInfo(tBTargetFolder.Text);
            }
            catch (Exception ex){
                MessageBox.Show(ex.ToString());
            }

            if (FIInfo == null)
            {
                RestCurrentInfo();
                ctrsCurrentIEnabled(true);
                toolStripStatusLabel1.Text = "---";
                this.Cursor = Cursors.Default;
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
            this.Cursor = Cursors.Default;
            this.Refresh();
        }

        private void ctrsNewIEnabled(bool enabled)
        {
            bClearNewInfo.Enabled = enabled;
            nUpDownIconIndex.Enabled = enabled;
            BGetNew.Enabled = enabled;
            BBrowseIcon.Enabled = enabled;
            bIconFromImage.Enabled = enabled;
            bNewShowIconGroup.Enabled = enabled ? newIconInfo != null : false;
        }
        private IconInfo newIconInfo;
        private void GetNewIconInfo(string FilePath, int iconIndex)
        {
            this.Cursor = Cursors.WaitCursor;
            ctrsNewIEnabled(false);
            FilePath = GetIconFileFullPathIfInFolder(tBTargetFolder.Text, FilePath);
            if (!File.Exists(FilePath))
            {
                GetNewIconInfo(null);
                return;
            }
            var ExIcon = IconExtractor.ExtractIcon(FilePath, iconIndex);
            if (ExIcon == null)
            {
                //ToDo : Can not find an icon by the index in the file
                GetNewIconInfo(null);
                return;
            }
            Select_Icon.SelectedIconInfo SNewIConInfo = new Select_Icon.SelectedIconInfo();
            SNewIConInfo.FilePath = FilePath;
            SNewIConInfo.SourceIcon = ExIcon;
            SNewIConInfo.Index = iconIndex;
            SNewIConInfo.iCount = IconExtractor.GetIconsCount(FilePath);
            GetNewIconInfo(SNewIConInfo);
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
            var SICon = Ezz_Helper.WinForms.IconsManager.Select_Icon.ShowD(this, DefTarget, defindex, string.IsNullOrEmpty(DefTarget) ? true : string.IsNullOrEmpty(TBNewIcon.Text));
            if (SICon.DialogResult == System.Windows.Forms.DialogResult.OK)
            {
                GetNewIconInfo(SICon.GetFirstItem());
            }
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
                var DFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                var DDD = DFiles[0];
                if (Directory.Exists(DDD) || DDD.EndsWith("ico", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("dll", StringComparison.CurrentCultureIgnoreCase) ||
                    DDD.EndsWith("exe", StringComparison.CurrentCultureIgnoreCase))
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
                var SICon = Ezz_Helper.WinForms.IconsManager.Select_Icon.ShowD(this, DDD, 0);
                if (SICon.DialogResult == System.Windows.Forms.DialogResult.OK)
                {
                    GetNewIconInfo(SICon.GetFirstItem());
                }
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
                toolStripStatusLabel1.Text = "Done :)";
            }
            else
            {
                toolStripStatusLabel1.Text = "There is an error!";
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
            IconGroup.Show_D(this, CurrentIconInfo);
        }

        private void bNewShowIconGroup_Click(object sender, EventArgs e)
        {
            IconGroup.Show_D(this, newIconInfo);
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
                    toolStripStatusLabel1.Text = "Click 'Get icon' button to get the icon first.";
                    BApply.Enabled = false;
                }
                else
                {
                    toolStripStatusLabel1.Text = "---";
                    CheckNSetAllowToApply();
                }
            }
        }

      




        //
        //[DllImport("user32")]
        //public static extern UInt32 SendMessage
        //    (IntPtr hWnd, UInt32 msg, UInt32 wParam, UInt32 lParam);

        //internal const int BCM_FIRST = 0x1600; //Normal button
        //internal const int BCM_SETSHIELD = (BCM_FIRST + 0x000C); //Elevated button
        //public static bool IsAdministrator()
        //{
        //    return (new WindowsPrincipal(WindowsIdentity.GetCurrent()))
        //            .IsInRole(WindowsBuiltInRole.Administrator);
        //}
        //static internal void AddShieldToButton(Button b)
        //{
        //    b.FlatStyle = FlatStyle.System;
        //    SendMessage(b.Handle, BCM_SETSHIELD, 0, 0xFFFFFFFF);
        //}
    }
}
