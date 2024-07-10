using Assessment.Models;
using Prism.Navigation;

namespace Assessment.ViewModels
{
    public class ProductDetailsViewModel : ViewModelBase
    {
        private Product _product;
        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        public ProductDetailsViewModel(INavigationService navigationService) : base(navigationService) 
        { 
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
            }
        }
    }
}