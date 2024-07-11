using System.IO;
using Xamarin.Essentials;

namespace Assessment.Data
{
    public static class Database
    {
        private static string dbPath = Path.Combine(FileSystem.AppDataDirectory, "AppData.db3");

        private static IProductRepository _productRepository;
        public static IProductRepository ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(dbPath);
                }
                return _productRepository;
            }
        }

        private static ICheckoutRepository _checkoutRepository;
        public static ICheckoutRepository CheckoutRepository
        {
            get
            {
                if (_checkoutRepository == null)
                {
                    _checkoutRepository = new CheckoutRepository(dbPath);
                }
                return _checkoutRepository;
            }
        }
    }
}
