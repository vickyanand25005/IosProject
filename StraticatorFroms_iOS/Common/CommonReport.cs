using Straticator.LocalizationConverter;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Globalization;
using Xamarin.Forms;
using StraticatorFroms_iOS;

namespace Straticator.Common
{
    public class CommonReport
    {
        static Dictionary<string, short> DDSymbList;
        static public string sysUIFormat;
        static public string sysUIFormatTime;
        static public DateTime getToDate(DateTime dtToDate)
        {
            if (dtToDate.Date == null )
                return DateTime.MinValue;

            // if user enters from 1/5/2013 to 31/5-2013 then it is because he want all the transactions in May
            // eg, the todate contains the date ifself. Therefore we have to select that last time in the day. 
            // we do that by add time so time will be 23:59:59
            DateTime res = Convert.ToDateTime(dtToDate.Date);

            // DateTime res = CommonReport.getDateObj(dtToDate.Text);
            return res.AddSeconds(60 * 60 * 24 - 1).ToUniversalTime();
        }

        public static short getSymbol(CustomPickerRenderer cbSymbols)
        {
            if (cbSymbols == null)
                return 0;

            var sym = cbSymbols.SelectedItem.ToString();
            if (sym == null)
                return 0;

            short selectedsymb = 0;
            DDSymbList.TryGetValue(sym, out selectedsymb);
            return selectedsymb;
        }

        public static void loadSymbols(ref CustomPickerRenderer cb)
        {
            List<string> Items = new List<string>();
            Items.Add(ChangeCulture.Lookup("ShowAll"));
            DDSymbList = new Dictionary<string, short>();
            DDSymbList.Add(ChangeCulture.Lookup("ShowAll"), 0);
            foreach (var price in Common.SessionManager.Instance.Session.PriceList)
            {
                if (price.RemoveInPriceList)
                    continue;
                Items.Add(price.Symbol);
                DDSymbList.Add(price.Symbol, price.SymbolId);
            }
            cb.ItemsSource = Items;
            cb.SelectedIndex =0;
        }

        public static void GetDateFormat()
        {
            System.Threading.Thread threadForCulture = new System.Threading.Thread(delegate() { });
            sysUIFormat = threadForCulture.CurrentCulture.DateTimeFormat.ShortDatePattern;
            sysUIFormatTime = threadForCulture.CurrentCulture.DateTimeFormat.LongTimePattern;
        }
    }
}
