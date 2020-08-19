// formatter that implements the tabular mimetype as described at https://specs.frictionlessdata.io/table-schema/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.DotNet.Interactive.Formatting;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Interactive.nteract
{
    public static class TableFormatter
    {
        public static string MimeType => "application/table-schema+json";

        public static string nteractMimeType => "application/vnd.dataresource+json";

        public static TabularJsonString ToTabularData(this IEnumerable source)
        {
            var (schema, data) = Generate(source);

            var tabularData = new JObject
            {
                ["schema"] = schema,
                ["data"] = data
            };

            return new TabularJsonString(tabularData.ToString(Newtonsoft.Json.Formatting.Indented));
        }

        public static void Explore(this IEnumerable source)
        {
            KernelInvocationContext.Current.Display(new DataExplorer(source), HtmlFormatter.MimeType);
        }

        private static (JObject schema, JArray data) Generate(IEnumerable source)
        {
            var fields = new JArray();
            var schema = new JObject
            {
                ["primaryKey"] = new JArray(),
                ["fields"] = fields
            };

            var members = new HashSet<(string name, Type type)>();
            var data = new JArray();
            
            foreach (var item in source)
            {
                foreach (var memberInfo in item
                                           .GetType()
                                           .GetMembers(BindingFlags.Public | BindingFlags.Instance))
                {
                    switch (memberInfo)
                    {
                        case PropertyInfo pi:
                            members.Add((memberInfo.Name, pi.PropertyType));
                            break;
                    }
                }

                data.Add(JObject.FromObject(item));
            }

            foreach (var memberInfo in members)
            {
                fields.Add(new JObject
                {
                    ["name"] = memberInfo.name,
                    ["type"] = memberInfo.type.ToTableFieldType()
                });
            }

            return (schema, data);
        }

        private static string ToTableFieldType(this Type type) =>
            type switch
            {
                { } t when t == typeof(string) => "string",
                { } t when t == typeof(bool) => "boolean",
                { } t when t == typeof(DateTime) => "date",
                { } t when t == typeof(int) => "integer",
                { } t when t == typeof(double) => "number",
                _ => throw new InvalidOperationException($"Type {type} is not supported.")
            };
    }
}