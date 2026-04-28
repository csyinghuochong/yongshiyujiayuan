using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PastureSet : MonoBehaviour {

    public GameObject Obj_PastureSet;
    public GameObject Obj_PastureSceneSet;
    public Dictionary<string,GameObject> NowPastureObjList = new Dictionary<string, GameObject>();
    public Vector3 PastureInitVec3;

    public GameObject KuangCreateEffect;
    public GameObject[] KuangObjList;
    public GameObject[] KuangPosiObj;
    public GameObject Obj_KuangWeiKaiFa;
    public bool ShowKuangTitleStatus;

    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj = this.gameObject;

        PastureInitVec3 = Obj_PastureSet.transform.position;

        string PastureLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (PastureLvStr == "")
        {
            //PastureLvStr = "10001";
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PastureLv", "10001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RosePastureData");
        }

        //测试用的创建一个牧场动物
        //Game_PublicClassVar.Get_function_Pasture.CreatePastureAI("10002");
        //Game_PublicClassVar.Get_function_Pasture.CreatePastureAI("10002");
        //创建有没有新的蛋掉落
        Game_PublicClassVar.Get_function_Pasture.PastureUpdateDrop();
        //更新家园订单
        Game_PublicClassVar.Get_function_Pasture.UpdatePastureTrader();

        //隐藏任务栏,显示牧场栏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RoseTask.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainRosePastureData.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainRosePastureData.GetComponent<UI_MainRosePastureData>().ShowData();

        //隐藏战斗功能
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightBtnSet.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightPastureBtnSet.SetActive(true);

        //初始化创建牧场
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePastureMaxNum; i++) {

            string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", i.ToString(), "RosePasture");
            if (nowPastureID != "" && nowPastureID != "0" && nowPastureID != null) {

                //获取怪物
                string nowModelID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", nowPastureID, "Pasture_Template");
                GameObject shiliObj = (GameObject)Resources.Load("3DModel/Pasture/" + nowModelID, typeof(GameObject));
                if (shiliObj != null) {
                    GameObject monsterObj = Instantiate(shiliObj);
                    //创建怪物
                    if (monsterObj != null)
                    {
                        string vec3Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position", "ID", i.ToString(), "RosePasture");
                        if (vec3Str == "" || vec3Str == "0")
                        {
                            vec3Str = "0,0,0";
                        }
                        string[] vec3StrList = vec3Str.Split(',');
                        monsterObj.transform.SetParent(Obj_PastureSet.transform);
                        monsterObj.transform.localPosition = new Vector3(float.Parse(vec3StrList[0]), float.Parse(vec3StrList[1]), float.Parse(vec3StrList[2]));
                        monsterObj.transform.localScale = new Vector3(1, 1, 1);
                        monsterObj.SetActive(false);
                        monsterObj.SetActive(true);
                        monsterObj.GetComponent<PastureAI>().RosePastureID = i.ToString();

                        //添加记录
                        if (NowPastureObjList.ContainsKey(i.ToString()))
                        {
                            NowPastureObjList.Remove(i.ToString());
                        }
                        NowPastureObjList.Add(i.ToString(), monsterObj);
                    }
                }
            }
        }



        //创建掉落
        string xiaDanData = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiaDanData", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string[] dropItemStrList = xiaDanData.Split('#');
        if (dropItemStrList.Length >= 1)
        {
            for (int i = 0; i < dropItemStrList.Length; i++)
            {
                if (dropItemStrList[i] != "" && dropItemStrList[i] != null)
                {
                    string[] nowDropDataList = dropItemStrList[i].Split(';');
                    if (nowDropDataList.Length >= 3) {
                        string weiyiID = nowDropDataList[0];
                        string scenceID = nowDropDataList[1];
                        string[] scencePosiStrList = nowDropDataList[2].Split(',');
                        string qiangZhuang = "1";
                        if (nowDropDataList.Length>=4) {
                            qiangZhuang = nowDropDataList[3];
                        }

                        if (qiangZhuang == "") {
                            qiangZhuang = "0";
                        }

                        //读取掉落模型ID
                        string modelName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelName", "ID", scenceID, "SceneItem_Template");
                        GameObject monsterObj = Instantiate((GameObject)Resources.Load("3DModel/SceneItemObj/" + modelName, typeof(GameObject)));
                        if (monsterObj != null)
                        {
                            monsterObj.transform.SetParent(Obj_PastureSceneSet.transform);
                            float posY = float.Parse(scencePosiStrList[1]);
                            if (posY < 1.1f) {
                                posY = 1.1f;
                            }
                            monsterObj.transform.localPosition = new Vector3(float.Parse(scencePosiStrList[0]), posY, float.Parse(scencePosiStrList[2]));
                            monsterObj.transform.localScale = new Vector3(1, 1, 1);
                            monsterObj.SetActive(false);
                            monsterObj.SetActive(true);
                            monsterObj.GetComponent<UI_GetherItem>().SceneItemPastureID = weiyiID;
                            monsterObj.GetComponent<UI_GetherItem>().SceneItmeID = scenceID;
                            monsterObj.GetComponent<UI_GetherItem>().SceneItemPastureQiangZhuangValue = float.Parse(qiangZhuang);
                        }
                    }
                }
            }
        }


        //创建矿点
        /*
        string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowPastureKuangSetStr == "" || nowPastureKuangSetStr == null || nowPastureKuangSetStr == "0")
        {
            Game_PublicClassVar.Get_function_Pasture.PastureKuang_Create();
        }
        else {
            
            string[] pastureKuangSetList = nowPastureKuangSetStr.Split('@');

            for (int i = 0; i < pastureKuangSetList.Length; i++) {
                if (i == 0) {
                    if (pastureKuangSetList[i] == "0" || pastureKuangSetList[i] == "" || pastureKuangSetList[i] == null) {
                        Game_PublicClassVar.Get_function_Pasture.PastureKuang_Create();
                    }
                }
            }
        }
        */

        //初始化矿数据
        Game_PublicClassVar.Get_function_Pasture.PastureKuang_Create();

        //创建自身矿点
        InitKuangPosi();

    }

	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        //显示战斗功能
        /*
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightBtnSet.SetActive(true);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FightPastureBtnSet.SetActive(false);
        */
    }


    //矿属性
    public void InitKuangPosi() {

        //创建自身矿点
        for (int i = 1; i <= KuangPosiObj.Length; i++)
        {
            //清理目标物体
            Game_PublicClassVar.Get_function_UI.DestoryTargetObj(KuangPosiObj[i - 1]);

            string kuangDataStr = Game_PublicClassVar.Get_function_Pasture.PastuteKuang_GetKuangData(i);
            if (kuangDataStr != "" && kuangDataStr != "0" && kuangDataStr != null)
            {
                string[] kuangDataList = kuangDataStr.Split(';');
                if (kuangDataList.Length >= 3)
                {
                    string kuangType = kuangDataList[0];
                    GameObject obj = (GameObject)Instantiate(KuangObjList[int.Parse(kuangType) - 1]);
                    obj.transform.SetParent(KuangPosiObj[i - 1].transform);
                    obj.transform.localPosition = new Vector3(0, 0, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.GetComponent<PastureKuangSet>().KuangSpaceID = i.ToString();

                    //创建特效  矿的时间为0表示为新矿
                    if (kuangDataList[2] == "0")
                    {
                        GameObject effectObj = (GameObject)Instantiate(KuangCreateEffect);
                        effectObj.gameObject.transform.position = KuangPosiObj[i - 1].transform.position;
                        Destroy(effectObj, 3);
                    }
                }
            }
            else
            {
                //Debug.Log("创建未开发的矿...");
                //创建未开发的矿
                GameObject obj = (GameObject)Instantiate(Obj_KuangWeiKaiFa);
                obj.transform.SetParent(KuangPosiObj[i - 1].transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<PastureKuangSet>().KuangSpaceID = i.ToString();
                obj.GetComponent<PastureKuangSet>().WeiKaiFaStatus = true;
            }

        }

        //发送服务器矿数据
        Pro_ComStr_4 com_4 = new Pro_ComStr_4();
        com_4.str_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002011, com_4);

        //请求被掠夺数据
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002014, "");

    }

    public void AddPastureObj(string addPastureID) {


        string nowPastureID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureID", "ID", addPastureID, "RosePasture");
        if (nowPastureID != "" && nowPastureID != "0" && nowPastureID != null)
        {

            //获取怪物
            string nowModelID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", nowPastureID, "Pasture_Template");
            GameObject monsterObj = Instantiate((GameObject)Resources.Load("3DModel/Pasture/" + nowModelID, typeof(GameObject)));
            //创建怪物
            if (monsterObj != null)
            {
                string vec3Str = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Position", "ID", addPastureID, "RosePasture");
                if (vec3Str == "" || vec3Str == "0")
                {
                    vec3Str = "0,0,0";
                }

                string[] vec3StrList = vec3Str.Split(',');
                monsterObj.transform.SetParent(Obj_PastureSet.transform);
                monsterObj.transform.localPosition = new Vector3(float.Parse(vec3StrList[0]), float.Parse(vec3StrList[1]), float.Parse(vec3StrList[2]));
                monsterObj.transform.localScale = new Vector3(1, 1, 1);
                monsterObj.SetActive(false);
                monsterObj.SetActive(true);
                monsterObj.GetComponent<PastureAI>().RosePastureID = addPastureID;

                //添加记录
                if (NowPastureObjList.ContainsKey(nowPastureID))
                {
                    NowPastureObjList.Remove(nowPastureID);
                }
                NowPastureObjList.Add(nowPastureID, monsterObj);
            }
        }
    }

}
