using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Assessment.Data
{
    public class RepositoryBase<T> : IRepository<T> where T : new()
    {
        private readonly SQLiteAsyncConnection _database;

        public RepositoryBase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<T>().Wait();
        }

        public async Task<int> SaveItemAsync(T item)
        {
            var mapping = await _database.GetMappingAsync(typeof(T));
            var idProp = mapping.PK?.PropertyName;

            if (idProp != null)
            {
                var value = typeof(T).GetProperty(idProp)?.GetValue(item);
                if (value is int id && id != 0)
                {
                    return await _database.UpdateAsync(item);
                }
            }
            return await _database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(T item)
        {
            return _database.DeleteAsync(item);
        }

        public async Task<T> GetItemAsync(int id)
        {
            var items = await _database.Table<T>().ToListAsync();
            return items.Find(i => (int)typeof(T).GetProperty("Id").GetValue(i) == id);
        }

        public Task<List<T>> GetItemsAsync()
        {
            return _database.Table<T>().ToListAsync();
        }
    }
}
