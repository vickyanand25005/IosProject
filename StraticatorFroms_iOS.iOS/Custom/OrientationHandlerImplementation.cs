using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using StraticatorFroms_iOS.Controls;
using UIKit;

namespace App16.iOS.Custom
{
    public class OrientationHandlerImplementation : IOrientationHandler
    {
        public void ForceLandscape()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.LandscapeLeft), new NSString("orientation"));
        }

        public void ForcePortrait()
        {
            UIDevice.CurrentDevice.SetValueForKey(new NSNumber((int)UIInterfaceOrientation.Portrait), new NSString("orientation"));
        }
    }
}