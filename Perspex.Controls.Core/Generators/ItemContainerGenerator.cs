// -----------------------------------------------------------------------
// <copyright file="ItemContainerGenerator.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.Generators
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.Controls.Core.Templates;

    /// <summary>
    /// Creates containers for items and maintains a list of created containers.
    /// </summary>
    public class ItemContainerGenerator : IItemContainerGenerator
    {
        private IControl owner;

        private Dictionary<int, IControl> containers = new Dictionary<int, IControl>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator"/> class.
        /// </summary>
        /// <param name="owner">The owner control.</param>
        public ItemContainerGenerator(IControl owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Clears the created containers from the index and returns the removed controls.
        /// </summary>
        /// <returns>The removed controls.</returns>
        public IEnumerable<IControl> ClearContainers()
        {
            var result = this.containers;
            this.containers = new Dictionary<int, IControl>();
            return result.Values;
        }

        /// <summary>
        /// Creates container controls for a collection of items.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item of the data in the containing collection.
        /// </param>
        /// <param name="items">The items.</param>
        /// <param name="itemTemplate">An optional item template.</param>
        /// <returns>The created container controls.</returns>
        public IEnumerable<IControl> CreateContainers(
            int startingIndex,
            IEnumerable items,
            IDataTemplate itemTemplate)
        {
            Contract.Requires<ArgumentNullException>(items != null);

            int index = startingIndex;
            var result = new List<IControl>();

            foreach (var item in items)
            {
                IControl container = this.CreateContainer(item, itemTemplate);
                result.Add(container);
            }

            this.AddContainers(startingIndex, result);
            return result.Where(x => x != null);
        }

        /// <summary>
        /// Removes a set of created containers from the index and returns the removed controls.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item of the data in the containing collection.
        /// </param>
        /// <param name="count">The number of items to remove.</param>
        /// <returns>The removed controls.</returns>
        public IEnumerable<IControl> RemoveContainers(int startingIndex, int count)
        {
            var result = new List<IControl>();

            for (int i = startingIndex; i < startingIndex + count; ++i)
            {
                var container = this.containers[i];

                if (container == null)
                {
                    throw new InvalidOperationException("Container not created.");
                }

                result.Add(container);
                this.containers[i] = null;
            }

            return result.Where(x => x != null);
        }

        /// <summary>
        /// Creates the container for an item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="itemTemplate">An optional item template.</param>
        /// <returns>The created container control.</returns>
        protected virtual IControl CreateContainer(object item, IDataTemplate itemTemplate)
        {
            if (item == null)
            {
                return null;
            }
            else if (itemTemplate != null && itemTemplate.Match(item))
            {
                var result = itemTemplate.Build(item);
                result.DataContext = item;
                return result;
            }
            else
            {
                return this.owner.MaterializeDataTemplate(item);
            }
        }

        /// <summary>
        /// Adds a collection of containers to the index.
        /// </summary>
        /// <param name="index">The starting index.</param>
        /// <param name="container">The container.</param>
        protected void AddContainers(int index, IList<IControl> container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            foreach (var c in container)
            {
                if (!this.containers.ContainsKey(index))
                {
                    this.containers[index] = c;
                }
                else
                {
                    throw new InvalidOperationException("Container already created.");
                }

                ++index;
            }
        }
    }
}
