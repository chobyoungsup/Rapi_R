using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QuickChargeDisplay.Converter
{
    public class CurrentTimeTwoUnitPrice : IValueConverter
    {
        #region IValueConverter 멤버

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                return AVRIO.avrio.ChargeUnitPriceArray[(int)value];
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
