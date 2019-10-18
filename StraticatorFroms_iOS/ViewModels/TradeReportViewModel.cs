using System;
using System.Collections.Generic;
using System.Text;
using LiveChartTrader.BaseClass;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.Utility;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;

namespace StraticatorFroms_iOS.ViewModels
{
    public class TradeReportViewModel : BaseViewModel
    {
        private List<TradeDetail> tradedetails;

        public List<TradeDetail> TradeDetails
        {
            get { return tradedetails; }
            set
            {
                tradedetails = value;
                OnPropertyChanged("TradeDetails");
            }
        }

        internal void LoadTradeReport(IList<UserAccountTrade> tradedetails)
        {
            TradeDetails = new List<TradeDetail>();

            foreach (var item in tradedetails)
            {
                TradeDetail tradeDetail = new TradeDetail();
                tradeDetail.OrderId = item.oid;
                tradeDetail.Time = item.time.ToString(CommonReport.sysUIFormat);
                tradeDetail.Symbol = MarketInfo.getSymbol(item.symbolId);
                tradeDetail.Volume = item.volume;
                tradeDetail.Price = new SymbPrice(item.symbolId, item.price).Price;
                tradeDetail.SymbolType = SetSymbolTypeText(MarketInfo.getMarketInfo(item.symbolId).SymbolType);
                tradeDetail.SymbolId = item.symbolId;
                TradeDetails.Add(tradeDetail);
            }
        }

        private string SetSymbolTypeText(TradingSymbolType info)
        {
            string symbolType = string.Empty;
            switch (info)
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
    }

    

    public class TradeDetail : LiveChartTrader.BaseClass.UserAccountTrade
    {
        public short SymbolId { get { return symbolId; }set { symbolId = value; } }
        public string Symbol { get; set; }
        public long OrderId { get { return oid; } set { oid = value; } }
        public string Time { get; set; }
        public int Volume { get { return volume; } set { volume = value; } }
        public Lot Lots { get { return new Lot(volume, symbolId); } }
        public int Aid { get { return aid; } }
        public float Price { get; set; }
        public byte Track { get { return track; } }
        public short BrokerAct { get { return baId; } }
        public int UserId { get { return Uid; } }
        public string OrderOrigin { get { return ChangeCulture.lookupEnum("EnumUserOrderOrigin", orderOrigin); } }
        public double Amount
        {
            get
            {
                return Math.Round(MarketInfo.getMarketInfo(symbolId).Value(price, volume), 2);
            }
        }
        public double AmountCur
        {
            get
            {
                return Math.Round(ExchangeRate * MarketInfo.getMarketInfo(symbolId).Value(price, volume), 2);
            }
        }
        public Currencies Currency { get { return (Currencies)currencyId; } }
        public float BrokerProfit
        {
            get
            {
                if (MarkupPips != 0)
                    return ExchangeRate * Math.Abs((long)volume * MarkupPips) / (MarketInfo.getMarketInfo(symbolId).PipsMultiplier * 10);
                return 0f;
            }
        }
        public string OrderType { get { return ChangeCulture.lookupEnum("MarketType", orderType); } }

        public string SymbolType
        {
            get;
            set;
        }
    }
}
