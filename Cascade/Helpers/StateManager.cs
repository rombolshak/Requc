﻿using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Reactive.Linq;

namespace Cascade.Helpers
{
    public class StateManager : VisualStateManager
    {
        public static string GetGoToState(DependencyObject obj)
        {
            return (string)obj.GetValue(GoToStateProperty);
        }

        public static void SetGoToState(DependencyObject obj, string value)
        {
            obj.SetValue(GoToStateProperty, value);
        }

        public static readonly DependencyProperty GoToStateProperty =
            DependencyProperty.RegisterAttached(
                "GoToState",
                typeof (string),
                typeof (StateManager),
                new PropertyMetadata((s, e) =>
                    {
                        var ctrl = s as FrameworkElement;
                        if (ctrl == null)
                            throw new InvalidOperationException(
                                "This attached property only supports types derived from Control.");
                        ChangeState(ctrl, (string) e.NewValue, OnAnimationCompleted);
                    }));

        private static void ChangeState(FrameworkElement ctrl, string state, Action onCompleted)
        {
            AddAnimation();
            SubscribeToStateCompletion(ctrl, state, onCompleted);
            if (GoToState(ctrl, state, true)) return;
            GoToElementState(ctrl, state, true);
        }

        public static void OnAnimationCompleted()
        {
            try
            {
                WaitHandle.Signal();
            }
            catch (InvalidOperationException)
            {
            }
        }

        public static void AddAnimation()
        {
            try
            {
                WaitHandle.AddCount();
            }
            catch (InvalidOperationException)
            {
                WaitHandle.Reset();
                WaitHandle.AddCount();
            }
        }

        public static void WaitAnimations()
        {
            Thread.Sleep(500);
            WaitHandle.Signal();
            WaitHandle.Wait(1000);
            WaitHandle.Reset();
        }

        private static void SubscribeToStateCompletion(FrameworkElement ctrl, string stateName, Action onCompleted)
        {
            var state = TryGetState(stateName, ctrl);
            if (state == null || state.Storyboard == null)
            {
                return;
            }

            var onCompletedObservable = Observable.FromEventPattern(e => state.Storyboard.Completed += e, e => state.Storyboard.Completed -= e);
            onCompletedObservable.Take(1).Subscribe(_ => onCompleted());
        }

        private static VisualState TryGetState(string stateName, FrameworkElement ctrl)
        {
            var groups = GetVisualStateGroups(ctrl);
            if (groups.Count == 0)
            {
                var templateControl = ctrl as Control;
                if (templateControl != null)
                    groups = GetVisualStateGroups(templateControl.GetType()
                                                                 .GetProperty("TemplateChild",
                                                                              BindingFlags.Instance |
                                                                              BindingFlags.NonPublic)
                                                                 .GetValue(templateControl, null) as FrameworkElement);
            }

            return groups.OfType<VisualStateGroup>().SelectMany(g => g.States.OfType<VisualState>()).FirstOrDefault(s => s.Name == stateName);
        }

        private static readonly CountdownEvent WaitHandle = new CountdownEvent(1);
    }
}
