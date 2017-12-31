using AzureTableStoragePocoConverter.Sample.Pocos;
using Microsoft.WindowsAzure.Storage.Table;
using System.Threading.Tasks;

namespace AzureTableStoragePocoConverter.Sample
{
    class Repository
    {
        private CloudTable _cloudTable;

        public Repository(CloudTable cloudTable)
        {
            _cloudTable = cloudTable;
        }

        public async Task<Customer> GetCustomer(string lastName, string firstName)
        {
            // Load the DynamicTableEntity object from Storage using the keys
            var operation = TableOperation.Retrieve(lastName, firstName);
            var result = await _cloudTable.ExecuteAsync(operation);

            // Convert into the POCO using TableEntityConvert.FromTableEntity<T>()
            var customer = TableEntityConvert.FromTableEntity<Customer>(result.Result);

            return customer;
        }

        public async Task AddOrUpdateCustomer(Customer customer)
        {
            // Convert POCO to ITableEntity object using TableEntityConvert.ToTableEntity()
            var tableEntity = TableEntityConvert.ToTableEntity(customer);

            // Save the new or updated entity in the Storage
            var operation = TableOperation.InsertOrReplace(tableEntity);
            await _cloudTable.ExecuteAsync(operation);
        }
    }
}
