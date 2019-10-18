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

namespace StraticatorFroms_iOS.Views.Reports.TradeReport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradeReportPage : ContentPage
    {
        short _currDisplaySymbol;
        IList<UserAccountTrade> tradedetails;
        public static int SelectedTradeReportDetails = 0;
        protected ReportAPI reportAPI;

        TradeReportViewModel tradeReportViewModel;
        private TradeDetail selectedTradeDetail;

        public TradeReportPage()
        {
            InitializeComponent();
           
            
           // OnNavigatedTo();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            lblTitle.Text = ChangeCulture.Lookup("TradestitleKey");
            lblFromDate.Text = ChangeCulture.Lookup("FromDate");
            lblSymbolTitle.Text = ChangeCulture.Lookup("Symbol");
            lblToDate.Text = ChangeCulture.Lookup("ToDate");
            lblOrderId.Text = ChangeCulture.Lookup("OrderID").Replace(":", "");
            lblSymbolList.Text = ChangeCulture.Lookup("Symbolkey").Replace(":", "");
            lblTime.Text = ChangeCulture.Lookup("Time").Replace(":", "");
            lblPrice.Text = ChangeCulture.Lookup("Price").Replace(":", "");
            lblVolume.Text = ChangeCulture.Lookup("MPVolume").Replace(":", "");
            dtpFromDate.Date = DateTime.Now.AddMonths(-6);
            this.BindingContext = tradeReportViewModel = new TradeReportViewModel();
            TradeReport_Loaded();
        }

        private async void TradeReport_Loaded()
        {
            CommonReport.loadSymbols(ref pkrSymbol);
            ActivityIndicator activityIndicator = new ActivityIndicator();
            activityIndicator.IsRunning = true;
            activityIndicator.IsEnabled = true;
            int symbolId;
            DateTime toDate;
            DateTime fromDate;
            if (_currDisplaySymbol == 0)
            {

                gridFilter1.IsVisible = true;
                gridFilter2.IsVisible = true;
                lblSymbolTitle.IsVisible= false;
                symbolId = CommonReport.getSymbol(pkrSymbol);
                fromDate = Convert.ToDateTime(dtpFromDate.Date);
                toDate = CommonReport.getToDate(dtpToDate.Date);
            }
            else
            {
                gridFilter1.IsVisible = false;
                gridFilter2.IsVisible = false;
               lblSymbolTitle.IsVisible = true;
                symbolId = _currDisplaySymbol;
               lblSymbol.Text = MarketInfo.getSymbol(_currDisplaySymbol);
                fromDate = DateTime.Now.Date.AddDays(-7);
                toDate = DateTime.MinValue;
            }
            SelectedTradeReportDetails = 0;
           // flxGridReport.Adapter = null;
            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            tradedetails = await reportAPI.SearchAccountTradeAsync(fromDate.Date.ToUniversalTime(), toDate, 0, symbolId, IdType.AccountId, aid, demo);
            if (tradedetails != null)
               // lstView.ItemsSource = tradedetails;
            tradeReportViewModel.LoadTradeReport(tradedetails);
               // flxGridReport.Adapter = new TradeReportAdapter(this, tradedetails);
        }

        private void OnNavigatedTo()
        {
            var parameter = NavigatePages.PageContext();
            if (parameter != null)
                _currDisplaySymbol = (short)parameter;
            //if (_currDisplaySymbol == 0)
                
        }

        private void PkrSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            TradeReport_Loaded();
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedTradeDetail = (TradeDetail)e.SelectedItem;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selectedTradeDetail != null)
            {
                var res = await DisplayActionSheet("", ChangeCulture.Lookup("cancelKey"), "", ChangeCulture.Lookup("FVTrade"));
                if (res == ChangeCulture.Lookup("FVTrade"))
                {
                    var orderTicketPage = new OrderTicketPage(selectedTradeDetail.Symbol, selectedTradeDetail.SymbolId);
                    NavigationExtensions.Navigate(typeof(OrderTicketPage), selectedTradeDetail.SymbolId);
                    await Navigation.PushModalAsync(orderTicketPage);
                }
            }
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PositionPage());
        }
    }
}