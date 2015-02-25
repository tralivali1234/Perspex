// -----------------------------------------------------------------------
// <copyright file="PmlRunner.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Markup.Pml.Runtime
{
    using System;
    using System.Reflection;
    using Perspex.Markup.Pml.Dom;

    public class PmlRunner
    {
        public void Run(Document document, object target)
        {
            this.Run(document.RootNode, target);
        }

        public void Run(ObjectInstantiation obj, object target)
        {
            foreach (var child in obj.Children)
            {
                var setter = child as PropertySetter;

                if (setter != null)
                {
                    this.Run(setter, target);
                }
            }
        }

        public void Run(PropertySetter setter, object target)
        {
            var property = target.GetType().GetTypeInfo().GetDeclaredProperty(setter.PropertyName.Name);

            if (property == null)
            {
                // TODO: Do this properly.
                throw new Exception("Could not find property: " + setter.PropertyName.Name);
            }

            var expression = setter.Value as ExpressionValue;

            if (expression != null)
            {
                this.SetProperty(property, expression, target);
            }
        }

        private void SetProperty(PropertyInfo property, ExpressionValue expression, object target)
        {
            // TODO: Implement when Roslyn gets scripting.
        }
    }
}
