using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public enum Strength
    {
        None,
        Example,
        Preferred,
        Extensible,
        Required
    };

    public static class StrengthHelper
    {
        public static String ToFsh(this Strength strength)
        {
            switch (strength)
            {
                case Strength.Example:
                    return "example";
                case Strength.Extensible:
                    return "extensible";
                case Strength.Preferred:
                    return "preferred";
                case Strength.Required:
                    return "required";
                case Strength.None:
                    return "";
                default: throw new NotImplementedException();
            }
        }
    }
}
