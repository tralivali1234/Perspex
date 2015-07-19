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
    public class ItemsPresenter : Repeat, IPresenter
    {
        /// <summary>
        /// Requests that the visual children of the control use another control as their logical
        /// parent.
        /// </summary>
        /// <param name="logicalParent">
        /// The logical parent for the visual children of the control.
        /// </param>
        /// <param name="children">
        /// The <see cref="ILogical.LogicalChildren"/> collection to modify.
        /// </param>
        void IReparentingControl.ReparentLogicalChildren(ILogical logicalParent, IPerspexList<ILogical> children)
        {
            if (this.Panel == null)
            {
                throw new InvalidOperationException("ItemsPresenter.Panel may not be null.");
            }

            var reparenting = this.Panel as IReparentingControl;

            if (reparenting == null)
            {
                throw new InvalidOperationException("ItemsPresenter.Panel must implement IReparentingControl.");
            }

            reparenting.ReparentLogicalChildren(logicalParent, children);
        }
    }
}
