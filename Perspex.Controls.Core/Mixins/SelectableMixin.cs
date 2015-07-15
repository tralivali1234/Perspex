// -----------------------------------------------------------------------
// <copyright file="SelectableMixin.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Mixins
{
    /// <summary>
    /// Adds selectable functionality to control classes.
    /// </summary>
    public static class SelectableMixin
    {
        /// <summary>
        /// Attaches the mixin to the specified control type.
        /// </summary>
        /// <typeparam name="TControl">The control type.</typeparam>
        /// <typeparam name="TItem">The item type.</typeparam>
        /// <param name="selectedIndex">The SelectedIndex property.</param>
        /// <param name="selectedItem">The SelectedItem property.</param>
        public static void Attach<TControl, TItem>(
            PerspexProperty<int> selectedIndex,
            PerspexProperty<TItem> selectedItem)
            where TControl : IControl
        {
            //selectedIndex.AddCoercion<TControl>();
        }
    }
}
