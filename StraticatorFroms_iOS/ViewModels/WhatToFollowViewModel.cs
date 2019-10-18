using LiveChartTrader.BaseClass;
using StraticatorAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class WhatToFollowViewModel : BaseViewModel
    {
        CopyTradeAPI copytrade;
        private IList<PortfolioCommon> moneyManager;

        public WhatToFollowViewModel(CopyTradeAPI copytradeApi)
        {
            copytrade = copytradeApi;
        }

        public IList<PortfolioCommon> MoneyManager
        {
            get => moneyManager;
            set
            {
                moneyManager = value;
                OnPropertyChanged("MoneyManager");
            }
        }

        public async void LoadCopyTradeMasters()
        {
            MoneyManager = await copytrade.getUserMoneyManager();
            if(MoneyManager!= null)
            {

            }
        }
    }
}
