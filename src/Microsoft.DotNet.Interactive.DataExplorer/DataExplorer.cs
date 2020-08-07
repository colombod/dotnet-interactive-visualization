// formatter that implements the tabular mimetype as described at https://specs.frictionlessdata.io/table-schema/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Interactive.DataExplorer
{
    public static class TableFormatter
    {
        public static string MimeType => "application/table-schema+json";

        public static JObject ToTabularData(this IEnumerable enumerable)
        {
            var (schema, data) = Generate(enumerable);

            var tabularData = new JObject
            {
                ["schema"] = schema,
                ["data"] = data
            };
            return tabularData;
        }

        public static Explorer Explore(this IEnumerable source)
        {
            return new Explorer(source);
        }

        private static (JObject schema, JArray data) Generate(IEnumerable source)
        {
            var fields = new JArray();
            var schema = new JObject
            {
                ["primaryKey"] = new JArray(),
                ["fields"] = fields
            };

            var processedField = new HashSet<PropertyInfo>();
            var data = new JArray();
            foreach (var element in source)
            {
                foreach (var propertyInfo in element.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    processedField.Add(propertyInfo);
                }

                data.Add(JObject.FromObject(element));
            }

            foreach (var propertyInfo in processedField)
            {
                fields.Add(new JObject
                {
                    ["name"] = propertyInfo.Name,
                    ["type"] = ToTableFieldType(propertyInfo.PropertyType)
                });
            }
            return (schema, data);
        }

        private static string ToTableFieldType(Type propertyType)
        {
            return propertyType switch
            {
                { } t when t == typeof(string) => "string",
                { } t when t == typeof(bool) => "boolean",
                { } t when t == typeof(DateTime) => "date",
                { } t when t == typeof(int) => "integer",
                { } t when t == typeof(double) => "number",
                _ => throw new InvalidOperationException($"Type {propertyType} is not supported.")
            };
        }
    }
}
