// -----------------------------------------------------------------------
// <copyright file="PanelTests_ReparentLogicalChildren.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using Perspex.Collections;
    using Perspex.LogicalTree;
    using Xunit;

    public class PanelTests_ReparentLogicalChildren
    {
        [Fact]
        public void Reparented_Panel_Should_Set_Child_LogicalParent()
        {
            var target = new Panel();
            var parent = new Panel();
            var child = new Border();
            var children = new PerspexList<ILogical>();

            ((IReparentingControl)target).ReparentLogicalChildren(parent, children);
            target.Children.Add(child);

            Assert.Equal(parent, child.Parent);
            Assert.Equal(parent, child.GetLogicalParent());
        }

        [Fact]
        public void Reparented_Panel_Should_Add_Controls_To_LogicalParent_Collection()
        {
            var target = new Panel();
            var parent = new Panel();
            var child = new Border();
            var children = new PerspexList<ILogical>();

            ((IReparentingControl)target).ReparentLogicalChildren(parent, children);
            target.Children.Add(child);

            Assert.Equal(new[] { child }, children);
        }

        [Fact]
        public void Reparenting_Existing_Children_Should_Work()
        {
            var target = new Panel();
            var parent = new Panel();
            var child = new Border();
            var children = new PerspexList<ILogical>();

            target.Children.Add(child);
            ((IReparentingControl)target).ReparentLogicalChildren(parent, children);

            Assert.Equal(parent, child.Parent);
            Assert.Equal(parent, child.GetLogicalParent());
            Assert.Equal(new[] { child }, children);
        }
    }
}
