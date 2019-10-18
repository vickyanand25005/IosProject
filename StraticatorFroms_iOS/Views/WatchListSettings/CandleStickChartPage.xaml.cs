using LiveChartTrader.Common;
using LiveChartTrader.IndicatorDataModel;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.Models;
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

namespace StraticatorFroms_iOS.Views.WatchListSettings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CandleStickChartPage : CustomContentPage
    {
        Session session;
        GraphAPI graphApi;
        DateTime startDate;
        DateTime endDate;

        ObservableCollection<BusinessDataObject> CandleList;
        TimeFrame timeframe;

        

        string selectedsymbol;
        static int pricetype = 2;
        int strt_flag;

        string param;

        public CandleStickViewModel candleStickViewModel;
        public CandleStickChartPage(string symbol)
        {
            session = SessionManager.Instance.Session;
            graphApi = new GraphAPI(session);
            startDate = DateTime.Now.AddDays(-1);

            int interval = IsolatedStorage.GetCandleChartSettings();
            if (interval == 0)
                interval = 15;
            timeframe = new TimeFrame(interval * 60);
            selectedsymbol = symbol;
            InitializeComponent();
            CandleChart_Loaded();
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitializeComponent();
            this.BindingContext = candleStickViewModel = new CandleStickViewModel();

            ChartZoomPanBehavior zoomPanBehavior = new ChartZoomPanBehavior();
            zoomPanBehavior.EnableSelectionZooming = true;
            
            zoomPanBehavior.EnablePanning = true;
            zoomPanBehavior.EnableDirectionalZooming = true;
            zoomPanBehavior.EnableDoubleTap = true;
            zoomPanBehavior.ZoomMode = ZoomMode.XY;

            candleWatchlistChart.ChartBehaviors.Add(zoomPanBehavior);
           

        }

        private void CandleStickChartPage_OnChangeChartSetting()
        {
            
        }

        void CandleChart_Loaded()
        {
            ShowGraph(true);
        }

        private async void ShowGraph(bool showAll)
        {
            try
            {
                var graphInput = SetGraphInputs();
                var gOutput = await graphApi.GetGraphAsync(graphInput);
                LoadChart(gOutput);
            }
            catch (Exception)
            {

            }
        }

        private GraphInput SetGraphInputs()
        {
            GraphInput gi = new GraphInput();
            if (startDate != DateTime.MinValue)
                gi.from = startDate;
            if (endDate != DateTime.MinValue)
                gi.to = endDate;
            if (selectedsymbol == null)
                return null;
            GraphSymbolInput[] ListSymbols = new GraphSymbolInput[1];
            gi.graphSymbol = ListSymbols;
            GraphSymbolInput gsi = new GraphSymbolInput();
            gsi.graphIndicator = new List<GraphIndicatorInput>();
            gsi.symbol = selectedsymbol;
            gsi.CandleStickInterval = timeframe;
            gsi.priceType = pricetype;
            gsi.UseIncompleteCandlestick = true;
            gsi.IncompleteCandlestickTimeFrame = 0; // ticks
            ListSymbols[0] = gsi;
            return gi;
        }

        private void LoadChart(GraphOutput resGraph)
        {
            LoadStockPriceData(resGraph);
        }

        private void LoadStockPriceData(GraphOutput resGraph)
        {
            if (resGraph.graphSymbol[0].CandleSticks != null)
            {
                double max = resGraph.graphSymbol[0].CandleSticks[0].CandleData.High;
                double min = resGraph.graphSymbol[0].CandleSticks[0].CandleData.Low;
                for (int i = 0; i < resGraph.graphSymbol[0].CandleSticks.Length; i++)
                {
                    if (max < resGraph.graphSymbol[0].CandleSticks[i].CandleData.High)
                        max = resGraph.graphSymbol[0].CandleSticks[i].CandleData.High;
                    if (min > resGraph.graphSymbol[0].CandleSticks[i].CandleData.Low)
                        min = resGraph.graphSymbol[0].CandleSticks[i].CandleData.Low;
                }

                //lblCandleInfo.Text = 
                //candleWatchlistChart.PrimaryAxis = new DateTimeAxis() { ZoomFactor = 0.5, AutoScrollingDeltaType = DateTimeDeltaType.Hours };

                //candleWatchlistChart.SecondaryAxis = new NumericalAxis() { Minimum = min, Maximum = max, ZoomFactor = 0.5 };
                CandleList = candleStickViewModel.LoadCandleChart(resGraph.graphSymbol[0].CandleSticks);
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var res = await DisplayActionSheet("", ChangeCulture.Lookup("cancelKey"), "", ChangeCulture.Lookup("Settings"));
            if (res == ChangeCulture.Lookup("Settings"))
            {
                if (strt_flag == 0)
                    param = string.Format("{0};{1};{2}", timeframe.NumberOfSeconds, timeframe.TimeFrameType, startDate);
                else
                    param = string.Format("{0};{1};{2}", timeframe.NumberOfSeconds, timeframe.TimeFrameType, endDate);
                var page = new CandleSettingPage(param, this);
                await Navigation.PushModalAsync(page);
                page.OnChangeChartSetting += Page_OnChangeChartSetting;  
            }
        }

        private void Page_OnChangeChartSetting()
        {
            ShowGraph(true);
        }

        private void CandleWatchlistChart_SelectionChanged(object sender, ChartSelectionEventArgs e)
        {
            var series = e.SelectedSeries;
            var p1= e.SelectedDataPointIndex;
            if (p1 > 0 && p1 < CandleList.Count)
            {
                lblCandleInfo.Text = "O:" + CandleList[p1].Open + " H:" + CandleList[p1].High + " L:" + CandleList[p1].Low + " C:" + CandleList[p1].Close + " Time:" + CandleList[p1].Year.ToString();
            }
        }

        private void CandleWatchlistChart_SelectionChanging(object sender, ChartSelectionChangingEventArgs e)
        {
            
        }
    }
}