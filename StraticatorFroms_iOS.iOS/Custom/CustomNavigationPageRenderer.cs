using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using StraticatorFroms_iOS.Views.Custom;
using Xamarin.Forms;

using Xamarin.Forms.Platform.iOS;
using System.Threading.Tasks;
using UIKit;
using StraticatorFroms_iOS.iOS.Custom;
using StraticatorFroms_iOS.Views.Custom;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]

namespace StraticatorFroms_iOS.iOS.Custom
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {

        IPageController PageController => Element as IPageController;
        CustomNavigationPage CustomNavigationPage => Element as CustomNavigationPage;



        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }
        
    }
}