using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WishlistApps
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel VM;

        public MainPage()
        {
            this.InitializeComponent();
            VM = new MainPageViewModel();

            ReviewReminder();
        }

        private async void Button_ClickInstall(object sender, RoutedEventArgs e)
        {
            await VM.Install(((Button)e.OriginalSource).Tag as ApplicationInfo);
        }

        private async void Button_ClickRemove(object sender, RoutedEventArgs e)
        {
            await VM.Remove(((Button)e.OriginalSource).Tag as ApplicationInfo);
        }

        private async Task ReviewReminder()
        {
            await Task.Delay(2000);
            var settings = Windows.Storage.ApplicationData.Current.LocalSettings;

            if (!settings.Values.ContainsKey(SettingsKeys.IsReviewed))
            {
                settings.Values.Add(SettingsKeys.IsReviewed, 0);
                settings.Values.Add(SettingsKeys.LastReviewRemind, DateTime.Today.Ticks);
            }
            else
            {
                int isReviewed = Convert.ToInt32(settings.Values[SettingsKeys.IsReviewed]);
                long dateticks = (long)(settings.Values[SettingsKeys.LastReviewRemind]);
                TimeSpan elapsed = TimeSpan.FromTicks(DateTime.Today.Ticks - dateticks);
                if (isReviewed >= 0 && isReviewed < 8 && TimeSpan.FromDays(5) <= elapsed)
                {
                    settings.Values[SettingsKeys.LastReviewRemind] = DateTime.Today.Ticks;
                    settings.Values[SettingsKeys.IsReviewed] = isReviewed++;

                    MessageDialog dialog = new MessageDialog("If you enjoy using WishlistApps, would you mind taking a moment to rate it? It won't take more than a minute. Thanks for your support!");
                    dialog.Title = "Rate app";
                    dialog.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandInvokedHandler)) { Id = 0 });
                    dialog.Commands.Add(new UICommand("Later") { Id = 1 });
                    dialog.DefaultCommandIndex = 0;
                    dialog.CancelCommandIndex = 1;

                    await dialog.ShowAsync();
                }
            }
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            Review();
        }

        private async Task Review()
        {
            var uri = new Uri("ms-windows-store://review/?ProductId=9N6NL52MLPP9");
            await Launcher.LaunchUriAsync(uri);
        }

        public class SettingsKeys
        {
            public const string IsReviewed = "isreviewed5";
            public const string LastReviewRemind = "lastreviewremind5";
        }
    }
}
