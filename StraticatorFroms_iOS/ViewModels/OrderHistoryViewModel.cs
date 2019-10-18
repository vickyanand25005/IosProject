using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.Utility;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        private List<OrderArchive> orderList;

        public List<OrderArchive> OrderList
        {
            get { return orderList; }
            set
            {
                orderList = value;
                OnPropertyChanged("OrderList");
            }
        }

        public OrderHistoryViewModel()
        {
            
        }

        public void LoadOrderHistory(IList<CommonOrderArchive> orderArchieve)
        {
            OrderList = new List<OrderArchive>();
            foreach (var item in orderArchieve)
            {
                OrderArchive orderArchive = new OrderArchive
                {
                    SymbolId = item.symbolId,
                    Symbol = MarketInfo.getSymbol(item.symbolId),
                    Lots = new Lot(item.volume, item.symbolId),
                    Price = new SymbPrice(item.symbolId, item.price).Price,
                    OrderStep = ChangeCulture.lookupEnum("EnumUserOrderArchieveStep", item.UserOrderStep),
                    OrderId = item.oid,
                    CreatedTime = item.createdTime,
                    Volume = item.volume,
                    SL = item.sl,
                    TP = item.tp,
                    Duration = ChangeCulture.lookupEnum("DurationType", item.UserOrderDuration),
                    OrderType = ChangeCulture.lookupEnum("MarketType", item.UserOrderType),
                    OrderOrigin = ChangeCulture.lookupEnum("MarketType", item.UserOrderOrigin),
                    Track = item.track
                };
                orderArchive.Time = item.time.ToString(CommonReport.sysUIFormat);
                orderArchive.OriginOid = item.originOid;
                orderArchive.RelatedOid = item.relatedOid;
                OrderList.Add(orderArchive);
            }
        }
    }


    public class OrderArchive : LiveChartTrader.Common.CommonOrderArchive
    {
        public short SymbolId { get; set; }
        public string Symbol { get; set; }
        public long OrderId { get; set; }
        public string Time { get; set; }
        public float Price { get; set; }
        public int Volume { get; set; }
        public string OrderStep { get; set; }
        public int Aid { get { return aid; } }
        public long ExternalId { get { return externalId; } }
        public DateTime CreatedTime { get; set; }
        public Lot Lots
        {
            get;
            set;
        }
        public byte Flag { get { return flag; } }
        public short SL { get; set; }
        public short TP { get; set; }
        public long? RelatedOid { get; set; }
        public string Duration { get; set; }
        public string OrderType { get; set; }
        public string OrderOrigin { get; set; }
        public int Account { get { return aid; } }
        public short BrokerAct { get { return baId; } }
        public int UserId { get { return Uid; } }
        public IntNoZero DisplaySL { get { return new IntNoZero(sl); } }
        public IntNoZero DisplayTP { get { return new IntNoZero(tp); } }
        public byte Track { get; set; }
        public long? OriginOid
        {
            get
            {
                return originOid;
            }
            set
            {
                originOid = value;
            }
        }
    }
}
