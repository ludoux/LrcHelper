using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace cloudlrc_win.Helpers
{
    internal class ImageConvert : IValueConverter
    {
        public ImageConvert() { }
        /// <summary>
        /// 转换图片（解决图片被占用问题）
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage bitmapImage = null;
            using (BinaryReader reader = new BinaryReader(File.Open(value.ToString(), FileMode.Open)))
            {
                try
                {
                    FileInfo fi = new FileInfo(value.ToString());
                    byte[] bytes = reader.ReadBytes((int)fi.Length);
                    reader.Close();

                    bitmapImage = new BitmapImage();
                    bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

                    bitmapImage.BeginInit();
                    bitmapImage.StreamSource = new MemoryStream(bytes);
                    bitmapImage.EndInit();
                }
                catch (Exception) { }
            }
            return bitmapImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
