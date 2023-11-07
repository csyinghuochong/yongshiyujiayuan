using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureKuangListSet : MonoBehaviour
{

    public Pro_PetListData ProPetListData;

    public GameObject Obj_PlayerName;
    public GameObject[] Obj_PlayerPetList;
    public GameObject Obj_PlayerHuoBi;
    public GameObject Obj_PlayerKuangImg;
    public GameObject Obj_PlayerKuangName;
    public ObscuredString petFightData;
    private ObscuredString kuangType;
    public ObscuredString petSpaceIDStr;
    public Sprite[] Obj_KuangImageList;
    public GameObject Obj_NowKuangImage;
    public ObscuredInt SendNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {

        SendNum = 0;    //初始化值

        if (ProPetListData.PetData == null || ProPetListData.PetData == "") {
            //如果没有宠物就默认
            ProPetListData.PetData = "0@10001020@10@100@500@0@矿脉守护者@0@0@80;15;50;30@0@6000@1500@1500@1500@1500@3000@0.6@|0@10001010@10@100@500@0@矿脉守护者@0@0@80;15;50;30@0@6000@1500@1500@1500@1500@3000@0.6@|0@10001020@10@100@500@0@矿脉守护者@0@0@80;15;50;30@0@6000@1500@1500@1500@1500@3000@0.6@";
        }

        string[] petDataList = ProPetListData.PetData.Split('|');

        //如果目标矿场的数据为空,就把前3个设置为目标
        if (ProPetListData.RankPetID == "" || ProPetListData.RankPetID == null) {
            if (petDataList.Length >= 3)
            {
                ProPetListData.RankPetID = "1;1,2,3;0";
            }
        }

        petFightData = "";

        Obj_PlayerName.GetComponent<Text>().text = ProPetListData.PetPlayerName;
        string nowPetKuangStr = ProPetListData.RankPetID;
        kuangType = "1";
        if (nowPetKuangStr != "" && nowPetKuangStr != null && nowPetKuangStr != "0")
        {
            string[] nowPetKuangList = nowPetKuangStr.Split(';');
            if (nowPetKuangList.Length >= 3)
            {
                kuangType = nowPetKuangList[0];
                petSpaceIDStr = nowPetKuangList[1];
                string[] petSpaceIDList = petSpaceIDStr.ToString().Split(',');
                for (int i = 0; i < petSpaceIDList.Length; i++)
                {
                    //获取头像数据进行显示
                    if (petSpaceIDList[i] != "" && petSpaceIDList[i] != "0" && petSpaceIDList[i] != null)
                    {

                        /*
                        string nowPetID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petSpaceIDList[i], "RosePet");
                        //显示头像
                        string nowHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", nowPetID, "Pet_Template");
                        //显示底图
                        Object obj = Resources.Load("PetHeadIcon/" + nowHeadIcon, typeof(Sprite));
                        Sprite img = obj as Sprite;
                        Obj_PlayerPetList[i].GetComponent<Image>().sprite = img;
                        */

                        ObscuredInt nowXuHao = int.Parse(petSpaceIDList[i]);

                        //Debug.Log("nowXuHao = " + nowXuHao);

                        if (petDataList.Length > nowXuHao)
                        {
                            string[] listData = petDataList[nowXuHao-1].Split('@');
                            if (listData.Length >= 10)
                            {
                                string nowPetID = listData[1];

                                //显示头像
                                string nowHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", nowPetID, "Pet_Template");
                                if (nowHeadIcon != "" && nowHeadIcon != "0" && nowHeadIcon != null)
                                {
                                    //显示底图
                                    Object obj = Resources.Load("PetHeadIcon/" + nowHeadIcon, typeof(Sprite));
                                    Sprite img = obj as Sprite;
                                    Obj_PlayerPetList[i].GetComponent<Image>().sprite = img;
                                }

                                petFightData = petFightData + petDataList[nowXuHao - 1] + "|";
                                //Debug.Log("petFightData = " + petFightData);
                            }
                        }
                    }
                }
            }
        }
        else {

            int xuhaoID = petDataList.Length;
            if (xuhaoID >= 3) {
                xuhaoID = 2;
            }

            //显示Icon
            for (int i = 0; i <= xuhaoID; i++)
            {
                //清空显示
                //Obj_PlayerPetList[i].GetComponent<Image>().sprite = null;
                if (petDataList.Length > i)
                {
                    string[] listData = petDataList[i].Split('@');
                    if (listData.Length >= 10)
                    {
                        string nowPetID = listData[1];

                        //显示头像
                        string nowHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", nowPetID, "Pet_Template");

                        if (nowHeadIcon != "" && nowHeadIcon != "0" && nowHeadIcon != null) {
                            //显示底图
                            Object obj = Resources.Load("PetHeadIcon/" + nowHeadIcon, typeof(Sprite));
                            Sprite img = obj as Sprite;
                            Obj_PlayerPetList[i].GetComponent<Image>().sprite = img;
                        }
                    }
                }
            }
        }

        if (petFightData != "") {
            petFightData = petFightData.ToString().Substring(0, petFightData.Length - 1);
        }


        Obj_PlayerKuangName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeName(kuangType);
        SendNum = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeZiYuan(kuangType);
        SendNum = (int)(Game_PublicClassVar.Get_function_Pasture.PastureKuang_KuangPro() * SendNum + 20000);
        Obj_PlayerHuoBi.GetComponent<Text>().text = SendNum.ToString();


        Obj_NowKuangImage.GetComponent<Image>().sprite = Obj_KuangImageList[int.Parse(kuangType) - 1];

    }

    //点击
    public void Btn_Click()
    {
        //Debug.Log("我点击了挑战按钮...");

        //判断当前是否拥有队伍,没有队伍需要配置队伍
        string petTiaoZhanTeam = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetTiaoZhanTeam", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanTeam == "" || petTiaoZhanTeam == "0" || petTiaoZhanTeam == ";;")
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("请打开排行榜中的宠物天梯处设置自己当前的出战队伍!");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStr);
            return;
        }

        //获取当前挑战次数
        string petTiaoZhanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (petTiaoZhanNum == "")
        {
            petTiaoZhanNum = "0";
        }

        //测试功能
        //petTiaoZhanNum = "0";
        if (int.Parse(petTiaoZhanNum) >= 5)
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("hint_440");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("今日掠夺次数已用完!");
            return;
        }

        int writeInt = int.Parse(petTiaoZhanNum) + 1;

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KuangLvDuoNum", writeInt.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

        //向服务器发送挑战信息（等服务器验证没有问题 由服务器发送消息过来进入挑战地图）


        //设置当前进入挑战信息

        if (petFightData != "")
        {
            Pro_PetListData nowPetListData = ProPetListData;
            nowPetListData.PetData = petFightData;
            Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData = nowPetListData;
        }
        else {
            Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData = ProPetListData;
        }

        //存入天梯相关数据
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoStatus = true;
        //Game_PublicClassVar.Get_gameLinkServerObj.LveDuoKuangSpace = SendNum;
        if (Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.RankVlaue != "" && Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.RankVlaue != null) {
            Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.RankVlaue = "0";
        }
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoKuangSpace = int.Parse(Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.RankVlaue);
        Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangZhangHaoID = Game_PublicClassVar.Get_gameLinkServerObj.PetTianTi_ProPetListData.PetTeamName;
        Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangType = kuangType;
        Game_PublicClassVar.Get_gameLinkServerObj.LvDuoKuangPetSpaceStr = petSpaceIDStr;
        //Debug.Log("掠夺:" + SendNum);
        Game_PublicClassVar.Get_gameLinkServerObj.LveDuoGoldNum = SendNum;


        //关闭排行界面
        //Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenPaiHangBang();  RankVlaue
        Destroy(Game_PublicClassVar.gameServerObj.Obj_KuangSet.transform.parent.gameObject);

        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002012, "1");   //掠夺一次重新刷新列表

        //测试进入地图
        Game_PublicClassVar.Get_function_Rose.RoseMoveTargetMap("PetTianTi");



    }

    public void Btn_ShowPet(string select)
    {




    }
}
