// -----------------------------------------------------------------------
// <copyright file="SelectorTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System;
    using Collections;
    using Perspex.Input;
    using Xunit;

    public class SelectorTests
    {
        [Fact]
        public void Selection_Should_Initially_Be_None()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Setting_SelectedContainer_Should_Set_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.SelectedContainer = target.Panel.Children[1];

            Assert.Equal(target.Panel.Children[1], target.SelectedContainer);
            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal("Bar", target.SelectedItem);
        }

        [Fact]
        public void Clearing_SelectedContainer_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
                SelectedIndex = 1,
            };

            target.SelectedContainer = null;

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Setting_SelectedIndex_Should_Set_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.SelectedIndex = 1;

            Assert.Equal(target.Panel.Children[1], target.SelectedContainer);
            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal("Bar", target.SelectedItem);
        }

        [Fact]
        public void Setting_SelectedIndex_To_Minus1_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
                SelectedIndex = 1,
            };

            target.SelectedIndex = -1;

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Setting_SelectedItem_Should_Set_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.SelectedItem = "Bar";

            Assert.Equal(target.Panel.Children[1], target.SelectedContainer);
            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal("Bar", target.SelectedItem);
        }

        [Fact]
        public void Clearing_SelectedItem_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
                SelectedIndex = 1,
            };

            target.SelectedItem = null;

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Removing_SelectedItem_Should_Clear_Selection()
        {
            var items = new PerspexList<string>(new[] { "Foo", "Bar" });

            var target = new Selector
            {
                Items = items,
                Panel = new StackPanel(),
                SelectedIndex = 1,
            };

            items.RemoveAt(1);

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Clearing_Panel_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
                SelectedIndex = 0
            };

            target.Panel = null;

            Assert.Null(target.SelectedContainer);
            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void PointerDown_Event_Should_Set_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.Panel.Children[1].RaiseEvent(new PointerPressEventArgs
            {
                RoutedEvent = InputElement.PointerPressedEvent,
            });

            Assert.Equal(1, target.SelectedIndex);
        }

        [Fact]
        public void GotFocus_Event_Should_Set_Selection()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.Panel.Children[1].RaiseEvent(new GotFocusEventArgs
            {
                RoutedEvent = InputElement.GotFocusEvent
            });

            Assert.Equal(1, target.SelectedIndex);
        }

        [Fact]
        public void PointerDown_Event_Should_Not_Set_Selection_When_IsUserSelectable_False()
        {
            var target = new Selector
            {
                IsUserSelectable = false,
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            target.Panel.Children[1].RaiseEvent(new PointerPressEventArgs
            {
                RoutedEvent = InputElement.PointerPressedEvent,
            });

            Assert.Equal(-1, target.SelectedIndex);
        }

        [Fact]
        public void Selected_Container_Should_Have_Selected_Class_If_Not_ISelectable()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
                SelectedIndex = 0,
            };

            Assert.True(target.Panel.Children[0].Classes.Contains("selected"));
            Assert.False(target.Panel.Children[1].Classes.Contains("selected"));

            target.SelectedIndex = 1;

            Assert.False(target.Panel.Children[0].Classes.Contains("selected"));
            Assert.True(target.Panel.Children[1].Classes.Contains("selected"));
        }

        [Fact]
        public void Selected_Container_Should_Have_IsSelected_True_If_ISelectable()
        {
            var target = new Selector
            {
                Items = new[] { "Foo", "Bar" },
                ItemTemplate = new DataTemplate<string>(_ => new Selectable()),
                Panel = new StackPanel(),
                SelectedIndex = 0,
            };

            Assert.True(((ISelectable)target.Panel.Children[0]).IsSelected);
            Assert.False(((ISelectable)target.Panel.Children[1]).IsSelected);

            target.SelectedIndex = 1;

            Assert.False(((ISelectable)target.Panel.Children[0]).IsSelected);
            Assert.True(((ISelectable)target.Panel.Children[1]).IsSelected);
        }

        [Fact]
        public void Adding_Selected_ISelectable_Container_Should_Set_Selection()
        {
            var items = new PerspexList<string>(new[] { "Foo", "Bar" });

            var target = new Selector
            {
                Items = items,
                ItemTemplate = new DataTemplate<string>(x => new Selectable
                {
                    IsSelected = x == "Baz",
                }),
                Panel = new StackPanel(),
                SelectedIndex = 0,
            };

            Assert.True(((ISelectable)target.Panel.Children[0]).IsSelected);
            Assert.False(((ISelectable)target.Panel.Children[1]).IsSelected);

            items.Add("Baz");

            Assert.False(((ISelectable)target.Panel.Children[0]).IsSelected);
            Assert.False(((ISelectable)target.Panel.Children[1]).IsSelected);
            Assert.True(((ISelectable)target.Panel.Children[2]).IsSelected);
        }

        private class Selectable : Border, ISelectable
        {
            public bool IsSelected { get; set; }
        }
    }
}
