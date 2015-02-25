// -----------------------------------------------------------------------
// <copyright file="PropertySetter.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    public class PropertySetter : Node
    {
        public Identifier PropertyName { get; set; }

        public PropertyValue Value { get; set; }

        public BindingMode BindingMode { get; set; }
    }
}