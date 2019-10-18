using LiveChartTrader.Common;
using Rg.Plugins.Popup.Services;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Password
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangePasswordPage : ContentPage
    {
        private int userId;
        UserAPI userAPI;
        PopupProgressPage popupProgressPage;
        public ChangePasswordPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            userId = SessionManager.Instance.Session.Uid;
            base.OnAppearing();
            userAPI = new UserAPI(SessionManager.Instance.Session);
            lbltitle.Text = ChangeCulture.Lookup("ChangePasswordkey");
            lblPassword.Text = ChangeCulture.Lookup("Password");
            btnSave.Text = ChangeCulture.Lookup("Send");
            btnCancel.Text = ChangeCulture.Lookup("cancelKey");
            lblConfirmPassword.Text = ChangeCulture.Lookup("ConfirmPassword");
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            string title = ChangeCulture.Lookup("Message");
            string message = ChangeCulture.Lookup("EmailSentMsg");
            try
            {
                if (Validate())
                {
                    popupProgressPage = new PopupProgressPage(ChangeCulture.Lookup("Loading"));
                    await PopupNavigation.Instance.PushAsync(popupProgressPage);
                    var res = await userAPI.SavePassword(txtPassword.Text, true, userId);
                    SavePasswordCompleted(res);
                    await PopupNavigation.Instance.PopAsync(true);
                }
            }
            catch (Exception)
            {
                await PopupNavigation.Instance.PopAsync(true);
                await DisplayAlert(title, message, ChangeCulture.Lookup("OK"));
                await Navigation.PopModalAsync();
            }
        }

        private async void SavePasswordCompleted(ErrorCodes res)
        {
            if (res == ErrorCodes.Succes)
            {
                string title = ChangeCulture.Lookup("Message");
                string message = ChangeCulture.Lookup("EmailSentMsg");
                await DisplayAlert(title, message, ChangeCulture.Lookup("OK"));
                await Navigation.PopModalAsync();
            }
            else
            {
                string error = ChangeCulture.Lookup("OperationUnsuccessfulMsg") + ' ' + ChangeCulture.Lookup(res.ToString());
                showErrorMessage(error);
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private bool Validate()
        {
            string errorMessage = string.Empty;
            bool validated = true;
            var passRegex = new Regex(@"^.*[^a-zA-Z0-9].*$");

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidatePassword");
                showErrorMessage(errorMessage);
                validated = false;
            }
            else if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidatePassword");
                showErrorMessage(errorMessage);
                validated = false;
            }
            else if (txtPassword.Text.Trim().Length < 7 || txtConfirmPassword.Text.Trim().Length < 7)
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorBadPasswordLength");
                showErrorMessage(errorMessage);
                validated = false;
            }
            else if (!passRegex.IsMatch(txtPassword.Text) || !passRegex.IsMatch(txtConfirmPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorBadPasswordStrength");
                showErrorMessage(errorMessage);
                validated = false;
            }
            else if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorPasswordConfirmationMismatch");
                showErrorMessage(errorMessage);
                validated = false;
            }

            return validated;
        }

        async void showErrorMessage(string message)
        {
            await DisplayAlert(ChangeCulture.Lookup("Error"), message, ChangeCulture.Lookup("OK"));
        }
    }
}