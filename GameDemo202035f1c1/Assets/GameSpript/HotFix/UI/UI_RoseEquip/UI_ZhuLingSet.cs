using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using UnityEngine.UI;

public class UI_ZhuLingSet : MonoBehaviour
{
    public GameObject Obj_ZhuLingSet;
    private ObscuredString ZhuLingIDStr;
    public ObscuredString NowZhuLingID;
    public GameObject ZhuLingShowListObj;
    public GameObject ZhuLingShowListObjSet;

    public GameObject ZhuLingName;
    public GameObject ZhuLingDes;
    public GameObject ZhuLingNeedLv;
    public GameObject ZhuLingJiHuoImg;

    public GameObject CostItem_1;
    public GameObject CostItem_2;
    public ObscuredString NeedItemIDCost_1;
    public ObscuredString NeedItemIDCost_2;
    public ObscuredInt NeedItemNumCost_1;
    public ObscuredInt NeedItemNumCost_2;
    public ObscuredInt NeedRoseLv;

    public ObscuredBool UpdateZhuLingShowStatus;
    public int UpdateZhuLingShowStatusNum;

    // Start is called before the first frame update
    void Start()
    {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_ZhuLingSet);

        NowZhuLingID = "10001";
        InitListShow();
        Init();

    }

    void InitListShow() {

        //初始化列表
        ZhuLingIDStr = "10001,10002,10003,10004,10005,10006,10007,10008,10009,10010,10011,10012,10013,10014,10015";

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(ZhuLingShowListObjSet);

        string[] ZhuLingIDList = ZhuLingIDStr.ToString().Split(',');
        for (int i = 0; i < ZhuLingIDList.Length; i++)
        {
            GameObject zhuLingSpace = (GameObject)Instantiate(ZhuLingShowListObj);
            zhuLingSpace.transform.SetParent(ZhuLingShowListObjSet.transform);
            zhuLingSpace.transform.localScale = new Vector3(1, 1, 1);
            zhuLingSpace.GetComponent<UI_ZhuLingShowList>().ParObj = this.gameObject;
            zhuLingSpace.GetComponent<UI_ZhuLingShowList>().ShowZhuLingID = ZhuLingIDList[i];
            zhuLingSpace.GetComponent<UI_ZhuLingShowList>().Init();
        }

    }

    //初始化
    public void Init() {

        NeedItemIDCost_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemID_1", "ID", NowZhuLingID, "ZhuLing_Template");
        NeedItemNumCost_1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemNum_1", "ID", NowZhuLingID, "ZhuLing_Template"));

        NeedItemIDCost_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemID_2", "ID", NowZhuLingID, "ZhuLing_Template");
        NeedItemNumCost_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostItemNum_2", "ID", NowZhuLingID, "ZhuLing_Template"));

        if (NeedItemNumCost_1 != 0 && NeedItemIDCost_1 != null)
        {
            CostItem_1.GetComponent<XiLianEquipNeedItem>().ItemID = NeedItemIDCost_1;
            CostItem_1.GetComponent<XiLianEquipNeedItem>().NeedItemNum = NeedItemNumCost_1;
            CostItem_2.GetComponent<XiLianEquipNeedItem>().ItemID = NeedItemIDCost_2;
            CostItem_2.GetComponent<XiLianEquipNeedItem>().NeedItemNum = NeedItemNumCost_2;

            CostItem_1.GetComponent<XiLianEquipNeedItem>().UpdateShow();
            CostItem_2.GetComponent<XiLianEquipNeedItem>().UpdateShow();

            string des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", NowZhuLingID, "ZhuLing_Template");
            ZhuLingDes.GetComponent<Text>().text = des;

            string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", NowZhuLingID, "ZhuLing_Template");
            ZhuLingName.GetComponent<Text>().text = name;
        }

        NeedRoseLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedLv", "ID", NowZhuLingID, "ZhuLing_Template"));
        ZhuLingNeedLv.GetComponent<Text>().text = "需要角色等级:" + NeedRoseLv;

        bool zhuLingStatus = Game_PublicClassVar.Get_function_Rose.IfRoseZhuLing(NowZhuLingID);
        if (zhuLingStatus)
        {
            ZhuLingJiHuoImg.SetActive(true);
        }
        else {
            ZhuLingJiHuoImg.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (UpdateZhuLingShowStatus) {
            UpdateZhuLingShowStatusNum = UpdateZhuLingShowStatusNum + 1;
            if (UpdateZhuLingShowStatusNum >= 2) {
                UpdateZhuLingShowStatusNum = 0;
                UpdateZhuLingShowStatus = false;
            }
        }
    }

    private void LateUpdate()
    {
        
    }


    //开始注灵
    public void Btn_ZhuLing() {

        //Debug.Log("点击开始注灵");
        bool ifJiHuo = Game_PublicClassVar.Get_function_Rose.IfRoseZhuLing(NowZhuLingID);
        if (ifJiHuo == true) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前已注灵成功,无需重复注灵!");
            return;
        }

        //判断等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < NeedRoseLv) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("角色等级不足,无法注灵!");
            return;
        }

        //检测背包
        bool itemStatus_1 = Game_PublicClassVar.Get_function_Rose.ReturnNeedBagItemNum(NeedItemIDCost_1, NeedItemNumCost_1);
        bool itemStatus_2 = Game_PublicClassVar.Get_function_Rose.ReturnNeedBagItemNum(NeedItemIDCost_2, NeedItemNumCost_2);

        if (itemStatus_1 && itemStatus_2)
        {
            //注灵成功,扣除道具
            Game_PublicClassVar.Get_function_Rose.CostBagItem(NeedItemIDCost_1, NeedItemNumCost_1);
            Game_PublicClassVar.Get_function_Rose.CostBagItem(NeedItemIDCost_2, NeedItemNumCost_2);

            //写入注灵数据
            Game_PublicClassVar.Get_function_Rose.AddRoseZhuLing(NowZhuLingID);
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("注灵成功!");
            ZhuLingJiHuoImg.SetActive(true);
            UpdateZhuLingShowStatus = true;
            Init();     //刷新显示
        }
        else {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("注灵失败,需要道具不足!");
        }

    }

    //关闭
    public void Btn_Close() {

        Destroy(this.gameObject);

    }
}
