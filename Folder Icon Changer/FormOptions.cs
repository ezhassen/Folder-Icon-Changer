using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Ezz_Helper.Managers;
using Ezz_Helper;

namespace Folder_Icon_Changer
{
    public partial class FormOptions : Form
    {
        public FormOptions()
        {
            InitializeComponent();
            gbLanguage.MouseDown += FormOptions_MouseDown;
            gbOtherOptions.MouseDown += FormOptions_MouseDown;
        }
        public void RefreshLng()
        {
            this.Text = Program.mlm.GetString("strings", "FormOptionsTitle");
            bOK.Text = Program.mlm.GetString("Buttons", "OK");
            bCancel.Text = Program.mlm.GetString("Buttons", "Cancel");
            bRefresh.Text = Program.mlm.GetString("Buttons", "Refresh");
            cbShowCurrentFolderForIconFromImage.Text = Program.mlm.GetString("strings", "ShowCurrentFolderForIconFromImage");
            gbOtherOptions.Text = Program.mlm.GetString("Label", "OtherOptions");
            bool rtl = Program.mlm.CurrentLng?.GetLngInfo_Value("RTL", "false").ToLower() == "true";
            gbOtherOptions.RightToLeft = rtl ? RightToLeft.Yes : RightToLeft.Inherit;
        }

        private void FormOptions_Load(object sender, EventArgs e)
        {
            RefreshLng();
            RefreshlngList();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = !_ctrlsEnabled;
            base.OnFormClosing(e);
        }

        bool _ctrlsEnabled = true;
        private void ctrlsEnabled(bool _vale)
        {
            gbLanguage.Enabled = _vale;
            bOK.Enabled = _vale;
            bCancel.Enabled = _vale;
            _ctrlsEnabled = _vale;
        }
        private void RefreshlngList()
        {
            ctrlsEnabled(false);
            this.UseWaitCursor = true;
            //
            var selectedLng = cbLangs.SelectedItem as lngInfo_2;
            cbLangs.Items.Clear();
            Program.mlm.ReloadLngsInfo();
            foreach (var li in Program.mlm.LngsInfo)
            {
                cbLangs.Items.Add(new lngInfo_2(li));
            }
            bool selectCurrentlng = true;
            if (selectedLng != null)
            {
                var item = cbLangs.Items.OfType<lngInfo_2>().FirstOrDefault(li => li.lngInfo.FileName.ToLower() == selectedLng.lngInfo.FileName.ToLower());
                if (item != null)
                {
                    selectCurrentlng = false;
                    cbLangs.SelectedItem = item;
                }
            }
            if (selectCurrentlng)
            {
                var citem = cbLangs.Items.OfType<lngInfo_2>().FirstOrDefault(li => li.lngInfo.FileName.ToLower() == Program.mlm.CurrentLng.FileName.ToLower());
                if (citem != null)
                {
                    cbLangs.SelectedItem = citem;
                }
            }
            //
            ctrlsEnabled(true);
            this.UseWaitCursor = false;
        }

        public class lngInfo_2
        {

            public lngInfo_2(lngInfo lngI)
            {
                lngInfo = lngI;
            }

            public lngInfo lngInfo { get; private set; }

            public override string ToString()
            {
                return lngInfo.LGroup.GetValue("Name", Path.GetFileNameWithoutExtension(lngInfo.FileName));
                //return base.ToString();
            }
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            var selectedLng = cbLangs.SelectedItem as lngInfo_2;
            if (selectedLng == null)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }
            ctrlsEnabled(false);
            Program.Preloader.Show();
            Program.mlm.ChangeCurrentLng(selectedLng.lngInfo);
            Program.mlm.CurrentLng.xmlEzz.Load(OnFinish_1: () =>
            {
                Program.SaveCurrentLng();
                ctrlsEnabled(true);
                Program.Preloader.Hide();
                DialogResult = DialogResult.OK;
            });
        }

        private void FormOptions_MouseDown(object sender, MouseEventArgs e)
        {
            this.MoveFormByMouseDown();
        }

        private void bRefresh_Click(object sender, EventArgs e)
        {
            RefreshlngList();
        }
    }
}
