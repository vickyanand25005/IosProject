using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Helper;
using StraticatorFroms_iOS.Models;
using StraticatorFroms_iOS.ViewModels;
using StraticatorFroms_iOS.Views;
using StraticatorFroms_iOS.Views.Profile;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuPage : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        List<HomeMenuItem> menuItems;
        private ObservableCollection<MenuGroup> _allGroups;
        private ObservableCollection<MenuGroup> _expandedGroups;

        //MenuViewModel viewModel = new MenuViewModel();

        public MenuPage()
        {
            InitializeComponent();
            _allGroups = MenuGroup.All;
            UpdateListContent();
            if (SessionManager.Instance.Session.User != null)
            {
                lblUserName.Text =string.Format("{0} {1}",ChangeCulture.Lookup("Welcome"),SessionManager.Instance.Session.User.name);
            }
           // BindingContext = new MenuViewModel();
        }

        private void UpdateListContent()
        {
            _expandedGroups = new ObservableCollection<MenuGroup>();
            foreach (MenuGroup group in _allGroups)
            {
                //Create new FoodGroups so we do not alter original list
                MenuGroup newGroup = new MenuGroup(group.Title, group.ShortName, group.Expanded);
                //Add the count of food items for Lits Header Titles to use
                newGroup.FoodCount = group.Count;
                if (group.Expanded)
                {
                    foreach (Menus menu in group)
                    {
                        newGroup.Add(menu);
                    }
                }
                _expandedGroups.Add(newGroup);
            }
            GroupedView.ItemsSource = _expandedGroups;
        }

        private void HeaderTapped(object sender, EventArgs args)
        {
            int selectedIndex = _expandedGroups.IndexOf(
                ((MenuGroup)((Button)sender).CommandParameter));
            _allGroups[selectedIndex].Expanded = !_allGroups[selectedIndex].Expanded;
            UpdateListContent();
        }
        private void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
       => ((ListView)sender).SelectedItem = null;

        private async void Handle_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var id = (int)((Menus)e.SelectedItem).Id;
            await RootPage.NavigateFromMenu(id);
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new ProfilePage()));
        }
    }
}