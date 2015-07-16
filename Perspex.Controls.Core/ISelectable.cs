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
    public interface ISelectable
    {
        /// <summary>
        /// Gets or sets the selected state of the object.
        /// </summary>
        bool IsSelected { get; set; }
    }
}
