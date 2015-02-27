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
            this.login.OkCommand.Subscribe(_ => this.ShowRepositories());
            this.Content = this.login;
        }

        private void ShowRepositories()
        {
            var vm = new UserRepositoriesViewModel();
            vm.Load(this.login.Username);
            this.Content = vm;
        }
    }
}
