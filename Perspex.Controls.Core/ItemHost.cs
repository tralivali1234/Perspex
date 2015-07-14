// -----------------------------------------------------------------------
// <copyright file="ItemHost.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using Perspex.Collections;
    using Perspex.Controls.Core.Templates;

    /// <summary>
    /// Displays a piece of data according to a <see cref="DataTemplate"/>.
    /// </summary>
    public class ItemHost : Control, ILogical
    {
        /// <summary>
        /// Defines the <see cref="Content"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> ContentProperty =
            PerspexProperty.Register<ItemHost, object>("Content");

        private PerspexSingleItemList<ILogical> logicalChild = new PerspexSingleItemList<ILogical>();

        /// <summary>
        /// Initializes static members of the <see cref="ItemHost"/> class.
        /// </summary>
        static ItemHost()
        {
            ContentProperty.Changed.AddClassHandler<ItemHost>(x => x.ItemChanged);
        }

        /// <summary>
        /// Gets or sets the content of the item.
        /// </summary>
        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return this.logicalChild; }
        }

        /// <summary>
        /// Called when the <see cref="Content"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ItemChanged(PerspexPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((ISetLogicalParent)this.logicalChild.SingleItem).SetParent(null);
                this.logicalChild.SingleItem = null;
                this.ClearVisualChildren();
            }

            if (e.NewValue != null)
            {
                var child = this.MaterializeDataTemplate(e.NewValue);
                this.AddVisualChild(child);
                this.logicalChild.SingleItem = child;
                ((ISetLogicalParent)child).SetParent(this);
            }
        }
    }
}
