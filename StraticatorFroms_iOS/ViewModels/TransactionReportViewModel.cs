using LiveChartTrader.Host.DataModel;
using Straticator.Common;
using Straticator.LocalizationConverter;
using System;
using System.Collections.Generic;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class TransactionReportViewModel : BaseViewModel
    {
        List<TransactionDetail> transactionDetails;
        public TransactionReportViewModel()
        {

        }

        public List<TransactionDetail> TransactionDetails
        {
            get => transactionDetails;
            set
            {
                transactionDetails = value;
                OnPropertyChanged("TransactionDetails");
            }
        }

        public void LoadTransactions(IList<LiveChartTrader.BaseClass.CommonUserAccountTrans> transaction)
        {
            TransactionDetails = new List<TransactionDetail>();
            foreach (var item in transaction)
            {
                TransactionDetail detail = new TransactionDetail();
                detail.SymbolId = item.symbolId;
                detail.Symbol = MarketInfo.getSymbol(item.symbolId);
                detail.TransactionType = ChangeCulture.lookupEnum("EnumTransactionType", item.transactionType);
                detail.Time = item.time.ToString(CommonReport.sysUIFormat); ;
                detail.AmountCurrency = item.amountCur;
                TransactionDetails.Add(detail);
            }
        }
    }


    public class TransactionDetail : LiveChartTrader.BaseClass.CommonUserAccountTrans
    {
        public short SymbolId { get; set; }
        public string Symbol { get; set; }
        public string TransactionType { get; set; }
        public string Time { get; set; }
        public double AmountCurrency { get; set; }
    }

}
