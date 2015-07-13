// -----------------------------------------------------------------------
// <copyright file="NameDictionary.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Reactive.Disposables;

    /// <summary>
    /// Stores a mapping of names to <see cref="INamed"/> objects.
    /// </summary>
    public class NameDictionary : IEnumerable<KeyValuePair<string, INamed>>
    {
        private Dictionary<string, INamed> inner = new Dictionary<string, INamed>();

        /// <summary>
        /// Gets the named object from the dictionary.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>
        /// The named object.  If the specified key is not found, a throws 
        /// <see cref="KeyNotFoundException"/>.
        /// </returns>
        public INamed this[string name]
        {
            get { return this.inner[name]; }
        }

        /// <summary>
        /// Tries to get the named object from the dictionary.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>
        /// The named object or null if the name doesn't exist in the dictionary.
        /// </returns>
        public INamed Find(string name)
        {
            INamed result;
            this.inner.TryGetValue(name, out result);
            return result;
        }

        /// <summary>
        /// Returns an enumerator which iterates through the entries.
        /// </summary>
        /// <returns>An enumerator of type <see cref="KeyValuePair{string, INamed}"/>.</returns>
        public IEnumerator<KeyValuePair<string, INamed>> GetEnumerator()
        {
            return this.inner.GetEnumerator();
        }

        /// <summary>
        /// Registers a named object in the name dictionary.
        /// </summary>
        /// <param name="o">The named object.</param>
        public void Register(INamed o)
        {
            Contract.Requires<ArgumentNullException>(o != null);
            Contract.Requires<InvalidOperationException>(!string.IsNullOrWhiteSpace(o.Name));

            if (this.inner.ContainsKey(o.Name))
            {
                throw new InvalidOperationException(
                    $"A control with the name '{o.Name}' is already registered in this name scope.");
            }

            this.inner.Add(o.Name, o);
        }

        /// <summary>
        /// Deregisters a named object in the name dictionary.
        /// </summary>
        /// <param name="o">The named object.</param>
        public void Deregister(INamed o)
        {
            Contract.Requires<ArgumentNullException>(o != null);
            Contract.Requires<InvalidOperationException>(!string.IsNullOrWhiteSpace(o.Name));

            INamed existing;

            if (!this.inner.TryGetValue(o.Name, out existing) || existing != o)
            {
                throw new InvalidOperationException(
                    "The control is not currently registered in this name scope.");
            }

            this.inner.Remove(o.Name);
        }

        /// <summary>
        /// Returns an enumerator which iterates through the entries.
        /// </summary>
        /// <returns>An enumerator of type <see cref="KeyValuePair{string, INamed}"/>.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
