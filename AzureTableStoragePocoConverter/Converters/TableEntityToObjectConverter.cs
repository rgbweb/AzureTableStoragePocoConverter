using AzureTableStoragePocoConverter.Attributes;
using AzureTableStoragePocoConverter.Extensions;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AzureTableStoragePocoConverter.Converters
{
    public class TableEntityToObjectConverter<T> where T : class, new()
    {
        private T _resultObject;
        private PropertyInfo[] _reflectedProperties;


        public TableEntityToObjectConverter()
        {
            _resultObject = new T();
            _reflectedProperties = _resultObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        public TableEntityToObjectConverter(DynamicTableEntity tableEntity) : this()
        {
            SetPartitionKey(tableEntity.PartitionKey);
            SetRowKey(tableEntity.RowKey);
            SetETag(tableEntity.ETag);
            SetTimestamp(tableEntity.Timestamp);
            SetProperties(tableEntity.Properties);
        }

        public void SetPartitionKey(string value)
        {
            var property = _reflectedProperties.Single(typeof(PartitionKeyAttribute));
            property.SetValue(_resultObject, value);
        }

        public void SetRowKey(string value)
        {
            var property = _reflectedProperties.Single(typeof(RowKeyAttribute));
            property.SetValue(_resultObject, value);
        }

        public void SetETag(string value)
        {
            var property = _reflectedProperties.SingleOrDefault(typeof(ETagAttribute));
            if (property != default(PropertyInfo))
            {
                property.SetValue(_resultObject, value);
            }
        }

        public void SetTimestamp(DateTimeOffset value)
        {
            var property = _reflectedProperties.SingleOrDefault(typeof(TimestampAttribute));
            if (property != default(PropertyInfo))
            {
                property.SetValue(_resultObject, value);
            }
        }

        public void SetProperties(IDictionary<string, EntityProperty> properties)
        {
            foreach (var tableProperty in properties)
            {
                var objectProperty = _reflectedProperties.FirstOrDefault(p => p.Name.Equals(tableProperty.Key));
                if (objectProperty != default(PropertyInfo))
                {
                    objectProperty.SetTableEntityValue(_resultObject, tableProperty.Value);
                }
            }
        }

        public T GetObject()
        {
            return _resultObject;
        }
    }
}
