using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Folder_Icon_Changer
{
    public static class Helper
    {


        public static DialogResult ShowMsgBox(this Form owner, string msg, Ezz_Helper.Managers.MultiLanguageManager mlm, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK,
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.None, MessageBoxDefaultButton messageBoxDefaultButton = MessageBoxDefaultButton.Button1, bool RtlReading = false)
        {
            bool rtl = Program.mlm.CurrentLng.IsRTL();
            return ShowMsgBox(owner, msg, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton, rtl, rtl? RtlReading : false);
        }

        public static DialogResult ShowMsgBox(this Form owner, string msg, MessageBoxButtons messageBoxButtons = MessageBoxButtons.OK,
            MessageBoxIcon messageBoxIcon = MessageBoxIcon.None, MessageBoxDefaultButton messageBoxDefaultButton = MessageBoxDefaultButton.Button1,
            bool RightAlign = false, bool RtlReading = false)
        {
            if (RtlReading || RightAlign)
            {
                MessageBoxOptions messageBoxOptions = MessageBoxOptions.DefaultDesktopOnly;
                if (RightAlign && RtlReading)
                {
                    messageBoxOptions = MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading;
                }
                else
                {
                    if (RightAlign) messageBoxOptions = MessageBoxOptions.RightAlign;
                    if (RtlReading) messageBoxOptions = MessageBoxOptions.RtlReading;
                }

                return MessageBox.Show(owner, msg, owner.Text, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton, messageBoxOptions);
            }
            else
            {
                return MessageBox.Show(owner, msg, owner.Text, messageBoxButtons, messageBoxIcon, messageBoxDefaultButton);
            }
        }

        public static bool IsRTL(this Ezz_Helper.Managers.Lng lng)
        {
            return lng.GetLngInfo_Value("RTL", "false").ToLower() != "false";
        }
    }
}
