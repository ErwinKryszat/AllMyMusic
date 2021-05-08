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

namespace AllMyMusic.Controls
{
    /// <summary>
    /// Interaction logic for FormLabelUOM.xaml
    /// </summary>
    public partial class FormLabelUOM : UserControl
    {
        #region Label
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(String), typeof(FormLabelUOM),
            new PropertyMetadata("label"));

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelPadding
        public static readonly DependencyProperty LabelPaddingProperty = DependencyProperty.Register("LabelPadding", typeof(Thickness), typeof(FormLabelUOM),
           new PropertyMetadata(new Thickness(0)));

        public Thickness LabelPadding
        {
            get { return (Thickness)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }
        #endregion

        #region LabelWidth
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(Double), typeof(FormLabelUOM),
            new PropertyMetadata(100.0d));

        public Double LabelWidth
        {
            get { return (Double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }

       
        #endregion

        #region LabelHorizontalContentAlignment
        public static readonly DependencyProperty LabelHorizontalContentAlignmentProperty = DependencyProperty.Register("LabelHorizontalContentAlignment", typeof(HorizontalAlignment), typeof(FormLabelUOM),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment LabelHorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalContentAlignmentProperty); }
            set { SetValue(LabelHorizontalContentAlignmentProperty, value); }
        }
        #endregion


        
        #region Text
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FormLabelUOM));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region ContentMargin
        public static readonly DependencyProperty ContentMarginProperty = DependencyProperty.Register("ContentMargin", typeof(Thickness), typeof(FormLabelUOM),
           new PropertyMetadata(new Thickness(0)));

        public Thickness ContentMargin
        {
            get { return (Thickness)GetValue(ContentMarginProperty); }
            set { SetValue(ContentMarginProperty, value); }
        }
        #endregion

        #region  ContentWidth
        public static readonly DependencyProperty ContentWidthProperty = DependencyProperty.Register("ContentWidth", typeof(Double), typeof(FormLabelUOM),
            new PropertyMetadata(Double.NaN, OnContentWidthPropertyChanged));

        public Double  ContentWidth
        {
            get { return (Double)GetValue( ContentWidthProperty); }
            set { SetValue( ContentWidthProperty, value); }
        }

        private static void OnContentWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FormLabelUOM formLabel = (FormLabelUOM)sender;
            if (Double.IsNaN((Double)e.NewValue) == false)
            {
                formLabel.content.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }


        #endregion


        #region UOM
        public static readonly DependencyProperty UOMProperty = DependencyProperty.Register("UOM", typeof(String), typeof(FormLabelUOM));

        public String UOM
        {
            get { return (String)GetValue(UOMProperty); }
            set { SetValue(UOMProperty, value); }
        }
        #endregion

        public FormLabelUOM()
        {
            InitializeComponent();
        }
    }
}
