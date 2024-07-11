using Assessment.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace Assessment.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public DelegateCommand<Product> SelectProductCommand { get; set; }
        public DelegateCommand<Product> AddQuantityCommand { get; set; }
        public DelegateCommand<Product> SubtractQuantityCommand { get; set; }

        private Checkout _checkout;
        public Checkout Checkout
        {
            get => _checkout;
            set
            {
                SetProperty(ref _checkout, value);
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

        public CheckoutViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            SubtractQuantityCommand = new DelegateCommand<Product>(OnSubtractQuantity);
        }

        private void OnAddQuantity(Product product)
        {
            if (product != null)
            {
                product.Quantity += 1;
                Checkout.UpdateTotal();
            }
        }

        private void OnSubtractQuantity(Product product)
        {
            if (product != null && product.Quantity > 0)
            {
                product.Quantity -= 1;
                Checkout.UpdateTotal();
            }
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
                Checkout = new Checkout();
                Checkout.Products = parameters.GetValue<ObservableCollection<Product>>("products");
                RaisePropertyChanged(nameof(Checkout));
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            SelectedProduct = null;
        }
    }
}