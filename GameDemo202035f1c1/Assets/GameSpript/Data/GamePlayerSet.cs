using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayerSet : MonoBehaviour {

    //玩家集合
    public Dictionary<string, GameObject> PlayerSet = new Dictionary<string, GameObject>();

    //战斗
    //移动数据集合
    public MapThread_PlayerMoveList mapThread_PlayerMoveList = new MapThread_PlayerMoveList();
    //技能数据集合
    public MapThread_PlayerSkillList mapThread_PlayerSkillList = new MapThread_PlayerSkillList();

    public GameObject PlayerObjSet;         //玩家的集合
    public GameObject Obj_Player;

    //public bool MoveStatus;
    public string Rose_PlayerID;            //自己的服务器线程ID
    public GameObject Obj_PlayerHpName;     //玩家血条
    public GameObject Obj_PlayerPetHp;      //玩家宠物血条
    public string selfZhanghaoID;
    public string nowYanSeID;
    public string nowNowYanSeHairID;

    // Use this for initialization
    void Start () {

        //设置角色开始发送坐标
        //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendStatus = true;

        //设置自己的线程ID
        Rose_PlayerID = Game_PublicClassVar.Get_wwwSet.ServerRoseThreadID;
        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj = this.gameObject;
        selfZhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        SendEnterMap();

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void SendEnterMap() {

        //进入地图
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        MapThread_EnterMapData mapThread_EnterMapData = new MapThread_EnterMapData();
        //mapThread_EnterMapData.MapName = "Map_1102";
        mapThread_EnterMapData.MapName = Application.loadedLevelName;
        mapThread_EnterMapData.ZhangHaoID = zhanghaoID;
        mapThread_EnterMapData.Position_X = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.x * 100);
        mapThread_EnterMapData.Position_Y = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y * 100);
        mapThread_EnterMapData.Position_Z = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.z * 100);
        mapThread_EnterMapData.MoveDirection = (int)(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.rotation.y * 100);
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000201, mapThread_EnterMapData);

        //MoveStatus = true;

        //设置角色开始发送坐标
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendType = "2";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendServerType = "2";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().EnterMapServerName = mapThread_EnterMapData.MapName;

    }

    //销毁调用
    void OnDestroy()
    {
        //离开地图
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        MapThread_EnterMapData mapThread_EnterMapData = new MapThread_EnterMapData();
        //mapThread_EnterMapData.MapName = "Map_1102";
        mapThread_EnterMapData.MapName = Application.loadedLevelName;
        mapThread_EnterMapData.ZhangHaoID = zhanghaoID;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20000202, mapThread_EnterMapData);

        //设置角色开始发送坐标
        //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendStatus = false;
    }


    //添加玩家
    public void Player_Add(string playerID, Pro_Fight_CreatePlayer createDataList)
    {

        //判断场景内怪物大于40个则不再获得
        if (PlayerSet.Count >= 20) {
            //Debug.Log("玩家人数已满！" + playerID);
            return;
        }

        //单机模式不在添加
        if (Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus == 1) {
            return;
        }

        //判定此ID是不是自己
        
        if (Rose_PlayerID == playerID)
        {
            return;
        }

        if (selfZhanghaoID == createDataList.ZhangHaoID) {

            return;
        }
        
        if (PlayerSet.ContainsKey(playerID))
        {
           //Debug.Log("重复ID = " + playerID);
            return;
        }

        //其他特殊情况
        if (playerID == "" || playerID == "0" || playerID == null) {
            return;
        }

        if (Rose_PlayerID == "" || Rose_PlayerID == "0" || Rose_PlayerID == null)
        {
            return;
        }

        //Debug.Log("开始创建角色playerID = " + playerID);
        //创建角色
        GameObject playerObj = (GameObject)Instantiate(Obj_Player);
        playerObj.transform.SetParent(PlayerObjSet.transform);
        playerObj.transform.localScale = new Vector3(1, 1, 1);

        //设置坐标点
        Vector3 poisitionVec3 = new Vector3(createDataList.Move_X, createDataList.Move_Y + 100, createDataList.Move_Z);
        poisitionVec3 = poisitionVec3 / 100;
        playerObj.SetActive(false);
        playerObj.transform.position = poisitionVec3;
        //随机一个小范围  不要挤在一起
        //playerObj.transform.position = new Vector3(poisitionVec3.x + Random.value*5 - 2.5f, poisitionVec3.y, poisitionVec3.z + Random.value*5 - 2.5f);
        playerObj.SetActive(true);

        //创建名称显示
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(playerObj.GetComponent<Player_Bone>().Bone_Head.transform.position);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);
        GameObject UI_Hp = (GameObject)Instantiate(Obj_PlayerHpName);

        //显示UI,并对其相应的属性修正
        UI_Hp.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_AIHpSet.transform);
        UI_Hp.transform.localPosition = new Vector3(Hp_show_position.x, Hp_show_position.y, 0);
        UI_Hp.transform.localScale = new Vector3(1f, 1f, 1f);

        //显示当前UI名称 （不显示等级,显示等级在createDataList里有参数直接调用即可）
        UI_Hp.GetComponent<UI_PlayerHp>().Obj_Name.GetComponent<Text>().text = createDataList.PlayerName;
        UI_Hp.GetComponent<UI_PlayerHp>().Obj_Lv.GetComponent<Text>().text = createDataList.PlayerLv;
        playerObj.GetComponent<Player_Status>().Obj_HpNameUI = UI_Hp;
        playerObj.GetComponent<Player_Status>().Player_ZhangHaoID = createDataList.ZhangHaoID;
        //createDataList.ZuoQiID = "10004";
        playerObj.GetComponent<Player_Status>().ZuoQiID = createDataList.ZuoQiID;
        playerObj.GetComponent<Player_Status>().PlayerShowZuoQi();

        //创建对应职业的模型
        Player_Bone playerBone = playerObj.GetComponent<Player_Bone>();
        playerBone.FaShi_Obj.SetActive(false);
        playerBone.ZhanShi_Obj.SetActive(false);
        playerBone.LieRen_Obj.SetActive(false);

        //createDataList.OccType = "2";

        playerObj.GetComponent<Player_Status>().OccType = createDataList.OccType;

        switch (createDataList.OccType)
        {
            //战士
            case "1":
                playerObj.GetComponent<Animator>().runtimeAnimatorController = playerBone.ZhanShi_Animator;
                playerObj.GetComponent<Animator>().avatar = playerBone.ZhanShi_Avatar;
                playerBone.ZhanShi_Obj.SetActive(true);

                break;
            //法师
            case "2":
                playerObj.GetComponent<Animator>().runtimeAnimatorController = playerBone.FaShi_Animator;
                playerObj.GetComponent<Animator>().avatar = playerBone.FaShi_Avatar;
                playerBone.FaShi_Obj.SetActive(true);
                break;

            //法师
            case "3":
                playerObj.GetComponent<Animator>().runtimeAnimatorController = playerBone.LieRen_Animator;
                playerObj.GetComponent<Animator>().avatar = playerBone.LieRen_Avatar;
                playerBone.LieRen_Obj.SetActive(true);
                break;
        }

        //显示称号
        if (createDataList.ChengHaoID != null) {
            string nowUseChengHaoID = createDataList.ChengHaoID;
            Player_ShowChengHao(UI_Hp, nowUseChengHaoID);
            Player_ShowZhenYingImg(UI_Hp, createDataList.NowZhenYing);
        }

        //显示武器
        Player_ShowWeapon(playerObj, createDataList.EquipData);

        //显示宠物
        Player_ShowPet(playerObj, createDataList.PetData);

        //显示外观
        nowYanSeID = createDataList.NowYanSeID;
        if (nowYanSeID != "" && nowYanSeID != "0" && nowYanSeID != null) {
            string nowYanSe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowYanSeID, "RanSe_Template");
            Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSe, nowYanSeID, false, "1", playerObj);
        }

        nowNowYanSeHairID = createDataList.NowNowYanSeHairID;
        if (nowNowYanSeHairID != "" && nowNowYanSeHairID != "0" && nowNowYanSeHairID != null)
        {
            string nowYanSeHair = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", nowNowYanSeHairID, "RanSe_Template");
            Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSeHair, nowNowYanSeHairID, false, "2", playerObj);
        }
        
        //添加其他玩家集合
        if (playerID != null && playerID != "")
        {
            if (PlayerSet.ContainsKey(playerID))
            {
                PlayerSet.Remove(playerID);
            }
            PlayerSet.Add(playerID, playerObj);
        }

    }



    //显示称号
    public void Player_ShowChengHao(GameObject ShowObj,string ChengHaoID) {

        if (ChengHaoID != ""&& ChengHaoID!=null&& ShowObj!=null)
        {
            string chenghaoIamge = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Iamge", "ID", ChengHaoID, "ChengHao_Template");
            if (chenghaoIamge != "" && chenghaoIamge != null && chenghaoIamge != "0")
            {
                object obj = Resources.Load("ChengWeiIcon/" + chenghaoIamge, typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_ChengHao.SetActive(true);
                ShowObj.GetComponent<UI_PlayerHp>().Obj_ChengHao.GetComponent<Image>().sprite = itemIcon;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_ChengHao.GetComponent<Image>().SetNativeSize();
            }
            else
            {
                ShowObj.GetComponent<UI_PlayerHp>().Obj_ChengHao.SetActive(false);
            }
        }
    }

    //显示阵营
    public void Player_ShowZhenYingImg(GameObject ShowObj,string zhenying) {

        ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.SetActive(false);
        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.SetActive(false);
        }
        else
        {
            if (zhenying == "1")
            {
                object obj = Resources.Load("GameUI/Img/UI_Image_319", typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.GetComponent<Image>().sprite = itemIcon;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.SetActive(true);
            }

            if (zhenying == "2")
            {
                object obj = Resources.Load("GameUI/Img/UI_Image_318", typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.GetComponent<Image>().sprite = itemIcon;
                ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.SetActive(true);
            }

            //战士修正位置
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1")
            {
                ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.transform.localPosition = new Vector3(ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.transform.localPosition.x, 0, ShowObj.GetComponent<UI_PlayerHp>().Obj_PaiImg.transform.localPosition.z);
            }

        }


    }



    //显示武器
    public void Player_ShowWeapon(GameObject playerObj, string EquipDataStr)
    {

        string EquipID = "";
        if (EquipDataStr != null && EquipDataStr != "" && playerObj != null)
        {
            string[] equipIDList = EquipDataStr.Split('|');
            for (int i = 0; i < equipIDList.Length; i++)
            {
                if (equipIDList[i] != "0" && equipIDList[i] != "" && equipIDList.Length >= 2)
                {
                    string[] equipDataList = equipIDList[i].Split('@');
                    string equipID = equipDataList[0];
                    string equipType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemType", "ID", equipID, "Item_Template");
                    string equipSubType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemSubType", "ID", equipID, "Item_Template");
                    if (equipType == "3" && equipSubType == "1")
                    {
                        //Player_ShowWeapon(playerObj, equipID);
                        EquipID = equipID;
                        break;
                    }
                }
            }
        }

        //显示武器模型
        Player_ShowWeaponEquipID(playerObj, EquipID);

    }


    //显示武器
    public void Player_ShowWeaponEquipID(GameObject playerObj, string EquipID)
    {
        GameObject targetObj = playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj.gameObject;
        for (int i = 0; i < targetObj.transform.childCount; i++)
        {
            GameObject go = targetObj.transform.GetChild(i).gameObject;
            if (go != playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base)
            {
                MonoBehaviour.Destroy(go);
            }
        }

        if (EquipID == "")
        {
            return;
        }

        //显示角色武器
        string showWeaponID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemMondel", "ID", EquipID, "Item_Template", playerObj.GetComponent<Player_Status>().OccType);
        //Debug.Log("showWeaponID = " + showWeaponID);
        GameObject weaponObj = (GameObject)Resources.Load("3DModel/WeaponModel/" + showWeaponID, typeof(GameObject));
        if (weaponObj == null)
        {
            playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(true);
            playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(true);
            playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(true);
            //Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseModelWeaponBaseModel.SetActive(true);
            return;
        }

        GameObject SkillObject_p = (GameObject)MonoBehaviour.Instantiate(weaponObj);
        SkillObject_p.SetActive(false);
        playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(false);
        playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(false);
        playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj_Base.SetActive(false);

        //战士位置
        if (playerObj.GetComponent<Player_Status>().OccType == "1"|| playerObj.GetComponent<Player_Status>().OccType == ""|| playerObj.GetComponent<Player_Status>().OccType == null) {
            SkillObject_p.transform.SetParent(playerObj.GetComponent<Player_Bone>().ZhanShi_WuQiObj.transform);
        }
        //法师位置
        if (playerObj.GetComponent<Player_Status>().OccType == "2")
        {
            SkillObject_p.transform.SetParent(playerObj.GetComponent<Player_Bone>().FaShi_WuQiObj.transform);
        }

        //法师位置
        if (playerObj.GetComponent<Player_Status>().OccType == "3")
        {
            SkillObject_p.transform.SetParent(playerObj.GetComponent<Player_Bone>().LieRen_WuQiObj.transform);
        }

        //设置位置
        SkillObject_p.transform.localRotation = Quaternion.Euler(90, 0, 0);
        SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);
        SkillObject_p.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        SkillObject_p.SetActive(true);
    }

    //显示宠物
    public void Player_ShowPet(GameObject ShowObj, string PetData)
    {

        if (PetData == "" || PetData == null) {
            return;
        }

        string PetID = "";
        string PetName = "";
        string PetLv = "";
        string[] petDataList = PetData.Split('|');
        for (int i = 0; i< petDataList.Length; i++) {
            string[] nowPetDataList = petDataList[i].Split('@');
            if (nowPetDataList[0] == "1") {
                if (nowPetDataList.Length >= 7)
                {
                    PetID = nowPetDataList[1];
                    PetName = nowPetDataList[6];
                    PetLv = nowPetDataList[2];
                    break;
                }
            }
        }

        //显示宠物模型
        Player_ShowPetID(ShowObj, PetID, PetName, PetLv);
    }



    //显示宠物
    public void Player_ShowPetID(GameObject ShowObj, string PetID,string PetName,string PetLv)
    {
        if (PetID == "")
        {
            return;
        }

        //清空宠物列表显示
        if (ShowObj.GetComponent<Player_Status>().PlayerPetObj != null) {
            Destroy(ShowObj.GetComponent<Player_Status>().PlayerPetObj);
        }

        string petModel = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetModel", "ID", PetID, "Pet_Template");

        //获取怪物并设置位置
        if (petModel != "")
        {
            GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + petModel, typeof(GameObject)));
            monsterObj.SetActive(false);
            monsterObj.transform.SetParent(this.gameObject.transform);
            //Vector3 CreateVec3 = ShowObj.GetComponent<Player_Bone>().PetPositionSet_Posi_1.transform.position;
            //Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
            Vector3 CreateVec3 = ShowObj.transform.position;
            monsterObj.transform.position = new Vector3(CreateVec3.x, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position.y+0.1f, CreateVec3.z);
            //Debug.Log("CreateVec3 = " + CreateVec3);

            //增加脚本
            monsterObj.AddComponent<Player_Pet>();
            monsterObj.GetComponent<Player_Pet>().Obj_Player = ShowObj;
            monsterObj.GetComponent<Player_Pet>().Ai_HpShowPosition = monsterObj.GetComponent<AIPet>().Ai_HpShowPosition;
            monsterObj.GetComponent<Player_Pet>().Obj_PetNameUI = Obj_PlayerPetHp;
            monsterObj.GetComponent<Player_Pet>().PetName = PetName;
            monsterObj.GetComponent<Player_Pet>().PetLv = PetLv;
            monsterObj.GetComponent<Player_Pet>().ModelMesh = monsterObj.GetComponent<AIPet>().ModelMesh;
            monsterObj.layer = 21;                                          //设置层级为玩家层级,要不原始层级为宠物的话会报错

            //删除脚本
            Destroy(monsterObj.GetComponent<AIPet>());
            Destroy(monsterObj.GetComponent<AI_Property>());

            //查看当前是否为变异
            string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetType", "ID", PetID, "Pet_Template");
            if (monsterType == "1")
            {
                Game_PublicClassVar.Get_function_AI.AI_AddBianYiTieTu(monsterObj, PetID);
            }

            monsterObj.SetActive(true);

            ShowObj.GetComponent<Player_Status>().PlayerPetObj = monsterObj;

        }

    }

    //展示模型
    public void Player_ShowModel() {


    }


    //删除玩家
    public void Player_Delete(string playerID)
    {

        //添加其他玩家集合
        if (PlayerSet.ContainsKey(playerID))
        {
            //播放传送特效
            GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + "Eff_ChuanSong", typeof(GameObject));
            if (SkillEffect != null)
            {
                GameObject effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                effect.transform.position = PlayerSet[playerID].transform.position;
                effect.SetActive(true);
                Destroy(effect, 1.5f);
            }
            //删除玩家模型
            Destroy(PlayerSet[playerID],0.7f);
            //删除集合体
            PlayerSet.Remove(playerID);
        }

    }

    //删除全部玩家
    public void Player_DeleteAll()
    {
        foreach (string playerID in PlayerSet.Keys)
        {
            //添加其他玩家集合
            if (PlayerSet.ContainsKey(playerID))
            {
                //删除玩家模型
                Destroy(PlayerSet[playerID]);
                //删除集合体
                PlayerSet.Remove(playerID);
            }
        }
        PlayerSet.Clear();
    }

    public void PlayerAllMove()
    {

        for (int i = 0; i < mapThread_PlayerMoveList.mapThread_PlayerMove.Count; i++)
        {
            string playerID = mapThread_PlayerMoveList.mapThread_PlayerMove.ElementAt(i).Key;
            //Debug.Log("PlayerAllMove ___________  playerID = " + playerID);
            MapThread_PlayerMove pro_Fight_MoveList = mapThread_PlayerMoveList.mapThread_PlayerMove[playerID];
            //如果没有角色则创建目标
            if (PlayerSet.ContainsKey(playerID))
            {
                //设置每个玩家的移动到的坐标位置
                if (PlayerSet.ContainsKey(playerID))
                {
                    //Debug.Log("接受移动时间33333：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    if (PlayerSet[playerID] != null)
                    {
                        PlayerSet[playerID].GetComponent<Player_Status>().MovePosition = new Vector3(pro_Fight_MoveList.Move_X, pro_Fight_MoveList.Move_Y, pro_Fight_MoveList.Move_Z);
                        PlayerSet[playerID].GetComponent<Player_Status>().PlayerMove();
                        //Debug.Log("playerID = " + playerID + ";pro_Fight_MoveList = " + pro_Fight_MoveList.Move_X + "," + pro_Fight_MoveList.Move_Y);
                    }
                    else
                    {
                        //Debug.Log("移动目标为空！");
                    }
                }
            }
            else
            {
                //判定此ID是不是自己
                if (Rose_PlayerID == playerID)
                {
                    //break;
                }
                else {
                    //重新申请显示对应的玩家
                    //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001003, playerID);
                }
            }
        }

        /*
                    foreach (string playerID in  mapThread_PlayerMoveList.mapThread_PlayerMove.Keys)
                    {
                    Debug.Log("playerID = " + playerID);
                    MapThread_PlayerMove pro_Fight_MoveList =  mapThread_PlayerMoveList.mapThread_PlayerMove[playerID];
                    //如果没有角色则创建目标
                    if (PlayerSet.ContainsKey(playerID))
                    {
                        //设置每个玩家的移动到的坐标位置
                        if (PlayerSet.ContainsKey(playerID))
                        {
                            //Debug.Log("接受移动时间33333：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                            if (PlayerSet[playerID] != null)
                            {
                                PlayerSet[playerID].GetComponent<Player_Status>().MovePosition = new Vector3(pro_Fight_MoveList.Move_X, pro_Fight_MoveList.Move_Y, pro_Fight_MoveList.Move_Z);
                                PlayerSet[playerID].GetComponent<Player_Status>().PlayerMove();
                                Debug.Log("playerID = " + playerID + ";pro_Fight_MoveList = " + pro_Fight_MoveList.Move_X);
                            }
                            else
                            {
                                Debug.Log("移动目标为空！");
                            }
                        }
                    }
                    else
                    {
                        //判定此ID是不是自己
                        if (Rose_PlayerID == playerID)
                        {
                            break;
                        }

                        //重新申请显示对应的玩家
                        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001003, playerID);
                    }
                }
                */
    }

}
