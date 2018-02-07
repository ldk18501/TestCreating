using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
	public class ProductItem {
		public string nId;
		public int nRemainTime;
		public ProductItem(string id , int remaintime = 0) {
			nId = id;
			nRemainTime = remaintime;
		}
	}

    public class EntityBuilding : Entity
    {
        //! 肖：想要用来记录建筑的id，为了点击该建筑，知道id
        public BuildingData dataBuilding;

		public List<ProductItem> lstProductItem;

        public UITimerCtrl timer;

        // Use this for initialization
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void StartBuildingTimer(float time)
        {
            if (timer)
            {
                timer.SetTimer(time, OnTimerStop);
                timer.StartTimer();
            }

        }


        protected virtual void OnTimerStop()
        {
            Debug.Log("hehe");
        }
        
    }
}

