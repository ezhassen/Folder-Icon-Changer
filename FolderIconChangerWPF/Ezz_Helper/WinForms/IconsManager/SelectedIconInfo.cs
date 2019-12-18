using Ezz_Helper.Drawing.IconsManager;
using System.Drawing;

namespace Ezz_Helper.WinForms.IconsManager
{
	public class SelectedIconInfo
    {
		public SelectedIconInfo()
		{
		}

		public bool? DialogResult { get; set; }

		//
		public IconViewTypes IconViewType { get; set; }

		public Icon SourceIcon { get; set; }

		public IconInfo.IconImageInfo SelectedIconImageInfo { get; set; }

		/// <summary>
		/// Icon Index in the file
		/// </summary>
		public int Index { get; set; }

		public string FilePath { get; set; }

		public int ICount { get; set; }

		//
		public static SelectedIconInfo InitializeNew() => new SelectedIconInfo();
	}
}
