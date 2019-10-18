using Straticator.Common;
using StraticatorAPI;
using StraticatorFroms_iOS.ViewModels;
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
    public partial class WhatToFollowPage : ContentPage
    {
        CopyTradeAPI copytradeApi;
        WhatToFollowViewModel whatToFollowViewModel;
        public WhatToFollowPage()
        {
            InitializeComponent();
            copytradeApi = new CopyTradeAPI(SessionManager.Instance.Session);
            copytradeApi.PortfolioType = typeof(PortfolioCommon);
            whatToFollowViewModel = new WhatToFollowViewModel(copytradeApi);
            LoadCopyTradeMasters();
            this.BindingContext = whatToFollowViewModel;
        }

        private async void BtnFollow_Clicked(object sender, EventArgs e)
        {
            if (lstView.SelectedItem != null)
            {
                await Navigation.PushModalAsync(new AddFollowerPage((LiveChartTrader.BaseClass.PortfolioCommon)lstView.SelectedItem));
            }
        }

        private void LoadCopyTradeMasters()
        {
            whatToFollowViewModel.LoadCopyTradeMasters();
        }

        private void LstView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void LstView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }

    public class PortfolioCommon : LiveChartTrader.BaseClass.PortfolioCommon
    {
        public int PFid { get { return pFid; } set { pFid = value; } }

        public int Aid { get { return aid; } set { aid = value; } }

        public int Amount { get { return amount; } set { amount = value; } }

        public bool Active { get { return active; } }

        public byte currency { get { return Currency; } }

    }
}