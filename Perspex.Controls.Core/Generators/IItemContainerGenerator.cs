// -----------------------------------------------------------------------
// <copyright file="IItemContainerGenerator.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Generators
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Creates containers for items and maintains a list of created containers.
    /// </summary>
    public interface IItemContainerGenerator
    {
        /// <summary>
        /// Clears the created containers from the index and returns the removed controls.
        /// </summary>
        /// <returns>The removed controls.</returns>
        IEnumerable<IControl> ClearContainers();

        /// <summary>
        /// Creates container controls for a collection of items.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item of the data in the containing collection.
        /// </param>
        /// <param name="items">The items.</param>
        /// <param name="itemTemplate">An optional item template.</param>
        /// <returns>The created controls.</returns>
        IEnumerable<IControl> CreateContainers(
            int startingIndex,
            IEnumerable items,
            IDataTemplate itemTemplate);

        /// <summary>
        /// Removes a set of created containers from the index and returns the removed controls.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item of the data in the containing collection.
        /// </param>
        /// <param name="count">The number of items to remove.</param>
        /// <returns>The removed controls.</returns>
        IEnumerable<IControl> RemoveContainers(int startingIndex, int count);
    }
}
