using System;
using Octokit;
using ReactiveUI;

namespace PerspexGitHubClient.ViewModels
{
    public class LogInViewModel : ReactiveObject
    {
        private string username;

        public LogInViewModel()
        {
            this.OkCommand = ReactiveCommand.Create(
                this.WhenAnyValue(
                    x => x.Username,
                    x => !string.IsNullOrWhiteSpace(x)));
        }

        public string Username
        {
            get { return this.username; }
            set { this.RaiseAndSetIfChanged(ref this.username, value); }
        }

        public ReactiveCommand<object> OkCommand
        {
            get;
            private set;
        }
    }
}
