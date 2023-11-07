using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_FengYinZhiTaShow : MonoBehaviour {

    public ObscuredString nowShangHaiID;
    public ObscuredString nowShangHaiRewardID;
    public ObscuredString nowShangHaiIDValue;
    public ObscuredFloat fightTimeSum;
    public ObscuredBool FightStatus;
    public ObscuredFloat FightTime;
    public GameObject Obj_Par;
    public GameObject Obj_MapTime;
    public GameObject Obj_UpdateMonsterTime;
    public GameObject Obj_Ceng;

    public ObscuredFloat UpdateTimeSum;
    public ObscuredInt CostZuanShi;

    // Use this for initialization
    void Start()
    {

        //fightTimeSum = 180;         //设置每次战斗时间
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //初始化显示
        initShow();

        string cengStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FengYinCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (cengStr == "" || cengStr == "0" || cengStr == null) {
            cengStr = "1";
        }

        UpdateZuanShi(int.Parse(cengStr));
    }

    // Update is called once per frame
    void Update()
    {
        if (FightStatus)
        {
            UpdateTimeSum = UpdateTimeSum + Time.deltaTime;

            if (UpdateTimeSum >= 0.1f)
            {
                UpdateTimeSum = 0;
                UpdateShow();
            }

            FightTime = FightTime + Time.deltaTime;
            if (FightTime >= fightTimeSum)
            {
                //战斗结束
                Obj_Par.GetComponent<Main_100007>().EndFuBen();
            }
        }
    }

    void UpdateShow()
    {
        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;
        ObscuredLong fubenValue = gameVar.FuBen_ShangHaiValue_Rose + gameVar.FuBen_ShangHaiValue_Pet;
        Obj_MapTime.GetComponent<Text>().text = "剩余战斗时间:" + (int)(fightTimeSum - FightTime) + "秒";
    }

    //初始化显示
    void initShow()
    {

        Game_PositionVar gameVar = Game_PublicClassVar.Get_game_PositionVar;
        Obj_MapTime.GetComponent<Text>().text = "战斗准备开始";

    }

    //重置
    public void Btn_ChongZhi()
    {

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_4");
        //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, Enter_DaMiJing, null);
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否继续开始下面的挑战？\n 本次挑战需要消耗封印之塔的凭证或花费"+ CostZuanShi +"钻石直接开启!", ChongZhi, ChongZhi_ZuanShi, "封印之塔", "凭证挑战", "钻石挑战",null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }

    //重置
    private void ChongZhi() {

        int bagNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum("10000060");
        if (bagNum<=0)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要凭证数量不足！请先确定背包内是否已有:封印之塔的凭证");
            return;
        }

        if (Obj_Par.GetComponent<Main_100007>() != null)
        {
            bool ifInit = Obj_Par.GetComponent<Main_100007>().InitFuBen();
            if (ifInit) {
                Game_PublicClassVar.Get_function_Rose.CostBagItem("10000060", 1);
            }
        }

        initShow();

    }


    //重置
    private void ChongZhi_ZuanShi()
    {

        if (CostZuanShi <= 0) {
            return;
        }

        if (Game_PublicClassVar.Get_function_Rose.GetRoseRMB() < CostZuanShi)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("需要钻石数量不足！");
            return;
        }
        else {
            Game_PublicClassVar.Get_function_Rose.CostRMB(CostZuanShi);
        }


        if (Obj_Par.GetComponent<Main_100007>() != null)
        {
            Obj_Par.GetComponent<Main_100007>().ZuanShiCreate = true;
            Obj_Par.GetComponent<Main_100007>().InitFuBen();
        }

        initShow();

    }

    //重置
    public void Btn_End()
    {

        Obj_Par.GetComponent<Main_100007>().EndFuBen();
        FightStatus = false;

    }


    //更新钻石
    public void UpdateZuanShi(int nowCengNum) {

        if (nowCengNum >= 1) {
            CostZuanShi = 520;
        }

        if (nowCengNum >= 10)
        {
            CostZuanShi = 520;
        }

        if (nowCengNum >= 20)
        {
            CostZuanShi = 520;
        }

        if (nowCengNum >= 30)
        {
            CostZuanShi = 600;
        }

        if (nowCengNum >= 40)
        {
            CostZuanShi = 680;
        }

        if (nowCengNum >= 50)
        {
            CostZuanShi = 760;
        }

        if (nowCengNum >= 60)
        {
            CostZuanShi = 840;
        }

        if (nowCengNum >= 70)
        {
            CostZuanShi = 920;
        }

        if (nowCengNum >= 80)
        {
            CostZuanShi = 1000;
        }

        if (nowCengNum >= 90)
        {
            CostZuanShi = 1080;
        }
    }
}
