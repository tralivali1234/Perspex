// -----------------------------------------------------------------------
// <copyright file="VisualTests.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.SceneGraph.UnitTests
{
    using System.Linq;
    using Perspex.VisualTree;
    using Xunit;

    public class VisualTests
    {
        [Fact]
        public void Added_Child_Should_Have_VisualParent_Set()
        {
            var target = new TestVisual();
            var child = new Visual();

            target.AddChild(child);

            Assert.Equal(target, child.GetVisualParent());
        }

        [Fact]
        public void Added_Child_Should_Have_InheritanceParent_Set()
        {
            var target = new TestVisual();
            var child = new TestVisual();

            target.AddChild(child);

            Assert.Equal(target, child.InheritanceParent);
        }

        [Fact]
        public void Removed_Child_Should_Have_VisualParent_Cleared()
        {
            var target = new TestVisual();
            var child = new Visual();

            target.AddChild(child);
            target.RemoveChild(child);

            Assert.Null(child.GetVisualParent());
        }

        [Fact]
        public void Removed_Child_Should_Have_InheritanceParent_Cleared()
        {
            var target = new TestVisual();
            var child = new TestVisual();

            target.AddChild(child);
            target.RemoveChild(child);

            Assert.Null(child.InheritanceParent);
        }

        [Fact]
        public void Clearing_Children_Should_Clear_VisualParent()
        {
            var children = new[] { new Visual(), new Visual() };
            var target = new TestVisual();

            target.AddChildren(children);
            target.ClearChildren();

            var result = children.Select(x => x.GetVisualParent()).ToList();

            Assert.Equal(new Visual[] { null, null }, result);
        }

        [Fact]
        public void AttachedToVisualTree_Should_Be_Called()
        {
            var target = new TestRoot();
            var child = new TestVisual();
            var called = false;

            child.AttachedToVisualTreeCalled += (s, e) => called = true;
            target.AddChild(child);

            Assert.True(called);
        }

        [Fact]
        public void DetachedFromVisualTree_Should_Be_Called()
        {
            var target = new TestRoot();
            var child = new TestVisual();
            var called = false;

            target.AddChild(child);
            child.DetachedFromVisualTreeCalled += (s, e) => called = true;
            target.ClearChildren();

            Assert.True(called);
        }

        [Fact]
        public void Added_Child_Should_Have_IsAttachedToVisualTree_True()
        {
            var target = new TestRoot();
            var child = new TestVisual();

            target.AddChild(child);

            Assert.True(((IVisual)child).IsAttachedToVisualTree);
        }

        [Fact]
        public void Removed_Child_Should_Have_IsAttachedToVisualTree_False()
        {
            var target = new TestRoot();
            var child = new TestVisual();

            target.AddChild(child);
            target.RemoveChild(child);

            Assert.False(((IVisual)child).IsAttachedToVisualTree);
        }
    }
}
