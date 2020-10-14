using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;

namespace AllMyMusic_v3.View
{
    public class ColumnVisibilityToCheckboxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DataGridColumn)
            {
                if ((((DataGridColumn)value).Visibility == System.Windows.Visibility.Collapsed) || (((DataGridColumn)value).Visibility == System.Windows.Visibility.Hidden))
	            {
                    return false;
	            }
                else
                {
                    return true;
                }
            }

            if (value is Visibility)
            {
                if (((Visibility)value == Visibility.Collapsed) ||((Visibility)value == Visibility.Hidden))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
        }

        public object ConvertBack(object value, Type targetType,  object parameter, CultureInfo culture)
        {
            if (value is Boolean)
            {
                if ((Boolean)value == true)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }

            throw new NotImplementedException();
        }
    }   
}
