using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Linq;

public class UI_HuoDong_BaoXiang : MonoBehaviour {

    //宝箱集合
    public Dictionary<int, BaoXiangData> ChestSet = new Dictionary<int, BaoXiangData>();

    //public List<GameObject> ChestSet = new List<GameObject>();

    public GameObject[] Obj_ChestPositionList;      //表象随机刷新点
    public GameObject Obj_ChestSet;           //宝箱节点
    public GameObject Obj_Chest_1;            //宝箱Obj
    public GameObject Obj_Chest_2;            //宝箱Obj
    public GameObject Obj_Chest_3;            //宝箱Obj
    public GameObject Obj_Chest_4;            //宝箱Obj

    public ObscuredBool BaoXiangActiveStatus;           //宝箱活动激活状态
    public ObscuredBool IfSendBaoXiangStatus;           //是否发送了一次宝箱
    public ObscuredBool IfEndBaoXiangActive;            //是否结束宝箱活动
    public int ChestXuHao = 0;
    public float SendBaoXiangTimeSum;                   //每60秒发一次,多了玩家可以重新登陆
    private float SendBaoXiangJianGeTime;                   //两次发送宝箱的间隔时间
    public int GaoJiChestNum = 0;

    //内部计时
    private ObscuredFloat JiShiNum;                     //活动累计时间
    public ObscuredFloat HuoDongCostTime;              //活动剩余时间

    public GameObject Obj_ShiQuBtn;

    public bool ttt;

    // Use this for initialization
    void Start () {

        SendBaoXiangJianGeTime = 60;
        SendBaoXiangTimeSum = SendBaoXiangJianGeTime;
        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang = this.gameObject;
        Obj_ShiQuBtn.SetActive(false);
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001801, "");
        //Init();

    }
	
	// Update is called once per frame
	void Update () {

        if (BaoXiangActiveStatus)
        {

            //活动时间超过5分钟直接关闭
            JiShiNum = JiShiNum + Time.deltaTime;
            if (JiShiNum >= HuoDongCostTime)
            {
                IfEndBaoXiangActive = true;
            }
            else
            {
                SendBaoXiangTimeSum = SendBaoXiangTimeSum + Time.deltaTime;
                if (SendBaoXiangTimeSum >= SendBaoXiangJianGeTime)
                {
                    SendBaoXiangTimeSum = 0;
                    IfSendBaoXiangStatus = true;
                }

                if (IfSendBaoXiangStatus)
                {

                    for (int i = 1; i <= 10; i++)
                    {
                        //创建普通宝箱
                        Chest_Create();
                        Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet = ChestSet;
                    }

                    IfSendBaoXiangStatus = false;
                }
            }



            //活动结束 清除数据
            if (IfEndBaoXiangActive)
            {
                BaoXiangActiveStatus = false;
                JiShiNum = 0;
                Clearn();
                Obj_ShiQuBtn.SetActive(false);
                IfEndBaoXiangActive = false;
            }

            Obj_ShiQuBtn.SetActive(true);

        }
        else {
            Obj_ShiQuBtn.SetActive(false);
        }

        if (ttt) {
            BaoXiangActiveStatus = true;
            HuoDongCostTime = 600;
            ttt = false;
            Init();
        }
	}


    public void Init() {

        GaoJiChestNum = 0;

        if (BaoXiangActiveStatus)
        {
            if (Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet.Count >= 1)
            {

                SendBaoXiangTimeSum = 0;

                foreach (BaoXiangData baoXiangData in Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet.Values)
                {
                    CreateChestObj(baoXiangData.BaoXiangType, baoXiangData.BaoXiangID, baoXiangData.BaoXiangVec3);
                    Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet = ChestSet;
                }
            }
            else {
                SendBaoXiangTimeSum = SendBaoXiangJianGeTime;
            }
        }
        else {
            SendBaoXiangTimeSum = SendBaoXiangJianGeTime;
        }

    }


    //创建宝箱
    public void Chest_Create()
    {

        if (Application.loadedLevelName != "EnterGame") {
            return;
        }

        //场内最多25个宝箱
        if (ChestSet.Count >= 25)
        {
            return;
        }

        string CreateType = "1";

        //根据概率换算类型
        float ranValueNow = Random.value;
        bool ranStatusNow = false;
        if (ranValueNow <= 0.835f && ranStatusNow == false)
        {
            CreateType = "1";
            ranStatusNow = true;
        }
        if (ranValueNow <= 0.935f && ranStatusNow == false)
        {
            CreateType = "2";
            ranStatusNow = true;
        }
        if (ranValueNow <= 0.95f && ranStatusNow == false)
        {
            CreateType = "3";
            ranStatusNow = true;
        }
        if (ranValueNow <= 1f && ranStatusNow == false)
        {
            CreateType = "4";
            ranStatusNow = true;
        }

        //更具传过来的类型生成不同的宝箱
        string scenceItemID = "42001001";
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        float ranValue = Random.value;
        bool ranStatus = false;
       // GameObject scenceItemObj = Obj_Chest_1;
        switch (CreateType)
        {

            //低级宝箱
            case "1":
                if (ranValue <= 0.88f && ranStatus == false)
                {
                    scenceItemID = "42101001";
                    ranStatus = true;
                }
                if (ranValue <= 0.98f && ranStatus == false)
                {
                    scenceItemID = "42101002";
                    ranStatus = true;
                }
                if (ranValue <= 1f && ranStatus == false)
                {
                    scenceItemID = "42101003";
                    ranStatus = true;
                }
                break;

            //中级宝箱
            case "2":
                if (ranValue <= 0.1f && ranStatus == false)
                {
                    scenceItemID = "42101004";
                    ranStatus = true;
                }
                if (ranValue <= 0.6f && ranStatus == false)
                {
                    scenceItemID = "42101005";
                    ranStatus = true;
                }
                if (ranValue <= 0.8f && ranStatus == false)
                {
                    scenceItemID = "42101006";
                    ranStatus = true;
                }
                if (ranValue <= 1f && ranStatus == false)
                {
                    scenceItemID = "42101007";
                    ranStatus = true;
                }
                break;

            //高级宝箱
            case "3":
                scenceItemID = "42001008";
                //scenceItemObj = Obj_Chest_3;

                break;

            //高级宝箱
            case "4":
                scenceItemID = "42001009";
                //scenceItemObj = Obj_Chest_4;
                break;
        }

        //特殊处理（等级对应不同的宝箱ID）
        int addValue = 0;
        if (roseLv >= 18)
        {
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
        if (addValue != 0)
        {
            scenceItemID = (int.Parse(scenceItemID) + addValue).ToString();
        }

        //创建宝箱
        Vector3 vec3 = RetuenChestVec3();

        CreateChestObj(CreateType, scenceItemID, vec3);


    }

    public void CreateChestObj(string CreateType, string scenceItemID, Vector3 vec3) {

        //不在主城不创建
        if (Application.loadedLevelName != "EnterGame")
        {
            return;
        }

        GameObject scenceItemObj = Obj_Chest_1;
        switch (CreateType)
        {

            //低级宝箱
            case "1":
                scenceItemObj = Obj_Chest_1;
                break;

            //中级宝箱
            case "2":
                scenceItemObj = Obj_Chest_2;
                break;

            //高级宝箱
            case "3":
                scenceItemObj = Obj_Chest_3;
                GaoJiChestNum = GaoJiChestNum + 1;

                //高级宝箱超过3个 就不在创建高级宝箱
                if (GaoJiChestNum >= 4)
                {
                    return;
                }

                break;

            //高级宝箱
            case "4":
                scenceItemObj = Obj_Chest_4;
                break;
        }

        GameObject chestObj = (GameObject)Instantiate(scenceItemObj);
        chestObj.transform.SetParent(Obj_ChestSet.transform);
        chestObj.transform.localScale = new Vector3(1, 1, 1);
        chestObj.transform.position = vec3;

        //创建宝箱数据
        ChestXuHao = ChestXuHao + 1;
        chestObj.GetComponent<UI_GetherItem>().SceneItemType = "3";
        chestObj.GetComponent<UI_GetherItem>().ChestID = ChestXuHao;
        chestObj.GetComponent<UI_GetherItem>().SceneItmeID = scenceItemID;
        chestObj.GetComponent<UI_GetherItem>().ChestInitia();

        //添加宝箱集合
        BaoXiangData baoXiangData = new BaoXiangData();
        baoXiangData.BaoXiangID = scenceItemID;
        baoXiangData.BaoXiangVec3 = vec3;
        baoXiangData.BaoXiangType = CreateType;
        baoXiangData.BaoXiangObj = chestObj;

        //Debug.Log("添加宝箱序号:" + ChestXuHao);
        ChestSet.Add(ChestXuHao, baoXiangData);

        //播放特效  Eff_HuoDongBaoXiangShow
        GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + "Eff_HuoDongBaoXiangShow", typeof(GameObject));
        GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);

        //SkillObject_p.transform.position = monsterObj.transform.position;
        SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
        SkillObject_p.transform.SetParent(chestObj.transform);
        SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);

    }

    //随机获取一个坐标点
    public Vector3 RetuenChestVec3() {
        int maxValue = Obj_ChestPositionList.Length;
        int nowValue = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(1, maxValue);
        nowValue = nowValue - 1;
        Vector3 vec3 = new Vector3(Obj_ChestPositionList[nowValue].transform.position.x + 10 * Random.value - 5, Obj_ChestPositionList[nowValue].transform.position.y, Obj_ChestPositionList[nowValue].transform.position.z + 10 * Random.value - 5);
        return vec3;
    }


    //清理宝箱
    public void Clearn() {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_ChestSet);
    }

    //打开宝箱
    public void ChestOpen(int XuHaoID) {

        if (ChestSet.ContainsKey(XuHaoID)) {
            ChestSet.Remove(XuHaoID);
        }

        if (Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet.ContainsKey(XuHaoID))
        {
            Game_PublicClassVar.gameServerObj.HuoDong_BaoXiang_ChestSet.Remove(XuHaoID);
        }

    }


}

public class BaoXiangData {
    public string BaoXiangID;
    public string BaoXiangType;
    public Vector3 BaoXiangVec3;
    public GameObject BaoXiangObj;
}
