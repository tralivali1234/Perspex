// -----------------------------------------------------------------------
// <copyright file="PmlCompilerTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.UnitTests.Compiler
{
    using Microsoft.CodeAnalysis;
    using Perspex.Markup.Pml.Compiler;
    using Perspex.Markup.Pml.Parsers;
    using Xunit;

    public class PmlCompilerTests
    {
        [Fact]
        public void Simple_Class_Declaration_With_Property_Returns_Correct_Code()
        {
            var markup = "Namespace.TestTarget { Title = \"Hello World\" }";
            var document = PmlParser.ParseMarkup(markup);
            var compiler = new PmlCompiler();

            var result = compiler.Compile(document).NormalizeWhitespace().ToString();

            var expected = @"namespace Namespace
{
    public class TestTarget : Window
    {
        void InitializeComponent()
        {
            this.Title = ""Hello World"";
        }
    }
}";

            Assert.Equal(expected, result);
        }
    }
}
