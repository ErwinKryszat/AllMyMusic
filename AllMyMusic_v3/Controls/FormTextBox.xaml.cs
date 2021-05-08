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
    /// Interaction logic for FormTextBox.xaml
    /// </summary>
    public partial class FormTextBox : UserControl
    {
        #region Label
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(String), typeof(FormTextBox),
            new PropertyMetadata("label"));

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelPadding
        public static readonly DependencyProperty LabelPaddingProperty = DependencyProperty.Register("LabelPadding", typeof(Thickness), typeof(FormTextBox),
           new PropertyMetadata(new Thickness(0)));

        public Thickness LabelPadding
        {
            get { return (Thickness)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }
        #endregion

        #region LabelWidth
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(Double), typeof(FormTextBox),
            new PropertyMetadata(100.0d));

        public Double LabelWidth
        {
            get { return (Double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        #endregion

        #region LabelHorizontalContentAlignment
        public static readonly DependencyProperty LabelHorizontalContentAlignmentProperty = DependencyProperty.Register("LabelHorizontalContentAlignment", typeof(HorizontalAlignment), typeof(FormTextBox),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment LabelHorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalContentAlignmentProperty); }
            set { SetValue(LabelHorizontalContentAlignmentProperty, value); }
        }
        #endregion

        #region TextBoxText
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FormTextBox));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region TextBoxMargin
        public static readonly DependencyProperty TextBoxMarginProperty = DependencyProperty.Register("TextBoxMargin", typeof(Thickness), typeof(FormTextBox),
           new PropertyMetadata(new Thickness(0)));

        public Thickness TextBoxMargin
        {
            get { return (Thickness)GetValue(TextBoxMarginProperty); }
            set { SetValue(TextBoxMarginProperty, value); }
        }
        #endregion

        #region TextBoxWidth
        public static readonly DependencyProperty TextBoxWidthProperty = DependencyProperty.Register("TextBoxWidth", typeof(Double), typeof(FormTextBox),
            new PropertyMetadata(Double.NaN, OnTextBoxWidthPropertyChanged));

        public Double TextBoxWidth
        {
            get { return (Double)GetValue(TextBoxWidthProperty); }
            set { SetValue(TextBoxWidthProperty, value); }
        }

        private static void OnTextBoxWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FormTextBox formTextBox = (FormTextBox)sender;
            if (Double.IsNaN((Double)e.NewValue) == false)
            {
                formTextBox.textbox.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }
        #endregion

        #region TextBoxHeight
        public static readonly DependencyProperty TextBoxHeightProperty = DependencyProperty.Register("TextBoxHeight", typeof(Double), typeof(FormTextBox),
            new PropertyMetadata(Double.NaN, OnTextBoxHeightPropertyChanged));

        public Double TextBoxHeight
        {
            get { return (Double)GetValue(TextBoxHeightProperty); }
            set { SetValue(TextBoxHeightProperty, value); }
        }

        private static void OnTextBoxHeightPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            FormTextBox formTextBox = (FormTextBox)sender;
            if (Double.IsNaN((Double)e.NewValue) == false)
            {
                formTextBox.textbox.HorizontalAlignment = HorizontalAlignment.Left;
                formTextBox.textbox.AcceptsReturn = true;
            }
        }
        #endregion

        #region events
        public delegate void TextBoxClickEventHandler(object sender, EventArgs e);
  
        // declare the event
        public event TextBoxClickEventHandler TextBoxClicked;

        private void textbox_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TextBoxClicked != null)
            {
                TextBoxClicked(this, new EventArgs());
            }
        }
    

        #endregion

        public FormTextBox()
        {
            InitializeComponent();
        }

      

      
    }
}
