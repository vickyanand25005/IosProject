using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using StraticatorFroms_iOS;
using StraticatorFroms_iOS.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ObjCRuntime;

[assembly: ExportRenderer(typeof(CustomDatePicker), typeof(CustomDatePickerRenderer))]
namespace StraticatorFroms_iOS.iOS.Custom
{
    public class CustomDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                UIDatePicker dateTimePicker = (UIDatePicker)Control.InputView;

                dateTimePicker.Mode = UIDatePickerMode.DateAndTime;
                dateTimePicker.AddTarget(this, new Selector("DateChanged:"), UIControlEvent.ValueChanged);
                NSDateFormatter dateFormat = new NSDateFormatter();
                dateFormat.DateFormat = "dd/MM/yyyy HH:mm";
                var text = (UITextField)Control;
                text.Text = dateFormat.ToString(dateTimePicker.Date);
            }
        }


        [Export("DateChanged:")]
        public void DateChanged(UIDatePicker picker)
        {
            NSDateFormatter dateFormat = new NSDateFormatter
            {
                DateFormat = "dd/MM/yyyy HH:mm"
            };
            var text = Control;
            text.Text = dateFormat.ToString(picker.Date);
        }

    }
}