using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.Models
{

    public enum MenuItemType
    {
        Position,
        Watchlist,
        CopyTrade,
        WhatToFollow,
        Trade,
        Orders,
        OrderHistory,
        Transaction,
        Currency,
        Symbol,
        TradeStatistics,
        PLReport,
        Logout
    }
    public class Menus
    {
        public MenuItemType Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}
