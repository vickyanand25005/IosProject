using LiveChartTrader.Common;
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

namespace StraticatorFroms_iOS.Views.Reports.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryPage : ContentPage
    {

        public static int SelectedOrderHistoryDetails = 0;
        IList<CommonOrderArchive> orderArchieve;
        public readonly ReportAPI reportAPI;
        OrderHistoryViewModel orderHistoryViewModel;
        private OrderArchive selecteditem;

        public OrderHistoryPage()
        {
            InitializeComponent();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            dtpFromDate.Date = Convert.ToDateTime(DateTime.Now.AddDays(-2).Date.ToString(CommonReport.sysUIFormat));
            //dtpToDate.Date= "";
            reportAPI.CommonOrderArchiveType = typeof(OrderArchive);
            this.BindingContext = orderHistoryViewModel = new OrderHistoryViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            OrderReport_Loaded();
        }

        private void OrderReport_Loaded()
        {
            CommonReport.loadSymbols(ref pkrSymbol);
            LoadOrderArchieve();
        }

        private async void LoadOrderArchieve()
        {
            SelectedOrderHistoryDetails = 0;
            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            int symbolId = CommonReport.getSymbol(pkrSymbol);

            DateTime toDate = CommonReport.getToDate(dtpToDate.Date);
            DateTime objFromDate = Convert.ToDateTime(dtpFromDate.Date);
            orderArchieve = await reportAPI.SearchOrderArchieveAsync(objFromDate.ToUniversalTime(), toDate, 0, symbolId, IdType.AccountId, aid, demo);
            if (orderArchieve != null)
                orderHistoryViewModel.LoadOrderHistory(orderArchieve);
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

        private async void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selecteditem = (OrderArchive)e.SelectedItem;
            await Navigation.PushModalAsync(new OrderHistoryDetailPage(selecteditem));
        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            LoadOrderArchieve();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (selecteditem != null)
            {
                var res = await DisplayActionSheet("", ChangeCulture.Lookup("cancelKey"), "", ChangeCulture.Lookup("FVTrade"));
                if (res == ChangeCulture.Lookup("FVTrade"))
                {
                    var orderTicketPage = new OrderTicketPage(selecteditem.Symbol, selecteditem.SymbolId);
                    NavigationExtensions.Navigate(typeof(OrderTicketPage), selecteditem.SymbolId);
                    await Navigation.PushModalAsync(orderTicketPage);
                }
            }
        }

        private void PkrSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}