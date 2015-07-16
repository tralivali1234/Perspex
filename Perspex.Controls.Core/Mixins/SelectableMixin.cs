// -----------------------------------------------------------------------
// <copyright file="SelectableMixin.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Mixins
{
    using Perspex.Interactivity;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Adds selectable functionality to control classes.
    /// </summary>
    /// <typeparam name="TControl">The control type.</typeparam>
    /// <remarks>
    /// <para>
    /// The <see cref="SelectableMixin{TControl}"/> adds behavior to a control which can be
    /// selected. It adds the following behavior:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// Raises an <see cref="Selector.IsSelectedChangedEvent"/> when the value if the IsSelected
    /// property changes.
    /// </item>
    /// <item>
    /// Adds a 'selected' class to selected controls.
    /// </item>
    /// </list>
    /// <para>
    /// Mixins apply themselves to classes and not instances, and as such should be created in
    /// a static constructor.
    /// </para>
    /// </remarks>
    public class SelectableMixin<TControl> where TControl : class, IControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectableMixin{TControl}"/> class.
        /// </summary>
        /// <param name="isSelected">The IsSelected property.</param>
        public SelectableMixin(PerspexProperty<bool> isSelected)
        {
            isSelected.Changed.Subscribe(x =>
            {
                var sender = x.Sender as TControl;

                if (sender != null)
                {
                    if ((bool)x.NewValue)
                    {
                        sender.Classes.Add("selected");
                    }
                    else
                    {
                        sender.Classes.Remove("selected");
                    }

                    sender.RaiseEvent(new RoutedEventArgs
                    {
                        RoutedEvent = Selector.IsSelectedChangedEvent
                    });
                }
            });
        }
    }
}
