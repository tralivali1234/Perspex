// -----------------------------------------------------------------------
// <copyright file="LooklessControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using System.Collections.Generic;
    using System.Linq;
    using Serilog;
    using Serilog.Core.Enrichers;
    using Perspex.Controls.Standard.Presenters;
    using Perspex.VisualTree;
    using Perspex.Layout;

    /// <summary>
    /// Base class for lookless controls.
    /// </summary>
    /// <remarks>
    /// Lookless controls are controls which define behavior but not look. Instead of the look
    /// being defined by the control itself, it is provided by the control's <see cref="Template"/>
    /// property.
    /// </remarks>
    public abstract class LooklessControl : Control, ILooklessControl
    {
        /// <summary>
        /// Defines the <see cref="Template"/> property.
        /// </summary>
        public static readonly PerspexProperty<ILooklessControlTemplate> TemplateProperty =
            PerspexProperty.Register<LooklessControl, ILooklessControlTemplate>("Template");

        /// <summary>
        /// Defines the TemplatedParent attached property.
        /// </summary>
        public static readonly PerspexProperty<ILooklessControl> TemplatedParentProperty =
            PerspexProperty.RegisterAttached<LooklessControl, Control, ILooklessControl>("TemplatedParent");

        private bool templateApplied;

        private ILogger templateLog;

        /// <summary>
        /// Initializes static members of the <see cref="LooklessControl"/> class.
        /// </summary>
        static LooklessControl()
        {
            TemplateProperty.Changed.AddClassHandler<LooklessControl>(x => x.TemplateChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LooklessControl"/> class.
        /// </summary>
        public LooklessControl()
        {
            this.templateLog = Log.ForContext(new[]
            {
                new PropertyEnricher("Area", "Template"),
                new PropertyEnricher("SourceContext", this.GetType()),
                new PropertyEnricher("Id", this.GetHashCode()),
            });
        }

        /// <summary>
        /// Gets or sets the template that defines the look for the lookless control.
        /// </summary>
        public ILooklessControlTemplate Template
        {
            get { return this.GetValue(TemplateProperty); }
            set { this.SetValue(TemplateProperty, value); }
        }

        /// <summary>
        /// Gets the value of the TemplatedParent attached property.
        /// </summary>
        /// <param name="control">The control from which to read the property.</param>
        /// <returns>The value of the property on the control.</returns>
        public static ILooklessControl GetTemplatedParent(IControl control)
        {
            return (ILooklessControl)control.GetValue(TemplatedParentProperty);
        }

        /// <summary>
        /// Sets the value of the TemplatedParent attached property.
        /// </summary>
        /// <param name="control">The control on which to set the property.</param>
        /// <param name="value">The property value/</param>
        public static void SetTemplatedParent(IControl control, ILooklessControl value)
        {
            control.SetValue(TemplatedParentProperty, value, BindingPriority.LocalValue);
        }

        /// <summary>
        /// Applies the control's <see cref="Template"/> if it is not already applied.
        /// </summary>
        public sealed override void ApplyTemplate()
        {
            if (!this.templateApplied)
            {
                this.ClearVisualChildren();

                if (this.Template != null)
                {
                    this.templateLog.Verbose("Creating control template");

                    var child = this.Template.Build(this);
                    var children = this.ApplyTemplatedParent(child);

                    this.AddVisualChild(child);
                    ((ISetLogicalParent)child).SetParent(this);

                    foreach (var i in children.OfType<ILayoutable>())
                    {
                        i.ApplyTemplate();
                    }

                    this.OnTemplateApplied();
                }

                this.templateApplied = true;
            }
        }

        /// <summary>
        /// Measures the control.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The desired size of the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            Control child = ((IVisual)this).VisualChildren.SingleOrDefault() as Control;

            if (child != null)
            {
                child.Measure(availableSize);
                return child.DesiredSize;
            }

            return new Size();
        }

        /// <summary>
        /// Arranges the control's child.
        /// </summary>
        /// <param name="finalSize">The size allocated to the control.</param>
        /// <returns>The space taken.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            Control child = ((IVisual)this).VisualChildren.SingleOrDefault() as Control;

            if (child != null)
            {
                child.Arrange(new Rect(finalSize));
                return child.Bounds.Size;
            }
            else
            {
                return new Size();
            }
        }

        /// <summary>
        /// Called when the <see cref="LooklessControl"/>'s <see cref="Template"/> is applied.
        /// </summary>
        protected virtual void OnTemplateApplied()
        {
        }

        /// <summary>
        /// Called when the <see cref="Template"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void TemplateChanged(PerspexPropertyChangedEventArgs e)
        {
            this.templateApplied = false;
            this.InvalidateMeasure();
        }

        /// <summary>
        /// Sets the <see cref="TemplatedParentProperty"/> attached property to the specified
        /// control and optionally its children if the control is not an <see cref="IPresenter"/>.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns>
        /// The controls in the template.
        /// </returns>
        private IEnumerable<IControl> ApplyTemplatedParent(IControl control)
        {
            SetTemplatedParent(control, this);

            yield return control;

            if (!(control is IPresenter))
            {
                foreach (var child in control.GetVisualChildren().OfType<IControl>())
                {
                    foreach (var i in this.ApplyTemplatedParent(child))
                    {
                        yield return i;
                    }
                }
            }
        }
    }
}
