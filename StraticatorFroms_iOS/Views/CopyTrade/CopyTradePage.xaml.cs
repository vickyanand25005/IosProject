using LiveChartTrader.Common;
using LiveChartTrader.Utility;
using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views.CopyTrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CopyTradePage : ContentPage
    {
        CopyTradeAPI copytradeApi;
        CopyTradeViewModel copyTradeViewModel;
        public CopyTradePage()
        {
            InitializeComponent();
            //copytradeApi = new CopyTradeAPI(SessionManager.Instance.Session);
            //copytradeApi.CopyTraderFollowerType = typeof(FollowersCommon);
            //copyTradeViewModel = new CopyTradeViewModel(copytradeApi);
            //LoadFollowers();
            //this.BindingContext = copyTradeViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            copytradeApi = new CopyTradeAPI(SessionManager.Instance.Session);
            copytradeApi.CopyTraderFollowerType = typeof(FollowersCommon);
            copyTradeViewModel = new CopyTradeViewModel(copytradeApi);
            LoadFollowers();
            this.BindingContext = copyTradeViewModel;
        }
        private void LoadFollowers()
        {
            copyTradeViewModel.LoadFollowers();
        }

        private async void BtnEdit_Clicked(object sender, EventArgs e)
        {
            if (lstView.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new CopyTradeEditPage((LiveChartTrader.BaseClass.CopyTraderFollower)lstView.SelectedItem));
            }
        }

        private void BtnDelete_Clicked(object sender, EventArgs e)
        {
            if (lstView.SelectedItem != null)
            {
                CallDeletePortfolioFollowers((FollowersCommon)lstView.SelectedItem);
            }
        }

        private async void CallDeletePortfolioFollowers(FollowersCommon followers)
        {
            var res = await DisplayAlert(ChangeCulture.Lookup(""), ChangeCulture.Lookup("StopFollowingMessage"), ChangeCulture.Lookup("OK"), ChangeCulture.Lookup("cancelKey"));
            if(res)
            {
                DeleteFollowers(followers);
            }
            //await ShowMessage(ChangeCulture.Lookup("Message"), ChangeCulture.Lookup("StopFollowingMessage"), ChangeCulture.Lookup("OK"), ChangeCulture.Lookup("cancelKey"), async () =>
            // {
            //     if (true)
            //     {
            //         
            //     }
            // });
        }

        private async void DeleteFollowers(FollowersCommon followers)
        {
            var res = await copytradeApi.DeletePortfolioFollowers(followers.pFid, followers.aid);
            DeletePortfolioFollowersCompleted(res);
        }

        private void DeletePortfolioFollowersCompleted(ErrorCodes res)
        {
            ErrorCodes ec = res;
            if (ec != ErrorCodes.Succes)
            {
                //Utilities.Utility.ShowErrorMessage(ec);
            }
            else
            {
                LoadFollowers();
            }
        }

        public async Task ShowMessage(string message,string title,string ok,string cancel,Action afterHideCallback)
        {
            await DisplayAlert(title,message,ok,cancel);
            afterHideCallback?.Invoke();
        }

        private async void BtnFollow_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new NavigationPage(new WhatToFollowPage()));
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }


    public class FollowersCommon : LiveChartTrader.BaseClass.CopyTraderFollower
    {
        public int PFid { get { return pFid; } }
        public string Name { get { return name; } }
        public int Uid { get { return uid; } }
        public int Aid { get { return aid; } }
        public int Amount { get { return amount; } set { amount = value; } }
        public Currencies Currency { get { return (Currencies)currency; } }
        public bool Active { get { return (flag & 0x1) != 0; } set { flag = (byte)((flag & ~0x1) | (value ? 0x1 : 0)); } }
        public bool Approved { get { return (flag & 0x4) != 0; } }
    }
}