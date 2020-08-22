// formatter that implements the tabular mimetype as described at https://specs.frictionlessdata.io/table-schema/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.DotNet.Interactive.Formatting;
using Microsoft.ML;
using Newtonsoft.Json.Linq;

namespace Microsoft.DotNet.Interactive.nteract
{
    public static class TableFormatter
    {
        // https://specs.frictionlessdata.io/table-schema/#language
        public static string MimeType => "application/table-schema+json";

        public static string nteractMimeType => "application/vnd.dataresource+json";

        public static TabularJsonString ToTabularData(this IDataView dataview)
        {
            var schema = new TabularDataSchema();
            foreach (var column in dataview.Schema)
            {
                schema.fields.Add(new TabularDataSchemaField(column.Name, column.Type.RawType.ToTableFieldType()));
            }

            var data = new JArray();

            var cursor = dataview.GetRowCursor(dataview.Schema);

            while (cursor.MoveNext())
            {
                var rowObj = new JObject();

                foreach (var column in dataview.Schema)
                {
                    var type = column.Type.RawType;
                    var getGetterMethod = cursor.GetType()
                                                .GetMethod(nameof(cursor.GetGetter))
                                       .MakeGenericMethod(type);

                    var valueGetter = getGetterMethod.Invoke(cursor, new object[] { column });

                    object value = GetValue((dynamic)valueGetter);

                    if (value is ReadOnlyMemory<char>)
                    {
                        value = value.ToString();
                    }

                    var fromObject = JToken.FromObject(value);

                    rowObj.Add(column.Name, fromObject);
                }

                data.Add(rowObj);
            }

            return new TabularDataSet(schema, data).ToJson();
        }

        private static T GetValue<T>(ValueGetter<T> valueGetter)
        {
            T value = default;
            valueGetter(ref value);
            return value;
        }

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

        public static void Explore(this IDataView source)
        {
            KernelInvocationContext.Current.Display(source.ToTabularData(), HtmlFormatter.MimeType);
        }
        
        public static void Explore(this IEnumerable source)
        {
            KernelInvocationContext.Current.Display(
                source.ToTabularData(), 
                HtmlFormatter.MimeType);
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

                        var tuples = valueTuples.ToArray();

                        EnsureFieldsAreInitializedFromValueTuples(tuples);

                        var o = new JObject();
                        foreach (var tuple in tuples)
                        {
                            o.Add(tuple.name, JToken.FromObject(tuple.value ?? "NULL"));
                        }

                        data.Add(o);
                        break;

                    case IEnumerable<KeyValuePair<string, object>> keyValuePairs:

                        var pairs = keyValuePairs.ToArray();

                        EnsureFieldsAreInitializedFromKeyValuePairs(pairs);

                        var obj = new JObject();
                        foreach (var pair in pairs)
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
                { } t when t == typeof(DateTime) => "datetime",
                { } t when t == typeof(int) => "integer",
                { } t when t == typeof(UInt16) => "integer",
                { } t when t == typeof(UInt32) => "integer",
                { } t when t == typeof(UInt64) => "integer",
                { } t when t == typeof(long) => "integer",
                { } t when t == typeof(Single) => "number",
                { } t when t == typeof(float) => "number",
                { } t when t == typeof(double) => "number",
                { } t when t == typeof(decimal) => "number",
                { } t when t == typeof(string) => "string",
                { } t when t == typeof(ReadOnlyMemory<char>) => "string",
                _ => "any",
            };

        internal class TabularDataSet
        {
            public TabularDataSet(TabularDataSchema schema, JArray data)
            {
                Schema = schema;
                Data = data;
            }

            public TabularDataSchema Schema { get; }

            public JArray Data { get; }

            public TabularJsonString ToJson()
            {
                var schema = JObject.FromObject(Schema);

                var tabularData = new JObject
                {
                    ["schema"] = schema,
                    ["data"] = Data
                };

                return new TabularJsonString(tabularData.ToString(Newtonsoft.Json.Formatting.Indented));
            }
        }

        internal class TabularDataSchema
        {
            public List<string> primaryKey { get; } = new List<string>();

            public TabularDataFieldList fields { get; } = new TabularDataFieldList();
        }

        internal class TabularDataSchemaField
        {
            public TabularDataSchemaField(string name, string type)
            {
                this.name = name;
                this.type = type;
            }

            public string name { get; }
            public string type { get; }
        }

        internal class TabularDataFieldList : List<TabularDataSchemaField>
        {
        }
    }
}