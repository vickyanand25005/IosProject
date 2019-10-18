using Straticator.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace StraticatorFroms_iOS.Model
{
    public class ChartModel : INotifyPropertyChanged
    {
        public double AskPrices;
        public double BidPrices;
        public int askCount;
        public int bidCount;

        public ChartModel()
        {

        }

        public ChartModel(double x, int y)
        {
            AskPrices = x;
            askCount = y;
        }

        public ChartModel(int y,float x)
        {
            BidPrices = x;
            bidCount = y;
        }

        public string Month { get; set; }

        public double Target { get; set; }

        public ChartModel(string xValue, double yValue)
        {
            Month = xValue;
            Target = yValue;
        }


        private string year;

        public string Year
        {
            get { return year; }
            set { year = value; NotifyPropertyChanged(); }
        }


        private double value1;

        public double Value1
        {
            get { return value1; }
            set { value1 = value; NotifyPropertyChanged(); }
        }


        private double value2;

        public double Value2
        {
            get { return value2; }
            set { value2 = value; NotifyPropertyChanged(); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

   
}
