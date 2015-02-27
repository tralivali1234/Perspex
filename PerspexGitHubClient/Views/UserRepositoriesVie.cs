using Octokit;
using Perspex;
using Perspex.Controls;
using Perspex.Layout;
using Perspex.Media;
using PerspexGitHubClient.ViewModels;
using ReactiveUI;

namespace PerspexGitHubClient.Views
{
    public class UserRepositoriesView : View<UserRepositoriesViewModel>
    {
        public UserRepositoriesView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Content = new Grid
            {
                ColumnDefinitions = new ColumnDefinitions
                {
                    new ColumnDefinition(new GridLength(1, GridUnitType.Star)),
                    new ColumnDefinition(new GridLength(3, GridUnitType.Star)),
                },
                Children = new Controls
                {
                    new ListBox
                    {
                        DataTemplates = new DataTemplates
                        {
                            new DataTemplate<Repository>(x => 
                                new StackPanel
                                {
                                    Orientation = Orientation.Vertical,
                                    Children = new Controls
                                    {
                                        new TextBlock
                                        {
                                            Text = x.Name,
                                            FontSize = 16,
                                        },
                                        new TextBlock
                                        {
                                            Text = x.Description,
                                            TextWrapping = TextWrapping.Wrap,
                                        },
                                    }
                                }),
                        },
                        [!ListBox.ItemsProperty] = this.WhenAnyValue(x => x.ViewModel.Repositories),
                    }
                }
            };
        }
    }
}
