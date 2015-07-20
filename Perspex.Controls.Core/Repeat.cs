// -----------------------------------------------------------------------
// <copyright file="Repeat.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Linq;
    using Perspex.Controls.Core.Generators;
    using Perspex.Collections;

    /// <summary>
    /// Displays a collection of data according to a <see cref="DataTemplate"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The <see cref="Repeat"/> control populates a <see cref="Panel"/> based on a collection
    /// of <see cref="Items"/>. A control is created in the panel for each non-null item based
    /// on a <see cref="IDataTemplate"/>:
    /// </para>
    /// <list type="bullet">
    /// <item>
    /// First the <see cref="ItemTemplate"/> is checked for a template that matches the item.
    /// </item>
    /// <item>
    /// Next the <see cref="DataTemplates"/> collection is checked.
    /// </item>
    /// <item>
    /// If neither of the previous two location contain a matching data template, then the logical
    /// tree is searched upwards to the root.
    /// </item>
    /// <item>
    /// If still no matching data template is found, then Application's DataTemplates collection is
    /// searched.
    /// </item>
    /// <item>
    /// Finally, a <see cref="TextBlock"/> will be created with the result of calling
    /// <see cref="object.ToString"/> on the item.
    /// </item>
    /// </list>
    /// </remarks>
    public class Repeat : Control, ILogical
    {
        /// <summary>
        /// Defines the <see cref="IsEmpty"/> property.
        /// </summary>
        public static readonly PerspexProperty<bool> IsEmptyProperty =
            PerspexProperty.Register<Repeat, bool>("IsEmpty", true);

        /// <summary>
        /// Defines the <see cref="Items"/> property.
        /// </summary>
        public static readonly PerspexProperty<ICollection> ItemsProperty =
            PerspexProperty.Register<Repeat, ICollection>("Items");

        /// <summary>
        /// Defines the <see cref="ItemTemplate"/> property.
        /// </summary>
        public static readonly PerspexProperty<IDataTemplate> ItemTemplateProperty =
            PerspexProperty.Register<Repeat, IDataTemplate>("ItemTemplate");

        /// <summary>
        /// Defines the <see cref="Panel"/> property.
        /// </summary>
        public static readonly PerspexProperty<IPanel> PanelProperty =
            PerspexProperty.Register<Repeat, IPanel>("Panel");

        private PerspexSingleItemList<ILogical> logicalChild = new PerspexSingleItemList<ILogical>();

        /// <summary>
        /// Initializes static members of the <see cref="Repeat"/> class.
        /// </summary>
        static Repeat()
        {
            ItemsProperty.Changed.AddClassHandler<Repeat>(x => x.ItemsChanged);
            PanelProperty.Changed.AddClassHandler<Repeat>(x => x.PanelChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repeat"/> class.
        /// </summary>
        public Repeat()
        {
            this.Generator = this.CreateItemContainerGenerator();
        }

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
        /// Gets or sets the template used to create the items.
        /// </summary>
        public IDataTemplate ItemTemplate
        {
            get { return this.GetValue(ItemTemplateProperty); }
            set { this.SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets the panel containing the items.
        /// </summary>
        public IPanel Panel
        {
            get { return this.GetValue(PanelProperty); }
            set { this.SetValue(PanelProperty, value); }
        }

        /// <inheritdoc/>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return this.logicalChild; }
        }

        /// <summary>
        /// Gets the item container generator used to generate the controls for the items.
        /// </summary>
        protected IItemContainerGenerator Generator
        {
            get;
        }

        /// <summary>
        /// Creates the <see cref="Generator"/> for the control.
        /// </summary>
        /// <returns>The generator.</returns>
        protected virtual IItemContainerGenerator CreateItemContainerGenerator()
        {
            return new ItemContainerGenerator(this);
        }

        /// <summary>
        /// Called when the <see cref="Items"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ItemsChanged(PerspexPropertyChangedEventArgs e)
        {
            var items = (ICollection)e.OldValue;

            if (items != null)
            {
                this.ResetItems(this.Panel);
            }

            items = (ICollection)e.NewValue;

            if (items != null)
            {
                var incc = items as INotifyCollectionChanged;

                if (incc != null)
                {
                    incc.CollectionChanged += this.ItemsCollectionChanged;
                }

                this.AddItems(0, items);
            }
        }

        /// <summary>
        /// Called when the <see cref="Items"/> collection mutates.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event args.</param>
        private void ItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddItems(e.NewStartingIndex, e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    this.RemoveItems(e.OldStartingIndex, e.OldItems.Count);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    this.ResetItems(this.Panel);
                    break;

                default:
                    throw new NotSupportedException($"Collection action '{e.Action}' not yet supported.");
            }
        }

        /// <summary>
        /// Called when the <see cref="Panel"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void PanelChanged(PerspexPropertyChangedEventArgs e)
        {
            var panel = (Panel)e.OldValue;

            if (panel != null)
            {
                this.ResetItems(panel);
                this.ClearVisualChildren();
                ((ISetLogicalParent)panel).SetParent(null);
                this.logicalChild.SingleItem = null;
            }

            panel = (Panel)e.NewValue;

            if (panel != null)
            {
                this.AddVisualChild(panel);
                ((ISetLogicalParent)panel).SetParent(this);
                this.logicalChild.SingleItem = panel;

                if (this.Items != null)
                {
                    this.AddItems(0, this.Items);
                }
            }
        }

        /// <summary>
        /// Adds containers for the specified items.
        /// </summary>
        /// <param name="startingIndex">The index of the first item in <see cref="Items"/>.</param>
        /// <param name="items">The items to add.</param>
        private void AddItems(int startingIndex, IEnumerable items)
        {
            if (this.Panel != null)
            {
                var containers = this.Generator.CreateContainers(startingIndex, items, this.ItemTemplate);
                this.Panel.Children.AddRange(containers);
                this.SetValue(IsEmptyProperty, this.Panel.Children.Count == 0);
            }
        }

        /// <summary>
        /// Adds containers for the specified items.
        /// </summary>
        /// <param name="startingIndex">The index of the first item in <see cref="Items"/>.</param>
        /// <param name="count">The number of items to remove.</param>
        private void RemoveItems(int startingIndex, int count)
        {
            if (this.Panel != null)
            {
                var containers = this.Generator.RemoveContainers(startingIndex, count);
                this.Panel.Children.RemoveAll(containers);
                this.SetValue(IsEmptyProperty, this.Panel.Children.Count == 0);
            }
        }

        /// <summary>
        /// Removes all containers.
        /// </summary>
        /// <param name="panel">The panel.</param>
        private void ResetItems(IPanel panel)
        {
            if (panel != null)
            {
                var containers = this.Generator.ClearContainers();
                panel.Children.RemoveAll(containers);
                this.SetValue(IsEmptyProperty, true);
            }
        }
    }
}
