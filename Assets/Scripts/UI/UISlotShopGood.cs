using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class UISlotShopGood : MonoBehaviour {

    public Image imgIcon;
    public Image imgPrice;
    public Text txtPrice;
    public Button btBuy;
    public Text txtBuy;

    public PaperShop datapaper;

	// Use this for initialization
	void Start () {


        // InitSlotInfo();

    }
	


	// Update is called once per frame
	void Update () {
		
	}

    private void OnEnable()
    {

       // EventCenter.Instance.RegisterGameEvent("Buy", OnBuy);
    }

    private void OnDisable()
    {
       // EventCenter.Instance.UnregisterGameEvent("Buy", OnBuy);
    }


    public void UpdateSlotInfo( PaperShop paper )
    {
        datapaper = paper;

        imgIcon.sprite = paper.IconSprite;

        Item it = DataCenter.Instance.dictItem[datapaper.Price[0].strId];

        imgPrice.sprite = it.IconSprite;
        txtPrice.text = datapaper.Price[0].nCount.ToString();

        bool CanBuy = false;
        // 钱
        if(it.ID == "1")
        {
            if (GameData.Coins >= datapaper.Price[0].nCount)
                CanBuy = true;
        }
        //水晶
        else if(it.ID == "2")
        {
            if (GameData.Gems >= datapaper.Price[0].nCount)
                CanBuy = true;
        }
        

        btBuy.GetComponent<Button>().interactable = CanBuy;

    }




}
