using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Straticator.Common;
using Xamarin.Forms;

namespace Straticator
{

    public static class NavigationExtensions
    {
        public static void Navigate(Type page, object data, bool NotByBack = true)
        {
            StraticatorBasePage.isNavigated = true;
            if (NotByBack)
            {
                NavigatePages.SetNavigatePage(page, data);
            }
            //Intent i = new Intent(context, page);
            //((Activity)context).StartActivity(i);
            //((Activity)context).Finish();
        }
        //public static void Navigate(Context context, Type page, bool NotByBack = true)
        //{
        //    StraticatorBasePage.isNavigated = true;
        //    if (NotByBack)
        //    {
        //        NavigatePages.SetNavigatePage(page);
        //    }
        //    Intent i = new Intent(context, page);
        //    ((Activity)context).StartActivity(i);
        //    ((Activity)context).Finish();
        //}
        //public static void StartActivity(object context, Intent i)
        //{
        //    var sb = context as StraticatorBasePage;
        //    if (sb != null)
        //        StraticatorBasePage.handleBack = false;
        //    StraticatorBasePage.isNavigated = true;
        //    ((Activity)context).StartActivity(i);
        //}
        //public static void GoBack(Context context)
        //{
        //    StraticatorBasePage.isNavigated = true;
        //    NavigationData backlink = NavigatePages.BackPage();
        //    if (backlink == null)
        //        return;
        //    if (backlink.SymbolId == 0)
        //        NavigationExtensions.Navigate(context, backlink.Uri, false);
        //    else
        //        NavigationExtensions.Navigate(backlink.Uri, (backlink.PageContent != null) ? (object)backlink.PageContent : (object)backlink.SymbolId, false);
        //}

    }


    public class PageSavedContent
    {
        public byte Track;
        public short SymbolId;
        public int Volume;
        public byte OrderType;
        public double LimitPrice;
        public short sl, tp;
        public bool TrailingStop;
        public byte Expiry;
    }

    public class NavigationData
    {
        public Type Uri;
        public short SymbolId;
        public PageSavedContent PageContent;
    }

    public class NavigatePages
    {
        static List<NavigationData> _pages;
        static NavigatePages()
        {
            var nv = IsolatedStorage.GetNavigation();
            if (nv != null)
                _pages = nv;
            else
                _pages = new List<NavigationData>();
        }

        public static void SetNavigatePage(Type uri, object data = null)
        {
            NavigationData pg = ResumePage();
            if (pg == null || pg.Uri != uri) // this is a new page
            {
                pg = new NavigationData() { Uri = uri };
                _pages.Add(pg);
            }

            if (data != null)
            {
                if (data is short)
                    pg.SymbolId = (short)data;
                else
                    pg.PageContent = (PageSavedContent)data;
            }
        }

        public static void SetNavigatePageContext(Type uri, PageSavedContent PageContent)
        {
            NavigationData pg = ResumePage();
            if (pg != null && pg.Uri == uri && PageContent != null)
            {
                pg.PageContent = PageContent;
                pg.SymbolId = PageContent.SymbolId;
            }
        }

        public static NavigationData BackPage()
        {
            if (_pages.Count > 1)
            {
                _pages.Remove(_pages.Last());
                return _pages.Last();
            }
            return null;
        }

        public static object PageContext()
        {
            NavigationData pc = ResumePage();
            if (pc == null)
                return null;
            return (pc.PageContent != null) ? (object)pc.PageContent : (object)pc.SymbolId;
        }
        public static NavigationData ResumePage()
        {
            if (_pages.Count > 0)
                return _pages.Last();
            return null;
        }
        public static List<NavigationData> GetNavigation()
        {
            return _pages;
        }
    }
}

