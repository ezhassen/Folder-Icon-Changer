using System;
using System.IO;
using System.Windows.Forms;
using Ezz_Helper;
using Ezz_Helper.Managers;
using UpdatingPreloaderEzz;
using System.Linq;
using System.Security;
using System.Security.Permissions;

namespace Folder_Icon_Changer
{
    static class Program
    {
        public const string LngVersion = "1.0.2";
        public static UpdatingPreloader Preloader;
        public static MultiLanguageManager mlm;
        private static bool _cDefaultLng;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(true);
            if (ApplicationSingleInstance.IsAlreadyRunning())
            {
                ApplicationSingleInstance.SwitchToCurrentInstance();
            }
            else
            {
                Preloader = new UpdatingPreloader();
                Preloader.Icon = Properties.Resources.icon;
                Preloader.ShowInTaskbar = false;
                Preloader.ShowIcon = false;
                Preloader.Refresh();
                //
                //var sync = SynchronizationContext.Current;

                mlm = new MultiLanguageManager(GetLangsFolder());
                mlm.CheckFolderNCreateIt();
                _cDefaultLng = cDefaultLng();
                if (_cDefaultLng)
                {
                    Preloader.Load += (sender, e) =>
                    {
                        mlm.DefaultLng.xmlEzz.Save(OnFinish_1: () => Arabic().xmlEzz.Save(OnFinish_1: () =>
                        {
                            mlm.ChangeCurrentLng(Properties.Settings.Default.lng, false, English);
                            ShowMainForm();
                        }));
                    };
                    Application.Run(Preloader);
                }
                else
                {
                    Preloader.Load += (sender, e) =>
                    {
                        mlm.ReloadLngsInfo();
                        var def = mlm.LngsInfo.FirstOrDefault((li) => li.FileName.Equals("english.xml", StringComparison.CurrentCultureIgnoreCase));
                        if (def == null || !LngVersion.Equals(def.LGroup.GetValue("LngVersion")))
                        {
                            var defLang = English();
                            defLang.xmlEzz.Save(OnFinish_1: () => Arabic().xmlEzz.Save(OnFinish_1: () =>
                            {
                                if (defLang.xmlEzz.Save_ErrorInLastOperation != null) { }
                                _LoadNShowMainForm();
                            }));
                        }
                        else
                        {
                            _LoadNShowMainForm();
                        }
                    };
                    Application.Run(Preloader);
                }
                SaveCurrentLng();
            }
        }
        private static void _LoadNShowMainForm()
        {
            var lastlngIsGood = mlm.ChangeCurrentLng(Properties.Settings.Default.lng, false, English);
            if (lastlngIsGood)
            {
                mlm.CurrentLng.xmlEzz.Load(OnFinish_1: () =>
                {
                    ShowMainForm();
                });
            }
            else
            {
                ShowMainForm();
            }
        }

        private static void ShowMainForm()
        {
            var MainForm = new FormMain(); MainForm.FormClosed += (object maForm, FormClosedEventArgs formClosedEventArgs) => { Application.Exit(); };
            MainForm.Show();
            Preloader.Hide();
        }
        //
        public static string GetLangsFolder()
        {
            if (FolderHasWritePermission(Application.StartupPath))
            {
                return Path.Combine(Application.StartupPath, "langs");
            }
            else
            {
                return Path.Combine(Application.CommonAppDataPath, "langs");
            }
        }
        public static bool FolderHasWritePermission(string folder)
        {
            PermissionSet permissionSet = new PermissionSet(PermissionState.None);

            FileIOPermission writePermission = new FileIOPermission(FileIOPermissionAccess.Write, folder);

            permissionSet.AddPermission(writePermission);
            return permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet);
            //if (permissionSet.IsSubsetOf(AppDomain.CurrentDomain.PermissionSet))
            //{
            //    // You have write permissions
            //}
            //else
            //{
            //    // You don't have write permissions
            //}

        }
        //
        public static void SaveCurrentLng()
        {
            Properties.Settings.Default.lng = Path.GetFileNameWithoutExtension(mlm.CurrentLng.FileName);
            Properties.Settings.Default.Save();
        }
        private static bool cDefaultLng()
        {
            if (mlm.DefaultLngIsRequired())
            {
                mlm.DefaultLng = English();
                return true;
            }
            return false;
        }


        private static Lng English()
        {
            Lng nlng = mlm.NewLng("english");

            nlng.SetLngInfoGroup(new LString("Name", "English"), new LString("RTL", "false", "If this lng is a RightToLeft language then \"true\" else \"false\""), 
                new LString("AppVersion", Application.ProductVersion), new LString("LngVersion", LngVersion));
            //
            var stringsG = nlng.AddNewGroup("strings");
            stringsG.SetValue("MainFormTitle", "Folder Icon Changer v{0}");
            stringsG.SetValue("AppDescription", "easy-to-use & fast way to change folder's icon");
            stringsG.SetValue("SelectAnyImageToBeConverted", "Select any Image/Picture to be converted to ico format with sizes (p256, p128, p64, p48, p32, p16) and the Color is Alpha_Channel (32bit)");
            stringsG.SetValue("FileNotExists", "File Not Exists!");
            stringsG.SetValue("SelectToSaveNewGeneratedIcon", "Select where to save the new generated icon?");
            stringsG.SetValue("NewIcon", "NewIcon");
            stringsG.SetValue("Done", "Done :)");
            stringsG.SetValue("Working", "Working...");
            stringsG.SetValue("ThereIsAnError", "There is an error!");
            stringsG.SetValue("ClickGetIconButtonToGetTheIconFirst", "Click '{0}' button to get the icon first.");

            stringsG.SetValue("FormOptionsTitle", "Options");
            stringsG.SetValue("msgGenBestFit", "Will try to Generate Best Fit Icon By generating multiple icons with deferent sizes.\r\n\r\nContinue?");
            //
            var ButtonsG = nlng.AddNewGroup("Buttons");
            ButtonsG.SetValue("GetIcon", "Get icon");
            ButtonsG.SetValue("Refresh", "Refresh");
            ButtonsG.SetValue("ShowIconGroup", "Show icon group");
            ButtonsG.SetValue("IconFromImage", "Icon from image");
            ButtonsG.SetValue("Clear", "Clear");
            ButtonsG.SetValue("ResetToDefaultIcon", "Reset to default icon");
            ButtonsG.SetValue("Apply", "Apply");
            ButtonsG.SetValue("Close", "Close");
            ButtonsG.SetValue("OK", "OK");
            ButtonsG.SetValue("Cancel", "Cancel");
            ButtonsG.SetValue("GenerateBestFit", "Generate Best Icon Fit");
            ButtonsG.SetValue("Options", "Options");
            ButtonsG.SetValue("About", "About");
            ButtonsG.SetValue("TopMost", "Stay On Top");
            //
            var labelsG = nlng.AddNewGroup("Label");
            labelsG.SetValue("TargetFolder", "Target folder : ");
            labelsG.SetValue("CurrentIcon", "Current Icon : ");
            labelsG.SetValue("NewIconInfo", "New Icon Info");
            labelsG.SetValue("CopyIconToFolder", "Copy icon to the folder");
            labelsG.SetValue("HideIcon", "Hide if the icon exists in the target folder");

            //
            var Select_IconG = nlng.AddNewGroup("Select_Icon");
            Select_IconG.SetValue("FormText", "Select Icon");
            Select_IconG.SetValue("Loading", "Loading...");
            Select_IconG.SetValue("NoIconToShow", "There isn't any icon to be shown!");
            Select_IconG.SetValue("Count", "Count");
            Select_IconG.SetValue("AllSupportedFormats", "All supported formats");
            Select_IconG.SetValue("Index", "Index");
            Select_IconG.SetValue("SelectedCount", "Selected count");
            Select_IconG.SetValue("GetIcons", "Get Icons");
            Select_IconG.SetValue("Abort", "Abort");
            Select_IconG.SetValue("Back", "Back");
            Select_IconG.SetValue("OK", "OK");
            Select_IconG.SetValue("Cancel", "Cancel");
            Select_IconG.SetValue("StretchedImage", "Stretched Image");
            Select_IconG.SetValue("StretchedSmallImagesToo", "Stretch Small Images Too");
            Select_IconG.SetValue("DrawText", "Draw Text");
            Select_IconG.SetValue("Select", "Select");
            Select_IconG.SetValue("ShowIconGroup", "Show icon group");
            Select_IconG.SetValue("View", "View");
            Select_IconG.SetValue("Refresh", "&Refresh");
            Select_IconG.SetValue("SaveAs", "Save as ...");
            //
            Select_IconG.SetValue("ExportIcon", "Export Icon");
            Select_IconG.SetValue("ExportImage", "Export Image");

            return nlng;
        }

        private static Lng Arabic()
        {
            Lng nlng = mlm.NewLng("arabic");

            nlng.SetLngInfoGroup(new LString("Name", "العربية"), new LString("RTL", "true", "If this lng is a RightToLeft language then 'true' else 'false'"),
                new LString("AppVersion", Application.ProductVersion), new LString("LngVersion", LngVersion));
            //
            var stringsG = nlng.AddNewGroup("strings");
            stringsG.SetValue("MainFormTitle", "مغير أيقونة المجلد إصدار {0}");
            stringsG.SetValue("AppDescription", "لتغيير أيقونات المجلدات بسهولة وبسرعة :)");
            stringsG.SetValue("SelectAnyImageToBeConverted", "إختر الصورة لكي يتم توليد أيقونة منها بأحجام (265ب, 128ب, 64ب, 48ب, 32ب, 16ب) والألوان تكون قناة ألفا 32بيت");
            stringsG.SetValue("FileNotExists", "الملف غير موجود!");
            stringsG.SetValue("SelectToSaveNewGeneratedIcon", "حدد مكان حفظ الأيقونة الجديدة المولدة?");
            stringsG.SetValue("NewIcon", "أيقونة جديد");
            stringsG.SetValue("Done", "تم :)");
            stringsG.SetValue("Working", "جار العمل...");
            stringsG.SetValue("ThereIsAnError", "يوجد خطأ ما!");
            stringsG.SetValue("ClickGetIconButtonToGetTheIconFirst", "إضغط على زر '{0}' لتحصل على الأيقونة أولاً.");

            stringsG.SetValue("FormOptionsTitle", "خيارات");
            stringsG.SetValue("msgGenBestFit", "سيحاول توليد أفضل أيقونة مناسبة عن طريق توليد أيقونات متعددة الأحجام.\r\n\r\nاستكمال؟");
            //
            var ButtonsG = nlng.AddNewGroup("Buttons");
            ButtonsG.SetValue("GetIcon", "الحصول على أيقونة");
            ButtonsG.SetValue("Refresh", "تحديث");
            ButtonsG.SetValue("ShowIconGroup", "عرض مجموعة الأيقونات");
            ButtonsG.SetValue("IconFromImage", "أيقونة من صورة");
            ButtonsG.SetValue("Clear", "مسح");
            ButtonsG.SetValue("ResetToDefaultIcon", "إعادة تعيين الأيقونة ");
            ButtonsG.SetValue("Apply", "تطبيق");
            ButtonsG.SetValue("Close", "إغلاق");
            ButtonsG.SetValue("OK", "حسناً");
            ButtonsG.SetValue("Cancel", "إلغاء");
            ButtonsG.SetValue("GenerateBestFit", "توليد أفضل أيقونة مناسبة");
            ButtonsG.SetValue("Options", "خيارات");
            ButtonsG.SetValue("About", "عن");
            ButtonsG.SetValue("TopMost", "البقاء أعلى التطبيقات");
            //
            var labelsG = nlng.AddNewGroup("Label");
            labelsG.SetValue("TargetFolder", "المجلد الهدف : ");
            labelsG.SetValue("CurrentIcon", "الأيقونة الحالية : ");
            labelsG.SetValue("NewIconInfo", "معلومات الأيقونة الجديدة");
            labelsG.SetValue("CopyIconToFolder", "نسخ الأيقونة للمجلد");
            labelsG.SetValue("HideIcon", "إخفاء الملف إذا كان موجوداً في المجلد الهدف");

            //
            var Select_IconG = nlng.AddNewGroup("Select_Icon");
            Select_IconG.SetValue("FormText", "إختر أيقونة");
            Select_IconG.SetValue("Loading", "جار التحميل");
            Select_IconG.SetValue("NoIconToShow", "لا توجد أي أيقونة لإظهارها!");
            Select_IconG.SetValue("Count", "العدد");
            Select_IconG.SetValue("AllSupportedFormats", "كل الصيغ المدعومة");
            Select_IconG.SetValue("Index", "الترتيب");
            Select_IconG.SetValue("SelectedCount", "عدد المحدد");
            Select_IconG.SetValue("GetIcons", "الحصول على الأيقونات");
            Select_IconG.SetValue("Abort", "إحباط");
            Select_IconG.SetValue("Back", "عودة");
            Select_IconG.SetValue("OK", "حسناً");
            Select_IconG.SetValue("Cancel", "إلغاء");
            Select_IconG.SetValue("StretchedImage", "صورة متمددة");
            Select_IconG.SetValue("StretchedSmallImagesToo", "مد الصور الصغيرة أيضاً");
            Select_IconG.SetValue("DrawText", "كتابة النص");
            Select_IconG.SetValue("Select", "إختيار");
            Select_IconG.SetValue("ShowIconGroup", "عرض مجموعة الأيقونات");
            Select_IconG.SetValue("View", "العرض");
            Select_IconG.SetValue("Refresh", "&تحديث");
            Select_IconG.SetValue("SaveAs", "حفظ كـ ...");
            Select_IconG.SetValue("ExportIcon", "تصدير الأيقون");
            Select_IconG.SetValue("ExportImage", "تصدير الصورة");

            return nlng;
        }
        //public static string GetString(string groupName, string Key)
        //{
        //    return mlm.GetString(groupName, Key);
        //}
    }
}
