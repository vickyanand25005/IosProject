using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Password
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPassword : ContentPage
    {
        UserAPI userAPI;
        public ForgotPassword()
        {
            InitializeComponent();
            //userAPI = new UserAPI(SessionManager.Instance.Session);
            ChangeCultures();
        }

        private void ChangeCultures()
        {
            lblForgotPassword.Text = ChangeCulture.Lookup("RecoverPassword");
            lblEmailPassword.Text = ChangeCulture.Lookup("EmailSentMsg");
            btnSave.Text = ChangeCulture.Lookup("Send");
            btnCancel.Text = ChangeCulture.Lookup("cancelKey");
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            string title = ChangeCulture.Lookup("Message");
            string message = ChangeCulture.Lookup("EmailSentMsg");
            ActivityIndicator activityIndicator = new ActivityIndicator();

            try
            {
                if (!string.IsNullOrEmpty(txtUserName.Text))
                {
                    activityIndicator.IsEnabled = true;
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    userAPI.RecoverPasswordAsync(txtUserName.Text);
                    DisplayAlert(title, message, "OK");
                    activityIndicator.IsEnabled = false;
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    Application.Current.MainPage.Navigation.PopAsync();

                }
            }
            catch (Exception)
            {
                DisplayAlert(title, message, "OK");
                activityIndicator.IsEnabled = false;
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                Application.Current.MainPage.Navigation.PopAsync();
            }

        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}