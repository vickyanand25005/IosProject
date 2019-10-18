using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Straticator.Common
{
    public class OnlineState
    {
        static bool onlineState;
        static public bool LastState { get { return onlineState; } }

        static public void IsOnline(bool value, Xamarin.Forms.Image imgstatus, ContentPage activity)
        {
            if (onlineState != value)
                SetStatus(value, imgstatus, activity);
        }

        static public void SetInitialOffState(Xamarin.Forms.Image imgstatus, ContentPage activity)
        {
            SetStatus(false, imgstatus, activity);
        }

        static void SetStatus(bool stateOn, Xamarin.Forms.Image imgstatus, ContentPage activity) //pass this for Activity from page
        {
            onlineState = stateOn;
            if (stateOn)
                Device.BeginInvokeOnMainThread(() => imgstatus.Source = "Online.png");
            else
                Device.BeginInvokeOnMainThread(() => imgstatus.Source = "offline.png");
        }

        static public ulong GetTickCount()
        {
            DateTime d = DateTime.UtcNow;
            return (ulong)d.Ticks / 10000;
        }

        static public bool Timeout(ulong StartTime, Xamarin.Forms.Image imgstatus, ContentPage activity) //pass this for Context from page
        {
            ulong curTime = GetTickCount();
            if (curTime > StartTime + 60000) // More than a minutes has passed. We time out
            {
                //if (Login.IsNetworkConnection(activity.ApplicationContext)) // We will only timeout if we have network
                //    return true;
            }
            else if (curTime > StartTime + 4000) // 4 sec has passed. We mark it offline
                IsOnline(false, imgstatus, activity);

            return false;
        }
    }
}