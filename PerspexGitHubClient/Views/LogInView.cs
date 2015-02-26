using Perspex;
using Perspex.Controls;
using Perspex.Layout;
using Perspex.Media;
using PerspexGitHubClient.ViewModels;
using ReactiveUI;

namespace PerspexGitHubClient.Views
{
    public class LogInView : View<LogInViewModel>
    {
        public LogInView()
        {
            this.InitializeComponent();
            this.HorizontalAlignment = HorizontalAlignment.Center;
            this.VerticalAlignment = VerticalAlignment.Center;
        }

        private void InitializeComponent()
        {
            this.Content = new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = 2,
                Padding = new Thickness(16),
                Content = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitions
                    {
                        new ColumnDefinition(GridLength.Auto),
                        new ColumnDefinition(GridLength.Auto),
                    },
                        RowDefinitions = new RowDefinitions
                    {
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(new GridLength(12, GridUnitType.Pixel)),
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(new GridLength(4, GridUnitType.Pixel)),
                        new RowDefinition(GridLength.Auto),
                        new RowDefinition(new GridLength(12, GridUnitType.Pixel)),
                        new RowDefinition(GridLength.Auto),
                    },
                    Children = new Controls
                    {
                        new TextBlock
                        {
                            Text = "Log In",
                            FontSize = 24,
                            HorizontalAlignment = HorizontalAlignment.Center,
                            [Grid.ColumnSpanProperty] = 2,
                        },
                        new TextBlock
                        {
                            Text = "Username",
                            Margin = new Thickness(4, 0),
                            VerticalAlignment = VerticalAlignment.Center,
                            [Grid.RowProperty] = 2,
                        },
                        new TextBox
                        {
                            MinWidth = 120,
                            [!TextBox.TextProperty] = this.WhenAnyValue(x => x.ViewModel.Username),
                            [Grid.ColumnProperty] = 2,
                            [Grid.RowProperty] = 2,
                        },
                        new TextBlock
                        {
                            Text = "Password",
                            Margin = new Thickness(4, 0),
                            VerticalAlignment = VerticalAlignment.Center,
                            [Grid.RowProperty] = 4,
                        },
                        new TextBox
                        {
                            MinWidth = 120,
                            [Grid.ColumnProperty] = 2,
                            [Grid.RowProperty] = 4,
                        },
                        new Button
                        {
                            Content = "OK",
                            HorizontalAlignment = HorizontalAlignment.Center,
                            MinWidth = 100,
                            [!Button.CommandProperty] = this.WhenAnyValue(x => x.ViewModel.OkCommand),
                            [Grid.ColumnSpanProperty] = 2,
                            [Grid.RowProperty] = 6,
                        }
                    },
                }
            };
        }
    }
}
