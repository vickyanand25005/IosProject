using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StraticatorFroms_iOS.Views.Custom;
using StraticatorFroms_iOS.Droid.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomProgressBar), typeof(CustomProgressBarRenderer))]
namespace StraticatorFroms_iOS.Droid.Custom
{
    public class CustomProgressBarRenderer :  ProgressBarRenderer
    {
        public CustomProgressBarRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ProgressBar> e)
        {
            base.OnElementChanged(e);

            CustomProgressBar p = Element as CustomProgressBar;


            if (Control != null)
            {
                p.FlowDirection = FlowDirection.LeftToRight;
                p.ProgressColor = Color.White;
                p.ProgressTo(0.2, 20, Easing.SpringIn);
            }
        }
    }
}