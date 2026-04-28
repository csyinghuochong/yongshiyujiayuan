using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_FindRose : MonoBehaviour {

    public GameObject Obj_Name;
    public GameObject Obj_ShenFenID;

    public GameObject Obj_FindRoseListData;
    public GameObject Obj_FindRoseListSet;

    public Pro_FindRoseList ProFindRoseList;

    public ObscuredString ShenFenName;
    public ObscuredString ShenFenID;

    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_gameServerObj.Obj_FindRose = this.gameObject;

        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {
            //不需要进行防沉迷验证,已经验证成功
            string xinghaoStr = "";
            for (int i = 0; i < name.Length - 1; i++)
            {
                xinghaoStr = xinghaoStr + "*";
            }

            //Obj_Name.GetComponent<InputField>().text = name.Substring(0, 1) + xinghaoStr;
            //Obj_ShenFenID.GetComponent<InputField>().text = shenfenID.Substring(0, 2) + "****************";

            Obj_Name.GetComponent<InputField>().text = name;
            Obj_ShenFenID.GetComponent<InputField>().text = shenfenID;
        }


        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_FindRoseListSet);

        //测试
        /*
        Pro_FindRoseList ProFindRoseList = new Pro_FindRoseList();
        Pro_FindRoseListData ProFindRoseListData = new Pro_FindRoseListData();

        ProFindRoseListData.Name = "测试角色";
        ProFindRoseListData.Occ = "1";
        ProFindRoseListData.Lv = "30";
        ProFindRoseListData.ServerName = "测试服务器";
        ProFindRoseListData.ZhangHaoID = "111111111";
        ProFindRoseList.ProFindRoseListData.Add(ProFindRoseListData);
        ProFindRoseList.ProFindRoseListData.Add(ProFindRoseListData);
        ProFindRoseList.ProFindRoseListData.Add(ProFindRoseListData);
        */



    }

    // Update is called once per frame
    void Update () {
		
	}


    //初始化
    public void Init()
    {

        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_FindRoseListSet);

        foreach (Pro_FindRoseListData nowProFindRoseListData in ProFindRoseList.ProFindRoseListData)
        {

            GameObject nowObj = (GameObject)Instantiate(Obj_FindRoseListData);
            nowObj.transform.SetParent(Obj_FindRoseListSet.transform);
            nowObj.transform.localScale = new Vector3(1, 1, 1);

            nowObj.GetComponent<UI_FindRoseList>().Name = nowProFindRoseListData.Name;
            nowObj.GetComponent<UI_FindRoseList>().Occ = nowProFindRoseListData.Occ;
            nowObj.GetComponent<UI_FindRoseList>().Lv = nowProFindRoseListData.Lv;
            nowObj.GetComponent<UI_FindRoseList>().ServerName = nowProFindRoseListData.ServerName;
            nowObj.GetComponent<UI_FindRoseList>().ZhangHaoID = nowProFindRoseListData.ZhangHaoID;
            nowObj.GetComponent<UI_FindRoseList>().Password = nowProFindRoseListData.ZhangHaoPassword;
            nowObj.GetComponent<UI_FindRoseList>().ShenFenName = ShenFenName;
            nowObj.GetComponent<UI_FindRoseList>().ShenFenID = ShenFenID;
            nowObj.GetComponent<UI_FindRoseList>().Init();
        }

    }


    public void Btn_ZhaoHui() {

        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        bool ifDown = false;

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {
            if (Obj_Name.GetComponent<InputField>().text == name)
            {
                if (Obj_ShenFenID.GetComponent<InputField>().text == shenfenID)
                {
                    ifDown = true;
                }
            }
        }
        else {

            string nowName = Obj_Name.GetComponent<InputField>().text;
            string nowShenFenID = Obj_ShenFenID.GetComponent<InputField>().text;

            if (nowName != null && nowShenFenID!=null && nowName != "" && nowShenFenID != "") {
                ifDown = true;
                name = nowName;
                shenfenID = nowShenFenID;
            }
        }

        if (ifDown) {

            //发送身份信息
            Pro_PlayerYanZheng proPlayerYanZheng = new Pro_PlayerYanZheng();
            proPlayerYanZheng.SheBeiID = SystemInfo.deviceUniqueIdentifier;
            proPlayerYanZheng.Name = name;
            proPlayerYanZheng.ShenFenID = shenfenID;

            //发送验证消息
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000141, proPlayerYanZheng);

            ShenFenName = name;
            ShenFenID = shenfenID;
        }
    }

    public void CloseUI() {

        if (Game_PublicClassVar.Get_wwwSet.QiangZhiDownRose) {
            return;
        }

        Destroy(this.gameObject);
    }
}
