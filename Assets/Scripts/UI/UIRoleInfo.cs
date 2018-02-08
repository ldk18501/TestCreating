using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using smallone;

public class UIRoleInfo : MonoBehaviour {

    public Button btRole;
    public Image imgHeroIcon;
    public Image imgAlert;
    public Button btMission;
	public int nRoleTag;
    public Image imgMissionIcom;

	public UITimerCtrl timer;
	public NPCData dataNpc;
	public NPCFavor dataNpcFavor;
	public NPCTask dataNpcTask;

    // Use this for initialization
    void Start () {
		
		btMission.gameObject.SetActive (false);
		imgMissionIcom.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		if (timer) {
			if (timer.Duration == 0 && dataNpc.CurNpcTask  == null) {
				// 判断任务刷新时间

				float min = float.Parse( dataNpc.ID ) * 5;
				float max = float.Parse( dataNpc.ID ) * 30;

				float rand = Random.Range ( min , max );

				StartMissionTimer (rand);

				Debug.Log ("timer.Duration = " + rand);

			}
		}
	}


	public void SetNpcInfo(string NpcId)
	{
		for (int i = 0 ; i < GameData.lstNpcs.Count ; i++) {
			if( GameData.lstNpcs[i].ID == NpcId )
			{
				dataNpc = GameData.lstNpcs[i];

				break;
			}
		}

	}
        

	public void StartMissionTimer(float time = 1)
	{

		if (timer && timer.Duration == 0)
		{

			btMission.gameObject.SetActive (false);
			imgMissionIcom.gameObject.SetActive (false);

			timer.SetTimer(time, OnTimerStop);
			timer.StartTimer();
		}
	}

	protected virtual void OnTimerStop()
	{
		Debug.Log("hehe");


		// 如果满足条件，刷新任务
		GameData.NewNpcTask(dataNpc);

		btMission.gameObject.SetActive (true);
		imgMissionIcom.gameObject.SetActive (true);


		timer.StopTimer(true);
	}

}
