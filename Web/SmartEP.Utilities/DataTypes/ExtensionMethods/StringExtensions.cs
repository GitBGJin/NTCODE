#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;

using SmartEP.Utilities.DataTypes.Formatters;
#endregion

namespace SmartEP.Utilities.DataTypes.ExtensionMethods
{
    /// <summary>
    /// String extensions
    /// </summary>
    public static class StringExtensions
    {
        #region Functions

        #region AlphaCharactersOnly

        /// <summary>
        /// Keeps only alpha characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>the string only containing alpha characters</returns>
        public static string AlphaCharactersOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z]");
        }

        #endregion

        #region AlphaNumericOnly

        /// <summary>
        /// Keeps only alphanumeric characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>the string only containing alphanumeric characters</returns>
        public static string AlphaNumericOnly(this string Input)
        {
            return Input.KeepFilterText("[a-zA-Z0-9]");
        }

        #endregion

        #region Center

        /// <summary>
        /// Centers the input string (if it's longer than the length) and pads it using the padding string
        /// </summary>
        /// <param name="Input"></param>
        /// <param name="Length"></param>
        /// <param name="Padding"></param>
        /// <returns>The centered string</returns>
        public static string Center(this string Input, int Length, string Padding = " ")
        {
            if (Input.IsNullOrEmpty())
                Input = "";
            string Output = "";
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            Output += Input;
            for (int x = 0; x < (Length - Input.Length) / 2; ++x)
            {
                Output += Padding[x % Padding.Length];
            }
            return Output;
        }

        #endregion

        #region Encode

        /// <summary>
        /// Converts a string to a string of another encoding
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="OriginalEncodingUsing">The type of encoding the string is currently using (defaults to ASCII)</param>
        /// <param name="EncodingUsing">The type of encoding the string is converted into (defaults to UTF8)</param>
        /// <returns>string of the byte array</returns>
        public static string Encode(this string Input, Encoding OriginalEncodingUsing = null, Encoding EncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            OriginalEncodingUsing = OriginalEncodingUsing.NullCheck(new ASCIIEncoding());
            EncodingUsing = EncodingUsing.NullCheck(new UTF8Encoding());
            return Encoding.Convert(OriginalEncodingUsing, EncodingUsing, Input.ToByteArray(OriginalEncodingUsing))
                           .ToEncodedString(EncodingUsing);
        }

        #endregion

        #region ExpandTabs

        /// <summary>
        /// Expands tabs and replaces them with spaces
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="TabSize">Number of spaces</param>
        /// <returns>The input string, with the tabs replaced with spaces</returns>
        public static string ExpandTabs(this string Input, int TabSize = 4)
        {
            if (Input.IsNullOrEmpty())
                return Input;
            string Spaces = "";
            for (int x = 0; x < TabSize; ++x)
                Spaces += " ";
            return Input.Replace("\t", Spaces);
        }

        #endregion

        #region FilterOutText

        /// <summary>
        /// Removes the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to filter out</param>
        /// <returns>The input text minus the filter text.</returns>
        public static string FilterOutText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            return string.IsNullOrEmpty(Filter) ? Input : new Regex(Filter).Replace(Input, "");
        }

        #endregion

        #region FormatString

        /// <summary>
        /// Formats a string based on a format string passed in:
        /// # = digits
        /// @ = alpha characters
        /// \ = escape char
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Format of the output string</param>
        /// <returns>The formatted string</returns>
        public static string FormatString(this string Input, string Format)
        {
            return new GenericStringFormatter().Format(Input, Format);
        }

        #endregion

        #region FromBase64

        /// <summary>
        /// Converts base 64 string based on the encoding passed in
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>string in the encoding format</returns>
        public static string FromBase64(this string Input, Encoding EncodingUsing)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            byte[] TempArray = Convert.FromBase64String(Input);
            return EncodingUsing.NullCheck(new UTF8Encoding()).GetString(TempArray);
        }

        /// <summary>
        /// Converts base 64 string to a byte array
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>A byte array equivalent of the base 64 string</returns>
        public static byte[] FromBase64(this string Input)
        {
            return string.IsNullOrEmpty(Input) ? new byte[0] : Convert.FromBase64String(Input);
        }

        #endregion

        #region IsUnicode

        /// <summary>
        /// Determines if a string is unicode
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>True if it's unicode, false otherwise</returns>
        public static bool IsUnicode(this string Input)
        {
            return string.IsNullOrEmpty(Input) ? true : Regex.Replace(Input, @"[^\u0000-\u007F]", "") != Input;
        }

        #endregion

        #region KeepFilterText

        /// <summary>
        /// Removes everything that is not in the filter text from the input.
        /// </summary>
        /// <param name="Input">Input text</param>
        /// <param name="Filter">Regex expression of text to keep</param>
        /// <returns>The input text minus everything not in the filter text.</returns>
        public static string KeepFilterText(this string Input, string Filter)
        {
            if (string.IsNullOrEmpty(Input) || string.IsNullOrEmpty(Filter))
                return "";
            Regex TempRegex = new Regex(Filter);
            MatchCollection Collection = TempRegex.Matches(Input);
            StringBuilder Builder = new StringBuilder();
            foreach (Match Match in Collection)
                Builder.Append(Match.Value);
            return Builder.ToString();
        }

        #endregion

        #region Left

        /// <summary>
        /// Gets the first x number of characters from the left hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Left(this string Input, int Length)
        {
            return string.IsNullOrEmpty(Input) ? "" : Input.Substring(0, Input.Length > Length ? Length : Input.Length);
        }

        #endregion

        #region MaskLeft

        /// <summary>
        /// Masks characters to the left ending at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="EndPosition">End position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskLeft(this string Input, int EndPosition = 4, char Mask = '#')
        {
            string Appending = "";
            for (int x = 0; x < EndPosition; ++x)
                Appending += Mask;
            return Appending + Input.Remove(0, EndPosition);
        }

        #endregion

        #region MaskRight

        /// <summary>
        /// Masks characters to the right starting at a specific character
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="StartPosition">Start position (counting from the left)</param>
        /// <param name="Mask">Mask character to use</param>
        /// <returns>The masked string</returns>
        public static string MaskRight(this string Input, int StartPosition = 4, char Mask = '#')
        {
            if (StartPosition > Input.Length)
                return Input;
            string Appending = "";
            for (int x = 0; x < Input.Length - StartPosition; ++x)
                Appending += Mask;
            return Input.Remove(StartPosition) + Appending;
        }

        #endregion

        #region NextSequence

        /// <summary>
        /// Function that is useful for generating a string in a series. so a becomes b, b becomes c, etc. 
        /// and after hitting the max character, it goes to two characters (so ~ becomes aa, then ab, ac, etc).
        /// </summary>
        /// <param name="Sequence">Current sequence</param>
        /// <returns>The next item in the sequence</returns>
        public static string NextSequence(this string Sequence, char Min = ' ', char Max = '~')
        {
            byte[] Values = Sequence.ToByteArray();
            byte MinValue = (byte)Min;
            byte MaxValue = (byte)Max;
            byte Remainder = 1;
            for (int x = Sequence.Length - 1; x >= 0; --x)
            {
                Values[x] += Remainder;
                Remainder = 0;
                if (Values[x] > MaxValue)
                {
                    Remainder = 1;
                    Values[x] = (byte)Min;
                }
                else
                    break;
            }
            if (Remainder == 1)
                return Min + Values.ToEncodedString();
            return Values.ToEncodedString();
        }

        #endregion

        #region NumericOnly

        /// <summary>
        /// Keeps only numeric characters
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="KeepNumericPunctuation">Determines if decimal places should be kept</param>
        /// <returns>the string only containing numeric characters</returns>
        public static string NumericOnly(this string Input, bool KeepNumericPunctuation = true)
        {
            return KeepNumericPunctuation ? Input.KeepFilterText(@"[0-9\.]") : Input.KeepFilterText("[0-9]");
        }

        #endregion

        #region NumberTimesOccurs

        /// <summary>
        /// returns the number of times a string occurs within the text
        /// </summary>
        /// <param name="Input">input text</param>
        /// <param name="Match">The string to match (can be regex)</param>
        /// <returns>The number of times the string occurs</returns>
        public static int NumberTimesOccurs(this string Input, string Match)
        {
            return string.IsNullOrEmpty(Input) ? 0 : new Regex(Match).Matches(Input).Count;
        }

        #endregion

        #region Pluralize

        /// <summary>
        /// Pluralizes a word
        /// </summary>
        /// <param name="Word">Word to pluralize</param>
        /// <param name="Culture">Culture info used to pluralize the word (defaults to current culture)</param>
        /// <returns>The word pluralized</returns>
        public static string Pluralize(this string Word, CultureInfo Culture = null)
        {
            if (Word.IsNullOrEmpty())
                return "";
            Culture = Culture.NullCheck(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Pluralize(Word);
        }

        #endregion

        #region RegexFormat

        /// <summary>
        /// Uses a regex to format the input string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Format">Regex string used to</param>
        /// <param name="OutputFormat">Output format</param>
        /// <param name="Options">Regex options</param>
        /// <returns>The input string formatted by using the regex string</returns>
        public static string RegexFormat(this string Input, string Format, string OutputFormat, RegexOptions Options = RegexOptions.None)
        {
            Input.ThrowIfNullOrEmpty("Input");
            return Regex.Replace(Input, Format, OutputFormat, Options);
        }

        #endregion

        #region Reverse

        /// <summary>
        /// Reverses a string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>The reverse of the input string</returns>
        public static string Reverse(this string Input)
        {
            return new string(Input.Reverse<char>().ToArray());
        }

        #endregion

        #region Right

        /// <summary>
        /// Gets the last x number of characters from the right hand side
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Length">x number of characters to return</param>
        /// <returns>The resulting string</returns>
        public static string Right(this string Input, int Length)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            Length = Input.Length > Length ? Length : Input.Length;
            return Input.Substring(Input.Length - Length, Length);
        }

        #endregion

        #region Singularize

        /// <summary>
        /// Singularizes a word
        /// </summary>
        /// <param name="Word">Word to singularize</param>
        /// <param name="Culture">Culture info used to singularize the word (defaults to current culture)</param>
        /// <returns>The word singularized</returns>
        public static string Singularize(this string Word, CultureInfo Culture = null)
        {
            if (Word.IsNullOrEmpty())
                return "";
            Culture = Culture.NullCheck(CultureInfo.CurrentCulture);
            return PluralizationService.CreateService(Culture).Singularize(Word);
        }

        #endregion

        #region StripLeft

        /// <summary>
        /// Strips out any of the characters specified starting on the left side of the input string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to string (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripLeft(this string Input, string Characters = " ")
        {
            if (Input.IsNullOrEmpty())
                return Input;
            if (Characters.IsNullOrEmpty())
                return Input;
            return Input.SkipWhile(x => Characters.Contains(x)).ToString(x => x.ToString(), "");
        }

        #endregion

        #region StripRight

        /// <summary>
        /// Strips out any of the characters specified starting on the right side of the input string (stops when a character not in the list is found)
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="Characters">Characters to string (defaults to a space)</param>
        /// <returns>The Input string with specified characters stripped out</returns>
        public static string StripRight(this string Input, string Characters = " ")
        {
            if (Input.IsNullOrEmpty())
                return Input;
            if (Characters.IsNullOrEmpty())
                return Input;
            int Position = Input.Length - 1;
            for (int x = Input.Length - 1; x >= 0; --x)
            {
                if (!Characters.Contains(Input[x]))
                {
                    Position = x + 1;
                    break;
                }
            }
            return Input.Left(Position);
        }

        #endregion

        #region ToBase64

        /// <summary>
        /// Converts from the specified encoding to a base 64 string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <param name="OriginalEncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>Bas64 string</returns>
        public static string ToBase64(this string Input, Encoding OriginalEncodingUsing = null)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            byte[] TempArray = OriginalEncodingUsing.NullCheck(new UTF8Encoding()).GetBytes(Input);
            return Convert.ToBase64String(TempArray);
        }

        #endregion

        #region ToByteArray

        /// <summary>
        /// Converts a string to a byte array
        /// </summary>
        /// <param name="Input">input string</param>
        /// <param name="EncodingUsing">The type of encoding the string is using (defaults to UTF8)</param>
        /// <returns>the byte array representing the string</returns>
        public static byte[] ToByteArray(this string Input, Encoding EncodingUsing = null)
        {
            return string.IsNullOrEmpty(Input) ? null : EncodingUsing.NullCheck(new UTF8Encoding()).GetBytes(Input);
        }

        #endregion

        #region ToFirstCharacterUpperCase

        /// <summary>
        /// Takes the first character of an input string and makes it uppercase
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with the first character capitalized</returns>
        public static string ToFirstCharacterUpperCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            char[] InputChars = Input.ToCharArray();
            for (int x = 0; x < InputChars.Length; ++x)
            {
                if (InputChars[x] != ' ' && InputChars[x] != '\t')
                {
                    InputChars[x] = char.ToUpper(InputChars[x]);
                    break;
                }
            }
            return new string(InputChars);
        }

        #endregion

        #region ToSentenceCapitalize

        /// <summary>
        /// Capitalizes each sentence within the string
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with each sentence capitalized</returns>
        public static string ToSentenceCapitalize(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string[] Seperator = { ".", "?", "!" };
            string[] InputStrings = Input.Split(Seperator, StringSplitOptions.None);
            for (int x = 0; x < InputStrings.Length; ++x)
            {
                if (!string.IsNullOrEmpty(InputStrings[x]))
                {
                    Regex TempRegex = new Regex(InputStrings[x]);
                    InputStrings[x] = InputStrings[x].ToFirstCharacterUpperCase();
                    Input = TempRegex.Replace(Input, InputStrings[x]);
                }
            }
            return Input;
        }

        #endregion

        #region ToTitleCase

        /// <summary>
        /// Capitalizes the first character of each word
        /// </summary>
        /// <param name="Input">Input string</param>
        /// <returns>String with each word capitalized</returns>
        public static string ToTitleCase(this string Input)
        {
            if (string.IsNullOrEmpty(Input))
                return "";
            string[] Seperator = { " ", ".", "\t", System.Environment.NewLine, "!", "?" };
            string[] InputStrings = Input.Split(Seperator, StringSplitOptions.None);
            for (int x = 0; x < InputStrings.Length; ++x)
            {
                if (!string.IsNullOrEmpty(InputStrings[x])
                    && InputStrings[x].Length > 3)
                {
                    Regex TempRegex = new Regex(InputStrings[x].Replace(")", @"\)").Replace("(", @"\(").Replace("*", @"\*"));
                    InputStrings[x] = InputStrings[x].ToFirstCharacterUpperCase();
                    Input = TempRegex.Replace(Input, InputStrings[x]);
                }
            }
            return Input;
        }

        #endregion

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(this string strContent, string strSplit)
        {

            if (!IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                    return new string[] { strContent };

                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
                return new string[0] { };
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <returns></returns>
        public static string[] SplitString(this string strContent, string strSplit, int count)
        {
            string[] result = new string[count];
            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < count; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }

        /// <summary>
        /// 过滤字符串数组中每个元素为合适的大小
        /// 当长度小于minLength时，忽略掉,-1为不限制最小长度
        /// 当长度大于maxLength时，取其前maxLength位
        /// 如果数组中有null元素，会被忽略掉
        /// </summary>
        /// <param name="minLength">单个元素最小长度</param>
        /// <param name="maxLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] PadStringArray(string[] strArray, int minLength, int maxLength)
        {
            if (minLength > maxLength)
            {
                int t = maxLength;
                maxLength = minLength;
                minLength = t;
            }

            int iMiniStringCount = 0;
            for (int i = 0; i < strArray.Length; i++)
            {
                if (minLength > -1 && strArray[i].Length < minLength)
                {
                    strArray[i] = null;
                    continue;
                }
                if (strArray[i].Length > maxLength)
                    strArray[i] = strArray[i].Substring(0, maxLength);

                iMiniStringCount++;
            }

            string[] result = new string[iMiniStringCount];
            for (int i = 0, j = 0; i < strArray.Length && j < result.Length; i++)
            {
                if (strArray[i] != null && strArray[i] != string.Empty)
                {
                    result[j] = strArray[i];
                    j++;
                }
            }
            return result;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <param name="maxElementLength">单个元素最大长度</param>
        /// <returns></returns>
        public static string[] SplitString(this string strContent, string strSplit, bool ignoreRepeatItem, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            return ignoreRepeatItem ? DistinctStringArray(result, maxElementLength) : result;
        }

        public static string[] SplitString(this string strContent, string strSplit, bool ignoreRepeatItem, int minElementLength, int maxElementLength)
        {
            string[] result = SplitString(strContent, strSplit);

            if (ignoreRepeatItem)
            {
                result = DistinctStringArray(result);
            }
            return PadStringArray(result, minElementLength, maxElementLength);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">被分割的字符串</param>
        /// <param name="strSplit">分割符</param>
        /// <param name="ignoreRepeatItem">忽略重复项</param>
        /// <returns></returns>
        public static string[] SplitString(this string strContent, string strSplit, bool ignoreRepeatItem)
        {
            return SplitString(strContent, strSplit, ignoreRepeatItem, 0);
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <param name="maxElementLength">字符串数组中单个元素的最大长度</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray, int maxElementLength)
        {
            Hashtable h = new Hashtable();

            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }

            string[] result = new string[h.Count];

            h.Keys.CopyTo(result, 0);

            return result;
        }

        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="strArray">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] strArray)
        {
            return DistinctStringArray(strArray, 0);
        }

        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            if (str == null || str.Trim() == string.Empty)
                return true;

            return false;
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(this string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(this string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }

        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(this List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStrNoEmpty(System.Collections.Generic.List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (IsNullOrEmpty(list[i]))
                    continue;
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(this string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }

        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }

        public static bool IsCompriseStr(string str, string stringarray, string strsplit)
        {
            if (StringExtensions.IsNullOrEmpty(stringarray))
                return false;

            str = str.ToLower();
            string[] stringArray = StringExtensions.SplitString(stringarray.ToLower(), strsplit);
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (str.IndexOf(stringArray[i]) > -1)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(this string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                        return i;
                }
                else if (strSearch == stringArray[i])
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>		
        public static int GetInArrayID(this string strSearch, string[] stringArray)
        {
            return GetInArrayID(strSearch, stringArray, true);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string strSearch, string[] stringArray, bool caseInsensetive)
        {
            return GetInArrayID(strSearch, stringArray, caseInsensetive) >= 0;
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">字符串数组</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string[] stringarray)
        {
            return InArray(str, stringarray, false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringarray)
        {
            return InArray(str, SplitString(stringarray, ","), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringarray, string strsplit)
        {
            return InArray(str, SplitString(stringarray, strsplit), false);
        }

        /// <summary>
        /// 判断指定字符串是否属于指定字符串数组中的一个元素
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="stringarray">内部以逗号分割单词的字符串</param>
        /// <param name="strsplit">分割字符串</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>判断结果</returns>
        public static bool InArray(this string str, string stringarray, string strsplit, bool caseInsensetive)
        {
            return InArray(str, SplitString(stringarray, strsplit), caseInsensetive);
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(this string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(this string str)
        {
            Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);
            Match m = null;

            for (m = RegexBr.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }
            return str;
        }

        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(this string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                        startIndex = startIndex - length;
                }

                if (startIndex > str.Length)
                    return "";
            }
            else
            {
                if (length < 0)
                    return "";
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                        return "";
                }
            }

            if (str.Length - startIndex < length)
                length = str.Length - startIndex;

            return str.Substring(startIndex, length);
        }

        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(this string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }

        /// <summary>
        /// 进行指定的替换
        /// </summary>
        public static string StrFilter(this string str, string bantext)
        {
            string text1 = "", text2 = "";
            string[] textArray1 = SplitString(bantext, "\r\n");
            for (int num1 = 0; num1 < textArray1.Length; num1++)
            {
                text1 = textArray1[num1].Substring(0, textArray1[num1].IndexOf("="));
                text2 = textArray1[num1].Substring(textArray1[num1].IndexOf("=") + 1);
                str = str.Replace(text1, text2);
            }
            return str;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(this string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        public static string GetUnicodeSubString(this string str, int len, string p_TailString)
        {
            str = str.TrimEnd();
            string result = string.Empty;// 最终返回的结果
            int byteLen = System.Text.Encoding.Default.GetByteCount(str);// 单字节字符长度
            int charLen = str.Length;// 把字符平等对待时的字符串长度
            int byteCount = 0;// 记录读取进度
            int pos = 0;// 记录截取位置
            if (byteLen > len)
            {
                for (int i = 0; i < charLen; i++)
                {
                    if (Convert.ToInt32(str.ToCharArray()[i]) > 255)// 按中文字符计算加2
                        byteCount += 2;
                    else// 按英文字符计算加1
                        byteCount += 1;
                    if (byteCount > len)// 超出时只记下上一个有效位置
                    {
                        pos = i;
                        break;
                    }
                    else if (byteCount == len)// 记下当前位置
                    {
                        pos = i + 1;
                        break;
                    }
                }

                if (pos >= 0)
                    result = str.Substring(0, pos) + p_TailString;
            }
            else
                result = str;

            return result;
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(this string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        public static string ReplaceString(this string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string GetSpacesString(this int spacesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>合并到的目的字符串</returns>
        public static string MergeString(this string source, string target)
        {
            return MergeString(source, target, ",");
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <param name="source">要合并的源字符串</param>
        /// <param name="target">要被合并到的目的字符串</param>
        /// <param name="mergechar">合并符</param>
        /// <returns>并到字符串</returns>
        public static string MergeString(this string source, string target, string mergechar)
        {
            if (StringExtensions.IsNullOrEmpty(target))
                target = source;
            else
                target += mergechar + source;

            return target;
        }

        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(this int bytes)
        {
            if (bytes > 1073741824)
                return ((double)(bytes / 1073741824)).ToString("0") + "G";

            if (bytes > 1048576)
                return ((double)(bytes / 1048576)).ToString("0") + "M";

            if (bytes > 1024)
                return ((double)(bytes / 1024)).ToString("0") + "K";

            return bytes.ToString() + "Bytes";
        }

        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time">参与比较的时间</param>
        /// <param name="Sec">将指定秒数增加到参与比较的时间上</param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(this string time, int Sec)
        {
            if (StringExtensions.IsNullOrEmpty(time))
                return 1;
            DateTime dateTime = DateTimeExtensions.StrToDateTime(time, DateTime.Parse("1900-01-01"));
            if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
                return 1;

            TimeSpan ts = DateTime.Now - dateTime.AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
                return int.MaxValue;

            else if (ts.TotalSeconds < int.MinValue)
                return int.MinValue;

            return (int)ts.TotalSeconds;
        }

        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(this string time, int minutes)
        {
            if (StringExtensions.IsNullOrEmpty(time))
                return 1;
            DateTime dateTime = DateTimeExtensions.StrToDateTime(time, DateTime.Parse("1900-01-01"));
            if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
                return 1;

            TimeSpan ts = DateTime.Now - dateTime.AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
                return int.MaxValue;
            else if (ts.TotalMinutes < int.MinValue)
                return int.MinValue;

            return (int)ts.TotalMinutes;
        }

        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(this string time, int hours)
        {
            if (StringExtensions.IsNullOrEmpty(time))
                return 1;

            DateTime dateTime = DateTimeExtensions.StrToDateTime(time, DateTime.Parse("1900-01-01"));
            if (dateTime.ToString("yyyy-MM-dd") == "1900-01-01")
                return 1;

            TimeSpan ts = DateTime.Now - dateTime.AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
                return int.MaxValue;
            else if (ts.TotalHours < int.MinValue)
                return int.MinValue;

            return (int)ts.TotalHours;
        }

        /// <summary>
        /// 为脚本替换特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(this string str)
        {
            return str.Replace("\\", "\\\\").Replace("'", "\\'").Replace("\"", "\\\"");
        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(this string str)
        {
            //A-Z, a-z, 0-9, +, /, =
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(this string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(this string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        /// <summary>
        /// 清理字符串
        /// </summary>
        public static string CleanInput(this string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }

        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetFilename(this string url)
        {
            if (url == null)
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }

        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrFormat(this string str)
        {
            string str2;

            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }

        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 返回指定日期格式
        /// </summary>
        public static string GetDate(this string datetimestr, string replacestr)
        {
            if (datetimestr == null)
                return replacestr;

            if (datetimestr.Equals(""))
                return replacestr;

            try
            {
                datetimestr = Convert.ToDateTime(datetimestr).ToString("yyyy-MM-dd").Replace("1900-01-01", replacestr);
            }
            catch
            {
                return replacestr;
            }
            return datetimestr;
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间 
        /// </sumary>
        public static string GetStandardDateTime(this string fDateTime, string formatStr)
        {
            if (fDateTime == "0000-0-0 0:00:00")
                return fDateTime;
            DateTime time = new DateTime(1900, 1, 1, 0, 0, 0, 0);
            if (DateTime.TryParse(fDateTime, out time))
                return time.ToString(formatStr);
            else
                return "N/A";
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd HH:mm:ss
        /// </sumary>
        public static string GetStandardDateTime(this string fDateTime)
        {
            return GetStandardDateTime(fDateTime, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 返回标准时间 yyyy-MM-dd
        /// </sumary>
        public static string GetStandardDate(this string fDate)
        {
            return GetStandardDateTime(fDate, "yyyy-MM-dd");
        }

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(this string str)
        {
            return (str == "") ? "" : str.Substring(0, str.Length - 1);
        }

        /// <summary>
        /// 根据字符串获取枚举值
        /// </summary>
        /// <typeparam name="T">枚举类型</typeparam>
        /// <param name="value">字符串枚举值</param>
        /// <param name="defValue">缺省值</param>
        /// <returns></returns>
        public static T GetEnum<T>(string value, T defValue)
        {
            try
            {
                return (T)Enum.Parse(typeof(T), value, true);
            }
            catch (ArgumentException)
            {
                return defValue;
            }
        }

        /// <summary>
        /// 将8位日期型整型数据转换为日期字符串数据
        /// </summary>
        /// <param name="date">整型日期</param>
        /// <param name="chnType">是否以中文年月日输出</param>
        /// <returns></returns>
        public static string FormatDate(int date, bool chnType)
        {
            string dateStr = date.ToString();

            if (date <= 0 || dateStr.Length != 8)
                return dateStr;

            if (chnType)
                return dateStr.Substring(0, 4) + "年" + dateStr.Substring(4, 2) + "月" + dateStr.Substring(6) + "日";
            return dateStr.Substring(0, 4) + "-" + dateStr.Substring(4, 2) + "-" + dateStr.Substring(6);
        }

        public static string FormatDate(int date)
        {
            return FormatDate(date, false);
        }

        /// <summary>
        /// 将全角数字转换为数字
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }

        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "\\\b");
            sourceStr = sourceStr.Replace("\t", "\\\t");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\n", "\\\n");
            sourceStr = sourceStr.Replace("\f", "\\\f");
            sourceStr = sourceStr.Replace("\r", "\\\r");
            return sourceStr.Replace("\"", "\\\"");
        }

        #region 补足位数
        /// <summary>
        /// 指定字符串的固定长度，如果字符串小于固定长度，
        /// 则在字符串的前面补足零，可设置的固定长度最大为9位
        /// </summary>
        /// <param name="text">原始字符串</param>
        /// <param name="limitedLength">字符串的固定长度</param>
        public static string RepairZero(string text, int limitedLength)
        {
            //补足0的字符串
            string temp = "";

            //补足0
            for (int i = 0; i < limitedLength - text.Length; i++)
            {
                temp += "0";
            }

            //连接text
            temp += text;

            //返回补足0的字符串
            return temp;
        }
        #endregion

        #region 各进制数间转换
        /// <summary>
        /// 实现各进制数间的转换。ConvertBase("15",10,16)表示将十进制数15转换为16进制的数。
        /// </summary>
        /// <param name="value">要转换的值,即原值</param>
        /// <param name="from">原值的进制,只能是2,8,10,16四个值。</param>
        /// <param name="to">要转换到的目标进制，只能是2,8,10,16四个值。</param>
        public static string ConvertBase(string value, int from, int to)
        {
            try
            {
                int intValue = Convert.ToInt32(value, from);  //先转成10进制
                string result = Convert.ToString(intValue, to);  //再转成目标进制
                if (to == 2)
                {
                    int resultLength = result.Length;  //获取二进制的长度
                    switch (resultLength)
                    {
                        case 7:
                            result = "0" + result;
                            break;
                        case 6:
                            result = "00" + result;
                            break;
                        case 5:
                            result = "000" + result;
                            break;
                        case 4:
                            result = "0000" + result;
                            break;
                        case 3:
                            result = "00000" + result;
                            break;
                    }
                }
                return result;
            }
            catch
            {
                return "0";
            }
        }
        #endregion

        #endregion
    }
}