using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assessment.Data
{
    public interface IRepository<T>
    {
        Task<int> SaveItemAsync(T item);
        Task<int> DeleteItemAsync(T item);
        Task<T> GetItemAsync(int id);
        Task<List<T>> GetItemsAsync();
    }
}
