// -----------------------------------------------------------------------
// <copyright file="SelectorMixin.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Mixins
{
    using System;
    using System.Collections.Generic;
    using Perspex.Interactivity;
    using Perspex.Styling;

    /// <summary>
    /// Adds selector functionality to control classes.
    /// </summary>
    /// <typeparam name="TControl">The control type.</typeparam>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <remarks>
    /// <para>
    /// The <see cref="SelectorMixin"/> adds behavior to a class which maintains a selection
    /// of items. It adds the following behavior:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// Maintains the SelectedIndex and SelectedItem properties to ensure that these
    /// properties are always in sync and have valid values.
    /// </item>
    /// <item>
    /// Listens to the selected state of the items in the selector and updates the selection
    /// accordingly.
    /// </item>
    /// <item>
    /// If an item implements <see cref="ISelectable"/> then sets its
    /// <see cref="ISelectable.IsSelected"/> property according to its selection state. If the
    /// item does not implement IsSelected but implements IStyleable, then adss a "selected"
    /// class to the selected child.
    /// </item>
    /// </list>
    /// <para>
    /// Note that this mixin does not handle selecting items based on user input.
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
            Func<TControl, IList<TItem>> itemsSelector)
        {
            Contract.Requires<ArgumentNullException>(selectedIndex != null);
            Contract.Requires<ArgumentNullException>(selectedItem != null);
            Contract.Requires<ArgumentNullException>(itemsSelector != null);

            // Ensure the SelectedIndex and SelectedItem properties are in the valid range for
            // the number of items.
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

            // Synchronize the SelectedIndex and SelectedItem properties.
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
                        sender.SetValue(selectedItem, itemsSelector(sender)[(int)x.NewValue]);
                    }
                }
            });

            // Set the IsSelected/'selected' class on the selected item.
            selectedItem.Changed.Subscribe(x =>
            {
                var sender = x.Sender as TControl;
                var item = x.OldValue as IStyleable;
                var selectable = x.OldValue as ISelectable;

                if (selectable != null)
                {
                    selectable.IsSelected = false;
                }
                else if (item != null)
                {
                    item.Classes.Remove("selected");
                }

                item = x.NewValue as IStyleable;
                selectable = x.NewValue as ISelectable;

                if (selectable != null)
                {
                    selectable.IsSelected = true;
                }
                else if (item != null)
                {
                    item.Classes.Add("selected");
                }

                if (sender != null)
                {
                    sender.SetValue(
                        selectedIndex,
                        itemsSelector(sender)?.IndexOf((TItem)x.NewValue) ?? -1);
                }
            });

            // Listen for IsSelectedChangedEvents and update the selection accordingly.
            EventHandler<RoutedEventArgs> isSelectedChangedHandler = (s, e) =>
            {
                var sender = s as TControl;

                if (sender != null)
                {
                    var items = itemsSelector(sender);

                    if (items != null)
                    {
                        var source = e.Source as TItem;
                        var selectable = e.Source as ISelectable;

                        if (selectable.IsSelected)
                        {
                            if (items.Contains(source))
                            {
                                sender.SetValue(selectedItem, source);
                            }
                        }
                        else if (selectable == sender.GetValue(selectedItem))
                        {
                            sender.SetValue(selectedItem, null);
                        }
                    }
                }
            };

            Selector.IsSelectedChangedEvent.AddClassHandler(typeof(TControl), isSelectedChangedHandler);

            // Provide methods to update the selection when items are added/removed.
            this.ItemAdded = (control, item) =>
            {
                var selectable = item as ISelectable;
                var styleable = item as IStyleable;

                if ((selectable != null && selectable.IsSelected) ||
                    (styleable != null && styleable.Classes.Contains("selected")))
                {
                    control.SetValue(selectedItem, item);
                }
            };

            this.ItemRemoved = (control, item) =>
            {
                if (item == control.GetValue(selectedItem))
                {
                    control.SetValue(selectedItem, null);
                }
            };
        }

        /// <summary>
        /// Should be called by a control when a new item is added.
        /// </summary>
        public Action<TControl, TItem> ItemAdded { get; }

        /// <summary>
        /// Should be called by the control when an item is removed.
        /// </summary>
        public Action<TControl, TItem> ItemRemoved { get; }
    }
}
