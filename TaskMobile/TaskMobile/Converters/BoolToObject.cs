using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TaskMobile.Converters
{
    public class BoolToObject<T> : IValueConverter
    {
        public T WhenTrue { set; get; }

        public T WhenFalse { set; get; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? WhenTrue : WhenFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((T)value).Equals(WhenTrue);
        }
    }
}
