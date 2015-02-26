using Perspex;
using Perspex.Controls;
using Perspex.Layout;
using Perspex.Media;
using PerspexGitHubClient.ViewModels;
using ReactiveUI;

namespace PerspexGitHubClient.Views
{
    public class LoadingView : View<LoadingViewModel>
    {
        public LoadingView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Content = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                [!TextBlock.TextProperty] = this.WhenAnyValue(x => x.ViewModel.Message),
            };
        }
    }
}
