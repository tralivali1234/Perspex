// -----------------------------------------------------------------------
// <copyright file="PmlCompilerTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.UnitTests.Compiler
{
    using Perspex.Markup.Pml.Compiler;
    using Perspex.Markup.Pml.Parsers;
    using Xunit;

    public class PmlCompilerTests
    {
        [Fact]
        public void Foo()
        {
            var markup = "Namespace.TestTarget { Title = \"Hello World\" }";
            var document = PmlParser.ParseMarkup(markup);
            var compiler = new PmlCompiler();

            compiler.Compile(document);
        }
    }
}
