using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerHuoDong_1 : MonoBehaviour {

    //玩家集合
    public Dictionary<string,GameObject> PlayerSet= new Dictionary<string, GameObject>();
    //宝箱集合
    public Dictionary<string, GameObject> ChestSet = new Dictionary<string, GameObject>();
    //玩家地图移动
    //Dictionary<string, PlayerThread_MoveValue> Player_Move = new Dictionary<string, PlayerThread_MoveValue>();

    //战斗
    public Pro_Fight_Move pro_Fight_Move = new Pro_Fight_Move();
    public GameObject PlayerObjSet;
    public GameObject Obj_Player;

    //public bool MoveStatus;
    public string Rose_PlayerID;            //自己的服务器线程ID
    public GameObject Obj_PlayerHpName;     //玩家血条

    public GameObject Obj_ChestSet;           //宝箱节点
    public GameObject Obj_Chest_1;            //宝箱Obj
    public GameObject Obj_Chest_2;            //宝箱Obj
    public GameObject Obj_Chest_3;            //宝箱Obj
    public GameObject Obj_Chest_4;            //宝箱Obj

    //结构体
    /*
    struct PlayerThread_MoveValue
    {
        //移动
        //public bool MoveStatus;
        public string MapName;
        public int Move_X;
        public int Move_Y;
        public int Move_Z;
    }
    */

    // Use this for initialization
    void Start () {
        //设置自己的线程ID
        Rose_PlayerID = Game_PublicClassVar.Get_wwwSet.ServerRoseThreadID;
        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1 = this.gameObject;

        //进入地图
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001001, zhanghaoID);
        //MoveStatus = true;

        //设置角色开始发送坐标
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendType = "1";
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendServerType = "1";
    }
	
	// Update is called once per frame
	void Update () {

        /*
        //更新坐标位置
        if (MoveStatus) {
            MoveStatus = false;
            foreach (string playerID in pro_Fight_Move.Fight_MoveList.Keys) {

                Pro_Fight_MoveList pro_Fight_MoveList = pro_Fight_Move.Fight_MoveList[playerID];
                //如果没有角色则创建目标
                if (!PlayerSet.ContainsKey(playerID))
                {
                    //创建角色
                    Vector3 poisitionVec3 = new Vector3(pro_Fight_MoveList.Move_X, pro_Fight_MoveList.Move_Y, pro_Fight_MoveList.Move_Z);
                    Player_Add(playerID, poisitionVec3);
                }

                //设置每个玩家的移动到的坐标位置
                if (PlayerSet.ContainsKey(playerID)) {
                    PlayerSet[playerID].GetComponent<Player_Status>().MovePosition = new Vector3(pro_Fight_MoveList.Move_X, pro_Fight_MoveList.Move_Y, pro_Fight_MoveList.Move_Z);
                    PlayerSet[playerID].GetComponent<Player_Status>().UpdateDataStatus = true;
                }
            }
        }
        */
	}

    //销毁调用
    void OnDestroy()
    {
        //离开地图
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(20001002, "");
        //设置角色开始发送坐标
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().MovePositionSendStatus = false;
    }

    //添加玩家
    public void Player_Add(string playerID, Pro_Fight_CreatePlayer createDataList ) {



        //判定此ID是不是自己
        if (Rose_PlayerID == playerID) {
            return;
        }
        

        if (PlayerSet.ContainsKey(playerID))
        {
            Debug.Log("重复ID = " + playerID);
            return;
        }

        Debug.Log("开始创建角色playerID = " + playerID);
        //创建角色
        GameObject playerObj = (GameObject)Instantiate(Obj_Player);
        playerObj.transform.SetParent(PlayerObjSet.transform);
        playerObj.transform.localScale = new Vector3(1, 1, 1);

        //设置坐标点
        Vector3 poisitionVec3 = new Vector3(createDataList.Move_X, createDataList.Move_Y + 100, createDataList.Move_Z);
        poisitionVec3 = poisitionVec3 / 100;
        Debug.Log("poisitionVec3 = " + poisitionVec3);
        playerObj.SetActive(false);
        //playerObj.transform.position = new Vector3(15,31,-36);
        playerObj.transform.position = poisitionVec3;
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
        playerObj.GetComponent<Player_Status>().Obj_HpNameUI = UI_Hp;


        //创建对应职业的模型
        Player_Bone playerBone = playerObj.GetComponent<Player_Bone>();
        playerBone.FaShi_Obj.SetActive(false);
        playerBone.ZhanShi_Obj.SetActive(false);
        switch (createDataList.OccType) {
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

            //猎人
            case "3":
                playerObj.GetComponent<Animator>().runtimeAnimatorController = playerBone.LieRen_Animator;
                playerObj.GetComponent<Animator>().avatar = playerBone.LieRen_Avatar;
                playerBone.LieRen_Obj.SetActive(true);
                break;
        }


        //添加其他玩家集合
        if (playerID != null && playerID != "") {
            if (PlayerSet.ContainsKey(playerID))
            {
                PlayerSet.Remove(playerID);
            }
            PlayerSet.Add(playerID, playerObj);
        }

    }

    //删除玩家
    public void Player_Delete(string playerID) {

        //添加其他玩家集合
        if (PlayerSet.ContainsKey(playerID))
        {
            //删除玩家模型
            Destroy(PlayerSet[playerID]);
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

    public void PlayerAllMove() {

        //Debug.Log("接受移动时间22222：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
        foreach (string playerID in pro_Fight_Move.Fight_MoveList.Keys)
        {
            Pro_Fight_MoveList pro_Fight_MoveList = pro_Fight_Move.Fight_MoveList[playerID];
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
                    }
                    else {
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
    }

    //创建宝箱
    public void Chest_Create(Pro_HuoDong1_CreateChest createChestData) {

        if (!ChestSet.ContainsKey(createChestData.ChestID.ToString()))
        {
            //更具传过来的类型生成不同的宝箱
            string scenceItemID = "42001001";
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            GameObject scenceItemObj = Obj_Chest_1;
            switch (createChestData.ChestType) {

                //低级宝箱
                case "1":
                    scenceItemID = "42001001";
                    scenceItemObj = Obj_Chest_1;
                    break;

                //中级宝箱
                case "2":
                    scenceItemID = "42001002";
                    scenceItemObj = Obj_Chest_2;
                    break;

                //高级宝箱
                case "3":
                    scenceItemID = "42001003";
                    scenceItemObj = Obj_Chest_3;
                    break;

                //高级宝箱
                case "4":
                    scenceItemID = "42001004";
                    scenceItemObj = Obj_Chest_4;
                    break;
            }

            //特殊处理
            int addValue = 0;
            if (roseLv >= 18) {
                addValue = 1000;
            }
            if (roseLv >= 30)
            {
                addValue = 2000;
            }
            if (roseLv >= 40)
            {
                addValue = 3000;
            }
            if (roseLv >= 50)
            {
                addValue = 4000;
            }
            if (addValue != 0) {
                scenceItemID = (int.Parse(scenceItemID) + addValue).ToString();
            }

            //创建宝箱
            Vector3 vec3 = new Vector3(createChestData.Posi_X, createChestData.Posi_Y, createChestData.Posi_Z);
            vec3 = vec3 / 100;
            GameObject chestObj = (GameObject)Instantiate(scenceItemObj);
            chestObj.transform.SetParent(Obj_ChestSet.transform);
            chestObj.transform.localScale = new Vector3(1, 1, 1);
            chestObj.transform.position = vec3;

            //创建宝箱数据
            chestObj.GetComponent<UI_GetherItem>().SceneItemType = "2";
            chestObj.GetComponent<UI_GetherItem>().ChestID = createChestData.ChestID;
            chestObj.GetComponent<UI_GetherItem>().SceneItmeID = scenceItemID;
            chestObj.GetComponent<UI_GetherItem>().ChestInitia();

            //添加宝箱集合
            ChestSet.Add(createChestData.ChestID.ToString(), chestObj);
            
        }
    }

    //打开宝箱
    public void Chest_Open(string chestID) {

        if (ChestSet.ContainsKey(chestID)) {
            GameObject openChestObj = ChestSet[chestID];
            openChestObj.GetComponent<UI_GetherItem>().ChestReward();
        }
    }

    //删除宝箱
    public void Chest_Delte(string chestID)
    {
        if (ChestSet.ContainsKey(chestID))
        {
            GameObject openChestObj = ChestSet[chestID];
            Destroy(openChestObj);
            ChestSet.Remove(chestID);
        }
    }
}
