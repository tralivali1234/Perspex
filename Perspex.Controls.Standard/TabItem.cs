// -----------------------------------------------------------------------
// <copyright file="TabItem.cs" company="Steven Kirk">
// Copyright 2013 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Standard
{
    using Perspex.Controls.Core;
    using Perspex.Controls.Core.Mixins;
    using Perspex.Controls.Standard.Primitives;

    /// <summary>
    /// An item in a <see cref="TabStrip"/> or <see cref="TabControl"/>.
    /// </summary>
    public class TabItem : HeaderedContentControl, ISelectable
    {
        /// <summary>
        /// Defines the <see cref="IsSelected"/> property.
        /// </summary>
        /// TODO: Define this on a lower-level control and .AddOwner here.
        public static readonly PerspexProperty<bool> IsSelectedProperty =
            PerspexProperty.Register<HeaderedContentControl, bool>("IsSelected");

        /// <summary>
        /// Initializes static members of the <see cref="TabItem"/> class.
        /// </summary>
        static TabItem()
        {
            var mixin = new SelectableMixin<TabItem>(IsSelectedProperty);
            FocusableProperty.OverrideDefaultValue(typeof(TabItem), true);
        }

        /// <summary>
        /// Gets or sets the selected state of the control.
        /// </summary>
        public bool IsSelected
        {
            get { return this.GetValue(IsSelectedProperty); }
            set { this.SetValue(IsSelectedProperty, value); }
        }
    }
}
