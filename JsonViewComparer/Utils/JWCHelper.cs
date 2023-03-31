using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using JsonViewComparer.Entities;
using JsonViewComparer.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace JsonViewComparer.Utils;

internal static class JWCHelper
{
    private const int MinLength = 5;

    public static JWCBase GenerateProperties(string firstJson, HashSet<string> noSplitKeys)
    {
        dynamic expandoObject = JsonConvert.DeserializeObject<ExpandoObject>(firstJson, new ExpandoObjectConverter());
        var properties = new JWCObject(null);
        IDictionary<string, object> dictionary = expandoObject;

        foreach (var keyValuePair in dictionary)
        {
            properties.Items.Add(Create(keyValuePair.Key, keyValuePair.Value, noSplitKeys));
        }

        return properties;
    }

    private static JWCBase Create(string key, object value, HashSet<string> noSplitKeys)
    {
        if (value is List<object> enumerable)
        {
            var itemType = enumerable.FirstOrDefault()?.GetType();

            if (noSplitKeys.Contains(key))
            {
                string convertedValue = null;

                if (enumerable.Count != 0)
                {
                    convertedValue = $"[{string.Join(", ", enumerable.Select(x => x))}]";
                }

                return new JWCProperty(key, convertedValue);
            }

            if (itemType != null && itemType.IsValueType)
            {
                var simpleArray = new JWCSimpleArray(key);

                simpleArray.Values.AddRange(enumerable);

                return simpleArray;
            }

            var objectArray = new JWCObjectArray(key);
            int index = 0;

            foreach (var item in enumerable)
            {
                string indexPath = $"{index++}";
                var buildObject = Create(indexPath, item, noSplitKeys) as JWCObject;// ?? new JWCObject(indexPath);

                objectArray.Items.Add(buildObject);
            }

            return objectArray;
        }

        if (value is ExpandoObject expandoObject)
        {
            var jwcObject = new JWCObject(key);
            IDictionary<string, object> dictionary = expandoObject;

            foreach (var keyValuePair in dictionary)
            {
                jwcObject.Items.Add(Create(keyValuePair.Key, keyValuePair.Value, noSplitKeys));
            }

            return jwcObject;
        }

        //Check is Value property is "Inserted json"
        string data = value?.ToString();

        if (!string.IsNullOrEmpty(data) && data.Length >= MinLength && data.TrimStart().StartsWith("{"))
        {
            dynamic innerExpandoObject = JsonConvert.DeserializeObject<ExpandoObject>(value.ToString(), new ExpandoObjectConverter());
            IDictionary<string, object> dictionary = innerExpandoObject;
            var inner = new JWCObject(key);

            foreach (var keyValuePair in dictionary)
            {
                inner.Items.Add(Create(keyValuePair.Key, keyValuePair.Value, noSplitKeys));
            }

            return inner;
        }

        return new JWCProperty(key, value);
    }

    public static void CompareProperties(JWCBase firstProperty, JWCBase secondProperty, bool inverseState)
    {
        if (firstProperty != null)
        {
            firstProperty.SetState(JWCState.Unchanged);
        }

        if (firstProperty == null || secondProperty == null)
        {
            return;
        }

        if (firstProperty.Key != secondProperty.Key)
        {
            firstProperty.SetState(JWCState.Modified);

            return;
        }

        if (firstProperty.GetType() != secondProperty.GetType())
        {
            firstProperty.SetState(JWCState.Modified);

            return;
        }

        if (firstProperty is JWCProperty fProperty && secondProperty is JWCProperty sProperty)
        {
            if (fProperty.Value?.ToString() == sProperty.Value?.ToString())
            {
                fProperty.SetState(JWCState.Unchanged);
            }
            else
            {
                fProperty.SetState(JWCState.Modified);
            }
        }

        if (firstProperty is JWCSimpleArray fSimpleArray && secondProperty is JWCSimpleArray sSimpleArray)
        {
            foreach (var item in fSimpleArray.Values)
            {
                if (!sSimpleArray.Values.Exists(x => x.Equals(item)))
                {
                    if (inverseState)
                    {
                        firstProperty.SetState(JWCState.Deleted);
                    }
                    else
                    {
                        firstProperty.SetState(JWCState.Added);
                    }
                }
            }
        }

        if (firstProperty is JWCObject fObject && secondProperty is JWCObject sObject)
        {
            foreach (var fItem in fObject.Items)
            {
                var sItem = sObject.Items.FirstOrDefault(x => x.Key == fItem.Key);

                if (sItem == null)
                {
                    if (inverseState)
                    {
                        fItem.SetState(JWCState.Deleted);
                    }
                    else
                    {
                        fItem.SetState(JWCState.Added);
                    }

                    continue;
                }

                CompareProperties(fItem, sItem, inverseState);
            }
        }

        if (firstProperty is JWCObjectArray fObjectArray && secondProperty is JWCObjectArray sObjectArray)
        {
            foreach (var fObjectItem in fObjectArray.Items)
            {
                if (fObjectItem == null)
                {
                    continue;
                }

                var sObjectItem = sObjectArray.Items.FirstOrDefault(x => x != null && x.Key == fObjectItem.Key);

                if (sObjectItem == null)
                {
                    if (inverseState)
                    {
                        fObjectItem.SetState(JWCState.Deleted);
                    }
                    else
                    {
                        fObjectItem.SetState(JWCState.Added);
                    }

                    continue;
                }

                CompareProperties(fObjectItem, sObjectItem, inverseState);
            }
        }
    }
}