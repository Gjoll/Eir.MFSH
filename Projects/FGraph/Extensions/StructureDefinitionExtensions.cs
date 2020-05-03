using System;
using System.Collections.Generic;
using System.Text;
using Eir.DevTools;
using Hl7.Fhir.Model;

namespace FGraph
{
    static class StructureDefinitionExtensions
    {
        public static ElementDefinition FindElement(this IEnumerable<ElementDefinition> elements, String id)
        {
            foreach (ElementDefinition e in elements)
            {
                if (e.ElementId == id)
                    return e;
            }
            return null;
        }


        public static ElementDefinition FindSnapElement(this StructureDefinition sd, String id)
        {
            String baseName = sd.BaseDefinition.LastUriPart();
            Int32 index = id.IndexOf('.');
            String normName;
            if (index > 0)
                normName = baseName + id.Substring(index);
            else
                normName = baseName;
            return sd.Snapshot.Element.FindElement(normName);
        }

        public static ElementDefinition FindDiffElement(this StructureDefinition sd, String id)
        {
            String baseName = sd.BaseDefinition.LastUriPart();
            Int32 index = id.IndexOf('.');
            String normName;
            if (index > 0)
                normName = baseName + id.Substring(index);
            else
                normName = baseName;
            return sd.Differential.Element.FindElement(normName);
        }
    }
}
