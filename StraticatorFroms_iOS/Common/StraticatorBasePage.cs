using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Globalization;

namespace Straticator.Common
{
    public class StraticatorBasePage //: Activity
    {
        public static bool isNavigated=false;
        public static bool handleBack = true;
        //protected override void OnDestroy()
        //{
        //    if (!isNavigated)
        //    {
        //        IsolatedStorage.SetNavigation();
        //        if (!Common.SessionManager.IsLoggedIn)
        //            return;
        //        IsolatedStorage.SaveSession(Common.SessionManager.Instance);
        //    }
        //        isNavigated = false;
        //    base.OnDestroy();
        //}
        //public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        //{
        //    if (keyCode == Keycode.Back && handleBack)
        //    {
        //        NavigationExtensions.GoBack(this);
        //    }
        //    handleBack = true;
        //    return base.OnKeyDown(keyCode, e);
        //}
    }
    public class StraticatorFragmentBasePage //: FragmentActivity
    {
        public static bool handleBack = true;
        //protected override void OnDestroy()
        //{
        //    if (!StraticatorBasePage.isNavigated)
        //    {
        //        IsolatedStorage.SetNavigation();
        //        if (!Common.SessionManager.IsLoggedIn)
        //            return;
        //        IsolatedStorage.SaveSession(Common.SessionManager.Instance);
        //    }
        //    StraticatorBasePage.isNavigated = false;
        //    base.OnDestroy();
        //}

        //public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
        //{
        //    if (keyCode == Keycode.Back && handleBack)
        //    {
        //        NavigationExtensions.GoBack(this);
        //    }
        //    handleBack = true;
        //    return base.OnKeyDown(keyCode, e);
        //}
    }
}