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
    public class Decorator : Control, ILogical
    {
        /// <summary>
        /// Defines the <see cref="Child"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> ChildProperty =
            PerspexProperty.Register<Decorator, IControl>("Content");

        private PerspexSingleItemList<ILogical> logicalChild = new PerspexSingleItemList<ILogical>();

        /// <summary>
        /// Initializes static members of the <see cref="Decorator"/> class.
        /// </summary>
        static Decorator()
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

            if (content != null)
            {
                content.Measure(availableSize);
                return content.DesiredSize;
            }
            else
            {
                return base.MeasureOverride(availableSize);
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
                content.Arrange(new Rect(finalSize));
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
                ((ISetLogicalParent)oldChild).SetParent(null);
                this.RemoveVisualChild(oldChild);
            }

            if (newChild != null)
            {
                this.AddVisualChild(newChild);
                ((ISetLogicalParent)newChild).SetParent(this);
            }

            this.logicalChild.SingleItem = newChild;
        }
    }
}
