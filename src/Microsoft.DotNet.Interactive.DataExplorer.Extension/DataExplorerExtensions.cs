using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.DotNet.Interactive.Formatting;

using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Interactive.DataExplorer.Extension
{
    public static class DataExplorerExtensions
    {
        public static T UseDataExplorer<T>(this T kernel) where T : Kernel
        {
            RegisterFormatters();
            return kernel;
        }

        public static void RegisterFormatters()
        {
            Formatter<IEnumerable>.Register((enumerable, writer) =>
           {
               var tabularData = ToTabularData(enumerable);

               writer.Write(tabularData.ToString(formatting: Newtonsoft.Json.Formatting.Indented));
           }, TableFormatter.MimeType);
        }

        private static JObject ToTabularData(IEnumerable enumerable)
        {
            var (schema, data) = Generate(enumerable);

            var tabularData = new JObject
            {
                ["schema"] = schema,
                ["data"] = data
            };
            return tabularData;
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