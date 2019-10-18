using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using Straticator.Model;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.WatchListSettings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SymbolsPage : ContentPage
    {
        SymbolPageViewModel symbolPageViewModel;
        public SymbolsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            GlobalValue.icSymbols = lstSymbol;
            OnNavigatedTo();

            btnAll.Text = ChangeCulture.Lookup("AllKey");
            btnNone.Text = ChangeCulture.Lookup("NoneKey");
            lblSave.Text = ChangeCulture.Lookup("savehidekey");
            lblCancel.Text = ChangeCulture.Lookup("cancel");
            this.BindingContext = symbolPageViewModel = new SymbolPageViewModel();
        }

        private async void BtnPlus_Clicked(object sender, EventArgs e)
        {
            var dialog = new OrganizeSymbolControlPage();
            await Navigation.PushModalAsync(dialog);
            dialog.Closing += Dialog_Closing;
        }

        private void Dialog_Closing(bool DialogResult, short symbolId)
        {
            symbolPageViewModel.DialogClosing(DialogResult, symbolId);
            

            //GlobalValue.icSymbols.ItemsSource = lst;
        }

        private void BtnNone_Clicked(object sender, EventArgs e)
        {

            //for (int i = 0; i < GlobalValue._dispSymbol1.Count; i++)
            //{
            //    GlobalValue._dispSymbol1[i].IsSelected = false;
            //    GlobalValue._dispSymbol2[i].IsSelected = false;
            //}
            //var lst = new List<DisplaySymbol>();
            //lst.AddRange(GlobalValue._dispSymbol1);
            //lst.AddRange(GlobalValue._dispSymbol2);

            //GlobalValue.icSymbols.ItemsSource = lst;
            symbolPageViewModel.LoadSymbols(false);
        }

        private void BtnAll_Clicked(object sender, EventArgs e)
        {
            symbolPageViewModel.LoadSymbols(true);
            //GlobalValue.icSymbols.ItemsSource = lst;
        }

        private void ImgOkTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            List<string> symbols = new List<string>();

            for (int i = 0; i < GlobalValue._dispSymbol1.Count; i++)
            {
                if (!GlobalValue._dispSymbol1[i].IsSelected)
                {
                    symbols.Add(GlobalValue._dispSymbol1[i].Symbol);
                }

                if (!GlobalValue._dispSymbol2[i].Symbol.Equals("NULL"))
                {
                    if (!GlobalValue._dispSymbol2[i].IsSelected)
                    {
                        symbols.Add(GlobalValue._dispSymbol2[i].Symbol);
                    }
                }
            }

            if (symbols.Count > 0)
            {
                IsolatedStorage.SaveUnusedSymbols(symbols);
            }
            else
            {
                symbols.Add("NULL");
                IsolatedStorage.SaveUnusedSymbols(symbols);

            }

            Navigation.PopAsync();
        }

        private void ImgCancelTapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        public void OnNavigatedTo()
        {
            var lst = new List<DisplaySymbol>();
            GlobalValue._dispSymbol1 = new List<DisplaySymbol>();
            GlobalValue._dispSymbol2 = new List<DisplaySymbol>();
            bool b1 = true;
            foreach (var symbol in SessionManager.Instance.Session.PriceList)
            {
                if (b1)
                {
                    GlobalValue._dispSymbol1.Add(new DisplaySymbol() { SymbolId = symbol.SymbolId, Symbol = symbol.Symbol, IsSelected = !symbol.RemoveInPriceList });
                    b1 = false;
                }
                else
                {
                    GlobalValue._dispSymbol2.Add(new DisplaySymbol() { SymbolId = symbol.SymbolId, Symbol = symbol.Symbol, IsSelected = !symbol.RemoveInPriceList });
                    b1 = true;
                }
            }
            if (GlobalValue._dispSymbol1.Count > GlobalValue._dispSymbol2.Count)
            {
                GlobalValue._dispSymbol2.Add(new DisplaySymbol() { SymbolId = -1, Symbol = "NULL", IsSelected = false });
            }

            lst.AddRange(GlobalValue._dispSymbol1);
            lst.AddRange(GlobalValue._dispSymbol2);

            GlobalValue.icSymbols.ItemsSource = lst;
        }
    }
}