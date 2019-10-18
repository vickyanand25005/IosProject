using Straticator.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.ViewModels
{
    public class PositionCurrentSymbolViewModel : BaseViewModel
    {

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

        private List<AccountPositionPrint> _accountPos;

        public PositionCurrentSymbolViewModel()
        {

        }

        public List<AccountPositionPrint> AccountPos
        {
            get => _accountPos;
            set
            {
                _accountPos = value;
                OnPropertyChanged("AccountPos");
            }
        }

        public void LoadPosition(List<AccountPositionPrint> _accountPos)
        {
            Positions = new ObservableCollection<PositionPrint>();
            foreach (var item in _accountPos)
            {
                if (item != null)
                {
                    var accountPositionPrint = new PositionPrint
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
                        OpenTime =  item.openTime,
                        Aid = item.aid,
                        HasCloseLink = item.HasCloseLink
                    };
                    Positions.Add(accountPositionPrint);
                }
            }
        }
    }
}
