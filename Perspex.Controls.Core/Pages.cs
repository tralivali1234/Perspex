// -----------------------------------------------------------------------
// <copyright file="Pages.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System.Linq;
    using Perspex.Controls.Core.Mixins;
    using Perspex.VisualTree;

    /// <summary>
    /// A panel which shows a single child at a time.
    /// </summary>
    public class Pages : Panel
    {
        /// <summary>
        /// Defines the <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly PerspexProperty<int> SelectedIndexProperty =
            Selector.SelectedIndexProperty.AddOwner<Pages>();

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> SelectedItemProperty =
            Selector.SelectedItemProperty.AddOwner<Pages>();

        /// <summary>
        /// Initializes static members of the <see cref="Pages"/> class.
        /// </summary>
        static Pages()
        {
            SelectableMixin.Attach<Pages, IControl>(
                SelectedIndexProperty,
                SelectedItemProperty,
                x => x.Children);
            AffectsMeasure(SelectedItemProperty);
        }

        /// <summary>
        /// Gets or sets the index of the child to be shown.
        /// </summary>
        /// <remarks>
        /// The valid range for this property value is from -1 (no selection) to
        /// <see cref="Children.Count"/> - 1. If an attempt is made to set the property
        /// to a value outside this range, then the value will be set to -1 and all children
        /// will be hidden.
        /// </remarks>
        public int SelectedIndex
        {
            get { return this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the child to be shown.
        /// </summary>
        /// <remarks>
        /// If an attempt is made to set the property to a object not contained in the
        /// <see cref="Children"/> collection then all children will be hidden.
        /// </remarks>
        public IControl SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            var item = this.SelectedItem;

            if (item != null)
            {
                item.Measure(availableSize);
                return item.DesiredSize;
            }
            else
            {
                return new Size();
            }
        }

        /// <inheritdoc/>
        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (var child in this.GetVisualChildren().OfType<Control>())
            {
                if (child == this.SelectedItem)
                {
                    child.IsVisible = true;
                    child.Arrange(new Rect(finalSize));
                }
                else
                {
                    child.IsVisible = false;
                }
            }

            return finalSize;
        }
    }
}
