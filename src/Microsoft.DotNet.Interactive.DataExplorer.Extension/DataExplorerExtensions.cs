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
            Formatter.Register<IEnumerable>((enumerable, writer) =>
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

            if (propertyType == typeof(string))
            {
                return "string";
            }

            if (propertyType == typeof(bool))
            {
                return "boolean";
            }

            if (propertyType == typeof(DateTime))
            {
                return "date";
            }

            if (propertyType == typeof(int))
            {
                return "integer";
            }

            if (propertyType == typeof(double))
            {
                return "number";
            }

            throw new InvalidOperationException($"Type {propertyType} is not supported.");

        }
    }
}