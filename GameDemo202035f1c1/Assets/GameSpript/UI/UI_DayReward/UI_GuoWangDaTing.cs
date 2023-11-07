using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GuoWangDaTing : MonoBehaviour {

    public GameObject Obj_GuoWangName;
    public GameObject Obj_GuoWangLv;
    public GameObject Obj_HonorHour;
    public GameObject Obj_CountryExpHour;

    public GameObject Obj_NextLvDes;
    public GameObject Obj_CountryExpValue;
    public GameObject Obj_CountryExpPro;
    public GameObject Obj_CostGoldText;
    public GameObject Obj_FanRongDu;

    private string guoWangLv;
    private string houreExp;
    private string houreHonor;
    private string countyDes;
    private string guoWangExpNow;
    private string guoWangExpMax;
    private string goldUp;
    private string zuanShiUp;
    private string goldGetExp;
    public float updataCountryExpSum;

	// Use this for initialization
	void Start () {
	    
        //获取国家拥有着
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name","ID",Game_PublicClassVar.Get_wwwSet.RoseID,"RoseData");
        Obj_GuoWangName.GetComponent<Text>().text = "国王：" + roseName;
        UpdataGuoWangData();    //更新数据

	}
	
	// Update is called once per frame
	void Update () {
        updataCountryExpSum = updataCountryExpSum + Time.deltaTime;
        //每秒钟更新经验
        if (updataCountryExpSum >= 1) {
            UpdataGuoWangData();
            updataCountryExpSum = 0;
        }
	}


    //更新数据
    void UpdataGuoWangData() {

        //读取数据
        guoWangLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //int guoWangID = 1000 + int.Parse(guoWangLv);
        //guoWangLv = guoWangID.ToString();
        houreExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureExp", "ID", guoWangLv, "Country_Template");
        houreHonor = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HoureHonor", "ID", guoWangLv, "Country_Template");
        countyDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountyDes", "ID", guoWangLv, "Country_Template");
        guoWangExpNow = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        guoWangExpMax = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryUp", "ID", guoWangLv, "Country_Template");
        goldUp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldUp", "ID", guoWangLv, "Country_Template");
        zuanShiUp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuanShiUp", "ID", guoWangLv, "Country_Template");
        goldGetExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldGetExp", "ID", guoWangLv, "Country_Template");

        //设置数据显示
        if (int.Parse(guoWangLv) >= 25)
        {
            Obj_GuoWangLv.GetComponent<Text>().text = "繁荣等级：" + guoWangLv+"(已满级)";
        }
        else {
            Obj_GuoWangLv.GetComponent<Text>().text = "繁荣等级：" + guoWangLv;
        }
        
        Obj_CountryExpHour.GetComponent<Text>().text = "繁荣度产出：" + houreExp + "/小时";
        Obj_HonorHour.GetComponent<Text>().text = "荣誉产出：" + houreHonor + "/小时";
        
        Obj_CountryExpValue.GetComponent<Text>().text = "繁荣度：" + guoWangExpNow + "/" + guoWangExpMax;
        Obj_CountryExpPro.GetComponent<Image>().fillAmount = float.Parse(guoWangExpNow) / float.Parse(guoWangExpMax);
        Obj_CostGoldText.GetComponent<Text>().text = "消耗：" + goldUp + "金币";
        int countryRoseLvMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryRoseLvMax", "ID", guoWangLv, "Country_Template"));

        //不能升级给予提示
        if (int.Parse(guoWangExpNow) >= int.Parse(guoWangExpMax)) {
            Obj_FanRongDu.SetActive(true);
            Obj_FanRongDu.GetComponent<Text>().text = "提示：请将角色等级提升至" + countryRoseLvMax.ToString() + "级后可继续升级下一级建筑";
        }

        //显示描述
        string[] desList = countyDes.Split(';');
        countyDes = "";
        for (int i = 0; i <= desList.Length-1; i++) {
            countyDes = countyDes + desList[i] + "\n";
        }
        Obj_NextLvDes.GetComponent<Text>().text = countyDes;
    }

    //金币升级
    public void Btn_Gold() {

        if (float.Parse(guoWangExpNow) >= float.Parse(guoWangExpMax)) {
            int countryRoseLvMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryRoseLvMax", "ID", guoWangLv, "Country_Template"));

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_135");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_136");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + countryRoseLvMax.ToString() + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示：请将角色等级提升至" + countryRoseLvMax.ToString() + "级后可继续升级下一级建筑");
            //更新每日任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "2", "1");
            return;
        }

        //判定金币是否足够
        long roseGold = Game_PublicClassVar.Get_function_Rose.GetRoseMoney();
        if (roseGold >= long.Parse(goldUp))
        {
            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("1", goldUp);
            //执行获取经验
            bool ifUpLv = Game_PublicClassVar.Get_function_Country.addCoutryExp(int.Parse(goldGetExp));

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_137");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("获得繁荣度：" + goldGetExp);
            Debug.Log("升级成功");
            UpdataGuoWangData();    //更新数据
            //更新每日任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "2", "1");
            /*
            if (ifUpLv)
            {
                //如果升级则更新数据
                UpdataGuoWangData();
            }
            else {
                UpdataGuoWangData();    //更新数据
            }
             */
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_138");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("金币不足！");
        }
    }

    //钻石升级
    public void Btn_Rmb()
    {
        if (float.Parse(guoWangExpNow) >= float.Parse(guoWangExpMax))
        {
            int countryRoseLvMax = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryRoseLvMax", "ID", guoWangLv, "Country_Template"));
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示：请将角色等级提升至" + countryRoseLvMax.ToString() + "级后可继续升级下一级建筑");
            //更新每日任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "2", "1");
            return;
        }
        //判定金币是否足够
        int roseRmb = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        if (roseRmb >= int.Parse(zuanShiUp))
        {
            //扣除金币
            Game_PublicClassVar.Get_function_Rose.CostReward("2", zuanShiUp);
            //执行获取经验
            bool ifUpLv = Game_PublicClassVar.Get_function_Country.addCoutryExp(int.Parse(goldGetExp));
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("获得繁荣度：" + goldGetExp);
            UpdataGuoWangData();    //更新数据
            //更新每日任务
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "2", "1");
            /*
            if (ifUpLv)
            {
                //如果升级则更新数据
                UpdataGuoWangData();
            }
             */
        }
    }

    public void Btn_CloseUI() {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }
}
