using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace AllMyMusic.Controls
{
    public class EditComboBox : ComboBox
    {
        public static readonly DependencyProperty ItemListChangedProperty = DependencyProperty.Register("ItemListChanged", typeof(Boolean), typeof(EditComboBox), new FrameworkPropertyMetadata(false));
        public Boolean ItemListChanged
        {
            get { return (Boolean)GetValue(ItemListChangedProperty); }
            set { SetValue(ItemListChangedProperty, value); }
        }

        private String _oldText = String.Empty;
        private String _previousSelection = String.Empty;

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if ((String)this.SelectedItem != this.Text)
            {
                if (((ObservableCollection<String>)this.ItemsSource).Count > 0)
                {
                    ObservableCollection<String> itemList = (ObservableCollection<String>)this.ItemsSource;
                    if (this.SelectedItem == null)
                    {
                        // User has deleted a row
                        if ((String.IsNullOrEmpty(this.Text) == true) && (String.IsNullOrEmpty(_previousSelection) == false))
                        {
                            itemList.Remove(_previousSelection);
                            ItemListChanged = true;
                            _oldText = String.Empty;
                        }

                        // User starts editing a row
                        if ((String.IsNullOrEmpty(this.Text) == false) && (String.IsNullOrEmpty(_oldText) == true))
                        {
                            ItemListChanged = true;
                            itemList.Add(this.Text);
                            this.SelectedIndex = itemList.Count - 1;
                        }

                        // User is editing a row
                        if (String.IsNullOrEmpty(_oldText) == false)
                        {
                            ItemListChanged = true;
                            int index = itemList.IndexOf(_oldText);
                            if (index >= 0)
                            {
                                itemList[index] = this.Text;
                                this.SelectedIndex = index;
                            }
                        }

                        this.ItemsSource = itemList;
                    }
                }
                else
                {
                    ObservableCollection<String> itemList = new ObservableCollection<string>();
                    itemList.Add(this.Text);
                    this.ItemsSource = itemList;
                    this.SelectedIndex = 0;
                    ItemListChanged = true;
                }
                _oldText = this.Text;
            }

        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            if (e.RemovedItems.Count > 0)
            {
                _previousSelection = (String)e.RemovedItems[0];
            }
        }
    }
}
