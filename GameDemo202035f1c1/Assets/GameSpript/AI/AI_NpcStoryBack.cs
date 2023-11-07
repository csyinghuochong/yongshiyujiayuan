using UnityEngine;
using System.Collections;

public class AI_NpcStoryBack : MonoBehaviour {

    private float disRose;      //距离主角的距离
    public string StoryID;      //故事ID
    public GameObject MoveTypeObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        //时刻判定与主角的距离
        

	
	}


    void OnTriggerStay(Collider collider) {

        //当碰撞的是主角
        if (collider.gameObject.layer == 14)
        {
            //初始化角色自身的故事状态
            string roseStoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            //特殊处理,600表示没有任何任务了
            /*
            if (roseStoryStatus == "600") {
                return;
            }
            */

            string IsStoryBackText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IsStoryBackText", "ID", roseStoryStatus, "GameStory_Template");

            if (IsStoryBackText == "1")
            {
                if (roseStoryStatus == StoryID) {
                    //Debug.Log("我进入了故事模式");
                    Game_PublicClassVar.Get_function_UI.CreateStoryBack();
                    //第一次进入弹出选择操作模式方案
                    /*
                    if (StoryID == "1") {
                        GameObject obj = (GameObject)Instantiate(MoveTypeObj);
                        obj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                        obj.transform.localPosition = new Vector3(0, 0, 0);
                        obj.transform.localScale = new Vector3(1,1,1);
                    }
                    */
                    //删除自己
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
