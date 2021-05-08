using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AllMyMusic.Controls
{
    public class CustomSlider : Slider
    {

        public static readonly DependencyProperty IsDraggingProperty = DependencyProperty.Register("IsDragging", typeof(Boolean), typeof(CustomSlider),
            new FrameworkPropertyMetadata(false) );

        // { BindsTwoWayByDefault = true }

                
        public Boolean IsDragging
        {
            get { return (Boolean)GetValue(IsDraggingProperty); }
            set { SetValue(IsDraggingProperty, value); }
        }


        protected override void OnThumbDragCompleted(System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            IsDragging = false;
            base.OnThumbDragCompleted(e);
        }

        protected override void OnThumbDragStarted(System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            IsDragging = true;
            base.OnThumbDragStarted(e);
        }
    }
}
