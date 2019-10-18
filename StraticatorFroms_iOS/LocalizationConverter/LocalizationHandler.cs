
using Straticator.LocalizationConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Straticator.LocalizationConverter
{
    public class LocalizationHandler
    {
        //ChangeColumnHeader
        public static void LocalizeGridHeader(ref string[] tempheader)
        {
            for(int i=0;i<tempheader.Length;i++)
            {
                if (!string.IsNullOrWhiteSpace(tempheader[i]))
                    tempheader[i] = ChangeCulture.Lookup(tempheader[i]).Replace(":", "");
            }
        }

        public static string LocalizeAppBar(string textkey)
        {
            return ChangeCulture.Lookup(textkey).ToLower() ;   
        }
    }
}
