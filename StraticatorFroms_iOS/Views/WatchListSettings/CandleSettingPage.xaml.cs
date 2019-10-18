using LiveChartTrader.IndicatorDataModel;
using Straticator.Common;
using Straticator.LocalizationConverter;
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
    public partial class CandleSettingPage : ContentPage
    {
        string data = string.Empty;
        string selectedTimeFrame = string.Empty;
        string selectedPriceType = string.Empty;
        string selectedDateType = string.Empty;
        CandleStickChartPage candleStickChart = null;
        public delegate void ChangeChartSetting();
        public event ChangeChartSetting OnChangeChartSetting;


        public CandleSettingPage(string param, CandleStickChartPage candleStickChartPage)
        {
            InitializeComponent();
            data = param;
            candleStickChart = candleStickChartPage;
        }


        protected override void OnAppearing()
        {
            base.OnAppearing();
            FillComboBox("EnumPriceType");
            pkrDateTime.SelectedIndex = 0;
            pkrTimeFrame.SelectedIndex = 0;
        }

        private void FillComboBox(string key)
        {
            string[] arr = ChangeCulture.LookupEnum(key);
            pkrPriceType.ItemsSource = arr;
            pkrPriceType.SelectedIndex = 0;

        }

        private void PkrTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTimeFrame = lblSelectedTimeFrame.Text = pkrTimeFrame.SelectedItem.ToString();
        }

        TimeFrame timeframe;
        DateTime Date;
        static int pricetype = 2;
        DateTime startDate;
        DateTime endDate;
        int strt_flag;
        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            TimeFrameResolution tfr = SetTfr();
            timeframe = new TimeFrame(tfr, Convert.ToInt32(txtNumber.Text));
            Date = Convert.ToDateTime(dtpDatePicker.Date);
            pricetype = pkrPriceType.SelectedIndex;

            if (selectedDateType == "From Date")
            {
                startDate = Date;
                strt_flag = 0;
            }
            else
            {
                endDate = Date;
                strt_flag = 1;
            }

            IsolatedStorage.SaveCandleChartSetting(int.Parse(txtNumber.Text));
            try
            {
                OnChangeChartSetting?.Invoke();
                await Navigation.PopModalAsync();
                //shinobiChart.RemoveSeries(series);
                //shinobiChart.RemoveXAxis(objDateTime);
                //shinobiChart.RemoveYAxis(objNumbers);
            }
            catch
            { }
            
        }

        private TimeFrameResolution SetTfr()
        {
            if (selectedTimeFrame == "Sec")
            {
                return TimeFrameResolution.Seconds;
            }
            else if (selectedTimeFrame == "Min")
            {
                return TimeFrameResolution.Minutes;
            }
            else if (selectedTimeFrame == "Hrs")
            {
                return TimeFrameResolution.Hours;
            }
            else
            {
                return TimeFrameResolution.Days;
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void PkrPriceType_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedPriceType = pkrPriceType.SelectedItem.ToString();
        }

        private void PkrDateTime_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDateType = pkrDateTime.SelectedItem.ToString();
        }
    }
}