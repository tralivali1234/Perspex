using System;
using Perspex.Controls;

namespace PerspexGitHubClient
{
    public class ViewLocator<TViewModel> : IDataTemplate
    {
        public Control Build(object data)
        {
            var name = data.GetType().FullName.Replace("ViewModel", "View");
            var type = Type.GetType(name);

            if (type != null)
            {
                return (Control)Activator.CreateInstance(type);
            }
            else
            {
                return new TextBlock { Text = name };
            }
        }

        public bool Match(object data)
        {
            return data is TViewModel;
        }
    }
}
