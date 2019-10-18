using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupProgressPage : PopupPage
    {
        public PopupProgressPage(string message)
        {
            InitializeComponent();
            busyReason.Text = message;
        }

        protected override void OnDisappearing()
        {

        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}