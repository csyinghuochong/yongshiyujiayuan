using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureShowDataListSet : MonoBehaviour {

    public ObscuredString RosePastureID;
    public ObscuredString PastureID;
    public ObscuredInt PastureUpLv;

    public GameObject Obj_Pasture;
    public GameObject Obj_BtnPetData;
    public GameObject Obj_BtnPetSell;

    public GameObject Obj_PastureViewData;
    private GameObject pastureViewDataObj;


    private ObscuredInt nowSellGold;
    private GameObject uiCommonHint;
    // Use this for initialization
    void Start () {



    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Init() {

        PastureUpLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PasturUpLv", "ID", RosePastureID, "RosePasture"));

        switch (PastureUpLv)
        {

            //幼崽期,不显示出售
            case 0:
                Obj_BtnPetSell.SetActive(false);
                break;

            default:
                Obj_BtnPetSell.SetActive(true);
                break;

        }

        //获取出售金币,后面会有单独的计算公式,随着时间越来越值钱
        nowSellGold = Game_PublicClassVar.Get_function_Pasture.PastureGetSellGold(RosePastureID);
    }

	//展示信息
	public void Btn_ViewData(){

        if (pastureViewDataObj != null) {
            Destroy(pastureViewDataObj);
        }

        pastureViewDataObj = (GameObject)Instantiate(Obj_PastureViewData);
        pastureViewDataObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        pastureViewDataObj.transform.localScale = new Vector3(1, 1, 1);
        pastureViewDataObj.transform.localPosition = new Vector3(0, 0, 0);
        pastureViewDataObj.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        pastureViewDataObj.GetComponent<UI_PastureShowDataViewSet>().RosePastureID = RosePastureID;

    }

	//展示宠物
	public void Btn_Sell(){

        if (uiCommonHint != null) {
            Destroy(uiCommonHint);
        }

        //弹出提示
        uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否出售此家园动物?\n你可以获得"+ nowSellGold + "家园资金", PastureSell, null, "系统提示", "确定", "取消", null);
        uiCommonHint.transform.SetParent(GameObject.Find("Canvas").transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

    }

    //牧场的动物出售
    public void PastureSell() {

        if (uiCommonHint != null)
        {
            Destroy(uiCommonHint);
        }

        //发送牧场资金
        Game_PublicClassVar.Get_function_Pasture.DeletePastureAI(RosePastureID);
        Game_PublicClassVar.Get_function_Rose.SendReward("5", nowSellGold.ToString(),"55");
        Btn_Close();

    }


    //抓回牧场
    public void Btn_ZhuaHui() {

        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("哼,想出来溜达都不行,我这就回去!");

        Vector3 moveVec3 = new Vector3(-7.5f, 1.4f, 1.78f);

        //随机一个范围
        moveVec3 = new Vector3(moveVec3.x - 6 + 12.0f * Random.value, moveVec3.y + moveVec3.z - 6 + 12.0f * Random.value);

        Obj_Pasture.GetComponent<PastureAI>().ai_NavMesh.isStopped = false;
        Obj_Pasture.GetComponent<PastureAI>().ai_NavMesh.SetDestination(moveVec3);
        Obj_Pasture.GetComponent<PastureAI>().walkPosition = moveVec3;
        Obj_Pasture.GetComponent<PastureAI>().ai_StarPosition = moveVec3;
        Obj_Pasture.GetComponent<PastureAI>().ai_FindNextPatrol = false;
        Obj_Pasture.GetComponent<PastureAI>().AI_Animator_Status = 0;
        Obj_Pasture.GetComponent<PastureAI>().ai_PatrolRestTime = 10;
        Obj_Pasture.GetComponent<PastureAI>().ai_NavMesh.isStopped = true;

        Btn_Close();
    }

    //关闭界面
    public void Btn_Close() {
        Destroy(this.gameObject);
    }

}
