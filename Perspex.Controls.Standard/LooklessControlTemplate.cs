// -----------------------------------------------------------------------
// <copyright file="LooklessControlTemplate.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using System;

    public class LooklessControlTemplate : ILooklessControlTemplate
    {
        private Func<ILooklessControl, IControl> build;

        public LooklessControlTemplate(Func<ILooklessControl, IControl> build)
        {
            Contract.Requires<NullReferenceException>(build != null);

            this.build = build;
        }

        public IControl Build(ILooklessControl control)
        {
            return this.build(control);
        }
    }
}
