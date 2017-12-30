using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace AzureTableStoragePocoConverter.Extensions
{
    static class PropertyInfoExtensions
    {
        public static EntityProperty GetValueAsEntityProperty(this PropertyInfo propertyInfo,
            object obj)
        {
            var value = propertyInfo.GetValue(obj);

            if (value is string)
            {
                return new EntityProperty((string)value);
            }

            if (value is byte[])
            {
                return new EntityProperty((byte[])value);
            }

            if (value is bool)
            {
                return new EntityProperty((bool)value);
            }

            if (value is bool?)
            {
                return new EntityProperty((bool?)value);
            }

            if (value is DateTime)
            {
                return new EntityProperty((DateTime)value);
            }

            if (value is DateTime?)
            {
                return new EntityProperty((DateTime?)value);
            }

            if (value is DateTimeOffset)
            {
                return new EntityProperty((DateTimeOffset)value);
            }

            if (value is DateTimeOffset?)
            {
                return new EntityProperty((DateTimeOffset?)value);
            }

            if (value is double)
            {
                return new EntityProperty((double)value);
            }

            if (value is double?)
            {
                return new EntityProperty((double?)value);
            }

            if (value is Guid?)
            {
                return new EntityProperty((Guid?)value);
            }

            if (value is Guid)
            {
                return new EntityProperty((Guid)value);
            }

            if (value is int)
            {
                return new EntityProperty((int)value);
            }

            if (value is int?)
            {
                return new EntityProperty((int?)value);
            }

            if (value is long)
            {
                return new EntityProperty((long)value);
            }

            if (value is long?)
            {
                return new EntityProperty((long?)value);
            }

            return new EntityProperty(JsonConvert.SerializeObject(value, Formatting.Indented));
        }


        public static void SetTableEntityValue(this PropertyInfo propertyInfo,
            object obj,
            EntityProperty value)
        {
            var type = propertyInfo.PropertyType;

            if (type == typeof(string))
            {
                propertyInfo.SetValue(obj, value.StringValue);
            }
            else if (type == typeof(byte[]))
            {
                propertyInfo.SetValue(obj, value.BinaryValue);
            }
            else if (type == typeof(bool))
            {
                propertyInfo.SetValue(obj, value.BooleanValue);
            }
            else if (type == typeof(bool?))
            {
                propertyInfo.SetValue(obj, value.BooleanValue);
            }
            else if (type == typeof(DateTime))
            {
                propertyInfo.SetValue(obj, value.DateTime);
            }
            else if (type == typeof(DateTime?))
            {
                propertyInfo.SetValue(obj, value.DateTime);
            }
            else if (type == typeof(DateTimeOffset))
            {
                propertyInfo.SetValue(obj, value.DateTimeOffsetValue);
            }
            else if (type == typeof(DateTimeOffset?))
            {
                propertyInfo.SetValue(obj, value.DateTimeOffsetValue);
            }
            else if (type == typeof(double))
            {
                propertyInfo.SetValue(obj, value.DoubleValue);
            }
            else if (type == typeof(double?))
            {
                propertyInfo.SetValue(obj, value.DoubleValue);
            }
            else if (type == typeof(Guid?))
            {
                propertyInfo.SetValue(obj, value.GuidValue);
            }
            else if (type == typeof(Guid))
            {
                propertyInfo.SetValue(obj, value.GuidValue);
            }
            else if (type == typeof(int))
            {
                propertyInfo.SetValue(obj, value.Int32Value);
            }
            else if (type == typeof(int?))
            {
                propertyInfo.SetValue(obj, value.Int32Value);
            }
            else if (type == typeof(long))
            {
                propertyInfo.SetValue(obj, value.Int64Value);
            }
            else if (type == typeof(long?))
            {
                propertyInfo.SetValue(obj, value.Int64Value);
            }
            else
            {
                var serilizedValue = value.StringValue;
                var valueObject = JsonConvert.DeserializeObject(serilizedValue, type);
                propertyInfo.SetValue(obj, valueObject);
            }
        }
    }
}
