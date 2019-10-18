using LiveChartTrader.BaseClass;
using Straticator.Common;
using Straticator.LocalizationConverter;
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
    public partial class OrderDetailPage : ContentPage
    {
        public static StraticatorFroms_iOS.ViewModels.OrderDetail currentOrder;
        public OrderDetailPage()
        {
            InitializeComponent();
            LoadPage();
        }

        private void LoadPage()
        {
            NumberFormatInfo nf = Thread.CurrentThread.CurrentCulture.NumberFormat;
            var objOrderDetail = currentOrder;
            lblOrderId.Text = ChangeCulture.Lookup("OdrId");
            lblOrderIdValue.Text = objOrderDetail.OrderId.ToString();

            lblCreatedTime.Text = ChangeCulture.Lookup("CreatedTimekey");
            lblCreatedTimeValue.Text = objOrderDetail.Time;

            lblSymbol.Text = ChangeCulture.Lookup("TSSymbols");
            lblSymbolValue.Text = objOrderDetail.SymbolName;

            lblPrice.Text = ChangeCulture.Lookup("Price");
            lblPriceValue.Text = objOrderDetail.Price.ToString();

            lblVolume.Text = ChangeCulture.Lookup("Volume");
            lblVolumeValue.Text = objOrderDetail.Volume.ToString("N0", nf);

            //FindViewById<TextView>(Resource.Id.ORtvLots).Text = ChangeCulture.Lookup("Lots").Replace(":", "");
            //FindViewById<TextView>(Resource.Id.ORetLots).Text = objOrderDetail.Lots.ToString("N2", nf);

            lblSL.Text = ChangeCulture.Lookup("SLKey");
            lblSLValue.Text = objOrderDetail.SL == 0 ? "" : objOrderDetail.SL.ToString();

            lblTP.Text = ChangeCulture.Lookup("TPKey");
            lblTPValue.Text = objOrderDetail.TP == 0 ? "" : objOrderDetail.TP.ToString();

            lblExpiry.Text = ChangeCulture.Lookup("ExpiryKey");
            lblExpiryValue.Text = objOrderDetail.Duration;

            lblSecond.Text = ChangeCulture.Lookup("SecondKey");
            lblSecondValue.Text = objOrderDetail.Seconds == 0 ? "" : objOrderDetail.Seconds.ToString();

            //lblM.Text = ChangeCulture.Lookup("MPType");
            //FindViewById<TextView>(Resource.Id.ORetMPType).Text = objOrderDetail.OrderType;

            lblOrderOrigin.Text = ChangeCulture.Lookup("OrderOriginKey");
            lblOrderOriginValue.Text = objOrderDetail.OrderOrigin;

            lblTrailing.Text = ChangeCulture.Lookup("TrailingKey");
            lblTrailingValue.Text = objOrderDetail.Trailing.ToString();

            lblOTO.Text = ChangeCulture.Lookup("OTOKey");
            lblOTOValue.Text = objOrderDetail.OTO == null ? "" : objOrderDetail.OTO.ToString();

            lblOrderOriginId.Text = ChangeCulture.Lookup("OriginOrder");
            lblOrderOriginIdValue.Text = objOrderDetail.OriginOid == null ? "" : objOrderDetail.OriginOid.ToString();

            lblRelatedOrderId.Text = ChangeCulture.Lookup("RelatedOrderId");
            lblRelatedOrderIdValue.Text = objOrderDetail.RelatedOid == null ? "" : objOrderDetail.RelatedOid.ToString();

            lblTrack.Text = ChangeCulture.Lookup("MPTrack");
            lblTrackValue.Text = objOrderDetail.Track.ToString();
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