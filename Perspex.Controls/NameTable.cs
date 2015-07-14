// -----------------------------------------------------------------------
// <copyright file="NameTable.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines a name scope lookup table.
    /// </summary>
    public class NameTable : INameScope
    {
        private Dictionary<string, object> inner = new Dictionary<string, object>();

        /// <summary>
        /// Returns an object tha thas the requested name.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>The named object or null if the named object was not found.</returns>
        public object FindName(string name)
        {
            object result;
            this.inner.TryGetValue(name, out result);
            return result;
        }

        /// <summary>
        /// Registers an object with the specified name in the name scope.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="o">The object.</param>
        /// <exception cref="ArgumentException">
        /// An object with the same name has already been registered.
        /// </exception>
        public void RegisterName(string name, object o)
        {
            if (this.inner.ContainsKey(name))
            {
                throw new ArgumentException(
                    $"An object with the name '{name}' is already registered in this name scope.");
            }

            this.inner.Add(name, o);
        }

        /// <summary>
        /// Unregisters the specified name in the name scope.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <exception cref="ArgumentException">
        /// The name does not exist in the name scope.
        /// </exception>
        public void UnregisterName(string name)
        {
            if (!this.inner.Remove(name))
            {
                throw new ArgumentException(
                    $"No object with the name '{name}' is registered in this name scope.");
            }
        }
    }
}
