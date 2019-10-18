
using LiveChartTrader.IndicatorDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.Utility;

namespace Straticator.Model
{
    public class AccountPositionPrint : LiveChartTrader.BaseClass.AccountPosition, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propName)
        {
            var propChanged = PropertyChanged;
            if (propChanged != null)
                propChanged(this, new PropertyChangedEventArgs(propName));
        }

        public LiveChartTrader.Host.DataModel.MarketInfo info;

        public short SymbolId { get { return symbolId; } }

        public long Volume { get { return volume; } }

        public string Type { get { return SetSymbolTypeText(info); } }

        private string SetSymbolTypeText(MarketInfo info)
        {
            string symbolType = string.Empty;
            switch (info.SymbolType)
            {
                case TradingSymbolType.CFD:
                    symbolType = "C";
                    break;
                case TradingSymbolType.Forex:
                    symbolType = "FX";
                    break;
                case TradingSymbolType.Forex_Related:
                    symbolType = "FR";
                    break;
                case TradingSymbolType.Futures:
                    symbolType = "FU";
                    break;
                case TradingSymbolType.Index:
                    symbolType = "I";
                    break;
                case TradingSymbolType.StocksCFD:
                    symbolType = "SC";
                    break;
            }
            return symbolType;
        }

        public Lot Lots
        {
            get
            {
                double vol = System.Convert.ToDouble(volume);
                MarketInfo mInfo = MarketInfo.getMarketInfo(symbolId);
                return new Lot(vol, mInfo);
            }
        }

        public byte Track { get { return track; } }

        public string Symbol { get { return info.Symbol; } }
        public double UnrealizedPL {
            get
            {
               return info.Value(currentPrice - openPrice, volume);
            }
        }
        public Currencies Currency
        {
            get
            {
                return (Currencies)info.QuotedId;
            }
        }
        public double UnrealizedPL_AC { get { return accountPL; } }

        public SymbPrice OpenPrice { get { return new SymbPrice(symbolId, openPrice); } }

        public SymbPrice CurrentPrice
        {
            get { return new SymbPrice(symbolId, currentPrice); }
            set
            {
                new SymbPrice(symbolId, currentPrice);
                OnPropertyChanged("CurrentPrice");
                OnPropertyChanged("UnrealizedPL");
            }
        }

        int cnt; // we need this for sorting, since in FIFO and LIFO we need to keep the order inside the track.

        class CompareDateDescend : IComparer<AccountPositionPrint>
        {
            public int Compare(AccountPositionPrint x, AccountPositionPrint y)
            {
                int c = x.aid - y.aid;
                if (c != 0)
                    return c;
                c = string.Compare(x.info.Symbol, y.info.Symbol);
                if (c != 0)
                    return c;
                c = x.track - y.track;
                if (c != 0)
                    return c;
                return x.cnt - y.cnt;
            }
        }

        static IComparer<AccountPositionPrint> cmp = new CompareDateDescend();

        static public AccountPositionPrint[] Convert(IList<LiveChartTrader.BaseClass.AccountPosition> lst)
        {
            if (lst == null)
                return null;
            AccountPositionPrint[] arr = (AccountPositionPrint[])lst;
            int len = arr.Length;
            for (int i = 0; (i < len); i++)
            {
                var a = arr[i];
                a.info = LiveChartTrader.Host.DataModel.MarketInfo.getMarketInfo(a.symbolId);
                a.cnt = i;
            }
            Array.Sort(arr, cmp);
            return arr;
        }

    }
}
