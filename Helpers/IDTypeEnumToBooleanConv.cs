using cloudlrc_win.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace cloudlrc_win.Helpers
{
    internal class IDTypeEnumToBooleanConv : IValueConverter
    {
        public IDTypeEnumToBooleanConv() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not String enumString)
                throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");

            if (!Enum.IsDefined(typeof(MainViewModel.IDType), value))
                throw new ArgumentException("ExceptionEnumToBooleanConverterValueMustBeAnEnum");

            var enumValue = Enum.Parse(typeof(MainViewModel.IDType), enumString);

            return enumValue.Equals(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is not String enumString)
                throw new ArgumentException("ExceptionEnumToBooleanConverterParameterMustBeAnEnumName");

            return Enum.Parse(typeof(MainViewModel.IDType), enumString);
        }
    }
}
