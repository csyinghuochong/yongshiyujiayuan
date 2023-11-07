using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZuoQiShowModel : MonoBehaviour {

    public string ZuoQiStatus;
    public GameObject Obj_RoseZuoQiPosition;
    public Animator ZuoQiAnimator;                       //角色当前动画状态机
    public string ZuoQiType;                             //坐骑类型,1表示玩家自己的坐骑, 2表示其他玩家的坐骑
    public GameObject Obj_Player;                        //玩家的实例
    private GameObject obj_ZuoQiTuoWei;                  //当前坐骑拖尾特效
    private string zuoQiPlayStatus;
	// Use this for initialization
	void Start () {

        ZuoQiStatus = "1";
        ZuoQiAnimator = this.gameObject.GetComponent<Animator>();

        if (ZuoQiType=="1")
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowPosition = Obj_RoseZuoQiPosition;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowModel = this.gameObject;
        }

        if (ZuoQiType == "2")
        {
            Obj_Player.GetComponent<Player_Status>().Obj_ZuoQiShowPosition = Obj_RoseZuoQiPosition;
            Obj_Player.GetComponent<Player_Status>().Obj_ZuoQiShowModel = this.gameObject;
        }

        //播放特效(第一次进入会在屏幕中间出现 因为角色出生点在屏幕中间)
        //PlayStartEffect();

        //播放拖尾特效
        if (obj_ZuoQiTuoWei == null) {

            string nowZuoQiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            string effName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TuoWeiEffectID", "ID", nowZuoQiShowID, "ZuoQiShow_Template");
            if (effName != "" && effName != "0" && effName != null) {
                obj_ZuoQiTuoWei = (GameObject)MonoBehaviour.Instantiate((GameObject)Resources.Load("Effect/Other/ZuoQiRun/" + effName, typeof(GameObject)));        //实例化特效
                obj_ZuoQiTuoWei.transform.SetParent(this.transform);
                obj_ZuoQiTuoWei.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform.position;
                obj_ZuoQiTuoWei.transform.localScale = new Vector3(1, 1, 1);
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //播放坐骑动画
    public void ZuoQiPlay() {

        ZuoQiAnimator.SetBool("Idle", false);
        ZuoQiAnimator.SetBool("Run", false);
        //Debug.Log("ZuoQiPlayZuoQiPlayZuoQiPlayZuoQiPlay  ZuoQiStatus = " + ZuoQiStatus);
        switch (ZuoQiStatus) {
            case "1":
                //ZuoQiAnimator.Play("Idle");
                ZuoQiAnimator.SetBool("Idle", true);

                if (obj_ZuoQiTuoWei != null&& zuoQiPlayStatus!="1")
                {
                    //obj_ZuoQiTuoWei.GetComponent<ParticleSystem>().Pause(false);
                    obj_ZuoQiTuoWei.GetComponent<ParticleSystem>().Stop();
                    zuoQiPlayStatus = "1";
                }

                break;

            case "2":
                //ZuoQiAnimator.Play("Run");
                ZuoQiAnimator.SetBool("Run", true);

                if (obj_ZuoQiTuoWei != null && zuoQiPlayStatus!="2")
                {
                    obj_ZuoQiTuoWei.GetComponent<ParticleSystem>().Pause(true);
                    obj_ZuoQiTuoWei.GetComponent<ParticleSystem>().Play();
                    zuoQiPlayStatus = "2";
                }

                break;
        }

    }

    public void PlayStartEffect() {

        //播放特效
        //实例化一个特效（上马特效）
        GameObject zhaoHuanEffect = (GameObject)MonoBehaviour.Instantiate((GameObject)Resources.Load("Effect/Skill/Rose_ZuoQiEffect", typeof(GameObject)));        //实例化特效
        zhaoHuanEffect.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform.position;
        zhaoHuanEffect.transform.localScale = new Vector3(1, 1, 1);

    }
}
