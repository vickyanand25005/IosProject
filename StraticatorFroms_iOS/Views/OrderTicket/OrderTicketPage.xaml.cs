using LiveChartTrader.IndicatorDataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorAPI;
using Straticator.Model;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.Common;
using System.Threading;
using Rg.Plugins.Popup.Services;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS.Views.Reports.Order;
using StraticatorFroms_iOS.Views.Reports.TradeReport;
using StraticatorFroms_iOS.Views.WatchListSettings;
using StraticatorFroms_iOS.Views.Position;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderTicketPage : ContentPage
    {
        bool LeavingPage;
        TradingAPI _tradingApi;
        Session session;
        System.Timers.Timer _dTimer;
        double prevVolume;
        short _currDisplaySymbol;
        int selMarketType;
        bool UseTracks;
        bool UseLots;
        TimeFrame _timefrm;
        PageSavedContent PageInfo;
        static long positionId;
        NumberFormatInfo numberFormat;
        int SymbolLotSize;
        double Limit_Step, Limit_MaxValue, StopLoss_MaxValue, TakeProfit_MaxValue;
        PopupProgressPage popupProgressPage;
        short selectedSymbolId;

        static ulong callServerStart;
        public static long PositionId
        {
            set
            {
                positionId = value;
            }
        }
        byte track;
        static byte _track;
        public static byte Track
        {
            set
            {
                _track = value;
            }
        }
        public float VolumeInLots { get; set; }

        public bool IsOnline { set { OnlineState.IsOnline(value, imgStatus, this); } }
        public OrderTicketPage(string symbol,short selectedSymbolID=0)
        {
            InitializeComponent();
            
            numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
            session = SessionManager.Instance.Session;
            lblSymbol.Text = symbol;
            lblSymbolId.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            FillComboBox("MarketType");
            FillComboBox("DurationType");

            Limit_Step = 0.0001;
            Limit_MaxValue = 1000000;
            StopLoss_MaxValue = 1000;
            TakeProfit_MaxValue = 1000;
            SymbolLotSize = 1;

            track = (_track > 0) ? _track : (byte)1;
            entTrack.Text = Convert.ToString(track);


            _tradingApi = new TradingAPI(session);
            selectedSymbolId = selectedSymbolID;
            OnResume();


            LoadPriceList();
            Device.StartTimer(TimeSpan.FromSeconds(500), () =>
            {
                LoadPriceList();
                return true;
            });
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (UseTracks)
                stkTrack.IsVisible=  true;
        }

        protected void OnResume()
        {
            OnlineState.SetInitialOffState(imgStatus, this);
            var MyUrl = typeof(OrderTicketPage);
            //get parameter for which is caused this page
            //var PageData = NavigatePages.PageContext();
            //if (PageData != null)
            //{
            //    if (PageData is PageSavedContent)
            //    {
            //        this.PageInfo = (PageSavedContent)PageData;
            //        var pg = this.PageInfo;
            //        entTakeProfit.Text = Convert.ToString(pg.Track);
            //        entStopLoss.Text = pg.sl == 0 ? string.Empty : Convert.ToString(pg.sl);
            //        entTakeProfit.Text = pg.tp == 0 ? string.Empty : Convert.ToString(pg.tp);
            //        if (UseLots)
            //        {
            //            var lot = new Lot(pg.Volume, pg.SymbolId);
            //            BindLotsVolume(lot.ToString("", numberFormat));
            //        }
            //        else
            //        {
            //            if (pg.Volume > 0)
            //                entVolume.Text = Convert.ToString(pg.Volume);
            //        }
            //        entLimit.Text = Convert.ToString(pg.LimitPrice);
            //        pkrOrderType.SelectedItem = pg.OrderType;
            //        //chTrailingStop.Checked = pg.TrailingStop;
            //        pkrExpiry.SelectedItem = pg.Expiry;

            //        _currDisplaySymbol = pg.SymbolId;

            //    }
            //    else
            //    {
            //        short SymbolId = (short)PageData;
            //        if (SymbolId != 0)
            //            _currDisplaySymbol = SymbolId;
            //    }
            //}
            if (selectedSymbolId != 0)
            {
                var info = MarketInfo.getMarketInfo(selectedSymbolId);
                this.SymbolLotSize = info.Lotsize;
                lblSymbolName.Text = info.Symbol;
                lblSymbol.Text = MarketInfo.getTradingName(selectedSymbolId);
                lblCurrencySymbol.Text = info.TradedCurrencyId.ToString();

                PrintPrice(info.SymbolId, info.LatestPrice.AskPrice, info.LatestPrice.BidPrice);

                if (PageInfo == null)
                {
                    PageInfo = new PageSavedContent();
                    PageInfo.SymbolId = selectedSymbolId;
                    NavigatePages.SetNavigatePageContext(MyUrl, PageInfo);
                }
            }

            //timer start for data loading
            StartTimer();
        }

        private void StartTimer()
        {
            if (LeavingPage)
            {
                LeavingPage = false;
                callServerStart = 0;
            }
            //_dTimer.Start();
        }

        private void BindLotsVolume(string text)
        {
            float volume = Straticator.Utility.Double.parseFloat(text.Replace(numberFormat.NumberGroupSeparator, ""));
            VolumeInLots = volume;
            if (VolumeInLots == 0f)
                entVolume.Text = string.Empty;
            else
                entVolume.Text = volume.ToString(Lot.Format);
        }

        private async void LoadPriceList()
        {
            //var tmp = callServerStart;
            //if (tmp != 0) // we are still waiting for this function to return from server.
            //{
            //    if (!OnlineState.Timeout(tmp, imgStatus, this))
            //        return;
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
                    return;
                }

                var currSymbolPrice = await session.LoadNewPriceSymbolAsync(selectedSymbolId);
                
                if (currSymbolPrice != null)
                {
                    PrintPrice(currSymbolPrice.SymbolId, currSymbolPrice.AskPrice, currSymbolPrice.BidPrice);
                    IsOnline = true;
                }
                else
                {
                    IsOnline = false;
                }
            }
            catch
            {
                IsOnline = false;
            }
            finally
            {
                // we clear that we leave Lthis function
                callServerStart = 0;
            }
        }

        private void PrintPrice(short symbolId, float askPrice, float bidPrice)
        {
            DisplayPrice dspPrice = new DisplayPrice(symbolId);
            string p1, p2, p3;
            SymbPrice.Format3parts(symbolId, askPrice, out p1, out p2, out p3);
            dspPrice.AskFormatted = new PriceFormatted(askPrice, p1, p2, p3);
            SymbPrice.Format3parts(symbolId, bidPrice, out p1, out p2, out p3);
            dspPrice.BidFormatted = new PriceFormatted(bidPrice, p1, p2, p3);
            Device.BeginInvokeOnMainThread(() =>
            {
                lblAsk.Text = dspPrice.AskFormatted.Part1 + dspPrice.AskFormatted.Part2 + dspPrice.AskFormatted.Part3;
                lblBid.Text = dspPrice.BidFormatted.Part1 + dspPrice.BidFormatted.Part2 + dspPrice.BidFormatted.Part3;
                btnSpread.Text = Convert.ToString(dspPrice.Spread);
            });
        }

        private void FillComboBox(string key)
        {
            string[] arr = ChangeCulture.LookupEnum(key);
            if (key == "MarketType")
            {
                arr = new string[] { "Market", "Quoted", "Limit", "Buy/Sell", "Stop", "Take Profit", "Stop Loss", "Trailing Stop Loss" };
                pkrOrderType.ItemsSource = arr;
            }
            else
            {
                arr = new string[] { "Good til Cancel", "End of Day", "End of Week", "Immediate or Cancel", "Fill or Kill", "Time Frame" };
                pkrExpiry.ItemsSource = arr;
            }
            pkrOrderType.SelectedIndex = 0;
            pkrExpiry.SelectedIndex = 0;

        }

        private void TrackUp_Clicked(object sender, EventArgs e)
        {
            entTrack.Text = Convert.ToString(Convert.ToInt32(entTrack.Text) + 1);
        }

        private void TrackDown_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToInt32(entTrack.Text) > 0)
                entTrack.Text = Convert.ToString(Convert.ToInt32(entTrack.Text) - 1);
        }

        private void StoplossUp_Clicked(object sender, EventArgs e)
        {
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl < StopLoss_MaxValue)
                entStopLoss.Text = Convert.ToString(sl + 5);
        }

        private void StoplossDown_Clicked(object sender, EventArgs e)
        {
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl > 0)
                sl -= 5;
            entStopLoss.Text = sl < 0 ? "0" : Convert.ToString(sl);
        }

        private void TakeProfitUp_Clicked(object sender, EventArgs e)
        {
            int tf = string.IsNullOrWhiteSpace(entTakeProfit.Text) ? 0 : Convert.ToInt32(entTakeProfit.Text);
            if (tf < TakeProfit_MaxValue)
                entTakeProfit.Text = Convert.ToString(tf + 5);
        }

        private void TakeProfitDown_Clicked(object sender, EventArgs e)
        {
            int tf = string.IsNullOrWhiteSpace(entTakeProfit.Text) ? 0 : Convert.ToInt32(entTakeProfit.Text);
            if (tf > 0)
                tf -= 5;
            entTakeProfit.Text = tf < 0 ? "0" : Convert.ToString(tf);
        }

        private void LimitUp_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToDouble(entLimit.Text) < Limit_MaxValue)
            {
                double limitval = Convert.ToDouble(entLimit.Text);
                limitval += Limit_Step;
                entLimit.Text = Convert.ToString(limitval);
            }
        }

        private void LimitDown_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToDouble(entLimit.Text) > 0)
            {
                double limitval = Convert.ToDouble(entLimit.Text);
                limitval -= Limit_Step;
                entLimit.Text = Convert.ToString(limitval);
            }
        }

        private void BtnSell_Clicked(object sender, EventArgs e)
        {
            setTextofUpDown();
            if (Validate(true))
                BuyOrSell(false);
        }

        private void BtnBuy_Clicked(object sender, EventArgs e)
        {
            setTextofUpDown();
            if (Validate(false))
                BuyOrSell(true);
        }

        private async void BuyOrSell(bool isBuy)
        {
            popupProgressPage = new PopupProgressPage(ChangeCulture.Lookup("OrderProcessing"));
            int CheckVolumeOfTrade;
            //if (UseLots)
            //{
            //    int.TryParse((this.SymbolLotSize * Utility.Double.parseFloat
            //       (txtLots.Text.Replace(numberFormat.NumberGroupSeparator, ""))).ToString("N0"),
            //       System.Globalization.NumberStyles.AllowThousands,
            //       numberFormat, out CheckVolumeOfTrade);
            //}
            //else
            //{
                int.TryParse(entVolume.Text, System.Globalization.NumberStyles.AllowThousands, numberFormat, out CheckVolumeOfTrade);
            //}
            var orderDetail = new LiveChartTrader.BaseClass.OrderDetail()
            {
                volume = isBuy ? CheckVolumeOfTrade : -CheckVolumeOfTrade,
                symbolId = selectedSymbolId,
                aid = SessionManager.Instance.CurrentAccount,
                orderOrigin = (byte)LiveChartTrader.Common.UserOrderOrigin.Mobile,
                orderType = (byte)int.Parse((pkrOrderType.SelectedIndex).ToString()),
                duration = (byte)int.Parse((pkrExpiry.SelectedIndex).ToString()),
                sl = Convert.ToInt16(string.IsNullOrWhiteSpace(entStopLoss.Text) ? "0" : entStopLoss.Text),
                tp = Convert.ToInt16(string.IsNullOrWhiteSpace(entTakeProfit.Text) ? "0" : entTakeProfit.Text),
                SL_is_Trailing = chkTrailingStop.IsChecked,
                track = (byte)Convert.ToInt16(entTrack.Text),
                positionId = positionId
            };

            if (orderDetail.orderType >= (byte)UserOrderType.Quoted)
            {
                if (orderDetail.orderType == (byte)UserOrderType.Quoted)
                {
                    MarketInfo info = MarketInfo.getMarketInfo(orderDetail.symbolId);
                    orderDetail.price = isBuy ? info.LatestPrice.Ask : info.LatestPrice.Bid;
                }
                else
                    orderDetail.price = (float)Convert.ToDouble(entLimit.Text);
            }

            //time frame value
            if (pkrExpiry.SelectedIndex== (int)UserOrderDuration.Timeframe)
                orderDetail.Seconds = _timefrm.NumberOfSeconds;

            PlaceOrder order = new PlaceOrder(_tradingApi, orderDetail);
            order.ServerNotResponded += async delegate
            {
                DependencyService.Get<IMessage>().ShortAlert("OrderHistoryReport");
                await Navigation.PushAsync(new NavigationPage(new OrderHistoryPage()));
            };
            order.OrderRequested += async delegate
            {
                await PopupNavigation.Instance.PushAsync(popupProgressPage);
            };
            order.OrderResponded += async delegate
            {
                await PopupNavigation.Instance.PopAsync(true);
            };
            await order.SubmitOrder(OrderAction.New);
        }

        private bool Validate(bool sell)
        {
            string msg;
            string title;
            string volLots = entVolume.Text.Replace(numberFormat.NumberGroupSeparator, "");
            string VolText = entVolume.Text.Replace(numberFormat.NumberGroupSeparator, "").Replace("-", "");
            int CheckVolumeOfTrade;
            if (UseLots)
            {
                int.TryParse((this.SymbolLotSize * Straticator.Utility.Double.parseFloat
                     (entVolume.Text.Replace(numberFormat.NumberGroupSeparator, ""))).ToString("N0"),
                     System.Globalization.NumberStyles.AllowThousands,
                     numberFormat, out CheckVolumeOfTrade);
                if (string.IsNullOrWhiteSpace(entVolume.Text) || CheckVolumeOfTrade <= 0)
                {
                    msg = ChangeCulture.Lookup("ValLotVolume");
                    title = ChangeCulture.Lookup("Invalid");
                    App.Current.MainPage.DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                    return false;
                }
            }
            else
            {
                int.TryParse(entVolume.Text, System.Globalization.NumberStyles.AllowThousands, numberFormat, out CheckVolumeOfTrade);
                if (string.IsNullOrWhiteSpace(VolText) || CheckVolumeOfTrade <= 0)
                {
                    msg = ChangeCulture.Lookup("ValVolumne");
                    title = ChangeCulture.Lookup("Invalid");

                    App.Current.MainPage.DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                    return false;
                }
            }
            selMarketType = pkrOrderType.SelectedIndex;

            UserOrderType orderType = (UserOrderType)selMarketType;
            if (selMarketType > (int)UserOrderType.Quoted)
            {
                double LimitPrice = Convert.ToDouble(entLimit.Text);
                if (double.IsNaN(LimitPrice) || LimitPrice <= 0)
                {
                    msg = ChangeCulture.Lookup("ValPrice");
                    title = ChangeCulture.Lookup("Invalid");
                    App.Current.MainPage.DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                    return false;
                }
                //string errorMsg = AmendOrder.validatePriceLimit(sell, float.Parse(numUpDown_Limit.Text), _currDisplaySymbol, (byte)orderType);
                //if (!string.IsNullOrWhiteSpace(errorMsg))
                //{
                //    msg = ChangeCulture.Lookup(errorMsg);
                //    title = ChangeCulture.Lookup("Invalid");
                //    var dialog = new CustomMessageBoxWithTitle(title, msg, ChangeCulture.Lookup("OK"));
                //    dialog.Show(SupportFragmentManager, "dialog");
                //    return false;
                //}
            }
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl != 0 && sl < 3)
            {
                msg = ChangeCulture.Lookup("ValStopLoss");
                title = ChangeCulture.Lookup("Invalid");
                App.Current.MainPage.DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                return false;
            }
            if (UseTracks)
            {
                var track = Convert.ToDouble(entTrack.Text.Equals("") ? "0.0" : entTrack.Text);
                if (track < 1)
                {
                    msg = ChangeCulture.Lookup("ValTrack");
                    title = ChangeCulture.Lookup("Invalid");
                    App.Current.MainPage.DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                    return false;
                }
            }
            if (pkrExpiry.SelectedIndex== (int)UserOrderDuration.Timeframe)
                SetTimeFrame();

            return true;
        }

        private void SetTimeFrame()
        {
            TimeFrameResolution tfr = TimeFrameResolution.Seconds;
            if (swtMinHrs.IsToggled== true)
                tfr = TimeFrameResolution.Minutes;
            else 
                tfr = TimeFrameResolution.Hours;
            _timefrm = new TimeFrame(tfr, Convert.ToInt32(entExpiryVolume.Text));
        }

        public void setTextofUpDown()
        {
            if (string.IsNullOrEmpty(entExpiryVolume.Text))
                entExpiryVolume.Text = "0";
            if (string.IsNullOrEmpty(entExpiryVolume.Text))
                entExpiryVolume.Text = "0";
            if (string.IsNullOrEmpty(entTrack.Text))
                entTrack.Text = "0";
            if (string.IsNullOrEmpty(entLimit.Text))
                entLimit.Text = "0";
        }

        private async void LblSymbolId_Clicked(object sender, EventArgs e)
        {
            //Application.Current.MainPage = new NavigationPage(new WatchlistPage());
            //await Navigation.PushAsync(new NavigationPage(new WatchlistPage()));
            await Navigation.PushModalAsync(new PositionPage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("OrderstitleKey"), ChangeCulture.Lookup("TradestitleKey"), ChangeCulture.Lookup("PositionsKey"),
                ChangeCulture.Lookup("ChartKey"));
            int action = 0;
            if (actionSheet == ChangeCulture.Lookup("OrderstitleKey")) action = 1;
            else if (actionSheet == ChangeCulture.Lookup("TradestitleKey")) action = 2;
            else if (actionSheet == ChangeCulture.Lookup("PositionsKey")) action = 3;
            else if (actionSheet == ChangeCulture.Lookup("ChartKey")) action = 4;

            switch (action)
            {
                case 1:
                    await Navigation.PushModalAsync(new OrderReportPage());
                    break;
                case 2:
                    await Navigation.PushModalAsync(new TradeReportPage());
                    break;
                case 3:
                    await Navigation.PushModalAsync(new PositionCurrentSymbolPage(selectedSymbolId));
                    break;
                case 4:
                    await Navigation.PushModalAsync(new ChartPage(selectedSymbolId));
                    break;
            }
        }

        private void PkrOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrOrderType.SelectedItem != null)
            {
                int orderType = pkrOrderType.SelectedIndex;
                if (orderType > (int)UserOrderType.Quoted)
                {
                    if (Convert.ToDouble(entLimit.Text) == 0d)
                    {
                        var info = MarketInfo.getMarketInfo(_currDisplaySymbol);
                        Limit_Step = 1d / info.PipsMultiplier;
                        entLimit.Text = Convert.ToString(Math.Round((info.LatestPrice.Ask + info.LatestPrice.Bid) / 2, info.PriceDecimal));
                    }
                    StkLimit.IsVisible = true;
                }
                else
                    StkLimit.IsVisible = false;
            }
        }

        private void PkrExpiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            StkExpiry.IsVisible= (pkrExpiry.SelectedIndex== (int)UserOrderDuration.Timeframe) ? true: false;
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PopModalAsync();
            return true;
        }
    }
}