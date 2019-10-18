using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Text;
using Straticator.Common;
using Straticator.Model;

namespace StraticatorFroms_iOS.ViewModels
{
    public class WatchlistViewModel :  INotifyPropertyChanged
    {
        //private List<DisplayPriceText> DisplayPrices;

        ObservableCollection<DisplayPriceText> Prices;
        public ObservableCollection<DisplayPriceText> DisplayPrices
        {
            get
            {
                return Prices;
            }
            set
            {
                Prices = value;
                OnPropertyChanged("DisplayPrices");
            }
        }

        public WatchlistViewModel(List<DisplayPrice> lPrice)
        {
            DisplayPrices = new ObservableCollection<DisplayPriceText>();

            foreach (var item in lPrice)
            {
                DisplayPriceText displayPriceText = new DisplayPriceText();
                displayPriceText.SymbolId = item.SymbolId;
                displayPriceText.Symbol = item.Symbol;
                displayPriceText.Spread = item.Spread;
                displayPriceText.Ask = item.AskFormatted.Part1 + item.AskFormatted.Part2 + item.AskFormatted.Part3;
                displayPriceText.AskColor = SetColor(item.AskFormatted.ColorIndex);
                displayPriceText.Bid = item.BidFormatted.Part1 + item.BidFormatted.Part2 + item.BidFormatted.Part3;
                displayPriceText.BidColor = SetColor(item.BidFormatted.ColorIndex);
                DisplayPrices.Add(displayPriceText);
            }
        }

        private Xamarin.Forms.Color SetColor(int ColorIndex)
        {
            if (ColorIndex == 1)
            {
                return Xamarin.Forms.Color.LawnGreen; //green
            }
            else if (ColorIndex == -1)
            {
                return Xamarin.Forms.Color.Red; //red
            }
            else
            {
                return Xamarin.Forms.Color.Gray; //gray
            }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
           [CallerMemberName]string propertyName = "",
           Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;
            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;
            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}


public class DisplayPriceText
{
    public string Symbol { get; set; }

    public string Bid { get; set; }

    public string Ask { get; set; }

    public double Spread { get; set; }

    public short SymbolId { get; set; }

    public Xamarin.Forms.Color BidColor { get; set; }

    public Xamarin.Forms.Color AskColor { get; set; }
}

