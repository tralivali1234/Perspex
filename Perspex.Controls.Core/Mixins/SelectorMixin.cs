// -----------------------------------------------------------------------
// <copyright file="SelectorMixin.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Mixins
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Adds selector functionality to control classes.
    /// </summary>
    /// <typeparam name="TControl">The control type.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <remarks>
    /// <para>
    /// The <see cref="SelectorMixin"/> maintains the SelectedIndex and SelectedItem properties
    /// to ensure that these properties are always in sync and have valid values.
    /// </para>
    /// <para>
    /// Mixins apply themselves to classes and not instances, and as such should be created in
    /// a static constructor.
    /// </para>
    /// </remarks>
    public class SelectorMixin<TControl, TItem>
        where TControl : PerspexObject, IControl
        where TItem : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectorMixin{TControl, TItem}"/> class.
        /// </summary>
        /// <param name="selectedIndex">The SelectedIndex property.</param>
        /// <param name="selectedItem">The SelectedItem property.</param>
        /// <param name="itemsSelector">Given a control, returns the children.</param>
        public SelectorMixin(
            PerspexProperty<int> selectedIndex,
            PerspexProperty<TItem> selectedItem,
            Func<TControl, IEnumerable> itemsSelector)
        {
            Contract.Requires<ArgumentNullException>(selectedIndex != null);
            Contract.Requires<ArgumentNullException>(selectedItem != null);
            Contract.Requires<ArgumentNullException>(itemsSelector != null);

            // Ensure the SelectedIndex and SelectedItem properties are in the valid range for
            // the number of items.
            selectedIndex.OverrideValidation<TControl>((obj, index) =>
            {
                var items = itemsSelector(obj);
                return (index >= 0 && index < items?.Cast<TItem>().Count()) ? index : -1;
            });

            selectedItem.OverrideValidation<TControl>((obj, item) =>
            {
                var items = itemsSelector(obj);
                return items != null && items.Cast<TItem>().Contains(item) ? item : default(TItem);
            });

            // Sets SelectedItem based on the SelectedIndex.
            selectedIndex.Changed.Subscribe(x =>
            {
                var sender = x.Sender as TControl;

                if (sender != null)
                {
                    var index = (int)x.NewValue;

                    if (index == -1)
                    {
                        sender.SetValue(selectedItem, null);
                    }
                    else
                    {
                        sender.SetValue(
                            selectedItem,
                            itemsSelector(sender).Cast<TItem>().ElementAt((int)x.NewValue));
                    }
                }
            });

            // Sets SelectedIndex based on the SelectedItem.
            selectedItem.Changed.Subscribe(x =>
            {
                var sender = x.Sender as TControl;

                if (sender != null)
                {
                    sender.SetValue(
                        selectedIndex,
                        IndexOf(itemsSelector(sender), (TItem)x.NewValue));
                }
            });

            this.ItemRemoved = (control, item) =>
            {
                if (item == control.GetValue(selectedItem))
                {
                    control.SetValue(selectedItem, null);
                }
            };
        }

        /// <summary>
        /// Should be called by the control when an item is removed.
        /// </summary>
        public Action<TControl, TItem> ItemRemoved { get; }

        /// <summary>
        /// Gets the index of an item in a collection.
        /// </summary>
        /// <param name="items">The collection.</param>
        /// <param name="item">The item.</param>
        /// <returns>The index of the item or -1 if the item was not found.</returns>
        private static int IndexOf(IEnumerable items, TItem item)
        {
            var list = items as IList<TItem>;

            if (list != null)
            {
                return list.IndexOf(item);
            }
            else
            {
                int index = 0;

                foreach (var i in items)
                {
                    if (object.Equals(i, item))
                    {
                        return index;
                    }

                    ++index;
                }

                return -1;
            }
        }
    }
}
