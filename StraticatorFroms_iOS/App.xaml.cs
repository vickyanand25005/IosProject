using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using StraticatorFroms_iOS.Views;
using Straticator;
using Straticator.Common;
using StraticatorFroms_iOS.Views.Custom;
using StraticatorFroms_iOS.Network;
using StraticatorFroms_iOS.Views.Login;
using Straticator.LocalizationConverter;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace StraticatorFroms_iOS
{
    public partial class App : Application
    {
        ProgressBar progressBar;
        public App()
        {
            progressBar = new ProgressBar();
            InitializeComponent();
            ChangeCulture.SetLocalizationStrings(System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName);
            OnNavigatedTo();
        }

        private async void OnNavigatedTo()
        {
            //await progressBar.ProgressTo(0.8, 1000, Easing.Linear);
            progressBar.ProgressColor = Color.White;
            NavigationData backlink = NavigatePages.ResumePage();
            if (CheckInternet())
            {
                await SessionManager.Instance.InitAsync();
            }

            MainPage = new CustomNavigationPage(new Login());

        }



        protected override void OnStart()
        {
            // Handle when your app starts
            base.OnStart();
            NavigationData backlink = NavigatePages.ResumePage();
            if (backlink != null)
            {
                if (SessionManager.IsLoggedIn)
                {
                    IsolatedStorage.SetRemovePriceList();
                    if (backlink.SymbolId == 0)
                    {

                        var page = (ContentPage)Activator.CreateInstance(backlink.Uri);
                        App.Current.MainPage = new MainPage();
                        MainPage = new NavigationPage(new Login());
                    }
                    else
                    {
                        //progress.Dismiss();
                        //Intent i = new Intent(this, backlink.Uri);
                        //NavigationExtensions.StartActivity(this, i);
                        //Finish();
                    }
                    return;
                }
            }
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            NavigationData backlink = NavigatePages.ResumePage();
            if (backlink != null)
            {
                if (SessionManager.IsLoggedIn)
                {
                    IsolatedStorage.SetRemovePriceList();
                    if (backlink.SymbolId == 0)
                    {
                        var page = (ContentPage)Activator.CreateInstance(backlink.Uri);

                        MainPage = new NavigationPage(page);
                    }
                    else
                    {
                        //progress.Dismiss();
                        //Intent i = new Intent(this, backlink.Uri);
                        //NavigationExtensions.StartActivity(this, i);
                        //Finish();
                    }
                    return;
                }
            }
        }

        protected override void OnResume()
        {
            NavigationData backlink = NavigatePages.ResumePage();
            if (backlink != null)
            {
                if (SessionManager.IsLoggedIn)
                {
                    IsolatedStorage.SetRemovePriceList();
                    if (backlink.SymbolId == 0)
                    {
                        var page = (ContentPage)Activator.CreateInstance(backlink.Uri);

                        MainPage = new NavigationPage(page);
                    }
                    else
                    {
                        //progress.Dismiss();
                        //Intent i = new Intent(this, backlink.Uri);
                        //NavigationExtensions.StartActivity(this, i);
                        //Finish();
                    }
                    return;
                }
            }
        }

        public static bool CheckInternet()
        {
            try
            {
                var networkConnection = DependencyService.Get<INetworkConnection>();
                networkConnection.CheckNetworkConnection();
                if (!networkConnection.IsConnected)
                {
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                return true;
            }
        }
    }
}
