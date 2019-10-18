using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorFroms_iOS.Views.Login;
using Rg.Plugins.Popup.Services;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NoNetworkPage : ContentPage
    {
        PopupProgressPage popupProgressPage;
        System.Timers.Timer timer;
        Type uri;
        string msg = string.Empty;
        public NoNetworkPage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            OnNavigatedTo();
            popupProgressPage = new PopupProgressPage(ChangeCulture.Lookup("Loading..."));
            await PopupNavigation.Instance.PushAsync(popupProgressPage);
            lblMsg.Text = string.Format(ChangeCulture.Lookup("NoConnection"), "\r\n"); 
        }

        async void timer_Tick(object sender, EventArgs e)
        {
            if (App.CheckInternet())
            {
                timer.Stop();
                try
                {
                    bool isLoggedIn = await SessionManager.Instance.Session.TestLoginAsync();
                    if (!isLoggedIn)
                    {
                        App.Current.MainPage = new NavigationPage(new Login.Login());
                        NavigationExtensions.Navigate(typeof(Login.Login), uri, false);
                    }
                    else if (uri != null)
                    {
                        NavigationExtensions.Navigate(typeof(NoNetworkPage), uri, false);
                    }
                    else
                    {
                        await PopupNavigation.Instance.PopAsync(true);
                        var positionPage = new PositionPage();
                        NavigationExtensions.Navigate(typeof(PositionPage), uri);
                        await Navigation.PushAsync(positionPage);
                    }
                }
                catch
                {
                    timer.Start();
                }
            }
        }


        protected void OnNavigatedTo()
        {

            /*if (NavigationContext.QueryString.TryGetValue("msg", out msg))
			{
				if (msg != string.Empty)
					txtmsg.Text = ChangeCulture.lookup("PageLoadMSG");
			}*/
            var lastpage = NavigatePages.BackPage();
            if (lastpage != null)
                uri = lastpage.Uri;
            else
                uri = null;
        }
    }
}