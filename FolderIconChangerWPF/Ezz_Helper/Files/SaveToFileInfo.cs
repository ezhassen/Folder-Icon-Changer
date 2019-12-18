using System;
using System.IO;

namespace Ezz_Helper.Files
{
    public class SaveToFileInfo
    {
        public SaveToFileInfo(string FilePath_1,
            SameFileNameDecisions SameFileNameDecision_1 = SameFileNameDecisions.Rename, string subFormat = " ({0})", DateTime? _ModifiedDate = null)
        {
            _FilePath = FilePath_1;
            NewFilePath = FilePath_1;
            SameFileNameDecision = SameFileNameDecision_1;
            NewNameFormat = subFormat;
            ModifiedDate = _ModifiedDate;
        }

        private string _FilePath;
        public string FilePath { get { return _FilePath; } }
        public SameFileNameDecisions SameFileNameDecision { get; set; }
        public string NewFilePath { get; private set; }
        public string NewFileNameWithoutExtension
        {
            get
            {
                return Path.GetFileNameWithoutExtension(NewFilePath);
            }
            set
            {
                if (string.IsNullOrEmpty(value)) return;
                string DefExt = Path.GetExtension(_FilePath);
                string DestFolder_ = Path.GetDirectoryName(_FilePath);
                NewFilePath = Path.Combine(DestFolder_, value + DefExt);
            }
        }

        public string NewNameFormat { get; set; }

        public DateTime? ModifiedDate { get; set; }

        //public bool _FileCreated = false;
        //public bool FileCreated { get { return _FileCreated; } }
        public bool? IsSkiped { get; private set; }

        public bool GenNewFilePath_ReturnCanWrite()
        {
            bool res = false;
            switch (SameFileNameDecision)
            {
                case SameFileNameDecisions.Skip:
                    res =  false;
                    break;
                case SameFileNameDecisions.Overwrite:
                    NewFilePath = _FilePath;
                    res =  true;
                    break;
                case SameFileNameDecisions.Overwrite_IfNew:
                    if (ModifiedDate.HasValue)
                    {
                        var OvNew = new FileInfo(_FilePath);
                        if (OvNew.Exists)
                        {
                            if (ModifiedDate.Value > OvNew.LastWriteTime)
                            {
                                NewFilePath = _FilePath;
                                res =  true;
                            }
                            else
                            {
                                res =  false;
                            }
                        }
                    }
                    else
                    {
                        NewFilePath = _FilePath;
                        res =  true;
                    }
                    break;
                case SameFileNameDecisions.Overwrite_IfOld:
                    if (ModifiedDate.HasValue)
                    {
                        var OvOld = new FileInfo(_FilePath);
                        if (OvOld.Exists)
                        {
                            if (ModifiedDate.Value < OvOld.LastWriteTime)
                            {
                                NewFilePath = _FilePath;
                                res =  true;
                            }
                            else
                            {
                                res =  false;
                            }
                        }
                    }
                    else
                    {
                        res =  false;
                    }
                    break;
                //
                case SameFileNameDecisions.Rename:
                    NewFilePath = GetNewFilePathIfExist(_FilePath, this.NewNameFormat);
                    res =  true;
                    break;
                case SameFileNameDecisions.Rename_IfNew:
                    if (ModifiedDate.HasValue)
                    {
                        var ReNew = new FileInfo(_FilePath);
                        if (ReNew.Exists)
                        {
                            if (ModifiedDate.Value > ReNew.LastWriteTime)
                            {
                                NewFilePath = GetNewFilePathIfExist(_FilePath, this.NewNameFormat);
                                res =  true;
                            }
                            else
                            {
                                res =  false;
                            }
                        }
                    }
                    else
                    {
                        NewFilePath = GetNewFilePathIfExist(_FilePath, this.NewNameFormat);
                        res =  true;
                    }
                    break;
                case SameFileNameDecisions.Rename_IfOld:
                    if (ModifiedDate.HasValue)
                    {
                        var ReNew = new FileInfo(_FilePath);
                        if (ReNew.Exists)
                        {
                            if (ModifiedDate.Value < ReNew.LastWriteTime)
                            {
                                NewFilePath = GetNewFilePathIfExist(_FilePath, this.NewNameFormat);
                                res =  true;
                            }
                            else
                            {
                                res =  false;
                            }
                        }
                    }
                    else
                    {
                        res =  false;
                    }
                    break;
            }
            IsSkiped = !res;
            return res;
        }
        //public FileStream CreateFile(bool GenNewFilePath = true)
        //{
        //    if (GenNewFilePath) if (!GenNewFilePath_ReturnCanWrite()) return null;
        //    _FileCreated = true;
        //    return File.Create(NewFilePath);
        //}

        /// <summary>
        /// Gets new file name in FullPath if it Exists and return the new full path
        /// </summary>
        /// <param name="FullPath">file full path.</param>
        /// <param name="subFormat">sub name format if file exists.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string GetNewFilePathIfExist(string FullPath, string subFormat = " ({0})")
        {
            if (!File.Exists(FullPath)) return FullPath;
            string DefName = Path.GetFileNameWithoutExtension(FullPath);
            string DefExt = Path.GetExtension(FullPath);
            string DestFolder_ = Path.GetDirectoryName(FullPath);
            string newPath = Path.Combine(DestFolder_, DefName + string.Format(subFormat, 1) + DefExt);
            int Int_ = 2;

            while (File.Exists(newPath))
            {
                newPath = Path.Combine(DestFolder_, DefName + string.Format(subFormat, Int_.ToString()) + DefExt);
                Int_++;
            }
            return newPath;
        }

    }

}
