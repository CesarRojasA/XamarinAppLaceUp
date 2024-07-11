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

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
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
                new Product { Id = 0, Name = "Ajo Pelado 20/1 Lb., China", Price = 1.33, ImageUrl = "https://ucosapobla.com/167-large_default/ajo-pelado-cat-1.jpg" },
                new Product { Id = 1, Name = "Ajo Porro (Leek), Ca & Guat", Price = 2.79, ImageUrl = "https://ve.biiligo.com/image/cache/catalog/biiligo-Ajo-Porro-de-500gr-2983img--0-1000x1000.jpg" },
                new Product { Id = 2, Name = "Albahaca Caja, Col", Price = 2.2},
                new Product { Id = 3, Name = "Albahaca Pqte, CR", Price = 1.33 },
                new Product { Id = 4, Name = "Apio (Celery) 3 Doc, Fl", Price = 1.33 },
                new Product { Id = 5, Name = "Apple Juice", Price = 15 },
                new Product { Id = 6, Name = "Aveleda Vinho Cerde", Price = 12 },
                new Product { Id = 7, Name = "B0816 Soothing Balm Stick", Price = 9 },
                new Product { Id = 8, Name = "B1273 November/December Special! Holiday Exclusive! NEW! Merry Berry Body Icing", Price = 13 },
                new Product { Id = 9, Name = "B1290 Holiday Exclusive! Buttercream Body Icing", Price = 9 },
                new Product { Id = 10, Name = "B1314 Holiday Exclusive! Holiday Healing Element 1oz", Price = 7 },
                new Product { Id = 11, Name = "B1334 NEW! Lemon Sorbet Body Icing", Price = 14 },
                new Product { Id = 12, Name = "B1373 November/December Bonus Buy! Holiday Exclusive! NEW! Merry Berry Body Icing 1oz", Price = 6 },
                new Product { Id = 13, Name = "B1390 Holiday Exclusive! NEW! Buttercream Hand Creme", Price = 8 },
                new Product { Id = 14, Name = "Baby Zucchini", Price = 1.33 },
                new Product { Id = 15, Name = "Banana Amigo, Hond", Price = 1.33 },
                new Product { Id = 16, Name = "Banana Chiqita , Guate", Price = 1.33 },
                new Product { Id = 17, Name = "Banana Del Monte & Rosy, Cr", Price = 1.33 },
                new Product { Id = 18, Name = "Banana Dole, Guate", Price = 1.33 }
            };
        }
    }
}