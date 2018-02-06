using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class UISlotItemInfo : MonoBehaviour {

    public Image imgItemType;
    public Text txtItemName;

    public Image imgScore;
    public Text txtScore;

    public Text txtItemInfo;

    public void UpdateItemInfo(Item item)
    {
        if (item != null)
        {
            imgItemType.sprite = null;
            txtItemName.text = item.Name;

            if (item.Category == ItemType.Equipment)
            {
                imgScore.gameObject.SetActive(true);
                txtScore.gameObject.SetActive(true);
				txtScore.text = item.Power.ToString();

            }
            else
            {
                imgScore.gameObject.SetActive(false);
                txtScore.gameObject.SetActive(false);
            }

            txtItemInfo.text = item.Info;
        }

    }


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
