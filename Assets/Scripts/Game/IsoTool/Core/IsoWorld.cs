using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ISO
{
    [ExecuteInEditMode]
    public class IsoWorld : MonoBehaviour
    {
        public Transform root;
        [Header("Params")]
        public int cellSize = 30;
        [Range(0.5f, 0.9f)]
        public float w_h_rate = 0.75f;//0.5f

        protected List<IsoScene> m_scenes = new List<IsoScene>();

        protected virtual void Start()
        {
            if (!root) root = transform;
            InitScene();
        }

        public virtual void InitScene()
        {
            m_scenes.Clear();
            for (int i = 0; i < root.childCount; ++i)
            {
                IsoScene scene = root.GetChild(i).GetComponent<IsoScene>();
                if (scene)
                {
                    m_scenes.Add(scene);
                }
            }
        }

        protected virtual void Update()
        {
#if UNITY_EDITOR
            IsoUtil.W_H_RATE = w_h_rate;
#endif

            foreach (IsoScene scene in m_scenes)
            {
                scene.UpdateFrame();
            }
        }

        public void PanTo(Vector2 v)
        {
            Debug.Log(v);
            Vector3 vv = root.localPosition;
            vv.x = v.x;
            vv.y = v.y;
            root.localPosition = vv;
        }

        public Vector2 LocalPosToGridPos(float px, float py, float offsetX = 0, float offsetY = 0)
        {
            float xx = (px - transform.localPosition.x) - root.localPosition.x - offsetX;
            float yy = (py - transform.localPosition.y) - root.localPosition.y - offsetY;
            return IsoUtil.LocalPosToIsoGrid(cellSize, xx, yy);
        }

        public void Clear()
        {
            foreach (IsoScene scene in m_scenes)
            {
                scene.Clear();
            }
        }

        public void Destroy()
        {
            foreach (IsoScene scene in m_scenes)
            {
                scene.Destroy();
            }
            m_scenes = null;
        }
    }

}