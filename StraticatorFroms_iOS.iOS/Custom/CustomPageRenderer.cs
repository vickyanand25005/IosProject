using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.Views.Custom;
using Xamarin.Forms.Platform.iOS;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomContentPage), typeof(CustomContentPageRenderer))]

namespace StraticatorFroms_iOS.Droid.Custom
{
    public class CustomContentPageRenderer : PageRenderer
    {

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);
        }

        
        //public CustomContentPageRenderer(Context context) : base(context)
        //{

        //}
        //private ScreenOrientation _previousOrientation = ScreenOrientation.Unspecified;

        //protected override void OnWindowVisibilityChanged(ViewStates visibility)
        //{
        //    base.OnWindowVisibilityChanged(visibility);

        //    var activity = (Activity)Context;

        //    if (visibility == ViewStates.Gone)
        //    {
        //        // Revert to previous orientation
        //        activity.RequestedOrientation = _previousOrientation == ScreenOrientation.Unspecified ? ScreenOrientation.Portrait : _previousOrientation;
        //    }
        //    else if (visibility == ViewStates.Visible)
        //    {
        //        if (_previousOrientation == ScreenOrientation.Unspecified)
        //        {
        //            _previousOrientation = activity.RequestedOrientation;
        //        }

        //        activity.RequestedOrientation = ScreenOrientation.Landscape;
        //    }
        //}
    }
}