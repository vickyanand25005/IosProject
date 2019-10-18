using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using StraticatorAPI;
using Straticator.LocalizationConverter;
using Xamarin.Forms;

namespace Straticator.Common
{
    public class ReportBase : StraticatorBasePage
    {
        Button btnAid, btnSubmit;
        protected readonly ReportAPI reportAPI;
        protected DateTime date;

        protected ReportBase()
            : base()
        {
            reportAPI = new ReportAPI(Common.SessionManager.Instance.Session);
        }
        #region Date Picker
        Entry dateSender;
        protected void InitializeAccount()
        {
            //btnAid = FindViewById<Button>(Resource.Id.buttonAccNo);
            //btnAid.Click += btnAid_Onclick;
            //btnAid.Text = Convert.ToString(Common.SessionManager.Instance.CurrentAccount);

            //btnSubmit = FindViewById<Button>(Resource.Id.btnSubmit);
            //btnSubmit.Click += btnSubmit_Click;
            //btnSubmit.Text = ChangeCulture.Lookup("Searchkey");
        }

        void btnAid_Onclick(object sender, EventArgs e)
        {
            //NavigationExtensions.Navigate(this, typeof(Position));
        }

        protected virtual void Load()
        {
        
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Load();
        }
        protected void EtDate_Click(object sender, EventArgs e)
        {
            date = DateTime.Today;
            dateSender = (Entry)sender;
            //ShowDialog(0);
        }

        void OnDateSet(object sender)
        {
            //this.date = e.Date;
            String strdate = this.date.ToString(CommonReport.sysUIFormat);
            dateSender.Text = strdate;
           // btnSubmit.PerformClick();
        }

        //protected override Dialog OnCreateDialog(int id)
        //{
        //    switch (id)
        //    {
        //        case 0:
        //            return new DatePickerDialog(this, OnDateSet, date.Year, date.Month - 1, date.Day);
        //    }
        //    return null;
        //}

        #endregion Date Picker
        
    }
}