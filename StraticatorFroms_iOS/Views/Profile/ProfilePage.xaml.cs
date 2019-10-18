using LiveChartTrader.Common;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Views.Password;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Profile
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        UserCommon userCommon;
        public ProfilePage()
        {
            InitializeComponent();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            userCommon = SessionManager.Instance.Session.User;
            lblTitle.Text = ChangeCulture.Lookup("ProfileKey");
            lblSave.Text = ChangeCulture.Lookup("SaveCustomer");
            lblId.Text= ChangeCulture.Lookup("Id");
            entName.Placeholder = ChangeCulture.Lookup("TSName");
            lblEmail.Text = ChangeCulture.Lookup("Email");
            lblPhone.Text = ChangeCulture.Lookup("PhoneNoInfo");
            lblChangePassword.Text = ChangeCulture.Lookup("ChangePasswordkey");
            LoadData();
        }

        private void LoadData()
        {
            lblIdValue.Text = userCommon.Uid.ToString();
            entName.Text = userCommon.name;
            entEmail.Text = userCommon.email;
            entPhone.Text = userCommon.phoneNo.ToString();
        }

        private async void ChangePassword_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ChangePasswordPage());
        }
    }
}