using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Data.Json;
using Windows.Storage;

namespace ZSCY.Data
{
    public class Morepageclass
    {
        public string UniqueID { get; set; }
        public string Itemname { get; set; }
        public string Itemimgsrc { get; set; }

        public Morepageclass(string name, string src,string id)
        {
            this.Itemimgsrc = src;
            this.Itemname = name;
            this.UniqueID = id;
        }

        public override string ToString()
        {
            return this.Itemname;
        }


    }

    public class Group
    {
        public Group()
        {
            this.items= new ObservableCollection<Morepageclass>();
        }
        public  ObservableCollection<Morepageclass> items { get;private set; }
    }


    public sealed class DataSource
    {
        private static DataSource ds=new DataSource();
        private  ObservableCollection<Group>  group=new ObservableCollection<Group>();

        public ObservableCollection<Group> Group
        {
            get { return group; }
        }
        public static  async Task<IEnumerable<Group>> Get()
        {
            await ds.GetItemsAsync();
            return ds.Group;
        }
        private async Task GetItemsAsync()
        {
            Uri dataUri = new Uri("ms-appx:///Data/moreitems.json");
            try
            {

            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            string jsonText = await FileIO.ReadTextAsync(file);
            JsonObject jsonObject= JsonObject.Parse(jsonText);
            JsonArray jsonArray = jsonObject["Items"].GetArray();
            Group g=new Group();
            foreach (JsonValue item in jsonArray)
            {
                JsonObject i = item.GetObject();
                g.items.Add(new Morepageclass(i["Itemname"].GetString(),i["Itemimgsrc"].GetString(),i["UniqueId"].GetString()));
            }
            this.Group.Add(g);
            }
            catch (Exception exception)
            {
                
                Debug.WriteLine(exception.Message);
            }
        }
    }
}
