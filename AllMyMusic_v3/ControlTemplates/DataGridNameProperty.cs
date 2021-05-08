using System;
using System.Text;
using System.Windows;

namespace AllMyMusic.View
{
    public class DataGridNameProperty
    {

        public static string GetName(DependencyObject obj)
        {
            return (string)obj.GetValue(NameProperty);
        }

        public static void SetName(DependencyObject obj, string value)
        {
            obj.SetValue(NameProperty, value);
        }

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.RegisterAttached("Name", typeof(string), typeof(DataGridNameProperty), new UIPropertyMetadata(""));

    }
}
