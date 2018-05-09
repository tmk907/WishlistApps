using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace WishlistApps
{
    public class ShareTargetPageViewModel : BindableBase
    {
        private ShareOperation shareOperation;
        private ApplicationInfo AppInfo;

        private string appName = "";
        public string AppName
        {
            get
            {
                return appName;
            }
            set
            {
                Set(ref appName, value);
            }
        }

        private bool isAppOnWishlist = false;
        public bool IsAppOnWishlist
        {
            get
            {
                return isAppOnWishlist;
            }
            set
            {
                Set(ref isAppOnWishlist, value);
            }
        }

        private bool isNewApp = true;
        public bool IsNewApp
        {
            get
            {
                return isNewApp;
            }
            set
            {
                Set(ref isNewApp, value);
            }
        }

        public void SetShareOperation(ShareOperation shareOperation)
        {
            this.shareOperation = shareOperation;
        }

        public async Task OnShareTargetActivated(ShareOperation shareOperation)
        {
            this.shareOperation = shareOperation;
            if (shareOperation.Data.Contains(StandardDataFormats.WebLink))
            {
                // https://www.microsoft.com/store/productId/9NNDJTDMH874

                Uri webLink = await shareOperation.Data.GetWebLinkAsync();

                if (!webLink.ToString().Contains("www.microsoft.com/store"))
                {
                    shareOperation.ReportError("Data doesn't contain link to Microsoft Store.");
                    return;
                }

                string appId = webLink.ToString().Split('/').LastOrDefault();
                var database = new Database();
                var list = await database.GetAll();
                if (list.Any(e=>e.AppId == appId))
                {
                    IsAppOnWishlist = true;
                    IsNewApp = false;
                }
                await CoreApplication.GetCurrentView().Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    AppName = shareOperation.Data.Properties.Title;
                    AppInfo = new ApplicationInfo()
                    {
                        AppId = appId,
                        AppName = AppName,
                        IsInstalledClicked = false,
                    };
                });
            }
        }

        public async void AddApp()
        {
            shareOperation.ReportStarted();
            var database = new Database();
            await database.Add(AppInfo);
            App.InvokeAppAdded();
            shareOperation.ReportCompleted();
        }

        public void Cancel()
        {
            shareOperation.ReportCompleted();
        }
    }
}
