using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AzureTableStoragePocoConverter.Extensions
{
    static class PropertyInfoArrayExtensions
    {
        public static PropertyInfo[] FindByAttributeType(this PropertyInfo[] propertyInfos,
            Type attributeType)
        {
            var result = new List<PropertyInfo>();

            foreach (var propertyInfo in propertyInfos)
            {
                var attributeTypes = propertyInfo.CustomAttributes.Select(attribute => attribute.AttributeType);

                if (attributeTypes.Any(type => type.IsEquivalentTo(attributeType)))
                {
                    result.Add(propertyInfo);
                }
            }

            return result.ToArray();
        }

        public static PropertyInfo Single(this PropertyInfo[] propertyInfos,
            Type attributeType)
        {
            return propertyInfos.GetMatchingPropertyInfo(attributeType, true);
        }

        public static PropertyInfo SingleOrDefault(this PropertyInfo[] propertyInfos,
            Type attributeType)
        {
            return propertyInfos.GetMatchingPropertyInfo(attributeType, false);
        }

        private static PropertyInfo GetMatchingPropertyInfo(this PropertyInfo[] propertyInfos,
            Type attributeType,
            bool oneIteRequired)
        {
            var matchingProperties = propertyInfos.FindByAttributeType(attributeType);

            if (oneIteRequired && matchingProperties.Length == 0)
            {
                throw new Exception($"Object needs to have exactly one public property decorated with the '{attributeType.Name}'. " +
                   "No matching property found.");
            }

            if (matchingProperties.Length > 1)
            {
                throw new Exception($"Object needs to have only one public property decorated with the '{attributeType.Name}'. " +
                    $"{matchingProperties.Length} matching properties were found.");
            }

            return matchingProperties.FirstOrDefault();
        }
    }
}
