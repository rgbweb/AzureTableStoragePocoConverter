using System;

namespace AzureTableStoragePocoConverter.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RowKeyAttribute : Attribute
    {
    }
}
