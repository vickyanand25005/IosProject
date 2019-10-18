using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.Views.Custom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(CustomTextCellRendererView), typeof(CustomTextCellRenderer))]
namespace StraticatorFroms_iOS.Droid.Custom
{
    public class CustomTextCellRenderer : ViewCellRenderer
    {
        private Android.Views.View _cell;
        private Drawable _normalBackground; // add this line
        private bool _isSelected;

        public CustomTextCellRenderer()
        {
            
        }

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cell = base.GetCellCore(item, convertView, parent, context);

            _normalBackground = _cell.Background; // add this line
            _isSelected = false;

            return _cell;
        }


        protected override void OnCellPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);

            if (args.PropertyName == "IsSelected")
            {
                _isSelected = !_isSelected;

                if (_isSelected)
                {
                    _cell.SetBackgroundColor(Color.Coral.ToAndroid());
                }
                else
                {
                    _cell.Background = _normalBackground; // change this line
                }
            }
        }

        
    }
}