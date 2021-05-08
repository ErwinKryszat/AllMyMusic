using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;



namespace AllMyMusic.Controls
{
    /// <summary>
    /// Interaction logic for knob.xaml
    /// </summary>
    public partial class KnobControl : UserControl
    {
        private Boolean _leftButtonDown;

        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(KnobControl),
            new PropertyMetadata(0d));
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleThumbProperty = DependencyProperty.Register("AngleThumb", typeof(double), typeof(KnobControl),
            new PropertyMetadata(0d));
        public double AngleThumb
        {
            get { return (double)GetValue(AngleThumbProperty); }
            set { SetValue(AngleThumbProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(float), typeof(KnobControl),
            new FrameworkPropertyMetadata(0f, OnValuePropertyChanged) { BindsTwoWayByDefault = true });
  
        public float Value
        {
            get { return (float)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }
        private static void OnValuePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            // Calculate the width for the rectangle that hides the stars partly
            KnobControl kc = (KnobControl)sender;

            // only set the Angle once when we initialize the Value property
            if (kc.Angle == 0)
            {
                double angleRange = kc.MaxAngle - kc.MinAngle;
                double range = kc.Maximum - kc.Minimum;
                kc.Angle = kc.MinAngle + (angleRange / range * kc.Value);
            }
        }

        public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register("BorderWidth", typeof(double), typeof(KnobControl),
          new PropertyMetadata(5d));
        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }


        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(KnobControl),
            new PropertyMetadata(200d));
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(KnobControl),
            new PropertyMetadata(200d));
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        public static readonly DependencyProperty DialSizeProperty = DependencyProperty.Register("DialSize", typeof(double), typeof(KnobControl),
            new PropertyMetadata(10d));
        public double DialSize
        {
            get { return (double)GetValue(DialSizeProperty); }
            set { SetValue(DialSizeProperty, value); }
        }

        public static readonly DependencyProperty DialMarginProperty = DependencyProperty.Register("DialMargin", typeof(double), typeof(KnobControl),
            new PropertyMetadata(10d));
        public double DialMargin
        {
            get { return (double)GetValue(DialMarginProperty); }
            set { SetValue(DialMarginProperty, value); }
        }

        // this should be DialSize / 2
        public static readonly DependencyProperty DialCenterXProperty = DependencyProperty.Register("DialCenterX", typeof(double), typeof(KnobControl),
            new PropertyMetadata(10d));
        public double DialCenterX
        {
            get { return (double)GetValue(DialCenterXProperty); }
            set { SetValue(DialCenterXProperty, value); }
        }

        public static readonly DependencyProperty DialCenterYProperty = DependencyProperty.Register("DialCenterY", typeof(double), typeof(KnobControl),
            new PropertyMetadata(10d));
        public double DialCenterY
        {
            get { return (double)GetValue(DialCenterYProperty); }
            set { SetValue(DialCenterYProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(KnobControl),
            new PropertyMetadata(0d));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }
       
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(KnobControl),
            new PropertyMetadata(1d));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", typeof(double), typeof(KnobControl),
            new PropertyMetadata(-125d));
        public double MinAngle
        {
            get { return (double)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }

        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", typeof(double), typeof(KnobControl),
            new PropertyMetadata(125d));
        public double MaxAngle
        {
            get { return (double)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }


        public static readonly DependencyProperty ScalingProperty = DependencyProperty.Register("Scaling", typeof(ObservableCollection<double>), typeof(KnobControl));
        public ObservableCollection<double> Scaling
        {
            get { return (ObservableCollection<double>)GetValue(ScalingProperty); }
            set { SetValue(ScalingProperty, value); }
        }

        public static readonly DependencyProperty ScalingStepSizeProperty = DependencyProperty.Register("ScalingStepSize", typeof(double), typeof(KnobControl),
           new PropertyMetadata(25d));
        public double ScalingStepSize
        {
            get { return (double)GetValue(ScalingStepSizeProperty); }
            set { SetValue(ScalingStepSizeProperty, value); }
        }

        public static readonly DependencyProperty StrokeCenterYProperty = DependencyProperty.Register("StrokeCenterY", typeof(double), typeof(KnobControl),
            new PropertyMetadata(200d));
        public double StrokeCenterY
        {
            get { return (double)GetValue(StrokeCenterYProperty); }
            set { SetValue(StrokeCenterYProperty, value); }
        }


        public KnobControl()
        {
            InitializeComponent();
            //this.DataContext = this;
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.CenterX = this.ActualHeight / 2 - this.BorderWidth;
            this.CenterY = this.ActualWidth / 2 - this.BorderWidth;

            this.StrokeCenterY = this.ActualWidth / 2;

            this.DialCenterX = this.DialSize / 2;
            this.DialCenterY = this.DialSize / 2;

            AddScaling();
        }
        private void AddScaling()
        {
            Scaling = new ObservableCollection<double>();
            for (double i = MinAngle; i <= MaxAngle; i += ScalingStepSize)
            {
                Scaling.Add(i);
            }
        }

        private void My_MouseMove(object sender, MouseEventArgs e)
        {
            if (_leftButtonDown)
            {
                Point newPos = e.GetPosition(mainGrid);
                CalculateAngle(newPos);
                CalculateValue();
            }
            e.Handled = true;
        }
        private void My_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_leftButtonDown == false)
            {
                Point newPos = e.GetPosition(mainGrid);
                CalculateAngle(newPos);
                CalculateValue();
            }
            _leftButtonDown = true;

            e.Handled = true;
        }
        private void My_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _leftButtonDown = false;
            e.Handled = true;
        }

        private void CalculateAngle(Point newPos)
        {
            Size gridSize = new Size(mainGrid.ActualWidth, mainGrid.ActualHeight);

            double a = GetAngle(newPos, gridSize);


            if (a < MinAngle)
            {
                a = MinAngle;
            }
            else if (a > MaxAngle)
            {
                a = MaxAngle;
            }

            Angle = a;

            AngleThumb = -1d * Angle;
        }
        public enum Quadrants : int { nw = 2, ne = 1, sw = 4, se = 3 }
        private double GetAngle(Point touchPoint, Size circleSize)
        {
            var _X = touchPoint.X - (circleSize.Width / 2d);
            var _Y = circleSize.Height - touchPoint.Y - (circleSize.Height / 2d);
            var _Hypot = Math.Sqrt(_X * _X + _Y * _Y);
            var _Value = Math.Asin(_Y / _Hypot) * 180 / Math.PI;
            var _Quadrant = (_X >= 0) ?
                (_Y >= 0) ? Quadrants.ne : Quadrants.se :
                (_Y >= 0) ? Quadrants.nw : Quadrants.sw;

            switch (_Quadrant)
            {
                case Quadrants.ne: _Value = 090 - _Value; break;
                case Quadrants.nw: _Value = -90 + _Value; break;
                case Quadrants.se: _Value = 090 - _Value; break;
                case Quadrants.sw: _Value = -90 + _Value; break;
            }
            return _Value;
        }
        private void CalculateValue()
        {
            double angleRange = MaxAngle - MinAngle;
            double range = Maximum - Minimum;

            // Y = m * x + b
            Value = (float) (range / angleRange * Angle + range / 2d);
            
        }
    }
 
}
