using LiveChartTrader.BaseClass;
using StraticatorFroms_iOS.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace StraticatorFroms_iOS.ViewModels
{
    public class CopyTradeViewModel : BaseViewModel
    {
        StraticatorAPI.CopyTradeAPI copyTrade;
        public CopyTradeViewModel(StraticatorAPI.CopyTradeAPI copytradeApi)
        {
            copyTrade = copytradeApi;
        }

        IList<CopyTraderFollower> Follower;

        public IList<CopyTraderFollower> Followers
        {
            get => Follower;
            set
            {
                Follower = value;
                OnPropertyChanged("Followers");
            }
        }

       

        public async void LoadFollowers()
        {
            var followers = await copyTrade.GetFollowAsync();

            Followers = followers;
        }
    }

    public class CopyTradeFollowers
    {
        public int Account { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
    }
}
