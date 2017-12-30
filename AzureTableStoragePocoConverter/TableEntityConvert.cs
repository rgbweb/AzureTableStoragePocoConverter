using AzureTableStoragePocoConverter.Converters;
using Microsoft.WindowsAzure.Storage.Table;
using System;

namespace AzureTableStoragePocoConverter
{
    public static class TableEntityConvert
    {
        public static ITableEntity ToTableEntity(object poco)
        {
            var converter = new ObjectToTableEntityConverter(poco);

            return converter.GetTableEntity();
        }

        public static T FromTableEntity<T>(DynamicTableEntity tableEntity) where T : class, new()
        {
            var converter = new TableEntityToObjectConverter<T>(tableEntity);

            return converter.GetObject();
        }

        public static T FromTableEntity<T>(object tableEntity) where T : class, new()
        {
            var dynamicTableEntity = tableEntity as DynamicTableEntity;

            if (dynamicTableEntity == default(DynamicTableEntity))
            {
                throw new ArgumentException("Parameter has to be of type DynamicTableEntity", nameof(tableEntity));
            }

            return FromTableEntity<T>(dynamicTableEntity);
        }
    }
}
