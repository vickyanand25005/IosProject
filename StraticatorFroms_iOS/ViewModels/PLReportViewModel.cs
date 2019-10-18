using LiveChartTrader.BaseClass;
using StraticatorFroms_iOS.Model;
using Syncfusion.SfChart.XForms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class PLReportViewModel : BaseViewModel
    {
        public ObservableCollection<ChartModel> Data { get; set; }

        public ObservableCollection<ChartDataPoint> SeriesData { get; set; }
        public PLReportViewModel(int selectedIndex, IList<LiveChartTrader.BaseClass.AccountPLReport> lstAccountPL)
        {
            Data = new ObservableCollection<ChartModel>();
            SeriesData = new ObservableCollection<ChartDataPoint>();
           

        }
    }
}
