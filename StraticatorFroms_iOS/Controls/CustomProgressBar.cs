using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Xamarin.Forms;

namespace StraticatorFroms_iOS.Controls
{
    public enum ProgressBarDisplayText
    {
        Percentage,
        CustomText
    }
    public class CustomProgressBar : ProgressBar
    {
        public ProgressBarDisplayText DisplayStyle { get; set; }

        //Property to hold the custom text
        public String CustomText { get; set; }

        public CustomProgressBar()
        {
            // Modify the ControlStyles flags
            //http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
        }


    }
}
