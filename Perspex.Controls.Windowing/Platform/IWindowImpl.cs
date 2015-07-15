// -----------------------------------------------------------------------
// <copyright file="IWindowImpl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Windowing.Platform
{
    using System;

    public interface IWindowImpl : ITopLevelImpl
    {
        void SetTitle(string title);

        void Show();

        IDisposable ShowDialog();

        void Hide();
    }
}
