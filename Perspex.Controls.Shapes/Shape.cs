// -----------------------------------------------------------------------
// <copyright file="Shape.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Shapes
{
    using System;
    using Perspex.Collections;
    using Perspex.Controls;
    using Perspex.Media;

    /// <summary>
    /// Base class for shape controls.
    /// </summary>
    public abstract class Shape : Control
    {
        /// <summary>
        /// Defines the <see cref="Fill"/> property.
        /// </summary>
        public static readonly PerspexProperty<Brush> FillProperty =
            PerspexProperty.Register<Shape, Brush>("Fill");

        /// <summary>
        /// Defines the <see cref="Stretch"/> property.
        /// </summary>
        public static readonly PerspexProperty<Stretch> StretchProperty =
            PerspexProperty.Register<Shape, Stretch>("Stretch");

        /// <summary>
        /// Defines the <see cref="Stroke"/> property.
        /// </summary>
        public static readonly PerspexProperty<Brush> StrokeProperty =
            PerspexProperty.Register<Shape, Brush>("Stroke");

        /// <summary>
        /// Defines the <see cref="StrokeDashArray"/> property.
        /// </summary>
        public static readonly PerspexProperty<PerspexList<double>> StrokeDashArrayProperty =
            PerspexProperty.Register<Shape, PerspexList<double>>("StrokeDashArray");

        /// <summary>
        /// Defines the <see cref="StrokeThickness"/> property.
        /// </summary>
        public static readonly PerspexProperty<double> StrokeThicknessProperty =
            PerspexProperty.Register<Shape, double>("StrokeThickness");

        private Matrix transform = Matrix.Identity;

        private Geometry renderedGeometry;

        /// <summary>
        /// Initializes static members of the <see cref="Shape"/> class.
        /// </summary>
        static Shape()
        {
            Control.AffectsRender(FillProperty);
            Control.AffectsMeasure(StretchProperty);
            Control.AffectsRender(StrokeProperty);
            Control.AffectsRender(StrokeDashArrayProperty);
            Control.AffectsMeasure(StrokeThicknessProperty);
        }

        /// <summary>
        /// Gets the <see cref="Geometry"/> of the <see cref="Shape"/> before transforms are
        /// applied.
        /// </summary>
        public abstract Geometry DefiningGeometry
        {
            get;
        }

        /// <summary>
        /// Gets or sets the brush used to fill the <see cref="Shape"/>.
        /// </summary>
        public Brush Fill
        {
            get { return this.GetValue(FillProperty); }
            set { this.SetValue(FillProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="Geometry"/> of the <see cref="Shape"/> with transforms applied.
        /// </summary>
        public Geometry RenderedGeometry
        {
            get
            {
                if (this.renderedGeometry == null)
                {
                    if (this.DefiningGeometry != null)
                    {
                        this.renderedGeometry = this.DefiningGeometry.Clone();
                        this.renderedGeometry.Transform = new MatrixTransform(this.transform);
                    }
                }

                return this.renderedGeometry;
            }
        }

        /// <summary>
        /// Gets or sets a value which describes how the <see cref="Shape"/> fills its allocated
        /// space.
        /// </summary>
        public Stretch Stretch
        {
            get { return this.GetValue(StretchProperty); }
            set { this.SetValue(StretchProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush used to draw the <see cref="Shape"/>'s outline.
        /// </summary>
        public Brush Stroke
        {
            get { return this.GetValue(StrokeProperty); }
            set { this.SetValue(StrokeProperty, value); }
        }

        /// <summary>
        /// Gets or sets a collection of values which describe the pattern of dashes and gaps that
        /// will be used to draw the <see cref="Shape"/>'s outline
        /// </summary>
        public PerspexList<double> StrokeDashArray
        {
            get { return this.GetValue(StrokeDashArrayProperty); }
            set { this.SetValue(StrokeDashArrayProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the <see cref="Shape"/>'s outline.
        /// </summary>
        public double StrokeThickness
        {
            get { return this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }

        /// <summary>
        /// Renders the control.
        /// </summary>
        /// <param name="context">The drawing context.</param>
        public override void Render(IDrawingContext context)
        {
            var geometry = this.RenderedGeometry;

            if (geometry != null)
            {
                var pen = new Pen(this.Stroke, this.StrokeThickness, this.StrokeDashArray);
                context.DrawGeometry(this.Fill, pen, geometry);
            }
        }

        /// <summary>
        /// Measures the control.
        /// </summary>
        /// <param name="availableSize">The available size.</param>
        /// <returns>The desired size of the control.</returns>
        protected override Size MeasureOverride(Size availableSize)
        {
            // This should probably use GetRenderBounds(strokeThickness) but then the calculations
            // will multiply the stroke thickness as well, which isn't correct.
            Rect shapeBounds = this.DefiningGeometry.Bounds;
            Size shapeSize = new Size(shapeBounds.Right, shapeBounds.Bottom);
            Matrix translate = Matrix.Identity;
            double width = this.Width;
            double height = this.Height;
            double desiredX = availableSize.Width;
            double desiredY = availableSize.Height;
            double sx = 0.0;
            double sy = 0.0;

            if (this.Stretch != Stretch.None)
            {
                shapeSize = shapeBounds.Size;
                translate = Matrix.Translation(-(Vector)shapeBounds.Position);
            }

            if (double.IsInfinity(availableSize.Width))
            {
                desiredX = shapeSize.Width;
            }

            if (double.IsInfinity(availableSize.Height))
            {
                desiredY = shapeSize.Height;
            }

            if (shapeBounds.Width > 0)
            {
                sx = desiredX / shapeSize.Width;
            }

            if (shapeBounds.Height > 0)
            {
                sy = desiredY / shapeSize.Height;
            }

            if (double.IsInfinity(availableSize.Width))
            {
                sx = sy;
            }

            if (double.IsInfinity(availableSize.Height))
            {
                sy = sx;
            }

            switch (this.Stretch)
            {
                case Stretch.Uniform:
                    sx = sy = Math.Min(sx, sy);
                    break;
                case Stretch.UniformToFill:
                    sx = sy = Math.Max(sx, sy);
                    break;
                case Stretch.Fill:
                    if (double.IsInfinity(availableSize.Width))
                    {
                        sx = 1.0;
                    }

                    if (double.IsInfinity(availableSize.Height))
                    {
                        sy = 1.0;
                    }

                    break;
                default:
                    sx = sy = 1;
                    break;
            }

            var t = translate * Matrix.Scaling(sx, sy);

            if (this.transform != t)
            {
                this.transform = t;
                this.renderedGeometry = null;
            }

            return new Size(shapeSize.Width * sx, shapeSize.Height * sy);
        }
    }
}
