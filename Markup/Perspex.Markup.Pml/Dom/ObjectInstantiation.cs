// -----------------------------------------------------------------------
// <copyright file="ObjectInstantiation.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    using System.Collections.Generic;

    public class ObjectInstantiation : Node
    {
        public Identifier Type { get; set; }

        public IEnumerable<Node> Children { get; set; }
    }
}