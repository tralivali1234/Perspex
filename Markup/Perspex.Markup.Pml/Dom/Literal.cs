// -----------------------------------------------------------------------
// <copyright file="Literal.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    public class Literal : Expression
    {
        public Literal(object value)
        {
            this.Value = value;
        }

        public object Value { get; set; }
    }
}
