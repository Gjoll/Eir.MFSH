using System;
using System.Collections.Generic;
using System.Text;
using Hl7.Fhir.Model;

namespace FGraph
{
    static class StructureDefinitionExtensions
    {
        public static ElementDefinition FindElement(this StructureDefinition sd, String id)
        {
            String baseName = sd.Snapshot.Element[0].Path;
            Int32 index = id.IndexOf('.');
            String normName;
            if (index > 0)
                normName = baseName + id.Substring(index);
            else
                normName = baseName;
            foreach (ElementDefinition e in sd.Snapshot.Element)
            {
                if (e.ElementId == normName)
                    return e;
            }
            return null;
        }
    }
}
