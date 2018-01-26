using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace smallone
{
    public class DataCenter : DoozyUI.Singleton<DataCenter>
    {
        public Dictionary<string, Item> dictItem = new Dictionary<string, Item>();
        public Dictionary<string, TaskData> dictTask = new Dictionary<string, TaskData>();
        public Dictionary<string, BuildingData> dictBuilding = new Dictionary<string, BuildingData>();

        void Awake()
        {
            LoadItem("Data/Items");
            LoadTask("Data/Tasks");
        }

        void LoadItem(string path)
        {
            List<Item> items = SerializationManager.LoadFromCSV<Item>(path);
            items.ForEach(x =>
            {
                dictItem.Add(x.ID, x);
            });
        }


        void LoadTask(string path)
        {
            List<TaskData> tasks = SerializationManager.LoadFromCSV<TaskData>(path);
            tasks.ForEach(x =>
            {
                dictTask.Add(x.ID, x);
            });
        }
    }
}
