using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;


namespace Straticator.LocalizationConverter
{
    public class ChangeCulture
    {
        static Dictionary<string, string> LocalizationStrings;
        static Dictionary<string, string[]> LocalizationEnums;
        public static void SetLocalizationStrings(string languageCode)
        {
            LocalizationStrings = new Dictionary<string, string>();
            ResourceManager rm = null;
            if (languageCode == "da")
            {
                rm = new ResourceManager(typeof(StraticatorFroms_iOS.Assets.DaResource));
            }
            else
            {
                rm = new ResourceManager(typeof(StraticatorFroms_iOS.Assets.EnResource));
            }

            ResourceSet resourceSet = rm.GetResourceSet(System.Globalization.CultureInfo.InvariantCulture, true, true);


            try
            {

                foreach (DictionaryEntry entry in resourceSet)
                {
                    string resourceKey = entry.Key.ToString();
                    string resource = entry.Value.ToString();

                    LocalizationStrings.Add(resourceKey, resource);
                }

                LocalizationEnums = new Dictionary<string, string[]>();
            }
            catch
            {
            }
        }

        static public string Lookup(string key)
        {
            string text;
            if (LocalizationStrings.TryGetValue(key, out text))
                return text;
            else
                return key;
        }

        static public string[] LookupEnum(string key)
        {
            LocalizationEnums = new Dictionary<string, string[]>();
            string[] enumList;
            if (LocalizationEnums.TryGetValue(key, out enumList))
                return enumList;

            enumList = ChangeCulture.Lookup(key).Split('#');
            LocalizationEnums.Add(key, enumList);
            return enumList;
        }

        static public string lookupEnum(string key, object value)
        {

            int n = Convert.ToInt32(value);
            string[] texts = LookupEnum(key);
            if (n < texts.Length)
                return texts[n];
            else
                return string.Empty;
        }
    }
}
