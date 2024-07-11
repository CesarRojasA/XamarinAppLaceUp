using Assessment.Data;
using Assessment.Models;
using Assessment.Resources;
using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Assessment.ViewModels
{
    public class ProductCatalogViewModel : ViewModelBase
    {
        #region Properties
        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Product> SelectedProducts { get; set; }
        public ObservableCollection<Product> FilteredProducts { get; set; }

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
        #endregion

        #region Commands
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand<Product> AddQuantityCommand { get; set; }
        public DelegateCommand<Product> SubtractQuantityCommand { get; set; }
        public DelegateCommand GoToCheckoutCommand { get; set; }
        public DelegateCommand<Product> SelectProductCommand { get; set; }
        #endregion

        #region Constructor
        public ProductCatalogViewModel(INavigationService navigationService) : base(navigationService)
        {
            InitializeCommands();
            InitializeCollections();
            LoadSampleData();
        }
        #endregion

        #region Initialization Methods
        private void InitializeCommands()
        {
            SearchCommand = new DelegateCommand(OnSearch);
            AddQuantityCommand = new DelegateCommand<Product>(OnAddQuantity);
            SelectProductCommand = new DelegateCommand<Product>(OnSelectProduct);
            GoToCheckoutCommand = new DelegateCommand(GoToCheckout);
        }

        private void InitializeCollections()
        {
            Products = new ObservableCollection<Product>();
            SelectedProducts = new ObservableCollection<Product>();
            FilteredProducts = new ObservableCollection<Product>();
        }

        private async void LoadSampleData()
        {
            await GetSampleData();
            FilteredProducts = new ObservableCollection<Product>(Products);
            RaisePropertyChanged(nameof(FilteredProducts));
        }
        #endregion

        #region Command Methods
        private async void GoToCheckout()
        {
            var parameters = new NavigationParameters
            {
                { "products", SelectedProducts },
                { "isVisibleControlsInit", false }
            };
            await NavigationService.NavigateAsync("CheckoutPage", parameters);
        }

        private async void OnAddQuantity(Product product)
        {
            if (product != null)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync(
                               ProductCatalogResource.Quantity,
                               ProductCatalogResource.InsertNewAmount,
                               ProductCatalogResource.OK,
                               ProductCatalogResource.Cancel,
                               keyboard: Keyboard.Numeric);

                if (result != null && int.TryParse(result, out var quantity) && quantity >= 0)
                {
                    product.Quantity = quantity.ToString();
                    RaisePropertyChanged(nameof(FilteredProducts));
                    if (!SelectedProducts.Contains(product))
                    {
                        SelectedProducts.Add(product);
                    }
                }
            }
        }

        private async void OnSelectProduct(Product product)
        {
            var parameters = new NavigationParameters { { "product", product } };
            await NavigationService.NavigateAsync("ProductDetailsPage", parameters);
        }

        private void OnSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                FilteredProducts = new ObservableCollection<Product>(Products);
            }
            else
            {
                FilteredProducts = new ObservableCollection<Product>(
                    Products.Where(p => p.Name.ToLower().Contains(SearchText.ToLower())));
            }
            RaisePropertyChanged(nameof(FilteredProducts));
        }
        #endregion

        #region Data Methods
        private async Task GetSampleData()
        {
            Products = new ObservableCollection<Product>(await Database.ProductRepository.GetItemsAsync());
            if (!Products.Any())
            {
                await Application.Current.MainPage.DisplayAlert(
                               "SQLite",
                               "Saving information locally...",
                               "OK");

                await SaveInitialData();
                Products = new ObservableCollection<Product>(await Database.ProductRepository.GetItemsAsync());
            }
        }

        private async Task SaveInitialData()
        {
            var initialProducts = new List<Product>
            {
                new Product { Name = "Ajo Pelado 20/1 Lb., China", Price = 1.33, ImageUrl = "https://ucosapobla.com/167-large_default/ajo-pelado-cat-1.jpg" },
                new Product { Name = "Ajo Porro (Leek), Ca & Guat", Price = 2.79, ImageUrl = "https://ve.biiligo.com/image/cache/catalog/biiligo-Ajo-Porro-de-500gr-2983img--0-1000x1000.jpg" },
                new Product { Name = "Albahaca Caja, Col", Price = 2.2, ImageUrl = "https://carmencita.com/shop/1240-large_default/albahaca.jpg" },
                new Product { Name = "Albahaca Pqte, CR", Price = 1.33, ImageUrl = "https://www.googleapis.com/download/storage/v1/b/global-cyd.appspot.com/o/product-images%2F032315_1?generation=1603303770797647&alt=media" },
                new Product { Name = "Apio (Celery) 3 Doc, Fl", Price = 1.33, ImageUrl = "https://agrisolver.s3.amazonaws.com/1503/conversions/medialibraryIw8Nit-medium.jpg" },
                new Product { Name = "Apple Juice", Price = 15, ImageUrl = "https://i0.wp.com/nycstreetgrill.com/wp-content/uploads/2019/08/glass-of-apple-juice-with-slices-of-red-apple-vector-18547427-copy.jpg" },
                new Product { Name = "Aveleda Vinho Cerde", Price = 12, ImageUrl = "https://www.marketviewliquor.com/mm5/graphics/00000001/AVELEDAVINHOVERDE.jpg" },
                new Product { Name = "B0816 Soothing Balm Stick", Price = 9, ImageUrl = "https://bloomhemp.com/wp-content/uploads/2023/07/Soothing-Balm-2.jpg" },
                new Product { Name = "B1273 November/December Special! Holiday Exclusive! NEW! Merry Berry Body Icing", Price = 13, ImageUrl = "https://cdn-jcghb.nitrocdn.com/oeLHKYDHpfvrqlFHKpqYUAfnMmAPBgKt/assets/images/optimized/rev-ed2f6cf/shopspaproducts.com/wp-content/uploads/2022/02/lavender-mint-body-icing-lemongrass-spa.png" },
                new Product { Name = "B1290 Holiday Exclusive! Buttercream Body Icing", Price = 9, ImageUrl = "https://jaquabathandbody.com/cdn/shop/files/body-lotion-buttercream-frosting.jpg?v=1684431381" },
                new Product { Name = "B1314 Holiday Exclusive! Christmas Cookie Happy New Year ", Price = 7, ImageUrl = "https://png.pngtree.com/png-clipart/20231020/original/pngtree-christmas-cookie-happy-new-year-painting-decoration-element-png-image_13384969.png" },
                new Product { Name = "B1334 NEW! Lemon Sorbet Body Icing", Price = 14, ImageUrl = "https://i.ebayimg.com/images/g/IYkAAOSwjyBkgic-/s-l1200.webp" },
                new Product { Name = "B1373 November/December Bonus Buy! Holiday Exclusive! NEW! Merry Berry Body Icing 1oz", Price = 6, ImageUrl = "https://static.wixstatic.com/media/5f2aa6_260c1f4ada834d2bb600742f98f90708~mv2.jpg/v1/fill/w_418,h_586,al_c,lg_1,q_80,enc_auto/5f2aa6_260c1f4ada834d2bb600742f98f90708~mv2.jpg" },
                new Product { Name = "B1390 Holiday Exclusive! NEW! Buttercream Hand Creme", Price = 8, ImageUrl = "https://cdnx.jumpseller.com/importadora-terra-austral/image/15945276/resize/1200/1200?1640116987" },
                new Product { Name = "Baby Zucchini", Price = 1.33, ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcSFj7Q93RV2z2_K5DvTDt-PAjaLT_nm8wME8w&s" },
                new Product { Name = "Banana Amigo, Hond", Price = 1.33, ImageUrl = "https://www.frutas-hortalizas.com/img/fruites_verdures/presentacio/1.jpg" },
                new Product { Name = "Banana Chiqita , Guate", Price = 1.33, ImageUrl = "https://static.libertyprim.com/files/familles/banane-large.jpg?1569271725" },
                new Product { Name = "Banana Del Monte & Rosy, Cr", Price = 1.33, ImageUrl = "https://delmonte-sitefinity-public.s3.amazonaws.com/images/default-source/product-slider-images/delmonte-isolatedimage-banana_monzano-988x604d1cfa12c-0b96-4eb5-bd3e-859cdaf6416e.jpg?sfvrsn=f1e2df52_1" },
                new Product { Name = "Banana Dole, Guate", Price = 1.33, ImageUrl = "https://cdn.farmjournal.com/s3fs-public/styles/840x600/public/2023-03/Dole-bananas-Conventional-Bananas_Small-GS1-Sticker.png" },
                new Product { Name = "Tomate Cherry", Price = 2.79, ImageUrl = "https://www.momorganics.co/images/virtuemart/product/TOMATE%20CHERRY.jpg" }
            };

            foreach (var product in initialProducts)
            {
                await Database.ProductRepository.SaveItemAsync(product);
            }
        }
        #endregion

        #region Navigation Methods
        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            SelectedProduct = null;
        }
        #endregion
    }
}
