using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FGraph
{
    public static class JTokenExtensions
    {
        public static Color RequiredColorValue(this JToken value, String name)
        {
            return Color.FromName(RequiredValue(value, name));
        }

        public static Color OptionalColorValue(this JToken value, String name)
        {
            if (value?.Value<String>() == null)
                return Color.White;
            return Color.FromName(RequiredValue(value, name));
        }

        public static String RequiredValue(this JToken value, String name)
        {
            String retVal = OptionalValue(value, name);
            if (String.IsNullOrEmpty(retVal))
                throw new Exception($"Required value for '{name} is missing");
            return retVal;
        }

        public static String OptionalValue(this JToken value, String name)
        {
            return value?[name]?.Value<String>();
        }

        public static Int32 OptionalIntValue(this JToken value, String name, Int32 defaultValue)
        {
            Int32? retVal = value?[name]?.Value<Int32>();
            if (retVal.HasValue)
                return retVal.Value;
            return defaultValue;
        }

        public static Boolean OptionalBoolValue(this JToken value, String name, bool defaultValue = false)
        {
            bool? retVal = value?[name]?.Value<Boolean>();
            if (retVal.HasValue)
                return retVal.Value;
            return defaultValue;
        }
    }
}
