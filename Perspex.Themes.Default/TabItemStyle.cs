// -----------------------------------------------------------------------
// <copyright file="TabItemStyle.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Themes.Default
{
    using System.Linq;
    using Perspex.Controls;
    using Perspex.Controls.Standard;
    using Perspex.Controls.Standard.Presenters;
    using Perspex.Media;
    using Perspex.Styling;

    /// <summary>
    /// The default style for the <see cref="TabItem"/> class.
    /// </summary>
    public class TabItemStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabItemStyle"/> class.
        /// </summary>
        public TabItemStyle()
        {
            this.AddRange(new[]
            {
                new Style(x => x.OfType<TabItem>())
                {
                    Setters = new[]
                    {
                        new Setter(TabItem.FontSizeProperty, 28.7),
                        new Setter(TabItem.ForegroundProperty, Brushes.Gray),
                        new Setter(TabItem.TemplateProperty, new LooklessControlTemplate<TabItem>(Template)),
                    },
                },
                new Style(x => x.OfType<TabItem>().Class("selected"))
                {
                    Setters = new[]
                    {
                        new Setter(TabItem.ForegroundProperty, Brushes.Black),
                    },
                },
            });
        }

        /// <summary>
        /// The default template for the <see cref="TabItem"/> control.
        /// </summary>
        /// <param name="control">The control to which the template is being applied.</param>
        /// <returns>The root of the materialized template.</returns>
        public static Control Template(TabItem control)
        {
            return new ContentPresenter
            {
                Name = "headerPresenter",
                [~ContentPresenter.ContentProperty] = control[~TabItem.HeaderProperty],
            };
        }
    }
}
