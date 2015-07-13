// -----------------------------------------------------------------------
// <copyright file="ILooklessControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    /// <summary>
    /// Defines the interface for lookless controls.
    /// </summary>
    public interface ILooklessControl
    {
        /// <summary>
        /// Applies the control's <see cref="Template"/> if it is not already applied.
        /// </summary>
        void ApplyTemplate();

        /// <summary>
        /// Gets or sets the control's template.
        /// </summary>
        ILooklessControlTemplate Template { get; set; }
    }
}