using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class LogInViewModel : ReactiveObject
    {
        private string username;

        private string password;

        public LogInViewModel()
        {
            this.OkCommand = ReactiveCommand.Create(
                this.WhenAnyValue(
                    x => x.Username,
                    x => x.Password,
                    (u, p) => !string.IsNullOrWhiteSpace(u) && !string.IsNullOrWhiteSpace(p)));
        }

        public string Username
        {
            get { return this.username; }
            set { this.RaiseAndSetIfChanged(ref this.username, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.RaiseAndSetIfChanged(ref this.password, value); }
        }

        public ReactiveCommand<object> OkCommand
        {
            get;
            private set;
        }
    }
}
