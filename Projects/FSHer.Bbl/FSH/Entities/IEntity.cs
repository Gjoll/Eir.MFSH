using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    /// <summary>
    /// FSH Entity
    /// </summary>
    public interface IEntity
    {
        List<SDRule> Rules { get; }
    }

    /// <summary>
    /// FSH Entity
    /// </summary>
    public interface TIEntity<T> : IEntity
    {
    }
}
