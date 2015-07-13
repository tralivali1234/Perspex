// -----------------------------------------------------------------------
// <copyright file="KeyboardNavigationTests.cs" company="Steven Kirk">
// Copyright 2015 MIT Licence. See licence.md for more information.
// </copyright>
// -----------------------------------------------------------------------

namespace Perspex.Input.UnitTests
{
    using Perspex.Controls;
    using Perspex.Controls.Core;
    using Xunit;

    public class KeyboardNavigationTests
    {
        [Fact]
        public void GetNextInTabOrder_Continue_Returns_Next_Control_In_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (current = new TestControl { Name = "Button2" }),
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Continue_Returns_First_Control_In_Next_Sibling_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            (current = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Continue_Returns_Next_Sibling()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            (current = new TestControl { Name = "Button3" }),
                        }
                    }),
                    (next = new TestControl { Name = "Button4" }),
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Continue_Returns_First_Control_In_Next_Uncle_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (container = new StackPanel
                            {
                                Children = new Controls
                                {
                                    new TestControl { Name = "Button1" },
                                    new TestControl { Name = "Button2" },
                                    (current = new TestControl { Name = "Button3" }),
                                }
                            }),
                        },
                    },
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Continue_Returns_Child_Of_Top_Level()
        {
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (next = new TestControl { Name = "Button1" }),
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(top);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Continue_Wraps()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (container = new StackPanel
                            {
                                Children = new Controls
                                {
                                    (next = new TestControl { Name = "Button1" }),
                                    new TestControl { Name = "Button2" },
                                    new TestControl { Name = "Button3" },
                                }
                            }),
                        },
                    },
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            (current = new TestControl { Name = "Button6" }),
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Cycle_Returns_Next_Control_In_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (current = new TestControl { Name = "Button2" }),
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Cycle_Wraps_To_First()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle,
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button1" }),
                            new TestControl { Name = "Button2" },
                            (current = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Once_Moves_To_Next_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Once,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (current = new TestControl { Name = "Button2" }),
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Once_Moves_To_Active_Element()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Once,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (next = new TestControl { Name = "Button2" }),
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            (current = new TestControl { Name = "Button6" }),
                        }
                    },
                }
            };

            KeyboardNavigation.SetTabOnceActiveElement(container, next);

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Never_Moves_To_Next_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Never,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (current = new TestControl { Name = "Button2" }),
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetNextInTabOrder_Never_Skips_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Never,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            (current = new TestControl { Name = "Button6" }),
                        }
                    },
                }
            };

            KeyboardNavigation.SetTabOnceActiveElement(container, next);

            var result = KeyboardNavigationHandler.GetNextInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Continue_Returns_Previous_Control_In_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (next = new TestControl { Name = "Button2" }),
                            (current = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Continue_Returns_Last_Control_In_Previous_Sibling_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (current = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Continue_Returns_Last_Child_Of_Sibling()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    (current = new TestControl { Name = "Button4" }),
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Continue_Returns_Last_Control_In_Previous_Nephew_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (container = new StackPanel
                            {
                                Children = new Controls
                                {
                                    new TestControl { Name = "Button1" },
                                    new TestControl { Name = "Button2" },
                                    (next = new TestControl { Name = "Button3" }),
                                }
                            }),
                        },
                    },
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (current = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Continue_Wraps()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (container = new StackPanel
                            {
                                Children = new Controls
                                {
                                    (current = new TestControl { Name = "Button1" }),
                                    new TestControl { Name = "Button2" },
                                    new TestControl { Name = "Button3" },
                                }
                            }),
                        },
                    },
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            (next = new TestControl { Name = "Button6" }),
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Cycle_Returns_Previous_Control_In_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle,
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button1" }),
                            (current = new TestControl { Name = "Button2" }),
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Cycle_Wraps_To_Last()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Cycle,
                        Children = new Controls
                        {
                            (current = new TestControl { Name = "Button1" }),
                            new TestControl { Name = "Button2" },
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Once_Moves_To_Previous_Container()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            new TestControl { Name = "Button2" },
                            (next = new TestControl { Name = "Button3" }),
                        }
                    }),
                    new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Once,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button4" },
                            (current = new TestControl { Name = "Button5" }),
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Once_Moves_To_Active_Element()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Once,
                        Children = new Controls
                        {
                            new TestControl { Name = "Button1" },
                            (next = new TestControl { Name = "Button2" }),
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (current = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            KeyboardNavigation.SetTabOnceActiveElement(container, next);

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        [Fact]
        public void GetPreviousInTabOrder_Once_Moves_To_First_Element()
        {
            StackPanel container;
            TestControl current;
            TestControl next;

            var top = new StackPanel
            {
                Children = new Controls
                {
                    (container = new StackPanel
                    {
                        [KeyboardNavigation.TabNavigationProperty] = KeyboardNavigationMode.Once,
                        Children = new Controls
                        {
                            (next = new TestControl { Name = "Button1" }),
                            new TestControl { Name = "Button2" },
                            new TestControl { Name = "Button3" },
                        }
                    }),
                    new StackPanel
                    {
                        Children = new Controls
                        {
                            (current = new TestControl { Name = "Button4" }),
                            new TestControl { Name = "Button5" },
                            new TestControl { Name = "Button6" },
                        }
                    },
                }
            };

            var result = KeyboardNavigationHandler.GetPreviousInTabOrder(current);

            Assert.Equal(next, result);
        }

        private class TestControl : Control
        {
            public TestControl()
            {
                Focusable = true;
            }
        }
    }
}
