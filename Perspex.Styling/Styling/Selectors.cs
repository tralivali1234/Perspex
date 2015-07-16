// -----------------------------------------------------------------------
// <copyright file="Selectors.cs" company="Steven Kirk">
// Copyright 2014 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Styling
{
    using System;
    using System.Collections.Generic;
    using System.Reactive.Linq;
    using System.Reflection;

    public static class Selectors
    {
        public static StyleSelector Child(this StyleSelector previous)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(previous, x => MatchChild(x, previous), " < ", stopTraversal: true);
        }

        public static StyleSelector Class(this StyleSelector previous, string name)
        {
            Contract.Requires<ArgumentNullException>(previous != null);
            Contract.Requires<ArgumentNullException>(name != null);

            return new StyleSelector(previous, x => MatchClass(x, name), name);
        }

        public static StyleSelector Descendent(this StyleSelector previous)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(previous, x => MatchDescendent(x, previous), " ", stopTraversal: true);
        }

        public static StyleSelector Is(this StyleSelector previous, Type type)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(previous, x => MatchIs(x, type), type.Name);
        }

        public static StyleSelector Is<T>(this StyleSelector previous) where T : IStyleable
        {
            return previous.Is(typeof(T));
        }

        public static StyleSelector Name(this StyleSelector previous, string name)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(previous, x => MatchName(x, name), '#' + name);
        }

        public static StyleSelector OfType(this StyleSelector previous, Type type)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(previous, x => MatchOfType(x, type), type.Name);
        }

        public static StyleSelector OfType<T>(this StyleSelector previous) where T : IStyleable
        {
            return previous.OfType(typeof(T));
        }

        public static StyleSelector PropertyEquals<T>(this StyleSelector previous, PerspexProperty<T> property, object value)
        {
            Contract.Requires<ArgumentNullException>(previous != null);
            Contract.Requires<ArgumentNullException>(property != null);

            return new StyleSelector(previous, x => MatchPropertyEquals(x, property, value), $"[{property.Name}={value}]");
        }

        public static StyleSelector Template(this StyleSelector previous)
        {
            Contract.Requires<ArgumentNullException>(previous != null);

            return new StyleSelector(
                previous, 
                x => MatchTemplate(x, previous), 
                " /deep/ ", 
                inTemplate: true, 
                stopTraversal: true);
        }

        private static SelectorMatch MatchChild(IStyleable control, StyleSelector previous)
        {
            var parent = ((ILogical)control).LogicalParent;

            if (parent != null)
            {
                return previous.Match((IStyleable)parent);
            }
            else
            {
                return SelectorMatch.False;
            }
        }

        private static SelectorMatch MatchClass(IStyleable control, string name)
        {
            return new SelectorMatch(
                Observable
                    .Return(control.Classes.Contains(name))
                    .Concat(control.Classes.Changed.Select(e => control.Classes.Contains(name))));
        }

        private static SelectorMatch MatchDescendent(IStyleable control, StyleSelector previous)
        {
            ILogical c = (ILogical)control;
            List<IObservable<bool>> descendentMatches = new List<IObservable<bool>>();

            while (c != null)
            {
                c = c.LogicalParent;

                if (c is IStyleable)
                {
                    var match = previous.Match((IStyleable)c);

                    if (match.ImmediateResult != null)
                    {
                        if (match.ImmediateResult == true)
                        {
                            return SelectorMatch.True;
                        }
                    }
                    else
                    {
                        descendentMatches.Add(match.ObservableResult);
                    }
                }
            }

            return new SelectorMatch(new StyleActivator(
                descendentMatches,
                ActivatorMode.Or));
        }

        private static SelectorMatch MatchIs(IStyleable control, Type type)
        {
            var controlType = control.StyleKey ?? control.GetType();
            return new SelectorMatch(type.GetTypeInfo().IsAssignableFrom(controlType.GetTypeInfo()));
        }

        private static SelectorMatch MatchName(IStyleable control, string name)
        {
            return new SelectorMatch(control.Name == name);
        }

        private static SelectorMatch MatchOfType(IStyleable control, Type type)
        {
            var controlType = control.StyleKey ?? control.GetType();
            return new SelectorMatch(controlType == type);
        }

        private static SelectorMatch MatchPropertyEquals<T>(IStyleable x, PerspexProperty<T> property, object value)
        {
            if (!x.IsRegistered(property))
            {
                return SelectorMatch.False;
            }
            else
            {
                return new SelectorMatch(x.GetObservable(property).Select(v => object.Equals(v, value)));
            }
        }

        private static SelectorMatch MatchTemplate(IStyleable control, StyleSelector previous)
        {
            throw new NotImplementedException();
            //IStyleable templatedParent = control.TemplatedParent as IStyleable;

            //if (templatedParent == null)
            //{
            //    throw new InvalidOperationException(
            //        "Cannot call Template selector on control with null TemplatedParent.");
            //}

            //return previous.Match(templatedParent);
        }
    }
}
