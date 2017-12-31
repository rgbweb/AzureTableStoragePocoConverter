# Azure Table Storage POCO Converter
[![Build status](https://ci.appveyor.com/api/projects/status/dtdem4fca8jabhm7?svg=true)](https://ci.appveyor.com/project/rgbweb/azuretablestoragepococonverter)
[![Release version](https://img.shields.io/github/release/rgbweb/AzureTableStoragePocoConverter.svg?style=flat-square)]()
[![NuGet version](https://img.shields.io/nuget/v/AzureTableStoragePocoConverter.svg?style=flat-square&colorB=004880)](https://www.nuget.org/packages/AzureTableStoragePocoConverter/)
[![License: MIT](https://img.shields.io/github/license/rgbweb/AzureTableStoragePocoConverter.svg?style=flat-square&colorB=969696)](https://github.com/rgbweb/AzureTableStoragePocoConverter/blob/master/LICENSE)

.NET Standard 2.0 library with helpers to convert your POCO objects to Azure Table Storage objects of type `ITableEntity` or `DynamicTableEntity` and vice versa.

It supports saving complex classes by converting not natively supported property types into JSON strings.

There is no need to have dedicated properties called PartitionKey and RowKey. Just decorate your properties with the `[PartitionKey]` and `[RowKey]` attributes. There is also an optional `[ETag]` attribute to read the ETag and ensure optimistic concurrency when writing it back. You can decorate a property with the `[Timestamp]` attribute to read the auto-generated time stamp from your table entity.

## NuGet Package
https://www.nuget.org/packages/AzureTableStoragePocoConverter/

Install the package `AzureTableStoragePocoConverter`

## How to use
You can find a working example application in the `AzureTableStoragePocoConverter.Sample` project in this repository.


### Create your POCO class
Create your POCO classes and decorate properties with the required `[PartitionKey]` and `[RowKey]` attributes and optionally with `[ETag]` and `[Timestamp]` attributes from `AzureTableStoragePocoConverter.Attributes` namespace. You can use the `[IgnoreProperty]` from `Microsoft.WindowsAzure.Storage.Table` namespace to exclude properties from being added to the Table Storage entity.

```csharp
// Simple object - no base class needed
public class Customer
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

    // Complex type will be converted to JSON ans stored as string
    public Address Address { get; set; }

    // IgnoreProperty marks a property to be ignored when creating the TableEntity
    [IgnoreProperty]
    public string TemporaryLoginHash { get; set; }
}
```

### Save Data to Table Storage
Convert your data object to `ITableEntity` in order to save your data to Azure Table Storage using an operation like `InsertOrReplace`.

```csharp
// Convert POCO to ITableEntity object using TableEntityConvert.ToTableEntity()
var tableEntity = TableEntityConvert.ToTableEntity(customer);

// Save the new or updated entity in the Storage
var operation = TableOperation.InsertOrReplace(tableEntity);
await _cloudTable.ExecuteAsync(operation);
```

### Read Data from Table Storage
Read your data from Azure Table Storage by using the ordinary `Retrieve` operation without providing a Type to it. Than convert it back to your POCO with a single method call.

```csharp
// Load the DynamicTableEntity object from Storage using the keys
var operation = TableOperation.Retrieve(lastName, firstName);
var result = await _cloudTable.ExecuteAsync(operation);

// Convert into the POCO using TableEntityConvert.FromTableEntity<T>()
var customer = TableEntityConvert.FromTableEntity<Customer>(result.Result);
```

## Releases
Have a look at the [Releases page](https://github.com/rgbweb/AzureTableStoragePocoConverter/releases)

## Ask, Help, Contribute
Feel free to open an issue if you have questions ore want to help in some way.

## License
The MIT license can be found [here](https://github.com/rgbweb/AzureTableStoragePocoConverter/blob/master/LICENSE)