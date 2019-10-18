using Straticator.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StraticatorAPI;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Threading;
using Straticator.Common;
using Straticator.LocalizationConverter;
using LiveChartTrader.Common;

namespace StraticatorFroms_iOS.Views.Position
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PositionDetailPage : ContentPage
    {
        public static AccountPositionPrint currentPosition;
        Session session;
        int minSLTPValue = -5000;
        int maxSLTPValue = 5000;
        public PositionDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LocalizeAndSetControls();
        }

        private void LocalizeAndSetControls()
        {
            NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
            
            var objPosition = currentPosition;
            SessionManager instance = SessionManager.Instance;
            session = instance.Session;
            var account = session.Accounts.Where(a => a.Aid == instance.CurrentAccount).SingleOrDefault();

            lblOpenTime.Text = ChangeCulture.Lookup("OpenTime");
            lblOpenTimeValue.Text = objPosition.openTime.ToString(CommonReport.sysUIFormat + " " + CommonReport.sysUIFormatTime);

            lblAccountNo.Text = ChangeCulture.Lookup("AccountNo");
            lblAccountNumberValue.Text = Convert.ToString(objPosition.aid);

            lblSymbol.Text = ChangeCulture.Lookup("MPSymbol").Replace(":", "");
            lblSymbolValue.Text = objPosition.Symbol;

            lblLots.Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            lblLotsValue.Text = objPosition.Lots.ToString("", nf);


            lblVolume.Text = ChangeCulture.Lookup("Volume").Replace(":", "");
            lblVolumeValue.Text = objPosition.Volume.ToString("N0", nf);

            lblOpenPrice.Text = ChangeCulture.Lookup("HOpenPrice");
            lblOpenPriceValue.Text = objPosition.OpenPrice.ToString();

            lblCurrentPrice.Text = ChangeCulture.Lookup("CurrentPrice");
            lblCurrentPriceValue.Text = objPosition.CurrentPrice.ToString();

            lblUnrealizedPl.Text = string.Format("{0} ({1})", ChangeCulture.Lookup("UnrealizedPL"), objPosition.Currency);
            lblUnrealizedPlValue.Text = objPosition.UnrealizedPL.ToString("N0", nf);

            lblUnrealizedPlAC.Text = string.Format("{0} ({1})", ChangeCulture.Lookup("UnrealizedPL"), account.Currency);
            lblUnrealizedPlACValue.Text = objPosition.UnrealizedPL_AC.ToString("N0", nf);

            lblTrack.Text = ChangeCulture.Lookup("MPTrack").Replace(":", "");
            lblTrackValue.Text = objPosition.track.ToString();

            if (session.User.useTracks)
                stTrack.IsVisible = true;
            else
                stTrack.IsVisible = false;
            lblPositionsId.Text = ChangeCulture.Lookup("PositionId");
            lblPositionsIdValue.Text = objPosition.positionId.ToString();

            lblStopLoss.Text = ChangeCulture.Lookup("HStopLoss");
            entStopLoss.Text = Convert.ToString(objPosition.sl);

            lblTakeProfit.Text = ChangeCulture.Lookup("HTakeProfit");
            entTakeProfit.Text = Convert.ToString(objPosition.tp);

            btnSave.Text = ChangeCulture.Lookup("Save");
            btnCancel.Text = ChangeCulture.Lookup("Cancel");

            if (session.Settings.UserLots)
            {
                stVolume.IsVisible= false;
            }
            else
            {
                stLots.IsVisible= false;
            }
            
            if (account != null && account.UsePositionId)
                stPosition.IsVisible = true;
            else
                stPosition.IsVisible = false;
            if (account != null && account.SLTPonPosition)
                stStopLoss.IsVisible = stTakeProfit.IsVisible = lblTakeProfit.IsVisible = lblStopLoss.IsVisible = stButton.IsVisible = true;

            else
                stStopLoss.IsVisible = stTakeProfit.IsVisible = lblTakeProfit.IsVisible = lblStopLoss.IsVisible = stButton.IsVisible = false;
        }

        private async void BtnSave_Clicked(object sender, EventArgs e)
        {
            short tp = string.IsNullOrWhiteSpace(entTakeProfit.Text) ? (short)0 : Convert.ToInt16(entTakeProfit.Text);
            short sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? (short)0 : Convert.ToInt16(entStopLoss.Text);
            if (currentPosition.sl != sl || currentPosition.tp != tp)
            {
                currentPosition.sl = sl;
                currentPosition.tp = tp;
                AccountAPI api = new AccountAPI(session);
                var errorCode = await api.UpdatePositionSLTP(currentPosition);
                string error = ChangeCulture.Lookup("OperationUnsuccessfulMsg") + ' ' + ChangeCulture.Lookup(errorCode.ToString());
                if (errorCode != ErrorCodes.Succes)
                await DisplayAlert(ChangeCulture.Lookup("Error"), error, ChangeCulture.Lookup("OK"));
            }
            await Navigation.PopModalAsync();
        }

        private async void BtnCancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void TakeProfitUp_Clicked(object sender, EventArgs e)
        {
            int tf = string.IsNullOrWhiteSpace(entTakeProfit.Text) ? 0 : Convert.ToInt32(entTakeProfit.Text);
            if (tf < maxSLTPValue)
                entTakeProfit.Text = Convert.ToString(tf + 5);
        }

        private void TakeProfitDown_Clicked(object sender, EventArgs e)
        {
            int tf = string.IsNullOrWhiteSpace(entTakeProfit.Text) ? 0 : Convert.ToInt32(entTakeProfit.Text);
            if (tf > minSLTPValue)
                tf -= 5;
            entTakeProfit.Text = tf < minSLTPValue ? minSLTPValue.ToString() : Convert.ToString(tf);
        }

        private void StoplossUp_Clicked(object sender, EventArgs e)
        {
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl < maxSLTPValue)
                entStopLoss.Text = Convert.ToString(sl + 5);
        }

        private void StoplossDown_Clicked(object sender, EventArgs e)
        {
            int sl = string.IsNullOrWhiteSpace(entStopLoss.Text) ? 0 : Convert.ToInt32(entStopLoss.Text);
            if (sl > -minSLTPValue)
                sl -= 5;
            entStopLoss.Text = sl < minSLTPValue ? minSLTPValue.ToString() : Convert.ToString(sl);
        }
    }
}