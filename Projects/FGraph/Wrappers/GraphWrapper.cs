using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FGraph
{
    public class GraphWrapper
    {
        protected FGrapher fGraph;

        public GraphWrapper(FGrapher fGraph)
        {
            this.fGraph = fGraph;
        }

        public Color RequiredColorValue(JToken value, String name)
        {
            return Color.FromName(RequiredValue(value, name));
        }

        public Color OptionalColorValue(JToken value, String name)
        {
            if (value?.Value<String>() == null)
                return Color.White;
            return Color.FromName(RequiredValue(value, name));
        }

        public String RequiredValue(JToken value, String name)
        {
            String retVal = OptionalValue(value, name);
            if (String.IsNullOrEmpty(retVal))
                throw new Exception($"Required value for '{name} is missing");
            return retVal;
        }

        public String OptionalValue(JToken value, String name)
        {
            return value?[name]?.Value<String>();
        }

        public Int32 OptionalIntValue(JToken value, String name, Int32 defaultValue)
        {
            Int32? retVal = value?[name]?.Value<Int32>();
            if (retVal.HasValue)
                return retVal.Value;
            return defaultValue;
        }

        public Boolean OptionalBoolValue(JToken value, String name, bool defaultValue = false)
        {
            bool? retVal = value?[name]?.Value<Boolean>();
            if (retVal.HasValue)
                return retVal.Value;
            return defaultValue;
        }
    }
}
