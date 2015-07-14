// -----------------------------------------------------------------------
// <copyright file="NameScope.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    /// <summary>
    /// Defines a name scope for its contained logical children.
    /// </summary>
    public class NameScope : Decorator, INameScope
    {
        private NameTable nameTable = new NameTable();

        /// <summary>
        /// Returns an object tha thas the requested name.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <returns>The named object or null if the named object was not found.</returns>
        object INameScope.FindName(string name)
        {
            return this.nameTable.FindName(name);
        }

        /// <summary>
        /// Registers an object with the specified name in the name scope.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <param name="o">The object.</param>
        /// <exception cref="ArgumentException">
        /// An object with the same name has already been registered.
        /// </exception>
        void INameScope.RegisterName(string name, object o)
        {
            this.nameTable.RegisterName(name, o);
        }

        /// <summary>
        /// Unregisters the specified name in the name scope.
        /// </summary>
        /// <param name="name">The name of the object.</param>
        /// <exception cref="ArgumentException">
        /// The name does not exist in the name scope.
        /// </exception>
        void INameScope.UnregisterName(string name)
        {
            this.nameTable.UnregisterName(name);
        }
    }
}
