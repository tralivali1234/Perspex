// -----------------------------------------------------------------------
// <copyright file="ItemsPresenterTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.UnitTests
{
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard.Presenters;
    using Xunit;

    public class ItemsPresenterTests
    {
        [Fact]
        public void ItemsPanel_Should_Initially_Be_Null()
        {
            var target = new RepeatPresenter();

            Assert.Null(target.Panel);
        }

        [Fact]
        public void Assigning_ItemsPanel_Should_Set_Panel()
        {
            var target = new RepeatPresenter();

            target.ItemsPanel = new FuncTemplate<IPanel>(() => new StackPanel());

            Assert.IsType<StackPanel>(target.Panel);
        }
    }
}
