// -----------------------------------------------------------------------
// <copyright file="IItemsPresenter.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard.Presenters
{
    using Perspex.Controls.Core;

    /// <summary>
    /// Presents a collection of data inside a <see cref="LooklessControl"/> template.
    /// </summary>
    public interface IItemsPresenter : IControl
    {
        /// <summary>
        /// Gets the panel that acts as the parent of the items.
        /// </summary>
        IPanel Panel { get; }
    }
}
