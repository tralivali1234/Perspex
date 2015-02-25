// -----------------------------------------------------------------------
// <copyright file="Expressionvalue.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Dom
{
    public class ObjectInstantiationValue : PropertyValue
    {
        public ObjectInstantiationValue(ObjectInstantiation instantiation)
        {
            this.Instantiation = instantiation;
        }

        public ObjectInstantiation Instantiation { get; }
    }
}