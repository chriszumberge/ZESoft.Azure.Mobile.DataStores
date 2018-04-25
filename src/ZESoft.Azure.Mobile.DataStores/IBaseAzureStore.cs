using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores
{
    public interface IBaseAzureStore<T> where T : class, IAzureDataObject, new()
    {
        string Identifier { get; }

        Task<IEnumerable<T>> GetItemsAsync();
        Task<T> GetItemAsync(string id);
        Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> queryPredicate);
        Task<T> InsertAsync(T item);
        Task<bool> UpdateAsync(T item);
        Task<bool> DeleteAsync(string id);
    }
}
