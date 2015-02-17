// -----------------------------------------------------------------------
// <copyright file="ExpressionParserTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------
namespace Perspex.Markup.Pml.UnitTests
{
    using System.Linq;
    using Perspex.Markup.Pml.Dom;
    using Perspex.Markup.Pml.Parsers;
    using Sprache;
    using Xunit;

    public class PmlParserTests
    {
        [Fact]
        public void Empty_File_Should_Throw_Exception()
        {
            Assert.Throws<ParseException>(() => PmlParser.ParseMarkup(""));
        }

        [Fact]
        public void Root_Node_Should_Have_Correct_Name()
        {
            var result = PmlParser.ParseMarkup("Root { }");

            Assert.Equal("Root", result.RootNode.Type.Name);
        }

        [Fact]
        public void Child_Nodes_Should_Have_Correct_Names()
        {
            var result = PmlParser.ParseMarkup("Root { Child1 { } Child2 { } }");
            
            Assert.Equal(new[] { "Child1", "Child2" }, result.RootNode.Children.OfType<ObjectInstantiation>().Select(x => x.Type.Name));
        }

        [Fact]
        public void Property_Names_Should_Be_Parsed()
        {
            var result = PmlParser.ParseMarkup(@"
                Root {
                    Property1: 1
                    Foo.Property2: 2
                }");

            Assert.Equal(new[] { "Property1", "Foo.Property2" }, result.RootNode.Children.OfType<PropertySetter>().Select(x => x.Name.Name));
        }

        [Fact]
        public void Boolean_Literal_Property_Value_Should_Be_Parsed()
        {
            var result = PmlParser.ParseMarkup("Root { Property1: true }");
            var propertySet = result.RootNode.Children.First() as PropertySetter;
            var literal = propertySet.Value as Literal;

            Assert.NotNull(propertySet);
            Assert.NotNull(literal);
            Assert.Equal(true, literal.Value);
        }

        [Fact]
        public void Integer_Literal_Property_Value_Should_Be_Parsed()
        {
            var result = PmlParser.ParseMarkup("Root { Property1: 42 }");
            var propertySet = result.RootNode.Children.First() as PropertySetter;
            var literal = propertySet.Value as Literal;

            Assert.NotNull(propertySet);
            Assert.NotNull(literal);
            Assert.Equal(42.0, literal.Value);
        }

        [Fact]
        public void Real_Literal_Property_Value_Should_Be_Parsed()
        {
            var result = PmlParser.ParseMarkup("Root { Property1: 42.12 }");
            var propertySet = result.RootNode.Children.First() as PropertySetter;
            var literal = propertySet.Value as Literal;

            Assert.NotNull(propertySet);
            Assert.NotNull(literal);
            Assert.Equal(42.12, literal.Value);
        }

        [Fact]
        public void String_Literal_Property_Value_Should_Be_Parsed()
        {
            var result = PmlParser.ParseMarkup("Root { Property1: \"hello\" }");
            var propertySet = result.RootNode.Children.First() as PropertySetter;
            var literal = propertySet.Value as Literal;

            Assert.NotNull(propertySet);
            Assert.NotNull(literal);
            Assert.Equal("hello", literal.Value);
        }

        [Fact]
        public void Property_Setters_Can_Be_Separated_With_Semicolon()
        {
            var result = PmlParser.ParseMarkup("Root { Property1: 42; Property2: 24 }");

            Assert.Equal(2, result.RootNode.Children.Count());
            Assert.IsType<PropertySetter>(result.RootNode.Children.ElementAt(0));
            Assert.IsType<PropertySetter>(result.RootNode.Children.ElementAt(1));
        }

        [Fact]
        public void Line_Not_ObjectDeclaration_Or_Property_Setter_Should_Throw_Exception()
        {
            Assert.Throws<ParseException>(() => PmlParser.ParseMarkup("Root { Invalid }"));
        }
    }
}
