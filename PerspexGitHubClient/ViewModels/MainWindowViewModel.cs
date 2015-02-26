using System;
using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private object content;

        private LogInViewModel login;

        public MainWindowViewModel()
        {
            this.ShowLogin();
        }

        public object Content
        {
            get { return this.content; }
            private set { this.RaiseAndSetIfChanged(ref this.content, value); }
        }

        private void ShowLogin()
        {
            this.login = new LogInViewModel();
            this.login.OkCommand.Subscribe(_ => this.TryLogin());
            this.Content = this.login;
        }

        private void TryLogin()
        {
            this.Content = new LoadingViewModel("Logging in...");
        }
    }
}
