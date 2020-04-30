using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace FGraph
{
    public class GraphWrapper
    {
        public String GraphName { get; set; }

        public GraphWrapper(JToken data)
        {
            this.GraphName = this.RequiredValue("GraphLinkByName.graphName", data["graphName"]);
        }

        public Color RequiredColorValue(String name, JToken value)
        {
            return Color.FromName(RequiredValue(name, value));
        }

        public Color OptionalColorValue(String name, JToken value)
        {
            if (value?.Value<String>() == null)
                return Color.White;
            return Color.FromName(RequiredValue(name, value));
        }

        public String RequiredValue(String name, JToken value)
        {
            if (String.IsNullOrWhiteSpace(value?.Value<String>()))
                throw new Exception($"Required value for '{name} is missing");
            return value.Value<String>();
        }

        public String OptionalValue(String name, JToken value)
        {
            return value?.Value<String>();
        }

    }
}
