using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PastureFirendListSet : MonoBehaviour {

    public GameObject Obj_FriendSet;
    public GameObject Obj_FriendListSet;
    public GameObject Obj_FriendShowList;
    public GameObject Obj_FirendJiaoHuNumShow;
    public GameObject Obj_FirendSuoFangImg;
    public GameObject Obj_FirendUpdateTime;
    public bool IfHind;
    public bool moveStatus;
    public float pos_x;
    public float sumTime;
    public bool sendStatus;
    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);
        Game_PublicClassVar.Get_gameServerObj.Obj_JiaoHuListSet = this.gameObject;

        string firendUpdateTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendUpdateTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (firendUpdateTime == "" || firendUpdateTime == "0" || firendUpdateTime == null) {
            //请求数据
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002001, "");
            sendStatus = true;
        }
        else {
            sumTime = float.Parse(firendUpdateTime);
        }

        //初始化隐藏
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_BtnFightingSet.SetActive(false);

        Init();



    }
	
	// Update is called once per frame
	void Update () {

        if (moveStatus) {
            if (IfHind)
            {
                pos_x = pos_x + 310 * Time.deltaTime * 2;
                if (pos_x >= 310)
                {
                    pos_x = 310;
                    moveStatus = false;
                    Obj_FirendSuoFangImg.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
            else {
                pos_x = pos_x - 310 * Time.deltaTime * 2;
                if (pos_x <= 0)
                {
                    pos_x = 0;
                    moveStatus = false;
                    Obj_FirendSuoFangImg.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }

            Obj_FriendSet.transform.localPosition = new Vector3(pos_x, Obj_FriendSet.transform.localPosition.y, Obj_FriendSet.transform.localPosition.z);
        }

        //当时间为0时 开始刷新
        if (sendStatus == false) {
            sumTime = sumTime - Time.deltaTime;
            if (sumTime <= 0)
            {
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002001, "");
                Obj_FirendUpdateTime.GetComponent<Text>().text = "正在请求服务器刷新数据...";
                sendStatus = true;
            }
            else
            {
                //显示时间
                int min = (int)(sumTime / 60);
                int sec = (int)(sumTime % 60);
                Obj_FirendUpdateTime.GetComponent<Text>().text = min + "分" + sec + "秒";
            }
        }

	}


    public void Init() {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_FriendListSet);

        //展示信息
        string PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendList", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (PastureFirendListStr == "")
        {
            //请求数据
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002001, "");
            return;
            //需要重新生成一些牧场数据,或者再次请求服务器打开heng
            //Game_PublicClassVar.Get_function_Pasture.PastureCreateJiaoHuData();
            PastureFirendListStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirendList", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        }

        string[] PastureFirendList = PastureFirendListStr.Split('@');

        for (int i = 0; i < PastureFirendList.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_FriendShowList);
            obj.transform.SetParent(Obj_FriendListSet.transform);
            //obj.GetComponent<UI_PastureFirendListShow>().FirendDataStr = "10001;0;小小法师;2;12;1;0.2;0.3";           //唯一ID,是否交互,名字,职业,农场等级,打扫概率,探索概率,偷摸概率
            obj.GetComponent<UI_PastureFirendListShow>().FirendDataStr = PastureFirendList[i];           //唯一ID,是否交互,名字,职业,农场等级,打扫概率,探索概率,偷摸概率
            obj.GetComponent<UI_PastureFirendListShow>().Init();
            obj.GetComponent<UI_PastureFirendListShow>().Obj_Par = this.gameObject;
        }

        //展示
        Show();
    }

    public void Show() {

        //展示当前交互次数
        string jiaohuNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JiaoHuNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        if (jiaohuNum == "")
        {
            jiaohuNum = "0";
        }

        Obj_FirendJiaoHuNumShow.GetComponent<Text>().text = jiaohuNum + "/" + "10";

    }

    //刷新
    public void Btn_ShuaXin() {



    }

    //缩放
    public void Btn_SuoFang() {

        if (moveStatus) {
            return;
        }

        if (IfHind == false)
        {
            moveStatus = true;
            IfHind = true;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_BtnFightingSet.SetActive(true);
        }
        else {
            moveStatus = true;
            IfHind = false;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_BtnFightingSet.SetActive(false);
        }

    }
}
