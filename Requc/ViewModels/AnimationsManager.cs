using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;

namespace Requc.ViewModels
{
    public static class AnimationsManager
    {
        public static void Add(Storyboard animation, FrameworkElement containingObject)
        {
            Animations.Add(new Tuple<Storyboard, FrameworkElement>(animation, containingObject));
            ////animation.SpeedRatio = 20;
        }

        public static void Remove(Storyboard animation, FrameworkElement containingObject)
        {
            Animations.RemoveAll(tuple => tuple.Item1.Equals(animation) && tuple.Item2.Equals(containingObject));
        }
        
        private static readonly List<Tuple<Storyboard, FrameworkElement>> Animations = new List<Tuple<Storyboard, FrameworkElement>>(5);
    }
}
