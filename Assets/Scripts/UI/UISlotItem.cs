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


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
