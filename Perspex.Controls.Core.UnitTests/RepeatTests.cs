// -----------------------------------------------------------------------
// <copyright file="RepeatTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System.Linq;
    using System.Collections.ObjectModel;
    using Perspex.Collections;
    using Xunit;

    public class RepeatTests
    {
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
    }
}
