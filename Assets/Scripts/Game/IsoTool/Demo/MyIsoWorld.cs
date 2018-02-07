using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using ISO;
using Lean.Touch;


// 前言：切忌使用Input.mouse相关方法来处理点击逻辑，应该全部替换成LeanTouch
namespace smallone
{
    public class MyIsoWorld : IsoWorld
    {
        public MapLayer mapLayer;

        [Header("Scenes")]
        public IsoScene buildingScene;
        public IsoScene groundScene;

        [Header("Prefabs")]
        public IsoObject[] isoObjectPrefabs;

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

                //! 获得当前点击建筑信息
				GameData.strCurConstructionId = component.GetComponent<EntityBuilding>().dataBuilding.ID;
                
                switch (component.transform.name)
                {
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
            //else
            //{
            //    Click(finger.ScreenPosition);
            //}
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
        }

        public void InitNpc(List<NPCData> npcdata) {
            UpdateScneneFrame();
            npcdata.ForEach(p =>
            {
                GameObject npcObj = GameObject.Instantiate(p.Prefab) as GameObject;
                EntityRole npcRole = npcObj.GetComponent<EntityRole>();
                npcRole.world = this;
                buildingScene.AddIsoObject(npcRole);
                npcRole.transform.localScale = Vector3.one;
                npcRole.SetNodePosition(76, 98);

            });
        }

        public void InitBuildings(Dictionary<string, BuildingData> buildingdata)
        {
            UpdateScneneFrame();
            //! 配置表添加建筑
            foreach (string id in buildingdata.Keys)
            {
                var obj = GameObject.Instantiate(buildingdata[id].Prefab) as GameObject;
                IsoObject isoobj = obj.GetComponent<MyIsoObject>();
                isoobj.world = this;
                
                buildingScene.AddIsoObject(isoobj);
                isoobj.SetNodePosition(buildingdata[id].NodeX, buildingdata[id].NodeZ);
                obj.transform.localScale = Vector3.one;
                isoobj.SetWalkable(false,gridData);

                isoobj.spanX = buildingdata[id].SpanX;
                isoobj.spanZ = buildingdata[id].SpanZ;

                //! 肖：用来记录建筑ID，为了点击建筑可以知道点了啥。。
				GameData.lstConstructionObj.Add(obj);

                EntityBuilding eb = obj.AddMissingComponent<EntityBuilding>();
                eb.dataBuilding = buildingdata[id];
                eb.timer = obj.AddMissingComponent<UITimerCtrl>();
                //计时器
            }

            //! 静态存在的建筑
            foreach (IsoObject obj in buildingScene.GetComponentsInChildren<IsoObject>(true))
            {
                if (obj != buildingScene)
                {
                    obj.world = this;
                    buildingScene.AddIsoObject(obj);
                    obj.SetWalkable(false, gridData);
                }
            }

            //! 地表上摆放的可走上去的物件
            foreach (IsoObject obj in groundScene.GetComponentsInChildren<IsoObject>(true))
            {
                if (obj != groundScene)
                {
                    obj.world = this;
                    groundScene.AddIsoObject(obj);
                    obj.SetWalkable(true, gridData);
                }
            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }

        //! 保留参考
        //void Click(Vector2 fingerPos)
        //{
        //    Vector3 localPos = buildingScene.transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(fingerPos));
        //    Vector2 nodePos = IsoUtil.LocalPosToIsoGrid(cellSize, localPos.x, localPos.y);

        //    int nodeX = (int)nodePos.x;
        //    int nodeZ = (int)nodePos.y;
        //    if (gridData.CheckInGrid(nodeX, nodeZ) && gridData.GetNode(nodeX, nodeZ).walkable)
        //    {

        //        if (toggle.isOn)
        //        {
        //            // Add Build
        //            IsoObject obj = (IsoObject)Instantiate(isoObjectPrefabs[Random.Range(0, isoObjectPrefabs.Length)]);
        //            obj.world = this;
        //            obj.SetNodePosition(nodeX, nodeZ);
        //            if (obj.GetWalkable(gridData))
        //            {
        //                buildingScene.AddIsoObject(obj);
        //                obj.SetNodePosition(nodeX, nodeZ);
        //                obj.transform.localScale = Vector3.one;
        //                obj.SetWalkable(false, gridData);
        //            }
        //            else
        //            {
        //                DestroyImmediate(obj.gameObject);
        //            }
        //        }
        //        else
        //        {
        //            // Search Path and move
        //            gridData.CalculateLinks();//when map is changed
        //                                      //Debug.Log(nodeX + "," + nodeZ);
        //            if (astar.FindPath(ball.nodeX, ball.nodeZ, nodeX, nodeZ))
        //            {
        //                ball.MoveByRoads(astar.path);
        //            }
        //        }
        //    }
        //}

        //public void OnToggleChange()
        //{
        //    if (!toggle.isOn)
        //    {
        //        ball.StopMove();
        //        if (!ball.GetWalkable(gridData))
        //        {
        //            System.Collections.Generic.List<PathNode> nodes = gridData.GetNodesByWalkable(true);
        //            if (nodes != null)
        //            {
        //                PathNode node = nodes[Random.Range(0, nodes.Count)];
        //                ball.SetNodePosition(node.x, node.z);
        //            }
        //        }

        //    }
        //}

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
}