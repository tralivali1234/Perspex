using System;
using Perspex;
using Perspex.Controls;
using ReactiveUI;

namespace PerspexGitHubClient.Views
{
    public class View<TViewModel> : UserControl, IViewFor<TViewModel>
        where TViewModel : class
    {
        public static readonly PerspexProperty<TViewModel> ViewModelProperty =
            PerspexProperty.Register<View<TViewModel>, TViewModel>("ViewModel");

        public View()
        {
            this.Bind(ViewModelProperty, this.GetObservable(DataContextProperty));
        }

        public TViewModel ViewModel
        {
            get { return this.GetValue(ViewModelProperty); }
            set { this.SetValue(ViewModelProperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return this.ViewModel; }
            set { this.ViewModel = (TViewModel)value; }
        }
    }
}
