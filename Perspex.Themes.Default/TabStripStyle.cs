// -----------------------------------------------------------------------
// <copyright file="TabStripStyle.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Themes.Default
{
    using System.Linq;
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard;
    using Perspex.Controls.Standard.Presenters;
    using Perspex.Styling;

    /// <summary>
    /// The default style for the <see cref="TabStrip"/> class.
    /// </summary>
    public class TabStripStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabStripStyle"/> class.
        /// </summary>
        public TabStripStyle()
        {
            this.AddRange(new[]
            {
                new Style(x => x.OfType<TabStrip>())
                {
                    Setters = new[]
                    {
                        new Setter(TabStrip.TemplateProperty, new LooklessControlTemplate<TabStrip>(Template)),
                    },
                },
                new Style(x => x.OfType<TabStrip>().Template().OfType<StackPanel>())
                {
                    Setters = new[]
                    {
                        new Setter(StackPanel.GapProperty, 16.0),
                        new Setter(StackPanel.OrientationProperty, Orientation.Horizontal),
                    },
                },
            });
        }

        /// <summary>
        /// The default template for the <see cref="TabStrip"/> control.
        /// </summary>
        /// <param name="control">The control to which the template is being applied.</param>
        /// <returns>The root of the materialized template.</returns>
        public static Control Template(TabStrip control)
        {
            return new SelectorPresenter
            {
                Name = "itemsPresenter",
                [!SelectorPresenter.ItemsProperty] = control[!TabStrip.ItemsProperty],
                [!SelectorPresenter.ItemsPanelProperty] = control[!TabStrip.ItemsPanelProperty],
                [!!SelectorPresenter.SelectedContainerProperty] = control[!!TabStrip.SelectedContainerProperty],
                [!!SelectorPresenter.SelectedIndexProperty] = control[!!TabStrip.SelectedIndexProperty],
                [!!SelectorPresenter.SelectedItemProperty] = control[!!TabStrip.SelectedItemProperty],
            };
        }
    }
}
