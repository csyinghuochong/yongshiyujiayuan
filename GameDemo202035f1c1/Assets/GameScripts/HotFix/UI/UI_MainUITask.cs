using UnityEngine;
using System.Collections;

public class UI_MainUITask : MonoBehaviour {

	private Game_PositionVar game_PositionVar;
	public GameObject Obj_UI_MainUITaskShow;
	public Transform Tra_TaskShowSet;

	// Use this for initialization
	void Start () {

        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        game_PositionVar.MainUITaskUpdata = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (game_PositionVar.MainUITaskUpdata) {
			game_PositionVar.MainUITaskUpdata = false;

			//循环阐述子控件
			for(int i = 0;i<Tra_TaskShowSet.transform.childCount;i++)
			{
				GameObject go = Tra_TaskShowSet.transform.GetChild(i).gameObject;
				Destroy(go);
			}
			//获取当前跟踪任务
			string[] mainUITaskID = Game_PublicClassVar.Get_function_Task.ReturnMainUITask();
			float uiPosition_y = 557;
			//循环创建追踪任务
			for(int i = 0;i<=mainUITaskID.Length-1;i++){
				if(mainUITaskID[i]!=""){
					GameObject obj = (GameObject)Instantiate(Obj_UI_MainUITaskShow);
					obj.transform.SetParent(Tra_TaskShowSet);
					obj.transform.localScale = new Vector3(1,1,1);
					//获取当前任务有几个目标
					int targetNum = Game_PublicClassVar.Get_function_Task.ReturnTaskTargetNum(mainUITaskID[i]);
					obj.transform.localPosition = new Vector3(10,uiPosition_y,0);
					obj.GetComponent<UI_MainUITaskShow>().TaskID = mainUITaskID[i];
					obj.GetComponent<UI_MainUITaskShow>().UpdataTask = true;
					uiPosition_y = uiPosition_y-(46+targetNum*18);
				}
			}
		}
	}
}
