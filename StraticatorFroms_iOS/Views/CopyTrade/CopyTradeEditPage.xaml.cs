using LiveChartTrader.BaseClass;
using LiveChartTrader.Common;
using LiveChartTrader.Utility;
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

namespace StraticatorFroms_iOS.Views.CopyTrade
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CopyTradeEditPage : ContentPage
    {
        CopyTradeAPI copytradeApi;
        CopyTraderFollower followeritem;
        public CopyTradeEditPage(CopyTraderFollower followersCommon)
        {
            InitializeComponent();
            copytradeApi = new CopyTradeAPI(SessionManager.Instance.Session);
            copytradeApi.CopyTraderFollowerType = typeof(FollowersCommon);
            followeritem = followersCommon;

            lblPfidValue.Text = followersCommon.pFid.ToString();
            lblNameValue.Text = followeritem.accountName.ToString();
            lblAccountValue.Text = followeritem.aid.ToString();
            txtAmount.Text = followeritem.amount.ToString();
            lblCurrency.Text = Enum.GetName(typeof(Currencies), followeritem.currency);
            chkTrailingStop.IsChecked = followeritem.active;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            int amnt = 0;
            if (txtAmount.Text.ToString().Trim().Length > 0)
            {
                amnt = int.Parse(txtAmount.Text);
                if (amnt == 0)
                {
                    await DisplayAlert("", ChangeCulture.Lookup("InvalidAmount"), ChangeCulture.Lookup("OK"));
                    return;
                }
                else
                {
                    followeritem.active = chkTrailingStop.IsChecked;
                    followeritem.amount = amnt;
                    ErrorCodes msg = await copytradeApi.UpdateFollowAsync(followeritem);
                    if (msg == ErrorCodes.Succes)
                    {
                        //OnBackPressed();
                    }
                    else
                    {
                        await DisplayAlert("", msg.ToString(), ChangeCulture.Lookup("OK").ToLower());
                        return;
                    }
                }
            }
            else
            {
                await DisplayAlert("", ChangeCulture.Lookup("InvalidAmount"), ChangeCulture.Lookup("OK").ToLower());
                return;
            }
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}