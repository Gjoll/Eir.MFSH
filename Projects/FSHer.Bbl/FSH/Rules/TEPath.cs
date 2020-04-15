using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    public interface IPath
    {
        IEntity Container { get; set; }
        String Value { get; set; }
    }

    public class EPath : IPath
    {
        public IEntity Container { get; set; }
        public String Value { get; set; }
    }

    public class TEPath<T> : EPath
    {
    }

    public static class PathExtensions
    {
        public static TEPath<T> Path<T>(this Entity container, String value)
            where T : new()
        {
            return new TEPath<T>
            {
                Container = container,
                Value = value
            };
        }
    }
}
