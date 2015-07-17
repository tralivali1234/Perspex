// -----------------------------------------------------------------------
// <copyright file="ContentHost.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Linq;
    using System.Collections;
    using Perspex.Collections;
    using Perspex.Controls.Core.Templates;

    /// <summary>
    /// Displays a piece of data according to a <see cref="DataTemplate"/>.
    /// </summary>
    public class ContentHost : Control, ILogical, IReparentingControl
    {
        /// <summary>
        /// Defines the <see cref="Content"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> ContentProperty =
            PerspexProperty.Register<ContentHost, object>("Content");

        private IPerspexList<ILogical> logicalChildren = new PerspexSingleItemList<ILogical>();

        private IControl logicalParent;

        /// <summary>
        /// Initializes static members of the <see cref="ContentHost"/> class.
        /// </summary>
        static ContentHost()
        {
            ContentProperty.Changed.AddClassHandler<ContentHost>(x => x.ContentChanged);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentHost"/> class.
        /// </summary>
        public ContentHost()
        {
            this.logicalParent = this;
        }

        /// <summary>
        /// Gets the control created by applying the data template to <see cref="Content"/>.
        /// </summary>
        public IControl Child
        {
            get { return this.logicalChildren.SingleOrDefault() as IControl; }
        }

        /// <summary>
        /// Gets or sets the content of the control.
        /// </summary>
        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return this.logicalChildren; }
        }

        /// <summary>
        /// Requests that the visual children of the control use another control as their logical
        /// parent.
        /// </summary>
        /// <param name="logicalParent">
        /// The logical parent for the visual children of the control.
        /// </param>
        /// <param name="children">
        /// The <see cref="ILogical.LogicalChildren"/> collection to modify.
        /// </param>
        void IReparentingControl.ReparentLogicalChildren(ILogical logicalParent, IPerspexList<ILogical> children)
        {
            var child = this.logicalChildren.SingleOrDefault();

            this.logicalParent = (IControl)logicalParent;
            this.logicalChildren = children;

            if (child != null)
            {
                ((ISetLogicalParent)child).SetParent(null);
                ((ISetLogicalParent)child).SetParent(this.logicalParent);
                this.logicalChildren.Add(child);
            }
        }

        /// <summary>
        /// Called when the <see cref="Content"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ContentChanged(PerspexPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((ISetLogicalParent)this.logicalChildren.Single()).SetParent(null);
                ((IList)this.logicalChildren).Clear();
                this.ClearVisualChildren();
            }

            if (e.NewValue != null)
            {
                var child = this.MaterializeDataTemplate(e.NewValue);
                this.AddVisualChild(child);
                ((IList)this.logicalChildren).Clear();
                this.logicalChildren.Add(child);
                ((ISetLogicalParent)child).SetParent(this.logicalParent);
            }
        }
    }
}
