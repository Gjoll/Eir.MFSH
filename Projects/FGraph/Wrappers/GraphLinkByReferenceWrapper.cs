using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    class GraphLinkByReferenceWrapper : GraphLinkWrapper
    {
        /// <summary>
        /// Regex name of sources.
        /// </summary>
        public String Source { get; set; }

        /// <summary>
        /// ElementId of element that has references to other
        /// profiles or elements that we should link to.
        /// Each source item must have an element of this element id.
        /// If elementId starts with a '.', then is is a relative elemenent id, with'
        /// its base value the source records ElementId.
        /// i.e. if the source record has an element id of 'ProfileName.alpha.beta' and
        /// elementId is ".delta' then the full element id is 'ProfileName.alpha.beta.delta'
        /// </summary>
        public String ElementId { get; set; }

        public GraphLinkByReferenceWrapper(FGrapher fGraph, JToken data) : base(fGraph, data)
        {
            this.Source = this.RequiredValue(data, "source");
            this.ElementId = this.OptionalValue(data, "elementId");
        }
    }
}
