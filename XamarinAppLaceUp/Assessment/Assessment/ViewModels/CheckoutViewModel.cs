using Assessment.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace Assessment.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public ObservableCollection<Product> FilteredProducts { get; set; }
        public DelegateCommand<Product> SelectProductCommand { get; set; }
        public DelegateCommand NavigateToCatalogCommand { get; set; }

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

        public CheckoutViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            NavigateToCatalogCommand = new DelegateCommand(OnNavigateToCatalog);
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("products"))
            {
                FilteredProducts = parameters.GetValue<ObservableCollection<Product>>("products");
                RaisePropertyChanged(nameof(FilteredProducts));
            }
        }

        private async void OnNavigateToCatalog()
        {
            await NavigationService.NavigateAsync("ProductCatalogPage");
        }
    }
}