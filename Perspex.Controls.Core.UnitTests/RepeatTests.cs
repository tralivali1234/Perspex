// -----------------------------------------------------------------------
// <copyright file="RepeatTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Perspex.Collections;
    using Perspex.LogicalTree;
    using Perspex.VisualTree;
    using Xunit;

    public class RepeatTests
    {
        [Fact]
        public void Panel_Should_Be_Visual_Child()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
            };

            Assert.Equal(new[] { target.Panel }, target.GetVisualChildren());
        }

        [Fact]
        public void Panel_Should_Be_Logical_Child()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
            };

            Assert.Equal(new[] { target.Panel }, target.GetLogicalChildren());
        }

        [Fact]
        public void Panel_Should_Have_LogicalParent_Set()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
            };

            Assert.Equal(target, target.Panel.Parent);
        }

        [Fact]
        public void Should_Create_Containers()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            Assert.Equal(2, target.Panel.Children.Count);
        }

        [Fact]
        public void Should_Create_Containers_If_Panel_Assigned_After_Items()
        {
            var target = new Repeat
            {
                Items = new[] { "Foo", "Bar" },
                Panel = new StackPanel(),
            };

            Assert.Equal(2, target.Panel.Children.Count);
        }

        [Fact]
        public void Should_Add_Containers()
        {
            var items = new PerspexList<string>
            {
                "Foo",
                "Bar",
            };

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.Add("Baz");

            Assert.Equal(3, target.Panel.Children.Count);
        }

        [Fact]
        public void Should_Remove_Containers()
        {
            var items = new PerspexList<string>
            {
                "Foo",
                "Bar",
            };

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.RemoveAt(0);

            Assert.Equal(1, target.Panel.Children.Count);
        }

        [Fact]
        public void Removing_Item_Should_Clear_Container_Parent()
        {
            var items = new PerspexList<string>
            {
                "Foo",
                "Bar",
            };

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            var item = target.Panel.Children.First();
            items.RemoveAt(0);

            Assert.Null(item.GetVisualParent());
            Assert.Null(item.GetLogicalParent());
            Assert.Null(item.Parent);
        }

        [Fact]
        public void Resetting_Items_Should_Clear_Container_Parent()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            var item = target.Panel.Children.First();
            target.Items = null;

            Assert.Null(item.GetVisualParent());
            Assert.Null(item.GetLogicalParent());
            Assert.Null(item.Parent);
        }

        [Fact]
        public void Should_Handle_Duplicate_Items()
        {
            var items = new PerspexList<int>(new[] { 1, 2, 1 });

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.RemoveAt(2);

            var text = target.Panel.Children.OfType<TextBlock>().Select(x => x.Text);
            Assert.Equal(new[] { "1", "2" }, text);
        }

        [Fact]
        public void Clearing_ObservableCollection_Should_Remove_Containers()
        {
            var items = new ObservableCollection<int>(new[] { 1, 2, 1 });

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.Clear();

            Assert.Empty(target.Panel.Children);
        }

        [Fact]
        public void Should_Use_ItemTemplate()
        {
            var target = new Repeat
            {
                ItemTemplate = new DataTemplate<string>(_ => new Border()),
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            var types = target.Panel.Children.Select(x => x.GetType());
            Assert.Equal(new[] { typeof(Border), typeof(Border) }, types);
        }

        [Fact]
        public void Removing_Panel_Should_Remove_Children()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            var panel = target.Panel;
            target.Panel = null;

            Assert.Empty(panel.Children);
        }

        [Fact]
        public void Replacing_Panel_Should_Recreate_Children()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            target.Panel = new StackPanel();

            Assert.Equal(2, target.Panel.Children.Count);
        }

        [Fact]
        public void Should_Handle_Null_Items()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", null, "Bar" },
            };

            Assert.Equal(2, target.Panel.Children.Count);
        }

        [Fact]
        public void IsEmpty_Should_Initially_Be_Set()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
            };

            Assert.True(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Set_With_Empty_Items()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new string[0],
            };

            Assert.True(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Cleared_When_Items_Assigned()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo", "Bar" },
            };

            Assert.False(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Cleared_When_Items_Added()
        {
            var items = new PerspexList<string>();

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.Add("Foo");

            Assert.False(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Set_When_All_Items_Removed()
        {
            var items = new PerspexList<string>(new[] { "Foo" });

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.RemoveAt(0);

            Assert.True(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Set_When_Items_Cleared()
        {
            var items = new ObservableCollection<string>(new[] { "Foo" });

            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = items,
            };

            items.Clear();

            Assert.True(target.IsEmpty);
        }

        [Fact]
        public void IsEmpty_Should_Be_Set_When_Items_Reset()
        {
            var target = new Repeat
            {
                Panel = new StackPanel(),
                Items = new[] { "Foo" },
            };

            target.Items = null;

            Assert.True(target.IsEmpty);
        }
    }
}
