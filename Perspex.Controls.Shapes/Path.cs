// -----------------------------------------------------------------------
// <copyright file="Path.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Shapes
{
    using Perspex.Media;

    /// <summary>
    /// Draws a series of connected lines and curves.
    /// </summary>
    public class Path : Shape
    {
        /// <summary>
        /// Defines the <see cref="Data"/> property.
        /// </summary>
        public static readonly PerspexProperty<Geometry> DataProperty =
            PerspexProperty.Register<Path, Geometry>("Data");

        /// <summary>
        /// Gets or sets a <see cref="Geometry"/> that specifies the shape to be drawn.
        /// </summary>
        public Geometry Data
        {
            get { return this.GetValue(DataProperty); }
            set { this.SetValue(DataProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="Geometry"/> of the <see cref="Shape"/> before transforms are
        /// applied.
        /// </summary>
        public override Geometry DefiningGeometry
        {
            get { return this.Data; }
        }
    }
}
