using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ZhangJie : MonoBehaviour {

    public string ZhangJieID;
    public Vector3 ZhangJiePosition;
    public float WaitTime;
    private float WaitTimeSum;
    public bool WaitStatus;
    private bool UIRunStatus;           //UI动态状态开关
    private float UIPositionSum;
    private float UIPositionTimeValue;
    public GameObject UI_ZhangJieName;
    public GameObject UI_ZhangJieNameOut;
    public GameObject UI_ZhangJieDi_Yes;
    public GameObject UI_ZhangJieDi_No;
    public bool IfOpen;                         //章节是否开启
    public GameObject UI_ZhangJieYes;           //章节开启
    public GameObject UI_ZhangJieNo;            //章节关闭
    public GameObject Obj_ChapterSet;
    public GameObject ZhangJieSet;
    public GameObject Obj_ChapterSonSet;
    public GameObject Obj_EnterPveSet;
    public GameObject Obj_LvOpenHint;

	// Use this for initialization
	void Start () {
        if (IfOpen)
        {
            UI_ZhangJieYes.SetActive(true);
            UI_ZhangJieNo.SetActive(false);

            //获取章节名称
            string chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterName", "ID", ZhangJieID, "Chapter_Template");
            string chapterNameOut = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterNameOut", "ID", ZhangJieID, "Chapter_Template");

            //给UI赋值
            UI_ZhangJieName.GetComponent<Text>().text = chapterName;
            UI_ZhangJieNameOut.GetComponent<Text>().text = chapterNameOut;

        }
        else {
            UI_ZhangJieYes.SetActive(false);
            UI_ZhangJieNo.SetActive(true);
        }

        string nowLanguage = PlayerPrefs.GetString("GameLanguageType");     //0表示中文 1表示英文
        string pathStr = "GameUI";
        if (nowLanguage == "1") {
            pathStr = "GameUI_EN";
        }
        object obj = Resources.Load(pathStr + "/Back/ZhangJie_" + ZhangJieID, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        UI_ZhangJieDi_Yes.GetComponent<Image>().sprite = itemIcon;
        UI_ZhangJieDi_No.GetComponent<Image>().sprite = itemIcon;

        //初始化位置
        this.transform.localPosition = new Vector3(ZhangJiePosition.x, -1000.0f, ZhangJiePosition.z);
        WaitStatus = true;
        //Debug.Log("章节:" + ZhangJieID + "  Y位置:" + ZhangJiePosition.y + "   fu:" + (-1000.0f - (-1000.0f - ZhangJiePosition.y)));
        UIPositionTimeValue = 0.2f;

        if (ZhangJieID == "6"&&Game_PublicClassVar.Get_function_Rose.GetRoseLv() >= 50)
        {
            Obj_LvOpenHint.SetActive(true);
        }
        else {
            Obj_LvOpenHint.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (WaitStatus) {
            WaitTimeSum = WaitTimeSum + Time.deltaTime;
            if (WaitTimeSum >= WaitTime)
            {
                UIRunStatus = true;
                WaitStatus = false;
            }
        }

        if (UIRunStatus) {
            UIPositionSum = UIPositionSum + Time.deltaTime;
            if (UIPositionSum >= UIPositionTimeValue)
            {
                UIPositionSum = UIPositionTimeValue;
                //Debug.Log("清空值");
            }
            this.transform.localPosition = new Vector3(ZhangJiePosition.x, -1000.0f - (-1000.0f - ZhangJiePosition.y) * UIPositionSum / UIPositionTimeValue, ZhangJiePosition.z);
            if (UIPositionSum >= UIPositionTimeValue)
            {
                UIRunStatus = false;
                UIPositionSum = 0.0f;
            }
        }
	}

    //点击章节
    public void ClickChapter() {

        //显示怪物模型
        Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterModelSheXiangJi.SetActive(true);

        //判定删除子集下的物体
        for (int i = 0; i < ZhangJieSet.transform.childCount; i++)
        {
            GameObject go = ZhangJieSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }


        string sceneIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", ZhangJieID, "Chapter_Template");
        Obj_ChapterSonSet.SetActive(true);
        Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().UpdataZhangJieStatus = true;
        Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().ChapterSonStr = sceneIDSet;
        Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().ChapterNum = ZhangJieID;
        //Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().ShowMonsterName = "怪物名称";
        Obj_EnterPveSet.GetComponent<UI_EnterPVESet>().UIPVEStatus = "1";
        

        string[] roseData_PveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');
        if (int.Parse(roseData_PveChapter[0]) > int.Parse(ZhangJieID)) {
            Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().AllChapterSonStatus = false;
        }

        if (int.Parse(roseData_PveChapter[0]) == int.Parse(ZhangJieID))
        {
            Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().AllChapterSonStatus = true;
            Obj_ChapterSonSet.GetComponent<UI_ChapterSonSet>().RoseData_ChapterSon = int.Parse(roseData_PveChapter[1]);
        }

        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");

    }
}
