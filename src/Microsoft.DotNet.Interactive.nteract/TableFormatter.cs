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
            JArray fields = null;

            var members = new HashSet<(string name, Type type)>();
            var data = new JArray();

            foreach (var item in source)
            {
                switch (item)
                {
                    case IEnumerable<(string name, object value)> valueTuples:

                        EnsureFieldsAreInitializedFromValueTuples(valueTuples);

                        var o = new JObject();
                        foreach (var tuple in valueTuples)
                        {
                            o.Add(tuple.name, JToken.FromObject(tuple.value ?? "NULL"));
                        }

                        data.Add(o);
                        break;

                    case IEnumerable<KeyValuePair<string, object>> keyValuePairs:

                        EnsureFieldsAreInitializedFromKeyValuePairs(keyValuePairs);

                        var obj = new JObject();
                        foreach (var pair in keyValuePairs)
                        {
                            obj.Add(pair.Key, JToken.FromObject(pair.Value));
                        }

                        data.Add(obj);
                        break;

                    default:
                        foreach (var memberInfo in item
                                                   .GetType()
                                                   .GetMembers(BindingFlags.Public | BindingFlags.Instance))
                        {
                            switch (memberInfo)
                            {
                                case PropertyInfo pi:
                                    members.Add((memberInfo.Name, pi.PropertyType));
                                    break;
                                case FieldInfo fi:
                                    members.Add((memberInfo.Name, fi.FieldType));
                                    break;
                            }
                        }

                        EnsureFieldsAreInitializedFromMembers();
                        data.Add(JObject.FromObject(item));
                        break;
                }
            }

            var schema = new JObject
            {
                ["primaryKey"] = new JArray(),
                ["fields"] = fields
            };

            return (schema, data);

            void EnsureFieldsAreInitializedFromMembers()
            {
                if (fields is null)
                {
                    fields = new JArray();
                    foreach (var memberInfo in members)
                    {
                        fields.Add(new JObject
                        {
                            ["name"] = memberInfo.name,
                            ["type"] = memberInfo.type.ToTableFieldType()
                        });
                    }
                }
            }

            void EnsureFieldsAreInitializedFromValueTuples(IEnumerable<(string name, object value)> valueTuples)
            {
                if (fields is null)
                {
                    fields = new JArray();
                    foreach (var tuple in valueTuples)
                    {
                        fields.Add(new JObject
                        {
                            ["name"] = tuple.name,
                            ["type"] = tuple.value?.GetType().ToTableFieldType()
                        });
                    }
                }
            }

            void EnsureFieldsAreInitializedFromKeyValuePairs(IEnumerable<KeyValuePair<string, object>> keyValuePairs)
            {
                if (fields is null)
                {
                    fields = new JArray();
                    foreach (var keyValuePair in keyValuePairs)
                    {
                        fields.Add(new JObject
                        {
                            ["name"] = keyValuePair.Key,
                            ["type"] = keyValuePair.Value?.GetType().ToTableFieldType()
                        });
                    }
                }
            }
        }

        private static string ToTableFieldType(this Type type) =>
            type switch
            {
                { } t when t == typeof(bool) => "boolean",
                { } t when t == typeof(DateTime) => "date",
                { } t when t == typeof(int) => "integer",
                { } t when t == typeof(long) => "integer",
                { } t when t == typeof(double) => "number",
                _ => "string",
            };
    }
}