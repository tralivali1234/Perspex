using Perspex;
using Perspex.Controls;

namespace PerspexGitHubClient.Views
{
    public class View<TViewModel> : UserControl
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
            private set { this.SetValue(ViewModelProperty, value); }
        }
    }
}
