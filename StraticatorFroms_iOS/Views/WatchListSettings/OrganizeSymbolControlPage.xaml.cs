using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using Straticator.Common;
using Straticator.LocalizationConverter;
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
    public partial class OrganizeSymbolControlPage : ContentPage
    {
        List<string> forexSymbols;
        List<string> stockSymbols;
        List<string> CFDsAndOtherSymbols;
        Exchange[] exchanges;
        public delegate void OnClose(bool DialogResult, short symbolId);
        public event OnClose Closing;

        public OrganizeSymbolControlPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            BindAllTradingSymbol();
            pkrExchangeType.IsVisible = false;
            btnSave.Text = ChangeCulture.Lookup("Save");
            btnCancel.Text = ChangeCulture.Lookup("Cancel");
        }

        private void BindAllTradingSymbol()
        {
            forexSymbols = GetSymbols(TradingSymbolType.Forex);
            List<string> cfdSymbols = GetSymbols(TradingSymbolType.CFD);
            List<string> indexSymbols = GetSymbols(TradingSymbolType.Index);
            List<string> forexRelatedSymbols = GetSymbols(TradingSymbolType.Forex_Related);
            List<string> futureSymbols = GetSymbols(TradingSymbolType.Futures);
            exchanges = Exchange.listExchanges;
            CFDsAndOtherSymbols = cfdSymbols.Concat(indexSymbols).Concat(forexRelatedSymbols).Concat(futureSymbols).OrderBy(s => s).ToList();
            BindExchanges();
            BindSymbolType();
        }

        private static List<string> GetSymbols(TradingSymbolType symbolType, short ExId = 0)
        {
            List<string> symbols = new List<string>();
            var mInfos = MarketInfo.Values.OrderBy(s => s.SymbolType).OrderBy(s => s.Symbol);
            foreach (MarketInfo m in mInfos)
            {
                if (!m.Active || SessionManager.Instance.Session.PriceList.Where(p => p.Symbol == m.Symbol).Count() > 0)
                    continue;
                if (m.SymbolType == symbolType)
                {
                    if (ExId != 0 && m.ExId != ExId)
                        continue;
                    string s;
                    if (symbolType == TradingSymbolType.Forex || symbolType == TradingSymbolType.StocksCFD || symbolType == TradingSymbolType.StocksReal)
                        s = string.Format("{0} ({1})", m.Symbol, m.Name);
                    else
                        s = string.Format("{0} ({1})", m.Symbol, m.SymbolType);
                    symbols.Add(s);
                }
            }
            return symbols;
        }

        void BindExchanges()
        {
            List<string> forexSymbols;
            List<string> exchangeWithSymbols = new List<string>();
            foreach (Exchange ex in exchanges)
            {
                forexSymbols = MarketInfo.Values.Where(s => (s.SymbolType == TradingSymbolType.StocksCFD|| s.SymbolType == TradingSymbolType.StocksReal) && s.Active && s.ExId == ex.exId).Select(s => s.Symbol).ToList();
                if (forexSymbols.Count > 0)
                    exchangeWithSymbols.Add(ex.name);
            }
            pkrExchangeType.ItemsSource= exchangeWithSymbols;
        }

        private void BindSymbolType()
        {
            List<string> cmbItems = new List<string>();
            if (forexSymbols != null && forexSymbols.Count > 0)
                cmbItems.Add(ChangeCulture.Lookup("ForexKey"));
            if (CFDsAndOtherSymbols != null && CFDsAndOtherSymbols.Count > 0)
                cmbItems.Add(ChangeCulture.Lookup("CFDKey"));
            if (pkrExchangeType.Items.Count > 0)
                cmbItems.Add(ChangeCulture.Lookup("Stocks"));
            if (cmbItems.Count > 0)
            {
                pkrSymbolType.ItemsSource= cmbItems;
                pkrSymbolType.SelectedIndex = 0;
            }
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(pkrSymbol.SelectedItem.ToString()))
                return;
            string symbol = Convert.ToString(pkrSymbol.SelectedItem).Substring(0, pkrSymbol.SelectedItem.ToString().IndexOf(" (")).Trim();
            short symbolId = MarketInfo.getSymbolId(symbol);
            if (SessionManager.Instance.Session.UserSymbols.Contains(symbolId))
            {
                //Dismiss();
                return;
            }
            ErrorCodes errCode = await SessionManager.Instance.Session.AddSymbolToWatchlist(symbolId);
            if (errCode == ErrorCodes.Succes)
            {
                this.Closing?.Invoke(true, symbolId);
                await Navigation.PopModalAsync();
            }
        }

        private void BtnCancel_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void PkrSymbolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = pkrSymbolType.SelectedIndex;
            switch (selectedIndex)
            {
                case 0:
                    pkrExchangeType.IsVisible= false;
                    pkrSymbol.ItemsSource= forexSymbols;
                    break;
                case 1:
                    pkrExchangeType.IsVisible = false;
                    pkrSymbol.ItemsSource = CFDsAndOtherSymbols;
                    break;
                case 2:
                    pkrExchangeType.SelectedIndex = 0;
                    pkrExchangeType.IsVisible=true;
                    PkrExchangeType_SelectedIndexChanged(null, null);
                    break;
            }
        }

        private void PkrSymbol_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PkrExchangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strExname = pkrExchangeType.SelectedItem.ToString();
            Exchange ex = (from exchange in exchanges where exchange.name.Equals(strExname) select exchange).SingleOrDefault();
            stockSymbols = GetSymbols(TradingSymbolType.StocksCFD, ex.exId);
            pkrSymbol.ItemsSource= stockSymbols;
            pkrSymbol.SelectedIndex = 0;
        }
    }
}