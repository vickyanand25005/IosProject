using Straticator.Common;
using Straticator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorAPI;
using Straticator;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views.Login;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Views.WatchListSettings;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WatchlistPage : ContentPage
    {

        List<DisplayPrice> _lPrice;
        public static int selectedRow = 0;
        bool LeavingPage;
        static int Cnt = 0;
        Session session;

        DisplayPriceText selectedDisplayPrice;
        public WatchlistPage()
        {
            InitializeComponent();
        }

        private ulong callPriceStart;

        protected override void OnAppearing()
        {
            base.OnAppearing();
            session = SessionManager.Instance.Session;
            _lPrice = new List<DisplayPrice>();


            Device.StartTimer(TimeSpan.FromSeconds(500), () =>
            {
                LoadPriceList();
                return true;
            });

            LoadPriceList();

            OnResume();
        }

        public bool IsOnline { set { OnlineState.IsOnline(value, imgStatus, this); } }
        static ulong callServerStart;
        private async void LoadPriceList()
        {
            var tmp = callPriceStart;
            if (tmp != 0) // we are still waiting for this function to return from server.
            {
                if (!OnlineState.Timeout(tmp, imgStatus, this))
                {
                    await CheckUserLogin();
                    return;
                }
                // clear flag, since we timed out
                LeavingPage = false;
            }

            try
            {
                // we mark that we are inside the LoadPriceList, and record the time
                callPriceStart = OnlineState.GetTickCount();

                if (!App.CheckInternet())
                {
                    IsOnline = false;
                    imgStatus.Source = "offline.png";
                    return;
                }

                bool Completed = await session.LoadNewPriceListAsync();
                if (LeavingPage)
                    return; /*return in case navigated from page*/
                if (Completed)
                {
                    IsOnline = true;
                    imgStatus.Source = "online.png";
                    UpdateGrid();
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
                callPriceStart = 0;
                //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
            finally
            {
                // we clear that we leave LoadPriceList()
                callPriceStart = 0;
            }
        }

        private async Task CheckUserLogin()
        {
            try
            {
                bool isLoggedIn = await SessionManager.Instance.Session.TestLoginAsync();
                if (!isLoggedIn)
                    await Navigation.PushAsync(new Login.Login());
            }
            catch
            {
                callServerStart = 0;
                //Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
            }
        }

        private void OnResume()
        {
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);

            //OnlineState.SetInitialOffState(imgStatus, this);
            imgStatus.Source = "offline.png";

            // we draw grid before we load, since the pricelist is saved globally and has the newest prices.
            UpdateGrid();

            if (LeavingPage)
            {
                LeavingPage = false;
                callPriceStart = 0;
            }
            //timer start for loading new prices.

            // load first without timer.
            UpdateAndLoadPriceList();
        }

        private async void UpdateAndLoadPriceList()
        {
            bool res = await session.UpdateUserSymbols();
            LoadPriceList();
        }

        int firstVisiblePosition = 0;
        private void UpdateGrid()
        {
           // firstVisiblePosition = lstView.FirstVisiblePosition;
            _lPrice.Clear();
            Cnt = 0;
            if (session.PriceList != null)
            {
                foreach (var price in session.PriceList)
                {
                    if (price.RemoveInPriceList)
                        continue;

                    DisplayPrice dspPrice = new DisplayPrice(price.SymbolId);
                    string p1, p2, p3;
                    SymbPrice.Format3parts(price.SymbolId, price.Ask, out p1, out p2, out p3);
                    dspPrice.AskFormatted = new PriceFormatted(price.Ask, p1, p2, p3) { ColorIndex = price.AskPriceDirection };
                    SymbPrice.Format3parts(price.SymbolId, price.Bid, out p1, out p2, out p3);
                    dspPrice.BidFormatted = new PriceFormatted(price.Bid, p1, p2, p3) { ColorIndex = price.BidPriceDirection };

                    dspPrice.IsSelectedRow = (selectedRow == Cnt);
                    Cnt += 1;

                    _lPrice.Add(dspPrice);
                }
                Device.BeginInvokeOnMainThread(() =>
                {
                    lstView.BindingContext = new WatchlistViewModel(_lPrice);
                    //if (firstVisiblePosition < listView.Count)
                    //{
                    //    if (selectedRow == firstVisiblePosition - 1 || firstVisiblePosition == 0)
                    //        listView.SetSelectionFromTop(firstVisiblePosition, 0);
                    //    else
                    //        listView.SetSelectionFromTop(firstVisiblePosition + 1, 0);
                    //}
                });
            }

        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PositionPage());
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedDisplayPrice = (DisplayPriceText)e.SelectedItem;
        }

        private void LstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selectedDisplayPrice != null)
            {
                var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("FVTrade"), ChangeCulture.Lookup("ChartKey"), ChangeCulture.Lookup("CandleStick"), ChangeCulture.Lookup("OrganizeKey"));
                int action = 0;
                if (actionSheet == ChangeCulture.Lookup("FVTrade")) action = 1;
                if (actionSheet == ChangeCulture.Lookup("ChartKey")) action = 2;
                if (actionSheet == ChangeCulture.Lookup("CandleStick")) action = 3;
                if (actionSheet == ChangeCulture.Lookup("OrganizeKey")) action = 4; 

                switch (action)
                {
                    case 1:
                        var orderTicketPage = new OrderTicketPage(selectedDisplayPrice.Symbol, selectedDisplayPrice.SymbolId);
                        NavigationExtensions.Navigate(typeof(OrderTicketPage), selectedDisplayPrice.SymbolId);
                        await Navigation.PushModalAsync(orderTicketPage);
                        break;
                    case 2:
                        await Navigation.PushModalAsync(new ChartPage(selectedDisplayPrice.SymbolId));
                        break;
                    case 3:
                        await Navigation.PushModalAsync(new CandleStickChartPage(selectedDisplayPrice.Symbol));
                        break;
                    case 4:
                        await Navigation.PushAsync(new SymbolsPage());
                        break;
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(ChangeCulture.Lookup("SelectRowMessage"));
            }
        }
    }
}