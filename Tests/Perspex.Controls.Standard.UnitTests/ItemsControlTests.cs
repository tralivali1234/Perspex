// -----------------------------------------------------------------------
// <copyright file="ItemsControlTests.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.UnitTests
{
    using System.Collections.Specialized;
    using System.Linq;
    using Perspex.Collections;
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard.Presenters;
    using Xunit;

    public class ItemsControlTests
    {
        [Fact]
        public void Presenter_Should_Have_TemplatedParent_Set_To_ItemsControl()
        {
            var target = new ItemsControl();

            target.Template = this.GetTemplate();
            target.Items = new[] { "Foo" };
            target.ApplyTemplate();

            var presenter = target.Presenter;
            var panel = presenter.Panel;

            Assert.Equal(target, LooklessControl.GetTemplatedParent(target.Presenter));
        }

        [Fact]
        public void Presenter_Panel_Should_Have_TemplatedParent_Set_To_ItemsControl()
        {
            var target = new ItemsControl();

            target.Template = this.GetTemplate();
            target.Items = new[] { "Foo" };
            target.ApplyTemplate();

            var presenter = target.Presenter;
            var panel = presenter.Panel;

            Assert.Equal(target, LooklessControl.GetTemplatedParent(target.Presenter.Panel));
        }

        [Fact]
        public void Item_Should_Have_TemplatedParent_Set_To_Null()
        {
            var target = new ItemsControl();

            target.Template = this.GetTemplate();
            target.Items = new[] { "Foo" };
            target.ApplyTemplate();

            var item = (TextBlock)target.Presenter.Panel.Children.First();

            Assert.Null(LooklessControl.GetTemplatedParent(item));
        }

        [Fact]
        public void Control_Item_Should_Have_Parent_Set()
        {
            var target = new ItemsControl();
            var child = new Control();

            target.Template = this.GetTemplate();
            target.Items = new[] { child };
            target.ApplyTemplate();

            Assert.Equal(target, child.Parent);
            Assert.Equal(target, ((ILogical)child).LogicalParent);
        }

        [Fact]
        public void Clearing_Control_Item_Should_Clear_Child_Controls_Parent()
        {
            var target = new ItemsControl();
            var child = new Control();

            target.Template = this.GetTemplate();
            target.Items = new[] { child };
            target.ApplyTemplate();
            target.Items = null;

            Assert.Null(child.Parent);
            Assert.Null(((ILogical)child).LogicalParent);
        }

        [Fact]
        public void Adding_Control_Item_Should_Make_Control_Appear_In_LogicalChildren()
        {
            var target = new ItemsControl();
            var child = new Control();

            target.Template = this.GetTemplate();
            target.Items = new[] { child };
            target.ApplyTemplate();

            Assert.Equal(new[] { child }, ((ILogical)target).LogicalChildren.ToList());
        }

        [Fact]
        public void Adding_String_Item_Should_Make_TextBlock_Appear_In_LogicalChildren()
        {
            var target = new ItemsControl();
            var child = new Control();

            target.Template = this.GetTemplate();
            target.Items = new[] { "Foo" };
            target.ApplyTemplate();

            var logical = (ILogical)target;
            Assert.Equal(1, logical.LogicalChildren.Count);
            Assert.IsType<TextBlock>(logical.LogicalChildren[0]);
        }

        [Fact]
        public void Setting_Items_To_Null_Should_Remove_LogicalChildren()
        {
            var target = new ItemsControl();
            var child = new Control();

            target.Template = this.GetTemplate();
            target.Items = new[] { "Foo" };
            target.ApplyTemplate();
            target.Items = null;

            Assert.Equal(new ILogical[0], ((ILogical)target).LogicalChildren.ToList());
        }

        [Fact]
        public void Setting_Items_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ItemsControl();
            var child = new Control();
            var called = false;

            target.Template = this.GetTemplate();
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) =>
                called = e.Action == NotifyCollectionChangedAction.Add;

            target.Items = new[] { child };

            Assert.True(called);
        }

        [Fact]
        public void Setting_Items_To_Null_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ItemsControl();
            var child = new Control();
            var called = false;

            target.Template = this.GetTemplate();
            target.Items = new[] { child };
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) =>
                called = e.Action == NotifyCollectionChangedAction.Remove;

            target.Items = null;

            Assert.True(called);
        }

        [Fact]
        public void Changing_Items_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ItemsControl();
            var child = new Control();
            var called = false;

            target.Template = this.GetTemplate();
            target.Items = new[] { child };
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) => called = true;

            target.Items = new[] { "Foo" };

            Assert.True(called);
        }

        [Fact]
        public void Adding_Items_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ItemsControl();
            var items = new PerspexList<string> { "Foo" };
            var called = false;

            target.Template = this.GetTemplate();
            target.Items = items;
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) =>
                called = e.Action == NotifyCollectionChangedAction.Add;

            items.Add("Bar");

            Assert.True(called);
        }

        [Fact]
        public void Removing_Items_Should_Fire_LogicalChildren_CollectionChanged()
        {
            var target = new ItemsControl();
            var items = new PerspexList<string> { "Foo", "Bar" };
            var called = false;

            target.Template = this.GetTemplate();
            target.Items = items;
            target.ApplyTemplate();

            ((ILogical)target).LogicalChildren.CollectionChanged += (s, e) =>
                called = e.Action == NotifyCollectionChangedAction.Remove;

            items.Remove("Bar");

            Assert.True(called);
        }

        [Fact]
        public void LogicalChildren_Should_Not_Change_Instance_When_Template_Changed()
        {
            var target = new ItemsControl()
            {
                Template = this.GetTemplate(),
            };

            var before = ((ILogical)target).LogicalChildren;

            target.Template = null;
            target.Template = this.GetTemplate();

            var after = ((ILogical)target).LogicalChildren;

            Assert.NotNull(before);
            Assert.NotNull(after);
            Assert.Same(before, after);
        }

        [Fact]
        public void IsEmpty_Should_Initially_Be_Set()
        {
            var target = new ItemsControl()
            {
                Template = this.GetTemplate(),
            };

            target.ApplyTemplate();

            Assert.True(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Cleared_When_Items_Added()
        {
            var target = new ItemsControl()
            {
                Template = this.GetTemplate(),
                Items = new[] { 1, 2, 3 },
            };

            target.ApplyTemplate();

            Assert.False(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Set_When_Empty_Collection_Set()
        {
            var target = new ItemsControl()
            {
                Template = this.GetTemplate(),
                Items = new[] { 1, 2, 3 },
            };

            target.Items = new int[0];
            target.ApplyTemplate();

            Assert.True(target.IsEmpty);
        }

        ////[Fact]
        ////public void Setting_Presenter_Explicitly_Should_Set_Item_Parent()
        ////{
        ////    var target = new TestItemsControl();
        ////    var child = new Control();

        ////    var presenter = new ItemsPresenter
        ////    {
        ////        TemplatedParent = target,
        ////        [~ItemsPresenter.ItemsProperty] = target[~ItemsControl.ItemsProperty],
        ////    };

        ////    presenter.ApplyTemplate();
        ////    target.Presenter = presenter;
        ////    target.Items = new[] { child };
        ////    target.ApplyTemplate();

        ////    Assert.Equal(target, child.Parent);
        ////    Assert.Equal(target, ((ILogical)child).LogicalParent);
        ////}

        private LooklessControlTemplate GetTemplate()
        {
            return new LooklessControlTemplate<ItemsControl>(parent =>
            {
                return new Border
                {
                    Background = new Perspex.Media.SolidColorBrush(0xffffffff),
                    Child = new ItemsPresenter
                    {
                        Name = "itemsPresenter",
                        Panel = parent.ItemsPanel.Build(),
                        [~ItemsPresenter.ItemsProperty] = parent[~ItemsControl.ItemsProperty],
                        [(~ItemsPresenter.IsEmptyProperty).WithMode(BindingMode.OneWayToSource)] = parent[!ItemsControl.IsEmptyProperty],
                    }
                };
            });
        }

        private class TestItemsControl : ItemsControl
        {
            public new IItemsPresenter Presenter
            {
                get { return base.Presenter; }
                ////set { base.Presenter = value; }
            }
        }
    }
}
