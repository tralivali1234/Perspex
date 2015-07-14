// -----------------------------------------------------------------------
// <copyright file="PanelTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reactive.Linq;
    using Perspex.Platform;
    using Perspex.Rendering;
    using Xunit;

    public class PanelTests
    {
        [Fact]
        public void Adding_Control_To_Panel_Should_Set_Child_Controls_Parent()
        {
            var panel = new Panel();
            var child = new Control();

            panel.Children.Add(child);

            Assert.Equal(child.Parent, panel);
            Assert.Equal(((ILogical)child).LogicalParent, panel);
        }

        [Fact]
        public void Setting_Controls_Should_Set_Child_Controls_Parent()
        {
            var panel = new Panel();
            var child = new Control();

            panel.Children = new Controls { child };

            Assert.Equal(child.Parent, panel);
            Assert.Equal(((ILogical)child).LogicalParent, panel);
        }

        [Fact]
        public void Removing_Control_From_Panel_Should_Clear_Child_Controls_Parent()
        {
            var panel = new Panel();
            var child = new Control();

            panel.Children.Add(child);
            panel.Children.Remove(child);

            Assert.Null(child.Parent);
            Assert.Null(((ILogical)child).LogicalParent);
        }

        [Fact]
        public void Clearing_Panel_Children_Should_Clear_Child_Controls_Parent()
        {
            var panel = new Panel();
            var child1 = new Control();
            var child2 = new Control();

            panel.Children.Add(child1);
            panel.Children.Add(child2);
            panel.Children.Clear();

            Assert.Null(child1.Parent);
            Assert.Null(((ILogical)child1).LogicalParent);
            Assert.Null(child2.Parent);
            Assert.Null(((ILogical)child2).LogicalParent);
        }

        [Fact]
        public void Resetting_Panel_Children_Should_Clear_Child_Controls_Parent()
        {
            var panel = new Panel();
            var child1 = new Control();
            var child2 = new Control();

            panel.Children.Add(child1);
            panel.Children.Add(child2);
            panel.Children = new Controls();

            Assert.Null(child1.Parent);
            Assert.Null(((ILogical)child1).LogicalParent);
            Assert.Null(child2.Parent);
            Assert.Null(((ILogical)child2).LogicalParent);
        }

        [Fact]
        public void Child_Control_Should_Appear_In_Panel_Children()
        {
            var panel = new Panel();
            var child = new Control();

            panel.Children.Add(child);

            Assert.Equal(new[] { child }, panel.Children);
            Assert.Equal(new[] { child }, ((ILogical)panel).LogicalChildren.ToList());
        }

        [Fact]
        public void Removing_Child_Control_Should_Remove_From_Panel_Children()
        {
            var panel = new Panel();
            var child = new Control();

            panel.Children.Add(child);
            panel.Children.Remove(child);

            Assert.Equal(new Control[0], panel.Children);
            Assert.Equal(new ILogical[0], ((ILogical)panel).LogicalChildren.ToList());
        }

        [Fact]
        public void Childs_Parent_Should_Be_Set_After_OnAttachedToVisualTree_Called()
        {
            var root = new TestRoot();
            var target = new Panel();
            var child = new TestControl();
            var fired = new List<string>();

            child.OnAttachedToVisualTreeFired += (s, e) => fired.Add("OnAttachedToVisualTree");
            child.GetObservable(Control.ParentProperty).Skip(1).Subscribe(_ => fired.Add("ParentChanged"));

            root.Child = target;
            target.Children.Add(child);

            Assert.Equal(
                new[] { "OnAttachedToVisualTree", "ParentChanged" },
                fired);
        }

        [Fact]
        public void Childs_Parent_Should_Be_Cleared_Before_OnDetachedToVisualTree_Called()
        {
            Panel target;
            TestControl child;

            var root = new TestRoot
            {
                Child = target = new Panel
                {
                    Children = new Controls
                    {
                        (child = new TestControl())
                    }
                }
            };

            var fired = new List<string>();

            child.OnDetachedFromVisualTreeFired += (s, e) => fired.Add("OnDetachedFromVisualTree");
            child.GetObservable(Control.ParentProperty).Skip(1).Subscribe(_ => fired.Add("ParentChanged"));
            target.Children.Remove(child);

            Assert.Equal(
                new[] { "ParentChanged", "OnDetachedFromVisualTree" },
                fired);
        }

        private class TestRoot : Decorator, IRenderRoot
        {
            public IRenderer Renderer { get; set; }

            public IRenderManager RenderManager { get; set; }

            public Point TranslatePointToScreen(Point p)
            {
                throw new NotImplementedException();
            }
        }

        private class TestControl : Control
        {
            public event EventHandler OnAttachedToVisualTreeFired;

            public event EventHandler OnDetachedFromVisualTreeFired;

            protected override void OnAttachedToVisualTree(IRenderRoot root)
            {
                base.OnAttachedToVisualTree(root);
                this.OnAttachedToVisualTreeFired?.Invoke(this, EventArgs.Empty);
            }

            protected override void OnDetachedFromVisualTree(IRenderRoot root)
            {
                base.OnDetachedFromVisualTree(root);
                this.OnDetachedFromVisualTreeFired?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
