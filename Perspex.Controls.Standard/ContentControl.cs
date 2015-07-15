// -----------------------------------------------------------------------
// <copyright file="ContentControl.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using Perspex.Controls.Core;
    using Perspex.Controls.Standard.Presenters;

    /// <summary>
    /// A lookless control that displays a piece of data according to a
    /// <see cref="DataTemplate"/>.
    /// </summary>
    public class ContentControl : LooklessControl
    {
        /// <summary>
        /// Defines the <see cref="Content"/> property.
        /// </summary>
        public static PerspexProperty<object> ContentProperty =
            ContentHost.ContentProperty.AddOwner<ContentControl>();

        /// <summary>
        /// Gets or sets the content of the control.
        /// </summary>
        public object Content
        {
            get { return this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        /// <summary>
        /// Gets the <see cref="ContentPresenter"/> from the control's template.
        /// </summary>
        public ContentPresenter Presenter
        {
            get;
            private set;
        }

        /// <inheritdoc/>
        protected override void OnTemplateApplied(INameScope nameScope)
        {
            this.Presenter = (ContentPresenter)nameScope.FindName("contentPresenter");
        }
    }
}
