using LiveChartTrader.BaseClass;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views.Custom;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Reports.PLReport
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PLReportPage : ContentPage
    {

        IList<AccountPLReport> accountPL;
        PLReportViewModel pLReportViewModel;
        protected ReportAPI reportAPI;
        static int selectedGraphPosition = 4;

        public ObservableCollection<ChartDataPoint> SeriesData { get; private set; }

        public PLReportPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            LocalizePage();
            BindGraph();
            LoadChart();
        }

        private void LocalizePage()
        {
            lblTitle.Text = ChangeCulture.Lookup("ProfitLossReport");
            lblGraph.Text = string.Format("{0}:", ChangeCulture.Lookup("Graph"));
            lblFromDate.Text = ChangeCulture.Lookup("FromDate");
            lblToDate.Text = ChangeCulture.Lookup("ToDate");
            btnSearch.Text = ChangeCulture.Lookup("Searchkey");
            lblSymbolPopup.Text = ChangeCulture.Lookup("Symbol").Replace(":", "");
            lblInterval.Text = ChangeCulture.Lookup("Interval").Replace(":", "");
            btnSave.Text = ChangeCulture.Lookup("savehidekey").Replace(":", "");
            btnCancel.Text = ChangeCulture.Lookup("cancel").Replace(":", "");
        }

        private void BindGraph()
        {
            List<string> graphType = new List<string>();
            graphType.Add(ChangeCulture.Lookup("MarginUsed"));
            graphType.Add(ChangeCulture.Lookup("PL"));
            graphType.Add(ChangeCulture.Lookup("PLSum"));
            graphType.Add(ChangeCulture.Lookup("Balance"));
            graphType.Add(string.Format("{0}{1}", ChangeCulture.Lookup("Profit"), "%"));
            pkrGraph.ItemsSource = graphType;
            pkrGraph.SelectedIndex = 4;
        }

        private async void LoadChart()
        {
            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            short symbolId = 0;
            if(pkrSymbolPopup.SelectedItem!=null)
                symbolId = CommonReport.getSymbol(pkrSymbolPopup);
            DateTime toDate = CommonReport.getToDate(dtpToDate.Date);
            int interval = 60 * 60;

            switch (pkrInterval.SelectedIndex)
            {
                case 1:
                    interval = 4 * interval;
                    break;
                case 2:
                    interval = 24 * interval;
                    break;
            }
            DateTime FromDate = Convert.ToDateTime(dtpFromDate.Date).ToUniversalTime();
            accountPL = await reportAPI.GetAccountPeriodPL(FromDate, toDate, aid, symbolId, interval, true);
            if (accountPL != null && accountPL.Count > 0)
                DrawChartValues(accountPL);
        }

        private void DrawChartValues(IList<AccountPLReport> lstAccountPL)
        {
            //this.BindingContext = pLReportViewModel = new PLReportViewModel(pkrGraph.SelectedIndex, lstAccountPL);
            SeriesData = new ObservableCollection<ChartDataPoint>();

            int i = 1;

            switch (pkrGraph.SelectedIndex)
            {
                case 1:
                    foreach (var item in lstAccountPL)
                    {
                        i++;
                        SeriesData.Add(new ChartDataPoint(i, item.PLInterval));
                    }
                    break;
                case 2:
                    i = 1;
                    foreach (var item in lstAccountPL)
                    {
                        i++;
                        SeriesData.Add(new ChartDataPoint(i.ToString(), item.PLSum));
                    }
                    break;
                case 3:
                    i = 1;
                    foreach (var item in lstAccountPL)
                    {
                        i++;
                        SeriesData.Add(new ChartDataPoint(i.ToString(), item.balance));
                    }
                    break;
                case 4:
                    i = 1;
                    foreach (var item in lstAccountPL)
                    {
                        i++;
                        SeriesData.Add(new ChartDataPoint(i.ToString(), item.pctProfit));
                    }
                    break;
                case 0:
                default:
                    i = 100;
                    foreach (var item in lstAccountPL)
                    {
                        i++;
                        SeriesData.Add(new ChartDataPoint(i.ToString(), item.pctMarginUsed));
                    }
                    break;
            }


            SeriesChart.ItemsSource = SeriesData;
            SeriesChart.XBindingPath = "XValue";
            SeriesChart.YBindingPath = "YValue";
            SeriesChart.EnableAnimation = true;
            PLChart.PrimaryAxis.AxisLineStyle = new ChartLineStyle();
            PLChart.PrimaryAxis.AxisLineStyle.StrokeColor = Color.FromRgba(166, 166, 166, 166);

            PLChart.SecondaryAxis.AxisLineStyle = new ChartLineStyle();
            PLChart.SecondaryAxis.AxisLineStyle.StrokeWidth = 1.0f;
            PLChart.SecondaryAxis.MajorTickStyle = new ChartAxisTickStyle();
            PLChart.SecondaryAxis.MajorTickStyle.StrokeColor = Color.Red;
            PLChart.SecondaryAxis.AxisLineStyle.StrokeColor = Color.FromRgba(166, 166, 166, 166);

            //SeriesChart.DataMarker = new ChartDataMarker
            //{
            //    ShowLabel = true,
            //    ShowMarker = true
            //};
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            LoadPopupControls();
            var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("SettingsKey"));
            if (actionSheet == ChangeCulture.Lookup("SettingsKey"))
            {
                overlay.IsVisible = true;
            }
        }

        private void LoadPopupControls()
        {
            CommonReport.loadSymbols(ref pkrSymbolPopup);
            BindInterval();
        }

        private void BindInterval()
        {
            string[] intervals = ChangeCulture.LookupEnum("PLInterval");
            pkrInterval.ItemsSource = intervals;
            pkrInterval.SelectedIndex = 1;
        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            LoadChart();
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PositionPage());
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            LoadChart();
            overlay.IsVisible = false;
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = false;
        }

        private void PkrGraph_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (accountPL != null)
                DrawChartValues(accountPL);
        }

        private void CandleWatchlistChart_SelectionChanged(object sender, Syncfusion.SfChart.XForms.ChartSelectionEventArgs e)
        {

        }
    }
}