using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CommonTools
{
    public class RegexHelper
    {
        //邮编
        public static bool CheckZip(string value)
        {
            string pattern = @"^[1-9]\d{5}$";
            bool isMatch = Regex.IsMatch(value, pattern);
            
            return isMatch;
        }

        //电话号码
        public static bool CheckTelPhone(string value)
        {
            string pattern = @"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}(-\d{1,4})?$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //手机号码
        public static bool CheckCellPhone(string value)
        {
            string pattern = @"^1{1}\d{10}$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //车牌号
        public static bool CheckCarNumber(string value)
        {
            string pattern = @"^[\u4e00-\u9fa5]{1}[A-Z]{1}[A-Z_0-9]{5}$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //字母（大小写）、数字、下划线
        public static bool CheckNumbersCharactersUnderLine(string value)
        {
            string pattern = @"^[a-zA-Z0-9_]+$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //字母（大小写）和数字
        public static bool CheckNumbersAndCharacters(string value)
        {
            string pattern = @"^[a-zA-Z0-9]+$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //字母（大小写）
        public static bool CheckCharacters(string value)
        {
            string pattern = @"^[a-zA-Z]+$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //数字
        public static bool CheckNumbers(string value)
        {
            string pattern = @"^[0-9]+$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //IP
        public static bool CheckIP(string value)
        {
            string pattern = @"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

        //HostName主机名
        public static bool CheckHostname(string value)
        {
            string pattern = @"^(([a-zA-Z0-9]|[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9])\.)*([A-Za-z0-9]|[A-Za-z0-9][A-Za-z0-9\-]*[A-Za-z0-9])$";
            bool isMatch = Regex.IsMatch(value, pattern);

            return isMatch;
        }

    }
}
