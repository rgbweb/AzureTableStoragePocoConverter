using AzureTableStoragePocoConverter.Attributes;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureTableStoragePocoConverter.Sample.Pocos
{
    // Simple object - no base class needed
    public partial class Customer
    {
        // PartitionKey must be of type string and exist exactly one time per class
        [PartitionKey]
        public string LastName { get; set; }

        // PartitionKey must be of type string and exist exactly one time per class
        [RowKey]
        public string FirstName { get; set; }

        // ETag is optional and can safely have a private setter
        [ETag]
        public string ETag { get; private set; }

        // Timestamp is optional and can safely have a private setter
        [Timestamp]
        public DateTimeOffset Timestamp { get; private set; }

        // Ordinary string field will be stored as string in the entity
        public string Email { get; set; }

        // Complex type will be converted to JSON and stored as string
        public Address Address { get; set; }

        // IgnoreProperty marks a property to be ignored when creating the TableEntity
        [IgnoreProperty]
        public string TemporaryLoginHash { get; set; }
    }
}
