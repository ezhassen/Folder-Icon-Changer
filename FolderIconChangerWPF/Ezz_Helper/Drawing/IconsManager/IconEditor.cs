using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using Ezz_Helper.Files;

namespace Ezz_Helper.Drawing.IconsManager
{

    public static partial class Extensions
    {
        public static void AddFirst(this LinkedList<IconEditor.KeyedIconImageInfo> UList, string Key_1, IconInfo.IconImageInfo[] value_1)
        {
            UList.AddFirst(new IconEditor.KeyedIconImageInfo(Key_1, value_1));
        }
    }

    /// <summary>
    /// To add and remove icons in an IconInfo(Icon group). v1
    /// </summary>
    public class IconEditor : IDisposable
    {
        public IconEditor() : this(new IconInfo()) { }
        public IconEditor(Bitmap image, params OneIconInfo[] OneIconInfos) : this(new IconInfo())
        {
            this.Add(image, OneIconInfos);
        }
        public IconEditor(Icon Icon_1) : this(new IconInfo(Icon_1)) { }
        public IconEditor(IconInfo IconInfo_1) { Initialize(IconInfo_1); }
        private void Initialize(Icon Icon_1, bool DisposeOldIconInfo = true, bool DisposeSourceIcon = false)
        { Initialize(new IconInfo(Icon_1), DisposeOldIconInfo, DisposeSourceIcon); }
        private void Initialize(IconInfo IconInfo_1, bool DisposeOldIconInfo = true, bool DisposeSourceIcon = false)
        {
            if (_IconInfo != null)
            {
                if (DisposeSourceIcon) _IconInfo.SourceIcon.Dispose();
                if (DisposeOldIconInfo) _IconInfo.Dispose();
            }
            //
            _IconInfo = IconInfo_1;
            CancelChanges();
        }

        public Icon SourceIcon { get { return _IconInfo.SourceIcon; } }

        private IconInfo _IconInfo;
        public IconInfo IconInfo { get { return _IconInfo; } }

        //
        private List<IconInfo.IconImageInfo> _IconsToRemove = new List<IconInfo.IconImageInfo>();
        public ReadOnlyCollection<IconInfo.IconImageInfo> IconsToRemove { get { return _IconsToRemove.AsReadOnly(); } }

        private List<IconInfo.IconImageInfo> _IconsToAdd = new List<IconInfo.IconImageInfo>();
        public ReadOnlyCollection<IconInfo.IconImageInfo> IconsToAdd { get { return _IconsToAdd.AsReadOnly(); } }

        //
        private LinkedList<KeyedIconImageInfo> UndoList = new LinkedList<KeyedIconImageInfo>();
        private LinkedList<KeyedIconImageInfo> RedoList = new LinkedList<KeyedIconImageInfo>();
        public class KeyedIconImageInfo
        {
            public KeyedIconImageInfo(string Key_1, IconInfo.IconImageInfo[] value_1)
            {
                key = Key_1;
                Images = value_1;
            }

            public string key { get; set; }
            public IconInfo.IconImageInfo[] Images { get; set; }

        }


        //
        public bool CanUndo { get { return UndoList.Count > 0; } }
        public bool CanRedo { get { return RedoList.Count > 0; } }
        private bool _Edited;
        public bool Edited
        {
            get { return _Edited; }
        }

        public delegate void ImageTransHandler(object sender, IconInfo.IconImageInfo[] IImI);

        public event ImageTransHandler ImageAdded;
        public virtual void OnImageAdded(IconInfo.IconImageInfo[] images) { if (ImageAdded != null) ImageAdded(this, images); }
        public event ImageTransHandler ImageRemoved;
        public virtual void OnImageRemoved(IconInfo.IconImageInfo[] images) { if (ImageRemoved != null) ImageRemoved(this, images); }
        //public event EventHandler ImagesCleared;
        //public virtual void OnImagesCleared() { if (ImagesCleared != null) ImagesCleared(this, EventArgs.Empty); }

        #region public methods

        /// <summary>
        /// To add a new icon to the source icon.
        /// </summary>
        public void Add(IconInfo.IconImageInfo IconImageInfo_1)
        {
            Add(new IconInfo.IconImageInfo[] { IconImageInfo_1 });
        }
        /// <summary>
        /// To add new icons to the source icon.
        /// </summary>
        /// <returns></returns>
        public void Add(params IconInfo.IconImageInfo[] images_1)
        {
            AddTo_AddList(images_1);
            UndoList.AddFirst("add", images_1);
            RedoList.Clear();
            OnImageAdded(images_1);
        }
        private void AddTo_AddList(params IconInfo.IconImageInfo[] images_1)
        {
            foreach (IconInfo.IconImageInfo _Image in images_1)
            {
                //check _IconsToRemove list
                if (_IconsToRemove.Contains(_Image))
                {
                    _IconsToRemove.Remove(_Image);
                }
                else
                {
                    _IconsToAdd.Add(_Image);
                }
            }
            //
            _Edited = true;
        }
        public List<OneIconInfo> Add(Bitmap Image, params OneIconInfo[] OneIconInfos)
        {
            var res = new List<OneIconInfo>();
            if (OneIconInfos == null || OneIconInfos.Length == 0) return res;
            foreach (var oneIconInfo in OneIconInfos)
            {
                if (!this.ContainsIconWithColors(oneIconInfo))
                {
                    this.Add(new IconInfo.IconImageInfo((new Bitmap(Image, oneIconInfo.Size)).ImageToIcon(null, null, true, oneIconInfo.ImageColorsType)));
                    res.Add(oneIconInfo);
                }
            }
            return res;
        }


        /// <summary>
        /// To remove an icon from the source icon.
        /// </summary>
        /// <param name="IconImageInfo_1"></param>
        /// <returns></returns>
        public void Remove(IconInfo.IconImageInfo IconImageInfo_1)
        {
            Remove(new IconInfo.IconImageInfo[] { IconImageInfo_1 });
        }
        public void Remove(params IconInfo.IconImageInfo[] images_1)
        {
            AddTo_RemoveList(images_1);
            UndoList.AddFirst("remove", images_1);
            RedoList.Clear();
            OnImageRemoved(images_1);
        }
        private void AddTo_RemoveList(params IconInfo.IconImageInfo[] images_1)
        {
            foreach (IconInfo.IconImageInfo _Image in images_1)
            {
                //check _IconsToAdd list
                if (_IconsToAdd.Contains(_Image))
                {
                    _IconsToAdd.Remove(_Image);
                }
                else
                {
                    //
                    //if (_IconInfo.ImageList.Contains(_Image))
                    _IconsToRemove.Add(_Image);
                }
            }
            _Edited = true;
        }

        public void Undo()
        {
            if (!CanUndo) return;
            var FItem = UndoList.First.Value;
            switch (FItem.key)
            {
                case "add": //
                    UndoList.RemoveFirst();
                    RedoList.AddFirst(FItem);
                    AddTo_RemoveList(FItem.Images);
                    OnImageRemoved(FItem.Images);
                    break;
                case "remove": //
                    UndoList.RemoveFirst();
                    RedoList.AddFirst(FItem);
                    AddTo_AddList(FItem.Images);
                    OnImageAdded(FItem.Images);
                    break;
                default:
                    break;
            }
        }
        public void Redo()
        {
            if (!CanRedo) return;
            var FItem = RedoList.First.Value;
            switch (FItem.key)
            {
                case "add":
                    RedoList.RemoveFirst();
                    UndoList.AddFirst(FItem);
                    AddTo_AddList(FItem.Images);
                    OnImageAdded(FItem.Images);
                    break;
                case "remove":
                    RedoList.RemoveFirst();
                    UndoList.AddFirst(FItem);
                    AddTo_RemoveList(FItem.Images);
                    OnImageRemoved(FItem.Images);
                    break;
                default:
                    break;
            }
        }

        public bool ContainsIconWithColors(IconInfo.IconImageInfo IconImageInfo_1)
        {
            return ContainsIconWithColors(IconImageInfo_1.Size, IconImageInfo_1.ColorsType);
        }
        public bool ContainsIconWithColors(OneIconInfo OneIconInfo)
        {
            return ContainsIconWithColors(OneIconInfo.Size, OneIconInfo.ImageColorsType);
        }
        public bool ContainsIconWithColors(Size Size_, ImageColorsTypes ColorsType_)
        {
            //Check added images
            if (_IconsToAdd.ContainsIconWithColors(Size_, ColorsType_)) return true;
            //Check removed N source icon images
            if (IconInfo.ContainsIconWithColors(Size_, ColorsType_))
            {
                return (!_IconsToRemove.ContainsIconWithColors(Size_, ColorsType_));
            }
            return false;
        }


        public int GetICount() { return ((_IconInfo.IconDir.Count - _IconsToRemove.Count) + _IconsToAdd.Count); }
        /// <summary>
        /// Saves changes of current icon to the target <see cref="Stream"/>.
        /// </summary>
        /// <param name="NewIMS">the target stream.</param>
        public void SaveTo(Stream NewIMS)
        {
            //if (!_Edited)
            //{
            //    _IconInfo.SourceIcon.Save(NewIMS);
            //    return;
            //}
            //
            using (MemoryStream SourceIMS = new MemoryStream())
            {
                //_IconInfo.SourceIcon.Save(SourceIMS);
                SourceIMS.Seek(0, SeekOrigin.Begin);

                //using (MemoryStream NewIMS = new MemoryStream())
                //{
                //Create Icon dir header

                IconDir IDir = IconDir.InitializeNew((short)GetICount());
                //IDir.Count = (short)GetICount();
                //IDir.Reserved = 0;
                //IDir.Type = 1;
                //Write the IconDir header.
                NewIMS.Seek(0, SeekOrigin.Begin);
                Tools.WriteStructure<IconDir>(NewIMS, IDir);
                //IDir.Write(NewIMS);
                //
                int imageIndex = 0;
                int imageOffset = IconInfo.SizeOfIconDir + IDir.Count * IconInfo.SizeOfIconDirEntry;
                //Remove first (adding icons that is not in remove list)
                foreach (var ImageInfo in _IconInfo.ImageList)
                {
                    if (!_IconsToRemove.Contains(ImageInfo))
                    {
                        IconDirEntry entry = ImageInfo.Write(NewIMS, imageIndex, imageOffset);
                        //
                        //Update the imageIndex and the imageOffset
                        imageIndex++;
                        imageOffset += entry.BytesInRes;
                    }
                }
                //
                //foreach (var entry in _IconInfo.IconDirEntries)
                //{
                //    if (!_IconsToRemove.Contains(entry))
                //    {
                //        //Read image
                //        byte[] rawData = new byte[entry.BytesInRes];
                //        SourceIMS.Seek(entry.ImageOffset, SeekOrigin.Begin);
                //        SourceIMS.Read(rawData, 0, rawData.Length);
                //        //Write image
                //        //update the ImageOffset to the new offset.
                //        IconDirEntry NewEntry = entry;
                //        NewEntry.ImageOffset = imageOffset;
                //        //Write the IconDirEntry to the stream.
                //        NewIMS.Seek(IconInfo.SizeOfIconDir + imageIndex * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
                //        Tools.WriteStructure<IconDirEntry>(NewIMS, NewEntry);
                //        //Write image raw data.
                //        NewIMS.Seek(imageOffset, SeekOrigin.Begin);
                //        NewIMS.Write(rawData, 0, NewEntry.BytesInRes);

                //        //Update the imageIndex and the imageOffset
                //        imageIndex++;
                //        imageOffset += entry.BytesInRes;
                //    }
                //}
                //then add new icons
                if (_IconsToAdd.Count > 0)
                {
                    foreach (IconInfo.IconImageInfo IconImageInfo_1 in _IconsToAdd)
                    {
                        IconDirEntry entry = IconImageInfo_1.Write(NewIMS, imageIndex, imageOffset);
                        //
                        //Update the imageIndex and the imageOffset
                        imageIndex++;
                        imageOffset += entry.BytesInRes;
                    }
                }

                NewIMS.Seek(0, SeekOrigin.Begin);
            }
        }
        /// <summary>
        /// Saves changes of current icon to the target File.
        /// </summary>
        /// <param name="FilePath">target file.</param>
        /// <param name="SameFileNameDecision">what to do if there is same file name in the folder?</param>
        public SaveToFileInfo SaveTo(string FilePath,
            SameFileNameDecisions SameFileNameDecision = SameFileNameDecisions.Rename, string subFormat = " ({0})")
        {
            return SaveTo(new SaveToFileInfo(FilePath, SameFileNameDecision, subFormat));
        }
        /// <summary>
        ///  Saves changes of current icon to the target File.
        /// </summary>
        /// <param name="SaveToFileInfo_1"></param>
        /// <returns></returns>
        public SaveToFileInfo SaveTo(SaveToFileInfo SaveToFileInfo_1)
        {
            if (SaveToFileInfo_1.GenNewFilePath_ReturnCanWrite())
            {
                using (FileStream CrFile = new FileStream(SaveToFileInfo_1.NewFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    SaveTo(CrFile);
                }
            }
            return SaveToFileInfo_1;
        }

        //
        /// <summary>
        ///  Saves changes of current icon to a new <see cref="System.Drawing.Icon"/>.
        /// </summary>
        /// <returns></returns>
        public Icon SaveToNewIcon()
        {
            Icon resIcon = null;
            using (MemoryStream newIM = new MemoryStream())
            {
                SaveTo(newIM);
                resIcon = new Icon(newIM);
            }
            return resIcon;
        }

        //
        /// <summary>
        /// To reset lists and values to default.
        /// </summary>
        public void CancelChanges()
        {
            _IconsToRemove = new List<IconInfo.IconImageInfo>();
            _IconsToAdd = new List<IconInfo.IconImageInfo>();
            _Edited = false;
        }
        /// <summary>
        /// Saves current changes to the source icon. and reset lists and values to default. and reload the IconInfo.
        /// </summary>
        public void SaveChanges(bool DisposeOldIconInfo = true, bool DisposeSourceIcon = false)
        {
            this.Initialize(SaveToNewIcon(), DisposeOldIconInfo, DisposeSourceIcon);
        }




        #endregion public methods

        //DLL
        //public static Other_Helplers.Other_H.SaveToFileInfo SaveIconsToNewDLL(Other_Helplers.Other_H.SaveToFileInfo SaveToFileInfo_1,
        //                                                                 Func<uint, string> IconName_1, params IconInfo[] IconsInfo_)
        //{
        //    // LoadLibraryEx only can load files from File System, lets create a tmp file
        //    string tmpFile = null;
        //    IntPtr hLib = IntPtr.Zero;
        //    MemoryStream ms;
        //    bool bResult;
        //    FileStream stream = SaveToFileInfo_1.CreateFile();
        //    try
        //    {
        //        stream.Position = 0;

        //        // Find a tmp file where to dump the DLL stream, later we will remove this file
        //        tmpFile = Path.GetTempFileName();

        //        FileStream fs = new FileStream(tmpFile, FileMode.Create, FileAccess.Write);
        //        byte[] buffer = Resource.EmptyDll;
        //        stream.Read(buffer, 0, buffer.Length);
        //        fs.Write(buffer, 0, buffer.Length);
        //        fs.Close();

        //        // Begin the injection process
        //        IntPtr updPtr = Win32.BeginUpdateResource(tmpFile, false);
        //        if (updPtr == IntPtr.Zero) throw new Exception("InvalidFile");
        //        //
        //        ushort iconIndex = 1;
        //        foreach (IconInfo IInf in IconsInfo_)
        //        {
        //            // Lets scan all groups
        //            GRPICONDIR grpIconDir = IInf.GroupIconDir; //GRPICONDIR.Initalizated;

        //            //grpIconDir.Count = IInf.IconDir.Count;
        //            //grpIconDir.Entries = new GRPICONDIRENTRY[grpIconDir.Count];
        //            int imageIndex = 0;
        //            int imageOffset = IconInfo.SizeOfIconDir + grpIconDir.Count * IconInfo.SizeOfIconDirEntry;
        //            for (int i = 0; i < grpIconDir.Count; i++)
        //            {
        //                var curGEntry = IInf.GroupIconDirEntries[i];
        //                curGEntry.ID = (short)iconIndex;
        //                var entry = curGEntry.ToIconDirEntry(imageOffset);

        //                // Buffer creation with the same size of the icon to optimize write call
        //                ms = new MemoryStream((int)curGEntry.BytesInRes);
        //                //Write the IconDirEntry to the stream.
        //                ms.Seek(IconInfo.SizeOfIconDir + imageIndex * IconInfo.SizeOfIconDirEntry, SeekOrigin.Begin);
        //                Tools.WriteStructure<IconDirEntry>(ms, entry);
        //                //Write the image raw data.
        //                ms.Seek(imageOffset, SeekOrigin.Begin);
        //                ms.Write(IInf.RawData[i], 0, entry.BytesInRes);
        //                //
        //                //Update the imageIndex and the imageOffset
        //                imageIndex++;
        //                imageOffset += entry.BytesInRes;

        //                buffer = ms.GetBuffer();

        //                //
        //                // Update resource but it doesn't write to disk yet
        //                bResult = Win32.UpdateResource(updPtr, (int)ResourceType.RT_ICON, iconIndex, 0, buffer, (uint)ms.Length);

        //                iconIndex++;

        //                // For some reason Windows will fail if there are many calls to update resource and no call to endUpdateResource
        //                // It is like there some internal buffer that gets full, after that all calls fail.
        //                // This workaround will save the changes every 70 icons, for big files this slow the saving process significantly
        //                // but I didn't find a way to make EndUpdateResource works without save frequently
        //                if ((iconIndex % 70) == 0)
        //                {
        //                    bResult = Win32.EndUpdateResource(updPtr, false);
        //                    updPtr = Win32.BeginUpdateResource(tmpFile, false);
        //                    if (updPtr == IntPtr.Zero) throw new Exception("InvalidFile");
        //                }
        //            }
        //            // Buffer creation with the same size of the group to optimize write call
        //            ms = new MemoryStream();//(IconInfo.SizeOfIconDir + grpIconDir.Count * IconInfo.SizeOfIconDirEntry);
        //            //grpIconDir.Write(ms);
        //            BinaryWriter bw = new BinaryWriter(stream);

        //            bw.Write(grpIconDir.Reserved);
        //            bw.Write(grpIconDir.Type);
        //            bw.Write(grpIconDir.Count);
        //            for (int i = 0; i < grpIconDir.Count; i++)
        //            {
        //                //idEntries[i].Write(stream);
        //                //byte[] array = new byte[sizeof(GRPICONDIRENTRY)];
        //                //fixed (GRPICONDIRENTRY* ptr = &IInf.GroupIconDirEntries[i])
        //                //    Marshal.Copy((IntPtr)ptr, array, 0, sizeof(GRPICONDIRENTRY));
        //                //stream.Write(array, 0, sizeof(GRPICONDIRENTRY));
        //                Tools.WriteStructure<IconDirEntry>(ms, IInf.IconDirEntries[i]);
        //            }
        //            buffer = ms.GetBuffer();

        //            int id;
        //            string IconName_ = IconName_1 != null ? IconName_1(iconIndex) : iconIndex.ToString();
        //            if (int.TryParse(IconName_, out id))
        //            {
        //                // Write id as an integer
        //                bResult = Win32.UpdateResource(updPtr, (int)ResourceType.RT_GROUP_ICON, (IntPtr)id, 0, buffer, (uint)ms.Length);
        //            }
        //            else
        //            {
        //                // Write id as string
        //                IntPtr pName = Marshal.StringToHGlobalAnsi(IconName_.ToUpper());
        //                bResult = Win32.UpdateResource(updPtr, (int)ResourceType.RT_GROUP_ICON, pName, 0, buffer, (uint)ms.Length);
        //                Marshal.FreeHGlobal(pName);
        //            }
        //            ms.Close();

        //        }

        //        // Last call to update the file with the rest not that was not write before
        //        bResult = Win32.EndUpdateResource(updPtr, false);
        //        //

        //        // Because Windows Resource functions require a filepath, and we need to return an string then lets open
        //        // the temporary file and dump it to the stream received as parameter.
        //        using (fs = new FileStream(tmpFile, FileMode.Open, FileAccess.Read))
        //        {
        //            buffer = new byte[fs.Length];
        //            fs.Read(buffer, 0, buffer.Length);
        //            stream.Write(buffer, 0, buffer.Length);
        //        }
        //        stream.Close();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        if (hLib != null)
        //            Win32.FreeLibrary(hLib);
        //        if (tmpFile != null)
        //            File.Delete(tmpFile);
        //    }

        //    return SaveToFileInfo_1;
        //}


        public void Dispose()
        {
            if (_IconsToAdd.Count != 0)
            {
                foreach (IconInfo.IconImageInfo item in _IconsToAdd)
                {
                    item.Dispose();
                }
            }
            if (_IconsToRemove.Count != 0)
            {
                foreach (IconInfo.IconImageInfo item in _IconsToRemove)
                {
                    item.Dispose();
                }
            }
            if (IconInfo.SourceIcon != null) IconInfo.SourceIcon.Dispose();
            IconInfo.Dispose();
        }
        ~IconEditor()
        {
            Dispose();
        }

        #region Static Methods

        //public static 

        #endregion //Static Methods

    }
    public class OneIconInfo
    {
        public OneIconInfo() { }
        public OneIconInfo(Size Size_, ImageColorsTypes ImageColorsTypes_) { Size = Size_; ImageColorsType = ImageColorsTypes_; }

        public Size Size { get; set; }
        public ImageColorsTypes ImageColorsType { get; set; }
    }
}