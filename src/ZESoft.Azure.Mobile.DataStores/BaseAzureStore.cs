using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZESoft.Azure.Mobile.Models;

namespace ZESoft.Azure.Mobile.DataStores
{
    public abstract class BaseAzureStore<T> : IBaseAzureStore<T> where T : class, IAzureDataObject, new()
    {
        public abstract string Identifier { get; }

        protected abstract string AzureServiceUrl { get; }

        MobileServiceClient _azureClient;
        IMobileServiceTable<T> _endpointTable;

        public BaseAzureStore()
        {
            _azureClient = new MobileServiceClient(AzureServiceUrl);
        }

        protected async Task InitializeAsync()
        {
            await Task.Run(() =>
            {
                if (_endpointTable != null)
                    return;

                _endpointTable = _azureClient.GetTable<T>();
            });
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            await InitializeAsync();

            return await _endpointTable.ToEnumerableAsync();
        }

        public async Task<T> GetItemAsync(string id)
        {
            await InitializeAsync();

            var items = await _endpointTable.Where(x => x.Id == id).ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items.First();
        }

        public async Task<IEnumerable<T>> QueryAsync(Expression<Func<T, bool>> queryPredicate)
        {
            await InitializeAsync();

            return await _endpointTable.Where(queryPredicate).ToListAsync();
        }

        public async Task<T> InsertAsync(T item)
        {
            await InitializeAsync();

            await _endpointTable.InsertAsync(item);

            return item;
        }

        public async Task<bool> UpdateAsync(T item)
        {
            await InitializeAsync();

            await _endpointTable.UpdateAsync(item);

            return true;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var item = await GetItemAsync(id);

            item.Deleted = true;

            await UpdateAsync(item);

            return true;
        }

    }
}
