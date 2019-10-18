using Straticator;
using Straticator.LocalizationConverter;
using StraticatorFroms_iOS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace StraticatorFroms_iOS
{
    public class LoginViewModel : BaseViewModel
    {
        private string userId;

        public ICommand loginCommand; 

        public string UserId
        {
            get
            {
                return userId;
            }
            set
            {
                userId = value;
                OnPropertyChanged("UserId");
            }
        }

        private string password;

        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                _isBusy = value;
                OnPropertyChanged("IsBusy");
            }
        }


        public LoginViewModel()
        {
            
        }

        public bool ValidateLogin()
        {
            if (string.IsNullOrEmpty(UserId))
            {
                App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("Message"), ChangeCulture.Lookup("ValidateUserName"), ChangeCulture.Lookup("OK"));
                return false;
            }
            if (string.IsNullOrEmpty(Password))
            {
                App.Current.MainPage.DisplayAlert(ChangeCulture.Lookup("Message"), ChangeCulture.Lookup("ValidatePassword"), ChangeCulture.Lookup("OK"));
                return false;
            }
            return true;
        }
    }


    public class ProductsViewModel
    {
        public IList<Product> Items { get; private set; }
        public int ItemsCount { get; private set; }
        public decimal ItemsSummary { get; private set; }

        public ProductsViewModel()
        {
            var service = new ProductService();
            Items = service.GetAll.OrderBy(c => c.Name).ToList();
            ItemsCount = service.GetAll.Count;
            ItemsSummary = service.GetAll.Sum(p => p.Price);
        }
    }
}
