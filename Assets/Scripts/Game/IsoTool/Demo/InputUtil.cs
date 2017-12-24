using UnityEngine.EventSystems;
using UnityEngine;

public class InputUtil {

	public static bool CheckMouseOnUI(){
		if(EventSystem.current){
			if(Input.touchSupported && Input.touchCount>0){
				for(int i=0;i<Input.touchCount;++i){
					if(EventSystem.current.IsPointerOverGameObject (Input.GetTouch(i).fingerId)){
						return true;
					}
				}
				return false;
			}else{
				return EventSystem.current.IsPointerOverGameObject ();
			}
		}else{
			// GameObject.Instantiate(Resources.Load("UI/EventSystem"));
		}
		return false;
	}
}
