using SQLite;
using System.ComponentModel;

namespace Assessment.Models
{
    public class Product : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _name;
        private double _price;
        private string _imageUrl;
        private string _quantity;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public double Price
        {
            get => _price;
            set
            {
                if (_price != value)
                {
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }

        public string ImageUrl
        {
            get => _imageUrl;
            set
            {
                if (_imageUrl != value)
                {
                    _imageUrl = value;
                    OnPropertyChanged(nameof(ImageUrl));
                }
            }
        }

        public string Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }
                if (_quantity == "0")
                {
                    _quantity = "+";
                    OnPropertyChanged(nameof(Quantity));
                }
            }
        }

        public Product()
        {
            if (string.IsNullOrEmpty(_quantity))
            {
                _quantity = "+";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
