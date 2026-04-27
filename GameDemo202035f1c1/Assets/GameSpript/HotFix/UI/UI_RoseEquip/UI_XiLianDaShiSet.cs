using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_XiLianDaShiSet : MonoBehaviour {


    public GameObject Obj_Total;
    public GameObject Obj_Type;

    public GameObject Obj_XiLianDaShiListShow;
    public GameObject XiLianDaShiListSet;

    public GameObject Obj_XiLianDaShiListShuLian;

    public GameObject Obj_ButtonLeft;
    public GameObject Obj_ButtonRight;
    public GameObject Obj_XiLianDashiItem;
    public GameObject Obj_XiLianDashiContainer;
    private List<string> mXilianDashiTitles;
    private List<UI_XiLianDaShi_ItemNew> mXiLianDashiItemList;

    private int currentIndex = -1;
    private bool bStartMove;
    private float targetPositionX;

    // Use this for initialization
    void Start () {
        mXilianDashiTitles = new List<string>();
        mXiLianDashiItemList = new List<UI_XiLianDaShi_ItemNew>();
        for (int i = 90001; i <= 90020; i++ )
        {
            mXilianDashiTitles.Add( i.ToString() );
        }
        ShowXiLianDaShi("1");
    }

    public void ShowXiLianDaShi(string showType) {
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(XiLianDaShiListSet);

        string hintTitleStr = "";

        //显示列表
        string xiLianDaShiIDSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (xiLianDaShiIDSetStr == "" || xiLianDaShiIDSetStr == "0")
        {
            xiLianDaShiIDSetStr = "0;0;0;0;0";
        }

        string[] xiLianDaShiIDSetList = xiLianDaShiIDSetStr.Split(';');
        string nowXiLianID = "10001;10002;10003";

        Obj_Total.SetActive(showType == "6");
        Obj_Type.SetActive(showType != "6");
        if (showType == "6")
        {
            UpdateXilianDashiTotal();
            return;
        }

        switch (showType) {
            case "1":
                nowXiLianID = "10001;10002;10003";
                hintTitleStr = "英勇洗炼";
                break;
            case "2":
                nowXiLianID = "20001;20002;20003";
                hintTitleStr = "海神洗炼";
                break;
            case "3":
                nowXiLianID = "30001;30002;30003";
                hintTitleStr = "漠灵洗炼";
                break;
            case "4":
                nowXiLianID = "40001;40002;40003";
                hintTitleStr = "光辉洗炼";
                break;
            case "5":
                nowXiLianID = "50001;50002;50003";
                hintTitleStr = "圣光洗炼";
                break;
        }

        hintTitleStr = Game_PublicClassVar.gameSettingLanguge.LoadLocalization(hintTitleStr);

        //显示熟练度
        string xiLianDaShiShuLianStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (xiLianDaShiShuLianStr == "" || xiLianDaShiShuLianStr == "0")
        {
            xiLianDaShiShuLianStr = "0;0;0;0;0";
        }
        string[] xilianDaShiShuLianList = xiLianDaShiShuLianStr.Split(';');
        int num = int.Parse(showType);
        num = num - 1;
        if (num >= 0)
        {
            string langStr_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("当前");
            string langStr_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("熟练度");

            int nowValue = int.Parse(xilianDaShiShuLianList[num]);
            Obj_XiLianDaShiListShuLian.GetComponent<Text>().text = langStr_1 + hintTitleStr + langStr_2 + ":<color=#2D882EFF>" + nowValue + "</color>";
        }



        string[] nowXiLianIDList = nowXiLianID.Split(';');
        for (int i = 0; i < nowXiLianIDList.Length; i++) {
            GameObject gameObj = (GameObject)Instantiate(Obj_XiLianDaShiListShow);
            gameObj.GetComponent<UI_XiLianDaShiListShow>().XiLianDaShiID = nowXiLianIDList[i];
            gameObj.GetComponent<UI_XiLianDaShiListShow>().XiLianXuHao = i;

            if (xiLianDaShiIDSetList[num].Contains(nowXiLianIDList[i])) {
                //表示已经激活
                gameObj.GetComponent<UI_XiLianDaShiListShow>().ifJIHuoStatus = true;
            }

            gameObj.transform.SetParent(XiLianDaShiListSet.transform);
            gameObj.transform.localScale = new Vector3(1, 1, 1);
            gameObj.GetComponent<UI_XiLianDaShiListShow>().showData();
        }
    }

    public void OnClickLeftButton()
    {
        if (bStartMove || currentIndex <= 0)
            return;

        currentIndex--;
        OnUpdateButton();
        SetXilianDashiPosition();
    }

    private void OnUpdateButton()
    {
         Obj_ButtonLeft.SetActive(currentIndex>0);
         Obj_ButtonRight.SetActive(currentIndex < this.mXiLianDashiItemList.Count-1);
    }

    public void OnClickRightButton()
    {
        if (bStartMove || currentIndex >= this.mXiLianDashiItemList.Count - 1)
            return;
        currentIndex++;
        OnUpdateButton();
        SetXilianDashiPosition();
    }

    private void SetXilianDashiPosition()
    {
        bStartMove = true;
        targetPositionX = currentIndex * -680 - 340;
    }

    // Update is called once per frame
    void Update()
    {
        if (bStartMove)
        {
            float current = Obj_XiLianDashiContainer.transform.localPosition.x;
            float delta = targetPositionX - current;
            if (Mathf.Abs(delta) < 1)
            {
                bStartMove = false;
            }

            float posx = 1200 * Time.deltaTime * (delta > 0 ? 1: -1);
            if (Mathf.Abs(posx) > Mathf.Abs(delta))
            {
                posx = delta;
            }
            current += posx;
            Obj_XiLianDashiContainer.transform.localPosition = new Vector3(current, 0, 0);
        }
    }

    private void UpdateXilianDashiTotal()
    {
        this.mXiLianDashiItemList = new List<UI_XiLianDaShi_ItemNew>();
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(this.Obj_XiLianDashiContainer);

        //最多只显示比自己高三个级别的称号
        int totalXilianValue = Game_PublicClassVar.Get_function_Rose.ReturnXiLianNum();
        int currentTitle = 0;

        for ( int i = 0; i < mXilianDashiTitles.Count; i++ )
        {
            string NeedXiLianValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", mXilianDashiTitles[i], "EquipXiLianDaShi_Template");
            if(totalXilianValue >= int.Parse(NeedXiLianValue) )
            {
                currentTitle = i;
            }
            if (i > (currentTitle + 3))
            {
                break;
            }

            GameObject item = (GameObject)MonoBehaviour.Instantiate(this.Obj_XiLianDashiItem);
            item.transform.SetParent(this.Obj_XiLianDashiContainer.transform);
            item.transform.localPosition = new Vector3(i * 680 + 340, 0, 0);
            item.transform.localScale = Vector3.one;
            mXiLianDashiItemList.Add(item.GetComponent<UI_XiLianDaShi_ItemNew>());

            mXiLianDashiItemList[i].UpdateData(mXilianDashiTitles[i], totalXilianValue);
        }

        //定位到当前激活的称号
        currentIndex = currentTitle;
        targetPositionX = currentIndex * -680 - 340;
        //若有问题，可以延迟一帧刷新
        Obj_XiLianDashiContainer.transform.localPosition = new Vector3(targetPositionX, 0, 0);

        OnUpdateButton();
    }
    
}
