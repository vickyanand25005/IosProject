using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Straticator
{
    //[Activity(Label = "Custom MessageBox With Title")]
    public class CustomMessageBoxWithTitle //: Android.Support.V4.App.DialogFragment
    {
        

        #region Constructor

        public CustomMessageBoxWithTitle(string titletext,string bodytext,string buttontext)
        {
            this.titletext = titletext;
            this.bodytext = bodytext;
            this.buttontext = buttontext;
        }

        #endregion

        #region Override Methods

        //public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //    //var view = inflater.Inflate(Resource.Layout.CustomMessageBoxWithTitle, container, true);
        //    //Button_Dismiss = view.FindViewById<Button>(Resource.Id.msgok);

        //    //view.FindViewById<TextView>(Resource.Id.msgTitle).Text = titletext;
        //    //view.FindViewById<TextView>(Resource.Id.msgbody).Text = bodytext;
        //    //Button_Dismiss.Text = buttontext;
        //    //Button_Dismiss.SetBackgroundColor(Color.White);
        //    //Button_Dismiss.Click += Button_Dismiss_Click;
        //    return null;
        //}

        //public override void OnResume()
        //{
        //    Dialog.Window.SetLayout(LinearLayout.LayoutParams.FillParent, LinearLayout.LayoutParams.FillParent);

        //    Dialog.Window.SetBackgroundDrawable(new ColorDrawable(Color.Transparent));
        //    Dialog.SetCancelable(false);
        //    SetStyle(Android.Support.V4.App.DialogFragment.StyleNoFrame, Android.Resource.Style.Theme);
        //    base.OnResume();
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    base.Dispose(disposing);
        //    if (disposing)
        //    {
        //        Button_Dismiss.Click -= Button_Dismiss_Click;
                
        //    }
        //}

        #endregion#region Variables

        Button Button_Dismiss;
        string titletext, bodytext, buttontext;


        #region Event

        private void Button_Dismiss_Click(object sender, EventArgs e)
        {
           // Dismiss();
        }

        #endregion

    }
}