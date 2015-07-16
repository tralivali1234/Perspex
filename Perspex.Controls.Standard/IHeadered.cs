// -----------------------------------------------------------------------
// <copyright file="IHeadered.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    /// <summary>
    /// Interface for controls which have a header.
    /// </summary>
    public interface IHeadered
    {
        /// <summary>
        /// Gets the header.
        /// </summary>
        object Header { get; }
    }
}
