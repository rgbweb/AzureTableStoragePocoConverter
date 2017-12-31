using AzureTableStoragePocoConverter.Sample.Pocos;
using Microsoft.WindowsAzure.Storage;
using System;
using System.Threading.Tasks;

namespace AzureTableStoragePocoConverter.Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Using the Azure Storage Emulator
            var storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            var cloudTableClient = storageAccount.CreateCloudTableClient();
            var cloudTable = cloudTableClient.GetTableReference("CustomersTable");
            await cloudTable.CreateIfNotExistsAsync();

            var repository = new Repository(cloudTable);

            // Creating a fresh customer object
            var newCustomer = new Customer
            {
                LastName = "Smith",
                FirstName = "Jeff",
                Email = "Jeff@contoso.com",
                TemporaryLoginHash = Guid.NewGuid().ToString("N"),
                Address = new Address
                {
                    Street = "Sesame Street",
                    City = "Seattle"
                }
            };

            // Save as new or update if the customer exists in the table
            await repository.AddOrUpdateCustomer(newCustomer);

            // Load from table by using the two keys
            var tableCustomer = await repository.GetCustomer("Smith", "Jeff");

            Console.WriteLine($"Name:\t{tableCustomer.FirstName} {tableCustomer.LastName}");
            Console.WriteLine($"Email:\t{tableCustomer.Email}");
            Console.WriteLine($"Street:\t{tableCustomer.Address?.Street}");
            Console.WriteLine($"City:\t{tableCustomer.Address?.City}");
            Console.WriteLine($"Hash (should be empty):\t{tableCustomer.TemporaryLoginHash}");
            Console.WriteLine($"Time stamp (generated):\t{tableCustomer.Timestamp}");
            Console.WriteLine($"ETag (generated):\t{tableCustomer.ETag}");

            Console.WriteLine();
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
    }
}
