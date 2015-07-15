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
    using System.Collections.Generic;


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

        private IPerspexList<ILogical> logicalChildren;

        private IControl logicalParent;

        private bool templateApplied;

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
            get { return this.LogicalChildren.SingleOrDefault() as IControl; }
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
            get { return this.LogicalChildren; }
        }

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        /// <remarks>
        /// Because the control implements <see cref="IReparentingControl"/> the logical children
        /// collection is lazily-created. Use this property to ensure it is created.
        /// </remarks>
        private IPerspexList<ILogical> LogicalChildren
        {
            get
            {
                if (this.logicalChildren == null)
                {
                    this.logicalChildren = new PerspexSingleItemList<ILogical>();
                }

                return this.logicalChildren;
            }
        }

        /// <summary>
        /// Materializes the data template for the <see cref="Content"/>.
        /// </summary>
        public override void ApplyTemplate()
        {
            if (!this.templateApplied)
            {
                var content = this.Content;

                if (content != null)
                {
                    var child = this.MaterializeDataTemplate(content);
                    this.AddVisualChild(child);
                    ((IList)this.LogicalChildren).Clear();
                    this.LogicalChildren.Add(child);
                    ((ISetLogicalParent)child).SetParent(this.logicalParent);
                }

                this.templateApplied = true;
            }
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
            if (this.logicalChildren != null)
            {
                throw new InvalidOperationException(
                    "ReparentLogicalChildren must be called before LogicalChildren accessed.");
            }

            this.logicalParent = (IControl)logicalParent;
            this.logicalChildren = children;
        }

        /// <summary>
        /// Called when the <see cref="Content"/> property changes.
        /// </summary>
        /// <param name="e">The event args.</param>
        private void ContentChanged(PerspexPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                ((ISetLogicalParent)this.LogicalChildren.Single()).SetParent(null);
                ((IList)this.LogicalChildren).Clear();
                this.ClearVisualChildren();
            }

            this.templateApplied = false;
        }
    }
}
