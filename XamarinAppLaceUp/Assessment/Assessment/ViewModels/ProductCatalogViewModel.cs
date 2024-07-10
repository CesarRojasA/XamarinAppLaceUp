using Assessment.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Assessment.ViewModels
{
    public class ProductCatalogViewModel : ViewModelBase
    {
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> SelectedProducts { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<Product> AddQuantityCommand { get; set; }
        public DelegateCommand<Product> SubtractQuantityCommand { get; set; }
        public DelegateCommand GoToCheckoutCommand { get; set; }
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
            SelectedProducts = new ObservableCollection<Product>();
            SearchCommand = new DelegateCommand(OnSearch);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            SubtractQuantityCommand = new DelegateCommand<Product>(OnSubtractQuantity);
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            GoToCheckoutCommand = new DelegateCommand(GoToCheckout);
        }

        private async void GoToCheckout()
        {
            var parameters = new NavigationParameters { { "products", SelectedProducts} };
            await NavigationService.NavigateAsync("CheckoutPage", parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            SelectedProduct = null;
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
            if (product != null)
            {
                product.Quantity += 1;
                RaisePropertyChanged(nameof(FilteredProducts));
                if (!SelectedProducts.Contains(product))
                {
                    SelectedProducts.Add(product);
                }
            }
        }

        private void OnSubtractQuantity(Product product)
        {
            if (product != null && product.Quantity > 0)
            {
                product.Quantity -= 1;
                RaisePropertyChanged(nameof(FilteredProducts));
                if (product.Quantity == 0)
                {
                    SelectedProducts.Remove(product);
                }
            }
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
                new Product { Id = 0, Name = "Ajo Pelado", Price = 1.33, ImageUrl = "https://ucosapobla.com/167-large_default/ajo-pelado-cat-1.jpg" },
                new Product { Id = 1, Name = "Ajo Porro", Price = 2.79, ImageUrl = "https://ve.biiligo.com/image/cache/catalog/biiligo-Ajo-Porro-de-500gr-2983img--0-1000x1000.jpg" },
                // Agrega más productos aquí...
            };
        }
    }
}