using Xamarin.Forms;
using CustomRenderer;
using CustomRenderer.Android;
using StraticatorFroms_iOS;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(MyEntryRenderer))]
namespace CustomRenderer.Android
{
    class MyEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {

                Control.BackgroundColor = UIColor.White;
                Control.TextColor = UIColor.Black;
                //Control.Background = UIImage.
                //GradientDrawable gd = new GradientDrawable();
                
                //gd.SetStroke(0, global::Android.Graphics.Color.LightGray);
                //Control.SetBackgroundDrawable(gd);
                //Control.SetBackgroundColor(global::Android.Graphics.Color.FloralWhite);
                //Control.SetTextColor(global::Android.Graphics.Color.Black);
            }
        }
    }
}

