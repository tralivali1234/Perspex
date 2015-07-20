// -----------------------------------------------------------------------
// <copyright file="ItemsControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using System;
    using System.Collections;
    using System.Diagnostics.CodeAnalysis;
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard.Presenters;

    /// <summary>
    /// A control that displays a collection of items.
    /// </summary>
    public class ItemsControl : StandardControl
    {
        [SuppressMessage("Microsoft.StyleCop.CSharp.NamingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Needs to be before or a NullReferenceException is thrown.")]
        private static readonly ITemplate<IPanel> DefaultPanel =
            new FuncTemplate<IPanel>(() => new StackPanel { Orientation = Orientation.Vertical });

        /// <summary>
        /// Defines the <see cref="IsEmpty"/> property.
        /// </summary>
        public static readonly PerspexProperty<bool> IsEmptyProperty =
            Repeat.IsEmptyProperty.AddOwner<ItemsControl>();

        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static readonly PerspexProperty<ICollection> ItemsProperty =
            Repeat.ItemsProperty.AddOwner<ItemsControl>();

        /// <summary>
        /// Defines the <see cref="ItemsPanel"/> property.
        /// </summary>
        public static readonly PerspexProperty<ITemplate<IPanel>> ItemsPanelProperty =
            PerspexProperty.Register<ItemsControl, ITemplate<IPanel>>("ItemsPanel", defaultValue: DefaultPanel);

        /// <summary>
        /// Defines the <see cref="ItemTemplate"/> property.
        /// </summary>
        public static readonly PerspexProperty<IDataTemplate> ItemTemplateProperty =
            Repeat.ItemTemplateProperty.AddOwner<ItemsControl>();

        /// <summary>
        /// Gets a value indicating whether there are currently no items.
        /// </summary>
        public bool IsEmpty
        {
            get { return this.GetValue(IsEmptyProperty); }
        }

        /// <summary>
        /// Gets or sets the items to be displayed.
        /// </summary>
        public ICollection Items
        {
            get { return this.GetValue(ItemsProperty); }
            set { this.SetValue(ItemsProperty, value); }
        }

        /// <summary>
        /// Gets or sets the items to be displayed.
        /// </summary>
        public ITemplate<IPanel> ItemsPanel
        {
            get { return this.GetValue(ItemsPanelProperty); }
            set { this.SetValue(ItemsPanelProperty, value); }
        }

        /// <summary>
        /// Gets or sets the template used to create the items.
        /// </summary>
        public IDataTemplate ItemTemplate
        {
            get { return this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="IItemsPresenter"/> created by the lookless control template.
        /// </summary>
        public IItemsPresenter Presenter
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(INameScope nameScope)
        {
            this.Presenter = (IItemsPresenter)nameScope.FindName("itemsPresenter");
        }
    }
}
