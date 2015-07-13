// -----------------------------------------------------------------------
// <copyright file="DataTemplate.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System;
    using System.Reflection;

    /// <summary>
    /// Builds a control for a piece of data.
    /// </summary>
    public class DataTemplate : IDataTemplate
    {
        /// <summary>
        /// The implementation of the <see cref="Match"/> method.
        /// </summary>
        private Func<object, bool> match;

        /// <summary>
        /// The implementation of the <see cref="Build"/> method.
        /// </summary>
        private Func<object, Control> build;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplate"/> class.
        /// </summary>
        /// <param name="type">The type of data which the data template matches.</param>
        /// <param name="build">
        /// A function which when passed an object of <paramref name="type"/> returns a control.
        /// </param>
        public DataTemplate(Type type, Func<object, Control> build)
            : this(o => IsInstance(o, type), build)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataTemplate"/> class.
        /// </summary>
        /// <param name="match">
        /// A function which determines whether the data template matches the specified data.
        /// </param>
        /// <param name="build">
        /// A function which returns a control for matching data.
        /// </param>
        public DataTemplate(Func<object, bool> match, Func<object, Control> build)
        {
            Contract.Requires<ArgumentNullException>(match != null);
            Contract.Requires<ArgumentNullException>(build != null);

            this.match = match;
            this.build = build;
        }

        /// <summary>
        /// Checks to see if this data template matches the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>
        /// True if the data template can build a control for the data, otherwise false.
        /// </returns>
        public bool Match(object data)
        {
            return this.match(data);
        }

        /// <summary>
        /// Builds a control for the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>The control.</returns>
        public Control Build(object data)
        {
            return this.build(data);
        }

        /// <summary>
        /// Determines of an object is of the specified type.
        /// </summary>
        /// <param name="o">The object.</param>
        /// <param name="t">The type.</param>
        /// <returns>
        /// True if <paramref name="o"/> is of type <paramref name="t"/>, otherwise false.
        /// </returns>
        private static bool IsInstance(object o, Type t)
        {
            return (o != null) ?
                t.GetTypeInfo().IsAssignableFrom(o.GetType().GetTypeInfo()) :
                false;
        }
    }
}
