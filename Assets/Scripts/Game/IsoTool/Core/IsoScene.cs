using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
	public class IsoScene : IsoObject {
		
		private List<IsoObject> m_sprites = new List<IsoObject>();
		private bool m_OrderInvalid = false;

		private PathGrid m_gridData;
		public PathGrid gridData{
			get {
				return m_gridData;
			}
			set {
				m_gridData=value;
			}
		}

		protected override void Start(){
			base.Start();
		}

		public void AddIsoObject(IsoObject obj){
			obj.transform.parent=transform;
			m_sprites.Add(obj);
		}

		public void RemoveIsoObject(IsoObject obj){
			m_sprites.Remove(obj);
			Destroy(obj.gameObject);
		}

		public override void UpdateFrame()
		{
			if(m_OrderInvalid)
			{
				m_OrderInvalid = false;
				for(int i=0;i<transform.childCount;++i){
					Transform child = transform.GetChild(i);
					IsoObject obj = child.GetComponent<IsoObject>();
					obj.UpdateFrame();

					Vector3 v = child.localPosition;
					v.z = -i;
					child.localPosition = v;
				}
			}
			else
			{
				foreach( IsoObject obj in m_sprites ) {
					obj.UpdateFrame();
				}
			}
		}

		public IsoObject GetIsoObjectByNodePos(int nodeX,int nodeZ){
			foreach(IsoObject obj in m_sprites){
				if(obj.x==nodeX && obj.z==nodeZ){
					return obj ;
				}
			}
			return null;
		}

		public List<IsoObject> GetIsoChildren(){
			return m_sprites;
		}

		public void Clear(){
			foreach(IsoObject obj in m_sprites){
				Destroy(obj.gameObject);
			}
			m_sprites.Clear();
		}

		public override void Destroy(){
			Clear();
			m_sprites = null;
		}

		#if UNITY_EDITOR
		protected virtual void OnDrawGizmos(){
			base.OnDrawGizmos();

			if(showGrid && world){
				var style = new GUIStyle();
				style.fontSize = 30;
				style.normal.textColor = new Color(gizmosColor.r,gizmosColor.g,gizmosColor.b,1f);
				UnityEditor.Handles.Label(transform.position+new Vector3(-0.75f,0.25f,0f), "Z",style);
				UnityEditor.Handles.Label(transform.position+new Vector3(0.5f,0.25f,0f), "X",style);
			}
		}
		#endif
	}

}