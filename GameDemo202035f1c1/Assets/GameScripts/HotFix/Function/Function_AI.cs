using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using CodeStage.AntiCheat.ObscuredTypes;

public class Function_AI : MonoBehaviour {

    private int randomSet;  //记录货币数量的随机值

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //掉落函数
    public bool AI_MonsterDrop(string monsterID,Vector3 vec3,float dropProValue=1) {
        
        //根据怪物ID获得掉落ID
        string dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropID", "ID", monsterID, "Monster_Template");
        if (dropID != "0")
        {
            DropIDToDropItem(dropID, vec3, monsterID, dropProValue);
            return true;
        }
        else {
            return false;
        }
    }

	//传入掉落ID，生成掉落数据
    public bool DropIDToDropItem(string dropID, Vector3 vec3, string monsterID="0", float dropProValue = 1)
    {
        //Debug.Log("DropIDToDropItemDropIDToDropItemDropIDToDropItem");
		//是否有子掉落
		bool DropSonStatus = false;
        //int dropLimit = 0;                    //设置掉落最大数量
        int dropLimit = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropLimit", "ID", dropID, "Drop_Template"));
        int dropNumNow = 0;                     //当前掉落道具的数量
        int dropLoopNum = 0;                    //掉落循环次数
        string dropIDInitial = dropID;          //设置初始掉落
		//循环每行掉落数据
		do
		{
			DropSonStatus = true;
            //传入当前掉落数量和最大掉落数量给行掉落数据
            //Debug.Log("dropID = " + dropID);
			//生成每行掉落数据
            dropNumNow = RowDrop(dropID, vec3, monsterID, dropNumNow, dropLimit, dropProValue);
			//获取子掉落
			dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropSonID", "ID", dropID,"Drop_Template");
			//没有子掉落循环取消（因为掉落ID里面可以套掉落ID）
            if (dropID == "0")
            {
                DropSonStatus = false;
                //如果掉落数量没达到指定数量再次随机
                if (dropLimit != 0)
                {
                    //子级为空时,判定当前数量是否已达到掉落上线,不足上线则再次执行一边
                    if (dropNumNow < dropLimit)
                    {
                        dropLoopNum = dropLoopNum + 1;
                        dropID = dropIDInitial;
                        DropSonStatus = true;
                        //Debug.Log("22222222");
                    }
                }
                //循环10次强制结束循环
                if (dropLoopNum > 10)
                {
                    DropSonStatus = false;
                    //return true;
                }
            }
            else {
                //当掉落数量已达上线时,自动取消掉落
                if (dropLimit != 0) {
                    if (dropNumNow >= dropLimit)
                    {
                        DropSonStatus = false;
                    }
                }
            }
		}
		while (DropSonStatus);
		return true;
	}

    //传入ID判定是否是特殊掉落ID
    public string IfTeShuDropItemID(string itemID) {

        //钥匙类 返回1
        if (itemID == "10010034") {
            return "1"; 
        }

        return "";

    }

    //行掉落ID数据
    public int RowDrop(string dropID, Vector3 vec3, string monsterID,int dropNumNow=0,int dropNumMax=0, float dropProValue = 1)
    {
        
        Function_DataSet function_DataSet = Game_PublicClassVar.Get_function_DataSet;

        //清空隐藏属性ID
        Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr = "0";
        //根据掉落ID获取掉落
        int dropNum = 0;
        //int dropNum
        string dropType = function_DataSet.DataSet_ReadData("DropType", "ID", dropID, "Drop_Template");

        for (int i = 1; i <= 10; i++)
        {
            //Debug.Log("Str1111 = " + i);
            //获取掉落道具的ID
            ObscuredString dropItemID = function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID, "Drop_Template");
            
            //string dropItemID = "1";
            //string dropType = "1";
            //Debug.Log("dropItemID = " + dropItemID);
            //如果道具ID不为空则触发掉落概率
            //Debug.Log("Str2222 = " + i);
            if (dropItemID != "0")
            {
                dropNum = dropNum + 1;      //掉落数量累计

                //每个掉落
                //获取每个掉落的概率
                string dropChance = function_DataSet.DataSet_ReadData("DropChance" + i.ToString(), "ID", dropID, "Drop_Template");
                //string dropChance = "100000";
                float randomdrop = Random.Range(0, 1000000);
                float dropChanceData = int.Parse(dropChance);
                dropChanceData = dropChanceData * dropProValue;     //概率附加

                //怪物ID
                if (monsterID != "" && monsterID != "0") {
                    //特殊道具概率降低
                    string teshuType = IfTeShuDropItemID(dropItemID);
                    //钥匙类
                    if (teshuType == "1") {
                        //获取当前等级差
                        int roseLv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                        int aiLv = int.Parse(function_DataSet.DataSet_ReadData("Lv", "ID", monsterID, "Monster_Template"));
                        //最低保障前两章是正常掉落
                        if (aiLv < 40)
                        {
                            int lvCha = roseLv - aiLv;
                            //超过10级不再执行掉落
                            if (lvCha >= 10) {
                                dropChanceData = 0;
                            }
                        }
                    }
                }

                //Debug.Log("Str3333 = " + i);
                //当随机值小于掉落概率值判定为掉落成功
                if (randomdrop <= dropChanceData)
                {
                    //Debug.Log("掉落成功！掉落成功！掉落成功！掉落成功！掉落成功！掉落成功！ dropID = " + dropID);
                    //掉落成功
                    //获取掉落数量

                    string dropMinNum = function_DataSet.DataSet_ReadData("DropItemMinNum" + i.ToString(), "ID", dropID,"Drop_Template");
                    string dropMaxNum = function_DataSet.DataSet_ReadData("DropItemMaxNum" + i.ToString(), "ID", dropID,"Drop_Template");
                    //string dropMinNum = "1";
                    //string dropMaxNum = "2";

                    int dropMinNum_Int = int.Parse(dropMinNum);
                    int dropMaxNum_Int = int.Parse(dropMaxNum);

                    //判定概率是否大于100%,大于100则掉落数量变多（此处触发BUG不能直接成掉落系数,要加算法）
                    if (dropChanceData > 1000000) {
                        //dropMinNum_Int = (int)(dropMinNum_Int * dropProValue);
                        //dropMaxNum_Int = (int)(dropMaxNum_Int * dropProValue);

                        //如果需要此处应该是这样的
                        dropMinNum_Int = (int)(dropMinNum_Int * (float.Parse(dropChance)/1000000 * dropProValue));
                        dropMaxNum_Int = (int)(dropMaxNum_Int * (float.Parse(dropChance)/1000000 * dropProValue));
                    }
                    



                    //随机掉落数量
                    int itemDropNum = Random.Range(dropMinNum_Int, dropMaxNum_Int);
                    randomSet = itemDropNum;

                    //读取掉落道具ID
                    //string itemID = function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID,"Drop_Template");
                    ObscuredString itemID = dropItemID;

                    //Debug.Log("Str4444 = " + i);
                    switch (dropType) {
                        //发送道具到地上实体
                        case "1":

                            //如果是金币掉落显示掉落数量为1
                            if (itemID == "1")
                            {
                                itemDropNum = 1;
                            }

                            //当掉落数量不为0时,循环实例化每个掉落
                            if (itemDropNum != 0)
                            {
                                for (int n = 1; n <= itemDropNum; n++)
                                {
                                    //执行掉落
                                    AI_DropItem(itemID, vec3, monsterID);
                                }
                            }
                        break;
                        //发送道具到背包
                        case "2":
                            //Debug.Log("Str5555 = " + i);
                            //获取道具的极品值
                            float HideDropPro = 0;
                            if (monsterID != "0") {
                                HideDropPro = float.Parse(function_DataSet.DataSet_ReadData("HideDropPro", "ID", monsterID, "Monster_Template"));
                            }
                            //Debug.Log("Str6666 = " + i);
                            //Debug.Log("HideDropPro = " + HideDropPro);
                            //Debug.Log("itemID = " + itemID + "itemDropNum = " + itemDropNum + "HideDropPro = " + HideDropPro);
                            bool ifDrop = Game_PublicClassVar.Get_function_Rose.SendRewardToBag(itemID, itemDropNum, "0", HideDropPro,"0",true,"12");
                            //Debug.Log("Str7777 = " + i);
                            //背包满了的话直接掉落到地上
                            if (!ifDrop)
                            {
                                //背包满了执行掉落地上
                                AI_DropItem(itemID, vec3, monsterID, itemDropNum);
                                //Debug.Log("Str8888 = " + i);
                            }
                            else {
                                //Debug.Log("Str9999 = " + i);
                                //道具进入背包成功,记录抽卡数据
                                if (Game_PublicClassVar.Get_game_PositionVar.ChouKaUIOpenStatus)
                                {
                                    //Debug.Log("Str0000 = " + i);
                                    string Str = Game_PublicClassVar.Get_game_PositionVar.ChouKaStr;
                                    string hindIDStr = Game_PublicClassVar.Get_game_PositionVar.ChouKaHindIdStr;
                                    Str = Str + itemID + "," + itemDropNum + "," + hindIDStr + ";";
                                    Game_PublicClassVar.Get_game_PositionVar.ChouKaStr = Str;
                                    Game_PublicClassVar.Get_game_PositionVar.ChouKaStatus = true;
                                    //Debug.Log("Str = " + Str);
                                }
                            }

                        break;
                    }
                    //累计掉落数量
                    //dropNum = dropNum + 1;
                    if (dropNumMax != 0)
                    {
                        //Debug.Log("掉落1");
                        dropNumNow = dropNumNow + 1;
                        if (dropNumNow >= dropNumMax) {
                            //Debug.Log("掉落2");
                            return dropNumNow;
                        }
                    }
                }
                else
                {
                    //掉落失败
                }


            }
            else {
                i = 10; //因为一条掉落最大支持10个道具数据
            }
        }

        return dropNumNow;
    }

    //生成掉落数量
    public bool AI_DropItem(ObscuredString itemID, Vector3 vec3, string monsterID,int dropNum = 1)
    {
        string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", itemID, "Item_Template");
        //获得道具品质
        string itemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", itemID, "Item_Template");

        //实例化一个掉落Obj
        GameObject obj_DropItem = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_Model_Drop);
        obj_DropItem.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Tr_Drop;
        obj_DropItem.transform.position = new Vector3(vec3.x, vec3.y, vec3.z);
        obj_DropItem.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        obj_DropItem.GetComponent<UI_DropName>().DropItemID = itemID;                     //设置掉落物品的ID
        if (itemID != "1")
        {
            obj_DropItem.GetComponent<UI_DropName>().DropItemNum = dropNum;               //设置掉落物品的数量
        }
        else {
            obj_DropItem.GetComponent<UI_DropName>().DropItemNum = randomSet;    
        }
        //添加极品属性字段
        if (monsterID != "0")
        {
            float HideDropPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideDropPro", "ID", monsterID, "Monster_Template"));
            obj_DropItem.GetComponent<UI_DropName>().HideDropPro = HideDropPro;
        }
        else {
            obj_DropItem.GetComponent<UI_DropName>().HideDropPro = 0;
        }

        //新手引导
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_ShiQu();
        return true;
    }


    //传入掉落ID，生成掉落数据,返回一个掉落ID和掉落数量
    public string ReturnDropItem(string dropID)
    {
        string dropReturnStr = "";
        string dropIDInitial = dropID;          //设置初始掉落
        int dropNumNow = 0;                     //当前掉落道具的数量

        //是否有子掉落
        bool DropSonStatus = false;

        //循环每行掉落数据
        do
        {
            DropSonStatus = true;
            //传入当前掉落数量和最大掉落数量给行掉落数据
            Debug.Log("dropID = " + dropID);
            //生成每行掉落数据
            for (int i = 1; i <= 10; i++)
            {
                //获取掉落道具的ID
                string dropItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID, "Drop_Template");
                string dropType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropType", "ID", dropID, "Drop_Template");
                Debug.Log("dropItemID = " + dropItemID);

                //如果道具ID不为空则触发掉落概率
                if (dropItemID != "0")
                {

                    //每个掉落
                    //获取每个掉落的概率
                    //string dropChance = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("DropChance" + i.ToString(), "ID", dropID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Drop_Template.xml");
                    string dropChance = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropChance" + i.ToString(), "ID", dropID, "Drop_Template");
                    float randomdrop = Random.Range(0, 1000000);
                    float dropChanceData = int.Parse(dropChance);
                    //Debug.Log("Str3333 = " + i);
                    //当随机值小于掉落概率值判定为掉落成功
                    if (randomdrop <= dropChanceData)
                    {
                        Debug.Log("掉落成功！掉落成功！掉落成功！掉落成功！掉落成功！掉落成功！ dropID = " + dropID);
                        //掉落成功
                        //获取掉落数量
                        string dropMinNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMinNum" + i.ToString(), "ID", dropID, "Drop_Template");
                        string dropMaxNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMaxNum" + i.ToString(), "ID", dropID, "Drop_Template");
                        //随机掉落数量
                        int itemDropNum = Random.Range(int.Parse(dropMinNum), int.Parse(dropMaxNum));
                        randomSet = itemDropNum;
                        //读取掉落道具ID
                        string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID" + i.ToString(), "ID", dropID, "Drop_Template");
                        dropReturnStr = itemID + "," + itemDropNum;
                        return dropReturnStr;
                    }
                }
                else
                {
                    i = 10; //因为一条掉落最大支持10个道具数据
                }
            }

            //获取子掉落
            dropID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropSonID", "ID", dropID, "Drop_Template");
            //没有子掉落循环取消（因为掉落ID里面可以套掉落ID）
            if (dropID == "0")
            {
                DropSonStatus = false;
            }
            //在最外层强制触发10次
            if (!DropSonStatus) {
                dropNumNow = dropNumNow + 1;
                if (dropNumNow <= 10)
                {
                    dropID = dropIDInitial;          //设置初始掉落
                    DropSonStatus = true;
                }
            }
        }
        while (DropSonStatus);

        //所有掉落都没有触发,强制掉落第一个道具
        string LastItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemID1", "ID", dropID, "Drop_Template");
        //获取掉落数量
        string LastMinNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMinNum1", "ID", dropID, "Drop_Template");
        string LastMaxNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropItemMaxNum1", "ID", dropID, "Drop_Template");
        //随机掉落数量
        int LastItemDropNum = Random.Range(int.Parse(LastMinNum), int.Parse(LastMaxNum));
        dropReturnStr = LastItemID + "," + LastItemDropNum;

        return dropReturnStr;
    }

    //AI加血（参数1：怪物Obj 参数2：加血的值 参数3 百分比加血）
    public bool AI_addHp(GameObject monster,int addHpValue,float addHpProValue =0) {

        //怪物扣血
        if (monster.GetComponent<AI_1>() != null) {
            //获取ai属性脚本
            AI_Property ai_Property = monster.GetComponent<AI_Property>();
            //判定加血后当前血量是否超越上限
            if (ai_Property.AI_Hp < ai_Property.AI_HpMax)
            {
                addHpValue = addHpValue + (int)(ai_Property.AI_HpMax * addHpProValue);
                int addHp = (int)((float)ai_Property.AI_Hp) + addHpValue;
                int addValue = addHp - ai_Property.AI_Hp;
                if (addHp >= ai_Property.AI_HpMax)
                {
                    ai_Property.AI_Hp = ai_Property.AI_HpMax;
                }
                else
                {
                    ai_Property.AI_Hp = ai_Property.AI_Hp + addHpValue;

                }
                if (addValue < 0) {
                    addValue = 0;
                }
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("3", addValue.ToString(), "1", monster, "", "");
            }
        }

        //宠物加血
        if (monster.GetComponent<AIPet>() != null)
        {
            //获取ai属性脚本
            AI_Property ai_Property = monster.GetComponent<AI_Property>();

            //判定加血后当前血量是否超越上限
            if (ai_Property.AI_Hp < ai_Property.AI_HpMax)
            {
                addHpValue = addHpValue + (int)(ai_Property.AI_HpMax * addHpProValue);
                int addHp = (int)((float)ai_Property.AI_Hp) + addHpValue;
                int addValue = addHp - ai_Property.AI_Hp;
                if (addHp >= ai_Property.AI_HpMax)
                {
                    ai_Property.AI_Hp = ai_Property.AI_HpMax;
                }
                else
                {
                    ai_Property.AI_Hp = ai_Property.AI_Hp + addHpValue;
                }
                if (addValue < 0)
                {
                    addValue = 0;
                }
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", addValue.ToString(), "1", monster, "", "");
            }
        }

        
        return true;
    }

    //AI扣血（参数1：怪物Obj 参数2：扣血的值）
    public bool AI_costHp(GameObject monster, int costHpValue,string flyType = "3")
    {
        //获取ai属性脚本
        AI_Property ai_Property = monster.GetComponent<AI_Property>();
        //判定加血后当前血量是否超越上限
        int addHp = ai_Property.AI_Hp - costHpValue;
        if (addHp <= 0 )
        {
            ai_Property.AI_Hp = 0;
        }
        else
        {
            ai_Property.AI_Hp = addHp;
        }
        //飘字
        Game_PublicClassVar.Get_function_UI.Fight_FlyText(flyType, costHpValue.ToString(), "0", monster, "", "");
        return true;
    }

    
    //创建怪物
    //创建怪物的ID,必须在Resources/CreateMonster目录下创建一同ID的怪物Obj   ,GameObject newMonsterObj
    public GameObject AI_CreatMonster(string monsterID, Vector3 CreateVec3, GameObject createrMonsterObj = null, Object newMonsterObj = null)
    {

        //Debug.Log("创建怪物！");

        //获取怪物
        GameObject monsterObj = null;
        if (newMonsterObj == null)
        {
            GameObject obj = (GameObject)Resources.Load("CreateMonster/" + monsterID, typeof(GameObject));
            if (obj != null)
            {
                monsterObj = Instantiate(obj);
            }
            else {
                Debug.LogError("未找到实例化对象:" + monsterID);
                return null;
            }
        }
        else {
            monsterObj = (GameObject)Instantiate(newMonsterObj);
        }
        //GameObject monsterSetObj = GameObject.Find("Monster");
        
        GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.MonsterSet;
        if (monsterSetObj == null) {
            monsterSetObj = GameObject.Find("Monster");
        }
        
        monsterObj.transform.SetParent(monsterSetObj.transform);
        monsterObj.transform.position = CreateVec3;
        monsterObj.SetActive(false);
        monsterObj.SetActive(true);

        if (createrMonsterObj != null) {
            monsterObj.GetComponent<AI_1>().MonsterCreateObj = createrMonsterObj;       //设置父级 用于父级怪物返回时删除召唤怪物
        }
        
        return monsterObj;

    }

    //存储怪物复活时间
    //怪物唯一ID   在线复活时间 离线复活时间
    public void SaveMonsterDeathTime(string ai_ID_Only, string deathTime,string offLineTime,string monsterID) {

        bool saveStatus = true;
        //检测存储的复活数据里有没有相同的怪物ID
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    saveStatus = false;
                }
            }
        }

        //存储状态怪物复活
        if (saveStatus) {
            if (deathMonsterIDListStr == "")
            {
                deathMonsterIDListStr = ai_ID_Only + "," + deathTime + "," + offLineTime + "," + monsterID;
            }
            else {
                deathMonsterIDListStr = deathMonsterIDListStr + ";" + ai_ID_Only + "," + deathTime + "," + offLineTime + "," + monsterID;
            }
            //Debug.Log("更新ai_ID_Only = " + ai_ID_Only + "     deathTime = " + deathTime + "    deathMonsterIDListStr = " + deathMonsterIDListStr);
        }

        //Debug.Log("deathMonsterIDListStr = " + deathMonsterIDListStr);

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
    }

    //更新怪物复活时间（废弃）
    public void UpdataMonsterDeathTime(float updataTime)
    {
        //Debug.Log("更新怪物复活时间");
        bool saveStatus = true;
        string deathMonsterIDListWriteStr = "";
        //Debug.Log("更新怪物复活时间11111111111");
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Debug.Log("更新怪物复活时间22222222222");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "") {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                float deathTimeValue = float.Parse(deathMonsterID[1]) - updataTime;
                //获取怪物ID
                string monsterID = "";
                if (deathMonsterID.Length >= 4) {
                    monsterID = deathMonsterID[3];
                }
                if (deathTimeValue < 0)
                {
                    deathTimeValue = 0;
                }
                else {
                    deathMonsterIDListWriteStr = deathMonsterIDListWriteStr + deathMonsterID[0] + "," + deathTimeValue.ToString() + "," + monsterID + ";";
                }
            }
        }

        //Debug.Log("更新怪物复活时间3333333333333");
        //写入复活时间值
        if (deathMonsterIDListWriteStr != "") {
            deathMonsterIDListWriteStr = deathMonsterIDListWriteStr.Substring(0, deathMonsterIDListWriteStr.Length - 1);
        }
        //Debug.Log("更新怪物复活时间4444444444444");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListWriteStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Debug.Log("更新怪物复活时间5555555555555");
    }


    //更新怪物离线复活时间
    public void UpdataMonsterDeathOffLineTime(float updataTime)
    {
        //Debug.Log("更新怪物复活时间" + updataTime);
        bool saveStatus = true;
        string deathMonsterIDListWriteStr = "";
        //Debug.Log("更新怪物复活时间11111111111");
        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //Debug.Log("更新怪物复活时间22222222222");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID.Length >= 4) {
                    float deathTimeValue = float.Parse(deathMonsterID[1]);
                    float deathOffLineTimeValue = float.Parse(deathMonsterID[2]) - updataTime;

                    //获取怪物ID
                    string monsterID = "";
                    if (deathMonsterID.Length >= 4)
                    {
                        monsterID = deathMonsterID[3];
                    }

                    if (deathOffLineTimeValue < 0)
                    {
                        //deathOffLineTimeValue = 0;
                        //置空删除本条复活数据
                    }
                    else
                    {
                        deathMonsterIDListWriteStr = deathMonsterIDListWriteStr + deathMonsterID[0] + "," + deathTimeValue.ToString() + "," + deathOffLineTimeValue + "," + monsterID + ";";
                    }
                }
            }
        }

        //Debug.Log("更新怪物复活时间3333333333333");
        //写入复活时间值
        if (deathMonsterIDListWriteStr != "")
        {
            deathMonsterIDListWriteStr = deathMonsterIDListWriteStr.Substring(0, deathMonsterIDListWriteStr.Length - 1);
        }
        //Debug.Log("更新怪物复活时间4444444444444");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DeathMonsterID", deathMonsterIDListWriteStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //Debug.Log("更新怪物复活时间5555555555555");
    }

    //获取怪物复活时间
    public float GetMonsterDeathTime(string ai_ID_Only)
    {

        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    //如果离线复活的时间小于在线复活的时间,则返回离线复活的时间
                    if (float.Parse(deathMonsterID[2]) < float.Parse(deathMonsterID[1])) {
                        return float.Parse(deathMonsterID[2]);
                    }
                    return float.Parse(deathMonsterID[1]);
                }
            }
        }
        return 0;
    }

    //获取怪物离线复活时间
    public float GetMonsterDeathOffLineTime(string ai_ID_Only)
    {

        string deathMonsterIDListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string[] deathMonsterIDList = deathMonsterIDListStr.Split(';');
        if (deathMonsterIDListStr != "")
        {
            for (int i = 0; i <= deathMonsterIDList.Length - 1; i++)
            {
                string[] deathMonsterID = deathMonsterIDList[i].Split(',');
                if (deathMonsterID[0] == ai_ID_Only)
                {
                    //如果离线复活的时间小于在线复活的时间,则返回离线复活的时间
                    /*
                    if (float.Parse(deathMonsterID[2]) < float.Parse(deathMonsterID[1]))
                    {
                        return float.Parse(deathMonsterID[2]);
                    }
                     */
                    return float.Parse(deathMonsterID[2]);
                }
            }
        }
        return 0;
    }


    //宠物属性成长
    public float GetPetPropertyChengZhang(string type) {
        switch (type) {
            //力量成长
            case "1":
                return 5;
                break;
            //魔法成长
            case "2":
                return 5;
                break;
            //血量成长
            case "3":
                return 80;
                break;
            //物防成长
            case "4":
                return 4f;
                break;
            //魔防成长
            case "5":
                return 4f;
                break;
        }

        return 1;
    }

    //更新宠物属性
    public void UpdatePetProperty(GameObject petObj,string petType,bool ifInit = false) {

        //Debug.Log("更新宠物属性");
        int pet_Hp = 0;
        int pet_Act = 0;
        int pet_MageAct = 0;
        int pet_Def = 0;
        int pet_Adf = 0;
        int pet_ActSpeed = 0;
        float pet_Cri = 0;
        float pet_Hit = 0;
        float pet_Dodge = 0;
        float pet_MoveSpeed = 0;

        float pet_HuiXuePro = 0;                //回血
        float pet_XiXieValue = 0;           	//AI吸血值
        float pet_XingYunPro = 0;				//幸运
        float pet_DiXiaoPro = 0;				//概率抵消本次伤害
        float pet_ChouHenValue = 0;			    //仇恨值
        float pet_FuHuoPro = 0;				    //复活概率

        float pet_resistance_1 = 0;                 //光抗性
        float pet_resistance_2 = 0;                 //暗抗性
        float pet_resistance_3 = 0;                 //火抗性
        float pet_resistance_4 = 0;                 //水抗性
        float pet_resistance_5 = 0;                 //电抗性
        float pet_raceDamge_1 = 0;                  //野兽攻击伤害
        float pet_raceDamge_2 = 0;                  //人物攻击伤害
        float pet_raceDamge_3 = 0;                  //恶魔攻击伤害
        float pet_raceResistance_1 = 0;             //野兽攻击抗性
        float pet_raceResistance_2 = 0;             //人物攻击抗性
        float pet_raceResistance_3 = 0;             //恶魔攻击抗性

        float pet_SkillActDefPro = 0;               //技能抗性
        float pet_SkillActDodgePro = 0;             //技能闪避

        float pet_MonsterActDameDefPro;           //首领攻击免伤率
        float pet_MonsterSkillDameDefPro;           //首领技能免伤率

        string rosePet_ID = petObj.GetComponent<AIPet>().RosePet_ID;
        string petID = "";

        string petTianTiType = petObj.GetComponent<AIPet>().PetTianTiType;

        //宠物
        if(petType == "0"){
            petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePet_ID, "RosePet");
        }
        
        //玩家召唤
        if (petType == "1"|| petType =="4") {
            petID = petObj.GetComponent<AIPet>().AI_ID.ToString();
        }

        //天梯召唤
        if (petType == "2")
        {
            petID = petObj.GetComponent<AIPet>().AI_ID.ToString();
        }

        int baseHp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Hp", "ID", petID, "Pet_Template"));
        int base_Act = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Act", "ID", petID, "Pet_Template"));
        int base_MageAct = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_MageAct", "ID", petID, "Pet_Template"));
        int base_Def = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Def", "ID", petID, "Pet_Template"));
        int base_Adf = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Adf", "ID", petID, "Pet_Template"));
        int base_ActSpeed = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_ActSpeed", "ID", petID, "Pet_Template"));
        float base_Cri = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Cri", "ID", petID, "Pet_Template"));
        float base_Hit = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Hit", "ID", petID, "Pet_Template"));
        float base_Dodge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Dodge", "ID", petID, "Pet_Template"));
        float base_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_MoveSpeed", "ID", petID, "Pet_Template"));
        int petLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Pet_Lv", "ID", petID, "Pet_Template"));


        //获取属性
        string petDataStr = petObj.GetComponent<AIPet>().PetTianTiDataStr;
        string[] petDataStrList = petDataStr.Split('@');

        //宠物装备
        string[] equipTianTiIDList = new string[4];
        string[] equipHideTianTiIDList = new string[4];

        if (petType == "2" && petTianTiType == "2")
        {
            if (petDataStrList.Length >= 20)
            {
                equipTianTiIDList[0] = petDataStrList[19];
                equipHideTianTiIDList[0] = petDataStrList[20];
                equipTianTiIDList[1] = petDataStrList[21];
                equipHideTianTiIDList[1] = petDataStrList[22];
                equipTianTiIDList[2] = petDataStrList[23];
                equipHideTianTiIDList[2] = petDataStrList[24];
            }
            if (petDataStrList.Length >= 26) {
                equipTianTiIDList[3] = petDataStrList[25];
                equipHideTianTiIDList[3] = petDataStrList[26];
            }
        }


        int hp_Equip_valueSum = 0;
        int act_EquipMax_valueSum = 0;
        int magact_EquipMax_valueSum = 0;
        int def_EquipMax_valueSum = 0;
        int adf_EquipMax_valueSum = 0;

        float cir_Equip_valueSum = 0;
        float hit_Equip_valueSum = 0;
        float dodge_Equip_valueSum = 0;

        //宠物需要读取装备属性
        if (petType == "0" || petType == "2")
        {

            for (int y = 1; y <= 4; y++) {

                Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;

                string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + y, "ID", rosePet_ID, "RosePet");

                //天梯特殊处理
                if (petType == "2" && petTianTiType == "2")
                {
                    nowItemID = equipTianTiIDList[y - 1];
                }

                string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", nowItemID, "Item_Template");
                if (equipID != "" && equipID != "0" && equipID != "-1") {
                    int hp_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipID, "Equip_Template"));
                    int act_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipID, "Equip_Template"));
                    int act_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipID, "Equip_Template"));
                    int magact_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equipID, "Equip_Template"));
                    int magact_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equipID, "Equip_Template"));
                    int def_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipID, "Equip_Template"));
                    int def_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipID, "Equip_Template"));
                    int adf_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipID, "Equip_Template"));
                    int adf_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipID, "Equip_Template"));
                    float cir_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipID, "Equip_Template"));
                    float hit_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipID, "Equip_Template"));
                    float dodge_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipID, "Equip_Template"));
                    float damgeAdd_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipID, "Equip_Template"));
                    float damgeSub_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipID, "Equip_Template"));
                    float speed_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipID, "Equip_Template"));
                    int lucky_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipID, "Equip_Template"));

                    //累加属性
                    hp_Equip_valueSum = hp_Equip_valueSum + hp_Equip_value;
                    act_EquipMax_valueSum = act_EquipMax_valueSum + act_EquipMax_value;
                    magact_EquipMax_valueSum = magact_EquipMax_valueSum + magact_EquipMax_value;
                    def_EquipMax_valueSum = def_EquipMax_valueSum + def_EquipMax_value;
                    adf_EquipMax_valueSum = adf_EquipMax_valueSum + adf_EquipMax_value;

                    cir_Equip_valueSum = cir_Equip_valueSum + cir_Equip_value;
                    hit_Equip_valueSum = hit_Equip_valueSum + base_Hit;
                    dodge_Equip_valueSum = dodge_Equip_valueSum + dodge_Equip_value;
                }
            }

            /*
            baseHp = hp_Equip_valueSum + baseHp;
            base_Act = act_EquipMax_valueSum + base_Act;
            base_MageAct = magact_EquipMax_valueSum + base_MageAct;
            base_Def = def_EquipMax_valueSum + base_Def;
            base_Adf = adf_EquipMax_valueSum + base_Adf;

            base_Cri = cir_Equip_valueSum + base_Cri;
            base_Hit = hit_Equip_valueSum + base_Hit;
            base_Dodge = dodge_Equip_valueSum + base_Dodge;
            */
        }


        //玩家召唤,属性跟着玩家角色走
        if (petType == "4")
        {
            float petPropertyPro = petObj.GetComponent<AI_Property>().AI_SummonPropertyPro;
            float petPropertyHpPro = petObj.GetComponent<AI_Property>().AI_SummonPropertyHpPro;
            float petPropertyActPro = petObj.GetComponent<AI_Property>().AI_SummonPropertyActPro;
            float petPropertyDefPro = petObj.GetComponent<AI_Property>().AI_SummonPropertyDefPro;

            //幸运加成
            float luckAdd = LuckRetuenAddDamge();
            Rose_Proprety roseProprety = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
            //Debug.Log("roseProprety.Rose_Hp = " + roseProprety.Rose_Hp + "petPropertyHpPro = " + petPropertyHpPro);

            ObscuredFloat base_1 = 0.65f;
            ObscuredFloat base_2 = 0.2f;
            ObscuredFloat base_3 = 0.6f;

            baseHp = (int)((float)(roseProprety.Rose_Hp) * petPropertyHpPro);
            base_Act = (int)((float)(roseProprety.Rose_ActMax) * (base_1 + luckAdd) * (petPropertyPro+ petPropertyActPro));
            base_MageAct = (int)((float)(roseProprety.Rose_MagActMax) * (base_1 + luckAdd) * (petPropertyPro + petPropertyActPro));
            base_Def = (int)((float)(roseProprety.Rose_DefMin + roseProprety.Rose_DefMax) * base_2 * (petPropertyPro + petPropertyDefPro));
            base_Adf = (int)((float)(roseProprety.Rose_AdfMin + roseProprety.Rose_AdfMax) * base_3 * (petPropertyPro + petPropertyDefPro));
            base_ActSpeed = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_ActSpeed", "ID", petID, "Pet_Template"));
            base_Cri = roseProprety.Rose_Cri;
            ObscuredFloat hit_Pro = 0.75f;
            base_Hit = hit_Pro + roseProprety.Rose_Hit;
            base_Dodge = roseProprety.Rose_Dodge;
            base_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_MoveSpeed", "ID", petID, "Pet_Template"));
            petLv = roseProprety.Rose_Lv;

        }


        //宠物
        if (petType == "0"|| petType=="2"&&petTianTiType=="1")
        {
            //Debug.Log("更新宠物资质111111!");

            //更新宠物资质
            float zizhiNow_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", rosePet_ID, "RosePet"));
            float zizhiNow_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", rosePet_ID, "RosePet"));

            //int petLv = petObj.GetComponent<AI_Property>().AI_Lv;
            petLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePet_ID, "RosePet"));

            //资质修正,做出1199和1200的数值差距
            zizhiNow_Hp = (int)(zizhiNow_Hp / 100) * 100 + (zizhiNow_Hp - (int)(zizhiNow_Hp / 100) * 100) / 1.5f;
            zizhiNow_Act = (int)(zizhiNow_Act / 100) * 100 + (zizhiNow_Act - (int)(zizhiNow_Act / 100) * 100) / 1.5f;
            zizhiNow_MageAct = (int)(zizhiNow_MageAct / 100) * 100 + (zizhiNow_MageAct - (int)(zizhiNow_MageAct / 100) * 100) / 1.5f;
            zizhiNow_Def = (int)(zizhiNow_Def / 100) * 100 + (zizhiNow_Def - (int)(zizhiNow_Def / 100) * 100) / 1.5f;
            zizhiNow_Adf = (int)(zizhiNow_Adf / 100) * 100 + (zizhiNow_Adf - (int)(zizhiNow_Adf / 100) * 100) / 1.5f;
            zizhiNow_ActSpeed = (int)(zizhiNow_ActSpeed / 100) * 100 + (zizhiNow_ActSpeed - (int)(zizhiNow_ActSpeed / 100) * 100) / 1.5f;

            float zizhiPro_Hp = zizhiNow_Hp / 6000;
            float zizhiPro_Act = zizhiNow_Act / 1500;
            float zizhiPro_MageAct = zizhiNow_MageAct / 1500;
            float zizhiPro_Def = zizhiNow_Def / 1500;
            float zizhiPro_Adf = zizhiNow_Adf / 1500;
            float zizhiPro_ActSpeed = 1;                        //暂时取消速度资质
            float zizhiPro_ChengZhang = zizhiNow_ChengZhang;

            float lv_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Hp", "ID", petID, "Pet_Template"));
            float lv_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Act", "ID", petID, "Pet_Template"));
            float lv_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_MageAct", "ID", petID, "Pet_Template"));
            float lv_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Def", "ID", petID, "Pet_Template"));
            float lv_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Adf", "ID", petID, "Pet_Template"));

            //加点属性
            string addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID", rosePet_ID, "RosePet");
            int NowPropertyValue_LiLiang = 0;
            int NowPropertyValue_ZhiLi = 0;
            int NowPropertyValue_TiZhi = 0;
            int NowPropertyValue_NaiLi = 0;
            if (addPropretyValue != "")
            {
                string[] addPropertyList = addPropretyValue.Split(';');
                NowPropertyValue_LiLiang = int.Parse(addPropertyList[0]);
                NowPropertyValue_ZhiLi = int.Parse(addPropertyList[1]);
                NowPropertyValue_TiZhi = int.Parse(addPropertyList[2]);
                NowPropertyValue_NaiLi = int.Parse(addPropertyList[3]);
            }

            //加点所加的属性点
            base_Act = base_Act + (int)(NowPropertyValue_LiLiang * GetPetPropertyChengZhang("1"));
            base_MageAct = base_MageAct + (int)(NowPropertyValue_ZhiLi * GetPetPropertyChengZhang("2"));
            baseHp = baseHp * 5 + NowPropertyValue_TiZhi * (int)(GetPetPropertyChengZhang("3"));
            base_Def = base_Def + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("4"));
            base_Adf = base_Adf + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("5"));
            
            /*
            baseHp = baseHp + (int)((lv_Hp * petLv) * zizhiPro_Hp * zizhiPro_ChengZhang);
            base_Act = base_Act + (int)((lv_Act * petLv) * zizhiPro_Act * zizhiPro_ChengZhang);
            base_MageAct = base_MageAct + (int)((lv_MageAct * petLv) * zizhiPro_MageAct * zizhiPro_ChengZhang);
            base_Def = base_Def + (int)((lv_Def * petLv) * zizhiPro_Def * zizhiPro_ChengZhang);
            base_Adf = base_Adf + (int)((lv_Adf * petLv) * zizhiPro_Adf * zizhiPro_ChengZhang);
            base_ActSpeed = (int)(base_ActSpeed / zizhiPro_ActSpeed);
            */

            //属性
            baseHp = (int)((baseHp + lv_Hp * petLv) * zizhiPro_Hp * zizhiPro_ChengZhang);
            base_Act = (int)((base_Act + lv_Act * petLv) * zizhiPro_Act * zizhiPro_ChengZhang);
            base_MageAct = (int)((base_MageAct + lv_MageAct * petLv) * zizhiPro_MageAct * zizhiPro_ChengZhang);
            base_Def = (int)((base_Def + lv_Def * petLv) * zizhiPro_Def * zizhiPro_ChengZhang);
            base_Adf = (int)((base_Adf + lv_Adf * petLv) * zizhiPro_Adf * zizhiPro_ChengZhang);
            base_ActSpeed = (int)(base_ActSpeed / zizhiPro_ActSpeed);


            //Debug.Log("baseHp = " + baseHp + "lv_Hp = " + lv_Hp + "petLv = " + petLv + "zizhiPro_Hp = " + zizhiPro_Hp + "zizhiPro_ChengZhang = " + zizhiPro_ChengZhang);

            //检测宠物属性
            int sumdianshu = NowPropertyValue_LiLiang + NowPropertyValue_ZhiLi + NowPropertyValue_TiZhi + NowPropertyValue_NaiLi;
            int nowdianshu = petLv * 10 + 200;
            if (sumdianshu >= nowdianshu)
            {
                //判定为封号
                Game_PublicClassVar.Get_function_Rose.SendFengHao("宠物属性异常");
            }
        }



        if (petType == "2"&&petTianTiType == "2") {

            //Debug.Log("更新宠物资质222222");
            //Debug.Log("读取宠物数据：" + petDataStr);
            //更新宠物资质
            float zizhiNow_Hp = float.Parse(petDataStrList[11]);
            float zizhiNow_Act = float.Parse(petDataStrList[12]);
            float zizhiNow_MageAct = float.Parse(petDataStrList[13]);
            float zizhiNow_Def = float.Parse(petDataStrList[14]);
            float zizhiNow_Adf = float.Parse(petDataStrList[15]);
            float zizhiNow_ActSpeed = float.Parse(petDataStrList[16]);
            float zizhiNow_ChengZhang = float.Parse(petDataStrList[17]);

            //int petLv = petObj.GetComponent<AI_Property>().AI_Lv;
            petLv = int.Parse(petDataStrList[2]);

            //资质修正,做出1199和1200的数值差距
            zizhiNow_Hp = (int)(zizhiNow_Hp / 100) * 100 + (zizhiNow_Hp - (int)(zizhiNow_Hp / 100) * 100) / 1.5f;
            zizhiNow_Act = (int)(zizhiNow_Act / 100) * 100 + (zizhiNow_Act - (int)(zizhiNow_Act / 100) * 100) / 1.5f;
            zizhiNow_MageAct = (int)(zizhiNow_MageAct / 100) * 100 + (zizhiNow_MageAct - (int)(zizhiNow_MageAct / 100) * 100) / 1.5f;
            zizhiNow_Def = (int)(zizhiNow_Def / 100) * 100 + (zizhiNow_Def - (int)(zizhiNow_Def / 100) * 100) / 1.5f;
            zizhiNow_Adf = (int)(zizhiNow_Adf / 100) * 100 + (zizhiNow_Adf - (int)(zizhiNow_Adf / 100) * 100) / 1.5f;
            zizhiNow_ActSpeed = (int)(zizhiNow_ActSpeed / 100) * 100 + (zizhiNow_ActSpeed - (int)(zizhiNow_ActSpeed / 100) * 100) / 1.5f;

            float zizhiPro_Hp = zizhiNow_Hp / 6000;
            float zizhiPro_Act = zizhiNow_Act / 1500;
            float zizhiPro_MageAct = zizhiNow_MageAct / 1500;
            float zizhiPro_Def = zizhiNow_Def / 1500;
            float zizhiPro_Adf = zizhiNow_Adf / 1500;
            float zizhiPro_ActSpeed = 1;                        //暂时取消速度资质
            float zizhiPro_ChengZhang = zizhiNow_ChengZhang;

            float lv_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Hp", "ID", petID, "Pet_Template"));
            float lv_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Act", "ID", petID, "Pet_Template"));
            float lv_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_MageAct", "ID", petID, "Pet_Template"));
            float lv_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Def", "ID", petID, "Pet_Template"));
            float lv_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Adf", "ID", petID, "Pet_Template"));

            //加点属性
            string addPropretyValue = petDataStrList[9];
            int NowPropertyValue_LiLiang = 0;
            int NowPropertyValue_ZhiLi = 0;
            int NowPropertyValue_TiZhi = 0;
            int NowPropertyValue_NaiLi = 0;
            if (addPropretyValue != "")
            {
                string[] addPropertyList = addPropretyValue.Split(';');
                NowPropertyValue_LiLiang = int.Parse(addPropertyList[0]);
                NowPropertyValue_ZhiLi = int.Parse(addPropertyList[1]);
                NowPropertyValue_TiZhi = int.Parse(addPropertyList[2]);
                NowPropertyValue_NaiLi = int.Parse(addPropertyList[3]);
            }

            //加点所加的属性点
            base_Act = base_Act + (int)(NowPropertyValue_LiLiang * GetPetPropertyChengZhang("1"));
            base_MageAct = base_MageAct + (int)(NowPropertyValue_ZhiLi * GetPetPropertyChengZhang("2"));
            baseHp = baseHp * 5 + NowPropertyValue_TiZhi * (int)(GetPetPropertyChengZhang("3"));
            base_Def = base_Def + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("4"));
            base_Adf = base_Adf + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("5"));

            baseHp = (int)((baseHp + lv_Hp * petLv) * zizhiPro_Hp * zizhiPro_ChengZhang);
            base_Act = (int)((base_Act + lv_Act * petLv) * zizhiPro_Act * zizhiPro_ChengZhang);
            base_MageAct = (int)((base_MageAct + lv_MageAct * petLv) * zizhiPro_MageAct * zizhiPro_ChengZhang);
            base_Def = (int)((base_Def + lv_Def * petLv) * zizhiPro_Def * zizhiPro_ChengZhang);
            base_Adf = (int)((base_Adf + lv_Adf * petLv) * zizhiPro_Adf * zizhiPro_ChengZhang);
            base_ActSpeed = (int)(base_ActSpeed / zizhiPro_ActSpeed);

            //Debug.Log("baseHp = " + baseHp + "lv_Hp = " + lv_Hp + "petLv = " + petLv + "zizhiPro_Hp = " + zizhiPro_Hp + "zizhiPro_ChengZhang = " + zizhiPro_ChengZhang);

            //检测宠物属性
            int sumdianshu = NowPropertyValue_LiLiang + NowPropertyValue_ZhiLi + NowPropertyValue_TiZhi + NowPropertyValue_NaiLi;
            int nowdianshu = petLv * 10 + 200;
            if (sumdianshu >= nowdianshu)
            {
                //判定为封号
                Game_PublicClassVar.Get_function_Rose.SendFengHao("宠物属性异常");
            }
        }

        //更新宠物技能
        string NowSclectPetID = "0";
        //宠物
        if (petType == "0")
        {
            NowSclectPetID = petObj.GetComponent<AIPet>().RosePet_ID;             //后期调整
        }

        int pet_Hp_Skill = 0;
        float pet_HpPro_Skill = 0.0f;
        int pet_Act_Skill = 0;
        float pet_ActPro_Skill = 0.0f;
        int pet_MageAct_Skill = 0;
        float pet_MageActPro_Skill = 0.0f;
        int pet_Def_Skill = 0;
        float pet_DefPro_Skill = 0.0f;
        int pet_Adf_Skill = 0;
        float pet_AdfPro_Skill = 0.0f;
        //int pet_ActSpeed_Skill = 0;
        float pet_Cri_Skill = 0;
        float pet_Hit_Skill = 0;
        float pet_Dodge_Skill = 0;
        float pet_MoveSpeed_Skill = 0;
        float pet_ActDamgeAdd_Skill = 0;

        float pet_Res_Skill = 0;                //怪物韧性
        float pet_DefAdd_Skill = 0;             //怪物物理免伤
        float pet_AdfAdd_Skill = 0;             //怪物魔法免伤
        float pet_DamgeAdd_Skill = 0;           //怪物伤害免伤

        float pet_HuiXuePro_Skill = 0;              //回血
        float pet_XiXieValue_Skill = 0;           	//AI吸血值
        float pet_XingYunPro_Skill = 0;				//幸运
        float pet_DiXiaoPro_Skill = 0;				//概率抵消本次伤害
        float pet_ChouHenValue_Skill = 0;			//仇恨值
        float pet_FuHuoPro_Skill = 0;				//复活概率

        float pet_resistance_1_Skill = 0;                //光抗性
        float pet_resistance_2_Skill = 0;                //暗抗性
        float pet_resistance_3_Skill = 0;                //火抗性
        float pet_resistance_4_Skill = 0;                //水抗性
        float pet_resistance_5_Skill = 0;                //电抗性

        float pet_raceDamge_1_Skill = 0;                //野兽攻击伤害
        float pet_raceDamge_2_Skill = 0;                //人物攻击伤害
        float pet_raceDamge_3_Skill = 0;                //恶魔攻击伤害

        float pet_raceResistance_1_Skill = 0;            //野兽攻击抗性
        float pet_raceResistance_2_Skill = 0;            //人物攻击抗性
        float pet_raceResistance_3_Skill = 0;            //恶魔攻击抗性

        float pet_SkillActDefPro_Skill = 0;               //技能抗性
        float pet_SkillActDodgePro_Skill = 0;             //技能闪避

        float pet_MonsterActDameDefPro_Skill = 0;           //怪物攻击伤害减免
        float pet_MonsterSkillDameDefPro_Skill = 0;         //怪怪物技能伤害减免


        string petSkillListStr = "";
        //宠物
        if (petType == "0")
        {
            petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", NowSclectPetID, "RosePet");
        }

        //玩家召唤
        if (petType == "1"|| petType == "4")
        {
            petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BaseSkillID", "ID", petID, "Pet_Template");
        }

        //玩家召唤
        if (petType == "2")
        {
            if (petTianTiType == "1") {
                petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePet_ID, "RosePet");
            }

            if (petTianTiType == "2") {
                petSkillListStr = petDataStrList[18];
            }
        }

        string[] petSkillList = petSkillListStr.Split(';');

        //宠物被动技能互斥技能ID
        string HuChiSkillIDStr = "";
        for (int i = 0; i <= petSkillList.Length - 1; i++)
        {
            string huchiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuChiID", "ID", petSkillList[i], "Skill_Template");
            if (huchiID != "" && huchiID != "0") {
                HuChiSkillIDStr = HuChiSkillIDStr + huchiID + ";";
            }
        }
        string[] HuChiSkillIDList = HuChiSkillIDStr.Split(';');

        //显示宠物技能
        for (int i = 0; i <= petSkillList.Length - 1; i++)
        {
            //检测当前ID是不是互斥ID是则当前技能无效
            bool IfHuChiStatus = false;
            for (int z = 0; z <= HuChiSkillIDList.Length - 1; z++)
            {
                if (petSkillList[i] == HuChiSkillIDList[z]) { 
                    IfHuChiStatus = true;
                }
            }

            if (!IfHuChiStatus)
            {
                string nowPetSkillID = petSkillList[i];
                string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", nowPetSkillID, "Skill_Template");
                //Debug.Log("nowPetSkillID = " + nowPetSkillID + ";gameObjectParameter = " + gameObjectParameter);
                if (gameObjectParameter != "0" && gameObjectParameter != "") {

                    //获取技能参数
                    string[] skillPar = gameObjectParameter.Split(';');
                    for (int y = 0; y <= skillPar.Length - 1; y++)
                    {
                        if (skillPar[y].Split(',').Length >= 2)
                        {
                            string skillPetType = skillPar[y].Split(',')[0];
                            string skillPetValue = skillPar[y].Split(',')[1];

                            switch (skillPetType)
                            {
                                //11 攻击
                                case "11":
                                    pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                    break;

                                //12 攻击百分比
                                case "12":
                                    pet_ActPro_Skill = pet_ActPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //15 魔法攻击
                                case "15":
                                    pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                    break;

                                //16 魔法攻击百分比
                                case "16":
                                    pet_MageActPro_Skill = pet_MageActPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //21 物防
                                case "21":
                                    pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                    break;

                                //31 物防百分比
                                case "22":
                                    pet_DefPro_Skill = pet_DefPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //31 魔防
                                case "31":
                                    pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                    break;

                                //32 魔防百分比
                                case "32":
                                    pet_AdfPro_Skill = pet_AdfPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //41 血量
                                case "41":
                                    pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                    break;

                                //42 血量百分比
                                case "42":
                                    pet_HpPro_Skill = pet_HpPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //暴击
                                case "101":
                                    pet_Cri_Skill = float.Parse(skillPetValue);
                                    //Debug.Log("技能添加暴击:" + pet_Cri_Skill);
                                    break;
                                //韧性
                                case "102":
                                    pet_Res_Skill = float.Parse(skillPetValue);
                                    break;
                                //命中
                                case "103":
                                    pet_Hit_Skill = float.Parse(skillPetValue);
                                    break;
                                //闪避
                                case "104":
                                    pet_Dodge_Skill = float.Parse(skillPetValue);
                                    break;
                                //移动速度
                                case "105":
                                    pet_MoveSpeed_Skill = float.Parse(skillPetValue);
                                    break;
                                //物理免伤
                                case "106":
                                    pet_DefAdd_Skill = float.Parse(skillPetValue);
                                    break;
                                //魔法免伤
                                case "107":
                                    pet_AdfAdd_Skill = float.Parse(skillPetValue);
                                    break;
                                //伤害加成
                                case "108":
                                    pet_DamgeAdd_Skill = float.Parse(skillPetValue);
                                    break;
                                //伤害减免
                                case "109":
                                    pet_ActDamgeAdd_Skill = float.Parse(skillPetValue);
                                    break;
                                //吸血值
                                case "110":
                                    pet_XiXieValue_Skill = float.Parse(skillPetValue);
                                    break;
                                //回血值
                                case "111":
                                    pet_HuiXuePro_Skill = float.Parse(skillPetValue);
                                    break;
                                //幸运
                                case "112":
                                    pet_XingYunPro_Skill = float.Parse(skillPetValue);
                                    break;
                                //仇恨值
                                case "113":
                                    pet_ChouHenValue_Skill = float.Parse(skillPetValue);
                                    break;
                                //概率抵消本次伤害
                                case "114":
                                    pet_DiXiaoPro_Skill = float.Parse(skillPetValue);
                                    break;
                                //复活概率
                                case "115":
                                    pet_FuHuoPro_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "121":
                                    pet_raceDamge_1_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "122":
                                    pet_raceDamge_2_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "123":
                                    pet_raceDamge_3_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "124":
                                    pet_raceResistance_1_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "125":
                                    pet_raceResistance_2_Skill = float.Parse(skillPetValue);
                                    break;
                                //野兽攻击
                                case "126":
                                    pet_raceResistance_3_Skill = float.Parse(skillPetValue);
                                    break;
                                //光抗性
                                case "131":
                                    pet_resistance_1_Skill = float.Parse(skillPetValue);
                                    break;
                                //暗抗性
                                case "132":
                                    pet_resistance_2_Skill = float.Parse(skillPetValue);
                                    break;
                                //火抗性
                                case "133":
                                    pet_resistance_3_Skill = float.Parse(skillPetValue);
                                    break;
                                //水抗性
                                case "134":
                                    pet_resistance_4_Skill = float.Parse(skillPetValue);
                                    break;
                                //电抗性
                                case "135":
                                    pet_resistance_5_Skill = float.Parse(skillPetValue);
                                    break;

                                //技能抗性
                                case "141":
                                    pet_SkillActDefPro_Skill = float.Parse(skillPetValue);
                                    break;

                                //技能闪避
                                case "142":
                                    pet_SkillActDodgePro_Skill = float.Parse(skillPetValue);
                                    break;

                                //怪物攻击伤害减免
                                case "151":
                                    pet_MonsterActDameDefPro_Skill = pet_MonsterActDameDefPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //怪怪物技能伤害减免
                                case "152":
                                    pet_MonsterSkillDameDefPro_Skill = pet_MonsterSkillDameDefPro_Skill + float.Parse(skillPetValue);
                                    break;
                            }
                        }
                    }            
                }
            }
        }


        string xiuLianStr = ";;;";
        string[] XiuLianLvList = xiuLianStr.Split(';');
        //宠物
        if (petType == "0")
        {
            xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            XiuLianLvList = xiuLianStr.Split(';');
        }

        //玩家召唤
        if (petType == "1")
        {
            xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            XiuLianLvList = xiuLianStr.Split(';');
        }

        //玩家召唤
        if (petType == "2")
        {
            if (petTianTiType == "1")
            {
                xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                XiuLianLvList = xiuLianStr.Split(';');
            }

            if (petTianTiType == "2")
            {
                xiuLianStr = petObj.GetComponent<AIPet>().PetTianTiXiuLianStr;
                if (xiuLianStr != null && xiuLianStr != "") {
                    XiuLianLvList = xiuLianStr.Split(';');
                }
            }
        }


        //获取宠物装备
        string EquioHideStr = "";
        for (int z = 1; z <= 4; z++)
        {
            string equipHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_" + z, "ID", rosePet_ID, "RosePet");

            if (equipHideID != "" && equipHideID != "0" && equipHideID != null) {
                string PrepeotyListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", equipHideID, "RoseEquipHideProperty");
                if (PrepeotyListStr != "" && PrepeotyListStr != "0" && PrepeotyListStr != null) {
                    EquioHideStr = EquioHideStr + PrepeotyListStr + ";";
                }
            }

            //天梯特殊处理
            if (petType == "2" && petTianTiType == "2")
            {
                equipHideID = equipHideTianTiIDList[z - 1];

                //获取缓存数据
                //通过传输字段来获取
                if (petObj.GetComponent<AIPet>() != null)
                {
                    string PrepeotyListStr = petObj.GetComponent<AIPet>().GetHidePro(equipHideID);
                    if (PrepeotyListStr != "" && PrepeotyListStr != "0" && PrepeotyListStr != null)
                    {
                        EquioHideStr = EquioHideStr + PrepeotyListStr + ";";
                    }
                }
                
            }
        }


        for (int i = 0; i < XiuLianLvList.Length; i++) {

            string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPro", "ID", XiuLianLvList[i], "PetXiuLian_Template");

            if (EquioHideStr != "" && i == 0) {
                gameObjectParameter = EquioHideStr + gameObjectParameter;
            }

            if (gameObjectParameter != "0" && gameObjectParameter != "")
            {

                //获取技能参数
                string[] skillPar = gameObjectParameter.Split(';');
                for (int y = 0; y <= skillPar.Length - 1; y++)
                {
                    if (skillPar[y].Split(',').Length >= 2)
                    {
                        string skillPetType = skillPar[y].Split(',')[0];
                        string skillPetValue = skillPar[y].Split(',')[1];

                        switch (skillPetType)
                        {

                            //血量上限
                            case "1":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;
                            //物理攻击最大值
                            case "2":
                                pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                break;
                            case "3":
                                //物理防御最大值
                                pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                break;
                            //魔法防御最大值
                            case "4":
                                pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                break;
                            //魔法攻击最大值
                            case "5":
                                pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                break;

                            //血量
                            case "10":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;

                            //11 攻击
                            case "11":
                                pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                break;

                            //12 攻击百分比
                            case "12":
                                pet_ActPro_Skill = pet_ActPro_Skill + float.Parse(skillPetValue);
                                break;

                            //15 魔法攻击
                            case "15":
                                pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                break;

                            //16 魔法攻击百分比
                            case "16":
                                pet_MageActPro_Skill = pet_MageActPro_Skill + float.Parse(skillPetValue);
                                break;

                            //21 物防
                            case "21":
                                pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                break;

                            //31 物防百分比
                            case "22":
                                pet_DefPro_Skill = pet_DefPro_Skill + float.Parse(skillPetValue);
                                break;

                            //31 魔防
                            case "31":
                                pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                break;

                            //32 魔防百分比
                            case "32":
                                pet_AdfPro_Skill = pet_AdfPro_Skill + float.Parse(skillPetValue);
                                break;

                            //41 血量
                            case "41":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;

                            //42 血量百分比
                            case "42":
                                pet_HpPro_Skill = pet_HpPro_Skill + float.Parse(skillPetValue);
                                break;

                            //暴击
                            case "101":
                                pet_Cri_Skill = pet_Cri_Skill + float.Parse(skillPetValue);
                                //Debug.Log("技能添加暴击:" + pet_Cri_Skill);
                                break;
                            //韧性
                            case "102":
                                pet_Res_Skill = pet_Res_Skill + float.Parse(skillPetValue);
                                break;
                            //命中
                            case "103":
                                pet_Hit_Skill = pet_Hit_Skill + float.Parse(skillPetValue);
                                break;
                            //闪避
                            case "104":
                                pet_Dodge_Skill = pet_Dodge_Skill + float.Parse(skillPetValue);
                                break;
                            //移动速度
                            case "105":
                                pet_MoveSpeed_Skill = pet_MoveSpeed_Skill + float.Parse(skillPetValue);
                                break;
                            //物理免伤
                            case "106":
                                pet_DefAdd_Skill = pet_DefAdd_Skill + float.Parse(skillPetValue);
                                break;
                            //魔法免伤
                            case "107":
                                pet_AdfAdd_Skill = pet_AdfAdd_Skill + float.Parse(skillPetValue);
                                break;
                            //伤害减免
                            case "108":
                                pet_DamgeAdd_Skill = pet_DamgeAdd_Skill + float.Parse(skillPetValue);
                                break;
                            //伤害减免
                            case "109":
                                pet_ActDamgeAdd_Skill = pet_ActDamgeAdd_Skill + float.Parse(skillPetValue);
                                break;
                            //吸血值
                            case "110":
                                pet_XiXieValue_Skill = pet_XiXieValue_Skill + float.Parse(skillPetValue);
                                break;
                            //回血值
                            case "111":
                                pet_HuiXuePro_Skill = pet_HuiXuePro_Skill + float.Parse(skillPetValue);
                                break;
                            //幸运
                            case "112":
                                pet_XingYunPro_Skill = pet_XingYunPro_Skill + float.Parse(skillPetValue);
                                break;
                            //仇恨值
                            case "113":
                                pet_ChouHenValue_Skill = pet_ChouHenValue_Skill + float.Parse(skillPetValue);
                                break;
                            //概率抵消本次伤害
                            case "114":
                                pet_DiXiaoPro_Skill = pet_DiXiaoPro_Skill + float.Parse(skillPetValue);
                                break;
                            //复活概率
                            case "115":
                                pet_FuHuoPro_Skill = pet_FuHuoPro_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "121":
                                pet_raceDamge_1_Skill = pet_raceDamge_1_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "122":
                                pet_raceDamge_2_Skill = pet_raceDamge_2_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "123":
                                pet_raceDamge_3_Skill = pet_raceDamge_3_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "124":
                                pet_raceResistance_1_Skill = pet_raceResistance_1_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "125":
                                pet_raceResistance_2_Skill = pet_raceResistance_2_Skill + float.Parse(skillPetValue);
                                break;
                            //野兽攻击
                            case "126":
                                pet_raceResistance_3_Skill = pet_raceResistance_3_Skill + float.Parse(skillPetValue);
                                break;
                            //光抗性
                            case "131":
                                pet_resistance_1_Skill = pet_resistance_1_Skill + float.Parse(skillPetValue);
                                break;
                            //暗抗性
                            case "132":
                                pet_resistance_2_Skill = pet_resistance_2_Skill + float.Parse(skillPetValue);
                                break;
                            //火抗性
                            case "133":
                                pet_resistance_3_Skill = pet_resistance_3_Skill + float.Parse(skillPetValue);
                                break;
                            //水抗性
                            case "134":
                                pet_resistance_4_Skill = pet_resistance_4_Skill + float.Parse(skillPetValue);
                                break;
                            //电抗性
                            case "135":
                                pet_resistance_5_Skill = pet_resistance_5_Skill + float.Parse(skillPetValue);
                                break;

                            //技能抗性
                            case "141":
                                pet_SkillActDefPro_Skill = pet_SkillActDefPro_Skill + float.Parse(skillPetValue);
                                break;

                            //技能闪避
                            case "142":
                                pet_SkillActDodgePro_Skill = pet_SkillActDodgePro_Skill + float.Parse(skillPetValue);
                                break;

                            //怪物攻击伤害减免
                            case "151":
                                pet_MonsterActDameDefPro_Skill = pet_MonsterActDameDefPro_Skill + float.Parse(skillPetValue);
                                break;

                            //怪怪物技能伤害减免
                            case "152":
                                pet_MonsterSkillDameDefPro_Skill = pet_MonsterSkillDameDefPro_Skill + float.Parse(skillPetValue);
                                
                                break;

                        }
                    }
                }
            }
        }

        //获取Buff
        AI_Property ai_Proprety = petObj.GetComponent<AI_Property>();


        //宠物基础有20%首领技能和攻击抗性
        pet_MonsterSkillDameDefPro_Skill = pet_MonsterSkillDameDefPro_Skill + 0.4f;
        pet_MonsterActDameDefPro_Skill = pet_MonsterActDameDefPro_Skill + 0.2f;

        //神兽加成  技能伤害免疫
        ObscuredFloat shenshouAddPro = 0;
        string nowPetTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");
        if (nowPetTypeStr == "2") {
            shenshouAddPro = 0.2f;
            pet_MonsterSkillDameDefPro_Skill = 1;
            pet_MonsterActDameDefPro_Skill = 0.8f;
        }


        ObscuredFloat pro_1 = 1;

        //计算宠物各项属性
        //宠物血量
        pet_Hp = (int)((baseHp + pet_Hp_Skill) * (1 + pet_HpPro_Skill) + hp_Equip_valueSum);
        //Debug.Log("baseHp = " + baseHp + "pet_Hp_Skill = " + pet_Hp_Skill + "pet_HpPro_Skill = " + pet_HpPro_Skill);
        //宠物攻击
        pet_Act = (int)((base_Act + pet_Act_Skill + ai_Proprety.ActAdd) * (pro_1 + pet_ActPro_Skill + ai_Proprety.ActMul + shenshouAddPro) + act_EquipMax_valueSum);
        //宠物魔法攻击
        pet_MageAct = (int)((base_MageAct + pet_MageAct_Skill + ai_Proprety.MageActAdd) * (pro_1 + pet_MageActPro_Skill + ai_Proprety.MageActMul + shenshouAddPro) + magact_EquipMax_valueSum);
        //宠物物防
        pet_Def = (int)((base_Def + pet_Def_Skill + ai_Proprety.DefAdd) * (pro_1 + pet_DefPro_Skill + ai_Proprety.DefMul + shenshouAddPro) + def_EquipMax_valueSum);
        //宠物魔防
        pet_Adf = (int)((base_Adf + pet_Adf_Skill + ai_Proprety.AdfAdd) * (pro_1 + pet_AdfPro_Skill + ai_Proprety.AdfMul + shenshouAddPro) + adf_EquipMax_valueSum);
        //宠物暴击
        pet_Cri = base_Cri + pet_Cri_Skill + ai_Proprety.AI_Cri_Add + cir_Equip_valueSum;
        //宠物命中
        pet_Hit = base_Hit + pet_Hit_Skill + ai_Proprety.AI_Hit_Add + hit_Equip_valueSum;
        //宠物闪避
        pet_Dodge = base_Dodge + pet_Dodge_Skill + ai_Proprety.AI_Dodge_Add + dodge_Equip_valueSum;
        //宠物移动速度
        pet_MoveSpeed = base_MoveSpeed * (pro_1 + pet_MoveSpeed_Skill + ai_Proprety.MoveSpeedMul);
        //宠物攻击速度
        pet_ActSpeed = base_ActSpeed;
        //宠物技能抗性
        pet_SkillActDefPro =  pet_SkillActDefPro_Skill;   //先默认一半抗性看下
        //宠物技能闪避
        pet_SkillActDodgePro = pet_SkillActDodgePro_Skill;



        pet_MonsterActDameDefPro = pet_MonsterActDameDefPro_Skill;               //首领攻击免伤率
        pet_MonsterSkillDameDefPro = pet_MonsterSkillDameDefPro_Skill;           //首领技能免伤率

        //Debug.Log("pet_Cri = " + pet_Cri);
        pet_HuiXuePro = pet_HuiXuePro + pet_HuiXuePro_Skill + ai_Proprety.AI_HuiXuePro_Add;                                     //回血
        pet_XiXieValue = pet_XiXieValue + pet_XiXieValue_Skill + ai_Proprety.AI_XiXieValue_Add;           	                    //AI吸血值
        pet_XingYunPro = pet_XingYunPro + pet_XingYunPro_Skill + ai_Proprety.AI_XingYunPro_Add;				                    //幸运
        pet_DiXiaoPro = pet_DiXiaoPro + pet_DiXiaoPro_Skill + ai_Proprety.AI_DiXiaoPro_Add;				                        //概率抵消本次伤害
        pet_ChouHenValue = pet_ChouHenValue + pet_ChouHenValue_Skill + ai_Proprety.AI_ChouHenValue_Add;		                    //仇恨值
        pet_FuHuoPro = pet_FuHuoPro + pet_FuHuoPro_Skill + ai_Proprety.AI_FuHuoPro_Add;				                            //复活概率

        pet_resistance_1 = pet_resistance_1 + pet_resistance_1_Skill + ai_Proprety.AI_resistance_1_Add;                         //光抗性
        pet_resistance_2 = pet_resistance_2 + pet_resistance_2_Skill + ai_Proprety.AI_resistance_2_Add;                         //暗抗性
        pet_resistance_3 = pet_resistance_3 + pet_resistance_3_Skill + ai_Proprety.AI_resistance_3_Add;                         //火抗性
        pet_resistance_4 = pet_resistance_4 + pet_resistance_4_Skill + ai_Proprety.AI_resistance_4_Add;                         //水抗性
        pet_resistance_5 = pet_resistance_5 + pet_resistance_5_Skill + ai_Proprety.AI_resistance_5_Add;                         //电抗性
        pet_raceDamge_1 = pet_raceDamge_1 + pet_raceDamge_1_Skill + ai_Proprety.AI_raceDamge_1_Add;                             //野兽攻击伤害
        pet_raceDamge_2 = pet_raceDamge_2 + pet_raceDamge_2_Skill + ai_Proprety.AI_raceDamge_2_Add;                             //人物攻击伤害
        pet_raceDamge_3 = pet_raceDamge_3 + pet_raceDamge_3_Skill + ai_Proprety.AI_raceDamge_3_Add;                             //恶魔攻击伤害
        pet_raceResistance_1 = pet_raceResistance_1 + pet_raceResistance_1_Skill + ai_Proprety.AI_raceResistance_1_Add;         //野兽攻击抗性
        pet_raceResistance_2 = pet_raceResistance_2 + pet_raceResistance_2_Skill + ai_Proprety.AI_raceResistance_2_Add;         //人物攻击抗性
        pet_raceResistance_3 = pet_raceResistance_3 + pet_raceResistance_3_Skill + ai_Proprety.AI_raceResistance_3_Add;         //恶魔攻击抗性

        //更新宠物属性
        petObj.GetComponent<AI_Property>().AI_Lv = petLv;
        petObj.GetComponent<AI_Property>().AI_HpMax = pet_Hp;
        //Debug.Log("AI_HpMax1111111111111 = " + pet_Hp);
        petObj.GetComponent<AI_Property>().AI_Act = pet_Act;
        petObj.GetComponent<AI_Property>().AI_MageAct = pet_MageAct;
        petObj.GetComponent<AI_Property>().AI_Def = pet_Def;
        petObj.GetComponent<AI_Property>().AI_Adf = pet_Adf;
        petObj.GetComponent<AI_Property>().AI_Cri = pet_Cri;
        petObj.GetComponent<AI_Property>().AI_Hit = pet_Hit;
        petObj.GetComponent<AI_Property>().AI_Dodge = pet_Dodge;
        petObj.GetComponent<AI_Property>().AI_MoveSpeed = pet_MoveSpeed;
        petObj.GetComponent<AI_Property>().AI_ActSpeed = pet_ActSpeed;
        petObj.GetComponent<AI_Property>().AI_HuiXuePro = pet_HuiXuePro;
        petObj.GetComponent<AI_Property>().AI_XiXieValue = pet_XiXieValue;
        petObj.GetComponent<AI_Property>().AI_XingYunPro = pet_XingYunPro;
        petObj.GetComponent<AI_Property>().AI_DiXiaoPro = pet_DiXiaoPro;
        petObj.GetComponent<AI_Property>().AI_ChouHenValue = pet_ChouHenValue;
        petObj.GetComponent<AI_Property>().AI_FuHuoPro = pet_FuHuoPro;
        petObj.GetComponent<AI_Property>().AI_resistance_1 = pet_resistance_1;
        petObj.GetComponent<AI_Property>().AI_resistance_2 = pet_resistance_2;
        petObj.GetComponent<AI_Property>().AI_resistance_3 = pet_resistance_3;
        petObj.GetComponent<AI_Property>().AI_resistance_4 = pet_resistance_4;
        petObj.GetComponent<AI_Property>().AI_resistance_5 = pet_resistance_5;
        petObj.GetComponent<AI_Property>().AI_raceDamge_1 = pet_raceDamge_1;
        petObj.GetComponent<AI_Property>().AI_raceDamge_1 = pet_raceDamge_2;
        petObj.GetComponent<AI_Property>().AI_raceDamge_1 = pet_raceDamge_3;
        petObj.GetComponent<AI_Property>().AI_raceResistance_1 = pet_raceResistance_1;
        petObj.GetComponent<AI_Property>().AI_raceResistance_2 = pet_raceResistance_2;
        petObj.GetComponent<AI_Property>().AI_raceResistance_3 = pet_raceResistance_3;
        petObj.GetComponent<AI_Property>().AI_SkillActDefPro = pet_SkillActDefPro;
        petObj.GetComponent<AI_Property>().AI_SkillActDodgePro = pet_SkillActDodgePro;
        petObj.GetComponent<AI_Property>().AI_MonsterActDameDefPro = pet_MonsterActDameDefPro;
        petObj.GetComponent<AI_Property>().AI_MonsterSkillDameDefPro = pet_MonsterSkillDameDefPro;

        //Debug.Log("更新宠物属性成功 pet_Hp = " + pet_Hp + "AI_Hp = " + petObj.GetComponent<AI_Property>().AI_Hp);

        //此处后期需要修正 要不每次更新属性AI的血量会满血
        /*
        if (petObj.GetComponent<AI_Property>().AI_Hp == 0) {
            //petObj.GetComponent<AI_Property>().AI_Hp = pet_Hp;
            Debug.Log("petObj.GetComponent<AI_Property>().AI_Hp = " + petObj.GetComponent<AI_Property>().AI_Hp);
            int petNowHp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetNowHp", "ID", NowSclectPetID, "RosePet"));
            petObj.GetComponent<AI_Property>().AI_Hp = petNowHp;
        }
        */

        if (petType == "0"|| petType == "1")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetMaxHp", pet_Hp.ToString(), "ID", NowSclectPetID, "RosePet");

            //修正宠物召唤血量
            string petNowHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetNowHp", "ID", NowSclectPetID, "RosePet");
            if (petNowHp != "")
            {
                int petNowHp_Int = int.Parse(petNowHp);
                if (petNowHp_Int <= 0)
                {
                    petObj.GetComponent<AI_Property>().AI_Hp = pet_Hp;
                    //记录当前血量值
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetNowHp", pet_Hp.ToString(), "ID", NowSclectPetID, "RosePet");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
                }
                else
                {
                    int NowHp = int.Parse(petNowHp);
                    petObj.GetComponent<AI_Property>().AI_Hp = NowHp;
                }
            }
        }

        if (petObj.GetComponent<AI_Property>().AI_Hp == 0)
        {
            if (petType == "2" && petTianTiType == "1")
            {
                //修正宠物召唤血量
                string petNowHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetMaxHp", "ID", NowSclectPetID, "RosePet");
                if (petNowHp != "")
                {
                    int NowHp = int.Parse(petNowHp);
                    petObj.GetComponent<AI_Property>().AI_Hp = pet_Hp;
                }
            }
        }

        if (petObj.GetComponent<AI_Property>().AI_Hp == 0) {
            if (petType == "2" && petTianTiType == "2")
            {
                //修正宠物召唤血量
                string petNowHp = pet_Hp.ToString();
                if (petNowHp != "")
                {
                    int NowHp = int.Parse(petNowHp);
                    petObj.GetComponent<AI_Property>().AI_Hp = NowHp;
                }
            }
        }

        //召唤怪物的血量初始化为满的
        if (petType == "4"&& ifInit == true) {
            petObj.GetComponent<AI_Property>().AI_Hp = pet_Hp;
        }
    }

	//showType = 1 表示显示自己的宠物  2 表示显示排行榜
	public void Pet_UpdateShowProperty(GameObject showPetObj, string rosePet_ID,string showType = "1",string[] showPetData = null)
    {

		string petID = "";

        //更新宠物基本属性
		switch(showType){
		//显示自身宠物
		case "1":
			petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePet_ID, "RosePet");
			break;

		//显示排行榜宠物
		case "2":
			petID = showPetData[1];
			break;
		}
        
        int baseHp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Hp", "ID", petID, "Pet_Template"));
        int base_Act = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Act", "ID", petID, "Pet_Template"));
        int base_MageAct = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_MageAct", "ID", petID, "Pet_Template"));
        int base_Def = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Def", "ID", petID, "Pet_Template"));
        int base_Adf = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Adf", "ID", petID, "Pet_Template"));
        int base_ActSpeed = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_ActSpeed", "ID", petID, "Pet_Template"));
        float base_Cri = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Cri", "ID", petID, "Pet_Template"));
        float base_Hit = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Hit", "ID", petID, "Pet_Template"));
        float base_Dodge = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_Dodge", "ID", petID, "Pet_Template"));
        float base_MoveSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Base_MoveSpeed", "ID", petID, "Pet_Template"));


        int hp_Equip_valueSum = 0;
        int act_EquipMax_valueSum = 0;
        int magact_EquipMax_valueSum = 0;
        int def_EquipMax_valueSum = 0;
        int adf_EquipMax_valueSum = 0;

        float cir_Equip_valueSum = 0;
        float hit_Equip_valueSum = 0;
        float dodge_Equip_valueSum = 0;

        //宠物需要读取装备属性
        //宠物装备
        string[] equipTianTiIDList = new string[4];
        string[] equipHideTianTiIDList = new string[4];

        if (showType == "2")
        {
            if (showPetData.Length >= 20)
            {
                equipTianTiIDList[0] = showPetData[19];
                equipHideTianTiIDList[0] = showPetData[20];
                equipTianTiIDList[1] = showPetData[21];
                equipHideTianTiIDList[1] = showPetData[22];
                equipTianTiIDList[2] = showPetData[23];
                equipHideTianTiIDList[2] = showPetData[24];
            }
            if (showPetData.Length >= 27)
            {
                equipTianTiIDList[3] = showPetData[25];
                equipHideTianTiIDList[3] = showPetData[26];
            }
        }

        //获取装备
        for (int y = 1; y <= 4; y++)
        { 
            Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;
            string nowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + y, "ID", rosePet_ID, "RosePet");
            if (showType == "2")
            {
                nowItemID = equipTianTiIDList[y - 1];
            }

            string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", nowItemID, "Item_Template");
            if (equipID != "" && equipID != "0" && equipID != "-1")
            {
                int hp_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipID, "Equip_Template"));
                int act_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipID, "Equip_Template"));
                int act_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipID, "Equip_Template"));
                int magact_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equipID, "Equip_Template"));
                int magact_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equipID, "Equip_Template"));
                int def_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipID, "Equip_Template"));
                int def_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipID, "Equip_Template"));
                int adf_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipID, "Equip_Template"));
                int adf_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipID, "Equip_Template"));
                float cir_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipID, "Equip_Template"));
                float hit_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipID, "Equip_Template"));
                float dodge_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipID, "Equip_Template"));
                float damgeAdd_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipID, "Equip_Template"));
                float damgeSub_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipID, "Equip_Template"));
                float speed_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipID, "Equip_Template"));
                int lucky_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipID, "Equip_Template"));

                //累加属性
                hp_Equip_valueSum = hp_Equip_valueSum + hp_Equip_value;
                act_EquipMax_valueSum = act_EquipMax_valueSum + act_EquipMax_value;
                magact_EquipMax_valueSum = magact_EquipMax_valueSum + magact_EquipMax_value;
                def_EquipMax_valueSum = def_EquipMax_valueSum + def_EquipMax_value;
                adf_EquipMax_valueSum = adf_EquipMax_valueSum + adf_EquipMax_value;

                cir_Equip_valueSum = cir_Equip_valueSum + cir_Equip_value;
                hit_Equip_valueSum = hit_Equip_valueSum + base_Hit;
                dodge_Equip_valueSum = dodge_Equip_valueSum + dodge_Equip_value;

            }
        }



        /*
        //宠物装备
        string[] equipTianTiIDList = new string[3];
        string[] equipHideTianTiIDList = new string[3];

        if (showType == "2")
        {

            if (showPetData.Length >= 20)
            {
                equipTianTiIDList[0] = showPetData[19];
                equipHideTianTiIDList[0] = showPetData[20];
                equipTianTiIDList[1] = showPetData[21];
                equipHideTianTiIDList[1] = showPetData[22];
                equipTianTiIDList[2] = showPetData[23];
                equipHideTianTiIDList[2] = showPetData[24];
            }
            

            //string equipHideStrID_1 = showPetData[20];
            //string equipHideStrID_2 = showPetData[22];
            //string equipHideStrID_3 = showPetData[24];

            //获取装备
            for (int y = 1; y <= 3; y++)
            {

                Function_DataSet functionDataSet = Game_PublicClassVar.Get_function_DataSet;

                string nowItemID = equipTianTiIDList[y-1];
                string equipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemEquipID", "ID", nowItemID, "Item_Template");
                if (equipID != "" && equipID != "0" && equipID != "-1")
                {
                    int hp_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Hp", "ID", equipID, "Equip_Template"));
                    int act_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAct", "ID", equipID, "Equip_Template"));
                    int act_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAct", "ID", equipID, "Equip_Template"));
                    int magact_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinMagAct", "ID", equipID, "Equip_Template"));
                    int magact_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxMagAct", "ID", equipID, "Equip_Template"));
                    int def_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinDef", "ID", equipID, "Equip_Template"));
                    int def_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxDef", "ID", equipID, "Equip_Template"));
                    int adf_EquipMin_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MinAdf", "ID", equipID, "Equip_Template"));
                    int adf_EquipMax_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_MaxAdf", "ID", equipID, "Equip_Template"));
                    float cir_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Cri", "ID", equipID, "Equip_Template"));
                    float hit_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Hit", "ID", equipID, "Equip_Template"));
                    float dodge_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Dodge", "ID", equipID, "Equip_Template"));
                    float damgeAdd_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeAdd", "ID", equipID, "Equip_Template"));
                    float damgeSub_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_DamgeSub", "ID", equipID, "Equip_Template"));
                    float speed_Equip_value = float.Parse(functionDataSet.DataSet_ReadData("Equip_Speed", "ID", equipID, "Equip_Template"));
                    int lucky_Equip_value = int.Parse(functionDataSet.DataSet_ReadData("Equip_Lucky", "ID", equipID, "Equip_Template"));

                    //累加属性
                    hp_Equip_valueSum = hp_Equip_valueSum + hp_Equip_value;
                    act_EquipMax_valueSum = act_EquipMax_valueSum + act_EquipMax_value;
                    magact_EquipMax_valueSum = magact_EquipMax_valueSum + magact_EquipMax_value;
                    def_EquipMax_valueSum = def_EquipMax_valueSum + def_EquipMax_value;
                    adf_EquipMax_valueSum = adf_EquipMax_valueSum + adf_EquipMax_value;

                    cir_Equip_valueSum = cir_Equip_valueSum + cir_Equip_value;
                    hit_Equip_valueSum = hit_Equip_valueSum + base_Hit;
                    dodge_Equip_valueSum = dodge_Equip_valueSum + dodge_Equip_value;

                }
            }
       
        }
         */

        //更新宠物资质
        float zizhiNow_Hp = 0;
		float zizhiNow_Act = 0;
		float zizhiNow_MageAct = 0;
        float zizhiNow_Def = 0;
        float zizhiNow_Adf = 0;
        float zizhiNow_ActSpeed = 0;
        float zizhiNow_ChengZhang = 0;

        int petLv = 0;


		//更新宠物基本属性
		switch(showType){
		//显示自身宠物
		case "1":

			zizhiNow_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", rosePet_ID, "RosePet"));
			zizhiNow_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", rosePet_ID, "RosePet"));
			zizhiNow_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", rosePet_ID, "RosePet"));
			zizhiNow_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", rosePet_ID, "RosePet"));
			zizhiNow_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", rosePet_ID, "RosePet"));
			zizhiNow_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", rosePet_ID, "RosePet"));
			zizhiNow_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", rosePet_ID, "RosePet"));
			petLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePet_ID, "RosePet"));

			break;

		//显示排行榜宠物
		case "2":
			zizhiNow_Hp = float.Parse(showPetData[11]);
			zizhiNow_Act = float.Parse(showPetData[12]);
			zizhiNow_MageAct = float.Parse(showPetData[13]);
			zizhiNow_Def = float.Parse(showPetData[14]);
			zizhiNow_Adf = float.Parse(showPetData[15]);
			zizhiNow_ActSpeed = float.Parse(showPetData[16]);
			zizhiNow_ChengZhang = float.Parse(showPetData[17]);
			petLv = int.Parse(showPetData[2]);

			break;
		}

        //资质修正,做出1199和1200的数值差距
        zizhiNow_Hp = (int)(zizhiNow_Hp / 100) * 100 + (zizhiNow_Hp - (int)(zizhiNow_Hp / 100) * 100) / 1.5f;
        zizhiNow_Act = (int)(zizhiNow_Act / 100) * 100 + (zizhiNow_Act - (int)(zizhiNow_Act / 100) * 100) / 1.5f;
        zizhiNow_MageAct = (int)(zizhiNow_MageAct / 100) * 100 + (zizhiNow_MageAct - (int)(zizhiNow_MageAct / 100) * 100) / 1.5f;
        zizhiNow_Def = (int)(zizhiNow_Def / 100) * 100 + (zizhiNow_Def - (int)(zizhiNow_Def / 100) * 100) / 1.5f;
        zizhiNow_Adf = (int)(zizhiNow_Adf / 100) * 100 + (zizhiNow_Adf - (int)(zizhiNow_Adf / 100) * 100) / 1.5f;
        zizhiNow_ActSpeed = (int)(zizhiNow_ActSpeed / 100) * 100 + (zizhiNow_ActSpeed - (int)(zizhiNow_ActSpeed / 100) * 100) / 1.5f;

        //资质计算
        float zizhiPro_Hp = zizhiNow_Hp / 6000;
        float zizhiPro_Act = zizhiNow_Act / 1500;
        float zizhiPro_MageAct = zizhiNow_MageAct / 1500;
        float zizhiPro_Def = zizhiNow_Def / 1500;
        float zizhiPro_Adf = zizhiNow_Adf / 1500;
        float zizhiPro_ActSpeed = zizhiNow_ActSpeed / 3000;
        float zizhiPro_ChengZhang = zizhiNow_ChengZhang;

        float lv_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Hp", "ID", petID, "Pet_Template"));
        float lv_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Act", "ID", petID, "Pet_Template"));
        float lv_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_MageAct", "ID", petID, "Pet_Template"));
        float lv_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Def", "ID", petID, "Pet_Template"));
        float lv_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv_Adf", "ID", petID, "Pet_Template"));


        //加点属性
		string addPropretyValue = "";
		switch (showType) {

			case "1":
				addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID", rosePet_ID, "RosePet");
			break;

			case "2":
				addPropretyValue = showPetData[9];
			break;

		}
			
        int NowPropertyValue_LiLiang = 0;
        int NowPropertyValue_ZhiLi = 0;
        int NowPropertyValue_TiZhi = 0;
        int NowPropertyValue_NaiLi = 0;
        if (addPropretyValue != "")
        {
            string[] addPropertyList = addPropretyValue.Split(';');
            NowPropertyValue_LiLiang = int.Parse(addPropertyList[0]);
            NowPropertyValue_ZhiLi = int.Parse(addPropertyList[1]);
            NowPropertyValue_TiZhi = int.Parse(addPropertyList[2]);
            NowPropertyValue_NaiLi = int.Parse(addPropertyList[3]);
        }

        //属性
        base_Act = base_Act + (int)(NowPropertyValue_LiLiang * GetPetPropertyChengZhang("1"));
        base_MageAct = base_MageAct + (int)(NowPropertyValue_ZhiLi * GetPetPropertyChengZhang("2"));
        baseHp = baseHp * 5 + (int)(NowPropertyValue_TiZhi * GetPetPropertyChengZhang("3"));
        //baseHp = baseHp + NowPropertyValue_TiZhi * 15;
        base_Def = base_Def + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("4"));
        base_Adf = base_Adf + (int)(NowPropertyValue_NaiLi * GetPetPropertyChengZhang("5"));

        //属性
        /*
        baseHp = baseHp + (int)((lv_Hp * petLv) * zizhiPro_Hp * zizhiPro_ChengZhang);
        base_Act = base_Act + (int)((lv_Act * petLv) * zizhiPro_Act * zizhiPro_ChengZhang);
        base_MageAct = base_MageAct + (int)((lv_MageAct * petLv) * zizhiPro_MageAct * zizhiPro_ChengZhang);
        base_Def = base_Def + (int)((lv_Def * petLv) * zizhiPro_Def * zizhiPro_ChengZhang);
        base_Adf = base_Adf + (int)((lv_Adf * petLv) * zizhiPro_Adf * zizhiPro_ChengZhang);
        base_ActSpeed = (int)(base_ActSpeed / zizhiPro_ActSpeed);
        */

        //属性
        baseHp = (int)((baseHp + lv_Hp * petLv) * zizhiPro_Hp * zizhiPro_ChengZhang);
        base_Act = (int)((base_Act + lv_Act * petLv) * zizhiPro_Act * zizhiPro_ChengZhang);
        base_MageAct = (int)((base_MageAct + lv_MageAct * petLv) * zizhiPro_MageAct * zizhiPro_ChengZhang);
        base_Def = (int)((base_Def + lv_Def * petLv) * zizhiPro_Def * zizhiPro_ChengZhang);
        base_Adf = (int)((base_Adf + lv_Adf * petLv) * zizhiPro_Adf * zizhiPro_ChengZhang);
        base_ActSpeed = (int)(base_ActSpeed / zizhiPro_ActSpeed);


        //Debug.Log("baseHp = " + baseHp + "lv_Hp = " + lv_Hp + "petLv = " + petLv + "zizhiPro_Hp = " + zizhiPro_Hp + "zizhiPro_ChengZhang = " + zizhiPro_ChengZhang);

        int pet_Hp_Skill = 0;
        float pet_HpPro_Skill = 0.0f;
        int pet_Act_Skill = 0;
        float pet_ActPro_Skill = 0.0f;
        int pet_MageAct_Skill = 0;
        float pet_MageActPro_Skill = 0.0f;
        int pet_Def_Skill = 0;
        float pet_DefPro_Skill = 0.0f;
        int pet_Adf_Skill = 0;
        float pet_AdfPro_Skill = 0.0f;

		//技能列表
        string petSkillListStr = "";
        
		switch (showType) {
		case "1":
			petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePet_ID, "RosePet");
			break;

		case "2":
			petSkillListStr = showPetData[18];
			break;
		}

        string[] petSkillList = petSkillListStr.Split(';');

        //宠物被动技能互斥技能ID
        string HuChiSkillIDStr = "";
        for (int i = 0; i <= petSkillList.Length - 1; i++)
        {
            string huchiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuChiID", "ID", petSkillList[i], "Skill_Template");
            if (huchiID != "" && huchiID != "0")
            {
                HuChiSkillIDStr = HuChiSkillIDStr + huchiID + ";";
            }
        }
        string[] HuChiSkillIDList = HuChiSkillIDStr.Split(';');

        //显示宠物技能
        for (int i = 0; i <= petSkillList.Length - 1; i++)
        {
            //Debug.Log("petSkillList = " + petSkillList[i]);
            //检测当前ID是不是互斥ID是则当前技能无效
            bool IfHuChiStatus = false;
            for (int z = 0; z <= HuChiSkillIDList.Length - 1; z++)
            {
                if (petSkillList[i] == HuChiSkillIDList[z])
                {
                    IfHuChiStatus = true;
                }
            }

            if (!IfHuChiStatus)
            {
                string nowPetSkillID = petSkillList[i];
                string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", nowPetSkillID, "Skill_Template");

                if (gameObjectParameter != "0" && gameObjectParameter != "")
                {
                    //获取技能参数
                    string[] skillPar = gameObjectParameter.Split(';');
                    for (int y = 0; y <= skillPar.Length - 1; y++)
                    {
                        if (skillPar[y].Split(',').Length >= 2)
                        {
                            string skillPetType = skillPar[y].Split(',')[0];
                            string skillPetValue = skillPar[y].Split(',')[1];

                            switch (skillPetType)
                            {
                                //11 攻击
                                case "11":
                                    pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                    break;

                                //12 攻击百分比
                                case "12":
                                    pet_ActPro_Skill = pet_ActPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //15 魔法攻击
                                case "15":
                                    pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                    break;

                                //16 魔法攻击百分比
                                case "16":
                                    pet_MageActPro_Skill = pet_MageActPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //21 物防
                                case "21":
                                    pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                    break;

                                //31 物防百分比
                                case "22":
                                    pet_DefPro_Skill = pet_DefPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //31 魔防
                                case "31":
                                    pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                    break;

                                //32 魔防百分比
                                case "32":
                                    pet_AdfPro_Skill = pet_AdfPro_Skill + float.Parse(skillPetValue);
                                    break;

                                //41 血量
                                case "41":
                                    pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                    break;

                                //42 血量百分比
                                case "42":
                                    pet_HpPro_Skill = pet_HpPro_Skill + float.Parse(skillPetValue);
                                    break;
                            }
                        }
                    }
                }
            }
        }


        string xiuLianStr = ";;;";
        string[] XiuLianLvList = xiuLianStr.Split(';');

        //自身
        if (showType == "1") { 
            xiuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        //目标
        if (showType == "2")
        {
            //通过传输字段来获取
            if (showPetObj.GetComponent<UI_PaiHangShowPet>() != null)
            {
                xiuLianStr = showPetObj.GetComponent<UI_PaiHangShowPet>().PetXiuLianStr;
            }
        }

        if (xiuLianStr != "" && xiuLianStr != "0" && xiuLianStr != null)
        {
            XiuLianLvList = xiuLianStr.Split(';');
        }

        //获取宠物装备
        string EquioHideStr = "";
        for (int z = 1; z <= 4; z++)
        {
            string equipHideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_" + z, "ID", rosePet_ID, "RosePet");

            if (showType == "1")
            {
                if (equipHideID != "" && equipHideID != "0" && equipHideID != null)
                {
                    string PrepeotyListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", equipHideID, "RoseEquipHideProperty");
                    if (PrepeotyListStr != "" && PrepeotyListStr != "0" && PrepeotyListStr != null)
                    {
                        EquioHideStr = EquioHideStr + PrepeotyListStr + ";";
                    }
                }
            }

            //天梯特殊处理
            if (showType == "2")
            {
                equipHideID = equipHideTianTiIDList[z - 1];

                //通过传输字段来获取
                if (showPetObj.GetComponent<UI_PaiHangShowPet>() != null) {
                    string PrepeotyListStr = showPetObj.GetComponent<UI_PaiHangShowPet>().GetHidePro(equipHideID);
                    if (PrepeotyListStr != "" && PrepeotyListStr != "0" && PrepeotyListStr != null)
                    {
                        EquioHideStr = EquioHideStr + PrepeotyListStr + ";";
                    }
                }
            }
        }


        for (int i = 0; i < XiuLianLvList.Length; i++)
        {
            string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPro", "ID", XiuLianLvList[i], "PetXiuLian_Template");

            if (EquioHideStr != "" && i==0)
            {
                gameObjectParameter = EquioHideStr + gameObjectParameter;
            }

            if (gameObjectParameter != "0" && gameObjectParameter != "")
            {
                //获取技能参数
                string[] skillPar = gameObjectParameter.Split(';');
                for (int y = 0; y <= skillPar.Length - 1; y++)
                {
                    if (skillPar[y].Split(',').Length >= 2)
                    {
                        string skillPetType = skillPar[y].Split(',')[0];
                        string skillPetValue = skillPar[y].Split(',')[1];

                        switch (skillPetType)
                        {
                            //血量上限
                            case "1":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;
                            //物理攻击最大值
                            case "2":
                                pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                break;
                            case "3":
                                //物理防御最大值
                                pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                break;
                            //魔法防御最大值
                            case "4":
                                pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                break;
                            //魔法攻击最大值
                            case "5":
                                pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                break;

                            //血量
                            case "10":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;

                            //11 攻击
                            case "11":
                                pet_Act_Skill = pet_Act_Skill + int.Parse(skillPetValue);
                                break;

                            //12 攻击百分比
                            case "12":
                                pet_ActPro_Skill = pet_ActPro_Skill + float.Parse(skillPetValue);
                                break;

                            //15 魔法攻击
                            case "15":
                                pet_MageAct_Skill = pet_MageAct_Skill + int.Parse(skillPetValue);
                                break;

                            //16 魔法攻击百分比
                            case "16":
                                pet_MageActPro_Skill = pet_MageActPro_Skill + float.Parse(skillPetValue);
                                break;

                            //21 物防
                            case "21":
                                pet_Def_Skill = pet_Def_Skill + int.Parse(skillPetValue);
                                break;

                            //31 物防百分比
                            case "22":
                                pet_DefPro_Skill = pet_DefPro_Skill + float.Parse(skillPetValue);
                                break;

                            //31 魔防
                            case "31":
                                pet_Adf_Skill = pet_Adf_Skill + int.Parse(skillPetValue);
                                break;

                            //32 魔防百分比
                            case "32":
                                pet_AdfPro_Skill = pet_AdfPro_Skill + float.Parse(skillPetValue);
                                break;

                            //41 血量
                            case "41":
                                pet_Hp_Skill = pet_Hp_Skill + int.Parse(skillPetValue);
                                break;

                            //42 血量百分比
                            case "42":
                                pet_HpPro_Skill = pet_HpPro_Skill + float.Parse(skillPetValue);
                                break;
                        }
                    }
                }
            }
        }

        //神兽加成
        ObscuredFloat shenshouAddPro = 0;
        string nowPetTypeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");
        if (nowPetTypeStr == "2")
        {
            shenshouAddPro = 0.2f;
        }

        ObscuredFloat pro_1 = 1;

        //宠物血量
        int pet_Hp = (int)((baseHp + pet_Hp_Skill) * (pro_1 + pet_HpPro_Skill) + hp_Equip_valueSum);
        //Debug.Log("baseHp = " + baseHp + "pet_Hp_Skill = " + pet_Hp_Skill + "pet_HpPro_Skill = " + pet_HpPro_Skill);
        //宠物攻击
        int pet_Act = (int)((base_Act + pet_Act_Skill) * (pro_1 + pet_ActPro_Skill + shenshouAddPro) + act_EquipMax_valueSum);
        //宠物魔法攻击
        int pet_MageAct = (int)((base_MageAct + pet_MageAct_Skill) * (pro_1 + pet_MageActPro_Skill + shenshouAddPro) + magact_EquipMax_valueSum);
        //宠物物防
        int pet_Def = (int)((base_Def + pet_Def_Skill) * (pro_1 + pet_DefPro_Skill + shenshouAddPro) + def_EquipMax_valueSum);
        //宠物魔防
        int pet_Adf = (int)((base_Adf + pet_Adf_Skill) * (pro_1 + pet_AdfPro_Skill + shenshouAddPro) + adf_EquipMax_valueSum);
        //宠物攻击速度
        int pet_ActSpeed = base_ActSpeed;

        //显示宠物属性
		switch(showType){
			//自身显示
			case "1":
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_Hp.GetComponent<Text>().text = pet_Hp.ToString();
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_Act.GetComponent<Text>().text = pet_Act.ToString();
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_MoveSpeed.GetComponent<Text>().text = pet_MageAct.ToString();
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_Def.GetComponent<Text>().text = pet_Def.ToString();
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_Adf.GetComponent<Text>().text = pet_Adf.ToString();
				showPetObj.GetComponent<UI_Pet>().Obj_PropertyValue_ActSpeed.GetComponent<Text>().text = pet_ActSpeed.ToString();
				break;
			//排行玩家显示
			case "2":
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_Hp.GetComponent<Text>().text = pet_Hp.ToString();
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_Act.GetComponent<Text>().text = pet_Act.ToString();
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_MoveSpeed.GetComponent<Text>().text = pet_MageAct.ToString();
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_Def.GetComponent<Text>().text = pet_Def.ToString();
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_Adf.GetComponent<Text>().text = pet_Adf.ToString();
				showPetObj.GetComponent<UI_PaiHangShowPet>().Obj_PropertyValue_ActSpeed.GetComponent<Text>().text = pet_ActSpeed.ToString();
				break;
		}
    }
    
    //宠物合成
    public bool Pet_HeCheng(string rosePetID_1,string rosePetID_2 ) {

        int petLv_1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePetID_1, "RosePet"));
        int petLv_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePetID_2, "RosePet"));

        //等级不足无法合成
        /*
        if (petLv_1 < 15 || petLv_2 < 15) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("合成宠物最低需要15级");
            return false;
        }
        */

        //判定一方是否没有宠物
        if(petLv_1 == 0 || petLv_2 == 0)
        {
            return false;
        }

        //判定是否出战
        string PetStatus_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", rosePetID_1, "RosePet");
        string PetStatus_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", rosePetID_2, "RosePet");
        if(PetStatus_1=="1"|| PetStatus_2 == "1")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_172");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("无法合成！合成宠物中存在出战宠物");
            return false;
        }

        //获取PetID
        string petID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePetID_1, "RosePet");
        string petID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePetID_2, "RosePet");

        //神兽无法合成
        string petType_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID_1, "Pet_Template");
        string petType_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID_2, "Pet_Template");
        if (petType_1 == "2" || petType_2 == "2") {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_173");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("神兽无法合成！");
            return false;
        }

        //其中一个是变异无法合成
        /*
        string petType_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID_1, "Pet_Template");
        string petType_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID_2, "Pet_Template");
        if (petType_1 != "0" || petType_2 != "0") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("变异或神兽无法进行宠物合成！");
            return false;
        }
        */

        //获取宠物的各项数据
        //获取宠物1
        string petName_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePetID_1, "RosePet");

        //资质
        float zizhiNow_Hp_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_Act_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_MageAct_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_Def_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_Adf_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_ActSpeed_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", rosePetID_1, "RosePet"));
        float zizhiNow_ChengZhang_1 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", rosePetID_1, "RosePet"));

        //技能
        string petSkillListStr_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePetID_1, "RosePet");
        string[] petSkillList_1 = petSkillListStr_1.Split(';');

        //获取宠物2
        string petName_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePetID_2, "RosePet");

        //资质
        float zizhiNow_Hp_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_Act_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_MageAct_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_Def_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_Adf_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_ActSpeed_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", rosePetID_2, "RosePet"));
        float zizhiNow_ChengZhang_2 = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", rosePetID_2, "RosePet"));

        //技能
        string petSkillListStr_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePetID_2, "RosePet");
        string[] petSkillList_2 = petSkillListStr_2.Split(';');

        string petID = "0";

        int fightLv_1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID_1, "Pet_Template"));
        int fightLv_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID_2, "Pet_Template"));
        if (fightLv_1 >= fightLv_2)
        {
            petID = petID_1;
        }
        else
        {
            petID = petID_2;
        }

        //合成形象,30%概率合成出战等级小的形象
        if (Random.value <= 0.25f) {
            if (fightLv_1 >= fightLv_2)
            {
                petID = petID_2;
            }
            else
            {
                petID = petID_1;
            }
        }

        //合成资质
        int zizhiNow_Hp = (int)Pet_HeCheng_ZiZhi(zizhiNow_Hp_1, zizhiNow_Hp_2,6500);
        int zizhiNow_Act = (int)Pet_HeCheng_ZiZhi(zizhiNow_Act_1, zizhiNow_Act_2,1600);
        int zizhiNow_MageAct = (int)Pet_HeCheng_ZiZhi(zizhiNow_MageAct_1, zizhiNow_MageAct_2,1800);
        int zizhiNow_Def = (int)Pet_HeCheng_ZiZhi(zizhiNow_Def_1, zizhiNow_Def_2,1600);
        int zizhiNow_Adf = (int)Pet_HeCheng_ZiZhi(zizhiNow_Adf_1, zizhiNow_Adf_2,1600);
        int zizhiNow_ActSpeed = (int)Pet_HeCheng_ZiZhi(zizhiNow_ActSpeed_1, zizhiNow_ActSpeed_2,3000);
        float zizhiNow_ChengZhang = Pet_HeCheng_ZiZhi(zizhiNow_ChengZhang_1, zizhiNow_ChengZhang_2,1.3f);

        //目前攻速不做调整,强制为3000
        zizhiNow_ActSpeed = 3000;

        //合成技能
        //删除重复的技能
        int skillNum = 0;
        string petSkilllListStr = petSkillListStr_1;
        if (petSkillList_2.Length > 0) {
            for (int i = 0; i <= petSkillList_2.Length - 1; i++)
            {
                bool ifSaveSkillID = true;
                for (int y = 0; y <= petSkillList_1.Length - 1; y++)
                {
                    if (petSkillList_1[y] == petSkillList_2[i])
                    {
                        ifSaveSkillID = false;
                    }
                }
                if (!ifSaveSkillID)
                {
                    ifSaveSkillID = true;
                    petSkillList_2[i] = "0";
                }
                else
                {
                    skillNum = skillNum + 1;
                    if (petSkilllListStr != "")
                    {
                        petSkilllListStr = petSkilllListStr + ";" + petSkillList_2[i];
                    }
                    else
                    {
                        petSkilllListStr = petSkillList_2[i];
                    }
                }
            }
        }

        //Debug.Log("petSkilllListStr = " + petSkilllListStr);
        string[] petSkilllList = petSkilllListStr.Split(';');

        //设定每个技能的留下的概率
        string savePetSkillID = "";
        for (int i = 0; i <= petSkilllList.Length - 1; i++)
        { 
            //暂定每个技能留下的概率为35%
            if (Random.value <= 0.5f) {
                if (savePetSkillID != "")
                {
                    savePetSkillID = savePetSkillID + ";" + petSkilllList[i];
                }
                else {
                    savePetSkillID = petSkilllList[i];
                }
            }
        }

        //填补必带技能
        string[] savePetSkillList = savePetSkillID.Split(';');
        string baseSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BaseSkillID", "ID", petID, "Pet_Template");
        if (baseSkillID != "" && baseSkillID != "0")
        {
            string[] baseSkillList = baseSkillID.Split(';');
            for (int i = 0; i < baseSkillList.Length; i++)
            {
                bool ifAddStatus = true;
                for (int y = 0; y < savePetSkillList.Length; y++)
                {
                    if (savePetSkillList[y] == baseSkillList[i])
                    {
                        ifAddStatus = false;
                        break;
                    }
                }
                //添加必带技能
                if (ifAddStatus) {
                    if (savePetSkillID != "")
                    {
                        savePetSkillID = savePetSkillID + ";" + baseSkillList[i];
                    }
                    else
                    {
                        savePetSkillID = baseSkillList[i];
                    }
                }
            }
        }

        //Debug.Log("合成成功！zizhiNow_Hp_1 = " + zizhiNow_Hp_1 + "zizhiNow_Hp_2 = " + zizhiNow_Hp_2 + "zizhiNow_Hp = " + zizhiNow_Hp);

        //合成等级
        int pet_Lv = 1;
        int pet_exp = 0;
        int addPropertyNum = pet_Lv * 5 + 20;
        string addPropertyValue = "0;0;0;0";

        
        //获取目标是否为宝宝,如果两个都为宝宝则本次必定变成宝宝
        string baby = "0";
        string baby_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", rosePetID_1, "RosePet");
        string baby_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", rosePetID_2, "RosePet");
        if (baby_1 == "1" && baby_2 == "1")
        {
            baby = "1";

            //合成等级
            pet_Lv = (int)(Mathf.Min(petLv_1, petLv_2) * 0.75f + (Mathf.Max(petLv_1, petLv_2) - Mathf.Min(petLv_1, petLv_2)) * (Random.value));
            pet_exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "RoseLv", pet_Lv.ToString(), "RoseExp_Template"));
            pet_exp = (int)(pet_exp * Random.value);
            if (pet_Lv < 1)
            {
                pet_Lv = 1;
            }
            //随机点数
            addPropertyNum = (pet_Lv - 1) * 5 + 20;
            //addPropertyValue = "15;15;15;15";
            int writeProValue = 15 + (pet_Lv - 1) * 1;
            addPropertyValue = writeProValue + ";" + writeProValue + ";" + writeProValue + ";" + writeProValue;
        }
        else {

            //每次合野生有5%概率让起变成宝宝
            if (Random.value <= 0.05f)
            {
                baby_1 = "1";
                pet_Lv = 1;
                pet_exp = 0;
                addPropertyNum = (pet_Lv - 1) * 5 + 20;
                int writeProValue = 15 + (pet_Lv - 1) * 1;
                addPropertyValue = writeProValue + ";"+ writeProValue + ";"+ writeProValue + ";" + writeProValue;
                //addPropertyValue = "15;15;15;15";
            }
            else {

                //每次合成有5%概率变成软泥怪
                if (Random.value <= 0.05f)
                {

                    baby_1 = "1";
                    pet_Lv = 1;
                    pet_exp = 0;
                    addPropertyNum = (pet_Lv - 1) * 5 + 20;
                    addPropertyValue = "0;0;0;0";
                    petID = "10001020";
                    savePetSkillID = "";

                    //读取必带技能
                    baseSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BaseSkillID", "ID", petID, "Pet_Template");
                    if (baseSkillID != "" && baseSkillID != "0")
                    {
                        string[] baseSkillList = baseSkillID.Split(';');
                        for (int i = 0; i < baseSkillList.Length; i++)
                        {
                            savePetSkillID = savePetSkillID + baseSkillList[i] + ";";
                        }
                    }

                    //读取随机技能
                    string randomSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RandomSkillID", "ID", petID, "Pet_Template");
                    if (randomSkillID != "" && randomSkillID != "0")
                    {
                        string[] randomSkillList = randomSkillID.Split(';');
                        for (int i = 0; i < randomSkillList.Length; i++)
                        {
                            float skillPro = float.Parse(randomSkillList[i].Split(',')[1]);
                            string skillID = randomSkillList[i].Split(',')[0];
                            if (Random.value <= skillPro && skillID!="" && skillID != null && skillID != "0")
                            {
                                savePetSkillID = savePetSkillID + skillID + ";";
                            }
                        }
                    }

                    if (savePetSkillID != "")
                    {
                        savePetSkillID = savePetSkillID.Substring(0, savePetSkillID.Length - 1);
                    }

                    //写入成就(大海龟)
                    Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("211", "0", "1");
                }
                else {
                    //普通合成等级
                    pet_Lv = (int)(Mathf.Min(petLv_1, petLv_2) * 0.75f + (Mathf.Max(petLv_1, petLv_2) - Mathf.Min(petLv_1, petLv_2)) * (Random.value));
                    pet_exp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "RoseLv", pet_Lv.ToString(), "RoseExp_Template"));
                    pet_exp = (int)(pet_exp * Random.value);
                    if (pet_Lv < 1)
                    {
                        pet_Lv = 1;
                    }
                    //随机点数
                    int AddProretyNum = (pet_Lv - 1) * 5 + 20 + (pet_Lv-1) * 4;
                    //如果是宝宝属性点会上升
                    if (baby == "1")
                    {
                        AddProretyNum = 60;
                        //addPropertyNum = 0;
                        addPropertyNum = (pet_Lv - 1) * 5 + (pet_Lv - 1) * 4;
                    }
                    else {
                        addPropertyNum = 0;
                    }

                    addPropertyValue = PetAddPropertyFenPei(AddProretyNum);

                }
            }
        }


        //重新写入宠物的数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetID", petID, "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", pet_Lv.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Exp", pet_exp.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("baby", baby, "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", addPropertyNum.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", addPropertyValue, "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetPingFen", "0", "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Hp", zizhiNow_Hp.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Act", zizhiNow_Act.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_MageAct", zizhiNow_MageAct.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Def", zizhiNow_Def.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Adf", zizhiNow_Adf.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ActSpeed", zizhiNow_ActSpeed.ToString(), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", zizhiNow_ChengZhang.ToString("f2"), "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", savePetSkillID, "ID", rosePetID_1, "RosePet");
        //写入名称
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", petName, "ID", rosePetID_1, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //删除怪物2的数据
        Pet_ClearnData(rosePetID_2);

        //写入成就(合成数量)
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("208", "0", "1");


        //写入成就(合成技能)
        int nowSkillNum = savePetSkillID.Split(';').Length;
        Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("209", "0", nowSkillNum.ToString());

        //写入活跃任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "132", "1");

        //广播
        if (nowSkillNum >= 6) {
            //获取玩家名称
            string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001019, "恭喜玩家" + roseName + "合成宠物时人品爆裂!获得了一个"+ nowSkillNum + "技能的宠物！");
            Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
            comStr_4.str_1 = "7";
            comStr_4.str_2 = nowSkillNum.ToString();
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(100010191, comStr_4);
        }

        return true;

    }


    //随机分配指定点数
    public string PetAddPropertyFenPei(int sumNum) {

        //取4个随机值
        float ran_1 = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 0.5f);
        float ran_2 = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 0.5f);
        float ran_3 = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Float(0, 1 - ran_1 - ran_2);
        float ran_4 = 1 - ran_1 - ran_2 - ran_3;
        int add_1 = (int)(sumNum * ran_1);
        int add_2 = (int)(sumNum * ran_2);
        int add_3 = (int)(sumNum * ran_3);
        int add_4 = (int)(sumNum * ran_4);

        return add_1 + ";" + add_2 + ";" + add_3 + ";" + add_4;

    }

    //各个属性分配指定点数
    public void PetAddPropertyNum(string rosePetID, int sumNum)
    {
        string addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID", rosePetID, "RosePet");
        string[] addProList = addPropretyValue.Split(';');

        if (addProList.Length >= 4) {
            int add_1 = int.Parse(addProList[0]) + sumNum;
            int add_2 = int.Parse(addProList[1]) + sumNum;
            int add_3 = int.Parse(addProList[2]) + sumNum;
            int add_4 = int.Parse(addProList[3]) + sumNum;

            string writeStr = add_1 + ";" + add_2 + ";" + add_3 + ";" + add_4;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", writeStr, "ID", rosePetID, "RosePet");
        }
    }

    //传入2个宠物资质返回合成后的资质（取2个值的差值进行调整）
    //ziZhiType表示资质的类型,0表示不额外处理,1表示额外处理满级资质,用于捕捉宠物
    public float Pet_HeCheng_ZiZhi(float zizhiValue_1, float zizhiValue_2, float maxZiZhi = 99999,string ziZhiType = "0") {

        ObscuredFloat zizhi_1 = 0.04f;
        ObscuredFloat zizhi_2 = 0.75f;
        ObscuredFloat zizhi_3 = 0.25f;
        ObscuredFloat zizhi_4 = 1.1f;

        if (ziZhiType == "1") {

            //5%概率满资质
            if (Random.value <= zizhi_1)
            {
                return Mathf.Max(zizhiValue_1, zizhiValue_2);
            }
        }

        //获取随机资质
        //float zhizhiValue = Mathf.Min(zizhiValue_1, zizhiValue_2) * 0.75f + (Mathf.Min(zizhiValue_1, zizhiValue_2) * 0.25f + Mathf.Max(zizhiValue_1, zizhiValue_2) - Mathf.Min(zizhiValue_1, zizhiValue_2)) * Random.value * 1.1f;
        float zhizhiValue = Mathf.Min(zizhiValue_1, zizhiValue_2) * zizhi_2 + ((Mathf.Min(zizhiValue_1, zizhiValue_2) * zizhi_3 + Mathf.Max(zizhiValue_1, zizhiValue_2) - Mathf.Min(zizhiValue_1, zizhiValue_2))) * Random.value * zizhi_4;
        //限制最高资质
        if (zhizhiValue > maxZiZhi) {
            zhizhiValue = maxZiZhi;
        }

        return zhizhiValue;
    }


    //宠物添加技能
    public bool Pet_AddSkill(string rosePetID, string addSkillID)
    {

        //技能
        string petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePetID, "RosePet");
        string[] petSkillList = petSkillListStr.Split(';');

        //合成技能
        bool ifTiHuanSkill = true;
        string savePetSkillID = "";

        //如果当前宠物技能少于4个,则有概率直接添加技能(概率算法为0.5/当前拥有的技能数量)
        if (petSkillList.Length < 4 && petSkillList.Length >= 1)
        {
            if (Random.value <= 0.5/petSkillList.Length) {
                ifTiHuanSkill = false;
                savePetSkillID = petSkillListStr + ";" + addSkillID;
            }
        }

        //如果当前宠物没有技能则直接成功
        if (petSkillList[0] == "" || petSkillList[0] == "0")
        {
            if (petSkillList.Length <= 1)
            {
                ifTiHuanSkill = false;
                savePetSkillID = addSkillID;
            }
        }


        //判断当前自身的技能是否已经拥有
        for(int i = 0 ; i< petSkillList.Length ;i++){
            if (petSkillList[i] == addSkillID) {
                return false;
            }
        }

        if (ifTiHuanSkill)
        {
            //随机获取替换的技能ID序号
            int tihuanNum = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, petSkillList.Length);

            //设定每个技能的留下的概率
            for (int i = 0; i < petSkillList.Length; i++)
            {
                string addSkillStr = petSkillList[i];
                //随机获取替换的技能
                if (i == tihuanNum-1)
                {
                    addSkillStr = addSkillID;
                }

				if (savePetSkillID != ""&& savePetSkillID !="0")
                {
                    savePetSkillID = savePetSkillID + ";" + addSkillStr;
                }
                else
                {
                    savePetSkillID = addSkillStr;
                }
            }
        }

        //重新写入宠物的数据
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", savePetSkillID, "ID", rosePetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
        return true;
    }

    //升级当前出战的宠物
    public void Pet_AddExp(int addExp)
    {
        GameObject[] petObjList = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj;
        for (int i = 0; i < petObjList.Length; i++) {
            if (petObjList[i] != null) {
                Pet_AddExpOne(petObjList[i].GetComponent<AIPet>().RosePet_ID, addExp);
            }
        }
    }

    //添加宠物血量AI_Hp < AI_HpMax
    public void PetAddFightHp(GameObject addHpPetObj,int addHp) {
        if (addHpPetObj.GetComponent<AIPet>() != null) {
            AI_Property aI_Property = addHpPetObj.GetComponent<AI_Property>();
            aI_Property.AI_Hp = aI_Property.AI_Hp + addHp;
            //防止过量加血
            if (aI_Property.AI_Hp >= aI_Property.AI_HpMax)
            {
                aI_Property.AI_Hp = aI_Property.AI_HpMax;
            }
            addHpPetObj.GetComponent<AIPet>().HitStatus = true;
            bool ifPlayFly = true;
            //飘字
            if (ifPlayFly)
            {
                Game_PublicClassVar.Get_function_UI.Fight_FlyText("2", addHp.ToString(), "2", addHpPetObj, "", "");
            }
        }
    }

    //升级某个宠物
    public void Pet_AddExpOne(string rosePet_ID,int addExp,string expShowType = "0")
    {
        int nowPetLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePet_ID, "RosePet"));
        int nowPetExp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetExp", "ID", rosePet_ID, "RosePet"));
        //最高级70级
        if (nowPetLv >= 70) {
            return;
        }

        //比主角大于5级不能获得经验
        if (nowPetLv >= Game_PublicClassVar.Get_function_Rose.GetRoseLv() + 5) {
            return;
        }

        int expUpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "ID", nowPetLv.ToString(), "RoseExp_Template"));
        nowPetExp = nowPetExp + addExp;
        //升级
		if (nowPetExp >= expUpValue) {
			nowPetLv = nowPetLv + 1;
			nowPetExp = nowPetExp - expUpValue;
			//播放特表


			//播放升级特效
			GameObject effRoseUp = (GameObject)Instantiate (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().Eff_RoseUp);
			if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().RosePetObj [int.Parse (rosePet_ID) - 1] != null) {
				effRoseUp.transform.SetParent (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ().RosePetObj [int.Parse (rosePet_ID) - 1].transform);
				effRoseUp.transform.localPosition = new Vector3 (0, 0, 0);
				effRoseUp.transform.localScale = new Vector3 (1, 1, 1);
				Destroy (effRoseUp, 2);
			}

			//有几率领悟技能
			if (Random.value <= 0.005f) {
				Pet_LingWuSkill (rosePet_ID);
			}


            //更新主界面
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", nowPetLv.ToString(), "ID", rosePet_ID, "RosePet");
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetUpLv();
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetUpExp();

            try
            {
                GameObject chuZhanObj = Pet_ReturnChuZhanObj();
                if (chuZhanObj != null)
                {
                    GameObject nameObj = chuZhanObj.GetComponent<AIPet>().UI_Hp.transform.Find("Lal_Name").gameObject;
                    if (nameObj != null)
                    {
                        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePet_ID, "RosePet");
                        nameObj.GetComponent<Text>().text = "Lv." + nowPetLv + " " + petName;
                    }
                }
            }
            catch {
                Debug.Log("报错！！！找不到宠物名称的控件!!!");
            }

            //增加点数
            int nowDianShu = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", rosePet_ID, "RosePet"));
            nowDianShu = nowDianShu + 5;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", nowDianShu.ToString(), "ID", rosePet_ID, "RosePet");

            //各个点数 + 1
            PetAddPropertyNum(rosePet_ID, 1);

        } else {
			//更新主界面
			Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseDataSet.GetComponent<UI_RoseDataSet>().PetUpExp();
		}

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", nowPetLv.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetExp", nowPetExp.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //弹出获取提示
        UI_RoseGetItemHint hint = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseGetItemHint.GetComponent<UI_RoseGetItemHint>();
        hint.UpdataHintText = "宠物获得" + addExp + "点经验";
        hint.UpdataHint = true;

        //通用快捷提示框
        if (expShowType == "0")
        {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_327");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_328");
            Game_PublicClassVar.Get_function_UI.GameGirdHint(langStrHint_1 + addExp + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("宠物获得" + addExp + "点经验");
        }

    }

    public bool Pet_AddLvOne(string rosePet_ID, int addLv) {

        //比主角大于5级不能获得经验
        int nowPetLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePet_ID, "RosePet"));
        if (nowPetLv >= Game_PublicClassVar.Get_function_Rose.GetRoseLv() + 5)
        {
            return false;
        }

        //循环增加经验
        for (int i = 0; i < addLv;i++ )
        {
            int expUpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetUpExp", "ID", nowPetLv.ToString(), "RoseExp_Template"));
            Pet_AddExpOne(rosePet_ID, expUpValue);
        }
        return true;
    }
    
    //宠物领悟
    public void Pet_LingWuSkill(string rosePet_ID)
    {
        //读取随机技能
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePet_ID, "RosePet");
        string petSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePet_ID, "RosePet");
        string randomSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RandomSkillID", "ID", petID, "Pet_Template");

        if (randomSkillID != "" && randomSkillID != "0")
        {
            string[] randomSkillList = randomSkillID.Split(';');
            for (int i = 0; i < randomSkillList.Length; i++)
            {
                float skillPro = float.Parse(randomSkillList[i].Split(',')[1]);
                string skillID = randomSkillList[i].Split(',')[0];
                if (Random.value <= skillPro)
                {
                    bool petSkillStatus = Pet_IfSkillExist(rosePet_ID, skillID);
                    //当技能不存在时写入技能
                    if (petSkillStatus == false) {
                        if (petSkillStr != "")
                        {
                            petSkillStr = petSkillStr + ";" + skillID;
                        }
                        else
                        {
                            petSkillStr = skillID;
                        }
                    }
                }
            }
        }
    }

    //获取一个技能宠物是否具备
    public bool Pet_IfSkillExist(string rosePetID, string petSkillID) {

        string petSkillListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", rosePetID, "RosePet");
        if (petSkillListStr != "" && petSkillListStr != "0") {
            string[] petSkillList = petSkillListStr.Split(';');

            for (int i = 0; i < petSkillList.Length; i++) {
                //找到相同技能返回为True
                if (petSkillID == petSkillList[i]) {
                    return true;
                }
            }
        }
        //未找到相同技能
        return false;
    }

    //创建一个宠物数据
    public void Pet_Create(string rosePet_ID,string petID,string createType)
    {

        //更新宠物基本属性
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");
        float zizhiMax_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang_Max", "ID", petID, "Pet_Template"));

        float zizhiMin_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed_Min", "ID", petID, "Pet_Template"));
        float zizhiMin_ChengZhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang_Min", "ID", petID, "Pet_Template"));
        string petType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", petID, "Pet_Template");

        //合成资质
        int zizhiNow_Hp = (int)Pet_HeCheng_ZiZhi(zizhiMin_Hp, zizhiMax_Hp,6500,"1");
        int zizhiNow_Act = (int)Pet_HeCheng_ZiZhi(zizhiMin_Act, zizhiMax_Act, 1600, "1");
        int zizhiNow_MageAct = (int)Pet_HeCheng_ZiZhi(zizhiMin_MageAct, zizhiMax_MageAct, 3200, "1");
        int zizhiNow_Def = (int)Pet_HeCheng_ZiZhi(zizhiMin_Def, zizhiMax_Def, 1600, "1");
        int zizhiNow_Adf = (int)Pet_HeCheng_ZiZhi(zizhiMin_Adf, zizhiMax_Adf, 1600, "1");
        int zizhiNow_ActSpeed = (int)Pet_HeCheng_ZiZhi(zizhiMin_ActSpeed, zizhiMax_ActSpeed, 1600, "1");
        float zizhiNow_ChengZhang = Pet_HeCheng_ZiZhi(zizhiMin_ChengZhang, zizhiMax_ChengZhang, 1.3f);

        //神兽特殊处理
        if (petType == "2") {
            zizhiNow_Hp = (int)(zizhiMax_Hp);
            zizhiNow_Act = (int)(zizhiMax_Act);
            zizhiNow_MageAct = (int)(zizhiMax_MageAct);
            zizhiNow_Def = (int)(zizhiMax_Def);
            zizhiNow_Adf = (int)(zizhiMax_Adf);
            zizhiNow_ActSpeed = (int)(zizhiMax_ActSpeed);
            zizhiNow_ChengZhang = zizhiMax_ChengZhang;
        }

        string petSkillStr = "";
        //读取必带技能
        string baseSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BaseSkillID", "ID", petID, "Pet_Template");
        if (baseSkillID != "" && baseSkillID != "0") { 
            string[] baseSkillList = baseSkillID.Split(';');
            for (int i = 0; i < baseSkillList.Length; i++) {
                petSkillStr = petSkillStr + baseSkillList[i] + ";";
            }
        }

        //读取随机技能
        string randomSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RandomSkillID", "ID", petID, "Pet_Template");
        if (randomSkillID != "" && randomSkillID != "0")
        {
            string[] randomSkillList = randomSkillID.Split(';');
            for (int i = 0; i < randomSkillList.Length; i++)
            {
                float skillPro = float.Parse(randomSkillList[i].Split(',')[1]);
                string skillID = randomSkillList[i].Split(',')[0];
                if (Random.value <= skillPro) {
                    petSkillStr = petSkillStr + skillID + ";";
                }
            }
        }

        if (petSkillStr != "") {
            petSkillStr = petSkillStr.Substring(0, petSkillStr.Length - 1);
        }

        //查询名称
        /*
        string nowPetName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", rosePet_ID, "RosePet");
        if (nowPetName != "" && nowPetName != "0") {
            petName = nowPetName;
        }
        */

        //保留等级和经验
        int nowPetLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", rosePet_ID, "RosePet"));
        string nowPetExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetExp", "ID", rosePet_ID, "RosePet");
        int nowDianShu = 0;
        string addPropretyValue = "0";
        string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", rosePet_ID, "RosePet");

        //创建类型
        switch (createType) { 
            //普通宠物创建
            case "0":
                //随机额外加3级
                nowPetLv = nowPetLv + Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, 3);

                if (addPropretyValue == "0" || addPropretyValue == "")
                {
                    int AddProretyNum = nowPetLv * 5 + 20 + (nowPetLv-1) * 4;
                    addPropretyValue = PetAddPropertyFenPei(AddProretyNum);
                }
                ifBaby = "0";
                nowDianShu = 0;

                break;
            //宝宝创建
            case "1":
                //为1级时清空数据
                nowPetLv = 1;
                nowPetExp = "0";
                nowDianShu = 20;
                //随机点数
                int addProretyNum = nowPetLv * 5 + 60;
                addPropretyValue = PetAddPropertyFenPei(addProretyNum);
                ifBaby = "1";

                //写入成就(宝宝数量)
                Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("207", "0", "1");

                break;
                /*
            //捕捉创建（因为之前是否宝宝都已经写入进去了）
            case "2":
                addProretyNum = nowPetLv * 5 + 60;
                addPropretyValue = PetAddPropertyFenPei(addProretyNum);
                break;
                */
        }
        
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", petName, "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", nowPetLv.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetExp", nowPetExp, "ID", rosePet_ID, "RosePet");
        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetPingFen", "0", "ID", petID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IfBaby", ifBaby, "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", nowDianShu.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", addPropretyValue, "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetID", petID, "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Hp", zizhiNow_Hp.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Act", zizhiNow_Act.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_MageAct", zizhiNow_MageAct.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Def", zizhiNow_Def.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Adf", zizhiNow_Adf.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ActSpeed", zizhiNow_ActSpeed.ToString(), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", zizhiNow_ChengZhang.ToString("f2"), "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", petSkillStr, "ID", rosePet_ID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //更新当前任务
        Game_PublicClassVar.Get_function_Task.updataTaskItemID();

        //写入活跃任务
        if (petSkillStr.Split(';').Length >= 3) {
            Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "111", "1");
        }

    }

    //增加随机资质
    public bool Pet_AddRandomZiZhi(string rosePet_ID)
    {
        
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePet_ID, "RosePet");
        //当前资质
        int ziZhi_Hp = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", rosePet_ID, "RosePet"));
        int ziZhi_Act = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", rosePet_ID, "RosePet"));
        int ziZhi_MageAct = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", rosePet_ID, "RosePet"));
        int ziZhi_Def = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", rosePet_ID, "RosePet"));
        int ziZhi_Adf = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", rosePet_ID, "RosePet"));
        int ziZhi_ActSpeed = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", rosePet_ID, "RosePet"));

        //最大资质
        float zizhiMax_Hp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Act = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_MageAct = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Def = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_Adf = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf_Max", "ID", petID, "Pet_Template"));
        float zizhiMax_ActSpeed = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed_Max", "ID", petID, "Pet_Template"));

        //增加状态
        bool addStatus = false;
        //血量资质
        if (ziZhi_Hp < zizhiMax_Hp) {
            if (addRandomZiZhi(rosePet_ID, "ZiZhi_Hp", zizhiMax_Hp)) {
                addStatus = true;
            }
        }
        //攻击资质
		if (ziZhi_Act < zizhiMax_Act)
        {
			if (addRandomZiZhi (rosePet_ID, "ZiZhi_Act", zizhiMax_Act)) {
				addStatus = true;
			}
        }
        //法力资质
		if (ziZhi_MageAct < zizhiMax_MageAct)
        {
			if (addRandomZiZhi (rosePet_ID, "ZiZhi_MageAct", zizhiMax_MageAct)) {
				addStatus = true;
			}
        }
        //物防资质
		if (ziZhi_Def < zizhiMax_Def)
        {
			if (addRandomZiZhi (rosePet_ID, "ZiZhi_Def", zizhiMax_Def)) {
				addStatus = true;
			}
        }
        //魔防资质
		if (ziZhi_Adf < zizhiMax_Adf)
        {
			if (addRandomZiZhi (rosePet_ID, "ZiZhi_Adf", zizhiMax_Adf)) {
				addStatus = true;
			}
        }
        //速度资质
		if (ziZhi_ActSpeed < zizhiMax_ActSpeed)
        {
			if (addRandomZiZhi (rosePet_ID, "ZiZhi_ActSpeed", zizhiMax_ActSpeed)) {
				addStatus = true;
			}
        }

        if (!addStatus)
        {
            return false;
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
            return true;
        }
    }

    //随机增加宠物资质
    public bool addRandomZiZhi(string rosePet_ID, string addZiZhiProStr,float maxValue)
    {
        int addZiZhiValue = 10 + (int)(Random.value * 15);
        //血量资质翻倍
        if (addZiZhiProStr == "ZiZhi_Hp") {
            addZiZhiValue = addZiZhiValue * 2;
        }
        
		int ziZhi_value = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData(addZiZhiProStr, "ID", rosePet_ID, "RosePet"));
        addZiZhiValue = addZiZhiValue + ziZhi_value;

        //资质不能超过上限
        if (addZiZhiValue > maxValue) {
            addZiZhiValue = (int)(maxValue);
        }
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData(addZiZhiProStr, addZiZhiValue.ToString(), "ID", rosePet_ID, "RosePet");

        return true;
    }

    //增加随机成长
    public bool Pet_AddRandomChengZhang(string rosePet_ID)
    {
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", rosePet_ID, "RosePet");
        float chengzhang = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", rosePet_ID, "RosePet"));
        float ziZhi_ChengZhang_Max = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang_Max", "ID", petID, "Pet_Template"));

        if (chengzhang >= ziZhi_ChengZhang_Max)
        {
            return false;
        }
        else {
            float addValue = 0.02f + Random.value * 0.03f;
            //保底增加
            if (addValue <= 0.01f) {
                addValue = 0.01f;
            }
            chengzhang = chengzhang + addValue;
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", chengzhang.ToString("0.00").ToString(), "ID", rosePet_ID, "RosePet");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");
            return true;
        }
    }

    //清理怪物数据
    public void Pet_ClearnData(string PetID) {

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetID", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetStatus", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("Exp", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IfBaby", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyNum", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("AddPropretyValue", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetPingFen", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Hp", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Act", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Def", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_Adf", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ActSpeed", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZiZhi_ChengZhang", "0", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetSkill", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("LockStatus", "", "ID", PetID, "RosePet");

        //清理宠物装备
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_1", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_2", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_3", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipID_4", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_1", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_2", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_3", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("EquipIDHide_4", "", "ID", PetID, "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //更新当前任务
        Game_PublicClassVar.Get_function_Task.updataTaskItemID();

    }

    //检测是否捕捉
    public bool Pet_JianCeZhuaPet(GameObject monsterObj)
    {

        int nullSpace = Pet_ReturnPetFirstNull();
        if (nullSpace == -1)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_177");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前宠物栏位已满！宠物栏位每15、25、35、45、55级均会新增一个宠物栏位！");
            return false;
        }


        //获取当前怪物ID
        string petID = monsterObj.GetComponent<AI_1>().AI_PetID;
        if (petID == "" || petID == "0")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_180");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("该目标无法捕捉,捕捉无效！");
            return false;
        }


        //捕捉时有5%概率让目标直接死亡
        if (Random.value <= 0.05f)
        {
            //销毁目标
            Destroy(monsterObj.GetComponent<AI_1>().UI_Hp);
            Destroy(monsterObj, 0.7f);

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_181");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的捕捉动作太大,导致目标发现你藏起来了");
            return false;
        }

        //进入战斗无法捕捉
        /*
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经被怪物发现,无法捕捉！");
            return false;
        }
        */

        //获取宠物等级
        int petLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < petLv)
        {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_182");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_183");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + petLv + langStrHint_2);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你需要达到" + petLv + "级才可以捕捉！");
            return false;
        }

        return true;
    }



    //捕捉宠物技能
    public bool Pet_ZhuaPet(GameObject monsterObj,float puzhuoPro) {

        int nullSpace = Pet_ReturnPetFirstNull();
        if (nullSpace == -1) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_184");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前宠物栏位已满！");
            return false;
        }

        //捕捉时有5%概率让目标直接死亡
        if (Random.value <= 0.05f) {
            //销毁目标
            Destroy(monsterObj.GetComponent<AI_1>().UI_Hp);
            Destroy(monsterObj, 0.7f);

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_185");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你的捕捉动作太大,导致目标发现你藏起来了");
            return false;
        }

        //进入战斗无法捕捉
        /*
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经被怪物发现,无法捕捉！");
            return false;
        }
        */

        //判断目标血量(血量额外增加10%的捕捉成功率)
        float monsterHp = monsterObj.GetComponent<AI_Property>().AI_Hp;
        float monsterHpMax = monsterObj.GetComponent<AI_Property>().AI_HpMax;
        float hpCha = monsterHp / monsterHpMax;
        float hpPro = 0;
        if (hpCha <= 0.5f) {
            hpPro = (1-hpCha) * 0.1f;
        }

        //获取目标等级比(每高怪物1级提升目标自身2%的捕捉成功率)
        int lvCha = Game_PublicClassVar.Get_function_Rose.GetRoseLv() - monsterObj.GetComponent<AI_Property>().AI_Lv;
        if (lvCha > 5) {
            lvCha = 5;
        }

        puzhuoPro = puzhuoPro + lvCha * 0.02f + hpPro;
        //puzhuoPro = 1;
        //判定捕捉概率是否成功
        if (Random.value > puzhuoPro + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_BuZhuoPro)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_186");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("捕捉失败！");
            return false;
        }

        //获取当前怪物ID
        string petID = monsterObj.GetComponent<AI_1>().AI_PetID;
        if(petID==""||petID=="0"){
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_187");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("该目标无法捕捉,捕捉无效！");
            return false;
        }

        //获取宠物等级
        int petLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < petLv) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_188");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_189");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + petLv + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你需要达到" + petLv + "级才可以捕捉！");
            return false;
        }

        
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petID, "Pet_Template");
        //获取目标是否为宝宝
        if (monsterObj.GetComponent<AI_1>().AI_Type == 1)
        {
            //给宝宝做个标识
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IfBaby", "1", "ID", nullSpace.ToString(), "RosePet");
            petLv = 1;
        }
        else {
            //获取当前怪物的等级
            petLv = monsterObj.GetComponent<AI_Property>().AI_Lv;
        }

        //写入等级和经验（因为创建宠物不会默认创建跟默认野生宠物一样的等级）
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetName",petName, "ID", nullSpace.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetLv",petLv.ToString(),"ID", nullSpace.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PetExp", "0","ID", nullSpace.ToString(), "RosePet");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePet");

        //获取目标是否为宝宝
        if (monsterObj.GetComponent<AI_1>().AI_Type == 1)
        {
            Pet_Create(nullSpace.ToString(), petID, "1");
        }
        else
        {
            Pet_Create(nullSpace.ToString(), petID, "0");
        }

        //销毁目标
        Destroy(monsterObj.GetComponent<AI_1>().UI_Hp);
        Destroy(monsterObj, 0.7f);
        return true;
    }

    //返回当前拥有的宠物列表
    public string Pet_ReturnRosePetList() {

        string petIDListStr = "";
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum; i++) {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID","ID", i.ToString(), "RosePet");
            if (petID == "0" || petID == "")
            {
             //为0则表示没有宠物,不做任何操作   
            }
            else {
                if (petIDListStr == "")
                {
                    petIDListStr = i.ToString();
                }
                else {
                    petIDListStr = petIDListStr + ";" + i.ToString();
            
                }
            }
        }
        //Debug.Log("RosePetMaxNum = " + Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum + "petIDListStr = " + petIDListStr);
        return petIDListStr;
    }

    //返回当前第一个宠物
    public string Pet_ReturnRoseListFirst() {

        string listPetStr = Pet_ReturnRosePetList();
        string[] listPet = listPetStr.Split(';');
        if (listPet[0] == "" || listPet[0] == "0")
        {
            return "0";
        }
        else {
            return listPet[0];
        }
    }

    //返回当前列表内空的宠物位置
    public int Pet_ReturnPetFirstNull() {

        //循环读取宠物栏数量
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum; i++)
        {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            if (petID == "0" || petID == "")
            {
                //为0则表示没有宠物
                return i;
            }
        }
        //返回-1表示没有空的宠物位置
        return -1;
    }

    //返回当前除列表内的其他宠物
    public string Pet_ReturnRoseListOther(string otherPetIDStr)
    {
        string listPetStr = Pet_ReturnRosePetList();
        string[] listPet = listPetStr.Split(';');

        string[] otherPetID = otherPetIDStr.Split(';');
        string returnStr = "";

        for (int i = 0; i <= listPet.Length - 1; i++) {
            bool ifSave = true;
            for (int y = 0; y <= otherPetID.Length - 1; y++) {
                if (listPet[i] == otherPetID[y])
                {
                    ifSave = false;
                }
            }

            if (ifSave)
            {
                if (returnStr == "")
                {
                    returnStr = listPet[i];
                }
                else
                {
                    returnStr = returnStr + ";" + listPet[i];
                }
            }
        }

        return returnStr;
    }

    //返回当前合成列表内的宠物
    public string Pet_ReturnHeChengListStr() {

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet == null) {
            return "";
        }

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>() == null)
        {
            return "";
        }

        string heCheng_1 = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_1;
        string heCheng_2 = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_rosePet.GetComponent<UI_Pet>().Obj_Pet_HeChengSet.GetComponent<UI_PetHeCheng>().PetID_List_2;
        string returnStr = heCheng_1 + ";" + heCheng_2;

        if (heCheng_1 == "") {
            returnStr = heCheng_2;
        }

        if (heCheng_2 == "")
        {
            returnStr = heCheng_1;
        }

        if (heCheng_1 == "" && heCheng_2=="")
        {
            returnStr = "";
        }

        return returnStr;
    }

    public void Pet_UpdateLvMaxNum() {

        /*
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        int lvPetMaxLv = 3;
        if (roseLv<=15)
        {
            Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum = lvPetMaxLv;
            return ;
        }

        int chaLv = roseLv - 15;
        lvPetMaxLv = lvPetMaxLv + (int)(chaLv / 5);

        if (lvPetMaxLv > 12) {
            lvPetMaxLv = 12;
        }
        Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum = lvPetMaxLv;
        */

        int petNum_Sum = 0;
        int petNum_ChuShi = 3;
        int petNum_Lv = (int)((Game_PublicClassVar.Get_function_Rose.GetRoseLv()-5) / 10);    //每15 25 35 - 55级多一个宠物栏位
        if (petNum_Lv > 5)
        {
            petNum_Lv = 5;
        }
        if (petNum_Lv <0)
        {
            petNum_Lv = 0;
        }
        int petNum_ItemAdd = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetAddMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

        petNum_Sum = petNum_ChuShi + petNum_Lv + petNum_ItemAdd;
        //最多支持12个宠物
        if (petNum_Sum > Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum) {
            petNum_Sum = Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum;
        }
        Game_PublicClassVar.Get_game_PositionVar.RosePetLvMaxNum = petNum_Sum;

        return ;
    }

    //更换变异贴图
    public void AI_AddBianYiTieTu(GameObject aiObj,string petID)
    {
        //petID = "10001010";         //临时,以后其他变异贴图加入进来将此代码删除即可
        //Debug.Log("变异变异！");
        string modelShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", petID, "Pet_Template");
        modelShowID = modelShowID;

        //更换贴图
        object huiObj = (Material)Resources.Load("3DModel/MonsterModel/BianYiTieTu/" + modelShowID, typeof(Material));
        Material huiMaterial = huiObj as Material;
        SkinnedMeshRenderer skinned = null;
        if (aiObj.GetComponent<AI_1>() != null) {
            skinned = aiObj.GetComponent<AI_1>().ModelMesh;
        }
        if (aiObj.GetComponent<AIPet>() != null)
        {
            skinned = aiObj.GetComponent<AIPet>().ModelMesh;
        }
        if (aiObj.GetComponent<Player_Pet>() != null)
        {
            skinned = aiObj.GetComponent<Player_Pet>().ModelMesh;
        }

        if (huiMaterial != null && skinned != null) {
            skinned.material = huiMaterial;
        }

    }

    //添加空的宠物技能,显示作用
    public void Pet_SkillShowNull(string[] petSkillList, GameObject Obj_PetSkillIcon, GameObject Obj_SkillListSet)
    {
        //填补空位
        if (petSkillList.Length <= 12)
        {
            int startNum = 0;
            if (petSkillList[0] != "" && petSkillList[0] != "0")
            {
                startNum = petSkillList.Length;
            }

            for (int i = startNum; i < 12; i++)
            {
                //实例化一个宠物列表控件
                GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = "0";
            }
        }
        else
        {
            int skillyushu = petSkillList.Length % 4;
            for (int i = petSkillList.Length; i < 4; i++)
            {
                //实例化一个宠物列表控件
                GameObject petSkillListObj = (GameObject)Instantiate(Obj_PetSkillIcon);
                petSkillListObj.transform.SetParent(Obj_SkillListSet.transform);
                petSkillListObj.transform.localScale = new Vector3(1, 1, 1);
                petSkillListObj.GetComponent<PetSkillIcon>().PetSkillID = "0";
            }
        }
    }

    //获取当前宠物的上限
    public void Pet_GetPetMaxNum() {

        int petNum_Sum = 0;
        int petNum_ChuShi = 3;
        int petNum_Lv = (int)(Game_PublicClassVar.Get_function_Rose.GetRoseLv()/15);    //每15级多一个宠物栏位
        if (petNum_Lv > 4) {
            petNum_Lv = 4;
        }
        int petNum_ItemAdd = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetAddMaxNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));

        petNum_Sum = petNum_ChuShi + petNum_Lv + petNum_ItemAdd;
        Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum = petNum_Sum;

    }


    //召唤出战宠物
    public bool Pet_ZhaoHuan(string NowSclectPetID)
    {
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //判定出战
        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowSclectPetID, "RosePet");
        int fightLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FightLv", "ID", petID, "Pet_Template"));
        if (roseStatus.GetComponent<Rose_Proprety>().Rose_Lv < fightLv)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_192");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("出战等级不足");
            return false;
        }

        //判定当前是否进入出战CD
        float petZhaoHuanCD = Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanCD(NowSclectPetID);
        if (petZhaoHuanCD > 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_193");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("宠物召唤冷却");
            return false;
        }

        //判定出战位
        if (roseStatus.RosePetObj[int.Parse(NowSclectPetID) - 1] == null)
        {
            //判定当前出战
            if (Game_PublicClassVar.Get_function_Rose.GetRosePetFightNum() >= 1)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_194");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前出战位已满！");
                return false;
            }

            //出战
            Game_PublicClassVar.Get_function_Rose.RosePetCreate(int.Parse(NowSclectPetID));
            return true;
        }

        return false;
    }

    //添加宠物召唤冷却CD
    public void Pet_AddZhaoHuanCD(string RosePet_ID) {
        if (!Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.ContainsKey(RosePet_ID))
        {
            Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Add(RosePet_ID, 300);
        }
        else {
            Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD[RosePet_ID] = 300;
        }
    }

    //获取宠物召唤冷却CD
    public float Pet_GetZhaoHuanCD(string RosePet_ID) {

        if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.ContainsKey(RosePet_ID))
        {
            return Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD[RosePet_ID];
        }
        else {
            return 0;
        }
    }

    //减少宠物召唤冷却时间CD
    public void Pet_CostZhaoHuanCD(float costValue) {

        if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Count <= 0)
        {
            return;
        }

        //public Dictionary<string, float> rosePetZhaoHuanCD = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD;

        for (int i = 0; i < Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Count; i++)
        {
            //发送移动数据
            string petID = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.ElementAt(i).Key;
            if (petID != "")
            {
                Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD[petID] = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD[petID] - costValue;
                if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD[petID] <= 0)
                {
                    Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Remove(petID);
                }
            }
        }
    }

    //清理宠物召唤CD
    public void Pet_ClearnZhaoHuanCD() {
        Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD.Clear();
    }




    //添加宠物召唤无敌冷却CD
    public void Pet_AddZhaoHuanWuDiCD(string RosePet_ID)
    {
        if (!Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.ContainsKey(RosePet_ID))
        {
            Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.Add(RosePet_ID, 60);
        }
        else
        {
            Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD[RosePet_ID] = 60;
        }
    }

    //获取宠物召唤无敌冷却CD
    public float Pet_GetZhaoHuanWuDiCD(string RosePet_ID)
    {

        if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.ContainsKey(RosePet_ID))
        {
            return Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD[RosePet_ID];
        }
        else
        {
            return 0;
        }
    }

    //减少宠物召唤无敌冷却时间CD
    public void Pet_CostZhaoHuanWuDiCD(float costValue)
    {

        if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.Count <= 0)
        {
            return;
        }

        //public Dictionary<string, float> rosePetZhaoHuanCD = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanCD;

        for (int i = 0; i < Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.Count; i++)
        {
            //发送移动数据
            string petID = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.ElementAt(i).Key;
            if (petID != "")
            {
                Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD[petID] = Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD[petID] - costValue;
                if (Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD[petID] <= 0)
                {
                    Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.Remove(petID);
                }
            }
        }
    }

    //清理宠物召唤无敌CD
    public void Pet_ClearnZhaoHuanWuDiCD()
    {
        Game_PublicClassVar.Get_wwwSet.RosePetZhaoHuanWuDiCD.Clear();
    }




    public string Pet_ReturnChuZhanID(){
		//召唤默认出战的宠物
		for (int i = 1; i <= 12;i++ )
		{
			string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", i.ToString(), "RosePet");
			if (petStatus == "1") {
				return i.ToString ();
			}
		}
		return "";
	}

	//获取当前出战的宠物Obj
	public GameObject Pet_ReturnChuZhanObj(){

		Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status> ();
		for (int i = 0; i < roseStatus.RosePetObj.Length; i++) {

			if (roseStatus.RosePetObj [i] != null) {
				return roseStatus.RosePetObj [i];
			}
		}

		return null;
	}

    //记录击杀BOSS的时间
    public void SaveKillBossTime(string killBossID,int killTime) {
        string killBossTimeStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KillBossTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        string writeStr = "";
        if (killBossTimeStr == "" || killBossTimeStr == null) {
            writeStr = killBossID + "," + killTime + ",1";
        }
        else
        {
            //获取是否有意相同的怪物Id
            if (killBossTimeStr.Contains(killBossID)) {
                string writeKillBossStr = "";
                string[] killBossTimeList = killBossTimeStr.Split(';');
                for (int i = 0; i < killBossTimeList.Length; i++) {
                    string[] list = killBossTimeList[i].Split(',');
                    if (list.Length >= 3) {
                        //获取BOSS的ID
                        string nowBossID = list[0];
                        if (nowBossID == killBossID) {
                            string killWriteStr = "";
                            //获取时间
                            int lastkillTime = int.Parse(list[1]);
                            if (killTime < lastkillTime) {

                            }
                            //获取次数
                            int lastkillNum = int.Parse(list[2]);
                            lastkillNum = lastkillNum + 1;
                            killWriteStr = nowBossID + "," + lastkillTime + "," + lastkillNum;
                            killBossTimeList[i] = killWriteStr;
                        }

                        if (killBossTimeList[i] != "" && killBossTimeList[i] != null) {
                            writeKillBossStr = writeKillBossStr + ";" + killBossTimeList[i];
                        }
                    }
                }
                writeStr = writeKillBossStr;
            }
            else {
                if (killBossTimeStr != ""&& killBossTimeStr != null) {
                    writeStr = killBossTimeStr + ";" + killBossID + "," + killTime + ",1";
                }
                else
                {
                    writeStr = killBossID + "," + killTime + ",1";
                }
            }
        }

        //有时候会有;;;;;这种情况出现,这里排除
        string writeKillStr = "";
        string[] writeStrList = writeStr.Split(';');
        for (int i = 0; i < writeStrList.Length; i++) {
            if (writeStrList[i] != ""&& writeStrList[i] != null) {
                if (writeKillStr == "")
                {
                    writeKillStr = writeStrList[i];
                }
                else {
                    writeKillStr = writeKillStr + ";" + writeStrList[i];
                }
            }
        }

        //写入
        if (writeStr != "" && writeStr != null) {
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KillBossTime", writeKillStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
        //发送记录
        Game_PublicClassVar.Get_function_Rose.SendGameJiLu_1();
        }
    }

    //记录今日击杀
    public void DayKillMonsterNum() {

        string dayKillNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayKillNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (dayKillNumStr == "" || dayKillNumStr == null) {
            dayKillNumStr = "0";
        }

        int dayKillNum = int.Parse(dayKillNumStr) + 1;
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("DayKillNum", dayKillNum.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //记录击杀值
        Game_PublicClassVar.Get_game_PositionVar.DayKillMonsterNum = dayKillNum;

    }

    //传入一段技能ID,将互斥的ID排除在技能外
    public string ReturnPetSkillHuChi(string skillStr) {

        if (skillStr == "" || skillStr == null || skillStr == "0") {
            return "";
        }


        string huChiIDStr = "";
        string[] skillIDList = skillStr.Split(';');
        for (int i = 0; i < skillIDList.Length;i++) {
            if (skillIDList[i] != "") {
                string huChiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuChiID", "ID", skillIDList[i], "Skill_Template");
                string[] huChiIDList = huChiID.Split(';');

                if (huChiIDStr != "")
                {
                    huChiIDStr = huChiIDStr + ";" + huChiID;
                }
                else
                {
                    huChiIDStr = huChiID;
                }
            }
        }

        string[] allHuChiIDList = huChiIDStr.Split(';');
        for (int i = 0; i < allHuChiIDList.Length; i++) {
            if (allHuChiIDList[i] != "" && allHuChiIDList[i] != "0" && allHuChiIDList[i] != null) {
                //替换对应技能字符
                skillStr = skillStr.Replace(allHuChiIDList[i], "");
            }
        }

        string returnSkillStr = "";
        string[] nowSkillIDList = skillStr.Split(';');
        for (int i = 0; i < nowSkillIDList.Length; i++)
        {
            if (nowSkillIDList[i] != ""&& nowSkillIDList[i]!="0"&& nowSkillIDList[i]!=null)
            {
                if (returnSkillStr != "")
                {
                    returnSkillStr = returnSkillStr + ";" + nowSkillIDList[i];
                }
                else {
                    returnSkillStr = nowSkillIDList[i];
                }
            }
        }

        return returnSkillStr;

    }

    //根据幸运返回属性加成
    public float LuckRetuenAddDamge() {
        float luckProValue = 0;
        int nowLuck = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lucky);
        if (nowLuck > 9) {
            nowLuck = 9;
        }
        switch ((int)(nowLuck)) {
            case 0:
                luckProValue = 0f;
                break;
            case 1:
                luckProValue = 0.025f;
                break;
            case 2:
                luckProValue = 0.05f;
                break;
            case 3:
                luckProValue = 0.075f;
                break;
            case 4:
                luckProValue = 0.1f;
                break;
            case 5:
                luckProValue = 0.15f;
                break;
            case 6:
                luckProValue = 0.2f;
                break;
            case 7:
                luckProValue = 0.25f;
                break;
            case 8:
                luckProValue = 0.3f;
                break;
            case 9:
                luckProValue = 0.35f;
                break;
        }
        return luckProValue;
    }

    public bool PetIfWearEquip(string PetID) {

        //判断合成是否佩戴装备
        bool ifWearEquip = false;

        for (int i = 1; i <= 3; i++)
        {

            string EquipID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_" + i, "ID", PetID, "RosePet");

            if (EquipID_1 != "" && EquipID_1 != "0" && EquipID_1 != null)
            {
                ifWearEquip = true;
            }

        }

        return ifWearEquip;

    }
}
