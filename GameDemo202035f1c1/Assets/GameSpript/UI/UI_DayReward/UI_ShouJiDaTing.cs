using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ShouJiDaTing : MonoBehaviour
{

    //
    public GameObject Obj_ShouJiTitle;         //兑换标题Obj
    public GameObject Obj_ShouJiItem;
    public GameObject Obj_ShowParentSet;        //显示父级节点;


    private string[] honorStoreItemList;
    private string roseLv;


    public bool testStatus;
    // Use this for initialization
    void Start()
    {
        //检验收集星数
        Game_PublicClassVar.Get_function_Rose.ShouJiJianYan();
        //获取角色等级
        roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv().ToString();
        //初始化显示位置
        float showTitle_X = 0;
        float showTitle_Y = 0;
        float showItem_X = 0;
        float showItem_Y = 0;
        //获取兑换列表
        honorStoreItemList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Value", "ID", "ShouJiItemList", "GameMainValue").Split(';');
        string startListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (startListStr == "0" || startListStr == "") {
            startListStr = "0;0;0;0;0;0";
        }
        string[] startList = startListStr.Split(';');
        //Obj_ShowParentSet.GetComponent<RectTransform>().sizeDelta = new Vector2(1366, 235 * honorStoreItemList.Length);
        Obj_ShowParentSet.GetComponent<RectTransform>().sizeDelta = new Vector2(1366, 11000);
        //string position_Y = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HonorStorePosition_Y", "ID", roseLv, "Country_Template");
        string position_Y = "-1500";
        Obj_ShowParentSet.transform.localPosition = new Vector3(0, float.Parse(position_Y), 0);
        //Debug.Log("honorStoreItemList.Length = " + honorStoreItemList.Length);
        for (int i = 0; i <= honorStoreItemList.Length - 1; i++) {
            bool ifshowBtn = true;
            if (honorStoreItemList[i] != "")
            {
                string openLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OpenLv", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");
                //实例化标题
                GameObject duiHuanTitleObj = (GameObject)Instantiate(Obj_ShouJiTitle);
                duiHuanTitleObj.transform.SetParent(Obj_ShowParentSet.transform);
                duiHuanTitleObj.transform.localPosition = new Vector3(showTitle_X, showTitle_Y+1600, 0);
                duiHuanTitleObj.transform.localScale = new Vector3(1, 1, 1);
                showTitle_Y = showTitle_Y - 235.0f;


                if (int.Parse(roseLv) >= int.Parse(openLv))
                {
                    //显示激活图标
                    //duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UISign.SetActive(true);
                    //duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "国家达到<color=#00ff00ff>(" + countryLv + "/" + countryLvlimit + ")</color>可激活兑换";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UITitleText.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterDes", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");
                }
                else {
                    //显示激活图标
                    object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                    Material huiMaterial = huiObj as Material;
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UISign.GetComponent<Image>().material = huiMaterial;
                    //duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UITitleText.GetComponent<Text>().text = "国家达到<color=#ff0000ff>(" + countryLv + "/" + countryLvlimit + ")</color>可激活兑换";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UITitleText.GetComponent<Text>().text = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterDes", "ID", honorStoreItemList[i], "ShouJiItemPro_Template") + "(未开启)";
                    ifshowBtn = false;
                }

                int rewardXingNum_1 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList1_StartNum", "ID", honorStoreItemList[i], "ShouJiItemPro_Template"));
                int rewardXingNum_2 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList2_StartNum", "ID", honorStoreItemList[i], "ShouJiItemPro_Template"));
                int rewardXingNum_3 = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList3_StartNum", "ID", honorStoreItemList[i], "ShouJiItemPro_Template"));

                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartValue_1.GetComponent<Text>().text = rewardXingNum_1.ToString();
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartValue_2.GetComponent<Text>().text = rewardXingNum_2.ToString();
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartValue_3.GetComponent<Text>().text = rewardXingNum_3.ToString();

                //显示属性
                int nowXingNum = int.Parse(startList[i]);                       //当前星星
                int maxXingNum = rewardXingNum_3;                               //最大星星
                float xingPro = (float)(nowXingNum / maxXingNum);

                //显示当前章节对应星级
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_UIXingNum.GetComponent<Text>().text = nowXingNum + "/" + maxXingNum;
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StarPro.GetComponent<Image>().fillAmount = xingPro;

                string rewardPro_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList1_Des", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");
                string rewardPro_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList2_Des", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");
                string rewardPro_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProList3_Des", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");

                if (nowXingNum >= rewardXingNum_1)
                {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_1.GetComponent<Text>().color = new Color(0.235f, 0.517f, 0.082f);
                    rewardPro_1 = rewardPro_1 + "(已激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_1.SetActive(true);
                }
                else {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_1.GetComponent<Text>().color = new Color(0.407f, 0.345f, 0.215f);
                    rewardPro_1 = rewardPro_1 + "(未激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_1.SetActive(false);
                }

                if (nowXingNum >= rewardXingNum_2)
                {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_2.GetComponent<Text>().color = new Color(0.235f, 0.517f, 0.082f);
                    rewardPro_2 = rewardPro_2 + "(已激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_2.SetActive(true);
                }
                else
                {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_2.GetComponent<Text>().color = new Color(0.407f, 0.345f, 0.215f);
                    rewardPro_2 = rewardPro_2 + "(未激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_2.SetActive(false);
                }

                if (nowXingNum >= rewardXingNum_3)
                {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_3.GetComponent<Text>().color = new Color(0.235f, 0.517f, 0.082f);
                    rewardPro_2 = rewardPro_2 + "(已激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_3.SetActive(true);
                }
                else
                {
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_3.GetComponent<Text>().color = new Color(0.407f, 0.345f, 0.215f);
                    rewardPro_3 = rewardPro_3 + "(未激活)";
                    duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_StartJiHuo_3.SetActive(false);
                }


                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_1.GetComponent<Text>().text = rewardPro_1;
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_2.GetComponent<Text>().text = rewardPro_2;
                duiHuanTitleObj.GetComponent<UI_ShouJiTitleObj>().Obj_Pro_3.GetComponent<Text>().text = rewardPro_3;

                string nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemListID", "ID", honorStoreItemList[i], "ShouJiItemPro_Template");
                int showNum = 0;
                showItem_X = -500;
                showItem_Y = 30;

                int doNum = 0;
                //获取下级ID
                do
                {
                    //新建兑换道具
                    doNum = doNum + 1;
                    showNum = showNum + 1;
                    //单排显示10个
                    if (showNum <= 7)
                    {
                        GameObject duiHuanItemObj = (GameObject)Instantiate(Obj_ShouJiItem);
                        duiHuanItemObj.transform.SetParent(duiHuanTitleObj.transform);
                        duiHuanItemObj.transform.localPosition = new Vector3(showItem_X, showItem_Y, 0);
                        duiHuanItemObj.transform.localScale = new Vector3(1, 1, 1);
                        duiHuanItemObj.GetComponent<UI_ShouJiItemObj>().HonorStoreID = nextID;
                        duiHuanItemObj.GetComponent<UI_ShouJiItemObj>().ifshow = ifshowBtn;
                        //Debug.Log("showItem_X = " + showItem_X + ";showNum = " + showNum);
                        showItem_X = showItem_X + 150.0f;    //每个ItemObj有50像素间隔

                        if (showNum == 7)
                        {

                            //重置每行显示数据
                            showItem_X = -500;
                            showItem_Y = showItem_Y - 180;
                            showNum = 0;

                            //Debug.Log("增加标题高度");
                            if (nextID != Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template")) {
                                string showNextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");
                                if (showNextID != "" && showNextID != "0")
                                {
                                showTitle_Y = showTitle_Y - 200.0f; //增加标题高度
                                }
                            }
                        }
                    }

                    nextID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NextID", "ID", nextID, "ShouJiItem_Template");

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

        if (testStatus) {
            testStatus = false;
            //Game_PublicClassVar.Get_game_PositionVar.dayClearnGameData();
            //Game_PublicClassVar.Get_function_Rose.AddShouJiItem("10030317");
            //Game_PublicClassVar.Get_function_Rose.ShouJiJianYan();
        }

    }


    //关闭UI
    public void Btn_CloseUI() {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().Close_UI();
        Destroy(this.gameObject);
    }

}