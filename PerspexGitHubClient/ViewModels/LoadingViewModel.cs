using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class LoadingViewModel : ReactiveObject
    {
        private string message;

        public LoadingViewModel()
        {
        }

        public LoadingViewModel(string message)
        {
            this.message = message;
        }

        public string Message
        {
            get { return this.message; }
            set { this.RaiseAndSetIfChanged(ref this.message, value); }
        }
    }
}
