// -----------------------------------------------------------------------
// <copyright file="INamed.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    /// <summary>
    /// Defines a named object.
    /// </summary>
    public interface INamed
    {
        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        string Name { get; }
    }
}
