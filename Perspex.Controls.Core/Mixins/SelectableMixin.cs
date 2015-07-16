// -----------------------------------------------------------------------
// <copyright file="SelectableMixin.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Mixins
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Adds selectable functionality to control classes.
    /// </summary>
    public static class SelectableMixin
    {
        /// <summary>
        /// Adds selectable functionality to the specified control type.
        /// </summary>
        /// <typeparam name="TControl">The control type.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="selectedIndex">The SelectedIndex property.</param>
        /// <param name="selectedItem">The SelectedItem property.</param>
        /// <param name="itemsSelector">Given a control, returns the children.</param>
        public static void Attach<TControl, TItem>(
            PerspexProperty<int> selectedIndex,
            PerspexProperty<TItem> selectedItem,
            Func<TControl, IList<TItem>> itemsSelector)
                where TControl : PerspexObject, IControl
        {
            Contract.Requires<ArgumentNullException>(selectedIndex != null);
            Contract.Requires<ArgumentNullException>(selectedItem != null);
            Contract.Requires<ArgumentNullException>(itemsSelector != null);

            selectedIndex.OverrideValidation<TControl>((obj, index) =>
            {
                var items = itemsSelector(obj);
                return (index >= 0 && index < items?.Count) ? index : -1;
            });

            selectedItem.OverrideValidation<TControl>((obj, item) =>
            {
                var items = itemsSelector(obj);
                return items != null && items.Contains(item) ? item : default(TItem);
            });

            selectedIndex.Changed.Subscribe(x =>
            {
                var target = x.Sender as TControl;

                if (target != null)
                {
                    var index = (int)x.NewValue;

                    if (index == -1)
                    {
                        target.SetValue(selectedItem, null);
                    }
                    else
                    {
                        target.SetValue(selectedItem, itemsSelector(target)[(int)x.NewValue]);
                    }
                }
            });

            selectedItem.Changed.Subscribe(x =>
            {
                var target = x.Sender as TControl;

                if (target != null)
                {
                    target.SetValue(
                        selectedIndex,
                        itemsSelector(target)?.IndexOf((TItem)x.NewValue) ?? -1);
                }
            });
        }
    }
}
