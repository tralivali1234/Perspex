// -----------------------------------------------------------------------
// <copyright file="ControlTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.UnitTests
{
    using Perspex.Controls.Core;
    using Xunit;

    public class ControlTests
    {
        [Fact]
        public void Controls_Should_Register_With_NameScope()
        {
            var scope = new NameScope
            {
                Child = new Decorator
                {
                    Name = "Decorator",
                    Child = new Border
                    {
                        Child = new TextBlock
                        {
                            Name = "TextBlock",
                        }
                    }
                }
            };

            Assert.IsType<Decorator>(((INameScope)scope).FindName("Decorator"));
            Assert.IsType<TextBlock>(((INameScope)scope).FindName("TextBlock"));
        }

        [Fact]
        public void Controls_Should_Unregister_With_NameScope()
        {
            var scope = new NameScope
            {
                Child = new Decorator
                {
                    Name = "Decorator",
                    Child = new Border
                    {
                        Child = new TextBlock
                        {
                            Name = "TextBlock",
                        }
                    }
                }
            };

            var decorator = ((INameScope)scope).FindName("Decorator") as Decorator;
            decorator.Child = null;
            Assert.Null(((INameScope)scope).FindName("TextBlock"));

            scope.Child = null;
            Assert.Null(((INameScope)scope).FindName("Decorator"));
        }
    }
}
