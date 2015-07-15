// -----------------------------------------------------------------------
// <copyright file="PagesTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.VisualTree;
    using Xunit;

    public class PagesTests
    {
        [Fact]
        public void Selection_Should_Initially_Be_None()
        {
            var target = new Pages
            {
                Children = new Controls
                {
                    new TextBlock { Text = "Foo" },
                    new TextBlock { Text = "Bar" },
                }
            };

            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(100, 100));

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
            Assert.Equal(new[] { false, false }, GetChildVisibility(target));
        }

        [Fact]
        public void Setting_SelectedIndex_Should_Show_Page()
        {
            var target = new Pages
            {
                Children = new Controls
                {
                    new TextBlock { Text = "Foo" },
                    new TextBlock { Text = "Bar" },
                }
            };

            target.SelectedIndex = 1;
            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(100, 100));

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetChildVisibility(target));
        }

        [Fact]
        public void Setting_SelectedItem_Should_Show_Page()
        {
            var target = new Pages
            {
                Children = new Controls
                {
                    new TextBlock { Text = "Foo" },
                    new TextBlock { Text = "Bar" },
                }
            };

            target.SelectedItem = target.Children[1];
            target.Measure(new Size(100, 100));
            target.Arrange(new Rect(100, 100));

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetChildVisibility(target));
        }

        private static IEnumerable<bool> GetChildVisibility(Pages target)
        {
            return target.GetVisualChildren().Select(x => x.IsVisible).ToList();
        }
    }
}
