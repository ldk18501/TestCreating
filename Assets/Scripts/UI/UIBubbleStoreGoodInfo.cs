using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class UIBubbleStoreGoodInfo : MonoBehaviour {

    public Text txtItemName;
    public Text txtItemFrom;
    public Text txtScore;
    public Transform trsNeedRoot;
    public GameObject objNeed;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateBubbleInfo(BuildingTask task)
    {
        
        txtItemName.text = DataCenter.Instance.dictItem[ task.Product.strId ].Name;

        txtItemFrom.text = DataCenter.Instance.dictBuilding[task.TableId ].Name;

        if(DataCenter.Instance.dictItem[task.Product.strId].Category == ItemType.Equipment  )
        {
            txtScore.text = DataCenter.Instance.dictItem[task.Product.strId].Power.ToString();
            txtScore.gameObject.SetActive(true);
        }
        else
        {
            txtScore.gameObject.SetActive(false);
        }



        // 材料
        for (int i = 0; i < trsNeedRoot.childCount; i++)
        {
            GameObject.Destroy( trsNeedRoot.GetChild(i).gameObject  );
        }


        if (task != null)
        {
            // 生成需求道具
            for (int i = 0; i < task.ItemRequire.Count; i++)
            {
                var need = GameObject.Instantiate(objNeed) as GameObject;
                need.name = objNeed.name;
                need.transform.SetParent(trsNeedRoot);
                need.transform.localScale = Vector3.one;

                Item item = DataCenter.Instance.dictItem[task.ItemRequire[i].strId];

                need.GetComponent<SlotMissionNeed>().imgIcon.sprite = item.IconSprite;
                need.GetComponent<SlotMissionNeed>().txtProgress.text = task.ItemRequire[i].nCount.ToString();

            }
        }
    }

}
