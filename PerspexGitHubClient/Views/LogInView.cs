using Perspex;
using Perspex.Controls;
using Perspex.Layout;

namespace PerspexGitHubClient.Views
{
    public class LogInView : UserControl
    {
        public LogInView()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
        }

        private void InitializeComponent()
        {
            this.Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(GridLength.Auto),
                    new ColumnDefinition(new GridLength(4, GridUnitType.Pixel)),
                    new ColumnDefinition(GridLength.Auto),
                },
                RowDefinitions = new RowDefinitions
                {
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(4, GridUnitType.Pixel)),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(4, GridUnitType.Pixel)),
                    new RowDefinition(GridLength.Auto),
                    new RowDefinition(new GridLength(4, GridUnitType.Pixel)),
                    new RowDefinition(GridLength.Auto),
                },
                Children = new Controls
                {
                    new TextBlock
                    {
                        Text = "Log In",
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [Grid.ColumnSpanProperty] = 3,
                    },
                    new TextBlock
                    {
                        Text = "Username",
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 2,
                    },
                    new TextBox
                    {
                        MinWidth = 120,
                        [Grid.ColumnProperty] = 2,
                        [Grid.RowProperty] = 2,
                    },
                    new TextBlock
                    {
                        Text = "Password",
                        VerticalAlignment = VerticalAlignment.Center,
                        [Grid.RowProperty] = 4,
                    },
                    new TextBox
                    {
                        MinWidth = 120,
                        [Grid.ColumnProperty] = 2,
                        [Grid.RowProperty] = 4,
                    },
                },
            };
        }
    }
}
