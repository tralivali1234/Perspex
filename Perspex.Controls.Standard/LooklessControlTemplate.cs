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
    public class LooklessControlTemplate : FuncTemplate<ILooklessControl, IControl>, ILooklessControlTemplate
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LooklessControlTemplate"/> class.
        /// </summary>
        /// <param name="build">The build function.</param>
        public LooklessControlTemplate(Func<ILooklessControl, IControl> build)
            : base(build)
        {
        }
    }
}
