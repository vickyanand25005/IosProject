using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.Models
{
    public class BusinessDataObject
    {
        public DateTime Year { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Open { get; set; }

        public double Close { get; set; }

        public BusinessDataObject(DateTime xValue, double open, double high, double low, double close)
        {
            Year = xValue;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
    }
}
