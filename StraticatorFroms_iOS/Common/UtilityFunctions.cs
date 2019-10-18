using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using Straticator.LocalizationConverter;

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using StraticatorFroms_iOS;
using StraticatorFroms_iOS;

namespace Straticator.Common
{
    class UtilityFunctions
    {
        internal static void ShowErrorMessage(ErrorCodes errorCodes, string errorText = null, string[] runtimeValues = null)
        {
            string error = ChangeCulture.Lookup("OperationUnsuccessfulMsg") + ' ' + ChangeCulture.Lookup(errorCodes.ToString());
            if (errorText != null)
                error += " " + errorText;
            if (runtimeValues != null)
                error = string.Format(error, runtimeValues);

            string title = "Error";
            App.Current.MainPage.DisplayAlert(title, error, ChangeCulture.Lookup("OK"));
        }

        public static string SetOrderConfirmationMsg(OrderResult oResult, bool showAcceptOrder = false, int OrderVolume = 0)
        {
            string strResult;
            if (oResult.Price != 0)
            {
                MarketInfo m = MarketInfo.getMarketInfo(oResult.SymbolId);
                NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
                string tradeConfirmation=ChangeCulture.Lookup("TradeConfirmationInfo");
                if (OrderVolume == 0)
                {
                    strResult = string.Format(tradeConfirmation,
                        oResult.Volume > 0 ? ChangeCulture.Lookup("MPBuy") : ChangeCulture.Lookup("MPSell"), m.Symbol, Math.Abs(oResult.Volume).ToString("N0", nf),
                        SymbPrice.Format(oResult.SymbolId, oResult.Price), oResult.Time.ToLocalTime(), oResult.Oid, oResult.Aid);
                }
                else
                {
                    string str = string.Format(ChangeCulture.Lookup("OrderConfirmationDiff"), OrderVolume, Math.Abs(oResult.Volume), m.Symbol);
                    string str1 = string.Format(tradeConfirmation,
                       oResult.Volume > 0 ? ChangeCulture.Lookup("MPBuy") : ChangeCulture.Lookup("MPSell"), m.Symbol, Math.Abs(oResult.Volume).ToString("N0", nf),
                       SymbPrice.Format(oResult.SymbolId, oResult.Price), oResult.Time.ToLocalTime(), oResult.Oid, oResult.Aid);
                    strResult = str + Environment.NewLine + str1;
                }
                
            }
            else
            {
                if (showAcceptOrder)
                    strResult = string.Format(ChangeCulture.Lookup("OrderAcceptedConfirmation"), oResult.Oid);
                else
                    strResult = string.Format(ChangeCulture.Lookup("OrderConfirmationInfo"), ChangeCulture.Lookup("OrderStatus"), oResult.Oid, oResult.Aid);
            }
            return strResult;
        }

        public static string PascalToSpace(string inputString)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("([A-Z]+[a-z]+)");
            string[] arr = reg.Split(inputString);
            string result = string.Empty;
            foreach (string s in arr)
            {
                if (s != string.Empty)
                {
                    result += s + ' ';
                }
            }
            return result;
        }
        //public static Color GetColorFromHexa(string hexaColor)
        //{
        //    try
        //    {
        //        return Color.FromArgb(
        //                  Convert.ToByte(hexaColor.Substring(1, 2), 16),
        //                  Convert.ToByte(hexaColor.Substring(3, 2), 16),
        //                  Convert.ToByte(hexaColor.Substring(5, 2), 16),
        //                  Convert.ToByte(hexaColor.Substring(7, 2), 16)
        //              );
        //    }
        //    catch
        //    {

        //        byte a = (byte)(Convert.ToInt32(255 * 1.0));
        //        return Color.FromArgb(a, Convert.ToByte(hexaColor.Substring(1, 2), 16),
        //                      Convert.ToByte(hexaColor.Substring(3, 2), 16),
        //                      Convert.ToByte(hexaColor.Substring(5, 2), 16)

        //                  );
        //    }

        //}
    }
}
