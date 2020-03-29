

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace TlhPlatform.Core.Extensions
{
    /// <summary>
    /// StringBuilder 扩展方法类
    /// </summary>
    public static class StringBuilderExtensions
    {
        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder TrimStart(this StringBuilder sb)
        {
            return sb.TrimStart(' ');
        }

        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的指定<seealso cref="char"/>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c">要去掉的<seealso cref="char"/></param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, char c)
        {
            sb.CheckNotNull("sb");
            if (sb.Length == 0)
                return sb;
            while (c.Equals(sb[0]))
            {
                sb.Remove(0, 1);
            }
            return sb;
        }

        /// <summary>
        /// 去除<seealso cref="StringBuilder"/>开头的指定字符数组
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="cs">要去掉的字符数组</param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, char[] cs)
        {
            cs.CheckNotNull("chars");
            return sb.TrimStart(new string(cs));
        }
        /// <summary>
        /// 去除<see cref="StringBuilder"/>开头的指定的<seealso cref="string"/>
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="str">要去掉的<seealso cref="string"/></param>
        /// <returns></returns>
        public static StringBuilder TrimStart(this StringBuilder sb, string str)
        {
            sb.CheckNotNull("sb");
            if (string.IsNullOrEmpty(str)
                || sb.Length == 0
                || str.Length > sb.Length)
                return sb;
            while (sb.SubString(0, str.Length).Equals(str))
            {
                sb.Remove(0, str.Length);
                if (str.Length>sb.Length)
                {
                    break;
                }
            }
            return sb;
        }

        /// <summary>
        /// 去除StringBuilder结尾的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder TrimEnd(this StringBuilder sb)
        {
            return sb.TrimEnd(' ');
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c">要去掉的字符</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, char c)
        {
            sb.CheckNotNull("sb");
             if (sb.Length == 0)
                return sb;
            while (c.Equals(sb[sb.Length - 1]))
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb;
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符数组
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="chars">要去除的字符数组</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, char[] chars)
        {
            chars.CheckNotNull("chars");
            return sb.TrimEnd(new string(chars));
        }

        /// <summary>
        /// 去除<see cref="StringBuilder"/>结尾指定字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="str">要去除的字符串</param>
        /// <returns></returns>
        public static StringBuilder TrimEnd(this StringBuilder sb, string str)
        {
            sb.CheckNotNull("sb");
            if (string.IsNullOrEmpty(str)
                || sb.Length == 0
                || str.Length > sb.Length)
                return sb;
            while (sb.SubString(sb.Length-str.Length, str.Length).Equals(str))
            {
                sb.Remove(sb.Length-str.Length, str.Length);
                if (sb.Length<str.Length)
                {
                    break;
                }
            }
            return sb;
        }

        /// <summary>
        /// 去除StringBuilder两端的空格
        /// </summary>
        /// <param name="sb">StringBuilder</param>
        /// <returns>返回修改后的StringBuilder，主要用于链式操作</returns>
        public static StringBuilder Trim(this StringBuilder sb)
        {
            sb.CheckNotNull("sb");
            if (sb.Length == 0)
                return sb;
            return sb.TrimEnd().TrimStart();
        }

        /// <summary>
        /// 返回<see cref="StringBuilder"/>从起始位置指定长度的字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="start">起始位置</param>
        /// <param name="length">长度</param>
        /// <returns>字符串</returns>
        /// <exception cref="OverflowException">超出字符串索引长度异常</exception>
        public static string SubString(this StringBuilder sb, int start, int length)
        {
            sb.CheckNotNull("sb");
            if (start + length > sb.Length)
                throw new IndexOutOfRangeException("超出字符串索引长度");
            char[] cs = new char[length];
            for (int i = 0; i < length; i++)
            {
                cs[i] = sb[start + i];
            }
            return new string(cs);
        }


        public static string Truncate(this string value, int maxLength, string suffix = "...")
        {
            if (string.IsNullOrEmpty(value))
                return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength) + suffix;
        }


        /// <summary>
        ///     替换空格字符
        /// </summary>
        /// <param name="input"></param>
        /// <param name="replacement">替换为该字符</param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceWhitespace(this string input, string replacement = "")
        {
            return string.IsNullOrEmpty(input) ? null : Regex.Replace(input, "\\s", replacement, RegexOptions.Compiled);
        }

        /// <summary>
        ///     返回一个值，该值指示指定的 String 对象是否出现在此字符串中。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value">要搜寻的字符串。</param>
        /// <param name="comparisonType">指定搜索规则的枚举值之一。</param>
        /// <returns>如果 value 参数出现在此字符串中则为 true；否则为 false。</returns>
        public static bool Contains(this string source, string value,
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            return source.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        ///     清除 Html 代码，并返回指定长度的文本。(连续空行或空格会被替换为一个)
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength">返回的文本长度（为0返回所有文本）</param>
        /// <returns></returns>
        public static string StripHtml(this string text, int maxLength = 0)
        {
            if (string.IsNullOrEmpty(text)) return string.Empty;
            text = text.Trim();

            text = Regex.Replace(text, "[\\r\\n]{2,}", "<&rn>"); //替换回车和换行为<&rn>，防止下一行代码替换空格的时候被替换掉
            text = Regex.Replace(text, "[\\s]{2,}", " "); //替换 2 个以上的空格为 1 个
            text = Regex.Replace(text, "(<&rn>)+", "\n"); //还原 <&rn> 为 \n
            text = Regex.Replace(text, "(\\s*&[n|N][b|B][s|S][p|P];\\s*)+", " "); //&nbsp;

            text = Regex.Replace(text, "(<[b|B][r|R]/*>)+|(<[p|P](.|\\n)*?>)", "\n"); //<br>
            text = Regex.Replace(text, "<(.|\n)+?>", " ", RegexOptions.IgnoreCase); //any other tags

            if (maxLength > 0 && text.Length > maxLength)
                text = text.Substring(0, maxLength);

            return text;
        }
    }
}
