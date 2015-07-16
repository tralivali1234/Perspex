// -----------------------------------------------------------------------
// <copyright file="LooklessControlTemplate.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using System;

    /// <summary>
    /// A template for a <see cref="LooklessControl"/>.
    /// </summary>
    public class LooklessControlTemplate : ILooklessControlTemplate
    {
        private Func<ILooklessControl, IControl> build;

        /// <summary>
        /// Initializes a new instance of the <see cref="LooklessControlTemplate"/> class.
        /// </summary>
        /// <param name="build">The build function.</param>
        public LooklessControlTemplate(Func<ILooklessControl, IControl> build)
        {
            Contract.Requires<ArgumentNullException>(build != null);

            this.build = build;
        }

        /// <summary>
        /// Builds the lookless control template.
        /// </summary>
        /// <param name="control">The lookless control.</param>
        /// <returns>The built control##</returns>
        public IControl Build(ILooklessControl control)
        {
            return this.build(control);
        }
    }
}
