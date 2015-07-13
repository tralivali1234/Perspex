// -----------------------------------------------------------------------
// <copyright file="LooklessControlTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.UnitTests
{
    using System;
    using System.Linq;
    using Perspex.Controls.Core;
    using Perspex.VisualTree;
    using Xunit;

    public class LooklessControlTests
    {
        [Fact]
        public void Initially_Has_No_Visual_Children()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            Assert.Empty(target.GetVisualChildren());
        }

        [Fact]
        public void ApplyTemplate_Creates_Visual_Child()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            target.ApplyTemplate();

            Assert.Equal(1, target.GetVisualChildren().Count());
            Assert.IsType<TextBlock>(target.GetVisualChildren().First());
        }

        [Fact]
        public void ApplyTemplate_Calls_OnTemplateApplied()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            var called = false;

            target.OnTemplateAppliedCalled += (s, e) => called = true;
            target.ApplyTemplate();

            Assert.True(called);
        }

        [Fact]
        public void ApplyTemplate_Doesnt_Call_OnTemplateApplied_If_Template_Null()
        {
            var target = new TestControl();
            var called = false;

            target.OnTemplateAppliedCalled += (s, e) => called = true;
            target.ApplyTemplate();

            Assert.False(called);
        }

        [Fact]
        public void ApplyTemplate_Doesnt_Apply_Template_If_Already_Applied()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            var called = false;

            target.ApplyTemplate();
            target.OnTemplateAppliedCalled += (s, e) => called = true;
            target.ApplyTemplate();

            Assert.False(called);
        }

        [Fact]
        public void ApplyTemplate_Applys_Changed_Template()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            var called = false;

            target.ApplyTemplate();
            target.Template = new LooklessControlTemplate(x => new Border());
            target.OnTemplateAppliedCalled += (s, e) => called = true;
            target.ApplyTemplate();

            Assert.True(called);
        }

        private class TestControl : LooklessControl
        {
            public event EventHandler OnTemplateAppliedCalled;

            protected override void OnTemplateApplied()
            {
                this.OnTemplateAppliedCalled?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
