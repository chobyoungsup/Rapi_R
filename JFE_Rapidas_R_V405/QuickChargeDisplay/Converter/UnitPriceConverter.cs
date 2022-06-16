using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;

namespace QuickChargeDisplay.Converter
{
    public class UnitPriceConverter : IValueConverter
    {
        #region IValueConverter 멤버

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double[])
            {
                int currentTime = DateTime.Now.Hour;
                double[] unitPrices = value as double[];

                return unitPrices[currentTime];
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
