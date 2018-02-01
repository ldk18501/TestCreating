using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace smallone
{
    public class DataCenter : DoozyUI.Singleton<DataCenter>
    {
        public Dictionary<string, Item> dictItem;
        public Dictionary<string, BuildingData> dictBuilding;
        public Dictionary<string, BuildingTask> dictBuildingTask;
        public Dictionary<string, NPCData> dictNPCData;
        public Dictionary<string, NPCFavor> dictNPCFavor;
        public Dictionary<string, NPCTask> dictNPCTask;
        public Dictionary<string, PaperShop> dictPaperShop;
        public Dictionary<string, PlayerLvlData> dictPlyerLvlData;
        public Dictionary<string, Languages> dictLanguages;

        void Awake()
        {
            dictItem = SerializationManager.LoadDictFromCSV<Item>("ID", "Data/Items");
            dictBuilding = SerializationManager.LoadDictFromCSV<BuildingData>("ID", "Data/Buildings");
            dictBuildingTask = SerializationManager.LoadDictFromCSV<BuildingTask>("ID", "Data/BuildingTasks");
            dictNPCData = SerializationManager.LoadDictFromCSV<NPCData>("ID", "Data/NPCs");
            dictNPCFavor = SerializationManager.LoadDictFromCSV<NPCFavor>("ID", "Data/NPCFavor");
            dictNPCTask = SerializationManager.LoadDictFromCSV<NPCTask>("ID", "Data/NPCTask");
            dictPaperShop = SerializationManager.LoadDictFromCSV<PaperShop>("ID", "Data/PaperShop");
            dictPlyerLvlData = SerializationManager.LoadDictFromCSV<PlayerLvlData>("ID", "Data/PlayerLvl");
            dictLanguages = SerializationManager.LoadDictFromCSV<Languages>("ID", "Data/Language");


        }
    }
}
