using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Straticator
{
    public class PositionRowItem
    {
        public short symbolId { set; get; }
        public string symbol { set; get; }
        public string volume { set; get; }
        public string lots { set; get; }
        public string stringopenprice { set; get; }
        public string currentprice { set; get; }
        public string pl { set; get; }
        public string track { set; get; }
        public string type { get; set; }
        public bool selected { set; get; }
        public long positionId;
    }
}