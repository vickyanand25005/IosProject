using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//using Newtonsoft.Json;
using LiveChartTrader.Common;
using LiveChartTrader.IndicatorDataModel;
using StraticatorAPI;
using Xamarin.Essentials;

namespace Straticator.Common
{
    //E:\Repose\StraticatorFroms_iOS\StraticatorFroms_iOS\StraticatorFroms_iOS\bin\Debug\netstandard2.0\Mono.Android.dll E:\App16\App16\App16\App16\bin\Newtonsoft.Json.dll
    class IsolatedStorage
    {
        public static ISKeys UserData = new ISKeys();


        //Same as in WP
        public static void SetRemovePriceList()
        {
            List<string> symbols = null;

            string result = Preferences.Get("RemovedSymbols", "");
            if (result != "")
            {
                //symbols = JsonConvert.DeserializeObject<List<string>>(result);
                //var pl = Common.SessionManager.Instance.Session.PriceList;
                //if (pl != null)
                //{
                //    foreach (var price in pl)
                //        price.RemoveInPriceList = symbols.Contains(price.Symbol);
                //}
            }


        }

        //Same as in WP
        public static void SaveUnusedSymbols(List<string> symbols)
        {
            //var result = JsonConvert.SerializeObject(symbols);
            // Preferences.Set("RemovedSymbols", result);
            //SetRemovePriceList();
        }

        //Same as in WP
        public static void SaveLoginCredentials(string username, bool isLive)
        {
            //string result =  Preferences.Get("LoginCredentials", "");
            //if (result != "")
            //{
            //    Preferences.Remove("LoginCredentials");
            //}
            //LoginCredentials logindata = new LoginCredentials();
            //logindata.Username = username;
            //logindata.LoginType = isLive ? (byte)LoginType.Live : (byte)LoginType.Demo;
            //var result2 = JsonConvert.SerializeObject(logindata);
            // Preferences.Set("LoginCredentials", result2);

        }

        //Same as in WP
        public static LoginCredentials GetLoginCredentials()
        {
            //string result =  Preferences.Get("LoginCredentials", "");
            //if (result != "")
            //{
            //    var LoginCredentials = JsonConvert.DeserializeObject<LoginCredentials>(result);
            //    return LoginCredentials;
            //}
            return null;
        }

        //Same as in WP
        public static void SetNavigation()
        {
            //var result = JsonConvert.SerializeObject(NavigatePages.GetNavigation());
            // Preferences.Set("lastPage", result);
            

        }

        //Same as in WP
        public static void RemoveNavigation()
        {
            string result =  Preferences.Get("lastPage", "");
            if (result != "")
            {
                Preferences.Remove("lastPage");
                
            }

        }

        //Same as in WP
        public static List<NavigationData> GetNavigation()
        {
            //string result =  Preferences.Get("lastPage", "");
            //if (result != "")
            //{
            //    var objNavigationData = JsonConvert.DeserializeObject<List<NavigationData>>(result);
            //    return objNavigationData;
            //}
            return null;
        }

        //Same as in WP
        public static void SaveSession(SessionManager sm)
        {

            try
            {
                Session s = sm.Session;
                if (s == null || s.User == null)
                    return;
                UserData.username = s.LoginId;
                UserData.loginType = s.LiveLogin ? (byte)LoginType.Live : (byte)LoginType.Demo;
                UserData.sessiondata = (byte[][])s.SessionData;
                UserData.chartSymbol = sm.CurrentChartSymbol;
                UserData.CurrentAccount = sm.CurrentAccount;


                string result =  Preferences.Get("SessionKey", "");
                if (result != "")
                {
                    Preferences.Remove("SessionKey");
                }

                //result = JsonConvert.SerializeObject(UserData);
                //Preferences.Set("SessionKey", result);
                
            }
            catch
            { }
        }

        //Same as in WP
        public static ISKeys GetSession()
        {

            string result =  Preferences.Get("SessionKey", "");
            if (result != "")
            {
                //var objSessionKeydata = JsonConvert.DeserializeObject<ISKeys>(result);
                //return objSessionKeydata;
            }
            return null;
        }

        //Same as in WP
        public static bool GetIsLive()
        {
            string result =  Preferences.Get("SessionKey", "");
            if (result != "")
            {
                //ISKeys ob = JsonConvert.DeserializeObject<ISKeys>(result);
                //return Convert.ToBoolean(ob.loginType);
            }
            return false;
        }

        //Same as in WP
        public static void RemoveSession()
        {
            string result =  Preferences.Get("SessionKey", "");
            if (result != "")
            {
                Preferences.Remove("SessionKey");
            }
            
        }

        //Same as in WP
        public static void SaveChartSetting(string data)
        {
            string result =  Preferences.Get("ChartSetting", "");
            if (result != "")
            {
                Preferences.Remove("ChartSetting");
            }
            ChartSettingGetChartSetting ch = new ChartSettingGetChartSetting();
            ch.strsettings = data;
            //result = JsonConvert.SerializeObject(ch);
            // Preferences.Set("ChartSetting", result);
            
        }

        //Same as in WP
        public static string GetChartSetting()
        {
            string result =  Preferences.Get("ChartSetting", "");
            if (result != "")
            {
                //var objChartSetting = JsonConvert.DeserializeObject<ChartSettingGetChartSetting>(result);
                //return objChartSetting.strsettings;
            }
            return null;
        }

       // Same as in WP
        public static void RemoveChartSetting()
        {
            string result =  Preferences.Get("ChartSetting", "");
            if (result != "")
            {
                Preferences.Remove("ChartSetting");
            }
            
        }

       /* //Same as in WP
        public static void SaveCandleChartSetting(ChartSetting ob)
        {
            string result =  Preferences.Get("CandleSetting", "");
            if (result != "")
            {
                settings_editor.Remove("CandleSetting");
            }
            result = JsonConvert.SerializeObject(ob);
             Preferences.Set("CandleSetting", result);
            settings_editor.Commit();
        }

        //Same as in WP
        public static ChartSetting GetCandleChartSettings()
        {
            string result =  Preferences.Get("CandleSetting", "");
            if (result != "")
            {
                var objCandleSetting = JsonConvert.DeserializeObject<ChartSetting>(result);
                return objCandleSetting;
            }
            return null;
        }*/

        public static void SaveCandleChartSetting(int numofSecond)
        {
            string result =  Preferences.Get("CandleSetting", "");
            if (result != "")
            {
                Preferences.Remove("CandleSetting");
            }

            ChartSetting objChartSetting = new ChartSetting();
            objChartSetting.numofSecond = numofSecond;
            //result = JsonConvert.SerializeObject(objChartSetting);
            // Preferences.Set("CandleSetting", result);
            
        }

        public static int GetCandleChartSettings()
        {
            string result =  Preferences.Get("CandleSetting", "");
            if (result != "")
            {
                //var objCandleSetting = JsonConvert.DeserializeObject<ChartSetting>(result);
                //return objCandleSetting.numofSecond;
            }
            return 0;
        }
    }


    /* Add by Differenz B.coz we can not directly Deserialize JSON object in to string or int */
    public class ChartSetting
    {
        public ChartSetting()
        { }
        public int numofSecond;

    }

    public class ChartSettingGetChartSetting
    {
        public ChartSettingGetChartSetting()
        {
        }
        public string strsettings;

    }

    //Same as in WP
    public class ISKeys
    {
        public string username;
        public byte[][] sessiondata;
        public int CurrentAccount;
        public short chartSymbol;
        public byte loginType;
    }


    //Same as in WP
    public class LoginCredentials
    {
        public string Username;
        public byte LoginType;
    }


    /*
    //in other file
    //Same as in WP
    public class ChartSetting
    {
        public int frame;
        public int pricetype;
        public TimeFrameResolution frameresolution;
        public bool Isfrmdate;
        public DateTime date;


    }
    //Same as in WP
    public class NavigationData
    {
        public NavigationData()
        {

        }
        public string Uri;
        public short SymbolId;
    }

    //Same as in WP
    class NavigatePages
    {
        static List<NavigationData> _pages;
        static NavigatePages()
        {
            _pages = new List<NavigationData>();
            SetNavigationPages();

        }
        public static void SetNavigationPages()
        {
            if (IsolatedStorage.GetNavigation() != null)
                _pages = IsolatedStorage.GetNavigation();
        }

        public static void SetNavigatePage(string uri, short symbol = 0)
        {
            if (_pages.Count != 0 && uri == _pages[_pages.Count - 1].Uri)
                return;
            _pages.Add(new NavigationData() { Uri = uri, SymbolId = symbol });

        }

        public static NavigationData BackPage()
        {
            if (_pages.Count > 0)
            {
                if (_pages.Count == 1)
                    return null;
                _pages.RemoveAt(_pages.Count - 1);
                return _pages.Last();
            }
            else
                return null;
        }

        public static NavigationData ResumePage()
        {
            if (_pages.Count > 0)
            {
                return _pages.Last();
            }
            else
                return null;
        }
        public static List<NavigationData> GetNavigation()
        {
            return _pages;
        }
    }*/
}