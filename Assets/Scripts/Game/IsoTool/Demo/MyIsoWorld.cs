using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ISO;

public class MyIsoWorld :IsoWorld {
	[Header("Scenes")]
	public IsoScene buildingScene;
	public IsoScene groundScene;

	[Header("Prefabs")]
	public IsoObject[] isoObjectPrefabs;
	public Ball ball;

	public PathGrid gridData;
	public Astar astar;

	private float m_touchTime = 0;
	private Vector3 m_touchPos ;

	[Header("UI")]
	public Toggle toggle;

	// Use this for initialization
	protected override void Start () {
		base.Start();

		Application.targetFrameRate = 60;

		//init grid
		gridData = new PathGrid(buildingScene.spanX,buildingScene.spanZ);
		gridData.SetAllWalkable(true);

		//init astar
		astar = new Astar(gridData);

		//init buildings
		if(Application.isPlaying){
			InitBuildings();
		}
	}

	void InitBuildings(){
		foreach(IsoObject obj in buildingScene.GetComponentsInChildren<IsoObject>(true)){
			if(obj!=buildingScene && obj!=ball) //exclude ball
			{
				obj.world = this;
				buildingScene.AddIsoObject(obj);
				obj.SetWalkable(false,gridData);
			}
		}

		foreach(IsoObject obj in groundScene.GetComponentsInChildren<IsoObject>(true)){
			if(obj!=groundScene)
			{
				obj.world = this;
				groundScene.AddIsoObject(obj);
				obj.SetWalkable(false,gridData);
			}
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		if(Input.GetMouseButtonDown(0)){
			m_touchTime = Time.realtimeSinceStartup;
			m_touchPos = Input.mousePosition;
		}
		else if(Input.GetMouseButtonUp(0))
		{
			if(Time.realtimeSinceStartup-m_touchTime<0.5f && Vector3.Distance(Input.mousePosition,m_touchPos)<20f){
				Click();
			}
		}
		ball.UpdateFrame();
	}

	void Click(){
		Vector3 localPos = buildingScene.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
		Vector2 nodePos = IsoUtil.LocalPosToIsoGrid(cellSize,localPos.x,localPos.y);

		int nodeX = (int)nodePos.x;
		int nodeZ = (int)nodePos.y;
		if(gridData.CheckInGrid(nodeX,nodeZ)  && gridData.GetNode(nodeX,nodeZ).walkable){

			if(toggle.isOn)
			{
				// Add Build
				IsoObject obj = (IsoObject)Instantiate(isoObjectPrefabs[Random.Range(0,isoObjectPrefabs.Length)]);
				obj.world = this;
				obj.SetNodePosition(nodeX,nodeZ);
				if(obj.GetWalkable(gridData)){
					buildingScene.AddIsoObject(obj);
					obj.SetNodePosition(nodeX,nodeZ);
					obj.transform.localScale = Vector3.one;
					obj.SetWalkable(false,gridData);
				}
				else
				{
					DestroyImmediate(obj.gameObject);
				}
			}
			else
			{
				// Search Path and move
				gridData.CalculateLinks();//when map is changed
				if(astar.FindPath(ball.nodeX,ball.nodeZ,nodeX,nodeZ)){
					ball.MoveByRoads(astar.path);
				}
			}
		}


	}


	public void OnRefreshMap(){
		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
	}

	public void OnToggleChange()
	{
		if(!toggle.isOn)
		{
			ball.StopMove();
			if(!ball.GetWalkable(gridData)){
				System.Collections.Generic.List<PathNode> nodes = gridData.GetNodesByWalkable(true);
				if(nodes!=null){
					PathNode node = nodes[Random.Range(0,nodes.Count)];
					ball.SetNodePosition(node.x,node.z);
				}
			}
			
		}
	}

}
