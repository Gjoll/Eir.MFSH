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
            if (index < 0)
                throw new Exception("Invalid id '{id}'");
            String normName = baseName + id.Substring(index);
            foreach (ElementDefinition e in sd.Snapshot.Element)
            {
                if (e.ElementId == normName)
                    return e;
            }
            return null;
        }
    }
}
