using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
	public class IsoObject : MonoBehaviour {

		#if UNITY_EDITOR
		[Header("Gizmos Grid")]
		public bool showGrid = false;
		public Color gizmosColor = Color.yellow;
		#endif


		[HideInInspector]
		[SerializeField]
		protected Vector3 m_pos3D;

		[Header("Node Position")]
		[HideInInspector]
		[SerializeField]
		protected int m_nodeX = 1;

		[HideInInspector]
		[SerializeField]
		protected int m_nodeZ = 1;
		private List<Vector3> m_spanPosArray = new List<Vector3>();

		[HideInInspector]
		[SerializeField]
		protected bool m_isRotated = false;

		[HideInInspector]
		[SerializeField]
		protected float m_centerOffsetY = 0;

		[Header("Size")]
		public int spanX = 1;
		public int spanZ = 1;

		[Header("World")]
		public IsoWorld world;

		public List<Vector3> spanPosArray{
			get { return m_spanPosArray; }
		}
		public float x{
			get { return m_pos3D.x; }
			set {
				m_pos3D.x = value;
				UpdateScreenPos();
			}
		}
		public float y{
			get { return m_pos3D.y; }
			set {
				m_pos3D.y = value;
				UpdateScreenPos();
			}
		}
		public float z{
			get { return m_pos3D.z; }
			set {
				m_pos3D.z = value;
				UpdateScreenPos();
			}
		}
		public Vector3 pos3D{
			get { return m_pos3D; }
			set { m_pos3D=value; }
		}
		public int nodeX{
			get {return m_nodeX; }
		}
		public int nodeZ{
			get {return m_nodeZ; }
		}
		public float centerY{
			get {
				return transform.localPosition.y-m_centerOffsetY;
			}
		}
		public float depth{
			//return (this._pos3D.x + this._pos3D.z) * .866 - this._pos3D.y * .707;
			get { return centerY; }
		}

		protected Rect m_boundRect = new Rect();
		public Rect boundRect
		{
			get{
				m_boundRect.x = x ;
				m_boundRect.y = z ;
				if(this.m_isRotated){
					m_boundRect.width = world.cellSize*(spanZ-1)  ;
					m_boundRect.height = world.cellSize*(spanX-1)  ;
				}else{
					m_boundRect.height = world.cellSize*(spanZ-1)  ;
					m_boundRect.width = world.cellSize*(spanX-1)  ;
				}
				return m_boundRect ;
			}
		}

		protected virtual void Start(){
			if(world)m_centerOffsetY = world.cellSize*spanX*0.005f;
		}

		public virtual void RotateX( bool value){
			m_isRotated = value;
			UpdateSpanPos();
		}

		public virtual void UpdateFrame()
		{
		}

		public virtual void UpdateSpanPos(){
			m_spanPosArray.Clear();
			int t1=0;
			int t2=0;
			if(m_isRotated){
				t1 = spanZ;
				t2 = spanX;
			}else{
				t1 = spanX;
				t2 = spanZ;
			}
			for(int i = 0 ;  i<t1 ; ++i)
			{
				for(int j = 0 ; j<t2 ; ++j)
				{
					Vector3 pos = new Vector3( i*world.cellSize+x, y, j*world.cellSize+z );
					m_spanPosArray.Add( pos );
				}
			}
		}

		public virtual void SetNodePosition(int nodeX,int nodeZ)
		{
			if(world){
				m_nodeX = nodeX;
				m_nodeZ = nodeZ;
				m_pos3D.x = nodeX*world.cellSize;
				m_pos3D.z = nodeZ*world.cellSize;
				UpdateScreenPos();
				UpdateSpanPos();
			}
		}

		public virtual void SetNodePosition(float nodeX,float nodeZ){
			SetNodePosition((int)nodeX,(int)nodeZ);
		}

		public virtual void SetScreenPos(float x,float y){
			this.x = x;
			this.y = y;
			m_pos3D.x = x;
			m_pos3D.y = y;
		}

		public virtual void SetWalkable(bool value,PathGrid grid){
			UpdateSpanPos();
			foreach(Vector3 v in m_spanPosArray){
				grid.SetWalkable( Mathf.FloorToInt(v.x/world.cellSize),Mathf.FloorToInt(v.z/world.cellSize),value);
			}
		}

		public virtual bool GetWalkable( PathGrid grid){
			bool flag = false;
			foreach(Vector3 v in m_spanPosArray){
				int nodeX = Mathf.FloorToInt(v.x/world.cellSize);
				int nodeY = Mathf.FloorToInt(v.z/world.cellSize);
				if(nodeX<0 || nodeX>grid.gridX-1) return false;
				if(nodeY<0 || nodeY>grid.gridZ-1) return false;
				flag = grid.GetNode(nodeX,nodeY).walkable;
				if(!flag) return false;
			}
			return true;
		}

		public virtual bool GetRotatable(PathGrid grid)
		{
			if (spanX==spanZ ) return true;

			SetWalkable(true,grid);
			m_isRotated = ! m_isRotated;
			UpdateSpanPos();
			bool flag = GetWalkable(grid);
			//还原
			m_isRotated = !m_isRotated;
			SetWalkable(false,grid);
			return flag;
		}

		public virtual void UpdateScreenPos(){
			Vector2 ScPos = IsoUtil.IsoPosToLocalPos(m_pos3D.x,m_pos3D.y,m_pos3D.z);
			Vector3 pos = transform.localPosition;
			pos.x = ScPos.x ;
			pos.y = ScPos.y ;
			transform.localPosition = pos;
			UpdateSpanPos();
		}

		public virtual void Destroy(){
			m_spanPosArray.Clear();
			m_spanPosArray = null;
		}


		#if UNITY_EDITOR
		protected virtual void OnDrawGizmos(){
			if(showGrid && world){
				Gizmos.color = gizmosColor;
				Vector3 p = Vector3.zero;

                for (int i = 0; i <= spanX; i++)
                {
                    p.x = i * world.cellSize;
                    p.z = 0;
                    Vector3 startPos = IsoUtil.IsoPosToLocalPos(p.x, p.y, p.z);
                    p.z = spanZ * world.cellSize;
                    Vector3 endPos = IsoUtil.IsoPosToLocalPos(p.x, p.y, p.z);
                    Gizmos.DrawLine(transform.TransformPoint(startPos), transform.TransformPoint(endPos));
                }

                for (int i = 0; i <= spanZ; i++)
                {
                    p.z = i * world.cellSize;
                    p.x = 0;
                    Vector3 startPos = IsoUtil.IsoPosToLocalPos(p.x, p.y, p.z);
                    p.x = spanX * world.cellSize;
                    Vector3 endPos = IsoUtil.IsoPosToLocalPos(p.x, p.y, p.z);
                    Gizmos.DrawLine(transform.TransformPoint(startPos), transform.TransformPoint(endPos));
                }
            }

		}
		#endif
	}
}
