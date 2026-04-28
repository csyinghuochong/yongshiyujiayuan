using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureShowDataViewSet : MonoBehaviour {

    public ObscuredString RosePastureID;

    private ObscuredString PastureID;
    private ObscuredString PasturUpLv;

    //UI相关空间
    public GameObject Obj_PastureName;
    public GameObject Obj_PastureRenKou;
    public GameObject Obj_PastureUpLvName;
    public GameObject Obj_PastureUpLvNextTime;
    public GameObject[] Obj_PastureQiangZhuangShow;
    public GameObject[] Obj_PastureFanZhiShow;
    public GameObject Obj_PastureDanName;
    public GameObject Obj_PastureDanNum;
    public GameObject Obj_PastureDanTime;
    public GameObject Obj_PastureSellGold;
    public GameObject Obj_PastureDes;
    public GameObject Obj_PastureItemIcon;

    // Use this for initialization
    void Start () {

        PastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", RosePastureID, "RosePasture");
        Init();

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PastureModelShowSet.SetActive(true);

    }

    // Update is called once per frame
    void Update () {
		
	}

    private void OnDestroy()
    {
        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_PastureModelShowSet.SetActive(false);
    }

    //初始化数据
    public void Init() {

        //获取怪物
        string nowModelID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", PastureID, "Pasture_Template");
        float nowShowPro = float.Parse( Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowImgPro", "ID", PastureID, "Pasture_Template"));
        
        GameObject monsterObj = Instantiate((GameObject)Resources.Load("3DModel/Pasture/" + nowModelID, typeof(GameObject)));

        //创建怪物
        if (monsterObj != null)
        {
            //清空显示
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_PastureModelPositionSet);
            //显示模型
            monsterObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_PastureModelPositionSet.transform);
            monsterObj.transform.localPosition = new Vector3(0,0,0);
            monsterObj.transform.localScale = new Vector3(nowShowPro, nowShowPro, nowShowPro);
            monsterObj.transform.localRotation = Quaternion.Euler(0, 45, 0);
            monsterObj.SetActive(false);
            monsterObj.SetActive(true);

            //删除目标身上的所有脚本
            Destroy(monsterObj.GetComponent<PastureAI>());
            Destroy(monsterObj.GetComponent<UnityEngine.AI.NavMeshAgent>());
        }

        //获取数据
        string pasName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", PastureID, "Pasture_Template");
        Obj_PastureName.GetComponent<Text>().text = pasName;

        string pasPeopleNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNum", "ID", PastureID, "Pasture_Template");
        Obj_PastureRenKou.GetComponent<Text>().text = pasPeopleNum;

        PasturUpLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PasturUpLv", "ID", RosePastureID, "RosePasture");
        Obj_PastureUpLvName.GetComponent<Text>().text = Game_PublicClassVar.function_Pasture.GetUpLvStatusName(PasturUpLv);

        string pasUpTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("UpTime", "ID", PastureID, "Pasture_Template");
        string[] pasUpTimeList = pasUpTime.Split(';');
        int nextPasturUpLv = int.Parse(PasturUpLv) + 1;
        if (nextPasturUpLv > 4) {
            Obj_PastureUpLvNextTime.GetComponent<Text>().text = "(已进行至最后阶段,不会生产)";
        }
        else {
            string nextTime = pasUpTimeList[nextPasturUpLv - 1];
            string nowTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Time", "ID", RosePastureID, "RosePasture");
            int chaValue = int.Parse(nextTime) - int.Parse(nowTime);
            if (chaValue >= 0) {
                int hous = (int)(chaValue / 3600);
                int fenzhong = (int)(chaValue % 3600/60);
                string nextName = Game_PublicClassVar.function_Pasture.GetUpLvStatusNameDes(nextPasturUpLv.ToString());
                if (hous >= 1)
                {
                    Obj_PastureUpLvNextTime.GetComponent<Text>().text = "（预计"+hous+"小时"+ fenzhong + "分钟进入"+ nextName + "）";
                }
                else {
                    Obj_PastureUpLvNextTime.GetComponent<Text>().text = "（预计"+ fenzhong + "分钟进入"+ nextName + "）";
                }
            }
            else {
                Obj_PastureUpLvNextTime.GetComponent<Text>().text = "";
            }
        }

        //显示强壮和繁殖
        string pasQiangZhuang = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangZhuang", "ID", RosePastureID, "RosePasture");
        string pasFanZhi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FanZhi", "ID", RosePastureID, "RosePasture");
        Debug.Log("pasQiangZhuang = " + pasQiangZhuang + ";pasFanZhi = " + pasFanZhi);
        showStat(Obj_PastureQiangZhuangShow, pasQiangZhuang);
        showStat(Obj_PastureFanZhiShow, pasFanZhi);

        //显示生产道具和次数
        string pasGetItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetItemID", "ID", PastureID, "Pasture_Template");
        string pasGetItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", pasGetItemID, "SceneItem_Template");
        string pasXiaDanNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiaDanNum", "ID", RosePastureID, "RosePasture");
        Obj_PastureDanName.GetComponent<Text>().text = pasGetItemName;
        Obj_PastureDanNum.GetComponent<Text>().text = pasXiaDanNum;
        string pasDropTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DropTime", "ID", PastureID, "Pasture_Template");
        Obj_PastureDanTime.GetComponent<Text>().text = "(每"+(int)(int.Parse(pasDropTime)/60)+ "分钟有概率生产一次)";
        //显示出售金币
        Obj_PastureSellGold.GetComponent<Text>().text = Game_PublicClassVar.Get_function_Pasture.PastureGetSellGold(RosePastureID).ToString();
        //显示牧场描述
        string pasDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", PastureID, "Pasture_Template");
        Obj_PastureDes.GetComponent<Text>().text = pasDes;

        //显示Icon
        string nowShowItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShowItemID", "ID", PastureID, "Pasture_Template");
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", nowShowItemID, "Item_Template");
        //string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_PastureItemIcon.GetComponent<Image>().sprite = itemIcon;

    }


    private void showStat(GameObject[] statList, string num) {

        if (num == ""|| num==null) {
            num = "0";
        }

        for (int i = 0; i < 5; i++) {
            if (i < int.Parse(num)) {
                statList[i].SetActive(true);
            }
            else
            {
                statList[i].SetActive(false);
            }
        }

    }

    public void CloseUI() {
        Destroy(this.gameObject);
    }

    public void Btn_ShowFanZhiJiYin()
    {

        string name = "繁殖基因说明";
        string des = "此属性越高,表示生产能力越强!";
        Game_PublicClassVar.Get_function_UI.ShowCommonTips_1(name, des);
    }

    public void Btn_ShowQiangZhuangJiYin()
    {

        string name = "强壮基因说明";
        string des = "此属性越高,表示出售生产的品质和出售的价格越高!";
        Game_PublicClassVar.Get_function_UI.ShowCommonTips_1(name, des);
    }

    public void Btn_CloseHint()
    {
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
    }
}
