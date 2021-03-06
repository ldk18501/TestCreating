﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class UISelectableItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool bSelectable = true;
    public System.Action<bool, GameObject> cbSelect;

    //Do this when the selectable UI object is selected.
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " was selected");
        if (bSelectable && cbSelect != null)
            cbSelect.Invoke(true, gameObject);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log(gameObject.name + " was unselected");
        if (bSelectable && cbSelect != null)
            cbSelect.Invoke(false, gameObject);
    }
}
