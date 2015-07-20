// -----------------------------------------------------------------------
// <copyright file="ItemsPresenter.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.Presenters
{
    using System;
    using Perspex.Collections;
    using Perspex.Controls.Core;

    /// <summary>
    /// Presents a collection of data inside a <see cref="LooklessControl"/> template.
    /// </summary>
    public class ItemsPresenter : Repeat, IItemsPresenter
    {
        /// <summary>
        /// Initializes static members of the <see cref="ItemsPresenter"/> class.
        /// </summary>
        static ItemsPresenter()
        {
            PanelProperty.Changed.AddClassHandler<ItemsPresenter>(x => x.PanelChanged);
        }

        /// <summary>
        /// Called when the <see cref="Panel"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void PanelChanged(PerspexPropertyChangedEventArgs e)
        {
            var panel = (IReparentingControl)e.NewValue;

            if (panel != null)
            {
                LooklessControl.SetIsPresenter(panel, true);
            }
        }
    }
}
