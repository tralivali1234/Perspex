// -----------------------------------------------------------------------
// <copyright file="Panel.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Perspex.Collections;

    /// <summary>
    /// Base class for controls that can contain multiple children.
    /// </summary>
    /// <remarks>
    /// Controls can be added to a <see cref="Panel"/> by adding them to its <see cref="Children"/>
    /// collection. All children are layed out to fill the panel.
    /// </remarks>
    public class Panel : Control, IPanel, ILogical, IReparentingControl
    {
        private Controls children;

        private ILogical childLogicalParent;

        private IPerspexList<ILogical> logicalChildren;

        /// <summary>
        /// Initializes a new instance of the <see cref="Panel"/> class.
        /// </summary>
        public Panel()
        {
            this.children = new Controls();
            this.children.CollectionChanged += this.ChildrenChanged;
            this.childLogicalParent = this;
        }

        /// <summary>
        /// Gets or sets the children of the <see cref="Panel"/>.
        /// </summary>
        /// <remarks>
        /// Even though this property can be set, the setter is only intended for use in object
        /// initializers. Assigning to this property does not change the underlying collection,
        /// it simply clears the existing collection and addds the contents of the assigned
        /// collection.
        /// </remarks>
        public Controls Children
        {
            get
            {
                return this.children;
            }

            set
            {
                Contract.Requires<ArgumentNullException>(value != null);

                this.ClearVisualChildren();
                this.children.Clear();
                this.children.AddRange(value);
                this.logicalChildren?.AddRange(value);
            }
        }

        /// <summary>
        /// Gets the logical children of the control.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return (IPerspexReadOnlyList<ILogical>)this.logicalChildren ?? this.children; }
        }

        /// <summary>
        /// Requests that the visual children of the panel use another control as their logical
        /// parent.
        /// </summary>
        /// <param name="logicalParent">
        /// The logical parent for the visual children of the panel.
        /// </param>
        /// <param name="children">
        /// The <see cref="ILogical.LogicalChildren"/> collection to modify.
        /// </param>
        void IReparentingControl.ReparentLogicalChildren(ILogical logicalParent, IPerspexList<ILogical> children)
        {
            Contract.Requires<ArgumentNullException>(logicalParent != null);
            Contract.Requires<ArgumentNullException>(children != null);

            this.childLogicalParent = logicalParent;
            this.logicalChildren = children;

            foreach (var control in this.Children)
            {
                ((ISetLogicalParent)control).SetParent(null);
                ((ISetLogicalParent)control).SetParent((IControl)logicalParent);
                children.Add(control);
            }
        }

        /// <summary>
        /// Clears <see cref="IControl.Parent"/> for the specified controls.
        /// </summary>
        /// <param name="controls">The controls.</param>
        private void ClearLogicalParent(IEnumerable<IControl> controls)
        {
            foreach (var control in controls)
            {
                ((ISetLogicalParent)control).SetParent(null);
            }
        }

        /// <summary>
        /// Sets <see cref="IControl.Parent"/> for the specified controls.
        /// </summary>
        /// <param name="controls">The controls.</param>
        private void SetLogicalParent(IEnumerable<IControl> controls)
        {
            foreach (var control in controls)
            {
                ((ISetLogicalParent)control).SetParent((IControl)this.childLogicalParent);
            }
        }

        /// <summary>
        /// Called when the <see cref="Children"/> collection changes.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            List<Control> controls;

            // TODO: Handle Replace.
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    controls = e.NewItems.OfType<Control>().ToList();
                    this.AddVisualChildren(e.NewItems.OfType<Visual>());
                    this.SetLogicalParent(controls);
                    this.logicalChildren?.AddRange(controls);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    controls = e.OldItems.OfType<Control>().ToList();
                    this.ClearLogicalParent(e.OldItems.OfType<Control>());
                    this.logicalChildren?.RemoveAll(e.OldItems.OfType<ILogical>());
                    this.RemoveVisualChildren(e.OldItems.OfType<Visual>());
                    break;

                case NotifyCollectionChangedAction.Reset:
                    controls = e.OldItems.OfType<Control>().ToList();
                    this.ClearLogicalParent(controls);
                    this.ClearVisualChildren();
                    this.AddVisualChildren(this.children.Cast<Visual>());
                    this.SetLogicalParent(this.children);
                    this.logicalChildren?.AddRange(controls);
                    break;
            }

            this.InvalidateMeasure();
        }
    }
}
