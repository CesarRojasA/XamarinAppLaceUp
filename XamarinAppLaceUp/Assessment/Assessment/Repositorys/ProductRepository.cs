using Assessment.Models;

namespace Assessment.Data
{
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(string dbPath) : base(dbPath)
        {
        }
    }
}
