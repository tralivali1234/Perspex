// -----------------------------------------------------------------------
// <copyright file="Identifier.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    using System.Collections.Generic;
    using System.Linq;

    public class Identifier
    {
        public Identifier(string name)
        {
            this.Name = name;
        }

        public Identifier(IEnumerable<Identifier> parts)
        {
            this.Name = string.Join(".", parts.Select(x => x.Name));
        }

        public string Name { get; set; }
    }
}
