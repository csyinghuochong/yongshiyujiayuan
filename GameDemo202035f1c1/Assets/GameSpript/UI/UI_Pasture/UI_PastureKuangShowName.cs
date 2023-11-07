using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_PastureKuangShowName : MonoBehaviour
{

    public ObscuredString KuangSpaceID;

    public GameObject Obj_KuangName;
    public GameObject Obj_KuangShouYi_Time;
    public GameObject Obj_KuangShouYi_Sum;
    public GameObject Obj_KuangLingQuTime;
    public GameObject Obj_KuangLvDuoDes;

    public GameObject Obj_HeChengXuanZe;
    public int NowSelectID;
    public ObscuredInt nowLvDuoNum;
    public ObscuredInt SendGoldNum;

    public GameObject[] Obj_PetHeadIcon;
    public GameObject[] Obj_PetName;

    public Sprite[] Obj_KuangImageList;
    public GameObject Obj_NowKuangImage;

    private bool FangZhiStatus;
    public ObscuredBool IfLingQuStatus;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //初始化
    public void Init() {

        lvDuoShow();

        string kuangDataStr = Game_PublicClassVar.Get_function_Pasture.PastuteKuang_GetKuangData(int.Parse(KuangSpaceID));
        if (kuangDataStr != "" && kuangDataStr != "0" && kuangDataStr != null)
        {

            string[] kuangDataList = kuangDataStr.Split(';');
            if (kuangDataList.Length >= 3)
            {

                string kuangType = kuangDataList[0];
                string kuangZhuShouSet = kuangDataList[1];
                string kuangTime = kuangDataList[2];

                Obj_KuangName.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeName(kuangType);
                int chanchuNum = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeZiYuan(kuangType);
                int shengchanHourNum = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeHourNum(kuangType);

                chanchuNum = (int)(Game_PublicClassVar.Get_function_Pasture.PastureKuang_KuangPro() * chanchuNum); 

                //获取收益(挖矿收益最多不能超过指定时间)
                float nowTimeHour = float.Parse(kuangTime) / 60.0f;
                if (nowTimeHour >= shengchanHourNum)
                {
                    nowTimeHour = shengchanHourNum;
                }

                Obj_KuangShouYi_Time.GetComponent<Text>().text = chanchuNum + "/小时";

                SendGoldNum = (int)(nowTimeHour * chanchuNum * (10 - nowLvDuoNum) * 0.1f);

                Obj_KuangShouYi_Sum.GetComponent<Text>().text = (SendGoldNum).ToString();  //+ " (-" + (nowLvDuoNum * 10) + "%)"

                if (nowTimeHour < shengchanHourNum)
                {

                    int fenzhongValueSum = (shengchanHourNum * 60) - int.Parse(kuangTime);
                    int hour = (fenzhongValueSum / 60);
                    int min = fenzhongValueSum - hour * 60;
                    if (hour == 0)
                    {
                        Obj_KuangLingQuTime.GetComponent<Text>().text = "领取剩余时间:" + min + "分钟";
                    }
                    else
                    {
                        Obj_KuangLingQuTime.GetComponent<Text>().text = "领取剩余时间:" + hour + "小时" + min + "分钟";
                    }
                }
                else
                {
                    Obj_KuangLingQuTime.GetComponent<Text>().text = "挖矿完成,可领取收益!";
                    IfLingQuStatus = true;
                }

                //显示矿类型图片
                Obj_NowKuangImage.GetComponent<Image>().sprite = Obj_KuangImageList[int.Parse(kuangType) - 1];
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未初始化挖矿数据...");
            Destroy(this.gameObject);
        }

        //展示宠物头像
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
        //for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
        //{

            string[] nowPastureKuangList = nowPastureKuangSetStrList[int.Parse(KuangSpaceID) - 1].Split(';');

            if (nowPastureKuangList.Length >= 3)
            {
                //设置驻守
                string[] petList = nowPastureKuangList[1].Split(',');
                for (int y = 0; y < petList.Length; y++)
                {
                    //显示宠物头像
                    if (petList[y] != "" && petList[y] != "0" && petList[y] != null) {

                        //获取宠物名称
                        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petList[y], "RosePet");

                        //获取宠物等级
                        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petList[y], "RosePet");

                        //获取宠物头像Icon
                        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petList[y], "RosePet");
                        string petHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petID, "Pet_Template");

                        //显示宠物信息
                        Obj_PetName[y].GetComponent<Text>().text = petName;

                        //显示底图
                        Object obj = Resources.Load("PetHeadIcon/" + petHeadIcon, typeof(Sprite));
                        Sprite img = obj as Sprite;
                        Obj_PetHeadIcon[y].GetComponent<Image>().sprite = img;
                    }
                }
            }


        //}
    }

    //掠夺
    public void lvDuoShow() {

        string KuangLvDuoDesStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("KuangLvDuoData", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

        if (KuangLvDuoDesStr == ""|| KuangLvDuoDesStr == "0"|| KuangLvDuoDesStr == null) {
            KuangLvDuoDesStr = "0@0@0";
        }

        string[] KuangLvDuoDesList = KuangLvDuoDesStr.Split('@');

        nowLvDuoNum = int.Parse(KuangLvDuoDesList[int.Parse(KuangSpaceID) - 1]);
        if (nowLvDuoNum >= 5) {
            nowLvDuoNum = 5;
        }

        if (nowLvDuoNum > 0)
        {
            Obj_KuangLvDuoDes.GetComponent<Text>().text = "被掠夺" + nowLvDuoNum + "次,收益降低" + (nowLvDuoNum * 10) + "%";
        }
        else {
            Obj_KuangLvDuoDes.GetComponent<Text>().text = "暂时无人掠夺!";
        }

    }

    //领取奖励
    public void Btn_LingQu() {

        //IfLingQuStatus = true;
        //SendGoldNum = 100000;

        if (IfLingQuStatus)
        {
            IfLingQuStatus = false;

            ObscuredString kuangDataStr = Game_PublicClassVar.Get_function_Pasture.PastuteKuang_GetKuangData(int.Parse(KuangSpaceID));
            if (kuangDataStr != "" && kuangDataStr != "0" && kuangDataStr != null)
            {
                string[] kuangDataList = kuangDataStr.ToString().Split(';');

                if (kuangDataList.Length >= 3)
                {
                    ObscuredString kuangType = kuangDataList[0];
                    ObscuredInt chanchuNum = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeZiYuan(kuangType);
                    ObscuredInt shengchanHourNum = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeHourNum(kuangType);

                    //ObscuredInt SendNum = chanchuNum * shengchanHourNum;

                    ObscuredInt SendNum = SendGoldNum;

                    //删除矿数据
                    Game_PublicClassVar.Get_function_Pasture.PastureKuang_Delete(int.Parse(KuangSpaceID));

                    //发送奖励
                    Game_PublicClassVar.Get_function_Rose.SendReward("1", SendNum.ToString());

                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("领取挖矿收益:" + SendNum);

                    //重新创建矿数据
                    Game_PublicClassVar.Get_function_Pasture.PastureKuang_Create();

                    if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>())
                    {
                        Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().InitKuangPosi();
                    }

                    Destroy(this.gameObject);

                }
            }
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待挖矿时间完成才可以领取喔!");
            return;
        }

    }

    public void Btn_XuanZePet(int select) {

        GameObject HeChengXuanZeObj = (GameObject)Instantiate(Obj_HeChengXuanZe);
        HeChengXuanZeObj.transform.SetParent(this.gameObject.transform.parent.transform);
        HeChengXuanZeObj.transform.localPosition = new Vector3(0, 0, 0);
        HeChengXuanZeObj.transform.localScale = new Vector3(1, 1, 1);
        //HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().HeChengWeiZhi = HeChengWeiZhi;
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().XuanZeType = 1;
        HeChengXuanZeObj.GetComponent<UI_PetHeChengXuanZe>().FuJiObj = this.gameObject;
        NowSelectID = select;
        FangZhiStatus = true;


    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }

    void OnDestroy()
    {
        if (FangZhiStatus) {
            FangZhiStatus = false;
            //发送服务器矿数据
            Pro_ComStr_4 com_4 = new Pro_ComStr_4();
            com_4.str_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002011, com_4);
        }
    }

}
