using LiveChartTrader.Host.DataModel;
using Straticator;
using Straticator.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class SymbolPageViewModel : BaseViewModel
    {
        private List<DisplaySymbol> displaySymbolList1;

        public List<DisplaySymbol> DisplaySymbolList
        {
            get { return displaySymbolList1; }
            set
            {
                displaySymbolList1 = value;
                OnPropertyChanged("DisplaySymbolList1");
            }
        }


        public SymbolPageViewModel()
        {
            var lst = new List<DisplaySymbol>();
            lst.AddRange(GlobalValue._dispSymbol1);
            lst.AddRange(GlobalValue._dispSymbol2);

            DisplaySymbolList = lst;
        }

        internal void LoadSymbols(bool flag)
        {
            for (int i = 0; i < GlobalValue._dispSymbol1.Count; i++)
            {
                GlobalValue._dispSymbol1[i].IsSelected = flag;
                GlobalValue._dispSymbol2[i].IsSelected = flag;
            }

            var lst = new List<DisplaySymbol>();
            lst.AddRange(GlobalValue._dispSymbol1);
            lst.AddRange(GlobalValue._dispSymbol2);

            DisplaySymbolList = lst;
        }

        internal void DialogClosing(bool DialogResult, short symbolId)
        {
            if (!DialogResult)
                return;
            int disp2Count = GlobalValue._dispSymbol2.Count;
            if (GlobalValue._dispSymbol2[disp2Count - 1].SymbolId == -1)
            {
                GlobalValue._dispSymbol2.Remove(GlobalValue._dispSymbol2.Last());
                GlobalValue._dispSymbol2.Add(new DisplaySymbol() { SymbolId = symbolId, Symbol = MarketInfo.getSymbol(symbolId), IsSelected = true });
            }
            else
            {
                GlobalValue._dispSymbol1.Add(new DisplaySymbol() { SymbolId = symbolId, Symbol = MarketInfo.getSymbol(symbolId), IsSelected = true });
                GlobalValue._dispSymbol2.Add(new DisplaySymbol() { SymbolId = -1, Symbol = "NULL", IsSelected = false });
            }
            var lst = new List<DisplaySymbol>();
            lst.AddRange(GlobalValue._dispSymbol1);
            lst.AddRange(GlobalValue._dispSymbol2);

            DisplaySymbolList = lst;
        }
    }
}
