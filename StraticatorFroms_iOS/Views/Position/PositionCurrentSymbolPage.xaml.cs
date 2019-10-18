using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using Straticator.Model;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views.Reports.Order;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Position
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionCurrentSymbolPage : ContentPage
    {
        short symbol;
        AccountAPI accountApi;
        IList<AccountPositionPrint> _accountPos;
        PositionPrint selectedAccountPositionPrint;
        short _currDisplaySymbol;
        TradingAPI tradingApi;
        public static int selectedPositionsCurrentSymbol = 0;
        public PositionCurrentSymbolViewModel positionCurrentSymbolViewModel;
        public PositionCurrentSymbolPage(short _currDisplaySymbol)
        {
            InitializeComponent();
            symbol = _currDisplaySymbol;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            accountApi = new AccountAPI(SessionManager.Instance.Session);
            accountApi.AccountPositionType = typeof(AccountPositionPrint);
            _accountPos = new List<AccountPositionPrint>();
            this.BindingContext = positionCurrentSymbolViewModel = new PositionCurrentSymbolViewModel();
            PositionsCurrentSymbol_Loaded();
            LocalizeText();
        }

        private void LocalizeText()
        {
            lblSymbol.Text = ChangeCulture.Lookup("PositionsKey");
            lblVolume.Text = ChangeCulture.Lookup("MPVolume").Replace(":", "");
            lblOpen.Text = ChangeCulture.Lookup("HOpenPrice").Replace(":", "");
            lblPrice.Text = ChangeCulture.Lookup("CurrentPriceKey").Replace(":", "");
            lblPl.Text = ChangeCulture.Lookup("PL").Replace(":", "");
            lblTrack.Text = ChangeCulture.Lookup("Trackkey").Replace(":", "");
            
            lblLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            if (SessionManager.Instance.Session.Settings.UserLots)
                lblVolume.IsVisible= false;
            else
                lblLots.IsVisible = false;
        }

        private void PositionsCurrentSymbol_Loaded()
        {
            if (symbol != 0)
                lblSymbol.Text = MarketInfo.getSymbol(symbol);

                LoadAccountPositions(SessionManager.Instance.CurrentAccount);
        }

        private async void LoadAccountPositions(int aid)
        {
            var ret = await accountApi.GetAccountPositionAsync(aid);
            if (ret != null)
            {
                _accountPos = AccountPositionPrint.Convert(ret);
                if (_currDisplaySymbol != 0)
                    _accountPos = _accountPos.Where(p => p.symbolId == symbol).ToList();
            }
            else
                _accountPos = null;

            positionCurrentSymbolViewModel.LoadPosition(_accountPos.ToList());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selectedAccountPositionPrint != null)
            {
                var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("MPClose"), ChangeCulture.Lookup("Watchlist"), ChangeCulture.Lookup("PositionsKey"),
                    ChangeCulture.Lookup("Details"));
                int action = 0;
                if (actionSheet == ChangeCulture.Lookup("MPClose")) action = 1;
                else if (actionSheet == ChangeCulture.Lookup("Watchlist")) action = 2;
                else if (actionSheet == ChangeCulture.Lookup("PositionsKey")) action = 3;
                else if (actionSheet == ChangeCulture.Lookup("Details")) action = 4;

                switch (action)
                {
                    case 1:
                        ClosePosition();
                        break;
                    case 2:
                        await Navigation.PushModalAsync(new WatchlistPage());
                        break;
                    case 3:
                        await Navigation.PushModalAsync(new PositionPage());
                        break;
                    case 4:
                        selectedAccountPositionPrint.info = MarketInfo.getMarketInfo((short)selectedAccountPositionPrint.SymbolId);
                        PositionDetailPage.currentPosition = selectedAccountPositionPrint;
                        await Navigation.PushModalAsync(new PositionDetailPage());
                        break;
                }
            }
        }

        private async void ClosePosition()
        {
            if(selectedAccountPositionPrint!=null)
            {
                PositionPrint position = selectedAccountPositionPrint;
                if (position == null) return;
                if (position.HasCloseLink)
                {
                    NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
                    string secondArg = (position.Volume).ToString("N0", nf);
                    if (SessionManager.Instance.Session.Settings.UserLots)
                        secondArg = new Lot((int)position.volume, position.symbolId).ToString("", nf);
                    string message = string.Format(ChangeCulture.Lookup("ClosePositionConfirmation"), position.Symbol, secondArg);
                    var res = await DisplayAlert(ChangeCulture.Lookup("Confirmation"), message, ChangeCulture.Lookup("OK"), ChangeCulture.Lookup("cancelKey"));
                    if (res)
                    {
                        ClosePosition(position);
                    }
                }
            }
        }

        public async void ClosePosition(AccountPositionPrint position)
        {
            var orderDetail = new LiveChartTrader.BaseClass.OrderDetail()
            {
                volume = unchecked((int)position.volume),
                symbolId = position.symbolId,
                track = position.track,
                aid = position.aid,
                positionId = position.positionId,
                UserOrderOrigin = UserOrderOrigin.Mobile
            };

            if (tradingApi == null)
                tradingApi = new StraticatorAPI.TradingAPI(SessionManager.Instance.Session);

            var order = new PlaceOrder(tradingApi, orderDetail);
            order.ServerNotResponded += async delegate
            {
               await Navigation.PushModalAsync(new OrderHistoryPage());
            };
            await order.SubmitOrder(StraticatorAPI.OrderAction.Close);

            LoadAccountPositions(SessionManager.Instance.CurrentAccount);
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedAccountPositionPrint = (PositionPrint)e.SelectedItem;
        }
    }
}