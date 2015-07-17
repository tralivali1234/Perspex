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
    using Perspex.Controls.Core.Generators;

    /// <summary>
    /// Displays a collection of data according to a <see cref="DataTemplate"/>.
    /// </summary>
    public class Repeat : Control
    {
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

        /// <summary>
        /// Initializes static members of the <see cref="Repeat"/> class.
        /// </summary>
        static Repeat()
        {
            ItemsProperty.Changed.AddClassHandler<Repeat>(x => x.ItemsChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Repeat"/> class.
        /// </summary>
        public Repeat()
        {
            this.Generator = this.CreateItemContainerGenerator();
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

        private void ItemsChanged(PerspexPropertyChangedEventArgs e)
        {
            var items = (ICollection)e.NewValue;

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
                    this.ResetItems();
                    break;

                default:
                    throw new NotSupportedException($"Collection action '{e.Action}' not yet supported.");
            }
        }

        private void AddItems(int startingIndex, IEnumerable items)
        {
            var containers = this.Generator.CreateContainers(startingIndex, items, this.ItemTemplate);
            this.Panel.Children.AddRange(containers);
        }

        private void RemoveItems(int startingIndex, int count)
        {
            var containers = this.Generator.RemoveContainers(startingIndex, count);
            this.Panel.Children.RemoveAll(containers);
        }

        private void ResetItems()
        {
            var containers = this.Generator.ClearContainers();
            this.Panel.Children.RemoveAll(containers);
        }
    }
}
