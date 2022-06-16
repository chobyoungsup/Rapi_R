using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Media;

namespace QuickChargeDisplay.Converter
{
    public class FaultColorConverter : IValueConverter
    {
        #region IValueConverter 멤버

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return Brushes.Green;
                else
                    return Brushes.Red;
            }

            return Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
