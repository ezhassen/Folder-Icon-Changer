using System;
using System.Linq;
using System.Xml.XPath;
using System.IO;
using System.Text.RegularExpressions;

namespace FolderIconChangerWPF
{
    public static class StringExtensions
    {

        /// <summary>
        ///  Returns a string containing a specified number of characters from the left side of a string.
        /// </summary>
        /// <param name="str">String expression from which the leftmost characters are returned.</param>
        /// <param name="length"> Integer expression. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in str, the entire string is returned.</param>
        /// <returns>Returns a string containing a specified number of characters from the left side of a string.</returns>
        public static string Left(this string str, int length)
        {
            return str.Substring(0, Math.Min(length, str.Length));
        }
        /// <summary>
        /// Returns a string containing a specified number of characters from the right side of a string
        /// </summary>
        /// <param name="str"> String expression from which the rightmost characters are returned.</param>
        /// <param name="length"> Integer. Numeric expression indicating how many characters to return. If 0, a zero-length string ("") is returned. If greater than or equal to the number of characters in str, the entire string is returned.</param>
        /// <returns>Returns a string containing a specified number of characters from the right side of a string.</returns>
        public static string Right(this string str, int length)
        {
            var SLeng = str.Length;
            return str.Substring(Math.Max((SLeng - length), 0), SLeng);
        }


        public static int Len(this string str, string Expression)
        {
            return Regex.Matches(str, Expression).Count;
        }
        public static int Len(this string str, char Expression)
        {
            return str.Count(x => x == Expression);
        }

        public static long ValLong(this string str)
        {
            long res = 0;
            long.TryParse(str, out res);
            return res;
        }
        public static int ValInt(this string str)
        {
            int res = 0;
            int.TryParse(str, out res);
            return res;
        }
        public static decimal ValDecimal(this string str)
        {
            decimal res = 0m;
            decimal.TryParse(str, out res);
            return res;
        }
        public static double Val(this string str)
        {
            double res = 0;
            double.TryParse(str, out res);
            return res;
        }
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
        public static bool IsNumeric(this string str, bool useLoop = false)
        {
            try
            {
                if (useLoop)
                {
                    bool therewasDigit = false;
                    foreach (Char c in str)//str.ToCharArray()
                    {
                        if (!Char.IsNumber(c))
                        {
                            if (therewasDigit) return false;
                            if (!Char.IsDigit(c))
                            {
                                return false;
                            }
                            else { therewasDigit = true; }
                        }
                    }
                    return true;
                }
                else
                {
                    double myNum = 0;
                    var res = double.TryParse(str, out myNum);

                    return res ? (!double.IsNaN(myNum)) : false;
                }
            }
            catch { }
            return false;
        }
        public static bool IsDate(this string str)
        {
            DateTime dt;
            return DateTime.TryParse(str, out dt);
        }

        public static string GetStringBetween2Strings(this string SourceString, string StartSearch, string EndSearch, bool UseRegex = true, bool IgnoreCase_ = false)
        {
            if (UseRegex)
            {
                try
                {
                    Regex rx;
                    if (IgnoreCase_)
                    {
                        rx = new Regex(StartSearch + "(.+?)" + EndSearch, RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        rx = new Regex(StartSearch + "(.+?)" + EndSearch);
                    }

                    Match m = rx.Match(SourceString);
                    if (m.Success)
                    {
                        return m.Groups[1].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                catch (Exception)
                {
                    return "";
                }
            }
            else
            {
                //String.
                try
                {
                    var comparison = IgnoreCase_ ? StringComparison.CurrentCultureIgnoreCase : StringComparison.InvariantCulture;
                    var fromLength = (StartSearch ?? string.Empty).Length;
                    var startIndex = !string.IsNullOrEmpty(StartSearch)
                        ? SourceString.IndexOf(StartSearch, comparison) + fromLength
                        : 0;

                    if (startIndex < fromLength) { return ""; }//{ throw new ArgumentException("from: Failed to find an instance of the first anchor"); }

                    var endIndex = !string.IsNullOrEmpty(EndSearch)
                    ? SourceString.IndexOf(EndSearch, startIndex, comparison)
                    : SourceString.Length;

                    if (endIndex < 0) { return ""; }//{ throw new ArgumentException("until: Failed to find an instance of the last anchor"); }

                    return SourceString.Substring(startIndex, endIndex - startIndex);

                    //if (SourceString.Contains(StartSearch) && SourceString.Contains(EndSearch))
                    //{
                    //    //int istart = FullString.IndexOf(StartSearch, 0);
                    //    //if (istart > 0) {
                    //    //}


                    //    //
                    //    //VB.Net
                    //    //int istart = InStr(FullString, StartSearch);
                    //    //if (istart > 0)
                    //    //{
                    //    //    int istop = InStr(istart, FullString, EndSearch);
                    //    //    if (istop > 0)
                    //    //    {
                    //    //        try
                    //    //        {
                    //    //            string value = FullString.Substring(istart + Len(StartSearch) - 1, istop - istart - Len(StartSearch));
                    //    //            return value;
                    //    //        }
                    //    //        catch (Exception ex)
                    //    //        {
                    //    //            return "";
                    //    //        }
                    //    //    }
                    //    //}
                    //}
                    //return "";
                }
                catch (Exception)
                {
                    return "";
                }
            }

        }

        public static bool GetBooleanFromString(this string boolString, bool Defaultbool = false)
        {
            bool res = false;
            if (!bool.TryParse(boolString, out res)) return Defaultbool;
            return res;
        }

        //
        /// <summary>
        /// Evaluate expression that contains  +, -, *, /, %(modulus)
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="DataTableMethod"></param>
        /// <returns></returns>
        public static string EvaluateString(this string expression)//, bool DataTableMethod = false)
        {
            //return expression.Evaluate(DataTableMethod).ToString();
            return expression.Evaluate().ToString();
        }
        /// <summary>
        /// Evaluate expression that contains  +, -, *, /, %(modulus)
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="DataTableMethod"></param>
        /// <returns></returns>
        public static double Evaluate(this string expression)//, bool DataTableMethod = false)
        {
            //if (DataTableMethod)
            //{
            //    return (double)new DataTable().Compute(expression, null);
            //}
            //else
            //{
            var xsltExpression =
                string.Format("number({0})",
                    new Regex(@"([\+\-\*])").Replace(expression, " ${1} ")
                                            .Replace("/", " div ")
                                            .Replace("%", " mod "));

            return (double)new XPathDocument
                (new StringReader("<r/>"))
                    .CreateNavigator()
                    .Evaluate(xsltExpression);
            //}
        }

        public static bool TryEvaluate_Decimal(this string expression, ref decimal? result)
        {
            try
            {
                result = (decimal)Evaluate(expression);
                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
        public static bool CanEvaluate(this string expression)
        {
            try
            {
                var res = Evaluate(expression);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Determines whether a sequence contains a specified element by using the default equality comparer.
        /// </summary>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="toCheck">The value to locate in the sequence.</param>
        /// <param name="comp">One of the enumeration values that specifies the rules for the comparison.</param>
        /// <returns>true if the source sequence contains an element that has the specified value;  otherwise, false.</returns>
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(toCheck)) return false;
            //if (string.IsNullOrEmpty(toCheck)) return true;
            return source.IndexOf(toCheck, comp) >= 0;
        }


        public static bool ContainsAnyOfAny(this string[] strs, StringComparison stringComparison, params string[][] searchStrings)
        {
            foreach (var str in strs)
            {
                if (str.ContainsAny(stringComparison, searchStrings)) return true;
            }
            return false;
        }

        public static bool ContainsAny(this string str, StringComparison stringComparison, params string[][] searchStrings)
        {
            foreach (var searchStrs in searchStrings)
            {
                if (str.ContainsAny(stringComparison, searchStrs)) return true;
            }
            return false;
        }

        public static bool ContainsAny(this string str, StringComparison stringComparison, params string[] searchStrings)
        {
            foreach (var search in searchStrings)
            {
                if (str.Contains(search, stringComparison)) return true;
            }
            return false;
        }

        public static bool ContainsAny(this string str, params string[] searchStrings) => ContainsAny(str, StringComparison.Ordinal, searchStrings);

        //public static bool Contains(this string str, string search, StringComparison stringComparison) => str.IndexOf(search, stringComparison) >= 0;
        public static bool EndsWithAny(this string str, params string[][] searchStrings) => EndsWithAny(str, StringComparison.Ordinal, searchStrings);
        public static bool EndsWithAny(this string str, StringComparison stringComparison, params string[][] searchStrings)
        {
            foreach (var endStrs in searchStrings)
            {
                if (str.EndsWithAny(stringComparison, endStrs)) return true;
            }
            return false;
        }
        public static bool EndsWithAny(this string str, params string[] searchStrings) => EndsWithAny(str, StringComparison.Ordinal, searchStrings);
        public static bool EndsWithAny(this string str, StringComparison stringComparison, params string[] searchStrings)
        {
            foreach (var endStr in searchStrings)
            {
                if (str.EndsWith(endStr, stringComparison)) return true;
            }
            return false;
        }
    }
}
