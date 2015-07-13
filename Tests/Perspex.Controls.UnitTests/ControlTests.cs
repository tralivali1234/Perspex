// -----------------------------------------------------------------------
// <copyright file="ControlTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Perspex.Controls.Core;
    using Xunit;
    using Perspex.Rendering;
    using Perspex.Platform;

    public class ControlTests
    {
        [Fact]
        public void Named_Control_Should_Register_With_NameScope()
        {
            Control target;

            var scope = new NameScope
            {
                Children = new Controls
                {
                    (target = new Control
                    {
                        Name = "Foo"
                    })
                }
            };

            Assert.Equal(
                new[] { new KeyValuePair<string, INamed>("Foo", target) },
                scope.Names);
        }

        [Fact]
        public void Named_Control_Should_Unregister_With_NameScope()
        {
            Control target;

            var scope = new NameScope
            {
                Children = new Controls
                {
                    (target = new Control
                    {
                        Name = "Foo"
                    })
                }
            };

            scope.Children.Remove(target);

            Assert.Empty(scope.Names);
        }

        private class NameScope : Panel, INameScope, IRenderRoot
        {
            public NameScope()
            {
                this.Names = new NameDictionary();
            }

            public NameDictionary Names { get; }

            public IRenderer Renderer { get; }

            public IRenderManager RenderManager { get; }

            public Point TranslatePointToScreen(Point p)
            {
                throw new NotImplementedException();
            }
        }
    }
}
