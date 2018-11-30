using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FolderIconChangerWPF
{
    public class CharsChecker
    {
        public CharsChecker()
        {
            EmptyStringIsAllowed = true;
        }

        public HashSet<CharWithProperties> AllowedChars { get; } = new HashSet<CharWithProperties>();

        public HashSet<CharWithProperties> NotAllowedChars { get; } = new HashSet<CharWithProperties>();

        public bool EmptyStringIsAllowed { get; set; }
        public bool StringIsAllowed(string String_)
        {
            if (string.IsNullOrEmpty(String_)) return EmptyStringIsAllowed;
            int ACharsCount = AllowedChars.Count;
            int NCharsCount = NotAllowedChars.Count;
            if (ACharsCount != 0 && NCharsCount != 0)
            {
                foreach (char ch in String_)
                {
                    var find_A = AllowedChars.FindByChar(ch);
                    if (find_A != null)
                    {
                        if (find_A.Repeating < 0 ? false : (find_A.Repeating == 0 ? true : String_.Len(ch) > find_A.Repeating)) return false;

                        var Find_N = NotAllowedChars.FindByChar(ch);
                        if (Find_N != null) if (Find_N.Repeating < 0 ? false : (Find_N.Repeating == 0 ? true : String_.Len(ch) > Find_N.Repeating)) return false;
                    }
                    else
                    {
                        var Find_N = NotAllowedChars.FindByChar(ch);
                        if (Find_N == null) return false;//Not in the 2 HashSets
                        if (Find_N.Repeating < 0 ? false : (Find_N.Repeating == 0 ? true : String_.Len(ch) > Find_N.Repeating)) return false;
                    }
                }
            }
            else if (ACharsCount != 0)//
            {
                foreach (char ch in String_)
                {
                    var find_A = AllowedChars.FindByChar(ch);
                    if (find_A == null) return false;//Not in the HashSet
                    if (find_A.Repeating < 0 ? false : (find_A.Repeating == 0 ? true : String_.Len(ch) > find_A.Repeating)) return false;
                }
            }
            else if (NCharsCount != 0)
            {
                foreach (char ch in String_)
                {
                    var find_A = NotAllowedChars.FindByChar(ch);
                    if (find_A != null) if (find_A.Repeating < 0 ? false : (find_A.Repeating == 0 ? true : String_.Len(ch) > find_A.Repeating)) return false;
                }
            }
            return true;  //string is Allowed
        }

    }
    public class CharWithProperties
    {
        CharWithProperties()
        {
            Repeating = -1;
        }
        public CharWithProperties(char char_, int Repeating_ = -1)
        {
            Char = char_;
            Repeating = Repeating_;
        }

        public char Char { get; set; }
        /// <summary>
        /// The maximum number of repeating this char in the string.(Allowed N of times?) . (-1 = No limits)
        /// </summary>
        [Description("The maximum number of repeating this char in the string.(Allowed N of times?) . (-1 = No limits)")]
        public int Repeating { get; set; }

        public int CharCode {
            get {
                //if (Char == null) return -1;
                return Char;
            }
        }

        public sealed class CWPEqualityComparer : IEqualityComparer<CharWithProperties>
        {
            public bool Equals(CharWithProperties x, CharWithProperties y)
            {
                return (x.Char.Equals(y.Char));
            }

            public int GetHashCode(CharWithProperties obj)
            {
                return obj.GetHashCode();
            }
        }
    }
    public static class CCExtensions
    {
        //public static int Len(this string str, char Expression)
        //{
        //    return str.Count(x => x == Expression);
        //}
        public static void AddChars(this ICollection<CharWithProperties> CWPL, char[] chars, int Repeating = -1)
        {
            foreach (char ch in chars)
            {
                AddChar(CWPL, ch, Repeating);
            }
        }
        public static void AddChars(this ICollection<CharWithProperties> CWPL, char[] chars, Func<char, int> Repeating)
        {
            foreach (char ch in chars)
            {
                AddChar(CWPL, ch, Repeating(ch));
            }
        }
        public static void AddChar(this ICollection<CharWithProperties> CWPL, char ch, int Repeating = -1)
        {
            CWPL.Add(new CharWithProperties(ch, Repeating));
        }

        //
        public static void RemoveChars(this ICollection<CharWithProperties> CWPL, char[] chars)
        {
            //CWPL.RemoveAll((cwp) => chars.Contains(cwp.Char));
            //
            foreach (char ch in chars)
            {
                CWPL.RemoveChar(ch);
            }
        }
        public static void RemoveChars(this ICollection<CharWithProperties> CWPL, ICollection<CharWithProperties> chars)
        {
            //CWPL.RemoveAll((cwp) => chars.Contains_1(cwp));
            //
            foreach (CharWithProperties cwp in chars)
            {
                CWPL.RemoveChar(cwp.Char);
            }
        }
        public static bool RemoveChar(this ICollection<CharWithProperties> CWPL, char ch)
        {
            var Find_1 = CWPL.FindByChar(ch);
            if (Find_1 == null) return false;
            return CWPL.Remove(Find_1);
        }
        //
        public static bool Contains_1(this ICollection<CharWithProperties> CWPL, CharWithProperties cwp)
        {
            return CWPL.Contains(cwp, new CharWithProperties.CWPEqualityComparer());
        }
        public static bool ContainsChar(this ICollection<CharWithProperties> CWPL, char ch)
        {
            return (CWPL.FindByChar(ch) != null);
        }
        public static CharWithProperties FindByChar(this ICollection<CharWithProperties> CWPL, char ch)
        {
            return CWPL.FirstOrDefault((cwp) => cwp.Char == ch);
        }
        //
        public static IEnumerable<char> ToArrayOfChars(this ICollection<CharWithProperties> CWPL)
        {
            return CWPL.Select(cwp => cwp.Char);
            //int Lent = CWPL.Count - 1;
            //char[] newC = new char[Lent];
            //for (int i = 0; i < Lent; i++)
            //{
            //    newC[i] = CWPL[i].Char;
            //}
            //return newC;
        }
        public static HashSet<char> ToHashSetOfChars(this ICollection<CharWithProperties> CWPL)
        {
            int Lent = CWPL.Count - 1;
            HashSet<char> newC = new HashSet<char>();
            foreach (CharWithProperties item in CWPL)
            {
                newC.Add(item.Char);
            }
            return newC;
        }
        //

        public static bool StringContainsUnlistedChars(this ICollection<CharWithProperties> CWPL, string str, char[] ExludeFromCheck = null)
        {
            if (CWPL.Count == 0) return false;
            if (ExludeFromCheck != null)
            {
                foreach (char ch in str)
                {
                    if (!ExludeFromCheck.Contains(ch) && !CWPL.ContainsChar(ch)) return true;
                }
            }
            else
            {
                foreach (char ch in str)
                {
                    if (!CWPL.ContainsChar(ch)) return true;
                }
            }
            return false;
        }
        public static bool StringContainsMaxCharRepeating(this ICollection<CharWithProperties> CWPL, string str)
        {
            if (CWPL.Count == 0) return false;
            foreach (char ch in str)
            {
                var find_1 = CWPL.FindByChar(ch);
                if (find_1 != null && (find_1.Repeating < 0 ? false : str.Len(ch) > find_1.Repeating)) return true;
            }
            return false;
        }
        public static bool StringContainsChars(this string str, ICollection<CharWithProperties> CWPL)
        {
            foreach (CharWithProperties ch in CWPL)
            {
                if (str.Contains(ch.Char)) return true;
            }
            return false;
        }
        public static bool StringContainsChars(this string str, char[] chars)
        {
            if (chars == null) return false;
            foreach (char ch in chars)
            {
                if (str.Contains(ch)) return true;
            }
            return false;
        }

        public static bool StartsWithAnyChar(this string str, params char[] chars)
        {
            if (chars == null) return false;
            if (string.IsNullOrEmpty(str)) return false;
            var firstCH = str[0];
            foreach (char ch in chars)
            {
                if (ch.Equals(firstCH)) return true;
            }
            return false;
        }
        public static bool EndsWithAnyChar(this string str, params char[] chars)
        {
            if (chars == null) return false;
            if (string.IsNullOrEmpty(str)) return false;
            var lastCH = str[str.Length - 1];
            foreach (char ch in chars)
            {
                if (ch.Equals(lastCH)) return true;
            }
            return false;
        }
    }
}
