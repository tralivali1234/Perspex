// -----------------------------------------------------------------------
// <copyright file="SelectingItemsControl.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Primitives
{
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard;

    /// <summary>
    /// An <see cref="ItemsControl"/> that maintains a selection.
    /// </summary>
    /// <remarks>
    /// TODO: Support multiple selection.
    /// </remarks>
    public abstract class SelectingItemsControl : ItemsControl
    {
        /// <summary>
        /// Defines the <see cref="SelectedContainer"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> SelectedContainerProperty =
            Selector.SelectedContainerProperty.AddOwner<SelectingItemsControl>();

        /// <summary>
        /// Defines the <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly PerspexProperty<int> SelectedIndexProperty =
            Selector.SelectedIndexProperty.AddOwner<SelectingItemsControl>();

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> SelectedItemProperty =
            Selector.SelectedItemProperty.AddOwner<SelectingItemsControl>();

        /// <summary>
        /// Gets or sets the selected container control.
        /// </summary>
        public IControl SelectedContainer
        {
            get { return this.GetValue(SelectedContainerProperty); }
            set { this.SetValue(SelectedContainerProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the selected item.
        /// </summary>
        public int SelectedIndex
        {
            get { return this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }
    }
}
