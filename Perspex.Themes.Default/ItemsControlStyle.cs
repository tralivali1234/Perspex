// -----------------------------------------------------------------------
// <copyright file="ItemsControlStyle.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Themes.Default
{
    using System.Linq;
    using Perspex.Controls;
    using Perspex.Styling;
    using Perspex.Controls.Standard;
    using Perspex.Controls.Standard.Presenters;

    /// <summary>
    /// The default style for the <see cref="ItemsControl"/> class.
    /// </summary>
    public class ItemsControlStyle : Styles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItemsControlStyle"/> class.
        /// </summary>
        public ItemsControlStyle()
        {
            this.AddRange(new[]
            {
                new Style(x => x.OfType<ItemsControl>())
                {
                    Setters = new[]
                    {
                        new Setter(ItemsControl.TemplateProperty, new LooklessControlTemplate<ItemsControl>(Template)),
                    },
                },
            });
        }

        /// <summary>
        /// The default template for the <see cref="ItemsControl"/> control.
        /// </summary>
        /// <param name="control">The control to which the template is being applied.</param>
        /// <returns>The root of the materialized template.</returns>
        public static Control Template(ItemsControl control)
        {
            return new RepeatPresenter
            {
                Name = "itemsPresenter",
                [~RepeatPresenter.ItemsProperty] = control[~ItemsControl.ItemsProperty],
                [~RepeatPresenter.ItemsPanelProperty] = control[~ItemsControl.ItemsPanelProperty],
                [(~RepeatPresenter.IsEmptyProperty).WithMode(BindingMode.OneWayToSource)] = control[!ItemsControl.IsEmptyProperty],
            };
        }
    }
}
