using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using CoreGraphics;
using StraticatorFroms_iOS.Controls;
using StraticatorFroms_iOS.Droid.Custom;
using StraticatorFroms_iOS.iOS.Custom;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomViewCell), typeof(CustomViewCellRenderer))]
namespace StraticatorFroms_iOS.iOS.Custom
{
    public class CustomViewCellRenderer : ViewCellRenderer
    {

        private bool _selected;

        NativeiOSCell cell;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var nativeCell = (CustomViewCell)item;

            cell = reusableCell as NativeiOSCell;
            if (cell == null)
                cell = new NativeiOSCell(item.GetType().FullName, nativeCell);
            else
                cell.NativeCell.PropertyChanged -= OnNativeCellPropertyChanged;

            nativeCell.PropertyChanged += OnNativeCellPropertyChanged;
            return cell;
        }

        void OnNativeCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var nativeCell = (CustomViewCell)sender;
            if (e.PropertyName == CustomViewCell.SelectedItemBackgroundColorProperty.PropertyName)
            {
                cell.BackgroundColor= UIColor.FromRGB(224,224,224);
            }
        }


    }

    internal class NativeiOSCell : UITableViewCell, INativeElementView
    {
        public CustomViewCell NativeCell { get; private set; }
        public Element Element => NativeCell;

        public NativeiOSCell(string cellId, CustomViewCell cell) : base(UITableViewCellStyle.Default, cellId)
        {
            NativeCell = cell;

            SelectionStyle = UITableViewCellSelectionStyle.Gray;
            ContentView.BackgroundColor = UIColor.FromRGB(255, 255, 224);
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();
        }
    }

}