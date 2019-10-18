using LiveChartTrader.Common;
using LiveChartTrader.Utility;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.CopyTrade
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddFollowerPage : ContentPage
    {
        CopyTradeAPI copytradeApi;
        public static int SelectedMoneyManagerDetails = 0;

        IList<string> Accounts;
        IList<UserAccount> accountList;
        FollowersCommon follower;
        LiveChartTrader.BaseClass.PortfolioCommon portfolio;
        string seperator = Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyGroupSeparator;
        NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
        public AddFollowerPage(LiveChartTrader.BaseClass.PortfolioCommon selectedportfolio)
        {
            InitializeComponent();
            portfolio = selectedportfolio;

            var cur = Enum.GetName(typeof(Currencies), portfolio.Currency);
            lblAmount.Text = ChangeCulture.Lookup("Amount") + " " + "(Min:" + portfolio.minimumFollow.ToString("N0", nf) + " " + cur + ")";
            accountList = SessionManager.Instance.Session.Accounts;
            follower = new FollowersCommon();
            copytradeApi = new CopyTradeAPI(SessionManager.Instance.Session)
            {
                PortfolioType = typeof(FollowersCommon)
            };
            FillCombobox();
        }

        private void FillCombobox()
        {
            int DefaultAid = 0;
            Accounts = new List<string>();
            foreach (LiveChartTrader.Common.UserAccount ua in accountList)
            {
                Accounts.Add(string.Format("{0} - {1} / {2}", ua.Aid, ua.Account, ua.Currency));
                if (ua.isDefault)
                    DefaultAid = ua.Aid;
            }

            pkrAccount.ItemsSource= Accounts.ToList();

            if (pkrAccount.Items.Count > 0)
            {
                int Aid = SessionManager.Instance.CurrentAccount;
                if (Aid != 0)
                    DefaultAid = Aid;
                else if (DefaultAid != 0)
                    SessionManager.Instance.CurrentAccount = DefaultAid;

                if (DefaultAid != 0)
                {
                    var index = 0;
                    for (int i = 0; i < pkrAccount.Items.Count; i++)
                    {
                        int tempAid = Convert.ToInt32(Accounts[i].Split(' ')[0]);
                        if (tempAid == DefaultAid)
                        {
                            index = i;
                            break;
                        }
                    }
                    pkrAccount.SelectedIndex = index;
                }
                else
                    pkrAccount.SelectedIndex = 0;
            }
        }

        private void BtnSave_Clicked(object sender, EventArgs e)
        {
            follower.pFid = portfolio.pFid;
            follower.aid = SessionManager.Instance.CurrentAccount;
            follower.Active = true;
            if (ValidateAmount())
            {
                if (txtAmount.Text.Contains(","))
                {
                    follower.amount = Convert.ToInt32(txtAmount.Text.Replace(",", "").Replace("-", ""));
                }
                else
                {
                    follower.amount = Convert.ToInt32(txtAmount.Text.Replace(".", "").Replace("-", ""));
                }

                SavePortfolio();
            }
        }

        private async void SavePortfolio()
        {
            var res = await copytradeApi.SavePortfolioFollowers(follower);
            SavePortfolioFollowersCompleted(res);
        }

        private async void SavePortfolioFollowersCompleted(ErrorCodes res)
        {
            ErrorCodes ec = res;
            if (ec != ErrorCodes.Succes)
            {
                var msg = ChangeCulture.Lookup(string.Format("OperationUnsuccessfulMsg")) + " " + ChangeCulture.Lookup(ec.ToString());
                DisplayAlert(ChangeCulture.Lookup("Error"), msg, "OK");
            }
            else
            {
                //Intent i = new Intent(this, typeof(CopyTradePage));
                //NavigationExtensions.StartActivity(this, i);
                //new NavigationPage(new CopyTradePage());
                //Application.Current.MainPage = new NavigationPage(new CopyTradePage());

                 await Navigation.PopModalAsync();
                 await Navigation.PushAsync(new NavigationPage(new CopyTradePage()));

            }
        }

        private bool ValidateAmount()
        {
            bool flag = true;
            string msg = string.Empty;
            string VolText = txtAmount.Text.Replace(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberGroupSeparator, "");
            int.TryParse(VolText, out int amount);

            if (amount == 0)
            {
                msg = ChangeCulture.Lookup(string.Format(ChangeCulture.Lookup("InvalidAmount")));
                flag = false;
            }

            else if (amount < portfolio.minimumFollow)
            {
                msg = string.Format(ChangeCulture.Lookup("InvalidMinimumFollow"), portfolio.MinimumFollow.ToString("N0", nf));
                flag = false;
            }

            if (!flag)
            {
                DisplayAlert(ChangeCulture.Lookup("Alert"), msg, "OK");
            }
            return flag;
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void TxtAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            string prevText = txtAmount.Text;

            string formattedText = txtAmount.Text.Replace(seperator, "").Replace("-", "");
            if (formattedText.Length > 3)
            {
                char[] chars = formattedText.ToCharArray();
                Array.Reverse(chars);

                formattedText = string.Join(seperator, chars.Select((c, i) => new { Char = c, Index = i }).
                    GroupBy(o => o.Index / 3)
                    .Select(g => new string(g.Select(o => o.Char).ToArray()))
                    .ToList());
                chars = formattedText.ToCharArray();
                Array.Reverse(chars);
                formattedText = new string(chars);
            }
            if (prevText.StartsWith("-"))
            {
                formattedText = formattedText.Insert(0, "-");
            }
            if (formattedText != prevText)
            {
                txtAmount.Text = formattedText;
            }

            txtAmount.SelectionLength = txtAmount.Text.Length;
        }
    }
}