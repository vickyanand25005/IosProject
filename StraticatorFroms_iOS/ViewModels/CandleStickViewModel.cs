using LiveChartTrader.Common;
using StraticatorFroms_iOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class CandleStickViewModel : BaseViewModel
    {
        private ObservableCollection<BusinessDataObject> _data;
        public ObservableCollection<BusinessDataObject> Data
        {
            get { return _data; }
            set { _data = value; OnPropertyChanged("Data"); }
        }

        public CandleStickViewModel()
        {
            
        }

        public ObservableCollection<BusinessDataObject> LoadCandleChart(CandleStickDate[] candleSticks)
        {
            Data = new ObservableCollection<BusinessDataObject>();
            for (int i = 0; i < candleSticks.Length; i++)
            {
                var data = new BusinessDataObject(candleSticks[i].CandleTime, candleSticks[i].CandleData.Open, candleSticks[i].CandleData.High, candleSticks[i].CandleData.Low, candleSticks[i].CandleData.Close);
                Data.Add(data);
            }

            return Data;
        }

    }


    public class CandelData
    {
        public CandelData()
        { }
        public float Open { set; get; }
        public float High { set; get; }
        public float Low { set; get; }
        public float Close { set; get; }
        public DateTime CandleTime { set; get; }

    }
}
