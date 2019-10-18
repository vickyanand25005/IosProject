using System;
using System.Collections.Generic;
using System.Text;
using LiveChartTrader.BaseClass;
using LiveChartTrader.Common;
using LiveChartTrader.Host.DataModel;
using LiveChartTrader.Utility;
using Straticator;
using Straticator.Common;
using Straticator.LocalizationConverter;

namespace StraticatorFroms_iOS.ViewModels
{
    public class OrderReportViewModel : BaseViewModel
    {

        private List<OrderDetail> orderDetails;

        public List<OrderDetail> OrderDetails
        {
            get { return orderDetails; }
            set
            {
                orderDetails = value;
                OnPropertyChanged("OrderDetails");
            }
        }

        public OrderReportViewModel()
        {

        }

        internal void LoadOrders(IList<LiveChartTrader.BaseClass.OrderDetail> orderdetails)
        {
            OrderDetails = new List<OrderDetail>();
            foreach (var item in orderdetails)
            {
                OrderDetails.Add((OrderDetail)item);
            }
        }
    }

    public class OrderDetail : LiveChartTrader.BaseClass.OrderDetail
    {
        public string Symbol { get { return MarketInfo.getSymbol(symbolId); } }
        public long OrderId { get { return oid; } }
        public string Time { get { return time.ToString(CommonReport.sysUIFormat); ; } }
        public SymbPrice Price { get { return new SymbPrice(symbolId, price); } }
        public int Volume { get { return volume; } }
        public UserOrderType Type { get { return UserOrderType; } }
        public int Aid { get { return aid; } }
        public short SymbolId { get { return symbolId; } }
        public long ExternalId { get { return externalId; } }
        public Lot Lots
        {
            get { return new Lot(volume, symbolId); }
        }
        public byte Track { get { return track; } }
        public byte Flag { get { return flag; } set { flag = value; } }
        public short SL { get { return sl; } set { sl = value; } }
        public short TP { get { return tp; } set { tp = value; } }
        public Nullable<long> RelatedOid { get { return relatedOid; } set { relatedOid = value; } }
        public string Duration { get { return ChangeCulture.lookupEnum("DurationType", UserOrderDuration); } }
        public string OrderType { get { return ChangeCulture.lookupEnum("MarketType", UserOrderType); } }
        public string OrderOrigin { get { return ChangeCulture.lookupEnum("EnumUserOrderOrigin", UserOrderOrigin); } }
        public int Account { get { return Aid; } }
        public short BrokerAct { get { return baId; } }
        public int UserId { get { return Uid; } }
        public IntNoZero DisplaySL { get { return new IntNoZero(sl); } }
        public IntNoZero DisplayTP { get { return new IntNoZero(tp); } }
        public SymbPrice DisplayPrice { get { return new SymbPrice(symbolId, price); } set { price = value.Price; } }
        public Nullable<long> OriginOid
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
        public string SymbolName
        {
            get
            {
                var sname = MarketInfo.getMarketInfo(symbolId);
                return sname == null ? string.Empty : sname.Name;
            }
        }
        public SymbPrice Trailing
        {
            get { return new SymbPrice(symbolId, trailingPrice); }
        }
        public IntNoZero Secs
        {
            get { return new IntNoZero(Seconds); }
        }
        public Nullable<long> OTO
        {
            get { return triggerOid; }
        }
        public string FixOId
        {
            get
            {
                return FIXOrderId;
            }
        }
    }
}
