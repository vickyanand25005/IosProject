using Straticator;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopMenu : ContentPage
    {
        public PositionPrint position{ get; set; }
        public PopMenu(PositionPrint model)
        {
            InitializeComponent();
            position = model;

            BackgroundColor = Color.FromHex("AB000000");
        }

        private void Ticket_Tapped(object sender, EventArgs e)
        {
            var orderTicketPage = new OrderTicketPage(position.Symbol, (short)position.SymbolId);
            NavigationExtensions.Navigate(typeof(OrderTicketPage), position.SymbolId);
            Navigation.PushModalAsync(orderTicketPage);
        }

        private void Trade_Tapped(object sender, EventArgs e)
        {

        }

        private void Close_Tapped(object sender, EventArgs e)
        {

        }

        private void Watchlist_Tapped(object sender, EventArgs e)
        {

        }

        private void Copy_Tapped(object sender, EventArgs e)
        {

        }
    }
}