using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : DoozyUI.Singleton<BuildingManager>
{
    //private Dictionary<string, Item> _mItemDic = new Dictionary<string, Item>();

	void Awake()
    {
        //List<Item> items = SerializationManager.LoadFromCSV<Item>("Data/Buildings");
        //for (int i = 0; i < items.Count; ++i)
        //{
        //    _mItemDic.Add(items[i].ID, items[i]);
        //}
    }

    public GameObject GetItem(string itemId)
    {
        //Item item;
        //if (_mItemDic.TryGetValue(itemId, out item))
        //    return item;

        return null;
    }
}
