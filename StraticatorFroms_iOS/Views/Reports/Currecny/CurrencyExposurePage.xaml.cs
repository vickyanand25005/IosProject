using LiveChartTrader.BaseClass;
using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.Controls;
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

namespace StraticatorFroms_iOS.Views.Reports.Currecny
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrencyExposurePage : ContentPage
    {

        protected ReportAPI reportAPI;
        public ViewModels.AmountExposure selectedReport;
        protected IList<Exposure> ReportList;
        SymbolViewModel symbolViewModel;
        List<ViewModels.AmountExposure> amountExposures;
        public ObservableCollection<ChartDataPoint> DoughnutSeriesData { get; private set; }
        public CurrencyExposurePage()
        {
            InitializeComponent();
            reportAPI = new ReportAPI(SessionManager.Instance.Session);
            reportAPI.ExposureType = typeof(Straticator.Common.AmountExposure);
            
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.BindingContext = symbolViewModel = new SymbolViewModel();
            btnAid.Text = Convert.ToString(SessionManager.Instance.CurrentAccount);
            btnSearch.Text = ChangeCulture.Lookup("Searchkey");
            lblTitle.Text = ChangeCulture.Lookup("currency");
            lblPerDate.Text = ChangeCulture.Lookup("PrDate");

            lblCurrency.Text = ChangeCulture.Lookup("currency");
            lblAmount.Text = ChangeCulture.Lookup("Amount");
            //var txtLots = FindViewById<TextView>(Resource.Id.tvlots);
            //txtLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            lblPrice.Text = ChangeCulture.Lookup("AmountUSD");
            lblPer.Text = "%";
            if (SessionManager.Instance.Session.Settings.UserLots) { }
            //lblAmount.IsVisible = true;
            else
            {
                // txtLots.Visibility = ViewStates.Gone;
            }
            InitializeChart();
        }

        private void InitializeChart()
        {
            LoadData();
        }

        private async void LoadData()
        {

            DateTime perDate = CommonReport.getToDate(dtpFromDate.Date);
            bool demo = !SessionManager.Instance.Session.LiveLogin;
            int aid = SessionManager.Instance.CurrentAccount;
            ReportList = await reportAPI.GetNetExposureAsync(perDate, IdType.AccountId, aid, true, demo);
            if (ReportList != null)
            {
                try
                {
                    amountExposures = symbolViewModel.LoadReport(ReportList.ToList());

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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

            if (selectedReport != null)
            {
                var res = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("Details"), ChangeCulture.Lookup("Amend"),
                    ChangeCulture.Lookup("cancelKey"));
                if (res == ChangeCulture.Lookup("FVTrade"))
                {
                    if (amountExposures.Count == 0)
                    {
                        await Navigation.PushAsync(new WatchlistPage());
                    }
                    else
                    {
                        var SymbolId = MarketInfo.getSymbolId(selectedReport.Currency);
                        var orderTicketPage = new OrderTicketPage(selectedReport.Currency);
                        NavigationExtensions.Navigate(typeof(OrderTicketPage), SymbolId);
                        await Navigation.PushModalAsync(orderTicketPage);
                    }
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert(ChangeCulture.Lookup("SelectRowMessage"));
            }
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            selectedReport = (ViewModels.AmountExposure)e.SelectedItem;
        }

        private async void BtnAid_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PositionPage());
        }

        private void BtnSearch_Clicked(object sender, EventArgs e)
        {
            LoadData();
        }

    }
}