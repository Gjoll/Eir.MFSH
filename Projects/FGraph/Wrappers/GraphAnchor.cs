using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FGraph
{
    public class GraphAnchor : IEquatable<GraphAnchor>
    {
        /// <summary>
        /// Url of item.
        /// </summary>
        public String Url { get; }

        /// <summary>
        /// Optional path to sub item.
        /// if empty, referes to whole item with url.
        /// i.e. if url is of a profile, this may be the element id of the
        /// element this anchor refers to.
        /// </summary>
        public String Item { get; }

        public GraphAnchor(String url, String item = "")
        {
            this.Url = url;
            this.Item = item;
        }

        public GraphAnchor(JToken data)
        {
            this.Url = data.RequiredValue("url");
            this.Item= data.OptionalValue("id");
        }

        public bool Equals(GraphAnchor other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.GetType() != other.GetType())
                return false;
            if (this.Url != other.Url) return false;
            if (this.Item != other.Item) return false;
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(GraphAnchor)) return false;
            return Equals((GraphAnchor)obj);
        }

        /// <summary>
        /// This must be overridden in all child classes.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => throw new Exception("Must be overridden in chils class(s)");
    }
}
