using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    public static class JsonExtensions
    {
        public static IEnumerable<String> GetStrings(this JToken t)
        {
            switch (t)
            {
                case JArray jArray:
                    foreach (JToken item in jArray)
                        yield return item.Value<String>();
                    break;
                case JToken jToken:
                    yield return jToken.Value<String>();
                    break;
                default: 
                    throw new Exception($"Unexpected string array type {t.GetType()}");
            }
        }

        public static IEnumerable<Tuple<String, String>> GetTuples(this JToken t)
        {
            switch (t)
            {
                case JObject jObject:
                    foreach (KeyValuePair<String, JToken> option in jObject)
                        yield return new Tuple<string, string>(option.Key, option.Value.Value<String>());
                        break;

                default: 
                    throw new Exception($"Unexpected string array type {t.GetType()}");
            }
        }
    }
}
