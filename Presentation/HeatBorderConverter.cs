using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace Presentation
{
    class HeatBorderConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte status = (byte)value;
            if (status == 0)
            {
                return new SolidColorBrush(Colors.DarkBlue);
            }
            else
            {
                return new SolidColorBrush(Colors.OrangeRed);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
