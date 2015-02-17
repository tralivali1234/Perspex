// -----------------------------------------------------------------------
// <copyright file="PropertySetter.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Perspex.Markup.Pml.Dom
{
    public class PropertySetter : Node
    {
        public Identifier Name { get; set; }

        public Expression Value { get; set; }
    }
}