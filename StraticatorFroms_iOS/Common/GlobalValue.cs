using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StraticatorAPI;
using Straticator.Model;
using Xamarin.Forms;

namespace Straticator
{
    public class GlobalValue
    {
        /* Package Name */
        public static string PackageName = "com.Straticator.Straticator";

        #region Variables

        public static List<DisplaySymbol> _dispSymbol1, _dispSymbol2;
        public static ListView icSymbols;

        #endregion 

        #region Activity Name

        public static string MainPageName = "straticator.MainPage";
        public static string NoNetworkPageName = "straticator.NoNetwork";
        public static string LoginPageName = "straticator.Login";
        public static string PositionPageName = "straticator.Position";
		public static string PositionsCurrentSymbolPageName = "straticator.PositionsCurrentSymbol";
        public static string PositionsDetailPageName = "straticator.PositionDetail";

        public static string WatchListPageName = "straticator.WatchlistPage";
        public static string OrderTicketPageName = "straticator.OrderTicketPage";
        public static string ChartPageName = "straticator.ChartPage";
        public static string CandleChartPageName = "straticator.CandleChart";
        public static string SymbolsPageName = "straticator.SymbolsPage";
       
        public static string ReportsPageName = "straticator.ReportsPage";
  
		public static string TradeReportPagename = "straticator.TradeReport";
		public static string TradeReportDetailsPagename = "straticator.TradeReportDetails";
		public static string OrderReportPagename = "straticator.OrderReport";
		public static string OrderReportDetailsPagename = "straticator.OrderReportDetails";
		public static string OrderHistoryReportPageName = "straticator.OrderHistoryReport";
		public static string OrderHistoryDetailsPageName = "straticator.OrderHistoryReportDetails";
        public static string SymbolExposureReportPageName = "straticator.SymbolExposureReport";
        public static string CurrencyExposureReportPageName = "straticator.CurrencyExposureReport";
		public static string CopyTradePageDetails = "straticator.CopyTradePageDetails";

		public static string CopyTradePageEdit = "straticator.CopyTradePageEdit";
        public static string TransactionReportPagename = "straticator.TransactionReport";
		public static string CopyTradePageName = "straticator.CopyTradePage";
        public static string TradingRobotPageName = "straticator.TradingRobot";
        public static string TradingRobotDetailsPageName = "straticator.TradingRobotDetails";
        public static string TradingStatisticsPageName = "straticator.TradeStatisticsReport";
        public static string PLPageName = "straticator.PLReport";
        public static string RegisterPage = "straticator.Register";

        public static string AddMoneyManager = "straticator.AddMoneyManager";


        #endregion

        #region Param Name

        public static string ParamTradeDetails = "TradeRec";
		public static string ParamOrderDetails = "OrderRec";
		public static string ParamOrderHDetails = "OrderHRec";
		public static string ParamTradingRobot = "RobotRec";
		public static string ParamCopyTrade = "CopyTrade";
		public static string ParamCopyTradeDetail = "CopyTradeDetail";
        public static string PortfolioCommonDetail = "PortfolioCommonDetail";
        #endregion

        #region Key



        #endregion

        #region Dialog

        //public static void Showpopup(string title, string msg, Context context)
        //{
        //    Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(context);
        //    builder.SetMessage(msg);
        //    builder.SetTitle(title);
        //    builder.SetPositiveButton("ok", (sender, e) => { builder.Dispose(); });
        //    builder.Show();
        //}

        #endregion
    }
}