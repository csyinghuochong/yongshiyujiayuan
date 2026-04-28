using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EmenyActSet : MonoBehaviour {

    public int EmenyActMax;             //怪物进攻数量上限
    public int FortressMax;             //要塞士兵数量上限
    private int EmenyNum_Now;           //怪物当前人数
    private int FortressNum_Now;        //要塞士兵当前人数
    private int EmenyNum_Death;         //怪物死亡人数
    private int FortressNum_Death;      //要塞士兵死亡人数
    private int EmenyNum_Wound;         //怪物受伤人数
    private int FortressNum_Wound;      //要塞士兵受伤人数

    public GameObject Obj_EmenyNum_Now;           //怪物当前人数
    public GameObject Obj_FortressNum_Now;        //要塞士兵当前人数
    public GameObject Obj_EmenyNum_Death;         //怪物死亡人数
    public GameObject Obj_FortressNum_Death;      //要塞士兵死亡人数
    public GameObject Obj_EmenyNum_Wound;         //怪物受伤人数
    public GameObject Obj_FortressNum_Wound;      //要塞士兵受伤人数

    public GameObject Obj_ActTextSet;
    public GameObject UI_EmenyActTextSet;
    public GameObject UI_FortressActTextSet;
    public GameObject Obj_Drag;
    public GameObject UI_Close;
    private float dragHight;

    private bool EmenyActStatus;
    private float ActTimeSum;
    private bool EmenyActType;


	// Use this for initialization
	void Start () {
        EmenyActType = true;
        EmenyActStatus = true;
        EmenyNum_Now = EmenyActMax;
        FortressNum_Now = FortressMax;
        dragHight = -120;
        Obj_Drag.GetComponent<RectTransform>().sizeDelta = new Vector2(100, dragHight);
        //Vector2 x = Obj_Drag.GetComponent<RectTransform>().sizeDelta;
        //初始化显示
        Obj_FortressNum_Now.GetComponent<Text>().text = "要塞士兵：" + FortressNum_Now;
        Obj_FortressNum_Death.GetComponent<Text>().text = "死亡人数：0";
        Obj_FortressNum_Wound.GetComponent<Text>().text = "受伤人数：0";
        Obj_EmenyNum_Now.GetComponent<Text>().text = "进攻怪物：" + EmenyNum_Now;
        Obj_EmenyNum_Death.GetComponent<Text>().text = "死亡人数：0";
        Obj_EmenyNum_Wound.GetComponent<Text>().text = "受伤人数：0";
	}
	
	// Update is called once per frame
	void Update () {

        if (EmenyActStatus) {
            ActTimeSum = ActTimeSum + Time.deltaTime;
            if (ActTimeSum >= 2) {
                ActTimeSum = 0;
                //循环执行攻击行为
                if (EmenyActType) {
                    fortressAct(); //执行怪物攻击
                    EmenyActType = false;
                }
                else
                {
                    emenyAct(); //执行怪物攻击
                    EmenyActType = true;
                }
            }
        }
	}


    //怪物攻击
    void emenyAct()
    {
        //怪物攻击,获取本次攻击           取15%-25%值
        int damge = (int)(EmenyActMax * (0.15f + 0.1f * Random.value));
        if(damge>=FortressNum_Now){
            damge = FortressNum_Now;
        }
        //死亡人数   取20%-40%值
        int deathNum = (int)(damge * (0.2f + 0.2f * Random.value));

        //实例化UI
        string str = "敌人向你发起攻击,要塞士兵受伤<color=#ff0000ff>" + damge.ToString() + "</color>人,死亡<color=#ff0000ff>" + deathNum.ToString() + "</color>人";
        GameObject obj = (GameObject)Instantiate(UI_EmenyActTextSet);
        obj.transform.SetParent(Obj_ActTextSet.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.GetComponent<UI_ActTextSet>().TextShow = str;
        dragHight = dragHight + 80.0f;
        Obj_Drag.GetComponent<RectTransform>().sizeDelta = new Vector2(100, dragHight);

        //存入要塞剩余士兵
        FortressNum_Now = FortressNum_Now - damge;
        FortressNum_Death = FortressNum_Death + deathNum;
        FortressNum_Wound = FortressNum_Wound + damge - deathNum;

        //UI更新
        Obj_FortressNum_Now.GetComponent<Text>().text = "要塞士兵：" + FortressNum_Now;
        Obj_FortressNum_Death.GetComponent<Text>().text = "死亡人数：" + FortressNum_Death;
        Obj_FortressNum_Wound.GetComponent<Text>().text = "受伤人数：" + FortressNum_Wound;

        if (FortressNum_Now <= 0) {
            FortressNum_Now = 0;

            fortressReward();

        }

    }


    //要塞士兵攻击
    void fortressAct()
    {

        //怪物攻击,获取本次攻击           取15%-25%值
        int damge = (int)(FortressMax * (0.15f + 0.1f * Random.value));
        if (damge >= EmenyNum_Now)
        {
            damge = EmenyNum_Now;
        }
        //死亡人数   取20%-40%值
        int deathNum = (int)(damge * (0.2f + 0.2f * Random.value));

        //实例化UI
        string str = "你向敌人发起攻击,进攻怪物受伤<color=#ff0000ff>" + damge.ToString() + "</color>人,死亡<color=#ff0000ff>" + deathNum.ToString() + "</color>人";
        GameObject obj = (GameObject)Instantiate(UI_FortressActTextSet);
        obj.transform.SetParent(Obj_ActTextSet.transform);
        obj.transform.localScale = new Vector3(1, 1, 1);
        obj.GetComponent<UI_ActTextSet>().TextShow = str;
        dragHight = dragHight + 80.0f;
        Obj_Drag.GetComponent<RectTransform>().sizeDelta = new Vector2(100, dragHight);

        //存入要塞剩余士兵
        EmenyNum_Now = EmenyNum_Now - damge;
        EmenyNum_Death = EmenyNum_Death + deathNum;
        EmenyNum_Wound = EmenyNum_Wound + damge - deathNum;

        //UI更新
        Obj_EmenyNum_Now.GetComponent<Text>().text = "进攻怪物：" + EmenyNum_Now;
        Obj_EmenyNum_Death.GetComponent<Text>().text = "死亡人数：" + EmenyNum_Death;
        Obj_EmenyNum_Wound.GetComponent<Text>().text = "受伤人数：" + EmenyNum_Wound;

        if (EmenyNum_Now <= 0)
        {
            EmenyNum_Now = 0;

            fortressReward();
        }

    }

    //发送奖励   
    void fortressReward() {

        EmenyActStatus = false;
        UI_Close.SetActive(true);           //显示关闭按钮

        //弹出离线UI
        GameObject fortressBuildReward = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIFortressBuildReward);
        fortressBuildReward.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.transform);
        fortressBuildReward.transform.localPosition = Vector3.zero;
        fortressBuildReward.transform.localScale = new Vector3(1, 1, 1);

        string resultTypeTextStr = "";
        string resultValueTextStr = "";

        //判定输赢
        if (FortressNum_Now <= 0)
        {
            //要塞防守_输,随机扣除一种资源
            int resourceTypeNum = 1;

            for (int i = 0; i <= resourceTypeNum - 1; i++) {
                int resourceValue = (int)(Random.value * 4.99)+1;
                if (resourceValue == 2) {
                    resourceValue = 6;
                }
                Debug.Log("resourceValue = " + resourceValue);
                //获取当前类型建筑的每分钟产出
                string MinuteValue = Game_PublicClassVar.Get_function_Building.retrunBuildingMinute(resourceValue.ToString());
                int sendValue = 0;
                switch(resourceTypeNum){
                    //1种资源 45分钟 - 1.5小时产出
                    case 1:
                        sendValue = (int)(float.Parse(MinuteValue) * (45.0f + Random.value * 45.0f));
                        sendValue = sendValue * -1;
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                    break;
                    /*
                    //2种资源 1 - 1.5小时产出
                    case 2:
                        sendValue = (int)(float.Parse(MinuteValue) * (60.0f + Random.value * 30.0f));
                        sendValue = sendValue * -1;
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                    break;
                    //3种资源 0.67 - 1.33小时产出
                    case 3:
                        sendValue = (int)(float.Parse(MinuteValue) * (40.0f + Random.value * 40.0f));
                        sendValue = sendValue * -1;
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                    break;
                    */
                }

                //设置输赢
                fortressBuildReward.GetComponent<UI_FortressBuildReward>().ActResult = false;

                //存储显示数据
                resultTypeTextStr = resultTypeTextStr + resourceValue + ",";
                resultValueTextStr = resultValueTextStr + sendValue + ",";
            }
        }
        else {
            //要塞防守_赢,随机获得3种资源
            //要塞防守_输,随机扣除一种资源
            int resourceTypeNum = (int)(Random.value * 2.99f);
            resourceTypeNum = resourceTypeNum + 1;

            for (int i = 0; i <= resourceTypeNum - 1; i++)
            {
                int resourceValue = (int)(Random.value * 4.99)+1;
                if (resourceValue == 2)
                {
                    resourceValue = 6;
                }
                Debug.Log("resourceValue = " + resourceValue);
                //获取当前类型建筑的每分钟产出
                string MinuteValue = Game_PublicClassVar.Get_function_Building.retrunBuildingMinute(resourceValue.ToString());
                int sendValue = 0;
                switch (resourceTypeNum)
                {
                    //1种资源 1.5 - 2小时产出
                    case 1:
                        sendValue = (int)(float.Parse(MinuteValue) * (60.0f + Random.value * 60.0f));
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                        break;
                    //2种资源 1 - 1.5小时产出
                    case 2:
                        sendValue = (int)(float.Parse(MinuteValue) * (60.0f + Random.value * 30.0f));
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                        break;
                    //3种资源 0.67 - 1.33小时产出
                    case 3:
                        sendValue = (int)(float.Parse(MinuteValue) * (40.0f + Random.value * 40.0f));
                        Game_PublicClassVar.Get_function_Building.SendResource(resourceValue.ToString(), sendValue);
                        break;
                }

                //存储显示数据
                resultTypeTextStr = resultTypeTextStr + resourceValue + ",";
                resultValueTextStr = resultValueTextStr + sendValue + ",";

            }
            //设置输赢
            fortressBuildReward.GetComponent<UI_FortressBuildReward>().ActResult = true;
        }

        //删除末尾“,”号
        if (resultTypeTextStr != "") {
            if (resultValueTextStr != "") {
                Debug.Log("resultTypeTextStr = " + resultTypeTextStr);
                resultTypeTextStr = resultTypeTextStr.Substring(0, resultTypeTextStr.Length - 1);
                resultValueTextStr = resultValueTextStr.Substring(0, resultValueTextStr.Length - 1);
            }
        }


        //设置奖励结果
        fortressBuildReward.GetComponent<UI_FortressBuildReward>().SelfDeathNum = FortressNum_Death.ToString();
        fortressBuildReward.GetComponent<UI_FortressBuildReward>().EnemyDeathNum = EmenyNum_Death.ToString();
        fortressBuildReward.GetComponent<UI_FortressBuildReward>().ResultTypeTextStr = resultTypeTextStr;
        fortressBuildReward.GetComponent<UI_FortressBuildReward>().ResultValueTextStr = resultValueTextStr;
        fortressBuildReward.GetComponent<UI_FortressBuildReward>().Obj_EmenyActSet = this.gameObject;

        string soldierNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SoldierNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        int outherSoildierNum = int.Parse(soldierNum) - FortressNum_Death;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SoldierNum", outherSoildierNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseBuilding");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseBuilding");
    }


}
