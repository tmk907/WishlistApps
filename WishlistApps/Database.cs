using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace WishlistApps
{
    public class Database
    {
        private string filename = "wishlist.json";
        private StorageFolder storageFolder = ApplicationData.Current.LocalFolder;

        public async Task Add(ApplicationInfo info)
        {
            var list = await ReadData();
            if (list.Any(e=>e.AppId == info.AppId))
            {
                return;
            }
            list.Add(info);
            await SaveData(list);
        }

        public async Task Remove(ApplicationInfo info)
        {
            var list = await ReadData();
            var toRemove = list.FirstOrDefault(e => e.AppId == info.AppId);
            if (toRemove == null)
            {
                return;
            }
            list.Remove(toRemove);
            await SaveData(list);
        }

        public async Task<List<ApplicationInfo>> GetAll()
        {
            var list = await ReadData();
            return list;
        }

        public async Task SaveAll(List<ApplicationInfo> list)
        {
            await SaveData(list);
        }

        private async Task<List<ApplicationInfo>> ReadData()
        {
            var file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var text = await FileIO.ReadTextAsync(file);
            var list = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ApplicationInfo>>(text);
            if (list == null)
            {
                list = new List<ApplicationInfo>();
            }
            return list;
        }

        private async Task SaveData(List<ApplicationInfo> list)
        {
            var file = await storageFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
            var text = Newtonsoft.Json.JsonConvert.SerializeObject(list);
            await FileIO.WriteTextAsync(file, text);
        }
    }
}
