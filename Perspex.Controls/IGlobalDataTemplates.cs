// -----------------------------------------------------------------------
// <copyright file="IGlobalDataTemplates.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    /// <summary>
    /// Defines an object that provides data templates for the whole application.
    /// </summary>
    public interface IGlobalDataTemplates
    {
        /// <summary>
        /// Gets the globals data templates.
        /// </summary>
        DataTemplates DataTemplates { get; }
    }
}
