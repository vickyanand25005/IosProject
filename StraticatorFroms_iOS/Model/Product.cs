using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace StraticatorFroms_iOS.Model
{
    public class Product
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductService
    {
        public ObservableCollection<Product> GetAll { get; private set; }

        public ProductService()
        {
            GetAll = new ObservableCollection<Product> {
                new Product { Name = "HP Stream LapTop", Price = 199.00M },
                new Product { Name = "Western Digital 1 TB", Price = 64.99M},
                new Product { Name = "Casio Calculator", Price = 7.79M },
                new Product { Name = "Microsoft Surface Pro", Price = 854.19M },
                new Product { Name = "Dell 27 LCD Monitor", Price = 168.36M },
                new Product { Name = "HP 27 LED Monitor", Price = 178.50M },
                new Product { Name = "Memorex Lens Cleaner", Price = 9.98M },
            };
        }
    }
}
