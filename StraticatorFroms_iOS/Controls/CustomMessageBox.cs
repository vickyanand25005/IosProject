using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Straticator
{
   // [Activity(Label = "CustomMessageBox")]
    public class CustomMessageBox //: Android.Support.V4.App.DialogFragment
    {
        #region Variables

        //Button Button_Dismiss;
        //TextView tvLine1, tvLine2, tvLine3;
        string Title1, Title2, Title3, Title4;
        Type Url;
        #endregion

        #region Constructor

        public CustomMessageBox(string setTitle1, string setTitle2, string setTitle3, string setTitle4, Type Url)
        {
            Title1 = setTitle1;
            Title2 = setTitle2;
            Title3 = setTitle3;
            Title4 = setTitle4;
            this.Url = Url;
        }

        #endregion

        #region Override Methods

        //public override Android.Views.View OnCreateView(Android.Views.LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        //{
        //    Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
        //    //var view = inflater.Inflate(Resource.Layout.CustomMessageBox, container, true);
        //    //Button_Dismiss = view.FindViewById<Button>(Resource.Id.btnmsgOK);

        //    //tvLine1 = view.FindViewById<TextView>(Resource.Id.lblline1);
        //    //tvLine2 = view.FindViewById<TextView>(Resource.Id.lblline2);
        //    //tvLine3 = view.FindViewById<TextView>(Resource.Id.lblline3);
        //    tvLine1.Text = Title1;
        //    tvLine2.Text = Title2;
        //    tvLine3.TextFormatted = Html.FromHtml("<U>" + Title3 + "</U>");

        //    Button_Dismiss.Text = Title4;
        //    Button_Dismiss.Click += Button_Dismiss_Click;
        //    tvLine3.Click += GotoLink;

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
        //        tvLine3.Click -= GotoLink;
        //    }
        //}

        #endregion

        #region Event

        private void Button_Dismiss_Click(object sender, EventArgs e)
        {
            //Dismiss();
        }

        private void GotoLink(object sender, EventArgs e)
        {
           // Dismiss();
            //NavigationExtensions.Navigate(Application.Context, Url);
        }

        #endregion
    }
}