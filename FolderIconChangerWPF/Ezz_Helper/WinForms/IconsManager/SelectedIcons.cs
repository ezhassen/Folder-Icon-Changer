using Ezz_Helper.Drawing.IconsManager;
using System.Collections.Generic;
using System.Drawing;

namespace Ezz_Helper.WinForms.IconsManager
{
	public class SelectedIcons : List<SelectedIconInfo>
	{
		public SelectedIcons()
		{
		}

		public bool? DialogResult { get; set; }

		public IconViewTypes IconViewType { get; set; }

		public string FilePath { get; set; }

		public int ICount { get; set; }

		public SelectedIconInfo GetFirstItem()
		{
			if (this.Count == 0) return null;
			return this[0];
		}
		public IconInfo.IconImageInfo GetFirstIconImageInfo()
		{
			if (this.Count == 0) return null;
			return this[0].SelectedIconImageInfo;
		}
		public List<IconInfo.IconImageInfo> GetAllIconImageInfo()
		{
			List<IconInfo.IconImageInfo> newL = new List<IconInfo.IconImageInfo>();
			foreach (SelectedIconInfo item in this)
			{
				newL.Add(item.SelectedIconImageInfo);
			}
			return newL;
		}

		public Icon GetFirstSourceIcon()
		{
			if (this.Count == 0) return null;
			return this[0].SourceIcon;
		}
		public List<Icon> GetAllSourceIcons()
		{
			List<Icon> newL = new List<Icon>();
			foreach (SelectedIconInfo item in this)
			{
				newL.Add(item.SourceIcon);
			}
			return newL;
		}


		public static SelectedIcons InitializeNew() => new SelectedIcons();
	}
}
