using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Straticator.Model
{
    public class PriceFormatted : BaseModel
    {
        public PriceFormatted(float price, string p1, string p2, string p3) 
        {
            _price = price;
            _part1 = p1;
            _part2 = p2;
            _part3 = p3;
        }

        internal float _price;

        public float Price
        {
            get { return _price; }
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged("Price");
                }
            }
        }

        private string _part1;

        public string Part1
        {
            get { return _part1; }
            set
            {
                _part1 = value;
                OnPropertyChanged("Part1");
            }
        }

        private string _part2;

        public string Part2
        {
            get { return _part2; }
            set
            {
                _part2 = value;
                OnPropertyChanged("Part2");
            }
        }

        private string _part3;

        public string Part3
        {
            get { return _part3; }
            set
            {
                _part3 = value;
                OnPropertyChanged("Part3");
            }
        }

        private int _colorIndex;

        public int ColorIndex
        {
            get { return _colorIndex; }
            set 
            {
                _colorIndex = value;
                OnPropertyChanged("ColorIndex");
            }
        }

    }
}
