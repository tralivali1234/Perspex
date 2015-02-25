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
            this.Parts = new[] { name };
        }

        public Identifier(IEnumerable<string> parts)
        {
            this.Parts = parts.ToArray();
        }

        public string Name
        {
            get { return this.Parts.Last(); }
        }

        public string FullName
        {
            get { return string.Join(".", this.Parts); }
        }

        public string NamespaceName
        {
            get { return string.Join(".", this.Parts.Take(this.Parts.Length - 1)); }
        }

        public string[] Parts { get; set; }
    }
}
