// -----------------------------------------------------------------------
// <copyright file="IdentifierParserTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.UnitTests
{
    using Perspex.Markup.Pml.Parsers;
    using Sprache;
    using Xunit;

    public class IdentifierParserTests
    {
        [Fact]
        public void Identifier_Can_Start_With_Underscore()
        {
            var result = IdentifierParser.Identifier.Parse("_Foo");

            Assert.Equal("_Foo", result.Name);
        }

        [Fact]
        public void Identifier_Cant_Start_With_Number()
        {
            Assert.Throws<ParseException>(() => IdentifierParser.Identifier.Parse("1Foo"));
        }
    }
}
