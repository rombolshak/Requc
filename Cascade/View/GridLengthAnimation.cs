using System;
using System.Windows.Media.Animation;
using System.Windows;

namespace Cascade.View
{
    internal class GridLengthAnimation : AnimationTimeline
    {
        static GridLengthAnimation()
        {
            FromProperty = DependencyProperty.Register("From", typeof(GridLength),
                typeof(GridLengthAnimation));

            ToProperty = DependencyProperty.Register("To", typeof(GridLength),
                typeof(GridLengthAnimation));
        }

        public override Type TargetPropertyType
        {
            get
            {
                return typeof(GridLength);
            }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new GridLengthAnimation();
        }

        public static readonly DependencyProperty FromProperty;
        public GridLength From
        {
            get
            {
                return (GridLength)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }

        public static readonly DependencyProperty ToProperty;
        public GridLength To
        {
            get
            {
                return (GridLength)GetValue(ToProperty);
            }
            set
            {
                SetValue(ToProperty, value);
            }
        }

        public override object GetCurrentValue(object defaultOriginValue,
            object defaultDestinationValue, AnimationClock animationClock)
        {
            var fromVal = ((GridLength)GetValue(FromProperty)).Value;
            var toVal = ((GridLength)GetValue(ToProperty)).Value;

            return fromVal > toVal
                       ? new GridLength((1 - animationClock.CurrentProgress.Value)*(fromVal - toVal) + toVal,
                                        GridUnitType.Star)
                       : new GridLength(animationClock.CurrentProgress.Value*(toVal - fromVal) + fromVal,
                                        GridUnitType.Star);
        }
    }
}
