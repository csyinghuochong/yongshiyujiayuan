using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureDuiHuanDaTing : MonoBehaviour
{

    //
    public GameObject Obj_DuiHuanTitle;         //兑换标题Obj
    public GameObject Obj_DuiHuanItem;
    public GameObject Obj_ShowParentSet;        //显示父级节点;


    private ObscuredString[] honorStoreItemList;
    private ObscuredString pastureLv;

    private float lang;

    // Use this for initialization
    void Start()
    {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        lang = 270;

        pastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

        //初始化显示位置
        float showTitle_X = 0;
        float showTitle_Y = 0;
        float showItem_X = 0;
        float showItem_Y = 0;

        //获取兑换列表
        string[] tttList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "PastureStoreItemList", "GameMainValue").Split(';');
        honorStoreItemList = new ObscuredString[tttList.Length];
        for (int i = 0; i < tttList.Length; i++) {
            honorStoreItemList[i] = tttList[i];
        }

        Obj_ShowParentSet.GetComponent<RectTransform>().sizeDelta = new Vector2(1366, lang * honorStoreItemList.Length);
        string position_Y = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position_Y", "ID", pastureLv, "PastureUpLv_Template");
        float posY = lang * honorStoreItemList.Length / 2 * -1;
        //Obj_ShowParentSet.transform.localPosition = new Vector3(0, float.Parse(position_Y), 0);
        Obj_ShowParentSet.transform.localPosition = new Vector3(0, posY, 0);
        for (int i = 0; i <= honorStoreItemList.Length - 1; i++) {
            bool ifshowBtn = true;
            if (honorStoreItemList[i] != "")
            {
                string pastureLvlimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLvlimit", "ID", honorStoreItemList[i], "PastureDuiHuanStore_Template");
                //实例化标题
                GameObject duiHuanTitleObj = (GameObject)Instantiate(Obj_DuiHuanTitle);
                duiHuanTitleObj.transform.SetParent(Obj_ShowParentSet.transform);
                duiHuanTitleObj.transform.localPosition = new Vector3(showTitle_X, showTitle_Y, 0);
                duiHuanTitleObj.transform.localScale = new Vector3(1, 1, 1);
                showTitle_Y = showTitle_Y - lang;

                int pastureLvInt = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", pastureLv, "PastureUpLv_Template"));

                if (pastureLvInt >= int.Parse(pastureLvlimit))
                {
                    //显示激活图标
                    duiHuanTitleObj.GetComponent<UI_PastureDuiHuanTitleObj>().Obj_UISign.SetActive(true);
                    duiHuanTitleObj.GetComponent<UI_PastureDuiHuanTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "家园达到<color=#00ff00ff>(" + pastureLvInt + "/" + pastureLvlimit + ")</color>可激活兑换";
                }
                else {
                    //显示激活图标
                    object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                    Material huiMaterial = huiObj as Material;
                    duiHuanTitleObj.GetComponent<UI_PastureDuiHuanTitleObj>().Obj_UISign.GetComponent<Image>().material = huiMaterial;
                    duiHuanTitleObj.GetComponent<UI_PastureDuiHuanTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "家园达到<color=#ff0000ff>(" + pastureLvInt + "/" + pastureLvlimit + ")</color>可激活兑换";
                    ifshowBtn = false;
                }

                string nextID = honorStoreItemList[i];
                int showNum = 0;
                showItem_X = -430;
                showItem_Y = 50;

                int doNum = 0;
                //获取下级ID
                do
                {
                    //新建兑换道具
                    doNum = doNum + 1;
                    showNum = showNum + 1;
                    //单排显示10个
                    if (showNum <= 6)
                    {
                        GameObject duiHuanItemObj = (GameObject)Instantiate(Obj_DuiHuanItem);
                        duiHuanItemObj.transform.SetParent(duiHuanTitleObj.transform);
                        duiHuanItemObj.transform.localPosition = new Vector3(showItem_X, showItem_Y, 0);
                        duiHuanItemObj.transform.localScale = new Vector3(1, 1, 1);
                        duiHuanItemObj.GetComponent<UI_PastureDuiHuanItemObj>().HonorStoreID = nextID;
                        duiHuanItemObj.GetComponent<UI_PastureDuiHuanItemObj>().ifshow = ifshowBtn;
                        showItem_X = showItem_X + 150.0f;    //每个ItemObj有50像素间隔

                        if (showNum == 9)
                        {
                            //重置每行显示数据
                            showItem_X = 0;
                            showItem_Y = showItem_Y - 150;
                            showNum = 0;
                            showTitle_Y = showTitle_Y - lang; //增加标题高度
                        }
                    }

                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "PastureDuiHuanStore_Template");

                    if (doNum > 99) {
                        nextID = "0";
                        doNum = 0;
                    }
                }
                while (nextID != "0");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    //关闭UI
    public void Btn_CloseUI() {
        //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

}