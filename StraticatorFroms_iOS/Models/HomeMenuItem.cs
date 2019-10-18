using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace StraticatorFroms_iOS.Models
{

    //public enum MenuItemType
    //{
    //    Browse,
    //    About,
    //    Position,
    //    Product,
    //    Logout
    //}
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }

        public string Name { get; set; }

    }
}
