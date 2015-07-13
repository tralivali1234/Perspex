// -----------------------------------------------------------------------
// <copyright file="Ellipse.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Shapes
{
    using Perspex.Media;

    /// <summary>
    /// Draws an ellipse.
    /// </summary>
    public class Ellipse : Shape
    {
        private Geometry geometry;

        private Size geometrySize;

        /// <summary>
        /// Gets the <see cref="Geometry"/> of the <see cref="Shape"/> before transforms are
        /// applied.
        /// </summary>
        public override Geometry DefiningGeometry
        {
            get
            {
                if (this.geometry == null || this.geometrySize != this.Bounds.Size)
                {
                    var rect = new Rect(this.Bounds.Size).Deflate(this.StrokeThickness);
                    this.geometry = new EllipseGeometry(rect);
                    this.geometrySize = this.Bounds.Size;
                }

                return this.geometry;
            }
        }

        /// <summary>
        /// Measures the control.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The desired size of the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(this.StrokeThickness, this.StrokeThickness);
        }
    }
}
