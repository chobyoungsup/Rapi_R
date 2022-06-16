using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QuickChargeDisplay.Converter
{
    public class DoublePrecisionConverter : IValueConverter
    {
        #region IValueConverter 멤버

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double)
            {
                double val = (double)value;

                string formatString = parameter as string;
                if (!string.IsNullOrEmpty(formatString))
                    return string.Format(formatString, val);
                else
                    return val;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
