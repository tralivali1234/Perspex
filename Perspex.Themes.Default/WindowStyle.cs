// -----------------------------------------------------------------------
// <copyright file="WindowStyle.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Themes.Default
{
    using System.Linq;
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard.Presenters;
    using Perspex.Controls.Windowing;
    using Perspex.Styling;
    using Perspex.Controls.Standard;

    /// <summary>
    /// Defines the default style for the <see cref="Window"/> class.
    /// </summary>
    public class WindowStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowStyle"/> class.
        /// </summary>
        public WindowStyle()
        {
            this.AddRange(new[]
            {
                new Style(x => x.OfType<Window>())
                {
                    Setters = new[]
                    {
                        new Setter(Window.TemplateProperty, new LooklessControlTemplate<Window>(this.Template)),
                        new Setter(Window.FontFamilyProperty, "Segoe UI"),
                        new Setter(Window.FontSizeProperty, 12.0),
                    },
                },
            });
        }

        private Control Template(Window control)
        {
            return new Border
            {
                [~Border.BackgroundProperty] = control[~Window.BackgroundProperty],
                Child = new ContentPresenter
                {
                    Name = "contentPresenter",
                    [~ContentPresenter.ContentProperty] = control[~Window.ContentProperty],
                }
                //Content = new AdornerDecorator
                //{
                //    Content = new ContentPresenter
                //    {
                //        Name = "contentPresenter",
                //        [~ContentPresenter.ContentProperty] = control[~Window.ContentProperty],
                //    }
                //}
        };
        }
    }
}
