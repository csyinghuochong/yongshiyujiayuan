using UnityEngine;
using System.Collections;

public class UI_EnterPVESet : MonoBehaviour {

    public GameObject UI_ZhangJieObj;
    public GameObject UI_ZhangJieSet;
    public int ChapterNum;
    public bool UpdataZhangJieStatus;
    public GameObject UI_ChapterSonSet;
    public string UIPVEStatus;                  //0:章节界面  1：关卡界面
    private string[] roseData_PveChapter;       //玩家关卡保存数据

    public GameObject Obj_PveSetObj;

	// Use this for initialization
	void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_PveSetObj);
        
        UIPVEStatus = "0";
        updataZhangJie();

    }
	
	// Update is called once per frame
	void Update () {

        if (UpdataZhangJieStatus) {
            updataZhangJie();
            UpdataZhangJieStatus = false;
        }
	}

    private void OnDestroy()
    {
        //关闭摄像机
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi != null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_MonsterModelistSet.SetActive(false);
        }
        
    }

    void updataZhangJie() {

        roseData_PveChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData").Split(';');

        //判定删除子集下的物体
        for (int i = 0; i < UI_ZhangJieSet.transform.childCount; i++)
        {
            GameObject go = UI_ZhangJieSet.transform.GetChild(i).gameObject;
            Destroy(go);
        }

        //循环实例化
        for (int i = 1; i <= ChapterNum; i++)
        {
            //实例化章节OBJ
            GameObject obj = (GameObject)Instantiate(UI_ZhangJieObj);
            obj.transform.SetParent(UI_ZhangJieSet.transform);
            obj.GetComponent<UI_ZhangJie>().ZhangJiePosition = new Vector3(0, 280.0f + (i - 1) * -145.0f, 0);
            obj.GetComponent<UI_ZhangJie>().WaitTime = 0.125f * i;
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ZhangJie>().ZhangJieID = i.ToString();
            obj.GetComponent<UI_ZhangJie>().ZhangJieSet = UI_ZhangJieSet;
            obj.GetComponent<UI_ZhangJie>().Obj_ChapterSonSet = UI_ChapterSonSet;
            obj.GetComponent<UI_ZhangJie>().Obj_EnterPveSet = this.gameObject;

            //根据章节显示位置
            UI_ZhangJieSet.transform.localPosition = new Vector3(UI_ZhangJieSet.transform.localPosition.x, -120, UI_ZhangJieSet.transform.localPosition.z);
            string pveStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (pveStr != "" && pveStr != "0" && pveStr != null) {
                string[] pveList = pveStr.Split(';');
                if (pveList.Length >= 2) {
                    if (int.Parse(pveList[0]) >= 6) {
                        UI_ZhangJieSet.transform.localPosition = new Vector3(UI_ZhangJieSet.transform.localPosition.x, 60, UI_ZhangJieSet.transform.localPosition.z);
                    }
                }
            }

            //判定玩家当前存档是否打到此关卡
            if (int.Parse(roseData_PveChapter[0]) >= i)
            {
                obj.GetComponent<UI_ZhangJie>().IfOpen = true;
            }
            else {
                obj.GetComponent<UI_ZhangJie>().IfOpen = false;
            }
        }

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_MonsterModelistSet.SetActive(true);


    }

    public void Btn_Close() {
        //隐藏怪物模型
        Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterModelSheXiangJi.SetActive(true);
        switch (UIPVEStatus) { 
            case "0":
                this.gameObject.SetActive(false);
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().ifHideMainUI = false;
                //判定删除子集下的物体
                for (int i = 0; i < UI_ZhangJieSet.transform.childCount; i++)
                {
                    GameObject go = UI_ZhangJieSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                break;

            case "1":
                UpdataZhangJieStatus = true;
                //判定删除子集下的物体
                for (int i = 0; i < UI_ChapterSonSet.GetComponent<UI_ChapterSonSet>().Obj_EnterPveSet.transform.childCount; i++)
                {
                    GameObject go = UI_ChapterSonSet.GetComponent<UI_ChapterSonSet>().Obj_EnterPveSet.transform.GetChild(i).gameObject;
                    Destroy(go);
                }
                UIPVEStatus = "0";
                UI_ChapterSonSet.SetActive(false);
                break;
        }

        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_MonsterModelistSet.SetActive(false);
    }
}
