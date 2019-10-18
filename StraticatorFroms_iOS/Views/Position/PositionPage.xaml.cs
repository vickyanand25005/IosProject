using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Straticator.LocalizationConverter;
using LiveChartTrader.Common;
using Straticator;
using StraticatorAPI;
using Straticator.Common;
using LiveChartTrader.BaseClass;
using Straticator.Model;
using System.Collections.ObjectModel;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS.Views.Custom;
using System.Globalization;
using System.Threading;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionPage : ContentPage
    {
        StraticatorAPI.AccountAPI accountApi;
        StraticatorAPI.TradingAPI tradingApi;
        IList<UserAccount> accountList;
        bool LeavingPage;
        public static int selectedRowPosition;
        PositionViewModel positionViewModel;
        /*Start Add By Differenz */

        Xamarin.Forms.ListView listView;
        List<PositionRowItem> tableItems;
        ObservableCollection<PositionPrint> PositionList;

        PositionPrint selecteditem;
        List<string> Accounts;
        string selectedSymbol = string.Empty;

        // public IList<Product> Items { get; private set; }


        public PositionPage()
        {
            InitializeComponent();
            tableItems = new List<PositionRowItem>();
            string[] Header = { "PositionsKey", "SelectaccountKey", "Symbolkey", "MPVolume", "HOpenPrice", "CurrentPriceKey", "PL", "Track", "Equity", "UnrealizedPL", "Balance", "Invested", "MarginUsedAmt", "MarginUsed", "Lots", "Typekey" };
            LocalizationHandler.LocalizeGridHeader(ref Header);
            lblPositions.Text = Header[0];
            lblSelectAccount.Text = Header[1];


            lblEquity.Text = Header[8] + ":";
            lblUnrealizedPl.Text = Header[9] + ":";
            lblBalance.Text = Header[10] + ":";
            lblInvested.Text = Header[11] + ":";
            lblMarginUsed.Text = Header[12] + ":";
            lblMarginUsed1.Text = Header[13] + ":";

            var service = new ProductService();
            // Items = service.GetAll.OrderBy(c => c.Name).ToList();
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();

            InitializeComponent();
            this.BindingContext = positionViewModel = new PositionViewModel();

            accountList = SessionManager.Instance.Session.Accounts;
            accountApi = new StraticatorAPI.AccountAPI(SessionManager.Instance.Session)
            {
                AccountBalanceType = typeof(AccountBalance),
                AccountPositionType = typeof(AccountPositionPrint)
            };

            LoadAccountPositions();

            Device.StartTimer(TimeSpan.FromSeconds(500), () =>
            {
                LoadAccountPositions();
                return true;
            });


            FillCombobox();


            int Aid = SessionManager.Instance.CurrentAccount;
            if (Aid != 0)
            {
                PaintAccount();
                PaintRows();
            }

            if (PositionList != null && tappedIndex >= 0)
            {
                lstView.SelectedItem = PositionList[tappedIndex];
            }

            OnNavigatedTo();
        }

        private void OnNavigatedTo()
        {
            NavigatePages.SetNavigatePage(typeof(PositionPage));
        }

        private void PaintRows()
        {
            tableItems.Clear();
            var accountPos = SessionManager.Instance.LastPosition;
            if (accountPos != null)
            {
                int CntPosition = 0;
                foreach (var acPos in accountPos)
                {
                    tableItems.Add(new PositionRowItem() { symbolId = acPos.SymbolId, symbol = acPos.Symbol, volume = acPos.Volume.ToString("N0"), stringopenprice = acPos.OpenPrice.ToString(), currentprice = acPos.CurrentPrice.ToString(), pl = Math.Round(acPos.accountPL, 2).ToString("N2"), track = Convert.ToString(acPos.Track), selected = (CntPosition == selectedRowPosition), lots = new Lot((int)acPos.volume, acPos.symbolId).ToString(), positionId = acPos.positionId, type = acPos.Type });
                    CntPosition++;
                }
            }
            RunAdapterUIThread();
        }

        private void RunAdapterUIThread()
        {
            Device.BeginInvokeOnMainThread(() =>
            {

                //var adapter = new PositionRowAdapter(this, tableItems);
                //listView.Adapter = adapter;
                //if (listView != null && selectedRowPosition < listView.Count)
                //    listView.SetSelectionFromTop(selectedRowPosition, 0);
            });
        }

        private void PaintAccount()
        {
            var acBalance = Straticator.Common.SessionManager.Instance.LastPositionAccount;
            if (acBalance == null)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                lblEquityValue.Text = FormatAmount(acBalance.equity);
                lblInvestedValue.Text = FormatAmount(acBalance.amountInvested);
                lblUnrealizedPlValue.Text = FormatAmount(acBalance.pL);
                lblMarginUsedValue.Text = FormatAmount(acBalance.marginUsed);
                lblBalanceValue.Text = FormatAmount(acBalance.balance);
                lblMarginUsed1Value.Text = Convert.ToString(acBalance.pctMarginUsed);
            });
        }

        static string FormatAmount(double d) { return d.ToString("N0"); }

        public PopupViewModel ViewModel => PopupViewModel.Instance;



        public bool IsOnline { set { OnlineState.IsOnline(value, imgStatus, this); } }



        static ulong callServerStart;

        private async void LoadAccountPositions()
        {
            //var tmp = callServerStart;

            //if (tmp != 0) // we are still waiting for this function to return from server.
            //{
            //    if (!OnlineState.Timeout(tmp, imgStatus, this))
            //    {
            //        await CheckUserLogin();
            //        return;
            //    }
            //    // clear flag, since we timed out
            //    LeavingPage = false;
            //}

            try
            {
                // we mark that we are inside this function, and record the time
                callServerStart = OnlineState.GetTickCount();

                if (!App.CheckInternet())
                {
                    IsOnline = false;
                    imgStatus.Source = "offline.png";
                    return;
                }
                int Aid;
                AccountLoad ret;
                do
                {
                    Aid = SessionManager.Instance.CurrentAccount;
                    ret = await accountApi.GetAccountStatusAsync(Aid);
                    if (LeavingPage)
                        return;
                    // we reload data if user has changed the combobox with account number while we were on the server.
                } while (Aid != SessionManager.Instance.CurrentAccount);

                if (ret != null)
                {
                    SessionManager.Instance.LastPositionAccount = (AccountBalance)ret.Balance;
                    SessionManager.Instance.LastPosition = AccountPositionPrint.Convert(ret.Positions);
                    PositionList = positionViewModel.LoadPositions();
                    if (tappedIndex > -1)
                    {
                        lstView.SelectedItem = PositionList[tappedIndex];
                    }
                    PaintAccount();
                    PaintRows();
                    IsOnline = true;
                    imgStatus.Source = "online.png";
                }
                else
                {
                    IsOnline = false;
                    imgStatus.Source = "offline.png";
                    await CheckUserLogin();
                }
            }
            catch
            {
                IsOnline = false;
                imgStatus.Source = "offline.png";
                callServerStart = 0;
                //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            finally
            {
                // we clear that we leave Lthis function
                callServerStart = 0;
            }
        }

        private async System.Threading.Tasks.Task CheckUserLogin()
        {
            try
            {
                bool isLoggedIn = await SessionManager.Instance.Session.TestLoginAsync();
                // if (!isLoggedIn)
                //Application.MainPage = new NavigationPage(new StraticatorFroms_iOS.Views.Login.Login());
            }
            catch
            {
                callServerStart = 0;
                //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }

        private void FillCombobox()
        {
            int DefaultAid = 0;
            Accounts = new List<string>();
            foreach (LiveChartTrader.Common.UserAccount ua in accountList)
            {
                //Accounts.Add(ua.Account);

                Accounts.Add(string.Format("{0} - {1} / {2}", ua.Aid, ua.Account, ua.Currency));
                if (ua.isDefault)
                    DefaultAid = ua.Aid;
            }

            cbAccount.ItemsSource = Accounts;
            if (cbAccount.Items.Count > 0)
            {
                int Aid = SessionManager.Instance.CurrentAccount;
                if (Aid != 0)
                    DefaultAid = Aid;
                else if (DefaultAid != 0)
                    SessionManager.Instance.CurrentAccount = DefaultAid;

                if (DefaultAid != 0)
                {
                    var index = 0;
                    for (int i = 0; i < cbAccount.Items.Count; i++)
                    {
                        int tempAid = Convert.ToInt32(Accounts[i].Split(' ')[0]);
                        if (tempAid == DefaultAid)
                        {
                            index = i;
                            break;
                        }
                    }
                    cbAccount.SelectedIndex = index;
                }
                else
                    cbAccount.SelectedIndex = 0;
            }
        }

        int tappedIndex;

        private async void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selecteditem = (PositionPrint)e.SelectedItem;
            tappedIndex = e.SelectedItemIndex;
            if (((ListView)sender).SelectedItem == null)
                return;
            //Get the item we have tapped on in the list. Because our ItemsSource is bound to a list of BoundObject, this is possible.


            foreach (var item in PositionList)
            {
                item.TextColor = Color.Transparent;
                item.TextColor2 = Color.Transparent;
            }


            selecteditem.TextColor = Color.Brown;
            selecteditem.TextColor2 = Color.FromHex("#77BC0000");
            //Loop through our List<BoundObject> - if the item is our selected item (checking on ID) - change the color. Else - set it back to blue 


            //ItemsSource must be set to null before it is re-assigned, otherwise it will not re-generate with the updated values. 
            lstView.ItemsSource = null;
            lstView.ItemsSource = PositionList;
            //lstView.SelectedItem = selecteditem;
        }

        private async void LstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (selecteditem != null)
            {
                foreach (var item in PositionList)
                {
                    item.TextColor = Color.Transparent;
                    item.TextColor2 = Color.Transparent;
                }


                selecteditem.TextColor = Color.Brown;
                selecteditem.TextColor2 = Color.FromHex("#77BC0000");
                //Loop through our List<BoundObject> - if the item is our selected item (checking on ID) - change the color. Else - set it back to blue 


                //ItemsSource must be set to null before it is re-assigned, otherwise it will not re-generate with the updated values. 
                lstView.ItemsSource = null;
                lstView.ItemsSource = PositionList;
            }
            OpenActionSheet();
        }

        private async void OpenActionSheet()
        {
            var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("FVTrade"),
              ChangeCulture.Lookup("MarketPriceKey"), ChangeCulture.Lookup("MPClose"));
            int action = 0;
            if (actionSheet == ChangeCulture.Lookup("OrderticketKey"))
            {
                action = 1;
            }
            else if (actionSheet == ChangeCulture.Lookup("FVTrade"))
            {
                action = 2;
            }
            else if (actionSheet == ChangeCulture.Lookup("MarketPriceKey"))
            {
                action = 3;
            }
            else if (actionSheet == ChangeCulture.Lookup("MPClose"))
            {
                action = 4;
            }

            switch (action)
            {
                case 1:
                    Ticket_Clicked();
                    break;
                case 2:
                    Trade_Clicked();
                    break;
                case 3:
                    OpenWatchlist();
                    break;
                case 4:
                    Close_Position();
                    break;
                default:

                    break;
            }
        }

        private async void OpenWatchlist()
        {
            await Navigation.PushModalAsync(new WatchlistPage());
        }

        private async void Trade_Clicked()
        {
            await Navigation.PushAsync(new OrderTicketPage(selecteditem.Symbol, (short)selecteditem.SymbolId));
        }

        private async void Close_Position()
        {
            PositionPrint position = selecteditem;
            string symbol = selecteditem.Symbol;
            if (position.HasCloseLink)
            {
                NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
                string secondArg = (position.Volume).ToString("N0", nf);
                if (SessionManager.Instance.Session.Settings.UserLots)
                    secondArg = new Lot((int)position.volume, position.symbolId).ToString("", nf);

                var res = await DisplayAlert(ChangeCulture.Lookup("Confirmation"), string.Format(ChangeCulture.Lookup("ClosePositionConfirmation"), position.Symbol, secondArg), ChangeCulture.Lookup("OK"), ChangeCulture.Lookup("cancelKey"));
                if (res)
                {
                    ClosePosition(position);
                }

            }
            else
            {
                await DisplayAlert(ChangeCulture.Lookup("ClosePosition"), string.Format(ChangeCulture.Lookup("ErrorClosePosition"), symbol), ChangeCulture.Lookup("OK"));
            }
        }

        private async void ClosePosition(AccountPositionPrint position)
        {
            var orderDetail = new LiveChartTrader.BaseClass.OrderDetail()
            {
                volume = unchecked((int)position.volume),
                symbolId = position.symbolId,
                track = position.track,
                aid = position.aid,
                UserOrderOrigin = UserOrderOrigin.Mobile,
                positionId = position.positionId
            };

            if (tradingApi == null)
                tradingApi = new StraticatorAPI.TradingAPI(SessionManager.Instance.Session);

            var order = new PlaceOrder(tradingApi, orderDetail);
            order.ServerNotResponded += delegate
            {
                // NavigationExtensions.Navigate(this, typeof(OrderHistoryReport));
            };
            await order.SubmitOrder(StraticatorAPI.OrderAction.Close);

            LoadAccountPositions();
        }

        private async void Ticket_Clicked()
        {
            short SymbolId = Convert.ToInt16(selecteditem.SymbolId);
            byte track = Convert.ToByte(selecteditem.Track);
            PositionPage.selectedRowPosition = tappedIndex;
            OrderTicketPage.PositionId = selecteditem.PositionId;
            OrderTicketPage.Track = track;
            var orderTicketPage = new OrderTicketPage(selecteditem.Symbol, SymbolId);
            NavigationExtensions.Navigate(typeof(OrderTicketPage), SymbolId);
            await Navigation.PushModalAsync(orderTicketPage);
        }

        private void CbAccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker spinner = (Picker)sender;
            if (spinner != null)
            {
                var tempaid = (string)cbAccount.SelectedItem;
                string[] aid = tempaid.Split(' ');
                btnAid.Text = aid[0];

                SessionManager.Instance.CurrentAccount = Convert.ToInt32(aid[0]);
                // StartTimer();
                selectedRowPosition = 0;
                //reload of data of the positions grid 
                LoadAccountPositions();
            }
            else
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    lblEquityValue.Text = lblBalanceValue.Text = lblInvestedValue.Text = lblMarginUsed1Value.Text = lblMarginUsedValue.Text = lblUnrealizedPlValue.Text = string.Empty;
                });

                //set Data of flxGridPosition
                //tableItems.Clear();
                //Bind position data into 
                RunAdapterUIThread();
            }
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            short symbolId = 0;
            if (selecteditem != null)
            {
                symbolId = selecteditem.symbolId;
            }
            await Navigation.PushModalAsync(new WatchlistPage());
        }

        private void OnLabelClicked(object sender, EventArgs e)
        {
            var entity = ((Label)sender);
            entity.BackgroundColor = Color.Wheat;

        }

    }



    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductService
    {
        public ObservableCollection<Product> GetAll { get; private set; }

        public ProductService()
        {
            GetAll = new ObservableCollection<Product> {
                new Product { Name = "HP Stream LapTop", Price = 199.00M },
                new Product { Name = "Western Digital 1 TB", Price = 64.99M},
                new Product { Name = "Casio Calculator", Price = 7.79M },
                new Product { Name = "Microsoft Surface Pro", Price = 854.19M },
                new Product { Name = "Dell 27 LCD Monitor", Price = 168.36M },
                new Product { Name = "HP 27 LED Monitor", Price = 178.50M },
                new Product { Name = "Memorex Lens Cleaner", Price = 9.98M },
            };
        }
    }
}