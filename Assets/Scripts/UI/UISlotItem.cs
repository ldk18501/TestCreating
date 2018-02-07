using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class UISlotItem : MonoBehaviour {

    public Image imgIcon;
    public Text txtScore;
    public Image imgQuality;
	public Item item;
	public Text txtCount;

	public void UpdateShowInfo(Item it)
	{
		item = it;
		imgIcon.sprite = it.IconSprite;
		imgQuality.sprite = null;
		txtScore.text = it.Power.ToString();
		txtCount.text = "x " + GameData.GetItemHave (it).ToString();
        
		ShowIcon = true;

		if (item.Category == ItemType.Equipment) {
			// TODO::品质
			ShowQuality = false;
			ShowScore = true;
			ShowCount = true;
		} else {

			ShowQuality = false;
			ShowScore = false;
			ShowCount = true;
		}
	}

    public bool ShowIcon {
        set {
            imgIcon.gameObject.SetActive(value);
        }
    }


	public bool ShowQuality {
		set {
			imgQuality.gameObject.SetActive(value);
		}
	}


	public bool ShowScore {
		set {
			txtScore.gameObject.SetActive(value);
		}
	}


	public bool ShowCount {
		set {
			txtCount.gameObject.SetActive(value);
		}
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
