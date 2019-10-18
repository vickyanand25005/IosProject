using LiveChartTrader.Common;
using LiveChartTrader.Utility;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Register
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterUserPage : ContentPage
    {
        static string loginId = string.Empty;
        static string pwd = string.Empty;

        UserCommon guestUser;
        List<string> countries;
        string errorMessage = string.Empty;
        private UserAPI userAPI;

        public RegisterUserPage()
        {
            InitializeComponent();
            userAPI = new UserAPI(SessionManager.Instance.Session);
            Localization();

            BindCountry();
            BindCountryCode();
            guestUser = new UserCommon();
            guestUser.userRole = (int)UserRoles.Guest;
            guestUser.tasks = UserInfo.DefaultTask;
            guestUser.status = (int)(UserStatus.Open);
            guestUser.nationality = -1;
            guestUser.CountryCode = -1;
        }

        private void Localization()
        {
            lblRegister.Text = ChangeCulture.Lookup("Register");
            lblName.Text = ChangeCulture.Lookup("LoginName");
            lblEmail.Text = ChangeCulture.Lookup("LoginEmail");
            lblPhone.Text = ChangeCulture.Lookup("PhoneNo");
            lblNationality.Text = ChangeCulture.Lookup("NationalityKey");
            lblLoginId.Text = ChangeCulture.Lookup("SUserName");
            lblPassword.Text = ChangeCulture.Lookup("TAPassword");
            lblConfirmPassword.Text = ChangeCulture.Lookup("ConfirmPassword");
            btnRegister.Text = ChangeCulture.Lookup("Register");
            btnCancel.Text = ChangeCulture.Lookup("cancelKey");
        }

        private async void BtnRegister_Clicked(object sender, EventArgs e)
        {
            ActivityIndicator activityIndicator = new ActivityIndicator();
            string title = ChangeCulture.Lookup("Message");
            string message = ChangeCulture.Lookup("ConfirmRegistrationUser");
            if (ValidateUser())
            {
                activityIndicator.IsEnabled = true;
                activityIndicator.IsVisible = true;
                activityIndicator.IsRunning = true;
                guestUser.name = txtName.Text;
                guestUser.loginId = txtLogin.Text;
                guestUser.brokerId = 9;
                guestUser.email = txtEmail.Text;
                guestUser.phoneNo = Convert.ToInt64(txtPhone.Text);
                string encryptPasswordInfo = EncryptionHard.Encrptdata(txtPassword.Text);
                ErrorCodes res = await SessionManager.Instance.CreateUser(guestUser, encryptPasswordInfo, false);
                activityIndicator.IsEnabled = false;
                activityIndicator.IsVisible = false;
                activityIndicator.IsRunning = false;
                if (res == ErrorCodes.Succes)
                {
                    await DisplayAlert(title, message, "OK");
                    await Navigation.PopModalAsync();
                }

                else
                    ShowErrorMessage(res.ToString());
            }
        }


        private bool ValidateUser()
        {
            bool validated = true;
            var passRegex = new Regex(@"^.*[^a-zA-Z0-9].*$");

            if (string.IsNullOrEmpty(txtName.Text))
            {
                errorMessage = ChangeCulture.Lookup("ValName");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                errorMessage = ChangeCulture.Lookup("EmaiValidationKey");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (!CommonUtilityFunction.EmailValidation(txtEmail.Text, out errorMessage))
            {
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (guestUser.CountryCode == -1)
            {
                errorMessage = ChangeCulture.Lookup("selectCountryCode");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (guestUser.nationality == -1)
            {
                errorMessage = ChangeCulture.Lookup("CountrySelectKey");
                ShowErrorMessage(errorMessage);
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                errorMessage = ChangeCulture.Lookup("LoginIdRequired");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (CommonUtilityFunction.WhiteSpaceRemoved(txtLogin.Text))
            {
                errorMessage = ChangeCulture.Lookup("LoginIdWhiteSpace");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (txtLogin.Text.Length < 3)
            {
                errorMessage = ChangeCulture.Lookup("LoginIdlength");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (!CommonUtilityFunction.CheckforSpecialCharacters(txtLogin.Text))
            {
                errorMessage = ChangeCulture.Lookup("UserIdSplChrMsg");
                ShowErrorMessage(errorMessage);
                return false;
            }

            if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidatePassword");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidatePassword");
                ShowErrorMessage(errorMessage);
                return false;
            }
            if (txtPassword.Text.Trim().Length < 7 || txtConfirmPassword.Text.Trim().Length < 7)
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorBadPasswordLength");
                ShowErrorMessage(errorMessage);
                return false;
            }
            else if (!passRegex.IsMatch(txtPassword.Text) || !passRegex.IsMatch(txtConfirmPassword.Text.Trim()))
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorBadPasswordStrength");
                ShowErrorMessage(errorMessage);
                return false;
            }
            else if (txtPassword.Text.Trim() != txtConfirmPassword.Text.Trim())
            {
                errorMessage = ChangeCulture.Lookup("ValidationErrorPasswordConfirmationMismatch");
                ShowErrorMessage(errorMessage);
                return false;
            }
            return validated;
        }

        void BindCountry()
        {
            string prefix = ChangeCulture.Lookup("SelectCountryKey");
            countries = new List<string>();
            countries.Add(prefix);
            countries.AddRange(GetEnumItems(typeof(Country)));
            pkrNationality.ItemsSource= countries;
            pkrNationality.SelectedIndex = 0;
        }
        public static string[] GetEnumItems(Type type)
        {
            List<string> enumNames = new List<string>();
            foreach (FieldInfo fi in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                enumNames.Add(fi.Name.Replace("_", ""));
            }
            return enumNames.ToArray();
        }
        void BindCountryCode()
        {
            string prefix = ChangeCulture.Lookup("selectCode");
            List<string> countriesCode = new List<string>();
            countriesCode.Add(prefix);
            countriesCode.AddRange(GetEnumItems(typeof(CountryPhoneCode)));
            pkrCountryCode.ItemsSource = countriesCode;
            pkrCountryCode.SelectedIndex = 0;
        }
        private void PkrCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrCountryCode.SelectedIndex == 0)
                return;
            guestUser.CountryCode = Convert.ToInt16(pkrCountryCode.SelectedIndex.ToString());
        }

        private void Pkrnationality_SelectionChanged(object sender, EventArgs e)
        {
            if (pkrNationality.SelectedIndex== 0)
                return;
            guestUser.nationality = Convert.ToInt16((short)pkrNationality.SelectedIndex-1);
        }


        void ShowErrorMessage(string msg)
        {
            DisplayAlert("", msg, ChangeCulture.Lookup("OK"));
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

       
    }
}