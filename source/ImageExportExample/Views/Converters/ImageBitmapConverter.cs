using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace ImageExportExample.Views.Converters
{
    public class ImageBitmapConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return System.Windows.Data.Binding.DoNothing;
            if (value is not Bitmap bitmap) return System.Windows.Data.Binding.DoNothing;

            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);

            var image = new BitmapImage();
            image.BeginInit();
            image.CacheOption = BitmapCacheOption.OnLoad;
            image.StreamSource = memoryStream;
            image.EndInit();

            return image;
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
