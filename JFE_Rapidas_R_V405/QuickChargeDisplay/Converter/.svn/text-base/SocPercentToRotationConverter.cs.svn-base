using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QuickChargeDisplay.Converter
{
    public class SocPercentToRotationConverter : IValueConverter
    {
        #region IValueConverter 멤버

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            double soc = (double)value;

            soc = soc / 2.0 * -1.0;

            if (soc > 0) soc = 0;
            if (soc < -100) soc = -100;

            return soc;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
