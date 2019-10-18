using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.iOS.Custom;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MessageIOS))]

namespace StraticatorFroms_iOS.iOS.Custom
{
    public class MessageIOS : IMessage
    {
        const double LONG_DELAY = 3.5;

        NSTimer alertDelay;
        UIAlertController alert;
        public void LongAlert(string message)
        {
            alertDelay = NSTimer.CreateScheduledTimer(LONG_DELAY, (obj) =>
            {
                dismissMessage();
            });
            alert = UIAlertController.Create(null, message, UIAlertControllerStyle.Alert);
            UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
        }

        public void ShortAlert(string message)
        {
            //Toast.MakeText(Application.Context, message, ToastLength.Short).Show();
        }

        void dismissMessage()
        {
            if (alert != null)
            {
                alert.DismissViewController(true, null);
            }
            if (alertDelay != null)
            {
                alertDelay.Dispose();
            }
        }
    }

    
}