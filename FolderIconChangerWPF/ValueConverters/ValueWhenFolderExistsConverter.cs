using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows;

namespace FolderIconChangerWPF.ValueConverters
{
    public abstract class ValueWhenFolderExistsConverterBase<ValueType> : ValueWhenConverterBase<bool, ValueType>
    {
        static readonly ConcurrentDictionary<string, (bool exists, long ticks)> _cache = new();
        const long CacheMs = 800;
        static bool CachedDirExists(string path)
        {
            var now = System.Environment.TickCount64;
            if (_cache.TryGetValue(path, out var e) && now - e.ticks < CacheMs) return e.exists;
            bool exists = Directory.Exists(path);
            _cache[path] = (exists, now);
            return exists;
        }

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Debug) Debugger.Break();
            try
            {
                string strValue = value as string;
                if (string.IsNullOrEmpty(strValue)) strValue = "";
                if (CachedDirExists(strValue) == When) return Value;

                return Otherwise;
            }
            catch
            {
                return Otherwise;
            }
        }
    }

    public class BoolWhenFolderExistsConverter : ValueWhenFolderExistsConverterBase<bool>
    {
        public BoolWhenFolderExistsConverter()
        {
            When = true;
            Value = true;
            Otherwise = false;
        }

    }
    public class VisibiltyWhenFolderExistsConverter : ValueWhenFolderExistsConverterBase<Visibility>
    {
        public VisibiltyWhenFolderExistsConverter()
        {
            When = true;
            Value = Visibility.Visible;
            Otherwise = Visibility.Hidden;
        }
    }

}
