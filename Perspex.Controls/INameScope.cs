// -----------------------------------------------------------------------
// <copyright file="INameScope.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    /// <summary>
    /// Interface implemented by controls that define a name scope.
    /// </summary>
    public interface INameScope
    {
        /// <summary>
        /// Gets the name lookup for the name scope.
        /// </summary>
        NameDictionary Names { get; }
    }
}
