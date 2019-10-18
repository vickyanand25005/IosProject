using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace StraticatorFroms_iOS.Helper
{
    public class MenuHelper
    {

        public static ObservableCollection<Menus> Menus { get; set; }

        public static ObservableCollection<Grouping<string, Menus>> MenusGrouped { get; set; }

        public static ObservableCollection<Menus> PriceMenusList { get; set; }

        public static ObservableCollection<Menus> CopyTradeMenusList { get; set; }

        public static ObservableCollection<Menus> ReportsMenusList { get; set; }

        public static ObservableCollection<Menus> UserMenusList { get; set; }
        static MenuHelper()
        {

            PriceMenusList = new ObservableCollection<Menus>
            {
                new Menus
                {
                    Name = ChangeCulture.Lookup("PositionsKey"),
                    Id = MenuItemType.Position,
                    Image = "Positions.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("Watchlist"),
                    Id = MenuItemType.Watchlist,
                    Image = "Watchlist.png"
                }
            };

            Grouping<string, Menus> priceGroup = new Grouping<string, Menus>(ChangeCulture.Lookup("MPPrice"), PriceMenusList);

            CopyTradeMenusList = new ObservableCollection<Menus>
            {
                 new Menus
                {
                    Name = "Copy Trade",
                    Id = MenuItemType.CopyTrade,
                    Image = "copy.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("WhatToFollow"),
                    Id = MenuItemType.WhatToFollow,
                    Image = "Positions.png"
                }

            };

            Grouping<string, Menus> copyTradeGroup = new Grouping<string, Menus>(ChangeCulture.Lookup("Copy Trade"), CopyTradeMenusList);

            ReportsMenusList = new ObservableCollection<Menus>
            {
                new Menus
                {
                    Name = ChangeCulture.Lookup("TradestitleKey"),
                    Id = MenuItemType.Trade,
                    Image = "traders.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("OrderstitleKey"),
                    Id = MenuItemType.Orders,
                    Image = "orders.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("OrderHistoryKey"),
                    Id = MenuItemType.OrderHistory,
                    Image = "orderhistory.png"
                },


                new Menus
                {
                    Name = ChangeCulture.Lookup("Transactions"),
                    Id = MenuItemType.Transaction,
                    Image = "transactions.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("CurrencyExposureKey"),
                    Id = MenuItemType.Currency,
                    Image = "exposure.png"
                },

                new Menus
                {
                    Name = ChangeCulture.Lookup("SymbolExposureKey"),
                    Id = MenuItemType.Symbol,
                    Image = "exposure.png"
                }
            };

            Grouping<string, Menus> reportsGroup = new Grouping<string, Menus>(ChangeCulture.Lookup("Reports"), ReportsMenusList);

            UserMenusList = new ObservableCollection<Menus>
            {
               new Menus
                {
                    Name = ChangeCulture.Lookup("LogoutKey"),
                    Id = MenuItemType.Logout,
                    Image = "Logout.png"
                }
            };

            Grouping<string, Menus> userGroup = new Grouping<string, Menus>(ChangeCulture.Lookup("User"), UserMenusList);



            MenusGrouped = new ObservableCollection<Grouping<string, Menus>>
            {
                priceGroup,
                copyTradeGroup,
                reportsGroup,
                userGroup
            };

        }
    }
}
