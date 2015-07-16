// -----------------------------------------------------------------------
// <copyright file="Selector.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Collections.Specialized;
    using Perspex.Controls.Core.Mixins;
    using Perspex.Collections;

    /// <summary>
    /// Hosts an <see cref="IPanel"/> whose children can be selected.
    /// </summary>
    /// <remarks>
    /// The Selector control hosts a <see cref="Panel"/> and makes the children of the panel
    /// selectable. A child will become selected when it is clicked, or when it gains keyboard
    /// focus. The selected control will be marked in one of two ways:
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
    public class Selector : Control
    {
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
            PanelProperty.Changed.AddClassHandler<Selector>(x => x.PanelChanged);
            SelectedItemProperty.Changed.AddClassHandler<Selector>(x => x.SelectedItemChanged);
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
            }

            if (panel != null)
            {
                this.childSubscription = panel.Children.ForEachItem(
                    this.ChildAdded,
                    this.ChildRemoved);
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
