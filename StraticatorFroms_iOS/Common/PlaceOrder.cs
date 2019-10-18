using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StraticatorAPI;
using Straticator.LocalizationConverter;
using System.Threading;
using LiveChartTrader.Common;
using Straticator.Common;
using System.Globalization;
using StraticatorFroms_iOS;
using Xamarin.Forms;
using StraticatorFroms_iOS;

namespace Straticator
{
    public class PlaceOrder
    {
        public delegate void NoServerResponse(object args);
        public delegate void OrderRequestStart(object args);
        public delegate void OrderRequestEnd(object args);
        public TradingAPI tradingApi;
        public readonly LiveChartTrader.BaseClass.OrderDetail orderDetail;
        public event NoServerResponse ServerNotResponded;
        public event OrderRequestStart OrderRequested;
        public event OrderRequestEnd OrderResponded;
        int CheckVolumeOfTrade;
        System.Timers.Timer timer;
        long orderId;
        string tradeConfirmation;
        NumberFormatInfo numberFormat;

        public PlaceOrder(TradingAPI tradingApi, LiveChartTrader.BaseClass.OrderDetail orderDetail)
        {
            this.tradingApi = tradingApi;
            this.orderDetail = orderDetail;
            tradeConfirmation = ChangeCulture.Lookup("TradeConfirmation").Replace(":", "");
            numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
        }

        private void StartTimer()
        {
            timer = new System.Timers.Timer() { Interval = 2000 };
            timer.Elapsed += (s, ea) => CheckOrderStatus();
            timerDuration = 0;
            timer.Start();
        }

        private void StopTimer()
        {
            if (timer != null && timer.Enabled)
                timer.Stop();
            timerDuration = 0;
        }

        void ShowTrade(OrderResult res)
        {
            string formattedString;
            orderId = 0;
            if (Math.Abs(CheckVolumeOfTrade) == Math.Abs(res.Volume))
                formattedString = UtilityFunctions.SetOrderConfirmationMsg(res);
            else
                formattedString = UtilityFunctions.SetOrderConfirmationMsg(res, false, CheckVolumeOfTrade);

            App.Current.MainPage.DisplayAlert(tradeConfirmation, formattedString, ChangeCulture.Lookup("OK"));
            StopTimer();
        }

        public async System.Threading.Tasks.Task SubmitOrder(OrderAction Action)
        {
            this.CheckVolumeOfTrade = orderDetail.volume;
            if (this.OrderRequested != null)
            {
                OrderRequested(null);
            }
            var orderResult = await tradingApi.ClientOrderAsync(orderDetail, Action, 1000);

            if (this.OrderResponded != null)
            {
                OrderResponded(null);
            }

            if (orderResult == null)
            {
                UtilityFunctions.ShowErrorMessage(ErrorCodes.Exception);
                return;
            }

            switch (orderResult.err)
            {
                case ErrorCodes.Succes:
                    if (Action == OrderAction.Cancel)
                    {
                        await App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("Confirmation"), ChangeCulture.Lookup("OrderCancelledkey"), ChangeCulture.Lookup("OK"));
                        return;
                    }

                    orderId = orderResult.Oid;
                    if (orderResult.Price != 0f)
                        ShowTrade(orderResult);
                    else
                    {
                        /*Order*/
                        if (orderDetail.UserOrderType > UserOrderType.Quoted)
                        {
                            string formattedString = UtilityFunctions.SetOrderConfirmationMsg(orderResult);

                            await App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("OrderConfirmation").Replace(":", ""), formattedString, ChangeCulture.Lookup("OK"));
                        }
                        else
                        {
                            StartTimer();
                        }
                    }
                    break;

                case ErrorCodes.VolumeTooSmall:
                case ErrorCodes.VolumeNotInMultipleOfMinVolume:
                    string volume;
                    if (Common.SessionManager.Instance.Session.Settings.UserLots)
                    {
                        volume = new Lot(orderResult.Volume, orderResult.SymbolId).ToString("", numberFormat);
                    }
                    else
                        volume = orderResult.Volume.ToString("N0", numberFormat);
                    string[] param = new string[] { volume };
                    UtilityFunctions.ShowErrorMessage(orderResult.err, null, param);
                    break;
                case ErrorCodes.OrderRejected:
                    string error = string.Format(ChangeCulture.Lookup("OrderRejected"), orderId, orderResult.ErrorText);
                    await App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("OrderRejected"), error, ChangeCulture.Lookup("OK"));
                    break;
                default:
                    UtilityFunctions.ShowErrorMessage(orderResult.err);
                    break;
            }
        }

        int timerDuration = 0;

        private async void CheckOrderStatus()
        {
            //if (orderId == 0 || !Login.IsNetworkConnection(tempContext))
            //    return;

            OrderResult orderResult;
            try
            {
                orderResult = await tradingApi.OrderStatusAsync(orderId);
            }
            catch
            {
                orderResult = null;
            }

            if (orderResult == null)
                return;

            if (orderResult.err == ErrorCodes.Succes)
            {
                if (orderResult.Price != 0f)
                    ShowTrade(orderResult);
                else
                {
                    /* Order */
                    if (timerDuration == 0) /*Show message only first time */
                    {
                        string formattedString = UtilityFunctions.SetOrderConfirmationMsg(orderResult, true);

                        await App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("OrderConfirmation").Replace(":", ""), formattedString, ChangeCulture.Lookup("OK"));
                    }
                }
            }
            else if (orderResult.err == ErrorCodes.OrderRejected)
            {
                string error = string.Format(ChangeCulture.Lookup("OrderRejected"), orderId, orderResult.ErrorText);

                await App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("Error"), error, ChangeCulture.Lookup("OK"));

                StopTimer();
            }
            else
            {
                UtilityFunctions.ShowErrorMessage(orderResult.err, orderResult.ErrorText);
                StopTimer();
            }

            if (timerDuration == 60)/* After 1 minute stop the timer*/
            {
                StopTimer();
                ShowNoServerResponseMessage();
            }
            else
            {
                timerDuration += 2;
            }
        }

        private void ShowNoServerResponseMessage()
        {
            string msg = string.Format(ChangeCulture.Lookup("OrderNoResponseMsg"), "~");
            //var dialog = new CustomMessageBox(msg.Split('~')[0], msg.Split('~')[1], ChangeCulture.Lookup("GoOrderHistory"), ChangeCulture.Lookup("OK"), typeof(OrderHistoryReport));
            //dialog.Show(objSupportFragmentManager, "dialog");
        }

    }


}