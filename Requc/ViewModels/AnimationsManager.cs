using System.Collections.Generic;
using System.Windows.Media.Animation;

namespace Requc.ViewModels
{
    public static class AnimationsManager
    {
        public static void Add(Storyboard animation)
        {
            Animations.Add(animation);
        }

        public static void Remove(Storyboard animation)
        {
            Animations.Remove(animation);
        }
        
        private static readonly List<Storyboard> Animations = new List<Storyboard>(5);
    }
}
