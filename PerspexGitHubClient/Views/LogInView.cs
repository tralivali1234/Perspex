using Perspex.Controls;
using Perspex.Layout;

namespace PerspexGitHubClient.Views
{
    public class LogInView : ContentControl
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
                        HorizontalAlignment = HorizontalAlignment.Center,
                        [Grid.ColumnSpanProperty] = 2,
                    }
                },
            };
        }
    }
}
