using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StraticatorFroms_iOS
{
    public class CustomPickerRenderer : Picker
    {
        public static readonly BindableProperty ImageProperty =
            BindableProperty.Create(nameof(Image), typeof(string), typeof(CustomPickerRenderer), string.Empty);

        public string Image
        {
            get { return (string)GetValue(ImageProperty); }
            set { SetValue(ImageProperty, value); }
        }
    }
}
