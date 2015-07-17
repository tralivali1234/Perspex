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
        private IControl control;

        private List<IControl> containers = new List<IControl>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ItemContainerGenerator"/> class.
        /// </summary>
        /// <param name="control">The owner control.</param>
        public ItemContainerGenerator(IControl control)
        {
            this.control = control;
        }

        /// <summary>
        /// Clears the created containers from the index and returns the removed controls.
        /// </summary>
        /// <returns>The removed controls.</returns>
        public IEnumerable<IControl> ClearContainers()
        {
            var result = this.containers;
            this.containers = new List<IControl>();
            return result;
        }

        /// <summary>
        /// Creates container controls for a collection of items.
        /// </summary>
        /// <param name="startingIndex">
        /// The index of the first item of the data in the containing collection.
        /// </param>
        /// <param name="items">The items.</param>
        /// <param name="itemTemplate">An optional item template.</param>
        /// <returns>The created controls.</returns>
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
                IControl container;

                if (itemTemplate != null && itemTemplate.Match(item))
                {
                    container = itemTemplate.Build(item);
                    container.DataContext = item;
                }
                else
                {
                    container = this.control.MaterializeDataTemplate(item);
                }

                result.Add(container);
            }

            this.AddContainers(startingIndex, result);
            return result;
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

            return result;
        }

        /// <summary>
        /// Adds a collection of containers to the index.
        /// </summary>
        /// <param name="index">The starting index.</param>
        /// <param name="container">The container.</param>
        protected void AddContainers(int index, IList<IControl> container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            int lastIndex = index + container.Count;

            if (lastIndex >= this.containers.Count)
            {
                this.containers.AddRange(Enumerable.Repeat<IControl>(null, lastIndex - this.containers.Count));
            }

            foreach (var c in container)
            {
                if (this.containers[index] == null)
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
