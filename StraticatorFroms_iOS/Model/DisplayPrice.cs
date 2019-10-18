using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Straticator.Model
{
    public class DisplayPrice : BaseModel
    {
        static string Price0 = "0.0";

        //public event EventHandler ShowChartEvent;
        private short symbolId;
        readonly int PointToIntMultiplier;
        readonly string symbol;

        public DisplayPrice(short symbolId)
        {
            this.SymbolId = symbolId;
            var info = LiveChartTrader.Host.DataModel.MarketInfo.getMarketInfo(symbolId);
            symbol = info.Symbol;
            PointToIntMultiplier = info.PipsMultiplier;
            _askFormatted = new PriceFormatted(0f, Price0, string.Empty, string.Empty);
            _bidFormatted = new PriceFormatted(0f, Price0, string.Empty, string.Empty);
        }

        #region Properties

        public string Symbol { get { return symbol; } }

        private PriceFormatted _askFormatted;
        public PriceFormatted AskFormatted
        {
            get { return _askFormatted; }
            set
            {
                if (_askFormatted != value)
                {
                    _askFormatted = value;
                    OnPropertyChanged("AskFormatted");
                    OnPropertyChanged("Spread");
                }
            }
        }

        private PriceFormatted _bidFormatted;
        public PriceFormatted BidFormatted
        {
            get { return _bidFormatted; }
            set
            {
                if (_bidFormatted != value)
                {
                    _bidFormatted = value;
                    OnPropertyChanged("BidFormatted");
                    OnPropertyChanged("Spread");
                }
            }
        }

        public double Spread { get { return Math.Round((_askFormatted._price - _bidFormatted._price) * PointToIntMultiplier, 1); } }


        private bool _IsSelectedRow;
        public bool IsSelectedRow { set { _IsSelectedRow = value; } get { return _IsSelectedRow; } }

        public short SymbolId { get => symbolId; set => symbolId = value; }

        #endregion
    }
}
