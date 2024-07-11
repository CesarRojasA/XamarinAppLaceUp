using Assessment.Models;
using Assessment.Resources;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Assessment.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        #region Properties
        private Checkout _checkout;
        public Checkout Checkout
        {
            get => _checkout;
            set => SetProperty(ref _checkout, value);
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

        private bool _isVisibleControlsInit = true;
        public bool IsVisibleControlsInit
        {
            get => _isVisibleControlsInit;
            set => SetProperty(ref _isVisibleControlsInit, value);
        }
        #endregion

        #region Commands
        public DelegateCommand<Product> SelectProductCommand { get; private set; }
        public DelegateCommand<Product> AddQuantityCommand { get; private set; }
        public DelegateCommand NavigateToCatalogCommand { get; private set; }
        #endregion

        #region Constructor
        public CheckoutViewModel(INavigationService navigationService) : base(navigationService)
        {
            InitializeCommands();
        }
        #endregion

        #region Initialization Methods
        private void InitializeCommands()
        {
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            NavigateToCatalogCommand = new DelegateCommand(OnNavigateToCatalog);
        }
        #endregion

        #region Command Methods
        private async void OnAddQuantity(Product product)
        {
            if (product != null)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync(
                               CheckoutResource.Quantity,
                               CheckoutResource.InsertNewAmount,
                               CheckoutResource.OK,
                               CheckoutResource.Cancel,
                               keyboard: Keyboard.Numeric);

                if (result != null && int.TryParse(result, out var quantity) && quantity >= 0)
                {
                    product.Quantity = quantity.ToString();
                    if (quantity == 0)
                    {
                        Checkout.Products.Remove(product);
                    }
                    Checkout.Update();
                }
            }
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        private async void OnNavigateToCatalog()
        {
            await NavigationService.NavigateAsync("NavigationPage/ProductCatalogPage");
        }
        #endregion

        #region Navigation Methods
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            Checkout = new Checkout();
            if (parameters.ContainsKey("products"))
            {
                Checkout.Products = parameters.GetValue<ObservableCollection<Product>>("products");
            }
            RaisePropertyChanged(nameof(Checkout));

            if (parameters.ContainsKey("isVisibleControlsInit"))
            {
                IsVisibleControlsInit = parameters.GetValue<bool>("isVisibleControlsInit");
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            SelectedProduct = null;
        }
        #endregion
    }
}
