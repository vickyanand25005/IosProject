
using StraticatorAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartTrader.Common;
using System.Windows;
using System.IO.IsolatedStorage;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
//using Newtonsoft.Json;
using Straticator.Common;
using StraticatorFroms_iOS.Views;
using LiveChartTrader.BaseClass;
using StraticatorFroms_iOS;
using StraticatorFroms_iOS;

namespace Straticator.Common
{
    public class SessionManager
    {
        internal static readonly SessionManager Instance = new SessionManager();

        private  bool isInitialized;
        private  bool hasConnection;

        public StraticatorAPI.Session Session;
        public int CurrentAccount;
        public AccountBalance LastPositionAccount;
        public IList<Straticator.Model.AccountPositionPrint> LastPosition;
        public short CurrentChartSymbol;
       
        public  bool IsInitialized { get { return isInitialized; } }
        public  bool HasConnection { get { return isInitialized && hasConnection; } }

        public static bool IsLoggedIn=false;

        //Same as in WP
        private SessionManager()
        {
        }

        //Same as in WP
        internal async Task<bool> InitAsync()
        {
            if (!isInitialized)
            {
                try
                {
                    ISKeys ob = IsolatedStorage.GetSession();
                    bool ok = await StraticatorAPI.Global.Initialize(0, APITarget.Live, ob != null ? ob.sessiondata : null);
                    hasConnection = ok;

                    if (ob != null)
                    {
                        Session = StraticatorAPI.Global.session;
                        if (Session != null)
                        {
                            hasConnection = true;
                            IsLoggedIn = ok;
                            this.CurrentChartSymbol = ob.chartSymbol;
                            this.CurrentAccount = ob.CurrentAccount;
                        }
                    }

                    if (Session == null && hasConnection)
                        Session = new Session();

                    isInitialized = true;
                }
                catch (Exception)
                {
                    isInitialized = false;
                    IsolatedStorage.RemoveSession();
                }
            }
            return isInitialized;
        }

        //Same as in WP
        internal async Task<bool> Login(string username, string password, bool isLiveLogin = false)
        {
            LoginType loginType = isLiveLogin ? LoginType.Live : LoginType.Demo;
            var res = await Session.LoginAsync(username, password, loginType, "2", Common.ClientOrigin.Mobile);
            IsLoggedIn = true;
            return res;
        }

        //Same as in WP
        internal void LogOut()
        {
            Session.LogOut();
        }

        //Same as in WP
        internal async void AmIloggedIn()
        {
            bool isLoggedIn = false;
            try
            {
                if (App.CheckInternet())
                    isLoggedIn = await Session.TestLoginAsync();
            }
            catch { }

            if (!isLoggedIn) { }
                //MainPage.isActivated = false;
        }

        //Same as in WP

        internal async Task<ErrorCodes> CreateUser(UserCommon guestUser, string encryptPasswordInfo, bool forGame)
        {
            return await Session.CreateNewUserAsync(guestUser, encryptPasswordInfo, true, forGame);
        }
    }
}
