using Straticator.Common;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StraticatorFroms_iOS.Views.Reports.Order
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderHistoryDetailPage : ContentPage
    {
        public static OrderArchive currentOrderHistory;
        public OrderHistoryDetailPage(OrderArchive selecteditem)
        {
            InitializeComponent();
            currentOrderHistory = selecteditem;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadPage();
        }

        private void LoadPage()
        {
            NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
            SessionManager instance = SessionManager.Instance;
            var account = instance.Session.Accounts.Where(a => a.Aid == instance.CurrentAccount).SingleOrDefault();
            var objOrderArchive = currentOrderHistory;

            lblOrderId.Text = ChangeCulture.Lookup("OdrId");
            lblOrderIdValue.Text = currentOrderHistory.OrderId.ToString();

            lblCreatedTime.Text = ChangeCulture.Lookup("CreatedTimekey");
            lblCreatedTimeValue.Text = currentOrderHistory.CreatedTime.ToString(CommonReport.sysUIFormat + " " + CommonReport.sysUIFormatTime);

            lblSymbol.Text = ChangeCulture.Lookup("TSSymbols");
            lblSymbolValue.Text = currentOrderHistory.Symbol;

            lblPrice.Text = ChangeCulture.Lookup("Price");
            lblPriceValue.Text = currentOrderHistory.Price.ToString();

            lblVolume.Text = ChangeCulture.Lookup("Volume");
            lblVolumeValue.Text = currentOrderHistory.Volume.ToString("N0", nf);

            //FindViewById<TextView>(Resource.Id.ORtvLots).Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            //FindViewById<TextView>(Resource.Id.ORetLots).Text = currentOrderHistory.Lots.ToString("N2", nf);

            lblSL.Text = ChangeCulture.Lookup("SLKey");
            lblSLValue.Text =currentOrderHistory.SL.ToString();

            lblTP.Text = ChangeCulture.Lookup("TPKey");
            lblTPValue.Text = currentOrderHistory.TP.ToString();

            lblExpiry.Text = ChangeCulture.Lookup("ExpiryKey");
            lblExpiryValue.Text = currentOrderHistory.Duration;

            lblAction.Text = ChangeCulture.Lookup("ActionKey");
            lblAction.Text = currentOrderHistory.Seconds == 0 ? "" : currentOrderHistory.OrderStep.ToString();

            lblOrderType.Text = ChangeCulture.Lookup("MPType");
            lblOrderTypeValue.Text = currentOrderHistory.OrderType;

            lblOrderOrigin.Text = ChangeCulture.Lookup("OrderOriginKey");
            lblOrderOriginValue.Text = currentOrderHistory.OrderOrigin;

            lblTime.Text = ChangeCulture.Lookup("Tmkey");
            lblTimeValue.Text = currentOrderHistory.Time;

            lblOrderOriginId.Text = ChangeCulture.Lookup("OriginOrder");
            lblOrderOriginIdValue.Text = currentOrderHistory.OriginOid == null ? "" : currentOrderHistory.OriginOid.ToString();

            lblRelatedOrderId.Text = ChangeCulture.Lookup("RelatedOrderId");
            lblRelatedOrderIdValue.Text = currentOrderHistory.RelatedOid == null ? "" : currentOrderHistory.RelatedOid.ToString();

            lblTrack.Text = ChangeCulture.Lookup("MPTrack");
            lblTrackValue.Text = currentOrderHistory.Track.ToString();
            //if (SessionManager.Instance.Session.Settings.UserLots)
            //{
            //    var layoutVolume = FindViewById<LinearLayout>(Resource.Id.ORlayoutVolume);
            //    layoutVolume.Visibility = ViewStates.Gone;
            //}
            //else
            //{
            //    var layoutLots = FindViewById<LinearLayout>(Resource.Id.ORlayoutLots);
            //    layoutLots.Visibility = ViewStates.Gone;
            //}
        }

        private void BtnClose_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}