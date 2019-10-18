using Straticator;
using Straticator.Common;
using Straticator.Model;
using StraticatorFroms_iOS.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class PositionViewModel : INotifyPropertyChanged
    {
        public IList<AccountPositionPrint> Items { get; private set; }

        ObservableCollection<PositionPrint> _PositionsList;
        public ObservableCollection<PositionPrint> Positions
        {
            get
            {
                return _PositionsList;
            }
            set
            {
                _PositionsList = value;
                OnPropertyChanged("Positions");
            }
        }

        public int ItemsCount { get; private set; }
        public decimal ItemsSummary { get; private set; }

        public PositionViewModel()
        {
            
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

        private Xamarin.Forms.ImageSource imgstatus;

        public Xamarin.Forms.ImageSource ImgStatus
        {
            get { return imgstatus; }
            set { imgstatus = value; }
        }

        public ObservableCollection<PositionPrint> LoadPositions()
        {
            if (SessionManager.Instance.LastPosition != null)
            {
                Items = SessionManager.Instance.LastPosition;
                Positions = new ObservableCollection<PositionPrint>();
                foreach (var item in SessionManager.Instance.LastPosition)
                {
                    if (item != null)
                    {
                        PositionPrint accountPositionPrint = new PositionPrint
                        {
                            Symbol = item.Symbol,
                            Volume = item.Volume,
                            OpenPrice = item.openPrice,
                            CurrentPrice = item.currentPrice,
                            PL = item.UnrealizedPL,
                            SymbolId = item.SymbolId,
                            Track = item.Track,
                            PositionId = item.positionId,
                            track = item.Track,
                            Currency = (int)item.Currency,
                            UnrealizedPL_AC = item.UnrealizedPL_AC,
                            UnrealizedPL = item.UnrealizedPL,
                            Lots = item.Lots,
                            OpenTime = item.openTime,
                            Aid = item.aid,
                            HasCloseLink = item.HasCloseLink
                        };
                        Positions.Add(accountPositionPrint);
                    }
                }
            }
            return Positions;
        }
    }

    public class PositionPrint : AccountPositionPrint
    {
        public int Aid { get; set; }
        public int SymbolId { get; set; }
        public string Symbol { get; set; }
        public long Volume { get; set; }
        public float OpenPrice { get; set; }
        public float CurrentPrice { get; set; }
        public double PL { get; set; }
        public byte Track { get;  set; }
        public long PositionId { get; internal set; }

        public int Currency { get; set; }

        public double UnrealizedPL_AC { get; set; }

        public double UnrealizedPL { get; set; }

        public Lot Lots { get; set; }

        public DateTime OpenTime { get; set; }

        public bool HasCloseLink { get; set; }

        public Xamarin.Forms.Color TextColor { get; set; }

        public Xamarin.Forms.Color TextColor2 { get; set; }
    }
}
