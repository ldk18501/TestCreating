using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using ISO;
using Lean.Touch;

// 前言：切忌使用Input.mouse相关方法来处理点击逻辑，应该全部替换成LeanTouch
public class MyIsoWorld : IsoWorld
{
    public MapLayer mapLayer;

    [Header("Scenes")]
    public IsoScene buildingScene;
    public IsoScene groundScene;

    [Header("Prefabs")]
    public IsoObject[] isoObjectPrefabs;
    public Ball ball;

    public PathGrid gridData;
    public Astar astar;

    [Header("UI")]
    public Toggle toggle;

    public LayerMask LayerMask = Physics.DefaultRaycastLayers;

    private void OnEnable()
    {
        LeanTouch.OnFingerTap += OnTestFingerTap;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerTap -= OnTestFingerTap;
    }

    // TODO::临时的，后面拆分掉
    void OnTestFingerTap(LeanFinger finger)
    {
        if (finger.StartedOverGui) return;

        var component = FindComponentUnder(finger);
        // Find the selectable associated with this component
        if (component)
        {
            Debug.Log(component.transform.name);
            switch (component.transform.name) {
                case "MonsterPoint":
                    UIPanelManager.Instance.ShowPanel("UIPanelExpedition");
                    break;
                case "NPC":
                    UIPanelManager.Instance.ShowPanel("UIPanelMission");
                    break;
                case "Store":
                    UIPanelManager.Instance.ShowPanel("UIPanelTrade");
                    break;
                case "SysthesisFurnace":
                    UIPanelManager.Instance.ShowPanel("UIPanelSyntheticFurnace");
                    break;
                default:
                    UIPanelManager.Instance.ShowPanel("UIPanelConstruction");
                    break;
            }
         
        }
        else
        {
            Click(finger.ScreenPosition);
        }
    }


    private Component FindComponentUnder(LeanFinger finger)
    {
        var component = default(Component);
        // Find the position under the current finger
        var point = finger.GetWorldPosition(1.0f);
        // Find the collider at this position
        component = Physics2D.OverlapPoint(point, LayerMask);
        return component;
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        Application.targetFrameRate = 60;

        //init grid
        gridData = new PathGrid(buildingScene.spanX, buildingScene.spanZ);
        //TODO::在这里可以处理哪些点是默认不可走的
        gridData.SetAllWalkable(true);

        //init astar
        astar = new Astar(gridData);

        //init buildings
        if (Application.isPlaying)
        {
            InitBuildings();
        }

        ball.GetComponent<smallone.AINpc>().isAIOff = false;
    }

    void InitBuildings()
    {
        foreach (IsoObject obj in buildingScene.GetComponentsInChildren<IsoObject>(true))
        {
            if (obj != buildingScene && obj != ball) //exclude ball
            {
                obj.world = this;
                buildingScene.AddIsoObject(obj);
                obj.SetWalkable(false, gridData);
            }
        }

        foreach (IsoObject obj in groundScene.GetComponentsInChildren<IsoObject>(true))
        {
            if (obj != groundScene)
            {
                obj.world = this;
                groundScene.AddIsoObject(obj);
                obj.SetWalkable(false, gridData);
            }
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        ball.UpdateFrame();
    }

    void Click(Vector2 fingerPos)
    {
        Vector3 localPos = buildingScene.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(fingerPos));
        Vector2 nodePos = IsoUtil.LocalPosToIsoGrid(cellSize, localPos.x, localPos.y);

        int nodeX = (int)nodePos.x;
        int nodeZ = (int)nodePos.y;
        if (gridData.CheckInGrid(nodeX, nodeZ) && gridData.GetNode(nodeX, nodeZ).walkable)
        {

            if (toggle.isOn)
            {
                // Add Build
                IsoObject obj = (IsoObject)Instantiate(isoObjectPrefabs[Random.Range(0, isoObjectPrefabs.Length)]);
                obj.world = this;
                obj.SetNodePosition(nodeX, nodeZ);
                if (obj.GetWalkable(gridData))
                {
                    buildingScene.AddIsoObject(obj);
                    obj.SetNodePosition(nodeX, nodeZ);
                    obj.transform.localScale = Vector3.one;
                    obj.SetWalkable(false, gridData);
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
                Debug.Log(nodeX + "," + nodeZ);
                if (astar.FindPath(ball.nodeX, ball.nodeZ, nodeX, nodeZ))
                {
                    ball.MoveByRoads(astar.path);
                }
            }
        }


    }


    public void OnRefreshMap()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    public void OnToggleChange()
    {
        if (!toggle.isOn)
        {
            ball.StopMove();
            if (!ball.GetWalkable(gridData))
            {
                System.Collections.Generic.List<PathNode> nodes = gridData.GetNodesByWalkable(true);
                if (nodes != null)
                {
                    PathNode node = nodes[Random.Range(0, nodes.Count)];
                    ball.SetNodePosition(node.x, node.z);
                }
            }

        }
    }

    //! 太暴力了
    public List<MyIsoObject> GetBuildings()
    {
        List<MyIsoObject> buildings = new List<MyIsoObject>();
        for (int i = 0; i < buildingScene.transform.childCount; i++)
        {
            if (buildingScene.transform.GetChild(i).name != "Ball")
                buildings.Add(buildingScene.transform.GetChild(i).GetComponent<MyIsoObject>());
        }
        return buildings;
    }
}
