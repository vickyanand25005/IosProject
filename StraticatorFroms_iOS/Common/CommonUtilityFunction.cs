using Straticator.LocalizationConverter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace StraticatorFroms_iOS.Common
{
    public class CommonUtilityFunction
    {
        public static bool WhiteSpaceRemoved(string input)
        {

            string pattern = "\\s+";
            string replacement = string.Empty;
            Regex rgx = new Regex(pattern);
            if (rgx.IsMatch(input))
            {
                return true;
            }
            return false;
        }
        public static bool CheckforSpecialCharacters(string input)
        {
            Regex rgx = new Regex("^[0-9a-zA-Z ]+$");
            if (rgx.IsMatch(input))
            {
                return true;
            }
            return false;
        }
        static public bool EmailValidation(string input, out string errorMessage)
        {
            errorMessage = string.Empty;
            string expression = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            if (!Regex.IsMatch(input, expression))
            {
                errorMessage = ChangeCulture.Lookup("EmailFormatkey");
                return false;
            }
            return true;
        }
    }
}
