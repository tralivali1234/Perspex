// -----------------------------------------------------------------------
// <copyright file="ILooklessControlTemplate.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    /// <summary>
    /// Interface representing a template used to build a <see cref="LooklessControl"/>.
    /// </summary>
    public interface ILooklessControlTemplate
    {
        /// <summary>
        /// Builds the lookless control template.
        /// </summary>
        /// <param name="control">The lookless control.</param>
        /// <returns>The built control##</returns>
        IControl Build(ILooklessControl control);
    }
}