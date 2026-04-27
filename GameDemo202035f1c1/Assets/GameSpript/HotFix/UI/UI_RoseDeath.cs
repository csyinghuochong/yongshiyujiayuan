using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseDeath : MonoBehaviour {

    public GameObject Obj_RoseDeathSet;
    public GameObject Obj_FuHuoEffect;
    public GameObject Obj_FuHuoBiNum;

	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_RoseDeathSet);

        int fuhuoBiNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000024");
        Obj_FuHuoBiNum.GetComponent<Text>().text = fuhuoBiNum.ToString() + "个";

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    //点击死亡按钮
    public void Btn_ClickDeath() {

        //Debug.Log("我点击了死亡按钮");
        Destroy(this.gameObject);
        //设置角色血量
        int rose_Hp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().rose_LastHp = rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = rose_Hp;
		Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", rose_Hp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
		//设置角色状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatusOnce = false;

        /*
        string nowScenceID = Application.loadedLevelName;
        
        string SceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HomeTransferID", "ID", nowScenceID, "Scene_Template");
        string ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
        //ScenceID = "EnterGame";
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();
        //Obj_UIEnterPveSet
        */

        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ReturnBuilding.GetComponent<UI_StartGame>().Btn_RetuenBuilding();

    }

    public void Btn_FuHuo()
    {

        /*
        //钻石复活
        int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (roseRmb >= 180) {
            Game_PublicClassVar.Get_function_Rose.CostRMB(180);
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("钻石不足,无法复活");
            return;
        }
        */


        //复活币复活
        int fuhuoBiNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000024");
        if (fuhuoBiNum >= 1)
        {
            Game_PublicClassVar.Get_function_Rose.CostBagItem("10000024",1);
        }
        else
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_295");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        //播放复活特效
        GameObject effect = (GameObject)Instantiate(Obj_FuHuoEffect);
        effect.SetActive(false);
        //effect.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform;
        effect.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Low.transform.position;
        effect.SetActive(true);


        Destroy(this.gameObject);

        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatusOnce = false;

        //设置角色血量
        int rose_Hp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().rose_LastHp = rose_Hp;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_HpNow = rose_Hp;

        //设置角色状态
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus = "1";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().roseAnimator.Play("Idle");

        //设置角色当前血量
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", rose_Hp.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //写入成就(重置天赋次数)
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("110", "0", "1");

        //给自己添加无敌buff
        Game_PublicClassVar.Get_function_Skill.SkillBuff("92000001", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);
        Game_PublicClassVar.Get_function_Skill.SkillBuff("92000002", Game_PublicClassVar.Get_game_PositionVar.Obj_Rose);
    }

}
