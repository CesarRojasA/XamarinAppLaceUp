using Assessment.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Assessment.ViewModels
{
    public class ProductCatalogViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<Product> AddQuantityCommand { get; set; }
        public DelegateCommand<Product> GoToCheckoutCommand { get; set; }
        public DelegateCommand<Product> SelectProductCommand { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                SearchCommand.Execute();
            }
        }

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                if (value != null)
                {
                    SelectProductCommand.Execute(value);
                }
            }
        }

        public ProductCatalogViewModel(INavigationService navigationService) : base(navigationService)
        {
            Products = new ObservableCollection<Product>(GetSampleData());
            FilteredProducts = new ObservableCollection<Product>(Products);
            SearchCommand = new DelegateCommand(OnSearch);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            GoToCheckoutCommand = new DelegateCommand<Product>(GoToCheckout);
        }

        private void GoToCheckout(Product product)
        {
            NavigationService.NavigateAsync("CheckoutPage");
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SelectedProduct = null;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        private void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<Product>(Products);
            }
            else
            {
                FilteredProducts = new ObservableCollection<Product>(Products.Where(p => p.Name.ToLower().Contains(SearchText.ToLower())));
            }
            RaisePropertyChanged(nameof(FilteredProducts));
        }

        private void OnAddQuantity(Product product)
        {
            // Lógica para añadir cantidad al producto
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        private List<Product> GetSampleData()
        {
            return new List<Product>
        {
            new Product { Name = "Ajo Pelado", Price = 1.33, ImageUrl = "https://ucosapobla.com/167-large_default/ajo-pelado-cat-1.jpg" },
            new Product { Name = "Ajo Porro", Price = 2.79, ImageUrl = "https://ve.biiligo.com/image/cache/catalog/biiligo-Ajo-Porro-de-500gr-2983img--0-1000x1000.jpg" },
            // Agrega más productos aquí...
        };
        }
    }
}