using Assessment.Models;
using Prism.Navigation;

namespace Assessment.ViewModels
{
    public class ProductDetailsViewModel : ViewModelBase
    {
        #region Fields

        private Product _product;

        #endregion

        #region Properties

        public Product Product
        {
            get => _product;
            set => SetProperty(ref _product, value);
        }

        #endregion

        #region Constructors

        public ProductDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {
        }

        #endregion

        #region Navigation Methods

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("product"))
            {
                Product = parameters.GetValue<Product>("product");
            }
        }

        #endregion
    }
}
