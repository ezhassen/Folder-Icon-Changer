using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Ezz_Helper.Drawing;
using System.IO;
using FolderIconChangerWPF;

namespace Ezz_Helper.Drawing
{
    public class StoredImage
    {
        public StoredImage(bool SaveToDisk_1)
        {
            ImageFilePath = "";
            _SaveToDisk = SaveToDisk_1;
        }
        public StoredImage(string _ImageFilePath, bool SaveToDisk_1 = true)
        {
            ImageFilePath = _ImageFilePath;
            _SaveToDisk = SaveToDisk_1;
        }

        private bool _SaveToDisk = false;
        public bool SaveToDisk { get { return _SaveToDisk; } }


        Image Image_;
        /// <summary>
        /// Gets/sets the image from/to file(ImageFilePath Property).
        /// </summary>
        /// <value>Sets the image to file(ImageFilePath Property).</value>
        /// <returns>Gets the image from file(ImageFilePath Property) if it exists else nothing.</returns>
        /// <remarks></remarks>
        public Image Image
        {
            get
            {
                if (SaveToDisk)
                {
                    if (Image_ != null)
                    {
                        Image_.Dispose();
                        Image_ = null;
                    }
                    if (File.Exists(ImageFilePath)) Image_ = ImageH.FromFile_Unlocked(ImageFilePath);
                }
                return Image_;
            }
            set
            {
                if (SaveToDisk)
                {
                    DeleteImage();
                    if (value != null && !ImageFilePath.IsNullOrEmpty()) value.Save(ImageFilePath);
                }
                Image_ = value;
            }
        }
        public string ImageFilePath { get; set; }

        public string ToBase64()
        {
            if (this.Image == null) return "";
            return Image_.ImageToBase64();
        }
        public Image FromBase64(string base64String, bool SaveToDisk_1 = false)
        {
            Image Res = base64String.Base64ToImage();
            if (SaveToDisk_1)
            {
                Image = Res;
            }
            else
            {
                Image_ = Res;
            }
            return Res;
        }
        public void SetImageFromFile(string FilePath)
        {
            Image Res = null;
            try
            {
                Res = ImageH.FromFile_Unlocked(FilePath);
            }
            catch { }
            this.Image = Res;
        }

        /// <summary>
        /// Gets current image of the last get method in Image Property.
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public Image CurrentImage
        {
            get { return Image_; }
        }

        /// <summary>
        /// Deletes current image from the hard desk.(if it Exists)
        /// </summary>
        /// <remarks></remarks>
        public bool DeleteImage()
        {
            try
            {
                if (Image_ != null) Image_.Dispose();
                Image_ = null;
                if (File.Exists(ImageFilePath))
                {
                    FileInfo fi = new FileInfo(ImageFilePath);
                    fi.Delete();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
