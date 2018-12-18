using System;
using System.Collections.Generic;

namespace FolderIconChangerWPF
{
    #region EqualityComparers

    public class DynamicEqualityComparer<T> : IEqualityComparer<T>
    {
        public Func<T, T, bool> CompareMethod { get; set; }

        public DynamicEqualityComparer(Func<T, T, bool> compareMethod = null)
        {
            CompareMethod = compareMethod;
        }

        public bool Equals(T x, T y)
        {
            if (CompareMethod == null)
            {
                return x.Equals(y);
            }
            else
            {
                return CompareMethod(x, y);
            }
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }

        public static bool DynamicEquals<TSource>(TSource x, TSource y, Func<TSource, TSource, bool> CompareMethod)
        {
            return (new DynamicEqualityComparer<TSource>(CompareMethod)).Equals(x, y);
        }
    }

    public class CharIEqualityComparer : IEqualityComparer<char>
    {
        public CharIEqualityComparer()
        {
        }

        #region IEqualityComparer<char> Members

        public bool Equals(char x, char y)
        {
            return char.Equals(char.ToLower(x), char.ToLower(y));
        }

        public int GetHashCode(char obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }
    public class StringIEqualityComparer : IEqualityComparer<string>
    {
        private StringComparison _SComparison;
        public StringIEqualityComparer(StringComparison SComparison_1 = StringComparison.CurrentCultureIgnoreCase)
        {
            _SComparison = SComparison_1;
        }

        #region IEqualityComparer<string> Members

        public bool Equals(string x, string y)
        {
            return string.Equals(x, y, _SComparison);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }

        #endregion
    }

    #endregion

}
