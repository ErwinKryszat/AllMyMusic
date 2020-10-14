using System;
using System.Collections.ObjectModel;
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

namespace AllMyMusic_v3.Controls
{
    /// <summary>
    /// Interaction logic for FormComboBox.xaml
    /// </summary>
    public partial class FormComboBox : UserControl
    {
        #region ItemList
        public static readonly DependencyProperty ItemListProperty = DependencyProperty.Register("ItemList", typeof(ObservableCollection<String>), typeof(FormComboBox));

        public ObservableCollection<String> ItemList
        {
            get { return (ObservableCollection<String>)GetValue(ItemListProperty); }
            set { SetValue(ItemListProperty, value); }
        }
        #endregion

        #region Label
        public static readonly DependencyProperty LabelProperty = DependencyProperty.Register("Label", typeof(String), typeof(FormComboBox),
            new PropertyMetadata("label"));

        public String Label
        {
            get { return (String)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
        #endregion

        #region LabelPadding
        public static readonly DependencyProperty LabelPaddingProperty = DependencyProperty.Register("LabelPadding", typeof(Thickness), typeof(FormComboBox),
           new PropertyMetadata(new Thickness(0)));

        public Thickness LabelPadding
        {
            get { return (Thickness)GetValue(LabelPaddingProperty); }
            set { SetValue(LabelPaddingProperty, value); }
        }
        #endregion

        #region LabelWidth
        public static readonly DependencyProperty LabelWidthProperty = DependencyProperty.Register("LabelWidth", typeof(Double), typeof(FormComboBox),
            new PropertyMetadata(100.0d));

        public Double LabelWidth
        {
            get { return (Double)GetValue(LabelWidthProperty); }
            set { SetValue(LabelWidthProperty, value); }
        }
        #endregion

        #region LabelHorizontalContentAlignment
        public static readonly DependencyProperty LabelHorizontalContentAlignmentProperty = DependencyProperty.Register("LabelHorizontalContentAlignment", typeof(HorizontalAlignment), typeof(FormComboBox),
            new PropertyMetadata(HorizontalAlignment.Left));

        public HorizontalAlignment LabelHorizontalContentAlignment
        {
            get { return (HorizontalAlignment)GetValue(LabelHorizontalContentAlignmentProperty); }
            set { SetValue(LabelHorizontalContentAlignmentProperty, value); }
        }
        #endregion

        #region ComboBoxText
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(String), typeof(FormComboBox));

        public String Text
        {
            get { return (String)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        #endregion

        #region ComboBoxMargin
        public static readonly DependencyProperty ComboBoxMarginProperty = DependencyProperty.Register("ComboBoxMargin", typeof(Thickness), typeof(FormComboBox),
           new PropertyMetadata(new Thickness(0)));

        public Thickness ComboBoxMargin
        {
            get { return (Thickness)GetValue(ComboBoxMarginProperty); }
            set { SetValue(ComboBoxMarginProperty, value); }
        }
        #endregion

        #region ComboBoxWidth
        public static readonly DependencyProperty ComboBoxWidthProperty = DependencyProperty.Register("ComboBoxWidth", typeof(Double), typeof(FormComboBox),
            new PropertyMetadata(Double.NaN, OnComboBoxWidthPropertyChanged));

        public Double ComboBoxWidth
        {
            get { return (Double)GetValue(ComboBoxWidthProperty); }
            set { SetValue(ComboBoxWidthProperty, value); }
        }

        private static void OnComboBoxWidthPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            FormComboBox formCombo = (FormComboBox)sender;


            if (Double.IsNaN((Double)e.NewValue) == false)
            {
                formCombo.comboBox.HorizontalAlignment = HorizontalAlignment.Left;
            }
        }

        #endregion


        #region SelectedItem
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(Object), typeof(FormComboBox),
           new PropertyMetadata(new Thickness(0)));

        public Object SelectedItem
        {
            get { return (Object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        

        public FormComboBox()
        {
            InitializeComponent();
        }


        private void comboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if ((String)comboBox.SelectedItem != comboBox.Text)
            {
                if (((ObservableCollection<String>)comboBox.ItemsSource).Count > 0)
                {
                    ObservableCollection<String> itemList = (ObservableCollection<String>)comboBox.ItemsSource;
                    itemList.Add(comboBox.Text);
                    comboBox.SelectedItem = comboBox.Text;
                }
                else
                {
                    ObservableCollection<String> itemList = new ObservableCollection<string>();
                    itemList.Add(comboBox.Text);
                    comboBox.ItemsSource = itemList;
                    comboBox.SelectedItem = comboBox.Text;
                }
            }
        }

        private String _oldText = String.Empty;
        private void comboBox_KeyUp(object sender, KeyEventArgs e)
        {
            if ((String)comboBox.SelectedItem != comboBox.Text)
            {
                if (((ObservableCollection<String>)comboBox.ItemsSource).Count > 0)
                {
                    ObservableCollection<String> itemList = (ObservableCollection<String>)comboBox.ItemsSource;
                    if (comboBox.SelectedItem == null)
                    {
                        if (String.IsNullOrEmpty(_oldText) == true)
                        {
                            itemList.Add(comboBox.Text);
                            comboBox.SelectedIndex = itemList.Count - 1;
                        }
                        else
                        {
                            int index = itemList.IndexOf(_oldText);
                            if (index >= 0)
                            {
                                itemList[index] = comboBox.Text;
                                comboBox.SelectedIndex = index;
                            }  
                        } 
                        comboBox.ItemsSource = itemList; 
                    }
                }
                else
                {
                    ObservableCollection<String> itemList = new ObservableCollection<string>();
                    itemList.Add(comboBox.Text);
                    comboBox.ItemsSource = itemList;
                    comboBox.SelectedIndex = 0;
                }
                _oldText = comboBox.Text;
            }
        }
    }
}
