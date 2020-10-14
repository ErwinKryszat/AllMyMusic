using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using AllMyMusic_v3.View;

namespace AllMyMusic_v3.Controls
{
    /// <summary>
    /// Interaction logic for FormRating.xaml
    /// </summary>
    public partial class FormRating : UserControl
    {
        #region Label
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(String), typeof(FormRating),
            new PropertyMetadata("label"));

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelPadding
        public static readonly DependencyProperty LabelPaddingProperty = DependencyProperty.Register("LabelPadding", typeof(Thickness), typeof(FormRating),
           new PropertyMetadata(new Thickness(0)));

        public Thickness LabelPadding
        {
            get { return (Thickness)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }
        #endregion

        #region LabelWidth
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(Double), typeof(FormRating),
            new PropertyMetadata(100.0d));

        public Double LabelWidth
        {
            get { return (Double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        #endregion

        #region LabelHorizontalContentAlignment
        public static readonly DependencyProperty LabelHorizontalContentAlignmentProperty = DependencyProperty.Register("LabelHorizontalContentAlignment", typeof(HorizontalAlignment), typeof(FormRating),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment LabelHorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalContentAlignmentProperty); }
            set { SetValue(LabelHorizontalContentAlignmentProperty, value); }
        }
        #endregion


        #region Rating

        /// <summary>
        /// This TagValue is save in the the ID3 tag, range is 0-255. It is calculted from the Value
        /// </summary>
        public static readonly DependencyProperty RatingProperty = DependencyProperty.Register("Rating", typeof(Int32), typeof(FormRating),
            new PropertyMetadata((int)0,OnRatingPropertyChanged));

        public Int32 Rating
        {
            get { return (Int32)GetValue(RatingProperty); }
            set { SetValue(RatingProperty, value); }
        }

        private static void OnRatingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FormRating formRating = (FormRating)sender;
            if (formRating.sliderUpdating == false)
            {
                formRating.ratingSlider.TagValue = (Int32)e.NewValue;

                //if (int.IsNaN((Double)e.NewValue) == false)
                //{
                    
                //}
            }
            else
            {
                formRating.sliderUpdating = false;
            }
        }

        #endregion


        private Boolean sliderUpdating = false;
        public FormRating()
        {
            InitializeComponent();
        }

        private void ratingSlider_ValueChanged(object sender, RoutedEventArgs e)
        {
            sliderUpdating = true;
            RatingSlider r = (RatingSlider)sender;
            Rating = r.TagValue;
        }
    }
}
