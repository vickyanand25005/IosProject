using Straticator.Common;
using StraticatorFroms_iOS.Models;
using StraticatorFroms_iOS.Views.CopyTrade;
using StraticatorFroms_iOS.Views.Reports;
using StraticatorFroms_iOS.Views.Reports.Currecny;
using StraticatorFroms_iOS.Views.Reports.Order;
using StraticatorFroms_iOS.Views.Reports.PLReport;
using StraticatorFroms_iOS.Views.Reports.Symbol;
using StraticatorFroms_iOS.Views.Reports.TradeReport;
using StraticatorFroms_iOS.Views.Reports.Transactions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : MasterDetailPage
    {
        MenuPage menuPage;

        Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        public MainPage()
        {
            InitializeComponent();

            MasterBehavior = MasterBehavior.Popover;

            MenuPages.Add((int)MenuItemType.Position, new NavigationPage(new PositionPage()));
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Position:
                        MenuPages.Add(id, new NavigationPage(new PositionPage()));
                        break;
                    case (int)MenuItemType.Watchlist:
                        MenuPages.Add(id, new NavigationPage(new WatchlistPage()));
                        break;
                    case (int)MenuItemType.CopyTrade:
                        MenuPages.Add(id, new NavigationPage(new CopyTradePage()));
                        break;
                    case (int)MenuItemType.WhatToFollow:
                        MenuPages.Add(id, new NavigationPage(new WhatToFollowPage()));
                        break;
                    case (int)MenuItemType.Trade:
                        MenuPages.Add(id, new NavigationPage(new TradeReportPage()));
                        break;
                    case (int)MenuItemType.Orders:
                        MenuPages.Add(id, new NavigationPage(new OrderReportPage()));
                        break;
                    case (int)MenuItemType.OrderHistory:
                        MenuPages.Add(id, new NavigationPage(new OrderHistoryPage()));
                        break;
                    case (int)MenuItemType.Transaction:
                        MenuPages.Add(id, new NavigationPage(new TransactionReportPage()));
                        break;
                    case (int)MenuItemType.Currency:
                        MenuPages.Add(id, new NavigationPage(new CurrencyExposurePage()));
                        break;
                    case (int)MenuItemType.Symbol:
                        MenuPages.Add(id, new NavigationPage(new SymbolReportPage()));
                        break;
                    case (int)MenuItemType.TradeStatistics:
                        MenuPages.Add(id, new NavigationPage(new TradeStatisticsReportPage()));
                        break;
                    case (int)MenuItemType.PLReport:
                        MenuPages.Add(id, new NavigationPage(new PLReportPage()));
                        break;
                    case (int)MenuItemType.Logout:
                        Logout();
                        break;
                }
            }

            if (id != 12)
            {
                var newPage = MenuPages[id];

                if (newPage != null && Detail != newPage)
                {
                    Detail = newPage;
                    if (Device.RuntimePlatform == Device.Android)
                        await Task.Delay(100);

                    IsPresented = false;
                }
            }
        }

        private void Logout()
        {
            //StopTimer();
            SessionManager.Instance.LogOut();
            IsolatedStorage.RemoveNavigation();
            IsolatedStorage.RemoveSession();

            //accountList = null;

            SessionManager.Instance.CurrentAccount = 0;
            SessionManager.Instance.LastPositionAccount = null;
            SessionManager.Instance.LastPosition = null;
            Application.Current.MainPage = new NavigationPage(new Login.Login());
        }
    }
}