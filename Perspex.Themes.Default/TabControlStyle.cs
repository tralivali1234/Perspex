// -----------------------------------------------------------------------
// <copyright file="TabControlStyle.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Themes.Default
{
    using System;
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard;
    using Perspex.Styling;

    /// <summary>
    /// The default style for the <see cref="TabControl"/> class.
    /// </summary>
    public class TabControlStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabControlStyle"/> class.
        /// </summary>
        public TabControlStyle()
        {
            this.AddRange(new[]
            {
                new Style(x => x.OfType<TabControl>())
                {
                    Setters = new[]
                    {
                        new Setter(TabControl.TemplateProperty, new LooklessControlTemplate<TabControl>(Template)),
                    },
                },
            });
        }

        /// <summary>
        /// The default template for the <see cref="TabControl"/> control.
        /// </summary>
        /// <param name="control">The control to which the template is being applied.</param>
        /// <returns>The root of the materialized template.</returns>
        private static Control Template(TabControl control)
        {
            return new Grid
            {
                RowDefinitions = new RowDefinitions
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(1, GridUnitType.Star)),
                },
                Children = new Controls
                {
                    new TabStrip
                    {
                        Name = "tabStrip",
                        [~TabStrip.ItemsProperty] = control[~TabControl.ItemsProperty],
                        [!!TabStrip.SelectedIndexProperty] = control[!!TabControl.SelectedIndexProperty],
                    },
                    new Repeat
                    {
                        Panel = new Pages
                        {
                            [!Pages.SelectedIndexProperty] = control[!TabControl.SelectedIndexProperty]
                        },
                        MemberSelector = x => (x as TabItem)?.Content ?? x,
                        [~Repeat.ItemsProperty] = control[~TabControl.ItemsProperty],
                        [Grid.RowProperty] = 1,
                    }
                }
            };
        }
    }
}
