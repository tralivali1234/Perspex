// -----------------------------------------------------------------------
// <copyright file="Decorator.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using Perspex.Collections;

    /// <summary>
    /// Base class for controls which decorate a single child control.
    /// </summary>
    public class Decorator : Control, IVisual, ILogical
    {
        /// <summary>
        /// Defines the <see cref="Child"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> ChildProperty =
            PerspexProperty.Register<Decorator, IControl>("Content");

        /// <summary>
        /// Defines the <see cref="Padding"/> property.
        /// </summary>
        public static readonly PerspexProperty<Thickness> PaddingProperty =
            PerspexProperty.Register<Decorator, Thickness>("Padding");

        private PerspexSingleItemList<ILogical> logicalChild = new PerspexSingleItemList<ILogical>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Decorator"/> class.
        /// </summary>
        public Decorator()
        {
            ChildProperty.Changed.AddClassHandler<Decorator>(x => x.ChildChanged);
        }

        /// <summary>
        /// Gets or sets the decorated control.
        /// </summary>
        public IControl Child
        {
            get { return this.GetValue(ChildProperty); }
            set { this.SetValue(ChildProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding to place around the decorated control.
        /// </summary>
        public Thickness Padding
        {
            get { return this.GetValue(PaddingProperty); }
            set { this.SetValue(PaddingProperty, value); }
        }

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return this.logicalChild; }
        }

        /// <summary>
        /// Measures the control.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The desired size of the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            var content = this.Child;
            var padding = this.Padding;

            if (content != null)
            {
                content.Measure(availableSize.Deflate(padding));
                return content.DesiredSize.Inflate(padding);
            }
            else
            {
                return new Size(padding.Left + padding.Right, padding.Bottom + padding.Top);
            }
        }

        /// <summary>
        /// Arranges the control's child.
        /// </summary>
        /// <param name="finalSize">The size allocated to the control.</param>
        /// <returns>The space taken.</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            var content = this.Child;

            if (content != null)
            {
                content.Arrange(new Rect(finalSize).Deflate(this.Padding));
            }

            return finalSize;
        }

        /// <summary>
        /// Called when the <see cref="Child"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ChildChanged(PerspexPropertyChangedEventArgs e)
        {
            var oldChild = (Control)e.OldValue;
            var newChild = (Control)e.NewValue;

            if (oldChild != null)
            {
                this.RemoveVisualChild(oldChild);
                ((ILogical)oldChild).LogicalParent = null;
            }

            if (newChild != null)
            {
                this.AddVisualChild(newChild);
                ((ILogical)newChild).LogicalParent = this;
            }

            this.logicalChild.SingleItem = newChild;
        }
    }
}
