using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiveChartTrader.Common;
using Straticator.Common;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorFroms_iOS.Network;
using Straticator.LocalizationConverter;
using System.Windows.Input;
using Straticator;
using StraticatorFroms_iOS.Views.Password;
using System.Globalization;
using Xamarin.Essentials;
using StraticatorFroms_iOS.Views.Register;
using StraticatorFroms_iOS.Controls;
using Rg.Plugins.Popup.Services;
using StraticatorFroms_iOS.Views.Custom;

namespace StraticatorFroms_iOS.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {

        ProgressBar progress;
        static string loginId = string.Empty;
        static string pwd = string.Empty;
        static bool isLiveLogin = true;
        LoginViewModel viewModel;
        INetworkConnection networkConnection;
        bool hasSession = false;
        string lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
        PopupProgressPage popupProgressPage;

        public Login()
        {
            InitializeComponent();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ChangeCulture.SetLocalizationStrings(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            OnNavigatedTo();
            BindingContext = viewModel = new LoginViewModel();
            progress = new ProgressBar();
            var vehicleTypes = new List<string>() { "Live", "Demo" };
            segment.Children = vehicleTypes;
            segment.Margin = new Thickness(100, 0, 100, 0);
            segment.ItemSelected = "Demo";
            segment.MinimumWidthRequest = 500;

            lnkCreateAccount.Margin = new Thickness(-40, 0, 0, -0);

            lnkForgotPassword.Margin = new Thickness(-9, 0, 0, 0);

            ChangeCultures();

        }

        private async void OnNavigatedTo()
        {
            //await progressBar.ProgressTo(0.8, 1000, Easing.Linear);
            // progressBar.ProgressColor = Color.White;
            NavigationData backlink = NavigatePages.ResumePage();
            if (App.CheckInternet())
            {
                await SessionManager.Instance.InitAsync();
            }

            App.Current.MainPage = new CustomNavigationPage(this);

        }


        private void ChangeCultures()
        {
            string[] Header = { "UserNameKey", "PasswordKey", "LIVETRADINGACCOUNT", "ForgotPassword", "LoginKey", "Cancel", "GuestRegistration", "BecomeCustomer" };
            LocalizationHandler.LocalizeGridHeader(ref Header);

            lblUserName.Text = Header[0];
            lblPassword.Text = Header[1];
            btnLogin.Text = Header[4];
            lnkCreateAccount.Text = Header[6];
            lnkForgotPassword.Text = Header[3];
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            txtPassword.IsReadOnly = txtUserName.IsReadOnly = true;
            popupProgressPage = new PopupProgressPage(ChangeCulture.Lookup("Loading"));
            await PopupNavigation.Instance.PushAsync(popupProgressPage);
            viewModel.IsBusy = true; // progressControl.IsVisible = true;
            isLiveLogin = segment.ItemSelected == "Live" ? true : false;
            if (!SessionManager.Instance.IsInitialized)
                hasSession = await SessionManager.Instance.InitAsync();

            if (viewModel.ValidateLogin())
            {
                IsolatedStorage.SaveSession(SessionManager.Instance);
                if (App.CheckInternet())
                {
                    try
                    {
                        var ok = await SessionManager.Instance.Login(txtUserName.Text, txtPassword.Text, isLiveLogin);
                        if (SessionManager.Instance.Session.User != null && SessionManager.Instance.Session.User.forcePasswordChange)
                        {
                            //Intent activity = new Intent(this, typeof(ChangePassword));
                            //NavigationExtensions.StartActivity(this, activity);
                            await Navigation.PushModalAsync(new ChangePasswordPage());

                        }
                        else if (ok)
                        {
                            CommonReport.GetDateFormat();
                            txtPassword.Text = string.Empty;
                            txtUserName.Text = string.Empty;
                            IsolatedStorage.SetRemovePriceList();
                            IsolatedStorage.SaveLoginCredentials(loginId, isLiveLogin);
                            viewModel.IsBusy = false; //progressControl.IsVisible = false;
                            
                            await Navigation.PushAsync(new MainPage());
                            Application.Current.MainPage = new MainPage();
                            await PopupNavigation.Instance.PopAsync(true);
                        }
                        else
                        {
                            btnLogin.IsVisible = true;
                            txtPassword.IsReadOnly = txtUserName.IsReadOnly = false;
                            await DisplayAlert(ChangeCulture.Lookup("Login"), ChangeCulture.Lookup("LoginErrorkey"), ChangeCulture.Lookup("OK"));
                            await PopupNavigation.Instance.PopAsync(true);
                        }
                        txtPassword.Text = string.Empty;
                    }
                    catch (Exception ex)
                    {
                        txtPassword.IsReadOnly = txtUserName.IsReadOnly = false;
                        throw;
                    }
                }
            }
            else
            {
                txtPassword.IsReadOnly = txtUserName.IsReadOnly = false;
                btnLogin.IsVisible = true;
                viewModel.IsBusy = false; //progressControl.IsVisible = false;
            }
        }

        public static bool IsNetworkConnection()
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void SwDemo_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void SwLive_Toggled(object sender, ToggledEventArgs e)
        {

        }

        private void Handle_SelectedItemChanged(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private async void lnkCreateAccount_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new RegisterUserPage());
        }

        private async void lnkBecomeCustomer_Tapped(object sender, EventArgs e)
        {
            var url = "https://npinvestor.com/en/create-customer";
            if (lang == "da")
            {
                url = "https://npinvestor.com/da/create-customer";
            }
            var uri = new Uri(url);
            await Browser.OpenAsync(uri);
        }

        private async void TapGesture_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPassword());
        }


    }
}