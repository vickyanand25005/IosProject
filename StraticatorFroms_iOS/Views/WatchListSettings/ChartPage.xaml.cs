using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views.Custom;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.WatchListSettings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChartPage : ContentPage
    {
        Session session;
        System.Timers.Timer _dTimer;
        private short _currSymbolId;
        private int _NumberOfPrices;
        bool LeavingPage;

        List<double> askValues;
        List<double> bidValues;
        int Decimals;

        int selectedTick = 0; 

        //ChartFragment chartFragment;
        //IShinobiChart shinobiChart;
        NumericalAxis xaxis, yaxis;
        LineSeries askSeries, bidSeries;
        static ulong callServerStart;

        public ChartViewModel chartViewModel;

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var actionSheet = await DisplayActionSheet("", ChangeCulture.Lookup("Cancel"), " ", ChangeCulture.Lookup("Watchlist"), ChangeCulture.Lookup("SettingsKey"), ChangeCulture.Lookup("CandleStick"));
            int action = 0;
            if (actionSheet == ChangeCulture.Lookup("Watchlist")) action = 1;
            else if (actionSheet == ChangeCulture.Lookup("SettingsKey")) action = 2;
            else if (actionSheet == ChangeCulture.Lookup("CandleStick")) action = 3;

            switch (action)
            {
                case 1:
                    await Navigation.PushModalAsync(new WatchlistPage());
                    break;
                case 2:
                    overlay.IsVisible = true;
                    break;
                case 3:
                    var info = MarketInfo.getMarketInfo(_currSymbolId);
                    await Navigation.PushModalAsync(new CandleStickChartPage(info.Symbol));
                    break;
            }
        }

        public ChartPage(short symbol)
        {
            InitializeComponent();
            _currSymbolId = symbol;
            
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                this.BindingContext = chartViewModel = new ChartViewModel();


                DependencyService.Get<IOrientationHandler>().ForceLandscape();

                MessagingCenter.Send(this, "AllowLandscape");

                session = SessionManager.Instance.Session;
                askValues = new List<double>();
                bidValues = new List<double>();

                var cs = IsolatedStorage.GetChartSetting();
                if (cs != null)
                    _NumberOfPrices = int.Parse(cs) * 60;
                else
                    _NumberOfPrices = 3 * 60;


                Device.StartTimer(new TimeSpan(0, 0, 0, 0, 1000), () =>
                {
                    IncrementChart();
                    return true;
                });

                pkrTicks.SelectedIndex = 0;

                OnNavigatedTo();
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send(this, "PreventLandscape");
        }

        private void OnNavigatedTo()
        {
            imgStatus.Source = "offline.png";
            //var parameter = NavigatePages.PageContext();
            //if (parameter != null)
                SessionManager.Instance.CurrentChartSymbol = _currSymbolId ;
            //else if (SessionManager.Instance.CurrentChartSymbol != 0)
            //    _currSymbolId = SessionManager.Instance.CurrentChartSymbol;
            var info = MarketInfo.getMarketInfo(_currSymbolId);
            this.Decimals = info.PriceDecimal;
            Device.BeginInvokeOnMainThread(() => txtSymbol.Text = info.Symbol);
            LoadChart();
        }

        static void AddNumbers(List<double> addto, float[] getFrom, int Decimals)
        {
            // we insert in reverse order.
            for (int i = getFrom.Length; (--i >= 0);)
                addto.Add(Math.Round(getFrom[i], Decimals));
        }

        private async void LoadChart()
        {
            var tradingApi = new TradingAPI(session);
            var symbolPrices = await tradingApi.getPricesCountAsync(_currSymbolId, _NumberOfPrices);
            if (symbolPrices != null)
            {
                AddNumbers(askValues, symbolPrices.AskPrices, Decimals);
                AddNumbers(bidValues, symbolPrices.BidPrices, Decimals);
            }

            DrawChartValues();

            if (LeavingPage)
            {
                LeavingPage = false;
                callServerStart = 0;
            }
           // _dTimer.Start();
        }

        private void DrawChartValues()
        {
            chartViewModel.LoadChart1Data(askValues, bidValues, _currSymbolId);
            //this.BindingContext = chartViewModel = new ChartViewModel(askValues,bidValues,_currSymbolId);

            imgStatus.Source = "online.png";
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = false;
            _NumberOfPrices = selectedTick * 60;
            IsolatedStorage.SaveChartSetting(Convert.ToString(selectedTick));

            askValues.Clear();
            bidValues.Clear();
            LoadChart();
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            overlay.IsVisible = false;
        }

        private void PkrTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PkrTicks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTicks.SelectedIndex == 0)
            {
                selectedTick = 3;
            }
            else if (pkrTicks.SelectedIndex == 1)
            {
                selectedTick = 5;
            }
            else if (pkrTicks.SelectedIndex == 2)
            {
                selectedTick = 10;
            }
            else if (pkrTicks.SelectedIndex == 3)
            {
                selectedTick = 20;
            }
        }

        private void CandleWatchlistChart_SelectionChanged(object sender, ChartSelectionEventArgs e)
        {

        }

        private async void IncrementChart()
        {

            try
            {
                // we mark that we are inside this function, and record the time
                //callServerStart = OnlineState.GetTickCount();

                if (!App.CheckInternet())
                {
                    imgStatus.Source = "offline.png";
                    return;
                }

                var price = await session.LoadNewPriceSymbolAsync(_currSymbolId);
               
                if (price != null)
                {
                    askValues.Add(Math.Round(price.AskPrice, this.Decimals));
                    bidValues.Add(Math.Round(price.BidPrice, this.Decimals));
                    askValues.RemoveAt(0);
                    bidValues.RemoveAt(0);

                    DrawChartValues();
                }
                else
                {
                    imgStatus.Source = "offline.png";
                }
            }
            catch
            {
                imgStatus.Source = "offline.png";
            }
            finally
            {
                // we clear that we leave Lthis function
              callServerStart = 0;
            }
        }

        
    }
}