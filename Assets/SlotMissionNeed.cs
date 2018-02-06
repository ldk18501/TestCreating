using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class SlotMissionNeed : MonoBehaviour {

    public Image imgIcon;
    public Text txtProgress;
    public bool bMeetConditions = false;
    public Item item;

    public void UpdataItemInfo( Item it , int need )
    {
        imgIcon.sprite = it.IconSprite;


        // 检查数量
        int count = 0;

        for (int i = 0; i < GameData.lstBagItems.Count; i++)
        {
            if(  GameData.lstBagItems[i] == it )
            {
                // todo::背包数量堆叠问题
                count++;
            }
        }

        txtProgress.text = count.ToString() + " / " + need.ToString();
        return;
    }






	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
