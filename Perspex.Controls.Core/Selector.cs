// -----------------------------------------------------------------------
// <copyright file="Selector.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.Controls.Core.Mixins;
    using Perspex.Input;
    using Perspex.VisualTree;
    using Perspex.Interactivity;
    using Perspex.Styling;

    /// <summary>
    /// Displays a selectable collection of data.
    /// </summary>
    /// <remarks>
    /// The <see cref="Selector"/> control extends the <see cref="Repeat"/> control to
    /// provide selection behavior.
    /// </remarks>
    public class Selector : Repeat
    {
        /// <summary>
        /// Defines the <see cref="IsUserSelectable"/> property.
        /// </summary>
        public static readonly PerspexProperty<bool> IsUserSelectableProperty =
            PerspexProperty.Register<Selector, bool>(nameof(IsUserSelectable), true);

        /// <summary>
        /// Defines the <see cref="SelectedContainer"/> property.
        /// </summary>
        public static readonly PerspexProperty<IControl> SelectedContainerProperty =
            PerspexProperty.Register<Selector, IControl>(nameof(SelectedContainer));

        /// <summary>
        /// Defines the <see cref="SelectedIndex"/> property.
        /// </summary>
        public static readonly PerspexProperty<int> SelectedIndexProperty =
            PerspexProperty.Register<Selector, int>(nameof(SelectedIndex), -1);

        /// <summary>
        /// Defines the <see cref="SelectedItem"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> SelectedItemProperty =
            PerspexProperty.Register<Selector, object>(nameof(SelectedItem));

        /// <summary>
        /// Event that should be raised by controls that implement <see cref="ISelectable"/> to
        /// notify the parent <see cref="Selector"/> that their selection state has changed.
        /// </summary>
        public static readonly RoutedEvent<RoutedEventArgs> IsSelectedChangedEvent =
            RoutedEvent.Register<Selector, RoutedEventArgs>("IsSelectedChanged", RoutingStrategies.Bubble);

        private static readonly SelectorMixin<Selector, object> SelectorMixin;

        /// <summary>
        /// Initializes static members of the <see cref="Selector"/> class.
        /// </summary>
        static Selector()
        {
            SelectorMixin = new SelectorMixin<Selector, object>(
                SelectedIndexProperty,
                SelectedItemProperty,
                x => x.Items);

            SelectedContainerProperty.Changed.AddClassHandler<Selector>(x => x.SelectedContainerChanged);
            SelectedIndexProperty.Changed.AddClassHandler<Selector>(x => x.SelectedIndexChanged);
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
        /// <remarks>
        /// The valid range for this property value is from -1 (no selection) to
        /// <see cref="Items.Count"/> - 1. If an attempt is made to set the property
        /// to a value outside this range, then the selection will be cleared.
        /// </remarks>
        public int SelectedIndex
        {
            get { return this.GetValue(SelectedIndexProperty); }
            set { this.SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        /// <remarks>
        /// If an attempt is made to set the property to a object not contained in the
        /// <see cref="Children"/> collection then the selection will be cleared.
        /// </remarks>
        public object SelectedItem
        {
            get { return this.GetValue(SelectedItemProperty); }
            set { this.SetValue(SelectedItemProperty, value); }
        }

        /// <inheritdoc/>
        protected override void ContainersAdded(IList<IControl> containers)
        {
            foreach (var selectable in containers.OfType<ISelectable>())
            {
                if (selectable.IsSelected)
                {
                    this.SelectedContainer = (IControl)selectable;
                }
            }
        }

        /// <inheritdoc/>
        protected override void ContainersRemoved(IList<IControl> containers)
        {
            var selected = this.SelectedContainer;

            if (selected != null && containers.Contains(selected))
            {
                this.SelectedIndex = -1;
            }
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
            this.SelectedIndex = -1;
        }

        /// <summary>
        /// Sets a container's 'selected' class or <see cref="ISelectable.IsSelected"/>.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="selected">Whether the control is selected</param>
        private void MarkContainerSelected(IControl container, bool selected)
        {
            var selectable = container as ISelectable;
            var styleable = container as IStyleable;

            if (selectable != null)
            {
                selectable.IsSelected = selected;
            }
            else if (styleable != null)
            {
                if (selected)
                {
                    styleable.Classes.Add("selected");
                }
                else
                {
                    styleable.Classes.Remove("selected");
                }
            }
        }

        /// <summary>
        /// Called when the <see cref="SelectedContainer"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void SelectedContainerChanged(PerspexPropertyChangedEventArgs e)
        {
            var container = (IControl)e.NewValue;

            this.MarkContainerSelected((IControl)e.OldValue, false);

            if (container != null)
            {
                this.MarkContainerSelected(container, true);
                this.SelectedIndex = this.ItemContainerGenerator.IndexFromContainer(container);
            }
            else
            {
                this.SelectedIndex = -1;
            }
        }

        /// <summary>
        /// Called when the <see cref="SelectedIndex"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void SelectedIndexChanged(PerspexPropertyChangedEventArgs e)
        {
            var index = (int)e.NewValue;

            if (index != -1)
            {
                var container = this.ItemContainerGenerator.ContainerFromIndex(index);
                this.SelectedContainer = container;
            }
            else
            {
                this.SelectedContainer = null;
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
                this.SelectedContainer = (IControl)((IVisual)e.Source).GetSelfAndVisualAncestors()
                    .First(x => x.VisualParent == this.Panel);
                e.Handled = true;
            }
        }
    }
}
