using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace smallone
{
    public class EntityBuilding : Entity
    {
        public class ProductItem
        {
            public Item item;
            public int nCount;
            public int nRemainTime;
            public ProductItem(Item it, int count, int remaintime = 0)
            {
                item = it;
                nCount = count;
                nRemainTime = remaintime;
            }
        }

        //! 肖：想要用来记录建筑的id，为了点击该建筑，知道id
        public BuildingData dataBuilding;

		public List<ProductItem> lstProductItem;

        public int nCurProductList;
        public int nMaxProductList;

        public GameObject objTimer;
        public Text txtTime;

        private UITimerCtrl _timer;
        

        // Use this for initialization
        void Start()
        {
            nCurProductList = 2;
            nMaxProductList = 4;
            lstProductItem = new List<ProductItem>();
            _timer = this.GetComponent<UITimerCtrl>();
            if (_timer)
            {
                _timer.imgTimer.fillAmount = 0;
            }
            if(objTimer)
            {
                objTimer.SetActive(false);
            }

        }

        // Update is called once per frame
        void Update()
        {
            if(txtTime && _timer)
            {
                if(_timer.nRemain > 0)
                {
                    txtTime.text = _timer.strTimeRemain;
                }
                
            }
        }

        public void StartBuildingTimer(float time)
        {

            if (_timer)
            {

                Debug.Log(" 开始生产ItemId : " + lstProductItem[0].item.ID);

                objTimer.SetActive(true);
                _timer.SetTimer(time, OnTimerStop);
                _timer.StartTimer();
            }
        }

        protected virtual void OnTimerStop()
        {
            Debug.Log("hehe");

			_timer.StopTimer(true);

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
            else
            {
                objTimer.SetActive(false);
            }

            // 刷新GAMEHUD
            UIPanelManager.Instance.GetPanel("UIGameHUD").Repaint();

        }

        // 增加生产队列数量
        public bool AddProductList()
        {
            bool CanAdd = false;

            if (nCurProductList < nMaxProductList)
            {
				nCurProductList++;

                CanAdd = true;
            }

            return CanAdd;
        }

    }
}

