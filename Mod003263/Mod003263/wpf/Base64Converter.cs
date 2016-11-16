using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;

/**
 * Author: Nick Guy
 * Date: 16/11/2016
 * Contains: Base64Converter
 */
namespace Mod003263.wpf {

    /// <summary>
    /// Generates image data from base64 string
    /// </summary>
    public class Base64Converter : IValueConverter
    {

        public const String TEST_IMAGE =
            "data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAEwAAABMCAYAAADHl1ErAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyRpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpBNjVDOEFGMkRFQjkxMUU1QjNCQUYyOTdBMEFEODYyQyIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpBNjVDOEFGM0RFQjkxMUU1QjNCQUYyOTdBMEFEODYyQyI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOjE4MjIwREUxREVCNTExRTVCM0JBRjI5N0EwQUQ4NjJDIiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOjE4MjIwREUyREVCNTExRTVCM0JBRjI5N0EwQUQ4NjJDIi8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+enDCHgAAAsFJREFUeNrsnEmuGjEQhotZzKOQQCCx4VbJFbJLdrlAcoXs3sVAAjE3k5inuJywSvLU7slN/P+SEQtatL+uKperrI7M5/MkEX0X44MYFYL+pqUYb2J8jouPb2J8ApN3Vf7N6BgRFraAZdmWFQUsJVWiYKAmAAMwAAMwAAMwCMAADMAADMAgAAMwAAuX4mG4icfjQYfDgbbbLe12Ozoej3S/3ymVSlE2m6VyuUzpdDoUwLji+tB5A6vViqbTKZ1Op3+7QTRKjUaDSqWS/G4ssOFwSJZl2b/ZSISazSZVKvqKxNoeF4NSgfV0XbZGI4P+crl0dN3lcpHxzihgHNDdTJoXBaOAXa9X6V6OA6+IZcZZmKtcKB43C5hbCzEOWCKRcAXNOGCcfCaTSewlVWLY7XYLPCV5WWD7/V6ulE612WzMc0m3aYlxQd+NYrGYecDcTFpnqUfbXrJerztOKWq1mnnAeNKdTkfpGk5Fut2uLCwaB4ylOvF8Pq81adUOTDXbd7tYvDwwthYVi9HpiqGxsHa7beu3hUJBDqOBPbdJdlIMTnbfa5QYAYz3k4PBwNa+krtL/X7fVeHxpYHxxBmAyiacLWy9XmsFpmWN5olPJhPZtFXVaDSSbpzL5bSUiALrS3K3h8sy7FpexaJMJiO74kE2eH0Hxt0h8R/SlfyKP7xoVKtVOfxObH0DxuckZrOZI7dzk6Zw6sHg+ExG6IGdz2daLBbSmtgFdW+72FV5eBnrPAHGQXg8HktYYVSxWKRWq+VJnPPE4Xu9XqCupyq2eH6oqtUR3/IwrtGHXV6tzJ4A0519v1ymr/uQW5AP1ZOZ6jwcorJv/S+qFUFWRQBMg7QfCjYy6AMYBGAABmAABmAQgAEYgAEYgAEYBGAAphOYBQy2JV9L+gYOtvWDG7lfxOAuxkf69UZc6E89X6389acAAwAgexefn3e9ZwAAAABJRU5ErkJggg==";

        /// <summary>
        /// Helper method to reduce source-code footprint and to handle casting
        /// </summary>
        /// <param name="base64">The base64-encoded image</param>
        /// <returns>Bitmap image of the provided base64</returns>
        public BitmapImage Convert(String base64) {
            return Convert(base64, null, null, null) as BitmapImage;
        }

        /// <summary>
        /// Converts a base64 string to a <see cref="BitmapImage" />
        /// </summary>
        /// <param name="value">The base64-encoded image</param>
        /// <param name="targetType">[Not used]</param>
        /// <param name="parameter">[Not used]</param>
        /// <param name="culture">[Not used]</param>
        /// <returns>Bitmap image of the provided base64</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null) return null;
            String s = value.ToString();
            BitmapImage img = new BitmapImage();

            img.BeginInit();
            img.StreamSource = new MemoryStream(System.Convert.FromBase64String(s));
            img.EndInit();

            return img;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
