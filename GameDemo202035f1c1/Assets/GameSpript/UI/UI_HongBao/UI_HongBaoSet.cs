using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HongBaoSet : MonoBehaviour {

    public int HongBaoAllGold;
    public Pro_HongBaoListData proHongBaoListData;
    public int SelfHongBaoValue;
    public GameObject Obj_SelfHongBaoGold;
    public GameObject Obj_LabHongBaoAllGold;
    public GameObject Obj_HongBaoCostTime;
    public GameObject Obj_NameListSet;
    public GameObject Obj_NameList;

    public GameObject Obj_GetHongBaoShowSet;
    public GameObject Obj_HongBaoShowSet;
    public GameObject Obj_LingQuHongBaoShowSet;

    public float CostTime;      //红包领取剩余时间

    private float sendLingQuTime;

    // Use this for initialization
    void Start () {

        Game_PublicClassVar.gameServerObj.Obj_HongBao = this.gameObject;

        Obj_GetHongBaoShowSet.SetActive(true);
        Obj_HongBaoShowSet.SetActive(false);
        Obj_LingQuHongBaoShowSet.SetActive(false);

        //初始化选择
        Init();

    }
	
	// Update is called once per frame
	void Update () {
        sendLingQuTime = sendLingQuTime + Time.deltaTime;

        CostTime = CostTime - Time.deltaTime;

        if (CostTime > 0)
        {
            Obj_HongBaoCostTime.SetActive(true);
            Obj_HongBaoCostTime.GetComponent<Text>().text = (int)(CostTime) + Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("秒后开启");
        }
        else {
            Obj_HongBaoCostTime.SetActive(false);
        }

    }

    private void OnDestroy()
    {
        //清空
        Game_PublicClassVar.gameServerObj.Obj_HongBao = null;
    }

    //红包展示
    public void Init() {
        HongBaoAllGold = Game_PublicClassVar.Get_gameLinkServerObj.HongBaoSumValue;
        CostTime = Game_PublicClassVar.Get_gameLinkServerObj.HongBaoCostTime;
        Obj_LabHongBaoAllGold.GetComponent<Text>().text = HongBaoAllGold.ToString();
    }

    //点击领取
    public void Btn_ClickLingQu()
    {

        //获取等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv < 12)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_216");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_215");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + "12" + langStrHint_2);
            return;
        }

        if (sendLingQuTime >= 1.0f) {
            sendLingQuTime = 0;
            //发送领取协议  10001401
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001401, "");
        }

    }

    //服务器返回显示领取
    public void LingQuReturn()
    {
        Obj_LingQuHongBaoShowSet.SetActive(true);

        //服务器领取返回
        if (SelfHongBaoValue != 0)
        {
            //string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_422");
            Obj_SelfHongBaoGold.GetComponent<Text>().text = SelfHongBaoValue.ToString();

            Debug.Log("恭喜你抢到红包!");
            //发送奖励
            Game_PublicClassVar.function_Rose.SendReward("1", SelfHongBaoValue.ToString(), "53");

        }
        else {
            Debug.Log("很遗憾,你未抢到红包!");
        }

        //领取完毕后隐藏主界面图标
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnHongBao.SetActive(false);
        Obj_GetHongBaoShowSet.SetActive(false);
    }

    //显示领取信息
    public void Btn_ShowPlayerLingQuData()
    {
        //显示领取信息
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001402, "");

    }

    //服务器返回显示领取信息
    public void Return_ShowPlayerLingQuData()
    {
        Obj_HongBaoShowSet.SetActive(true);

        //清理空间显示
        Game_PublicClassVar.function_UI.DestoryTargetObj(Obj_NameListSet);

        //领取信息
        foreach (string playerID in proHongBaoListData.HongBaoData.Keys)
        {
            GameObject obj = (GameObject)Instantiate(Obj_NameList);
            obj.GetComponent<UI_HongBaoNameList>().HongBaoValue = proHongBaoListData.HongBaoData[playerID].ToString();
            obj.transform.SetParent(Obj_NameListSet.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
        }
    }

    //关闭红包界面
    public void Close_HongBaoSet() {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGameHongBao();
    }

    //关闭红包领取界面
    public void Close_HongBaoShowSet() {
        Obj_HongBaoShowSet.SetActive(false);
    }


    //关闭红包领取名单界面
    public void Close_LingQuHongBaoShowSet() {
        Obj_LingQuHongBaoShowSet.SetActive(false);
    }

}
