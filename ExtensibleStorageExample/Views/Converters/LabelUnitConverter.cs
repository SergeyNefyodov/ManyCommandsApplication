using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace ExtensibleStorageExample.Views.Converters
{
    class LabelUnitConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var typeId = (ForgeTypeId)value;
            return LabelUtils.GetLabelForUnit(typeId);
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