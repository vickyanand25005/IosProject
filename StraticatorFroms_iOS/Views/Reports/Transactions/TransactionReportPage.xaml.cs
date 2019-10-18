using LiveChartTrader.BaseClass;
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

namespace StraticatorFroms_iOS.Views.Reports.Transactions
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionReportPage : ContentPage
    {
        public static int SelectedTransactionDetails = 0;
        IList<CommonUserAccountTrans> transactionDetails;

        TransactionReportViewModel transactionReportViewModel;
        ReportAPI reportAPI;
        TransactionDetail selectedTransactionDetail;
        public TransactionReportPage()
        {
            InitializeComponent();
            
        }


        private void Localize()
        {
            lblTitle.Text = ChangeCulture.Lookup("Transactions");
            lblSymbol.Text = ChangeCulture.Lookup("Symbol");
            lblType.Text = ChangeCulture.Lookup("Type").Replace(":", "");
            lblSymbolList.Text = ChangeCulture.Lookup("Symbolkey").Replace(":", "");
            lblTime.Text = ChangeCulture.Lookup("Time").Replace(":", "");
            lblAmount.Text = ChangeCulture.Lookup("curAmount").Replace(":", "");
            btnSearch.Text = ChangeCulture.Lookup("Searchkey");
            lblFromDate.Text = ChangeCulture.Lookup("FromDate");
            lblToDate.Text = ChangeCulture.Lookup("ToDate");
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            CommonReport.loadSymbols(ref pkrSymbol);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            this.BindingContext = transactionReportViewModel = new TransactionReportViewModel();
            Localize();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            reportAPI.OrderDetailType = typeof(ViewModels.OrderDetail);
            dtpFromDate.Date = DateTime.Now.AddMonths(-6);
            TransactionReport_Loaded();
        }

        private async void TransactionReport_Loaded()
        {
            try
            {
                SelectedTransactionDetails = 0;
                DateTime toDate = CommonReport.getToDate(dtpToDate.Date);
                bool demo = !SessionManager.Instance.Session.LiveLogin;
                int uid = SessionManager.Instance.Session.Uid;
                int aid = SessionManager.Instance.CurrentAccount;
                int symbolId = CommonReport.getSymbol(pkrSymbol);
                DateTime objFromDate = Convert.ToDateTime(dtpFromDate.Date);
                transactionDetails = await reportAPI.SearchUserAccountTransactionAsync(objFromDate.ToUniversalTime(), toDate, uid, (short)symbolId, IdType.AccountId, aid, demo, null);
                if (transactionDetails != null)
                    transactionReportViewModel.LoadTransactions(transactionDetails);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selectedTransactionDetail != null)
            {
                var res = await DisplayActionSheet("", ChangeCulture.Lookup("cancelKey"), "", ChangeCulture.Lookup("FVTrade"));
                if (res == ChangeCulture.Lookup("FVTrade"))
                {
                    var orderTicketPage = new OrderTicketPage(selectedTransactionDetail.Symbol, selectedTransactionDetail.SymbolId);
                    NavigationExtensions.Navigate(typeof(OrderTicketPage), selectedTransactionDetail.SymbolId);
                    await Navigation.PushModalAsync(orderTicketPage);
                }
            }
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedTransactionDetail = (TransactionDetail)e.SelectedItem;
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            TransactionReport_Loaded();
        }
    }
}