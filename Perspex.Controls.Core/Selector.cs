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
    /// The Selector control hosts a <see cref="Panel"/> and makes the children of the panel
    /// selectable. If the <see cref="IsUserSelectable"/> property is set (the default) then a
    /// child will become selected when it is clicked, or when it gains keyboard focus. The
    /// selected control will be marked in one of two ways:
    ///
    /// <list type="bullet">
    ///     <item>
    ///         If the control implements <see cref="ISelectable"/> then its
    ///         <see cref="ISelectable.IsSelected"/> property will be set.
    ///     </item>
    ///     <item>
    ///         Otherwise, a "selected" class will be added to the selected child.
    ///     </item>
    /// </list>
    /// </remarks>
    public class Selector : Control, ILogical
    {
        /// <summary>
        /// Defines the <see cref="IsUserSelectable"/> property.
        /// </summary>
        public static readonly PerspexProperty<bool> IsUserSelectableProperty =
            PerspexProperty.Register<Selector, bool>("IsUserSelectable", true);

        /// <summary>
        /// Defines the <see cref="Panel"/> property.
        /// </summary>
        public static readonly PerspexProperty<IPanel> PanelProperty =
            PerspexProperty.Register<Selector, IPanel>("Panel");

        /// <summary>
        /// Defines the <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly PerspexProperty<int> SelectedIndexProperty =
            PerspexProperty.Register<Selector, int>("SelectedIndex", -1);

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> SelectedItemProperty =
            PerspexProperty.Register<Selector, IControl>("SelectedItem");

        /// <summary>
        /// Event that should be raised by items that implement <see cref="ISelectable"/> to
        /// notify the parent <see cref="Selector"/> that their selection state has changed.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent =
            RoutedEvent.Register<Selector, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

        private PerspexSingleItemList<ILogical> logicalChild = new PerspexSingleItemList<ILogical>();

        private IDisposable childSubscription;

        /// <summary>
        /// Initializes static members of the <see cref="Selector"/> class.
        /// </summary>
        static Selector()
        {
            SelectableMixin.Attach<Selector, IControl>(
                SelectedIndexProperty,
                SelectedItemProperty,
                x => x.Panel?.Children);

            IsSelectedChangedEvent.AddClassHandler<Selector>(x => x.ItemIsSelectedChanged);
            PanelProperty.Changed.AddClassHandler<Selector>(x => x.PanelChanged);
            SelectedItemProperty.Changed.AddClassHandler<Selector>(x => x.SelectedItemChanged);
        }

        /// <summary>
        /// Gets or sets indicating whether the selection changes due to user interaction.
        /// </summary>
        public bool IsUserSelectable
        {
            get { return this.GetValue(IsUserSelectableProperty); }
            set { this.SetValue(IsUserSelectableProperty, value); }
        }

        /// <summary>
        /// Gets or sets the panel containing the selectable controls.
        /// </summary>
        public IPanel Panel
        {
            get { return this.GetValue(PanelProperty); }
            set { this.SetValue(PanelProperty, value); }
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

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return this.logicalChild; }
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
        protected void PanelChanged(PerspexPropertyChangedEventArgs e)
        {
            var panel = e.NewValue as IPanel;

            if (this.childSubscription != null)
            {
                this.childSubscription.Dispose();
                this.SelectedItem = null;
                this.ClearVisualChildren();
                this.logicalChild.SingleItem = null;
            }

            if (panel != null)
            {
                this.childSubscription = panel.Children.ForEachItem(
                    this.ChildAdded,
                    this.ChildRemoved);
                this.AddVisualChild(panel);
                this.logicalChild.SingleItem = panel;
            }
        }

        /// <summary>
        /// Called when a child is added to the <see cref="Panel"/>.
        /// </summary>
        /// <param name="child">The child.</param>
        private void ChildAdded(IControl child)
        {
            var selectable = child as ISelectable;

            if ((selectable != null && selectable.IsSelected) ||
                child.Classes.Contains("selected"))
            {
                this.SelectedItem = child;
            }
        }

        /// <summary>
        /// Called when a child is removed from the <see cref="Panel"/>.
        /// </summary>
        /// <param name="child">The child.</param>
        private void ChildRemoved(IControl child)
        {
            if (child == this.SelectedItem)
            {
                this.SelectedItem = null;
            }
        }

        /// <summary>
        /// Called when the selection on a child item changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ItemIsSelectedChanged(RoutedEventArgs e)
        {
            var source = e.Source as IControl;
            var selectable = e.Source as ISelectable;

            if (selectable.IsSelected)
            {
                if (this.Panel.Children.Contains(source))
                {
                    this.SelectedItem = source;
                }
            }
            else if (selectable == this.SelectedItem)
            {
                this.SelectedItem = null;
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

        /// <summary>
        /// Called when the <see cref="SelectedItem"/> changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void SelectedItemChanged(PerspexPropertyChangedEventArgs e)
        {
            if (this.Panel != null && this.Panel.Children != null)
            {
                var child = (IControl)e.OldValue;
                var selectable = child as ISelectable;

                if (selectable != null)
                {
                    selectable.IsSelected = false;
                }
                else if (child != null)
                {
                    child.Classes.Remove("selected");
                }

                child = (IControl)e.NewValue;
                selectable = child as ISelectable;

                if (selectable != null)
                {
                    selectable.IsSelected = true;
                }
                else if(child != null)
                {
                    child.Classes.Add("selected");
                }
            }
        }
    }
}
