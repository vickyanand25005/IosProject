using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Straticator.Model
{
    public class DisplaySymbol : BaseModel
    {
        private short symbolId;

        public short SymbolId
        {
            get { return symbolId; }
            set
            {
                if (symbolId != value)
                {
                    symbolId = value;
                    OnPropertyChanged("SymbolId");
                }
            }
        }

        private string symbol;

        public string Symbol
        {
            get { return symbol; }
            set
            {
                if (symbol != value)
                {
                    symbol = value;
                    OnPropertyChanged("Symbol");
                }
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {

                if (isSelected != value)
                {
                    isSelected = value;
                    OnPropertyChanged("IsSelected");
                }
            }

        }

    }
}
