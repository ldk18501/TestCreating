using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace smallone
{
    public class DataCenter : DoozyUI.Singleton<DataCenter>
    {
        public Dictionary<string, Item> dictItem;
        public Dictionary<string, TaskData> dictTask;
        public Dictionary<string, BuildingData> dictBuilding;

        void Awake()
        {
            dictItem = SerializationManager.LoadDictFromCSV<Item>("ID", "Data/Items");
            dictTask = SerializationManager.LoadDictFromCSV<TaskData>("ID", "Data/Tasks");
            dictBuilding = new Dictionary<string, BuildingData>();
        }
    }
}
