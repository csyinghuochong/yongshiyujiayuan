using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_WaBaoPro : MonoBehaviour {

    public ObscuredString UIBagSpaceNum;
    public ObscuredString DropID_WaBao;
    public ObscuredString ItemID;
    public GameObject Obj_WaBaoQiu;
    public GameObject Obj_WaBaoMaxHint;
    public GameObject Obj_WaBaoMinHint;
    public ObscuredInt WaBaoProMaxValue;
    public ObscuredFloat wabaoTimeSum;
    public ObscuredFloat wabaoNowProValue;
    private bool wanmeiWaBaoStatus;

    public ObscuredString wabaoProType;         //1表示增加  2表示减少

    public ObscuredFloat wabaoPro_Random;       //此值上下浮动25判定为完美
    public ObscuredInt wabaoFuDongValue;
    public GameObject WaBaoHintEffect;
    private float effectShowTime;
    public GameObject EffectPar;
    public GameObject WaBaoHintEffect_2;

    public Rose_Status rose_Status;

    // Use this for initialization
    void Start () {
        wabaoProType = "1";
        WaBaoProMaxValue = 400;
        wabaoFuDongValue = 40;
        wabaoPro_Random = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(wabaoFuDongValue, WaBaoProMaxValue);
        Obj_WaBaoMinHint.transform.localPosition = new Vector3(wabaoPro_Random - wabaoFuDongValue, 33, 0);
        Obj_WaBaoMaxHint.transform.localPosition = new Vector3(wabaoPro_Random + wabaoFuDongValue, 33, 0);
        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
    }
	
	// Update is called once per frame
	void Update () {
        //wabaoTimeSum = wabaoTimeSum + Time.deltaTime;
        if (wabaoProType == "1") {
            wabaoNowProValue = wabaoNowProValue + Time.deltaTime * 100;
        }
        if (wabaoProType == "2") {
            wabaoNowProValue = wabaoNowProValue - Time.deltaTime * 100;
        }

        if (wabaoNowProValue > WaBaoProMaxValue) {
            wabaoNowProValue = WaBaoProMaxValue;
            wabaoProType = "2";
        }

        if (wabaoNowProValue < 0) {
            wabaoNowProValue = 0;
            wabaoProType = "1";
        }

        Obj_WaBaoQiu.transform.localPosition = new Vector3(wabaoNowProValue,0,0);

        //判断如果玩家移动,也销毁自身
        if (rose_Status.Move_Target_Status) {
            //销毁自身
            Destroy(this.gameObject);
        }

        effectShowTime = effectShowTime + Time.deltaTime;
        if (effectShowTime >= 0.5f) {
            effectShowTime = 0;
            //攻击反馈
            GameObject actEffect = (GameObject)Instantiate(WaBaoHintEffect);
            actEffect.transform.SetParent(EffectPar.transform);
            actEffect.transform.localPosition = Vector3.zero;
            actEffect.transform.localScale = new Vector3(1, 1, 1);

            //攻击反馈
            GameObject actEffect_2 = (GameObject)Instantiate(WaBaoHintEffect_2);
            actEffect_2.transform.SetParent(Obj_WaBaoMinHint.transform);
            actEffect_2.transform.localPosition = new Vector3(wabaoFuDongValue, -34,0);
            actEffect_2.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    //点击挖宝
    public void Btn_WaBao() {

        //检测背包是否有道具
        int itemNum =  Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(ItemID);
        if (itemNum <= 0) {
            //销毁自身
            Destroy(this.gameObject);
            return;
        }

        //获取道具ID比对
        string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", UIBagSpaceNum, "RoseBag");
        if (nowItemID != ItemID) {
            //销毁自身
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请不要移动背包道具位置");
            Destroy(this.gameObject);
            return;
        }

        //获取背包道具
        string nowItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", UIBagSpaceNum, "RoseBag");
        if (nowItemID == "10000016" || nowItemID == "10000017") {

            if (int.Parse(nowItemNum) >= 1) {

                if (wabaoNowProValue >= wabaoPro_Random - wabaoFuDongValue)
                {
                    if (wabaoNowProValue <= wabaoPro_Random + wabaoFuDongValue)
                    {
                        //挖宝成功
                        Debug.Log("挖宝成功！");
                        //不是完美挖宝有5%概率获得双倍掉落
                        if (Random.value <= 0.1f)
                        {
                            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_8");
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你触发完美挖宝!获得2次奖励机会,每次各自随机!");
                            wanmeiWaBaoStatus = true;

                            //获取玩家名称
                            string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜" + roseName + "使用藏宝图时触发完美挖宝!获得2次奖励机会!");
                            Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                            comStr_4.str_1 = "4";
                            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
                        }
                    }
                }

                //触发掉落
                //销魂自身道具
                if (Game_PublicClassVar.Get_function_Rose.DeleteBagSpaceItem_Num(UIBagSpaceNum, 1))
                {
                    Game_PublicClassVar.Get_function_AI.DropIDToDropItem(DropID_WaBao, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    if (wanmeiWaBaoStatus)
                    {
                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem(DropID_WaBao, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position);
                    }

                    //写入成就
                    if (ItemID == "10000016")
                    {
                        //低级藏宝图
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("103", "0", "1");
                    }
                    if (ItemID == "10000017")
                    {
                        //高级藏宝图
                        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("104", "0", "1");
                    }

                    //写入活跃任务
                    Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "131", "1");
                }

                //销毁自身
                Destroy(this.gameObject);

            }
        }
    }
}
