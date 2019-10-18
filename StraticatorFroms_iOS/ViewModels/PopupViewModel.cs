using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class PopupViewModel
    {
        #region singleton
        public static PopupViewModel Instance => _instance ?? (_instance = new PopupViewModel());
        static PopupViewModel _instance;
        public PopupViewModel()
        {
            ListItems.Add("Item 1");
            ListItems.Add("This is the second item");
            ListItems.Add("3rd Item <3");
        }
        #endregion

        #region fields
        IList<string> _listItems = new List<string>();
        #endregion

        #region properties
        public IList<string> ListItems
        {
            get { return _listItems; }
            set { _listItems = value; }
        }
        #endregion
    }
}
