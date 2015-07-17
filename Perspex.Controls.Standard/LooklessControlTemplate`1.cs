// -----------------------------------------------------------------------
// <copyright file="LooklessControlTemplate`1.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using System;

    /// <summary>
    /// A template for a <see cref="LooklessControl"/>.
    /// </summary>
    /// <typeparam name="T">The type of the lookless control.</typeparam>
    public class LooklessControlTemplate<T> : LooklessControlTemplate where T : ILooklessControl
    {
        private Func<ILooklessControl, IControl> build;

        /// <summary>
        /// Initializes a new instance of the <see cref="LooklessControlTemplate{T}"/> class.
        /// </summary>
        /// <param name="build">The build function.</param>
        public LooklessControlTemplate(Func<T, IControl> build)
            : base(x => build((T)x))
        {
        }
    }
}
