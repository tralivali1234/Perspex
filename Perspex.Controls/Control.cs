// -----------------------------------------------------------------------
// <copyright file="Control.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls
{
    using System;
    using Perspex.Collections;
    using Perspex.Input;
    using Perspex.Rendering;
    using Perspex.Styling;
    using Splat;
    using Perspex.LogicalTree;
    using System.Linq;



    /// <summary>
    /// Base class for Perspex controls.
    /// </summary>
    /// <remarks>
    /// The control class extends <see cref="InputElement"/> and adds the following features:
    ///
    /// - A <see cref="Name"/>.
    /// - An inherited <see cref="DataContext"/>.
    /// - A <see cref="Tag"/> property to allow user-defined data to be attached to the control.
    /// - A collection of class strings for custom styling.
    /// - Implements <see cref="IStyleable"/> to allow styling to work on the control.
    /// - Implements <see cref="ILogical"/> to form part of a logical tree.
    /// </remarks>
    public class Control : InputElement, IControl
    {
        /// <summary>
        /// Defines the <see cref="DataContext"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> DataContextProperty =
            PerspexProperty.Register<Control, object>("DataContext", inherits: true);

        /// <summary>
        /// Defines the <see cref="Parent"/> property.
        /// </summary>
        public static readonly PerspexProperty<Control> ParentProperty =
            PerspexProperty.Register<Control, Control>("Parent");

        /// <summary>
        /// Defines the <see cref="Tag"/> property.
        /// </summary>
        public static readonly PerspexProperty<object> TagProperty =
            PerspexProperty.Register<Control, object>("Tag");

        private Classes classes;

        private DataTemplates dataTemplates;

        private string name;

        private Styles styles;

        /// <summary>
        /// Initializes static members of the <see cref="Control"/> class.
        /// </summary>
        static Control()
        {
            Control.AffectsMeasure(Control.IsVisibleProperty);
        }

        /// <summary>
        /// Gets or sets the controls classes.
        /// </summary>
        /// <remarks>
        /// Classes can be used to apply user-defined styling to controls, or to allow controls
        /// that share a common purpose to be easily selected.
        /// </remarks>
        public Classes Classes
        {
            get
            {
                if (this.classes == null)
                {
                    this.classes = new Classes();
                }

                return this.classes;
            }

            set
            {
                if (this.classes != value)
                {
                    this.classes.Clear();
                    this.classes.Add(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the control's data context.
        /// </summary>
        /// <remarks>
        /// The data context is an inherited property that specifies the default object that will
        /// be used for data binding.
        /// </remarks>
        public object DataContext
        {
            get { return this.GetValue(DataContextProperty); }
            set { this.SetValue(DataContextProperty, value); }
        }

        /// <summary>
        /// Gets or sets the data templates for the control.
        /// </summary>
        /// <remarks>
        /// Each control may define data templates which are applied to the control itself and its
        /// children.
        /// </remarks>
        public DataTemplates DataTemplates
        {
            get
            {
                if (this.dataTemplates == null)
                {
                    this.dataTemplates = new DataTemplates();
                }

                return this.dataTemplates;
            }

            set
            {
                this.dataTemplates = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the control.
        /// </summary>
        /// <remarks>
        /// A control's name is used to uniquely identify a control within the control's name
        /// scope. Once a control is added to a visual tree, its name cannot be changed.
        /// </remarks>
        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (((IVisual)this).IsAttachedToVisualTree)
                {
                    throw new InvalidOperationException(
                        "Cannot set Name : control already added to visual tree.");
                }

                this.name = value;
            }
        }

        /// <summary>
        /// Gets or sets the styles for the control.
        /// </summary>
        /// <remarks>
        /// Styles for the entire application are added to the Application.Styles collection, but
        /// each control may in addition define its own styles which are applied to the control
        /// itself and its children.
        /// </remarks>
        public Styles Styles
        {
            get
            {
                if (this.styles == null)
                {
                    this.styles = new Styles();
                }

                return this.styles;
            }

            set
            {
                this.styles = value;
            }
        }

        /// <summary>
        /// Gets the control's logical parent.
        /// </summary>
        public Control Parent
        {
            get { return this.GetValue(ParentProperty); }
        }

        /// <summary>
        /// Gets or sets a user-defined object attached to the control.
        /// </summary>
        public object Tag
        {
            get { return this.GetValue(TagProperty); }
            set { this.SetValue(TagProperty, value); }
        }

        /// <summary>
        /// Gets or sets the control's logical parent.
        /// </summary>
        ILogical ILogical.LogicalParent
        {
            get { return this.Parent; }
            set { this.SetValue(ParentProperty, value); }
        }

        /// <summary>
        /// Gets the control's logical children.
        /// </summary>
        IPerspexReadOnlyList<ILogical> ILogical.LogicalChildren
        {
            get { return PerspexSingleItemList<ILogical>.Empty; }
        }

        /// <summary>
        /// Gets the type by which the control is styled.
        /// </summary>
        /// <remarks>
        /// Usually controls are styled by their own type, but there are instances where you want
        /// a control to be styled by its base type, e.g. creating SpecialButton that
        /// derives from Button and adds extra functionality but is still styled as a regular
        /// Button.
        /// </remarks>
        Type IStyleable.StyleKey
        {
            get { return this.GetType(); }
        }

        /// <summary>
        /// Tries to being the control into view.
        /// </summary>
        public void BringIntoView()
        {
            this.BringIntoView(new Rect(this.Bounds.Size));
        }

        /// <summary>
        /// Tries to being the specified area on the control into view.
        /// </summary>
        /// <param name="rect">The area of the control to being into view.</param>
        public void BringIntoView(Rect rect)
        {
            ////var ev = new RequestBringIntoViewEventArgs
            ////{
            ////    RoutedEvent = RequestBringIntoViewEvent,
            ////    TargetObject = this,
            ////    TargetRect = rect,
            ////};

            ////this.RaiseEvent(ev);
        }

        /// <summary>
        /// Called when the control is attached to a visual tree.
        /// </summary>
        /// <param name="root">The root of the visual tree.</param>
        protected override void OnAttachedToVisualTree(IRenderRoot root)
        {
            base.OnAttachedToVisualTree(root);

            if (!string.IsNullOrWhiteSpace(this.name))
            {
                var nameScope = this.GetLogicalAncestors()
                    .OfType<INameScope>()
                    .FirstOrDefault();
                nameScope?.Names.Register(this);
            }

            IStyler styler = Locator.Current.GetService<IStyler>();
            styler?.ApplyStyles(this);
        }

        /// <summary>
        /// Called when the control is detached from a visual tree.
        /// </summary>
        /// <param name="root">The root of the visual tree.</param>
        protected override void OnDetachedFromVisualTree(IRenderRoot root)
        {
            base.OnDetachedFromVisualTree(root);

            if (!string.IsNullOrWhiteSpace(this.name))
            {
                var nameScope = this.GetLogicalAncestors()
                    .OfType<INameScope>()
                    .FirstOrDefault();
                nameScope?.Names.Deregister(this);
            }
        }
    }
}
