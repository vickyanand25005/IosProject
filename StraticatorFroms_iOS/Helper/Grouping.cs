using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace StraticatorFroms_iOS.Helper
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }

        public Grouping()
        {

        }

    }


    public class MenuGroup : ObservableCollection<Menus>, INotifyPropertyChanged
    {

        private bool _expanded;

        public string Title { get; set; }

        public string TitleWithItemCount
        {
            get { return string.Format("{0} ({1})", Title, FoodCount); }
        }

        public string ShortName { get; set; }

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged("Expanded");
                    OnPropertyChanged("StateIcon");
                }
            }
        }

        public string StateIcon
        {
            get { return Expanded ? "uparrow_icon.png" : "down_icon.png"; }
        }

        public int FoodCount { get; set; }

        public MenuGroup(string title, string shortName, bool expanded = true)
        {
            Title = title;
            ShortName = shortName;
            Expanded = expanded;
        }

        public static ObservableCollection<MenuGroup> All { private set; get; }

        static MenuGroup()
        {
            ObservableCollection<MenuGroup> Groups = new ObservableCollection<MenuGroup>
            {
                new MenuGroup(ChangeCulture.Lookup("MPPrice").Replace(":",""),"C")
                {
                     new Menus {Name = ChangeCulture.Lookup("PositionsKey"),Id = MenuItemType.Position,Image = "Positions.png"},
                     new Menus{Name = ChangeCulture.Lookup("Watchlist"),Id = MenuItemType.Watchlist,Image = "Watchlist.png"}
                },
                new MenuGroup(ChangeCulture.Lookup("Copy Trade"),"F")
                {
                     new Menus{Name = "Copy Trade",Id = MenuItemType.CopyTrade,Image = "copy.png"},
                     new Menus{Name = ChangeCulture.Lookup("WhatToFollow"),Id = MenuItemType.WhatToFollow,Image = "Positions.png"}
                },
                new MenuGroup(ChangeCulture.Lookup("Reports"),"V")
                {
                     new Menus{Name = ChangeCulture.Lookup("TradestitleKey"),Id = MenuItemType.Trade,Image = "traders.png"},
                     new Menus{ Name = ChangeCulture.Lookup("OrderstitleKey"),Id = MenuItemType.Orders,Image = "orders.png"},
                     new Menus{Name = ChangeCulture.Lookup("OrderHistoryKey"),Id = MenuItemType.OrderHistory,Image = "orderhistory.png"},
                     new Menus{Name = ChangeCulture.Lookup("Transactions"),Id = MenuItemType.Transaction,Image = "transactions.png"},
                     new Menus{Name = ChangeCulture.Lookup("CurrencyExposureKey"),Id = MenuItemType.Currency,Image = "exposure.png"},
                     new Menus{Name = ChangeCulture.Lookup("SymbolExposureKey"),Id = MenuItemType.Symbol,Image = "exposure.png"},
                     new Menus{Name = ChangeCulture.Lookup("TradeStatistics"),Id = MenuItemType.TradeStatistics,Image = "exposure.png"},
                     new Menus{Name = ChangeCulture.Lookup("ProfitLossReport"),Id = MenuItemType.PLReport,Image = "ProfitLoss.png"}

        },
                new MenuGroup(ChangeCulture.Lookup("User"),"D")
                {
                    new Menus{Name = ChangeCulture.Lookup("LogoutKey"),Id = MenuItemType.Logout,Image = "Logout.png"}
                }
            };
            All = Groups;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
