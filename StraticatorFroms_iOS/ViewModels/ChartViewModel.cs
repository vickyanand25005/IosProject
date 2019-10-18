using StraticatorFroms_iOS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using StraticatorAPI;
using Straticator.Common;
using LiveChartTrader.Host.DataModel;

namespace StraticatorFroms_iOS.ViewModels
{
    public class ChartViewModel : BaseViewModel
    {
        private ObservableCollection<ChartModel> data;
        public ObservableCollection<ChartModel> Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }

        internal void LoadChart1Data(List<double> askValues, List<double> bidValues, short _currSymbolId)
        {
            Data = new ObservableCollection<ChartModel>();
            var valueList = askValues.Zip(bidValues, (n, w) => new { Value1 = n, Value2 = w });

            int i = 1;
            foreach (var item in valueList)
            {
                i++;
                Data.Add(new ChartModel() { Year = i.ToString(), Value1 = item.Value1, Value2 = item.Value2 });
            }

        }
    }
}
