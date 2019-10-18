using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using StraticatorFroms_iOS;
using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.iOS.Custom;
using StraticatorFroms_iOS.Views.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(HyperlinkLabel), typeof(HyperlinkLabelRenderer))]

namespace StraticatorFroms_iOS.iOS.Custom
{
    public class HyperlinkLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            var View = (HyperlinkLabel)Element;
            if (View == null) return;

            var Attribute = new NSAttributedStringDocumentAttributes();
            var NsError = new NSError();

            Attribute.DocumentType = NSDocumentType.HTML;
            View.Text = string.IsNullOrEmpty(View.Text) ? string.Empty : View.Text;

            Control.AttributedText = new NSAttributedString(View.Text, Attribute, ref NsError);

        }
    }
}