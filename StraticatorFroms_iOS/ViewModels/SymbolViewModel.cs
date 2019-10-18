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
    public class SymbolViewModel : BaseViewModel
    {
        public ObservableCollection<ChartDataPoint> DoughnutSeriesData { get; set; }

        public SymbolViewModel()
        {
            
        }
        private List<AmountExposure> symbolList;

        public List<AmountExposure> SymbolList
        {
            get { return symbolList; }
            set
            {
                symbolList = value;
                OnPropertyChanged("SymbolList");
            }
        }

        internal List<AmountExposure> LoadReport(List<Exposure> reportList)
        {
            SymbolList = new List<AmountExposure>();
            DoughnutSeriesData = new ObservableCollection<ChartDataPoint>();

            foreach (var item in reportList)
            {
                AmountExposure exposure = new AmountExposure();
                
                exposure.Currency = item.name;
                exposure.Name = item.name;
                exposure.Amount = item.amount;
                exposure.Amount_USD = item.amount_USD;
                exposure.Volume = item.amount;
                exposure.Lots = new Lot(item.amount, MarketInfo.getMarketInfo(item.name));
                exposure.PositiveVolume = item.Plus_amount;
                exposure.NegativeVolume= item.Minus_amount;
                
                SymbolList.Add(exposure);
            }

            setPercentage(SymbolList.ToArray());


            return SymbolList;
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



    public class AmountExposure : LiveChartTrader.BaseClass.Exposure
    {
        public string Currency { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Amount_USD { get; set; }
        public double Volume { get; set; }
        public Lot Lots { get; set; }
        public double PositiveVolume { get; set; }
        public double NegativeVolume { get; set; }
        public double pct;
        public double Pct { get { return pct; } }

        public int Count { get; set; }

        public string LegendItem
        {
            get
            {
                if (name == null)
                    return null;
                string currentAmount = Amount.ToString("N0");
                string result = string.Format("{0}({1})", this.Currency, currentAmount);
                return result;
            }
        }

        static public void setPercentage(AmountExposure[] exposure)
        {
            double sum = 0;
            foreach (var Data in exposure)
                sum += Math.Abs(Data.Amount_USD);
            if (sum != 0)
                foreach (var Data in exposure)
                    Data.pct = Convert.ToDouble(String.Format("{0:0.00}", (100d * Math.Abs(Data.Amount_USD) / sum)));
            NameSort(exposure);
        }
    }



}
