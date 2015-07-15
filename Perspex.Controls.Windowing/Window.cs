// -----------------------------------------------------------------------
// <copyright file="Window.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Windowing
{
    using System;
    using System.Reactive.Linq;
    using System.Threading.Tasks;
    using Perspex.Controls.Windowing.Platform;
    using Perspex.Input;
    using Perspex.Media;
    using Perspex.Styling;
    using Splat;

    /// <summary>
    /// A window.
    /// </summary>
    public class Window : TopLevel, IStyleable, IFocusScope
    {
        /// <summary>
        /// Defines the <see cref="Title"/> property.
        /// </summary>
        public static readonly PerspexProperty<string> TitleProperty =
            PerspexProperty.Register<Window, string>("Title", "Window");

        private object dialogResult;

        /// <summary>
        /// Initializes static members of the <see cref="Window"/> class.
        /// </summary>
        static Window()
        {
            BackgroundProperty.OverrideDefaultValue(typeof(Window), Brushes.White);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Window"/> class.
        /// </summary>
        public Window()
            : base(Locator.Current.GetService<IWindowImpl>())
        {
        }

        /// <summary>
        /// Gets the platform-specific window implementation.
        /// </summary>
        public new IWindowImpl PlatformImpl
        {
            get { return (IWindowImpl)base.PlatformImpl; }
        }

        /// <summary>
        /// Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get { return this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        /// <summary>
        /// Gets the type by which the control is styled.
        /// </summary>
        Type IStyleable.StyleKey
        {
            get { return typeof(Window); }
        }

        /// <summary>
        /// Closes the window.
        /// </summary>
        public void Close()
        {
            this.PlatformImpl.Dispose();
        }

        /// <summary>
        /// Closes the window with the specified dialog result.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        /// <remarks>
        /// To retrieve a dialog result, the window should be opened with the
        /// <see cref="ShowDialog{TResult}"/> method.
        /// </remarks>
        public void Close(object dialogResult)
        {
            this.dialogResult = dialogResult;
            this.Close();
        }

        /// <summary>
        /// Hides the window.
        /// </summary>
        public void Hide()
        {
            using (this.BeginAutoSizing())
            {
                this.PlatformImpl.Hide();
            }
        }

        /// <summary>
        /// Shows the window.
        /// </summary>
        public void Show()
        {
            this.LayoutManager.ExecuteLayoutPass();

            using (this.BeginAutoSizing())
            {
                this.PlatformImpl.Show();
            }
        }

        /// <summary>
        /// Shows the window as a dialog.
        /// </summary>
        /// <returns>
        /// A task that can be used to track when the window closes.
        /// </returns>
        public Task ShowDialog()
        {
            return this.ShowDialog<object>();
        }

        /// <summary>
        /// Shows the window as a dialog and retrieves a result when closed.
        /// </summary>
        /// <typeparam name="TResult">The type of the dialog result.</typeparam>
        /// <returns>
        /// A task that can be used to retrieve the window's dialog result when it closes.
        /// </returns>
        public Task<TResult> ShowDialog<TResult>()
        {
            this.LayoutManager.ExecuteLayoutPass();

            using (this.BeginAutoSizing())
            {
                var modal = this.PlatformImpl.ShowDialog();
                var result = new TaskCompletionSource<TResult>();

                Observable.FromEventPattern(this, nameof(this.Closed))
                    .Take(1)
                    .Subscribe(_ =>
                    {
                        modal.Dispose();
                        result.SetResult((TResult)this.dialogResult);
                    });

                return result.Task;
            }
        }
    }
}
