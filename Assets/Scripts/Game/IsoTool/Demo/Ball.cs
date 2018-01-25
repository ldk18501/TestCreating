using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ISO;

public class Ball : MyIsoObject {

	public float speed = 6f;

	private List<PathNode> m_path;
	private int roadIndex;
	private PathNode nextPoint;

    public System.Action cbArrived;

    protected override void Start()
    {
        base.Start();
        gameObject.AddMissingComponent<smallone.AINpc>().RegisterMaster(this);
    }

    public void SearchRoadAndMove(){

	}

	public override void UpdateFrame ()
	{
		base.UpdateFrame ();
		if( m_path!=null && nextPoint!=null ){
			Move();
		}
	}

	protected bool MoveToPoint( PathNode point) 
	{
        // Debug.Log(point);
		float distance = Vector2.Distance(new Vector2(x,z),new Vector2(point.x,point.z) );
		if(distance < speed){
			m_nodeX = Mathf.FloorToInt(point.x/world.cellSize);
			m_nodeZ = Mathf.FloorToInt(point.z/world.cellSize);
			return true;
		}

		float moveNum = distance/this.speed ;

		m_pos3D.x += Mathf.FloorToInt((point.x - x)/moveNum);
		m_pos3D.z += Mathf.FloorToInt((point.z - z)/moveNum);
		UpdateScreenPos();
		return false;	
	}


	public void MoveByRoads(List<PathNode> roads){
		roadIndex = 0 ;
		this.m_path = roads ;
		this.nextPoint = GetNextPoint() ; 
	}

	protected PathNode GetNextPoint()
	{
		if(m_path==null) {
			return null ;
		}
		++roadIndex;
		if(roadIndex<m_path.Count){
			PathNode p = (m_path[roadIndex] as PathNode).Clone() ;
			p.x*= world.cellSize;
			p.z*= world.cellSize;
			return p ;
		}
		else
		{
			if(nextPoint!=null){
				x= nextPoint.x ;
				z= nextPoint.z ;
			}
			m_path = null ;
			nextPoint = null ;
			return null ;
		}
	}

	public void StopMove()
	{
		m_path = null ;
	}

	void Move()
	{
		if(MoveToPoint(nextPoint) ){
			nextPoint = GetNextPoint() ; //取下一个点
			if(nextPoint==null){
				Arrived();
			}
		}
	}

	protected void Arrived(){
        StopMove();
        if (cbArrived != null)
        {
            cbArrived.Invoke();
            cbArrived = null;
        }
	}


	#if UNITY_EDITOR
	protected override void OnDrawGizmos(){
		base.OnDrawGizmos();
		if(world!=null && m_path!=null&&m_path.Count>1){
			Gizmos.color = Color.red;

			PathNode startNode = m_path[0];
			Vector3 startPos = (Vector3)IsoUtil.IsoPosToLocalPos(startNode.x,0f,startNode.z)*world.cellSize-transform.localPosition;
			Vector3 startWorld  = transform.TransformPoint(startPos);
			startWorld.z = -1f;

			for(int i=1;i<m_path.Count;++i)
			{
				PathNode endNode = m_path[i];
				Vector3 nextPos = (Vector3)IsoUtil.IsoPosToLocalPos(endNode.x,0f,endNode.z)*world.cellSize-transform.localPosition;
				Vector3 worldNext  = transform.TransformPoint(nextPos);
				worldNext.z = -1f;

				Gizmos.DrawLine(startWorld,worldNext);
				startWorld = worldNext;
			}
		}
	}
	#endif
}
