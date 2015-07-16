// -----------------------------------------------------------------------
// <copyright file="SelectorTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Controls.Core.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Perspex.VisualTree;
    using Xunit;
    using Perspex.Input;

    public class SelectorTests
    {
        [Fact]
        public void Selection_Should_Initially_Be_None()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                }
            };

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedIndex_Should_Set_Selected_Class()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 1
            };

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedItem_Should_Set_Selected_Class()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                }
            };

            target.SelectedItem = target.Panel.Children[1];

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedClass(target));
        }

        [Fact]
        public void Changing_SelectedIndex_Should_Set_Selected_Class()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 1
            };

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedClass(target));

            target.SelectedIndex = 0;

            Assert.Equal(0, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[0], target.SelectedItem);
            Assert.Equal(new[] { true, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedIndex_Should_Set_IsSelected()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new SelectableBorder { Name = "Foo" },
                        new SelectableBorder { Name = "Bar" },
                    }
                },
                SelectedIndex = 1
            };

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedProperty(target));
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedItem_Should_Set_IsSelected()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new SelectableBorder { Name = "Foo" },
                        new SelectableBorder { Name = "Bar" },
                    }
                }
            };

            target.SelectedItem = target.Panel.Children[1];

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedProperty(target));
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Changing_SelectedIndex_Should_Set_IsSelected()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new SelectableBorder { Name = "Foo" },
                        new SelectableBorder { Name = "Bar" },
                    }
                },
                SelectedIndex = 1
            };

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
            Assert.Equal(new[] { false, true }, GetSelectedProperty(target));
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));

            target.SelectedIndex = 0;

            Assert.Equal(0, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[0], target.SelectedItem);
            Assert.Equal(new[] { true, false }, GetSelectedProperty(target));
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedIndex_Out_Of_Range_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                }
            };

            target.SelectedIndex = 3;

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Setting_SelectedItem_Out_Of_Range_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                }
            };

            target.SelectedItem = new Border();

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
            Assert.Equal(new[] { false, false }, GetSelectedClass(target));
        }

        [Fact]
        public void Adding_Item_With_IsSelected_True_Should_Change_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            var item = new SelectableBorder { IsSelected = true };
            target.Panel.Children.Add(item);

            Assert.Equal(2, target.SelectedIndex);
            Assert.Equal(item, target.SelectedItem);
        }

        [Fact]
        public void Removing_Selected_Child_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            target.Panel.Children.RemoveAt(0);

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Setting_Child_IsSelected_True_Should_Change_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new SelectableBorder { Name = "Foo" },
                        new SelectableBorder { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            ((ISelectable)target.Panel.Children[1]).IsSelected = true;

            Assert.Equal(1, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[1], target.SelectedItem);
        }

        [Fact]
        public void Setting_Child_IsSelected_False_Should_Change_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new SelectableBorder { Name = "Foo" },
                        new SelectableBorder { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            ((ISelectable)target.Panel.Children[0]).IsSelected = false;

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Selector_With_No_Panel_Is_Allowed()
        {
            var target = new Selector();
            target.SelectedIndex = 0;
            Assert.Equal(-1, target.SelectedIndex);
        }

        [Fact]
        public void Clearing_Panel_Should_Clear_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            target.Panel = null;

            Assert.Equal(-1, target.SelectedIndex);
            Assert.Null(target.SelectedItem);
        }

        [Fact]
        public void Setting_Panel_Should_Set_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
                SelectedIndex = 0
            };

            target.Panel = new StackPanel
            {
                Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                        new SelectableBorder { IsSelected = true },
                    }
            };

            Assert.Equal(2, target.SelectedIndex);
            Assert.Equal(target.Panel.Children[2], target.SelectedItem);
        }

        [Fact]
        public void PointerDown_Event_Should_Set_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
            };

            target.Panel.Children[1].RaiseEvent(new PointerPressEventArgs
            {
                RoutedEvent = InputElement.PointerPressedEvent,
            });

            Assert.Equal(1, target.SelectedIndex);
        }

        [Fact]
        public void GotFocus_Event_Should_Set_Selection()
        {
            var target = new Selector
            {
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
            };

            target.Panel.Children[1].RaiseEvent(new GotFocusEventArgs
            {
                RoutedEvent = InputElement.GotFocusEvent
            });

            Assert.Equal(1, target.SelectedIndex);
        }

        [Fact]
        public void PointerDown_Event_Should_Not_Set_Selection_When_IsUserSelectable_False()
        {
            var target = new Selector
            {
                IsUserSelectable = false,
                Panel = new StackPanel
                {
                    Children = new Controls
                    {
                        new Border { Name = "Foo" },
                        new Border { Name = "Bar" },
                    }
                },
            };

            target.Panel.Children[1].RaiseEvent(new PointerPressEventArgs
            {
                RoutedEvent = InputElement.PointerPressedEvent,
            });

            Assert.Equal(-1, target.SelectedIndex);
        }

        private static IEnumerable<bool> GetSelectedClass(Selector target)
        {
            return target.Panel.GetVisualChildren()
                .OfType<IControl>()
                .Select(x => x.Classes.Contains("selected"));
        }

        private static IEnumerable<bool> GetSelectedProperty(Selector target)
        {
            return target.Panel.GetVisualChildren()
                .OfType<ISelectable>()
                .Select(x => x.IsSelected);
        }

        private class SelectableBorder : Border, ISelectable
        {
            private bool isSelected;

            public bool IsSelected
            {
                get
                {
                    return this.isSelected;
                }

                set
                {
                    if (this.isSelected != value)
                    {
                        this.isSelected = value;
                        this.RaiseEvent(new Interactivity.RoutedEventArgs
                        {
                            RoutedEvent = Selector.IsSelectedChangedEvent
                        });
                    }
                }
            }
        }
    }
}
