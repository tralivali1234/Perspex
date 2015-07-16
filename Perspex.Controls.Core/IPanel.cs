// -----------------------------------------------------------------------
// <copyright file="IPanel.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    /// <summary>
    /// Represents a control that can contain multiple children.
    /// </summary>
    public interface IPanel : IControl
    {
        /// <summary>
        /// Gets the children of the <see cref="IPanel"/>.
        /// </summary>
        Controls Children { get; }
    }
}