using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChapterSonSet : MonoBehaviour {

    public GameObject Obj_EnterPve;
    public GameObject Obj_EnterPveSet;
    public string ChapterSonStr;    
    private int ChapterSonNum;
    public bool UpdataZhangJieStatus;
    private string[] chapterSonArrary;
    public int RoseData_ChapterSon;             //玩家子章节存储
    public bool AllChapterSonStatus;

    public string ChapterNum;
    public GameObject ShowMonsterNameObj;
    public GameObject ShowMonsterImageObj;
    public string ShowMonsterName;
    public GameObject Obj_OpenLvHint;
    //public GameObject Obj_UIEnterPveSet;

	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        chapterSonArrary = ChapterSonStr.Split(';');
        ChapterSonNum = chapterSonArrary.Length;
        updataEnterPveSet();
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdataZhangJieStatus) {
            UpdataZhangJieStatus = false;
            updataEnterPveSet();
        }
	}

    //更新
    void updataEnterPveSet() {

        chapterSonArrary = ChapterSonStr.Split(';');
        ChapterSonNum = chapterSonArrary.Length;
        Obj_OpenLvHint.SetActive(false);
        //当记录当前关卡时后面不予显示
        if (AllChapterSonStatus)
        {
            ChapterSonNum = RoseData_ChapterSon;
        }

        //判定删除子集下的物体
        for (int i = 0; i < Obj_EnterPveSet.transform.childCount; i++)
        {
            GameObject go = Obj_EnterPveSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //循环实例化
        for (int i = 0; i <= ChapterSonNum-1; i++)
        {
            //实例化章节OBJ
            GameObject obj = (GameObject)Instantiate(Obj_EnterPve);
            obj.transform.SetParent(Obj_EnterPveSet.transform);
            //obj.transform.localPosition = new Vector3(0f, 9999f, 0f);
            obj.GetComponent<UI_EnterPVE>().ChapterSonPosition = new Vector3(370, 300.0f + i* -125.0f, 0);
            obj.GetComponent<UI_EnterPVE>().WaitTime = 0.125f * i;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_EnterPVE>().ChapterSonID = chapterSonArrary[i];
            obj.GetComponent<UI_EnterPVE>().IfOpen = true;
            //Debug.Log("RoseData_ChapterSon = " + RoseData_ChapterSon + ", i = "+i);

        }

        //显示怪物模型信息
        switch (ChapterNum)
        {
            case "1":
                string monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70001905", "Monster_Template");
                ShowMonsterName = monsterName;
                break;
            case "2":
                monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70002903", "Monster_Template");
                ShowMonsterName = monsterName;
                break;
            case "3":
                monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70003904", "Monster_Template");
                ShowMonsterName = monsterName;
                break;
            case "4":
                monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70004904", "Monster_Template");
                ShowMonsterName = monsterName;
                break;
            case "5":
                monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70005905", "Monster_Template");
                ShowMonsterName = monsterName;
                break;
            case "6":
                monsterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterName", "ID", "70008903", "Monster_Template");
                ShowMonsterName = monsterName;
                Obj_OpenLvHint.SetActive(true);
                break;
        }
        //Debug.Log("ShowMonsterName = " + ShowMonsterName);
        ShowMonsterNameObj.GetComponent<Text>().text = ShowMonsterName;
        object obj2 = Resources.Load("CameraText/MonsterMolde_" + ChapterNum, typeof(Texture));
        Texture monsterImage = obj2 as Texture;
        //Obj_EquipQuality.GetComponent<Image>().sprite = itemQuality;
        ShowMonsterImageObj.GetComponent<RawImage>().texture = monsterImage;
    }
}
