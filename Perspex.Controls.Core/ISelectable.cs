// -----------------------------------------------------------------------
// <copyright file="ISelectable.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    /// <summary>
    /// Interface for objects that are selectable.
    /// </summary>
    /// <remarks>
    /// Controls such as <see cref="Selector"/> use this interface to indicate the selected
    /// control in a list. If changing the control's <see cref="IsSelected"/> property should
    /// update the selection in a <see cref="Selector"/> or equivalent, then the control should
    /// raise the <see cref="Selector.IsSelectedChangedEvent"/>.
    /// </remarks>
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets the selected state of the object.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
