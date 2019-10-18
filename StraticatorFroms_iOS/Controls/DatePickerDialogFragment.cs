using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Straticator.Model;

namespace Straticator
{
    [Activity(Label = "DatePickerDialogFragment", ConfigurationChanges = ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    class DatePickerDialogFragment : Android.Support.V4.App.DialogFragment
    {
        private readonly Context _context;
        private  DateTime _date;
        private readonly Android.App.DatePickerDialog.IOnDateSetListener _listener;

        public DatePickerDialogFragment(Context context, DateTime date, Android.App.DatePickerDialog.IOnDateSetListener listener  )
        {
            _context = context;
            _date = date;
            _listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedState)
        {
            var dialog = new Android.App.DatePickerDialog(_context, _listener, _date.Year, _date.Month - 1, _date.Day);
            return dialog;
        }

    }
}