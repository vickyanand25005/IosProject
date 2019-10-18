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
    public partial class TickChartSettingPage : ContentPage
    {
        public TickChartSettingPage()
        {
            InitializeComponent();
        }

        private void PkrTimeFrame_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}