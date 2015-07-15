// -----------------------------------------------------------------------
// <copyright file="IPopupImpl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Windowing.Platform
{
    using System;
    using Perspex.Controls;
    using Perspex.Input.Raw;

    public interface IPopupImpl : ITopLevelImpl
    {
        void SetPosition(Point p);

        void Show();

        void Hide();
    }
}
