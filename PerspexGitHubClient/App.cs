using Perspex;
using Perspex.Direct2D1;
using Perspex.Themes.Default;
using Perspex.Win32;
using PerspexGitHubClient.Views;
using ReactiveUI;

namespace PerspexGitHubClient
{
    public class App : Application
    {
        public App() : base(new DefaultTheme())
        {
            Win32Platform.Initialize();
            Direct2D1Platform.Initialize();

            this.ImplementHacks();
            this.InitializeDataTemplates();

            var mainWindow = new MainWindow();
            mainWindow.Show();
            this.Run(mainWindow);
        }

        private void ImplementHacks()
        {
            // HACK: The version of ReactiveUI currently included is for WPF and so expects a WPF
            // dispatcher. This makes sure it's initialized.
            var foo = System.Windows.Threading.Dispatcher.CurrentDispatcher;
        }

        private void InitializeDataTemplates()
        {
            this.DataTemplates.Add(new ViewLocator<ReactiveObject>());
        }

        private static void Main(string[] args)
        {
            var app = new App();
        }
    }
}
