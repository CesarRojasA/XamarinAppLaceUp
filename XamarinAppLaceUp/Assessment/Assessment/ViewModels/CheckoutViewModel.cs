using Assessment.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Collections.ObjectModel;

namespace Assessment.ViewModels
{
    public class CheckoutViewModel : ViewModelBase
    {
        public ObservableCollection<Product> CheckoutProducts { get; set; }
        public DelegateCommand<Product> EditQuantityCommand { get; set; }
        public DelegateCommand<Product> SelectProductCommand { get; set; }
        public DelegateCommand NavigateToCatalogCommand { get; set; }

        public CheckoutViewModel(INavigationService navigationService) : base(navigationService)
        {
            CheckoutProducts = new ObservableCollection<Product>();
            EditQuantityCommand = new DelegateCommand<Product>(OnEditQuantity);
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            NavigateToCatalogCommand = new DelegateCommand(OnNavigateToCatalog);
        }

        private void OnEditQuantity(Product product)
        {
            // Lógica para editar cantidad del producto
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        private async void OnNavigateToCatalog()
        {
            await NavigationService.NavigateAsync("ProductCatalogPage");
        }
    }
}