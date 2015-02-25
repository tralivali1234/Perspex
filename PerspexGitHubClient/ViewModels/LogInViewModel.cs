using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class LogInViewModel : ReactiveObject
    {
        private string username;

        private string password;

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
    }
}
