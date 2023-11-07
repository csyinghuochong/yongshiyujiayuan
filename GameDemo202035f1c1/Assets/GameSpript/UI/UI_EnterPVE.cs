using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EnterPVE : MonoBehaviour {

    private string ScenceID;         //进入场景ID
    public string SceneTransferID;



    public string ChapterSonID;
    public Vector3 ChapterSonPosition;
    public float WaitTime;
    private float WaitTimeSum;
    public bool WaitStatus;
    private bool UIRunStatus;           //UI动态状态开关
    private float UIPositionSum;
    public GameObject UI_ChapterSonName;
    public GameObject UI_ChapterSonNameOut;
    public bool IfOpen;                         //章节是否开启
    public GameObject UI_CostTiLi;

    //public GameObject Obj_UIEnterPveSet;
    //public GameObject UI_ChapterSonYes;           //章节开启
    //public GameObject UI_ChapterSonNo;           //章节关闭

	// Use this for initialization
	void Start () {

        if (IfOpen)
        {
            //获取章节名称
            string chapterName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonName", "ID", ChapterSonID, "ChapterSon_Template");
            string chapterNameOut = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChapterSonNameOut", "ID", ChapterSonID, "ChapterSon_Template");
            SceneTransferID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneIDSet", "ID", ChapterSonID, "ChapterSon_Template");
            

            //给UI赋值
            UI_ChapterSonName.GetComponent<Text>().text = chapterName;
            UI_ChapterSonNameOut.GetComponent<Text>().text = chapterNameOut;
        }
        else
        {
            this.gameObject.SetActive(false);
        }

        //初始化位置
        this.transform.localPosition = new Vector3(1000.0f, ChapterSonPosition.y, ChapterSonPosition.z);
        WaitStatus = true;

        //写入场景
        if (SceneTransferID != "" || SceneTransferID != "0")
        {
            ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");
            //Debug.Log("ScenceID = " + ScenceID);
            string costTiLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostTiLi", "ID", ScenceID, "Scene_Template");
            if (costTiLi == "0")
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("无消耗");
                UI_CostTiLi.GetComponent<Text>().text = langStr;
            }
            else
            {
                string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("消耗体力");
                UI_CostTiLi.GetComponent<Text>().text = langStr + "：" + costTiLi;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {

        if (WaitStatus)
        {
            WaitTimeSum = WaitTimeSum + Time.deltaTime;
            if (WaitTimeSum >= WaitTime)
            {
                UIRunStatus = true;
                WaitStatus = false;
            }
        }

        if (UIRunStatus)
        {
            UIPositionSum = UIPositionSum + Time.deltaTime;
            if (UIPositionSum >= 0.2f)
            {
                UIPositionSum = 0.2f;
                //Debug.Log("清空值");
            }
            this.transform.localPosition = new Vector3(1000.0f - (1000.0f - ChapterSonPosition.x) * UIPositionSum / 0.2f, ChapterSonPosition.y, ChapterSonPosition.z);

            if (UIPositionSum >= 0.2f)
            {
                UIRunStatus = false;
                UIPositionSum = 0.0f;
            }
        }
	}

    public void EnterPVE() {

        /*
        if (int.Parse(ChapterSonID) >= 5001) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("篝火测试目前只开放前四章！第五章地图暂不开放,敬请期待！");
            return;
        }
        */

        //判断等级进入
        string roseLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseLv", "ID", ScenceID, "Scene_Template");
        if (roseLvStr == "" || roseLvStr == null)
        {
            roseLvStr = "0";
        }

        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < int.Parse(roseLvStr)) {
            Debug.Log("进入此地图需要等级达到:" + roseLvStr);
            return;
        }

        //每次进入新地图设置血量为满的
        string roseHp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp.ToString();
        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("RoseHpNow", roseHp, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //写入场景
        ScenceID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MapID", "ID", SceneTransferID, "SceneTransfer_Template");

        //读取当前体力值
        string roseTiLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TiLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        //获取进入场景消耗体力
        int sceneTiLi = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CostTiLi", "ID", ScenceID, "Scene_Template"));
        int roseTiLi_Now = int.Parse(roseTiLi) - sceneTiLi;
        //roseTiLi_Now = 99999;

        if (roseTiLi_Now < 0)
        {
            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_131");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_132");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + sceneTiLi + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("体力不足" + sceneTiLi + "点,体力每5分钟恢复1点！");
            return;
        }
        else {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TiLi", roseTiLi_Now.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowMapName", ScenceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().DoorWayID = "1";
        //GameObject.Find("DoorWay").gameObject.GetComponent<DoorWay>().EnterGame();
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().SceneTransferID = SceneTransferID;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_EnterGame.GetComponent<UI_EnterGame>().EnterGame();

        //Obj_UIEnterPveSet
        Game_PublicClassVar.Get_function_UI.PlaySource("10003", "1");
        //隐藏BOSS关卡模型
        Game_PublicClassVar.Get_game_PositionVar.Obj_MonsterModelSheXiangJi.SetActive(false);

        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_MonsterModelistSet.SetActive(false);
    }
}
