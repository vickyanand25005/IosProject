using LiveChartTrader.BaseClass;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Reports
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TradeStatisticsReportPage : ContentPage
    {

        protected ReportAPI reportAPI;
        protected IList<Exposure> ReportList;
        List<ViewModels.AmountExposure> amountExposures;

        TradeStatisticsReportViewModel tradeStatisticsReportViewModel;

        public ObservableCollection<ChartDataPoint> DoughnutSeriesData { get; private set; }

        public TradeStatisticsReportPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext =tradeStatisticsReportViewModel = new TradeStatisticsReportViewModel();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            lblTitle.Text = ChangeCulture.Lookup("TradeStatistics");
            lblFromDate.Text = ChangeCulture.Lookup("FromDate");
            lblToDate.Text = ChangeCulture.Lookup("ToDate");
            lblSymbol.Text = ChangeCulture.Lookup("currency");
            lblVolume.Text = ChangeCulture.Lookup("MPVolume").Replace(":", "");
            lblLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");

            lblPrice.Text = ChangeCulture.Lookup("AmountUSD");
            lblPer.Text = "%";

            lblLstTrade.Text = ChangeCulture.Lookup("Trades");
            btnSearch.Text = ChangeCulture.Lookup("Searchkey");
            if (SessionManager.Instance.Session.Settings.UserLots)
                lblVolume.IsVisible = false;
            else
                lblLots.IsVisible = false;
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            dtpFromDate.Date = DateTime.Now.AddMonths(-6);
            InitializeChart();
        }

        private void InitializeChart()
        {
            LoadData();
        }

        private async void LoadData()
        {
            //selectedReport = 0;
            DateTime toDate = CommonReport.getToDate(dtpToDate.Date);
            DateTime fromDate = Convert.ToDateTime(dtpFromDate.Date).ToUniversalTime();
            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            ReportList = await reportAPI.SearchTradeStatisticAsync(fromDate, toDate, 0, IdType.AccountId, aid, demo);

            if (ReportList != null)
            {
                try
                {
                    amountExposures = tradeStatisticsReportViewModel.LoadReport(ReportList.ToList());
                }
                catch (Exception)
                {
                    throw;
                }
                LoadChart(amountExposures);
            }
            else
            {
                // symbolChart.SetMenuVisibility(false);
            }
        }

        private void LoadChart(List<ViewModels.AmountExposure> exposure)
        {
            DoughnutSeriesData = new ObservableCollection<ChartDataPoint>();

            int len = exposure.Count;
            for (int i = 0; i < len; i++)
            {
                DoughnutSeriesData.Add(new ChartDataPoint(exposure[i].Currency, Math.Round(exposure[i].Pct, 1)));
            }

            doughnutSeries.ItemsSource = DoughnutSeriesData;
            doughnutSeries.XBindingPath = "XValue";
            doughnutSeries.YBindingPath = "YValue";
            doughnutSeries.EnableAnimation = true;
            doughnutSeries.DataMarker = new ChartDataMarker
            {
                ShowLabel = true,
                ShowMarker = true
            };
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }
    }
}