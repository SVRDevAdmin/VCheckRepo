using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VCheckViewer.Converter
{
    public class IndexedConverter : IValueConverter
    {
        public object Convert
            (object value
            , Type targetType
            , object parameter
            , CultureInfo culture
            )
        {
            int index = (System.Convert.ToInt32(value)) + App.MainViewModel.CurrentUserIndexStart;

            return index;

        }

        public object ConvertBack(object value, Type targetType,
            object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
