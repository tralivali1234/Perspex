// -----------------------------------------------------------------------
// <copyright file="PmlParser.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Parsers
{
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.Markup.Pml.Dom;
    using Sprache;

    public class PmlParser
    {
        public static readonly Parser<PropertyValue> InstantiationPropertyValue =
            from instantiation in ObjectInstantiation()
            select new ObjectInstantiationValue(instantiation);

        public static readonly Parser<PropertyValue> ExpressionPropertyValue =
            from expression in ExpressionParser.Expression()
            select new ExpressionValue(expression);

        public static readonly Parser<PropertyValue> PropertyValue =
            InstantiationPropertyValue.Or(ExpressionPropertyValue);

        public static readonly Parser<BindingMode> EqualsBindingMode =
            Parse.Char('=').Return(BindingMode.None)
            .Or(Parse.String("<=>").Return(BindingMode.TwoWay))
            .Or(Parse.String("<=").Return(BindingMode.OneWay));

        public static readonly Parser<Node> PropertySetter =
            from name in IdentifierParser.NamespacedIdentifier.Token()
            from eq in EqualsBindingMode.Token()
            from value in PropertyValue.Token()
            from end in Parse.String(";").Text().Or(Parse.LineEnd).Optional()
            select new PropertySetter { Name = name, Value = value, BindingMode = eq };

        public static Parser<ObjectInstantiation> ObjectInstantiation()
        {
            return from name in IdentifierParser.NamespacedIdentifier.Token()
                   from leftBrace in Parse.Char('{').Token()
                   from children in Children
                   from rightBrace in Parse.Char('}').Token()
                   select new ObjectInstantiation { Type = name, Children = children };
        }

        public static readonly Parser<IEnumerable<Node>> Children =
            from children in ObjectInstantiation().Or(PropertySetter).Many()
            select children;

        public static readonly Parser<Document> Document =
            from leading in Parse.WhiteSpace.Many()
            from doc in ObjectInstantiation().Select(n => new Document { RootNode = (ObjectInstantiation)n }).End()
            select doc;

        public static Document ParseMarkup(string markup)
        {
            return Document.Parse(markup);
        }
    }
}
