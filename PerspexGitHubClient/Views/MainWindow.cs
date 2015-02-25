using Perspex.Controls;
using Perspex.Diagnostics;
using PerspexGitHubClient.ViewModels;
using ReactiveUI;

namespace PerspexGitHubClient.Views
{
    public class MainWindow : Window
    {
        private MainWindowViewModel viewModel = new MainWindowViewModel();

        public MainWindow()
        {
            this.InitializeComponent();
            DevTools.Attach(this);
        }

        private void InitializeComponent()
        {
            this.Content = new ContentControl
            {
                [!ContentControl.ContentProperty] = viewModel.WhenAnyValue(x => x.Content),
            };
        }
    }
}
