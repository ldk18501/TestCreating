using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace smallone
{
	public class ProductItem {
		public Item item;
        public int nCount;
        public int nRemainTime;
		public ProductItem(Item it, int count, int remaintime = 0) {
            item = it;
            nCount = count;
			nRemainTime = remaintime;
		}
	}

    public class EntityBuilding : Entity
    {
        //! 肖：想要用来记录建筑的id，为了点击该建筑，知道id
        public BuildingData dataBuilding;

		public List<ProductItem> lstProductItem;

        public int nCurProductList;
        public int nMaxProductList;

        public UITimerCtrl timer;

        // Use this for initialization
        void Start()
        {
            nCurProductList = 2;
            nMaxProductList = 4;
            lstProductItem = new List<ProductItem>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void StartBuildingTimer(float time)
        {

            if (timer)
            {

                Debug.Log(" 开始生产ItemId : " + lstProductItem[0].item.ID);

                timer.SetTimer(time, OnTimerStop);
                timer.StartTimer();
            }
        }

        protected virtual void OnTimerStop()
        {
            Debug.Log("hehe");

            ProductFinish();
        }
        

        public bool AddProduct(BuildingTask task)
        {
            bool CanAdd = true;

            if( lstProductItem.Count >= nCurProductList )
            {
                Debug.Log( " 生产队列满了!! ");
                CanAdd = false;
            }
            else
            {
                ProductItem item = new ProductItem( DataCenter.Instance.dictItem[ task.Product.strId ], task.Product.nCount , task.Time );

                lstProductItem.Add(item);

                Debug.Log(" 生产队列增加ItemId : " + task.Product.strId);

                if(lstProductItem.Count == 1)
                {
                    StartBuildingTimer(task.Time);
                }
                
            }


            return CanAdd;
        }

        // 任务完成
        public void ProductFinish()
        {
            Debug.Log(" 完成生产ItemId : " + lstProductItem[0].item.ID);

            // 放入背包
            GameData.AddItemToBag(lstProductItem[0].item, lstProductItem[0].nCount);

            // 销毁
            lstProductItem.Remove(lstProductItem[0]);

            // 检查是否还能再造
            if(lstProductItem.Count > 0)
            {
                StartBuildingTimer( lstProductItem[0].nRemainTime );
            }
        }

        // 增加生产队列数量
        public bool AddProductList()
        {
            bool CanAdd = true;

            if (nCurProductList < nMaxProductList)
            {
                CanAdd = false;
            }

            return CanAdd;
        }

    }
}

