using Assessment.Models;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Assessment.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public DelegateCommand<Product> SelectProductCommand { get; set; }
        public DelegateCommand<Product> AddQuantityCommand { get; set; }
        public DelegateCommand<Product> SubtractQuantityCommand { get; set; }
        public DelegateCommand NavigateToCatalogCommand { get; set; }

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
        
        private bool _isVisibleButtonGotoCatalog = true;
        public bool IsVisibleButtonGotoCatalog 
        {
            get => _isVisibleButtonGotoCatalog;
            set
            {
                SetProperty(ref _isVisibleButtonGotoCatalog, value);
            }
        }

        public CheckoutViewModel(INavigationService navigationService) : base(navigationService)
        {
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            NavigateToCatalogCommand = new DelegateCommand(OnNavigateToCatalog);
        }

        private async void OnAddQuantity(Product product)
        {
            if (product != null)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync(
                               "Enter a number",
                               "Please enter a number below:",
                               "OK",
                               "Cancel",
                               keyboard: Keyboard.Numeric);

                if (result == null)
                {
                    return;
                }

                if (int.TryParse(result, out var quantity) && quantity >= 0)
                {
                    product.Quantity = quantity.ToString();
                    if (quantity == 0)
                    {
                        Checkout.Products.Remove(product);
                    }
                    Checkout.UpdateTotal();
                }
            }
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Checkout = new Checkout();
            if (parameters.ContainsKey("products"))
            {
                Checkout.Products = parameters.GetValue<ObservableCollection<Product>>("products");
            }
            RaisePropertyChanged(nameof(Checkout));

            if (parameters.ContainsKey("isVisibleButtonGotoCatalog"))
            {
                IsVisibleButtonGotoCatalog = parameters.GetValue<bool>("isVisibleButtonGotoCatalog");
            }
        }
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            SelectedProduct = null;
        }
        private async void OnNavigateToCatalog()
        {
            await NavigationService.NavigateAsync("NavigationPage/ProductCatalogPage");
        }
    }
}