using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace Assessment.Models
{
    public class Checkout : INotifyPropertyChanged
    {
        private ObservableCollection<Product> _products;
        private double _total;
        private double _subtotal;
        private double _discount;
        private double _tax;
        private int _lines;
        private int _quantity;
        public string Term { get; set; } = "NET 60";

        public ObservableCollection<Product> Products
        {
            get => _products;
            set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged(nameof(Products));
                    UpdateTotal();
                }
            }
        }

        public double Total
        {
            get => _total;
            private set
            {
                if (_total != value)
                {
                    _total = value;
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        public double Subtotal
        {
            get => _subtotal;
            private set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged(nameof(Subtotal));
                }
            }
        }

        public double Discount
        {
            get => _discount;
            private set
            {
                if (_discount != value)
                {
                    _discount = value;
                    OnPropertyChanged(nameof(Discount));
                }
            }
        }

        public double Tax
        {
            get => _tax;
            private set
            {
                if (_tax != value)
                {
                    _tax = value;
                    OnPropertyChanged(nameof(Tax));
                }
            }
        }
        
        public int Lines
        {
            get => _lines;
            private set
            {
                if (_lines != value)
                {
                    _lines = value;
                    OnPropertyChanged(nameof(Lines));
                }
            }
        }
        
        public int Quantity
        {
            get => _quantity;
            private set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public Checkout()
        {
            Products = new ObservableCollection<Product>();
            Products.CollectionChanged += (sender, e) => UpdateTotal();
            Lines = Products.Count();
        }

        public void UpdateTotal()
        {
            Subtotal = Products.Sum(p => p.Price * p.Quantity);
            Quantity = Products.Sum(p => p.Quantity);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}