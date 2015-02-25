using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private object content;

        public MainWindowViewModel()
        {
            this.content = new LogInViewModel();
        }

        public object Content
        {
            get { return this.content; }
            private set { this.RaiseAndSetIfChanged(ref this.content, value); }
        }
    }
}
