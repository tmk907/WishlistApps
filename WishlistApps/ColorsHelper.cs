using Windows.UI.Xaml.Media;

namespace WishlistApps
{
    public class ColorsHelper
    {
        public static SolidColorBrush GetSolidColorBrush(string hex)
        {
            byte a = byte.Parse(hex.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
            byte r = byte.Parse(hex.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
            SolidColorBrush myBrush = new SolidColorBrush(Windows.UI.Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}
