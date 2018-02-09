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
        public Dictionary<string, MonsterData> dictMonster;

        void Awake()
        {
            dictItem = SerializationManager.LoadDictFromCSV<Item>("Id", "Data/Items");
            dictBuilding = SerializationManager.LoadDictFromCSV<BuildingData>("Id", "Data/Buildings");
            dictBuildingTask = SerializationManager.LoadDictFromCSV<BuildingTask>("Id", "Data/BuildingTasks");
            dictNPCData = SerializationManager.LoadDictFromCSV<NPCData>("Id", "Data/NPCs");
            dictNPCFavor = SerializationManager.LoadDictFromCSV<NPCFavor>("Id", "Data/NPCFavor");
            dictNPCTask = SerializationManager.LoadDictFromCSV<NPCTask>("Id", "Data/NPCTask");
            dictPaperShop = SerializationManager.LoadDictFromCSV<PaperShop>("Id", "Data/PaperShop");
            dictPlyerLvlData = SerializationManager.LoadDictFromCSV<PlayerLvlData>("PlayerLv", "Data/PlayerLvl");
            dictLanguages = SerializationManager.LoadDictFromCSV<Languages>("Id", "Data/Language");
            dictMonster = SerializationManager.LoadDictFromCSV<MonsterData>("Id", "Data/Monster");
            
        }
    }
}
