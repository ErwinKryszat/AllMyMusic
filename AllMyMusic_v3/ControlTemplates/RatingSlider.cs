using System;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media;
using System.Windows.Media.Imaging;

using AllMyMusic.ViewModel;

namespace AllMyMusic.View
{
    public class RatingSlider : Slider
    {
        #region Fields

        #endregion // Fields

        #region Attached Properties
        public static readonly DependencyProperty RemainingValueProperty = DependencyProperty.Register("RemainingValue", typeof(double), typeof(RatingSlider), new PropertyMetadata(0d));
        public double RemainingValue
        {
            get { return (double)GetValue(RemainingValueProperty); }
            set { SetValue(RemainingValueProperty, value); }
        }

        public static readonly DependencyProperty TagValueProperty = DependencyProperty.Register("TagValue", typeof(Int32), typeof(RatingSlider),
            new FrameworkPropertyMetadata(0, OnTagValuePropertyChanged) { BindsTwoWayByDefault = true });

        public Int32 TagValue
        {
            get { return (Int32)GetValue(TagValueProperty); }
            set { SetValue(TagValueProperty, value); }
        }

        public static readonly DependencyProperty ForegroundRectColorProperty = DependencyProperty.Register("ForegroundRectColor", typeof(Brush), typeof(RatingSlider), new PropertyMetadata(null));
        public Brush ForegroundRectColor
        {
            get { return (Brush)GetValue(ForegroundRectColorProperty); }
            set { SetValue(ForegroundRectColorProperty, value); }
        }
        #endregion


        #region Presentation Properties

        #endregion // Presentation Properties

        #region Events

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            base.OnValueChanged(oldValue, newValue);

            double resizeFactor = (this.Maximum - newValue) / this.Maximum;
            double hidingRectWidth = (this.ActualWidth) * resizeFactor;
            this.RemainingValue = hidingRectWidth;

            if ((Value >= 0.0) && (Value <= 0.5)) { TagValue = 0; }
            if ((Value > 0.5) && (Value <= 1.5)) { TagValue = 26; }
            if ((Value > 1.5) && (Value <= 2.5)) { TagValue = 51; }
            if ((Value > 2.5) && (Value <= 3.5)) { TagValue = 77; }
            if ((Value > 3.5) && (Value <= 4.5)) { TagValue = 102; }
            if ((Value > 4.5) && (Value <= 5.5)) { TagValue = 128; }
            if ((Value > 5.5) && (Value <= 6.5)) { TagValue = 153; }
            if ((Value > 6.5) && (Value <= 7.5)) { TagValue = 179; }
            if ((Value > 7.5) && (Value <= 8.5)) { TagValue = 204; }
            if ((Value > 8.5) && (Value <= 9.5)) { TagValue = 230; }
            if ((Value > 9.5) && (Value <= 10.0)) { TagValue = 255; }
        }



        private static void OnTagValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Calculate the width for the rectangle that hides the stars partly
            RatingSlider ratingSlider = (RatingSlider)sender;

            Int32 tagValue = (Int32)e.NewValue;

            if ((tagValue >= 0) && (tagValue <= 19)) { ratingSlider.TagValue = 0; ratingSlider.Value = 0; }
            if ((tagValue >= 20) && (tagValue <= 43)) { ratingSlider.TagValue = 26; ratingSlider.Value = 1; }
            if ((tagValue >= 44) && (tagValue <= 67)) { ratingSlider.TagValue = 51; ratingSlider.Value = 2; }
            if ((tagValue >= 68) && (tagValue <= 91)) { ratingSlider.TagValue = 77; ratingSlider.Value = 3; }
            if ((tagValue >= 92) && (tagValue <= 115)) { ratingSlider.TagValue = 102; ratingSlider.Value = 4; }
            if ((tagValue >= 116) && (tagValue <= 139)) { ratingSlider.TagValue = 128; ratingSlider.Value = 5; }
            if ((tagValue >= 140) && (tagValue <= 163)) { ratingSlider.TagValue = 153; ratingSlider.Value = 6; }
            if ((tagValue >= 164) && (tagValue <= 187)) { ratingSlider.TagValue = 179; ratingSlider.Value = 7; }
            if ((tagValue >= 188) && (tagValue <= 211)) { ratingSlider.TagValue = 204; ratingSlider.Value = 8; }
            if ((tagValue >= 212) && (tagValue <= 235)) { ratingSlider.TagValue = 230; ratingSlider.Value = 9; }
            if ((tagValue >= 236) && (tagValue <= 255)) { ratingSlider.TagValue = 255; ratingSlider.Value = 10; }
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            double resizeFactor = (this.Maximum - this.Value) / this.Maximum;
            double hidingRectWidth = (this.ActualWidth - 2) * resizeFactor;
            this.RemainingValue = hidingRectWidth;
        }
      
        #endregion

        public RatingSlider()
        {
            MyDefaults();
        }

        private void MyDefaults()
        {
            this.Minimum = 0;
            this.Maximum = 10;
            this.IsSnapToTickEnabled = true;
            this.TickFrequency = 1;
        }

    }
}
