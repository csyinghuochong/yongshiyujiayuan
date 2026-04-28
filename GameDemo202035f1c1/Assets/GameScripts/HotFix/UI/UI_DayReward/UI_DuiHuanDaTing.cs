using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_DuiHuanDaTing : MonoBehaviour
{

    //
    public GameObject Obj_DuiHuanTitle;         //兑换标题Obj
    public GameObject Obj_DuiHuanItem;
    public GameObject Obj_ShowParentSet;        //显示父级节点;


    private string[] honorStoreItemList;
    private string countryLv;

    // Use this for initialization
    void Start()
    {
        countryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        //初始化显示位置
        float showTitle_X = 0;
        float showTitle_Y = 0;
        float showItem_X = 0;
        float showItem_Y = 0;
        //获取兑换列表
        honorStoreItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "HonorStoreItemList", "GameMainValue").Split(';');
        Obj_ShowParentSet.GetComponent<RectTransform>().sizeDelta = new Vector2(1366, 235 * honorStoreItemList.Length);
        string position_Y = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HonorStorePosition_Y", "ID", countryLv, "Country_Template");
        Obj_ShowParentSet.transform.localPosition = new Vector3(0, float.Parse(position_Y), 0);
        for (int i = 0; i <= honorStoreItemList.Length - 1; i++) {
            bool ifshowBtn = true;
            if (honorStoreItemList[i] != "")
            {
                string countryLvlimit = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLvlimit", "ID", honorStoreItemList[i], "HonorStore_Template");
                //实例化标题
                GameObject duiHuanTitleObj = (GameObject)Instantiate(Obj_DuiHuanTitle);
                duiHuanTitleObj.transform.SetParent(Obj_ShowParentSet.transform);
                duiHuanTitleObj.transform.localPosition = new Vector3(showTitle_X, showTitle_Y, 0);
                duiHuanTitleObj.transform.localScale = new Vector3(1, 1, 1);
                showTitle_Y = showTitle_Y - 235.0f;


                if (int.Parse(countryLv) >= int.Parse(countryLvlimit))
                {
                    //显示激活图标
                    duiHuanTitleObj.GetComponent<UI_DuiHuanTitleObj>().Obj_UISign.SetActive(true);
                    duiHuanTitleObj.GetComponent<UI_DuiHuanTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "国家达到<color=#00ff00ff>(" + countryLv + "/" + countryLvlimit + ")</color>可激活兑换";
                }
                else {
                    //显示激活图标
                    object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                    Material huiMaterial = huiObj as Material;
                    duiHuanTitleObj.GetComponent<UI_DuiHuanTitleObj>().Obj_UISign.GetComponent<Image>().material = huiMaterial;
                    duiHuanTitleObj.GetComponent<UI_DuiHuanTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "国家达到<color=#ff0000ff>(" + countryLv + "/" + countryLvlimit + ")</color>可激活兑换";
                    ifshowBtn = false;
                }

                string nextID = honorStoreItemList[i];
                int showNum = 0;
                showItem_X = -500;
                showItem_Y = 50;

                int doNum = 0;
                //获取下级ID
                do
                {
                    //新建兑换道具
                    doNum = doNum + 1;
                    showNum = showNum + 1;
                    //单排显示10个
                    if (showNum <= 9)
                    {
                        GameObject duiHuanItemObj = (GameObject)Instantiate(Obj_DuiHuanItem);
                        duiHuanItemObj.transform.SetParent(duiHuanTitleObj.transform);
                        duiHuanItemObj.transform.localPosition = new Vector3(showItem_X, showItem_Y, 0);
                        duiHuanItemObj.transform.localScale = new Vector3(1, 1, 1);
                        duiHuanItemObj.GetComponent<UI_DuiHuanItemObj>().HonorStoreID = nextID;
                        duiHuanItemObj.GetComponent<UI_DuiHuanItemObj>().ifshow = ifshowBtn;
                        showItem_X = showItem_X + 150.0f;    //每个ItemObj有50像素间隔

                        if (showNum == 9)
                        {
                            //重置每行显示数据
                            showItem_X = 0;
                            showItem_Y = showItem_Y - 150;
                            showNum = 0;
                            showTitle_Y = showTitle_Y -235.0f; //增加标题高度
                        }
                    }

                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "HonorStore_Template");

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
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

}