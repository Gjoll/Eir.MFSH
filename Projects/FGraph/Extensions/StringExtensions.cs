using System;
using System.Collections.Generic;
using System.Text;

namespace FGraph
{
    static public class FGStringExtensions
    {
        public static String FirstSlashPart(this string s)
        {
            String[] parts = s.Split('/');
            return parts[0];
        }

        /// <summary>
        /// Get the base (non resource) part of a url.
        /// i.e.
        /// "http://hl7.org/fhir/us/breast-radiology/StructureDefinition/BreastRadiologyComposition"
        /// will return
        /// "http://hl7.org/fhir/us/breast-radiology"
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String FhirBaseUrl(this string s)
        {
            s = s.Substring(0, s.LastIndexOf('/'));
            s = s.Substring(0, s.LastIndexOf('/'));
            return s;
        }
    }
}
