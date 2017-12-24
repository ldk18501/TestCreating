using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
	public class IsoScene : IsoObject {

		private int m_SortVersion = 0;//0,1
		
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

		public void AddIsoObject(IsoObject obj,bool isSort = true){
			obj.transform.parent=transform;
			m_sprites.Add(obj);
			if(isSort){
				SortIsoObject(obj);
			}
		}

		public void RemoveIsoObject(IsoObject obj){
			m_sprites.Remove(obj);
			Destroy(obj.gameObject);
		}

		public void SortIsoObject(IsoObject obj){
			if(obj.spanX!=obj.spanZ) {
				m_SortVersion = 1;
			}

			if(m_SortVersion==0)
			{
				Vector3 pos = obj.transform.localPosition;
				pos.z = obj.depth;
				obj.transform.localPosition = pos;
			}
			else
			{
				obj.transform.SetAsFirstSibling();
				for(int i = transform.childCount-1 ; i>0 ; --i)
				{
					IsoObject target= transform.GetChild(i).GetComponent<IsoObject>() ;
					if(target && target!=obj && this.SortCompare(target,obj)<0)
					{
						obj.transform.SetSiblingIndex(i );
						m_OrderInvalid = true;
						break ;
					}
				}
			}
		}

		public void SortAll(){
			m_SortVersion = 0;
			foreach(IsoObject obj in m_sprites){
				SortIsoObject(obj);
				obj.isSort = false ;
			}
		}

		private int SortCompare(IsoObject target,IsoObject item){
			var targetRect = target.boundRect ;
			var itemRect = item.boundRect ;
			if(targetRect.x>itemRect.x+itemRect.width || targetRect.y>itemRect.y+itemRect.height || target.y>item.y){
				return 1;
			}else if(targetRect.x==itemRect.x+itemRect.width &&  targetRect.y==itemRect.y+itemRect.height && target.y==item.y){
				float temp  = target.depth - item.depth;
				if(temp==0) return 0;
				else if(temp<0) return 1;
			}
			return -1;
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
					obj.isSort = false;

					Vector3 v = child.localPosition;
					v.z = -i;
					child.localPosition = v;
				}
			}
			else
			{
				foreach( IsoObject obj in m_sprites ) {
					obj.UpdateFrame();
					if(obj.isSort)	{
						SortIsoObject(obj);
						obj.isSort = false ;
					}
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