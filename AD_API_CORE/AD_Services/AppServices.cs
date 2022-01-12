using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AD_Services
{
    public class AppServices
    {
        public static bool IsAlphabets(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
                return false;

            for (int i = 0; i < inputString.Length; i++)
                if (!char.IsLetter(inputString[i]))
                    return false;
            return true;
        }

        public static bool IsEmail(string inputString)
        {

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(inputString);
            if (match.Success)
                return true;
            else
                return false;
        }


        public static bool IsMin8Char(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return false;
            }
                

            if (inputString.Length < 8)
            {
                return false;
            }
            return true;
        }

        public static string GetGuidString()
        {
            Guid guid = Guid.NewGuid();
            string str = guid.ToString();
            return str;
        }

        public static string GetGuidSmallString()
        {
            StringBuilder builder = new StringBuilder();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(11)
                .ToList().ForEach(e => builder.Append(e));
            string id = builder.ToString();
            return id;
        }
    }
}
