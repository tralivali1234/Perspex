// -----------------------------------------------------------------------
// <copyright file="SelectorTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
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
    }
}
