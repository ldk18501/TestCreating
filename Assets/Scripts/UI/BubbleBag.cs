using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class BubbleBag : MonoBehaviour {

    public Transform trsSlotItemRoot;
    public GameObject objSlotItem;
    
    //! 生成内部道具
    public void GenerateItemsInBag(int itemtype)
    {
		ItemsClear ();

        for (int i = 0; i < GameData.lstBagItems.Count; i++)
        {
            if( GameData.lstBagItems[i].Type == itemtype)
            {
                var obj = GameObject.Instantiate(objSlotItem) as GameObject;
				obj.name = GameData.lstBagItems[i].ID;
                obj.transform.SetParent(trsSlotItemRoot);
                obj.transform.localScale = Vector3.one;
                
                UISlotItem item = obj.GetComponent<UISlotItem>();
                //UISelectableItem sItem = item.gameObject.AddMissingComponent<UISelectableItem>();

                item.imgIcon.sprite = GameData.lstBagItems[i].IconSprite;
                item.ShowIcon = true;
                item.ShowQuality = false;
                item.txtScore.text = GameData.lstBagItems[i].Power.ToString();

				if ( GameData.lstBagItems[i].Category== ItemType.Equipment)
                {
                    item.ShowScore = true;
                }
                //sItem.bSelectable = true;
                //sItem.cbSelect = OnSlotSelected;
            }
        }
    }

    public void ItemsClear()
    {
        for (int i = 0; i < trsSlotItemRoot.childCount; i++)
        {
            GameObject.Destroy( trsSlotItemRoot.GetChild(i).gameObject );
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
