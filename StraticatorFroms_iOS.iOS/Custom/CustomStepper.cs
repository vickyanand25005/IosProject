using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using StraticatorFroms_iOS;
using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomStepperRenderer), typeof(CustomStepper))]
namespace StraticatorFroms_iOS.iOS.Custom
{
    public class CustomStepper : StepperRenderer
    {
        //protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
        //{
        //    base.OnElementChanged(e);
        //    CustomStepperRenderer stepper = Element as CustomStepperRenderer;

        //    if (Control != null)
        //    {
        //        var button = Control.GetChildAt(0) as Android.Widget.Button;


        //        button.Clickable = true;
        //        button.Text = String.Empty;
        //        button.SetBackground(ResourcesCompat.GetDrawable(Resources, Resource.Drawable.button_selector, null));
        //        //button.SetBackgroundResource(Resource.Drawable.down);
        //        button.SetWidth(50);
        //        var button2 = Control.GetChildAt(1) as Android.Widget.Button;
        //        button2.Text = String.Empty;
        //        button2.SetBackground(ResourcesCompat.GetDrawable(Resources, Resource.Drawable.button_selector, null));
        //        button2.SetWidth(50);

        //    }
        //}


        protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                Control.TintColor = UIColor.Blue;
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            
        }

       
    }
}