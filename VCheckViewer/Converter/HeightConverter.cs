using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VCheckViewer.Converter
{
    public class HeightConverter : IValueConverter
    {
        public double Portion { get; set; } = 0.5; // Default to 50%

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double mainWindowHeight = App.Current.MainWindow.Height;

            return mainWindowHeight * Portion;

            //if (value is double windowHeight)
            //{
            //    return windowHeight * Portion;
            //}
            //return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
