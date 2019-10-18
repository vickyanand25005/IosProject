using LiveChartTrader.BaseClass;
using LiveChartTrader.Host.DataModel;
using Straticator;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class TradeStatisticsReportViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataPoint> DoughnutSeriesData { get; set; }

        public TradeStatisticsReportViewModel()
        {

        }
        private List<AmountExposure> tradeList;

        public List<AmountExposure> TradeList
        {
            get { return tradeList; }
            set
            {
                tradeList = value;
                OnPropertyChanged("TradeList");
            }
        }

        internal List<AmountExposure> LoadReport(List<Exposure> reportList)
        {
            TradeList = new List<AmountExposure>();
            DoughnutSeriesData = new ObservableCollection<ChartDataPoint>();

            foreach (var item in reportList)
            {
                AmountExposure exposure = new AmountExposure();

                exposure.Currency = item.name;
                exposure.Name = item.name;
                exposure.Amount = item.amount;
                exposure.Amount_USD = item.amount_USD;
                exposure.Volume = item.amount;
                exposure.Count = item.count;
                exposure.Lots = new Lot(item.amount, MarketInfo.getMarketInfo(item.name));
                exposure.PositiveVolume = item.Plus_amount;
                exposure.NegativeVolume = item.Minus_amount;

                TradeList.Add(exposure);
            }

            setPercentage(TradeList.ToArray());

            return TradeList;
        }

        static public void setPercentage(AmountExposure[] exposure)
        {
            double sum = 0;
            foreach (var Data in exposure)
                sum += Math.Abs(Data.Amount_USD);
            if (sum != 0)
                foreach (var Data in exposure)
                    Data.pct = Convert.ToDouble(String.Format("{0:0.00}", (100d * Math.Abs(Data.Amount_USD) / sum))); 
            //NameSort(exposure);
        }
    }
}
