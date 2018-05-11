using Microsoft.AppCenter.Analytics;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.System;

namespace WishlistApps
{
    public class MainPageViewModel : BindableBase
    {
        public MainPageViewModel()
        {
            Apps = new ObservableCollection<ApplicationInfo>();
            database = new Database();
            Init();
        }

        private async Task Init()
        {
            var list = await database.GetAll();
            foreach(var item in list)
            {
                Apps.Add(item);
            }
            App.AppAdded += App_AppAdded;
        }

        private async void App_AppAdded(object sender, EventArgs e)
        {
            await CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                Apps.Clear();
                var list = await database.GetAll();
                foreach (var item in list)
                {
                    Apps.Add(item);
                }
            });
        }

        private readonly Database database;

        public ObservableCollection<ApplicationInfo> Apps { get; }

        public async Task Install(ApplicationInfo info)
        {
            await Launcher.LaunchUriAsync(new Uri($"ms-windows-store://pdp/?ProductId={info.AppId}"));
            info.IsInstalledClicked = true;
            await database.SaveAll(Apps.ToList());
            Analytics.TrackEvent("WishlistAppInstall");
        }

        public async Task Remove(ApplicationInfo info)
        {
            Apps.Remove(info);
            await database.Remove(info);
            Analytics.TrackEvent("WishlistAppRemove");
        }
    }
}
