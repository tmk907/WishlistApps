using System;
using Windows.UI.Xaml.Data;

namespace WishlistApps
{
    public class IsInstalledToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isInstalled = (bool)value;
            if (isInstalled)
            {
                return ColorsHelper.GetSolidColorBrush("#FFE81123");
            }
            else
            {
                if (App.Current.RequestedTheme == Windows.UI.Xaml.ApplicationTheme.Dark)
                {
                    return ColorsHelper.GetSolidColorBrush("#FFFFFFFF");
                }
                else
                {
                    return ColorsHelper.GetSolidColorBrush("#FF000000");
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }        
    }
}
