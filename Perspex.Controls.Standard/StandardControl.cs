// -----------------------------------------------------------------------
// <copyright file="StandardControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using Perspex.Controls.Core;
    using Perspex.Media;

    /// <summary>
    /// Base class for standard lookless controls.
    /// </summary>
    /// <remarks>
    /// Extends <see cref="LooklessControl"/> with common properties for things such as
    /// backgrounds, borders and fonts/
    /// </remarks>
    public class StandardControl : LooklessControl
    {
        /// <summary>
        /// Defines the <see cref="Background"/> property.
        /// </summary>
        public static readonly PerspexProperty<Brush> BackgroundProperty =
            Border.BackgroundProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="BorderBrush"/> property.
        /// </summary>
        public static readonly PerspexProperty<Brush> BorderBrushProperty =
            Border.BorderBrushProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="BorderThickness"/> property.
        /// </summary>
        public static readonly PerspexProperty<double> BorderThicknessProperty =
            Border.BorderThicknessProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="FontFamily"/> property.
        /// </summary>
        public static readonly PerspexProperty<string> FontFamilyProperty =
            TextBlock.FontFamilyProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="FontSize"/> property.
        /// </summary>
        public static readonly PerspexProperty<double> FontSizeProperty =
            TextBlock.FontSizeProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="FontStyle"/> property.
        /// </summary>
        public static readonly PerspexProperty<FontStyle> FontStyleProperty =
            TextBlock.FontStyleProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="FontWeight"/> property.
        /// </summary>
        public static readonly PerspexProperty<FontWeight> FontWeightProperty =
            TextBlock.FontWeightProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="Foreground"/> property.
        /// </summary>
        public static readonly PerspexProperty<Brush> ForegroundProperty =
            TextBlock.ForegroundProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Defines the <see cref="Padding"/> property.
        /// </summary>
        public static readonly PerspexProperty<Thickness> PaddingProperty =
            Border.PaddingProperty.AddOwner<StandardControl>();

        /// <summary>
        /// Gets or sets the brush used to paint the control's background.
        /// </summary>
        public Brush Background
        {
            get { return this.GetValue(BackgroundProperty); }
            set { this.SetValue(BackgroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the brush used to paint the control's border.
        /// </summary>
        public Brush BorderBrush
        {
            get { return this.GetValue(BorderBrushProperty); }
            set { this.SetValue(BorderBrushProperty, value); }
        }

        /// <summary>
        /// Gets or sets the thickness of the control's border.
        /// </summary>
        public double BorderThickness
        {
            get { return this.GetValue(BorderThicknessProperty); }
            set { this.SetValue(BorderThicknessProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font family.
        /// </summary>
        public string FontFamily
        {
            get { return this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font size.
        /// </summary>
        public double FontSize
        {
            get { return this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font style.
        /// </summary>
        public FontStyle FontStyle
        {
            get { return this.GetValue(FontStyleProperty); }
            set { this.SetValue(FontStyleProperty, value); }
        }

        /// <summary>
        /// Gets or sets the font weight.
        /// </summary>
        public FontWeight FontWeight
        {
            get { return this.GetValue(FontWeightProperty); }
            set { this.SetValue(FontWeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets a brush used to paint text.
        /// </summary>
        public Brush Foreground
        {
            get { return this.GetValue(ForegroundProperty); }
            set { this.SetValue(ForegroundProperty, value); }
        }

        /// <summary>
        /// Gets or sets the padding to place around the control's content.
        /// </summary>
        public Thickness Padding
        {
            get { return this.GetValue(PaddingProperty); }
            set { this.SetValue(PaddingProperty, value); }
        }
    }
}
