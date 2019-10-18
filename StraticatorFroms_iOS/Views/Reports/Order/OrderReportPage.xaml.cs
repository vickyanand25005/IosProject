using LiveChartTrader.BaseClass;
using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorFroms_iOS.Controls;


namespace StraticatorFroms_iOS.Views.Reports.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderReportPage : ContentPage
    {
        ReportAPI reportAPI;
        TradingAPI _tradingApi;
        short _currDisplaySymbol;
        public static ViewModels.OrderDetail SelectedOrderReportDetails;
        IList<LiveChartTrader.BaseClass.OrderDetail> orderdetails;

        OrderReportViewModel orderReportViewModel;

        public OrderReportPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Localize();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            reportAPI.OrderDetailType = typeof(ViewModels.OrderDetail);
            this.BindingContext = orderReportViewModel = new OrderReportViewModel();
            OrderReport_Loaded();
        }

        void OrderReport_Loaded()
        {
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            CommonReport.loadSymbols(ref pkrSymbol);
            LoadActiveOrders();
        }

        private async void LoadActiveOrders()
        {
            int symbolId;
            if (_currDisplaySymbol == 0)
            {
                gridFilter1.IsVisible = true;
                gridFilter2.IsVisible = true;
                lblSymbolTitle.IsVisible = false;
                symbolId = CommonReport.getSymbol(pkrSymbol);
            }
            else
            {
                gridFilter1.IsVisible = false;
                gridFilter2.IsVisible = false;
                lblSymbolTitle.IsVisible = true;
                symbolId = _currDisplaySymbol;
                lblSymbolList.Text = MarketInfo.getSymbol(_currDisplaySymbol);
            }
            //SelectedOrderReportDetails = 0;

            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            orderdetails = await reportAPI.SearchActiveOrderAsync(DateTime.MinValue, DateTime.MinValue, 0, symbolId, IdType.AccountId, aid, demo);
            if (orderdetails != null)
                orderReportViewModel.LoadOrders(orderdetails);
        }

        public void Localize()
        {
            lblTitle.Text = ChangeCulture.Lookup("OrderstitleKey");
            lblSymbol.Text = ChangeCulture.Lookup("Symbol");
            lblOrderId.Text = ChangeCulture.Lookup("OrderID").Replace(":", "");
            lblSymbolList.Text = ChangeCulture.Lookup("Symbolkey").Replace(":", "");
            lblTime.Text = ChangeCulture.Lookup("Time").Replace(":", "");
            lblPrice.Text = ChangeCulture.Lookup("Price").Replace(":", "");
            btnSearch.Text = ChangeCulture.Lookup("Searchkey");

            //var txtVolume = FindViewById<TextView>(Resource.Id.tvlistvolume);
            //txtVolume.Text = ChangeCulture.Lookup("MPVolume").Replace(":", "");
            //var txtLots = FindViewById<TextView>(Resource.Id.tvlistLots);
            //txtLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            //if (Common.SessionManager.Instance.Session.Settings.UserLots)
            //    txtVolume.Visibility = ViewStates.Gone;
            //else
            //    txtLots.Visibility = ViewStates.Gone;
        }

        protected void OnNavigatedTo()
        {
            var parameter = NavigatePages.PageContext();
            if (parameter != null)
                _currDisplaySymbol = (short)parameter;
            // if (_currDisplaySymbol == 0)

        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {

            LoadActiveOrders();
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            SelectedOrderReportDetails = (ViewModels.OrderDetail)e.SelectedItem;
            OrderDetailPage.currentOrder = SelectedOrderReportDetails;
        }

        private void PkrSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (SelectedOrderReportDetails != null)
            {
                var menus = string.Empty;
                if (_currDisplaySymbol == 0)
                    menus = ChangeCulture.Lookup("FVTrade");
                else
                    menus = ChangeCulture.Lookup("PositionsKey");

                var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("Details"), ChangeCulture.Lookup("Amend"),
                    ChangeCulture.Lookup("cancelKey"), menus);

                int action = 0;

                if (actionSheet == ChangeCulture.Lookup("Details"))
                {
                    action = 1;
                }
                else if (actionSheet == ChangeCulture.Lookup("Amend"))
                {
                    action = 2;
                }
                else if (actionSheet == ChangeCulture.Lookup("cancelKey"))
                {
                    action = 3;
                }

                else if (actionSheet == ChangeCulture.Lookup("FVTrade"))
                {
                    action = 4;
                }
                else if (actionSheet == ChangeCulture.Lookup("PositionsKey"))
                {
                    action = 5;
                }

                switch (action)
                {
                    case 1:
                        await Navigation.PushModalAsync(new OrderDetailPage());
                        break;
                    case 2:
                        await Navigation.PushModalAsync(new AmendOrderPage(SelectedOrderReportDetails));
                        break;
                    case 3:
                        btnCancel_Click();
                        break;
                    case 4:
                        await Navigation.PushModalAsync(new WatchlistPage());
                        break;
                    case 5:
                        await Navigation.PushModalAsync(new PositionPage());
                        break;
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(ChangeCulture.Lookup("SelectRowMessage"));
            }
        }

        private async void btnCancel_Click()
        {
            ViewModels.OrderDetail odr = SelectedOrderReportDetails;
            if (odr == null)
                return;

            var res = await DisplayAlert(ChangeCulture.Lookup(""), ChangeCulture.Lookup("StopFollowingMessage"), ChangeCulture.Lookup("OK"), ChangeCulture.Lookup("cancelKey"));
            if (res)
            {
                CancelOrder(odr);
            }
        }

        private async void CancelOrder(ViewModels.OrderDetail order)
        {
            if (_tradingApi == null)
                _tradingApi = new StraticatorAPI.TradingAPI(reportAPI.session);
            var odr = new PlaceOrder(_tradingApi, order);
            //odr.ServerNotResponded += delegate
            //{
            //    NavigationExtensions.Navigate(this, typeof(OrderHistoryReport));
            //};
            await odr.SubmitOrder(StraticatorAPI.OrderAction.Cancel);
            LoadActiveOrders();
        }
    }
}