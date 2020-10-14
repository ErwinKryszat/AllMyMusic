using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;

namespace AllMyMusic_v3
{
    public class KnobControlDesignData : UserControl
    {
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(KnobControlDesignData),
           new PropertyMetadata(0d));
        public double Angle
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        public static readonly DependencyProperty AngleThumbProperty = DependencyProperty.Register("AngleThumb", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(0d));
        public double AngleThumb
        {
            get { return (double)GetValue(AngleThumbProperty); }
            set { SetValue(AngleThumbProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(0d));
        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty BorderWidthProperty = DependencyProperty.Register("BorderWidth", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(5d));
        public double BorderWidth
        {
            get { return (double)GetValue(BorderWidthProperty); }
            set { SetValue(BorderWidthProperty, value); }
        }


        public static readonly DependencyProperty CenterXProperty = DependencyProperty.Register("CenterX", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(100d));
        public double CenterX
        {
            get { return (double)GetValue(CenterXProperty); }
            set { SetValue(CenterXProperty, value); }
        }

        public static readonly DependencyProperty CenterYProperty = DependencyProperty.Register("CenterY", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(100d));
        public double CenterY
        {
            get { return (double)GetValue(CenterYProperty); }
            set { SetValue(CenterYProperty, value); }
        }

        public static readonly DependencyProperty DialSizeProperty = DependencyProperty.Register("DialSize", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(20d));
        public double DialSize
        {
            get { return (double)GetValue(DialSizeProperty); }
            set { SetValue(DialSizeProperty, value); }
        }

        public static readonly DependencyProperty DialMarginProperty = DependencyProperty.Register("DialMargin", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(10d));
        public double DialMargin
        {
            get { return (double)GetValue(DialMarginProperty); }
            set { SetValue(DialMarginProperty, value); }
        }

        // this should be DialSize / 2
        public static readonly DependencyProperty DialCenterXProperty = DependencyProperty.Register("DialCenterX", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(10d));
        public double DialCenterX
        {
            get { return (double)GetValue(DialCenterXProperty); }
            set { SetValue(DialCenterXProperty, value); }
        }

        public static readonly DependencyProperty DialCenterYProperty = DependencyProperty.Register("DialCenterY", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(10d));
        public double DialCenterY
        {
            get { return (double)GetValue(DialCenterYProperty); }
            set { SetValue(DialCenterYProperty, value); }
        }

        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(0d));
        public double Minimum
        {
            get { return (double)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(100d));
        public double Maximum
        {
            get { return (double)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        public static readonly DependencyProperty MinAngleProperty = DependencyProperty.Register("MinAngle", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(-135d));
        public double MinAngle
        {
            get { return (double)GetValue(MinAngleProperty); }
            set { SetValue(MinAngleProperty, value); }
        }

        public static readonly DependencyProperty MaxAngleProperty = DependencyProperty.Register("MaxAngle", typeof(double), typeof(KnobControlDesignData),
            new PropertyMetadata(135d));
        public double MaxAngle
        {
            get { return (double)GetValue(MaxAngleProperty); }
            set { SetValue(MaxAngleProperty, value); }
        }


        public static readonly DependencyProperty ScalingProperty = DependencyProperty.Register("Scaling", typeof(DoubleCollection), typeof(KnobControlDesignData));
        public DoubleCollection Scaling
        {
            get { return (DoubleCollection)GetValue(ScalingProperty); }
            set { SetValue(ScalingProperty, value); }
        }

        public KnobControlDesignData()
        {
            Scaling = new DoubleCollection();
            Scaling.Add(00);
            Scaling.Add(10);
            Scaling.Add(20);
            Scaling.Add(30);
            Scaling.Add(40);
            Scaling.Add(50);
            Scaling.Add(60);
            Scaling.Add(70);
            Scaling.Add(80);
            Scaling.Add(90);
            Scaling.Add(100);
            Scaling.Add(110);
            Scaling.Add(120);
            Scaling.Add(130);
            Scaling.Add(140);
            Scaling.Add(150);
            Scaling.Add(160);
            Scaling.Add(170);
            Scaling.Add(180);
            Scaling.Add(190);
            Scaling.Add(200);
            Scaling.Add(210);
            Scaling.Add(220);
            Scaling.Add(230);
            Scaling.Add(240);
            Scaling.Add(250);
            Scaling.Add(260);
            Scaling.Add(270);
            Scaling.Add(280);
            Scaling.Add(290);
            Scaling.Add(300);
            Scaling.Add(310);
            Scaling.Add(320);
            Scaling.Add(330);
            Scaling.Add(340);
            Scaling.Add(350);
            
        }
       
    }
}
