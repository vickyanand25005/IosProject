using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LiveChartTrader.BaseClass;
using Straticator.LocalizationConverter;

using LiveChartTrader.Host.DataModel;
using Xamarin.Forms;

namespace Straticator.Common
{
    public class PieBase : ReportBase
    {
        protected Entry etDate;
        

        protected IList<Exposure> ReportList;
        protected ListView flxGridReport;

        public int selectedReport;
        protected Type AdapterType;
        protected PieBase()
            : base()
        {
            reportAPI.ExposureType = typeof(AmountExposure);
        }

        //protected override void OnCreate(Bundle bundle)
        //{
        //    base.OnCreate(bundle);
        //    RequestWindowFeature(WindowFeatures.NoTitle);
        //}

        protected void AddEtDate()
        {
            //etDate = FindViewById<EditText>(Resource.Id.etDate);
            //etDate.Text = "";
            //etDate.Click += EtDate_Click;
        }

        protected void InitializeChart()
        {
            InitializeAccount();

            //flxGridReport = FindViewById<ListView>(Resource.Id.List);
            //flxGridReport.ItemClick += List_ItemClick;

            //chartFragment = (ChartFragment)FragmentManager.FindFragmentById(Resource.Id.chart);
            //shinobiChart = chartFragment.ShinobiChart;
            //shinobiChart.SetLicenseKey("wePAj4MrTYgDFrWMjAxNDAzMzBpbmZvQHNoaW5vYmljb250cm9scy5jb20=A8nXNo3Nk6VjGXPhDomNDo2yqAUmXTTbVnAjimOwdZDNFbHjnCryuZSpnnj8Ivijk7pVQ0f2kJ6Th3QdACBbO+jk/uU1sChK786yod3nc0jhBMIh3oonS31MA1MGaUceYL9KVADueGihPwyZYN/Bw4GlE1+c=BQxSUisl3BaWf/7myRmmlIjRnMU2cA7q+/03ZX9wdj30RzapYANf51ee3Pi8m2rVW6aD7t6Hi4Qy5vv9xpaQYXF5T7XzsafhzS3hbBokp36BoJZg8IrceBj742nQajYyV7trx5GIw9jy/V6r0bvctKYwTim7Kzq+YPWGMtqtQoU=PFJTQUtleVZhbHVlPjxNb2R1bHVzPnh6YlRrc2dYWWJvQUh5VGR6dkNzQXUrUVAxQnM5b2VrZUxxZVdacnRFbUx3OHZlWStBK3pteXg4NGpJbFkzT2hGdlNYbHZDSjlKVGZQTTF4S2ZweWZBVXBGeXgxRnVBMThOcDNETUxXR1JJbTJ6WXA3a1YyMEdYZGU3RnJyTHZjdGhIbW1BZ21PTTdwMFBsNWlSKzNVMDg5M1N4b2hCZlJ5RHdEeE9vdDNlMD08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjwvUlNBS2V5VmFsdWU+");
            //shinobiChart.Style.BackgroundColor = Android.Graphics.Color.Transparent;

            Load();
        }

        protected override void Load()
        {
            if (ReportList != null)
            {
                var exposure = (AmountExposure[])ReportList;
                AmountExposure.setPercentage(exposure);
                //flxGridReport.Adapter = (IListAdapter)Activator.CreateInstance(AdapterType,this,exposure);
                //try
                //{
                //    if (shinobiChart.Series.Count > 0)
                //    {
                //        shinobiChart.RemoveSeries(shinobiChart.Series[0]);
                //        chartFragment.SetMenuVisibility(true);
                //    }
                //}
                //catch
                //{
                //}
                LoadChart(exposure);
            }
            else
            {
               // chartFragment.SetMenuVisibility(false);
            }
        }



        //private void List_ItemClick(object sender, AdapterView.ItemClickEventArgs e) E:\App16\App16\App16\App16\bin\Xamarin.Android.Support.v4.dll
        //{
        //    selectedReport = e.Position;
        //    int index = flxGridReport.FirstVisiblePosition;
        //    var exposure = (AmountExposure[])ReportList;
        //    flxGridReport.Adapter = (IListAdapter)Activator.CreateInstance(AdapterType, this, exposure);
        //    flxGridReport.SetSelectionFromTop(index, 0);
        //}

        protected void LoadChart(AmountExposure[] exposure)
        {
           
        }


    }

    public class AmountExposure : LiveChartTrader.BaseClass.Exposure
    {
        public string Currency { get { return name; } }
        public string Name { get { return name; } }
        public double Amount { get { return amount; } }
        public double Amount_USD { get { return amount_USD; } }
        public double Volume { get { return amount; } }
        public Lot Lots { get { return new Lot(amount, MarketInfo.getMarketInfo(name)); } }
        public double PositiveVolume { get { return Plus_amount; } }
        public double NegativeVolume { get { return Minus_amount; } }
        public double pct;
        public double Pct { get { return pct; } }

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
                    Data.pct = (100d * Math.Abs(Data.Amount_USD) / sum);
            NameSort(exposure);
        }
    }
}
