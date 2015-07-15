// -----------------------------------------------------------------------
// <copyright file="ITopLevelImpl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Windowing.Platform
{
    using System;
    using Perspex.Input.Raw;
    using Perspex.Platform;

    public interface ITopLevelImpl : IDisposable
    {
        Size ClientSize { get; set; }

        IPlatformHandle Handle { get; }

        Action Activated { get; set; }

        Action Closed { get; set; }

        Action Deactivated { get; set; }

        Action<RawInputEventArgs> Input { get; set; }

        Action<Rect, IPlatformHandle> Paint { get; set; }

        Action<Size> Resized { get; set; }

        void Activate();

        void Invalidate(Rect rect);

        void SetOwner(TopLevel owner);

        Point PointToScreen(Point point);
    }
}
