using StraticatorFroms_iOS.Helper;
using StraticatorFroms_iOS.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class MenuViewModel
    {

        public ObservableCollection<Menus> Menus { get; set; }
        public ObservableCollection<Grouping<string, Menus>> MenusGrouped { get; set; }

        public MenuViewModel()
        {
            Menus = MenuHelper.Menus;
            MenusGrouped = MenuHelper.MenusGrouped;
        }

        public int MenuCount => Menus.Count;
    }
}
