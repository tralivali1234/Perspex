// -----------------------------------------------------------------------
// <copyright file="ContentHostTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System.Linq;
    using Perspex.LogicalTree;
    using Perspex.VisualTree;
    using Xunit;

    public class ContentHostTests
    {
        [Fact]
        public void Setting_Content_Should_Create_Child()
        {
            var target = new ContentHost
            {
                Content = "Foo"
            };

            Assert.IsType<TextBlock>(target.Child);
            Assert.Equal(1, target.GetVisualChildren().Count());
            Assert.IsType<TextBlock>(target.GetVisualChildren().Single());
            Assert.Equal(1, target.GetLogicalChildren().Count());
            Assert.IsType<TextBlock>(target.GetLogicalChildren().Single());
        }

        [Fact]
        public void Clearing_Content_Should_Remove_Child()
        {
            var target = new ContentHost
            {
                Content = "Foo"
            };

            target.Content = null;

            Assert.Null(target.Child);
            Assert.Empty(target.GetVisualChildren());
            Assert.Empty(target.GetLogicalChildren());
        }

        [Fact]
        public void Setting_Content_Should_Set_Child_Controls_Parent()
        {
            var target = new ContentHost
            {
                Content = "Foo"
            };

            var child = target.GetVisualChildren().Single() as TextBlock;
            Assert.Equal(child.Parent, target);
            Assert.Equal(((ILogical)child).LogicalParent, target);
        }

        [Fact]
        public void Clearing_Content_Should_Clear_Child_Controls_Parent()
        {
            var target = new ContentHost();

            target.Content = "Foo";
            var child = target.GetVisualChildren().Single() as TextBlock;
            target.Content = null;

            Assert.Null(child.Parent);
            Assert.Null(((ILogical)child).LogicalParent);
        }
    }
}
