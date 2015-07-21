// -----------------------------------------------------------------------
// <copyright file="Selector.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Linq;
    using Perspex.Controls.Core.Mixins;
    using Perspex.Collections;
    using Perspex.Input;
    using Perspex.VisualTree;
    using Perspex.Interactivity;

    /// <summary>
    /// Hosts an <see cref="IPanel"/> whose children can be selected.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Selector control extends <see cref="Repeat"/> and makes the children of the
    /// <see cref="Panel"/> selectable. If the <see cref="IsUserSelectable"/> property is set
    /// (the default) then a child will become selected when it is clicked, or when it gains
    /// keyboard focus. The selected control will be marked in one of two ways:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// If the control implements <see cref="ISelectable"/> then its
    /// <see cref="ISelectable.IsSelected"/> property will be set.
    /// </item>
    /// <item>
    /// Otherwise, a "selected" class will be added to the selected child.
    /// </item>
    /// </list>
    /// <para>
    /// The Panel can be populated in one of two ways:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// Controls can be added manually.
    /// </item>
    /// <item>
    /// Controls can be created from the <see cref="Repeat.Items"/> collection based on an
    /// <see cref="Repeat.ItemTemplate"/> (see the base <see cref="Repeat"/> control's
    /// documentation for more information).
    /// </item>
    /// </list>
    /// <para>
    /// Though it is possible to manipulate the children of <see cref="Panel"/> when
    /// <see cref="Repeat.Items"/> are assigned, it is not recommended.
    /// </para>
    /// </remarks>
    public class Selector : Repeat
    {
        /// <summary>
        /// Defines the <see cref="IsUserSelectable"/> property.
        /// </summary>
        public static readonly PerspexProperty<bool> IsUserSelectableProperty =
            PerspexProperty.Register<Selector, bool>(nameof(IsUserSelectable), true);

        /// <summary>
        /// Defines the <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly PerspexProperty<int> SelectedIndexProperty =
            PerspexProperty.Register<Selector, int>(nameof(SelectedIndex), -1);

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> SelectedItemProperty =
            PerspexProperty.Register<Selector, IControl>(nameof(SelectedItem));

        /// <summary>
        /// Event that should be raised by items that implement <see cref="ISelectable"/> to
        /// notify the parent <see cref="Selector"/> that their selection state has changed.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent =
            RoutedEvent.Register<Selector, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

        private static readonly SelectorMixin<Selector, IControl> SelectorMixin;

        private IDisposable childSubscription;

        /// <summary>
        /// Initializes static members of the <see cref="Selector"/> class.
        /// </summary>
        static Selector()
        {
            SelectorMixin = new SelectorMixin<Selector, IControl>(
                SelectedIndexProperty,
                SelectedItemProperty,
                x => x.Panel?.Children);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the selection changes due to user interaction.
        /// </summary>
        public bool IsUserSelectable
        {
            get { return this.GetValue(IsUserSelectableProperty); }
            set { this.SetValue(IsUserSelectableProperty, value); }
        }

        /// <summary>
        /// Gets or sets the index of the child to be shown.
        /// </summary>
        /// <remarks>
        /// The valid range for this property value is from -1 (no selection) to
        /// <see cref="Children.Count"/> - 1. If an attempt is made to set the property
        /// to a value outside this range, then the selection will be cleared.
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
        /// <see cref="Children"/> collection then the selection will be cleared.
        /// </remarks>
        public IControl SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        /// <inheritdoc/>
        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);
            this.SelectItemFromEvent(e);
        }

        /// <inheritdoc/>
        protected override void OnPointerPressed(PointerPressEventArgs e)
        {
            base.OnPointerPressed(e);
            this.SelectItemFromEvent(e);
        }

        /// <summary>
        /// Called when the <see cref="Panel"/> changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void PanelChanged(PerspexPropertyChangedEventArgs e)
        {
            base.PanelChanged(e);

            var panel = e.NewValue as IPanel;

            if (this.childSubscription != null)
            {
                this.childSubscription.Dispose();
                this.SelectedItem = null;
            }

            if (panel != null)
            {
                this.childSubscription = panel.Children.ForEachItem(
                    x => SelectorMixin.ItemAdded(this, x),
                    x => SelectorMixin.ItemRemoved(this, x));
            }
        }

        /// <summary>
        /// Selects an item from an event.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void SelectItemFromEvent(RoutedEventArgs e)
        {
            if (this.IsUserSelectable &&
                !e.Handled &&
                this.Panel != null &&
                e.Source != this &&
                e.Source != this.Panel)
            {
                this.SelectedItem = (IControl)((IVisual)e.Source).GetSelfAndVisualAncestors()
                    .First(x => x.VisualParent == this.Panel);
            }
        }
    }
}
