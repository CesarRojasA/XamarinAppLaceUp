using Assessment.Models;

namespace Assessment.Data
{
    public class CheckoutRepository : RepositoryBase<Checkout>, ICheckoutRepository
    {
        public CheckoutRepository(string dbPath) : base(dbPath)
        {
        }
    }
}