// -----------------------------------------------------------------------
// <copyright file="PmlRunnerTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.UnitTests.Runtime
{
    using Perspex.Markup.Pml.Parsers;
    using Perspex.Markup.Pml.Runtime;
    using Xunit;

    public class PmlRunnerTests
    {
        [Fact]
        public void Property_Title_Should_Be_Set()
        {
            var markup = "TestTarget { Title = \"Hello World\" }";
            var document = PmlParser.ParseMarkup(markup);
            var runner = new PmlRunner();
            var target = new TestTarget();

            runner.Run(document, target);

            Assert.Equal("Hello World", target.Title);
        }

        private class TestTarget
        {
            public string Title { get; set; }
        }
    }
}
