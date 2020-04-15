using System;
using System.Collections.Generic;
using System.Text;

namespace FSHer.Bbl.FSH
{
    /// <summary>
    /// FSH Entity
    /// </summary>
    public abstract class Entity : Item, IEntity
    {
        public List<SDRule> Rules { get; } = new List<SDRule>();

    }
}
