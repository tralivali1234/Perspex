// -----------------------------------------------------------------------
// <copyright file="RepeatPresenter.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.Presenters
{
    using Perspex.Controls.Core;

    /// <summary>
    /// Presents a collection of data inside a <see cref="LooklessControl"/> template using a
    /// <see cref="Repeat"/> control.
    /// </summary>
    public class RepeatPresenter : Repeat, IItemsPresenter
    {
        /// <summary>
        /// Defines the <see cref="ItemsPanel"/> property.
        /// </summary>
        public static readonly PerspexProperty<ITemplate<IPanel>> ItemsPanelProperty =
            ItemsControl.ItemsPanelProperty.AddOwner<RepeatPresenter>();

        /// <summary>
        /// Initializes static members of the <see cref="RepeatPresenter"/> class.
        /// </summary>
        static RepeatPresenter()
        {
            ItemsPanelProperty.OverrideDefaultValue<RepeatPresenter>(null);
            ItemsPanelProperty.Changed.AddClassHandler<RepeatPresenter>(x => x.ItemsPanelChanged);
        }

        /// <summary>
        /// Gets or sets the items to be displayed.
        /// </summary>
        public ITemplate<IPanel> ItemsPanel
        {
            get { return this.GetValue(ItemsPanelProperty); }
            set { this.SetValue(ItemsPanelProperty, value); }
        }

        /// <summary>
        /// Called when the <see cref="Panel"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        protected override void PanelChanged(PerspexPropertyChangedEventArgs e)
        {
            base.PanelChanged(e);

            var panel = (IReparentingControl)e.NewValue;

            if (panel != null)
            {
                LooklessControl.SetIsPresenter(panel, true);
            }
        }

        /// <summary>
        /// Called when the <see cref="ItemsPanel"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ItemsPanelChanged(PerspexPropertyChangedEventArgs e)
        {
            this.Panel = ((ITemplate<IPanel>)e.NewValue).Build();
        }
    }
}
