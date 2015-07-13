// -----------------------------------------------------------------------
// <copyright file="IControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using Perspex.Input;
    using Perspex.Styling;

    /// <summary>
    /// Interface for Perspex controls.
    /// </summary>
    public interface IControl : IInputElement, INamed, ILogical, IStyleable, IStyleHost
    {
    }
}
