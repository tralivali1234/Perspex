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
        public void ApplyTemplate_Creates_NameScope_And_Templated_Child()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            target.ApplyTemplate();

            Assert.Equal(1, target.GetVisualChildren().Count());
            var nameScope = target.GetVisualChildren().First() as NameScope;
            Assert.NotNull(nameScope);
            Assert.IsType<TextBlock>(nameScope.Child);
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

        [Fact]
        public void Templated_NameScope_Has_Parent_Property_Null()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            target.ApplyTemplate();

            Assert.Null(((NameScope)target.GetVisualChildren().First()).Parent);
        }

        [Fact]
        public void Templated_Child_Has_TemplatedParent_Property_Set()
        {
            var target = new TestControl
            {
                Template = new LooklessControlTemplate(x => new TextBlock())
            };

            target.ApplyTemplate();

            var textBlock = ((NameScope)target.GetVisualChildren().First()).Child;
            Assert.Equal(target, LooklessControl.GetTemplatedParent(textBlock));
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
