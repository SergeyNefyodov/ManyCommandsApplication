using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using Color = System.Windows.Media.Color;

namespace ExtensibleStorageExample.Views.Converters
{
    class BoolColorConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value! == false ? new SolidColorBrush(Color.FromArgb(120, 255, 0, 0)) : new SolidColorBrush(Color.FromArgb(120, 0, 255, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}