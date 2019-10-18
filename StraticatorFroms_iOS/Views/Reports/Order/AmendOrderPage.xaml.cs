using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.IndicatorDataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Reports.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AmendOrderPage : ContentPage
    {
        TradingAPI tradingApi;
        int Aid;
        public long OrderId;
        LiveChartTrader.BaseClass.OrderDetail editOdr;
        TimeFrame timeframe;
        double prevVolume;
        int SymbolLotSize;
        bool UseLots;
        double Limit_Step, Limit_MaxValue, StopLoss_MaxValue, TakeProfit_MaxValue;

        NumberFormatInfo numberFormat;
        Session session;
        public AmendOrderPage(OrderDetail param)
        {
            InitializeComponent();
            numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;

            timeframe = new TimeFrame(60);
            Limit_Step = 0.0001;
            Limit_MaxValue = 100000;
            StopLoss_MaxValue = 1000;
            TakeProfit_MaxValue = 1000;

            tradingApi = new TradingAPI(SessionManager.Instance.Session);
            OrderId = param.oid;
            Aid = param.Aid;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            session = SessionManager.Instance.Session;
            UseLots = session.Settings.UserLots;

            btnConfirm.Text = ChangeCulture.Lookup("Confirm");
            btnCancel.Text = ChangeCulture.Lookup("Cancel");
            lblPrice.Text = ChangeCulture.Lookup("Pricekey"); ;

            #region Spinner
            #endregion

            #region Up Down Controls


            lblTitle.Text = ChangeCulture.Lookup("AmendOrder");

            lblExpiry.Text = ChangeCulture.Lookup("Expiry");

            //radiomin.Text = ChangeCulture.Lookup("MinKey");
            //radiohrs.Text = ChangeCulture.Lookup("hrsKey");

            lblTakeProfit.Text = ChangeCulture.Lookup("HTakeProfit");
            lblStopLoss.Text = ChangeCulture.Lookup("HStopLoss");
            lblTrack.Text = ChangeCulture.Lookup("Track");
            lblTrailingStop.Text = ChangeCulture.Lookup("TrailingStop");
            lblVolume.Text = ChangeCulture.Lookup("MPVolume");
            // lblLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            #endregion

            //txtVolume = view.FindViewById<EditText>(Resource.Id.etVolume);
            //txtLots = view.FindViewById<EditText>(Resource.Id.etLots);
            //if (UseLots)
            //{
            //    lblVolume.Visibility = ViewStates.Gone;
            //    txtVolume.Visibility = ViewStates.Gone;
            //}
            //else
            //{
            //    lblLots.Visibility = ViewStates.Gone;
            //    txtLots.Visibility = ViewStates.Gone;
            //}
            //chTrailingStop = view.FindViewById<CheckBox>(Resource.Id.chTrailingStop);

            //tvsymbolId = view.FindViewById<TextView>(Resource.Id.tvsymbolId);
            //tvlimitsymbolId = view.FindViewById<TextView>(Resource.Id.tvlimitsymbolId);
            //txtLots.FocusChange += txtLots_FocusChange;

            FillComboBox("DurationType");
            AmendOrder_Loaded();
        }

        private void FillComboBox(string key)
        {
            var arr = new string[] { "Good til Cancel", "End of Day", "End of Week", "Immediate or Cancel", "Fill or Kill", "Time Frame" };
            pkrExpiry.ItemsSource = arr;
            pkrExpiry.SelectedIndex = 0;

        }

        void AmendOrder_Loaded()
        {
            GetOrders();
        }

        private async void GetOrders()
        {
            if (OrderId == 0)
                return;

            try
            {
                editOdr = await tradingApi.GetOrderAsync(OrderId, Aid);
                if (editOdr == null)
                    return;
            }
            catch
            {
                //Dismiss();
            }
            var info = MarketInfo.getMarketInfo(editOdr.symbolId);
            this.SymbolLotSize = info.Lotsize;
            lblSymbolName.Text = MarketInfo.getTradingName(editOdr.symbolId);
            lblCurrencySymbol.Text = info.TradedCurrencyId.ToString();
            //if (UseLots)
            //    txtLots.Text = new Lot(editOdr.volume, editOdr.symbolId).ToString("", numberFormat);
            //else
            entVolume.Text = editOdr.volume.ToString("N0");
            pkrExpiry.SelectedIndex = Convert.ToInt32(editOdr.duration);

            entTakeProfit.Text = editOdr.tp == 0 ? string.Empty : Convert.ToString(editOdr.tp);
            entStopLoss.Text = editOdr.sl == 0 ? string.Empty : Convert.ToString(editOdr.sl);
            chkTrailingStop.IsChecked = editOdr.SL_is_Trailing;
            entPrice.Text = Convert.ToString(editOdr.price);

            if (UserOrderType.Market == (UserOrderType)editOdr.orderType || UserOrderType.Quoted == (UserOrderType)editOdr.orderType)
            {
                entStopLoss.IsEnabled = true;
                entTakeProfit.IsEnabled = true;
                chkTrailingStop.IsEnabled = true;

                StoplossUp.IsEnabled = true;
                StoplossDown.IsEnabled = true;

                TakeProfitUp.IsEnabled = true;
                TakeProfitDown.IsEnabled = true;
            }
            else if (UserOrderType.LimitEntry == (UserOrderType)editOdr.orderType || UserOrderType.BuySellStop == (UserOrderType)editOdr.orderType)
            {
                entStopLoss.IsEnabled = true;
                entTakeProfit.IsEnabled = true;
                chkTrailingStop.IsEnabled = true;

                StoplossUp.IsEnabled = true;
                StoplossDown.IsEnabled = true;

                TakeProfitUp.IsEnabled = true;
                TakeProfitDown.IsEnabled = true;
            }
            else
            {
                entStopLoss.IsEnabled = false;
                entTakeProfit.IsEnabled = false;
                chkTrailingStop.IsEnabled = false;

                StoplossUp.IsEnabled = false;
                StoplossDown.IsEnabled = false;

                TakeProfitUp.IsEnabled = false;
                TakeProfitDown.IsEnabled = false;
            }
            if (pkrExpiry.SelectedIndex == (int)UserOrderDuration.Timeframe)
            {
                StkExpiry.IsVisible = true;
                if (editOdr.Seconds > 0)
                {
                    timeframe = new TimeFrame(editOdr.Seconds);
                    if (timeframe.TimeFrameType == TimeFrameResolution.Minutes)
                    {
                        swtMinHrs.IsToggled = true;
                        entExpiryVolume.Text = Convert.ToString(timeframe.Minutes);
                    }
                    else if (timeframe.TimeFrameType == TimeFrameResolution.Hours)
                    {
                        swtMinHrs.IsToggled = false;
                        entExpiryVolume.Text = Convert.ToString(timeframe.Hours);
                    }
                }
            }
            else
                StkExpiry.IsVisible = false;
        }

        private void BtnConfirm_Clicked(object sender, EventArgs e)
        {
            SetTextOfUpDown();

            bool sell = (editOdr.volume < 0);
            if (Validate(sell))
                SetUpdateOrderValue();
        }

        private void SetUpdateOrderValue()
        {
            throw new NotImplementedException();
        }

        private bool Validate(bool sell)
        {
            string msg;
            string title;
            //string volLots = txtLots.Text.Replace(numberFormat.NumberGroupSeparator, "").Replace("-", "");
            string VolText = entVolume.Text.Replace(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator, "").Replace("-", "");
            if (VolText.Equals(""))
            {
                VolText = "0";
            }
            int CheckVolumeOfTrade;
            if (UseLots)
            {
                //int.TryParse((this.SymbolLotSize * Straticator.Utility.Double.parseFloat
                //     (txtLots.Text.Replace(numberFormat.NumberGroupSeparator, ""))).ToString("N0"),
                //     System.Globalization.NumberStyles.AllowThousands,
                //     numberFormat, out CheckVolumeOfTrade);
                //if (string.IsNullOrWhiteSpace(txtLots.Text) || CheckVolumeOfTrade <= 0)
                //{
                //    msg = ChangeCulture.lookup("ValLotVolume");
                //    title = ChangeCulture.lookup("Invalid");
                //    var dialog = new CustomMessageBoxWithTitle(title, msg, ChangeCulture.lookup("OK"));
                //    dialog.Show(objSupportFragmentManager, "dialog");
                //    return false;
                //}
            }
            else
            {
                int.TryParse(entVolume.Text, System.Globalization.NumberStyles.AllowThousands, numberFormat, out CheckVolumeOfTrade);
                if (string.IsNullOrWhiteSpace(VolText) || CheckVolumeOfTrade <= 0)
                {
                    msg = ChangeCulture.Lookup("ValVolumne");
                    title = ChangeCulture.Lookup("Invalid");

                    DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                    return false;
                }
            }
            string PriceText = entPrice.Text;
            if (PriceText.Equals(""))
            {
                PriceText = "0.0";
            }
            float price = (float)Convert.ToDouble(PriceText);
            if (float.IsNaN(price) || (price == 0d && !string.IsNullOrWhiteSpace(PriceText)))
            {
                msg = ChangeCulture.Lookup("ValPrice");
                title = ChangeCulture.Lookup("Invalid");
                DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                return false;
            }
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl != 0 && sl < 3)
            {
                msg = ChangeCulture.Lookup("ValStopLoss");
                title = ChangeCulture.Lookup("Invalid");
                DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                return false;
            }
            if (editOdr.triggerOid.HasValue)  // if we have a trigger order, and we cannot evaluate price, since order is not open yet.
                return true;

            string errorMsg = ValidatePriceLimit(sell, price, editOdr.symbolId, editOdr.orderType);
            if (!string.IsNullOrWhiteSpace(errorMsg))
            {
                msg = ChangeCulture.Lookup(errorMsg);
                title = ChangeCulture.Lookup("Invalid");
                DisplayAlert(title, msg, ChangeCulture.Lookup("OK"));
                return false;
            }
            return true;
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

        private void TrackUp_Clicked(object sender, EventArgs e)
        {
            entTrack.Text = Convert.ToString(Convert.ToInt32(entTrack.Text) + 1);
        }

        private void TrackDown_Clicked(object sender, EventArgs e)
        {
            if (Convert.ToInt32(entTrack.Text) > 0)
                entTrack.Text = Convert.ToString(Convert.ToInt32(entTrack.Text) - 1);
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

        private void PkrExpiry_SelectedIndexChanged(object sender, EventArgs e)
        {
            StkExpiry.IsVisible = (pkrExpiry.SelectedIndex == (int)UserOrderDuration.Timeframe) ? true : false;
        }

        public static string ValidatePriceLimit(bool sell, float price, short SymbolId, byte marketType)
        {
            if (UserOrderType.Quoted != (UserOrderType)marketType && UserOrderType.Market != (UserOrderType)marketType)
            {
                var currSymbolPrice = MarketInfo.getMarketInfo(SymbolId).LatestPrice;
                float sellPrice = currSymbolPrice.BidPrice;
                float buyPrice = currSymbolPrice.AskPrice;
                float LimitPrice = price;
                int err = 0;

                if (sellPrice == 0f || buyPrice == 0f)  // we do not have prices. We cannot eveluate price input. This will also be done on the server, so not a problem.
                    return string.Empty;

                if ((byte)UserOrderType.LimitEntry == marketType || (byte)UserOrderType.LimitTakeProfit == marketType)
                {
                    if (!sell)
                    {
                        if (LimitPrice > buyPrice)
                            err = 1;
                    }
                    else
                    {
                        if (LimitPrice < sellPrice)
                            err = 2;
                    }
                }
                else if ((byte)UserOrderType.LimitStopLoss == marketType || (byte)UserOrderType.LimitTrailingStopLoss == marketType || (byte)UserOrderType.BuySellStop == marketType)
                {
                    if (!sell)
                    {
                        if (LimitPrice < buyPrice)
                            err = 3;
                    }
                    else
                    {
                        if (LimitPrice > sellPrice)
                            err = 4;
                    }
                }

                if (err != 0)
                {
                    string msg1 = ChangeCulture.Lookup(string.Format("ErrorinLimitprice{0}", err));
                    return msg1;
                }
            }
            return string.Empty;
        }

        private void SetTextOfUpDown()
        {
            if (entExpiryVolume.Text.Equals(""))
                entExpiryVolume.Text = "0";

            if (entVolume.Text.Equals(""))
                entVolume.Text = "0";

            if (entTrack.Text.Equals(""))
                entTrack.Text = "0";

            if (entLimit.Text.Equals(""))
                entLimit.Text = "0";

            if (entPrice.Text.Equals(""))
                entPrice.Text = "0";
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}