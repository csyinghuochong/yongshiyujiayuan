using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;


//创建角色调用此脚本
public class UI_CreateRose : MonoBehaviour {

    //public bool ifGetData;
    public GameObject UI_NameObj;
    public GameObject UI_OccObj;


    public bool MoveStatus;                 //选中移动状态
    public bool MoveStatus_One;     
    public bool ReturnMoveStatus;           //取消选中回去的状态
    public bool ReturnMoveStatus_One;
    public float MoveStatusTime;

    public GameObject UI_SetObj_MoFaShi;                //选中魔法师
    public GameObject UI_SetObj_ZhanShi;                //选中战士
    public GameObject UI_SetObj_LieRen;                //选中战士
    public GameObject UI_NameObj_MoFaShi;               //选中魔法师名字
    public GameObject UI_NameObj_ZhanShi;               //选中战士名字
    public GameObject UI_NameObj_LieRen;               //选中战士名字
    public GameObject UI_SetObj_Sclect;                 //选中名称
    public GameObject UI_SetObj_SclectShowName;         //展示角色名称
    public GameObject UI_SetObj_SclectCreateName;       //创建角色名称
    public GameObject UI_InputName;                     //输入的角色名称


    public GameObject Obj_MoFaShi;
    public GameObject Obj_ZhanShi;
    public GameObject Obj_LieRen;
    public GameObject Obj_MoFaShiStartPosi;             //战士出生点位
    public GameObject Obj_ZhanShiStartPosi;             //法师出生点位
    public GameObject Obj_LieRenStartPosi;             //法师出生点位
    public GameObject Obj_MovePosi;                     //选择英雄后移动的位置
    public GameObject Obj_ReturnMovePosi;               //取消影响选择返回的位置
    public string sclectOcctype;                        //当前选择职业移动  类型  0：什么都没选 1:战士  2：法师
    public string returnOcctype;                        //取消选择职业返回  类型  0：什么都没选 1:战士  2：法师
    private GameObject sclectRoseObj;                   //当前选中的职业模型
    private GameObject returnRoseObj;                   //取消选择的职业模型
    public GameObject Obj_EffectSclect;                 //选中特效源文件
    public GameObject Obj_EffectSclectLoop;             //选中时循环播放的特效
    private GameObject effectSclectLoopObj;
    public GameObject Obj_EffectSclectZhanShi;      //选中特效播放坐标点：战士
    public GameObject Obj_EffectSclectFaShi;        //选中特效播放坐标点：法师
    private GameObject Obj_EffectSclectObj;         //当前选择的坐标点
    public GameObject Obj_ShowText_ZhanShi;
    public GameObject Obj_ShowText_MoFaShi;
    public GameObject Obj_ShowText_LieRen;
    public GameObject Obj_ShowText_LieRen_1;
    public GameObject Obj_ShowText_LieRen_2;

    public GameObject Obj_CreatRoseEneterServerName;        //创建角色进入服务器名称

    public GameObject UI_JiaZaiObj;
    private string returnNoRoseStr;           //没有角色返回通用字符

    //角色创建列表
    public GameObject Obj_CreateListObj;
    public GameObject Obj_CreateListObjSet;
    public GameObject[] Obj_CreateRoseListObj;

    public GameObject Obj_DeleteRoseHint;
    public GameObject Obj_DeleteRoseBtn;
    public GameObject Obj_FindRoseBtn;

    public GameObject Obj_CreateRoseSet;
    public GameObject Obj_CreateList;
    public GameObject Obj_CreateRight;
    public GameObject Obj_CreateLeft;
    public GameObject Obj_CreateName;

    public float QingQiuNameTime;
    public bool TeShuStatus;
    public bool TeShuStatusOne;
    private GameObject nowTeShuObj;
    private GameObject TeShuMoveObject;

    public GameObject Btn_RenZheng;

    // Use this for initialization
    void Start () {

        //界面适配
        //float scalePro = Game_PublicClassVar.Get_function_UI.ReturnScreenScalePro();
        //Obj_CreateList.transform.localPosition = new Vector3(Obj_CreateList.transform.localPosition.x * scalePro, Obj_CreateList.transform.localPosition.y, Obj_CreateList.transform.localPosition.z);
        //Obj_CreateRight.transform.localPosition = new Vector3(Obj_CreateRight.transform.localPosition.x * scalePro, Obj_CreateRight.transform.localPosition.y, Obj_CreateRight.transform.localPosition.z);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_CreateRoseSet);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_CreateList);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_CreateRight);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_CreateLeft);
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_CreateName);

        returnNoRoseStr = "点击创建角色";

        Btn_RenZheng.SetActive(!EventHandle.IsHuiWeiChannel());

        Game_PublicClassVar.gameServerObj.Obj_ServerEnterName = Obj_CreatRoseEneterServerName;

        /*
        string occValue = PlayerPrefs.GetString("OccType");
        if (occValue == "") { 
            //表示第一次进入游戏默认选个值
            PlayerPrefs.SetString("OccType", "1");
        }

        //Debug.Log("occValue = " + occValue);
        sclectOcctype = PlayerPrefs.GetString("OccType");            //默认值
        returnOcctype = "0";
        CreateRose(sclectOcctype,true);          
        updateNameShow();       //更新名称显示
        */

        UI_SetObj_Sclect.SetActive(false);

        //默认动作（战士,其他职业另加）
        Animator roseAnimator = Obj_ZhanShi.GetComponent<Animator>();
        //roseAnimator.Play("CreateIdle");


        sclectOcctype = "0";  //默认为0
        returnOcctype = "0";
        updateNameShow();       //更新名称显示


        //检测老账号
        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
        string createIDListStr = "";
        try
        {
            createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        }
        catch (Exception ex)
        {
            Debug.LogError("Xml_GetDate报错:" + ex);
            File.Delete(createXmlPath + "GameCreate.xml");
            createIDListStr = "";
            Game_PublicClassVar.Get_xmlScript.Xml_CreateNoKey(Application.persistentDataPath + "/GameData/" + "Xml" + "/Set_Xml/" + "GameCreate.xml", Game_PublicClassVar.Get_wwwSet.WWWSet_GameCreate);
        }
        //第一次进入（createIDListStr为空表示第一次进入）
        if (createIDListStr == "" || createIDListStr == "-1")
        {
            string set_XmlPath_10001 = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + "10001" + "/" + "GameConfig.xml";
            string saveStr = "";
            if (File.Exists(set_XmlPath_10001))
            {
                //创建第一个账号
                Game_PublicClassVar.Get_wwwSet.NowSelectFileName = "10001";
                saveStr = "10001,10001";
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateIDList", saveStr, "ID", "1", createXmlPath + "GameCreate.xml");
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", "10001", "ID", "1", createXmlPath + "GameCreate.xml");
            }
            
            string set_XmlPath_10002 = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + "10002" + "/" + "GameConfig.xml";

            if (File.Exists(set_XmlPath_10002))
            {
                //创建第一个账号
                //string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
                Game_PublicClassVar.Get_wwwSet.NowSelectFileName = "10002";
                if (saveStr != "")
                {
                    saveStr = saveStr + ";" + "10002,10001";
                }
                else
                {
                    saveStr = "10002,10001";
                }
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateIDList", saveStr, "ID", "1", createXmlPath + "GameCreate.xml");
                Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", "10002", "ID", "1", createXmlPath + "GameCreate.xml");
            }
        }

        //显示列表
        ShowCreateRoseList();
    }
	
	// Update is called once per frame
	void Update () {

        //选中移动角色
        if (MoveStatus) {

            //防止出错
            MoveStatusTime = MoveStatusTime + Time.deltaTime;
            if (MoveStatusTime >= 1.5f) {
                MoveStatusTime = 0;
                MoveStatus = false;
            }


            //设置移动的模型
            if (!MoveStatus_One) {
                MoveStatus_One = true;
                switch (sclectOcctype)
                {
                    case "1":
                        sclectRoseObj = Obj_ZhanShi;
                        Obj_EffectSclectObj = Obj_EffectSclectZhanShi;

                        //显示UI
                        //UI_NameObj.GetComponent<Text>().text = getRoseData(sclectOcctype);
                        UI_OccObj.GetComponent<Text>().text = "战士";
                        
                        UI_SetObj_ZhanShi.SetActive(false);

                        Obj_ShowText_ZhanShi.SetActive(true);
                        Obj_ShowText_MoFaShi.SetActive(false);
                        Obj_ShowText_LieRen.SetActive(false);
                        break;

                    case "2":
                        sclectRoseObj = Obj_MoFaShi;
                        Obj_EffectSclectObj = Obj_EffectSclectFaShi;
                        //显示UI
                        //UI_NameObj.GetComponent<Text>().text = getRoseData(sclectOcctype);
                        UI_OccObj.GetComponent<Text>().text = "魔法师";
                        UI_SetObj_MoFaShi.SetActive(false);

                        Obj_ShowText_ZhanShi.SetActive(false);
                        Obj_ShowText_MoFaShi.SetActive(true);
                        Obj_ShowText_LieRen.SetActive(false);

                        break;

                    case "3":
                        sclectRoseObj = Obj_LieRen;
                        Obj_EffectSclectObj = Obj_EffectSclectFaShi;
                        //显示UI
                        //UI_NameObj.GetComponent<Text>().text = getRoseData(sclectOcctype);
                        UI_OccObj.GetComponent<Text>().text = "狩猎者";
                        UI_SetObj_MoFaShi.SetActive(false);

                        Obj_ShowText_ZhanShi.SetActive(false);
                        Obj_ShowText_MoFaShi.SetActive(false);
                        Obj_ShowText_LieRen.SetActive(true);

                        break;
                }

                //设置移动方向
                sclectRoseObj.transform.LookAt(Obj_MovePosi.transform.position);
                //Debug.Log("转转转！");
                Animator roseAnimator = sclectRoseObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", true);
                //roseAnimator.SetBool("Idle", false);
                roseAnimator.SetBool("CreateWalk", true);
                roseAnimator.SetBool("CreateIdle", false);
                roseAnimator.SetBool("CreateRest", false);
                roseAnimator.Play("CreateWalk");

                //播放特效
                GameObject effectObject = (GameObject)Instantiate(Obj_EffectSclect);
                effectObject.transform.SetParent(Obj_EffectSclectObj.transform);
                effectObject.transform.localPosition = Vector3.zero;
                effectObject.transform.localScale = new Vector3(1, 1, 1);

                //UI_SetObj_Sclect.SetActive(false);

                PlayerPrefs.SetString("OccType", sclectOcctype);

                //选中的角色是否现实名称个创建角色名字的UI
                if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
                {
                    //创建游戏名称
                    UI_SetObj_SclectShowName.SetActive(false);
                    UI_SetObj_SclectCreateName.SetActive(true);
                }
                else
                {
                    //UI_SetObj_SclectShowName.SetActive(true);
                    UI_SetObj_SclectCreateName.SetActive(false);
                }

            }


            //float zCha = sclectRoseObj.transform.localPosition.z - Mathf.Abs(Obj_MovePosi.transform.localPosition.z);
            //Debug.Log("zCha:" + zCha);|| zCha>0.5f
            if (Vector3.Distance(sclectRoseObj.transform.position, Obj_MovePosi.transform.position) < 1f) {
                //Debug.Log("MoveStatusTime111 = " + MoveStatusTime);
                MoveStatusTime = 1.3f;
            }

            if (Vector3.Distance(sclectRoseObj.transform.position, Obj_MovePosi.transform.position) < 0.03f|| MoveStatus == false)
            {
                //Debug.Log("MoveStatusTime222 = " + MoveStatusTime);
                MoveStatus = false;
                MoveStatusTime = 0;
                MoveStatus_One = false;
                Animator roseAnimator = sclectRoseObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", false);
                //roseAnimator.SetBool("Idle", true);
                roseAnimator.SetBool("CreateWalk", false);
                roseAnimator.SetBool("CreateIdle", true);
                roseAnimator.SetBool("CreateRest", false);
                //roseAnimator.Play("CreateIdle");
                sclectRoseObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                //更新UI
                if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
                {
                    //显示创角后面的数据
                    updateNameShow();
                }
                else
                {
                    //隐藏创角后面的数据
                    updateNameShow(true);
                }

                //播放持续特效
                if (effectSclectLoopObj == null)
                {
                    effectSclectLoopObj = (GameObject)Instantiate(Obj_EffectSclectLoop);
                    effectSclectLoopObj.transform.SetParent(Obj_MovePosi.transform);
                    effectSclectLoopObj.transform.localPosition = new Vector3(0, 0.1f, -0.3f);
                    effectSclectLoopObj.transform.localScale = new Vector3(1, 1, 1);
                }

                //单独修正战士位置
                if (sclectOcctype == "1")
                {
                    sclectRoseObj.transform.localPosition = new Vector3(sclectRoseObj.transform.localPosition.x - 0.1f, sclectRoseObj.transform.localPosition.y, sclectRoseObj.transform.localPosition.z);
                }
            }
            else {
                //持续移动
                sclectRoseObj.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            }
        }

        //返回角色
        if (ReturnMoveStatus)
        {
            //设置移动的模型
            if (!ReturnMoveStatus_One)
            {
                ReturnMoveStatus_One = true;
                //Debug.Log("返回:" + returnOcctype);
                switch (returnOcctype)
                {
                    case "1":
                        returnRoseObj = Obj_ZhanShi;
                        Obj_ReturnMovePosi = Obj_ZhanShiStartPosi;
                        break;
                    case "2":
                        returnRoseObj = Obj_MoFaShi;
                        Obj_ReturnMovePosi = Obj_MoFaShiStartPosi;
                        break;
                    case "3":
                        returnRoseObj = Obj_LieRen;
                        if(sclectOcctype == "1")
                        {
                            Obj_ReturnMovePosi = Obj_ZhanShiStartPosi;
                        }
                        if (sclectOcctype == "2")
                        {
                            Obj_ReturnMovePosi = Obj_MoFaShiStartPosi;
                        }
                        
                        break;
                }

                //设置移动方向
                returnRoseObj.transform.LookAt(Obj_ReturnMovePosi.transform.position);
                Animator roseAnimator = returnRoseObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", true);
                //roseAnimator.SetBool("Idle", false);
                roseAnimator.SetBool("CreateWalk", true);
                roseAnimator.SetBool("CreateIdle", false);
                roseAnimator.SetBool("CreateRest", false);
                //roseAnimator.Play("CreateWalk");
                //Debug.Log("移动!");
            }


            if (Vector3.Distance(returnRoseObj.transform.position, Obj_ReturnMovePosi.transform.position) < 0.1f)
            {
                ReturnMoveStatus = false;
                ReturnMoveStatus_One = false;
                Animator roseAnimator = returnRoseObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", false);
                //roseAnimator.SetBool("Idle", true);
                roseAnimator.SetBool("CreateWalk", false);
                roseAnimator.SetBool("CreateIdle", false);
                roseAnimator.SetBool("CreateRest", true);
                //roseAnimator.Play("CreateIdle");
                returnRoseObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else {
                //持续移动
                returnRoseObj.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            }
        }


        if (TeShuStatus) {

            if (TeShuStatusOne == false) {

                TeShuStatusOne = true;

                if (sclectOcctype == "1")
                {
                    TeShuMoveObject = Obj_ZhanShiStartPosi;
                }

                if (sclectOcctype == "2")
                {
                    TeShuMoveObject = Obj_MoFaShiStartPosi;
                }

                nowTeShuObj = Obj_LieRen;

                //设置移动方向
                nowTeShuObj.transform.LookAt(TeShuMoveObject.transform.position);
                Animator roseAnimator = nowTeShuObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", true);
                //roseAnimator.SetBool("Idle", false);
                roseAnimator.SetBool("CreateWalk", true);
                roseAnimator.SetBool("CreateIdle", false);
                roseAnimator.SetBool("CreateRest", false);

            }

 

            if (Vector3.Distance(nowTeShuObj.transform.position, TeShuMoveObject.transform.position) < 0.1f)
            {
                TeShuStatus = false;
                //ReturnMoveStatus_One = false;
                Animator roseAnimator = nowTeShuObj.GetComponent<Animator>();
                //roseAnimator.SetBool("Run", false);
                //roseAnimator.SetBool("Idle", true);
                roseAnimator.SetBool("CreateWalk", false);
                roseAnimator.SetBool("CreateIdle", false);
                roseAnimator.SetBool("CreateRest", true);
                //roseAnimator.Play("CreateIdle");
                nowTeShuObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                //持续移动
                nowTeShuObj.transform.Translate(Vector3.forward * Time.deltaTime * 1.5f);
            }


        }


        //加载数据
        if (Game_PublicClassVar.Get_wwwSet.updataNumSum > 0) {
            UI_JiaZaiObj.SetActive(true);
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_451");
            if (langStr == "hint_451") {
                langStr = "加载游戏数据中";
            }
            UI_JiaZaiObj.GetComponent<Text>().text = langStr + "……  " + Game_PublicClassVar.Get_wwwSet.updataNumSum + "/" + (Game_PublicClassVar.Get_wwwSet.updataNum - 1).ToString(); 
        }


        //加载数据
        QingQiuNameTime = QingQiuNameTime + Time.deltaTime;
        if (QingQiuNameTime >= 10.0f)
        {
            //Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000121, "");
        }
        //刷新显示
        if (QingQiuNameTime >= 12.0f) {
            QingQiuNameTime = 0;
            ShowCreateEnterServerName();
        }

    }


    public void btn_createZhanShi() {

        UI_SetObj_Sclect.SetActive(false);

        Obj_ShowText_ZhanShi.SetActive(false);
        Obj_ShowText_MoFaShi.SetActive(false);
        Obj_ShowText_LieRen_1.SetActive(false);
        Obj_ShowText_LieRen_2.SetActive(false);

        if (sclectOcctype == "2")
        {
            TeShuStatus = true;
            TeShuStatusOne = false;
        }


        if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
        {
            CreateRose("1");

        }
        else
        {
            Debug.Log("账户下已经拥有角色无法选择角色！");
            btn_createLieRen();
        }



        
    }

    public void btn_createFaShi()
    {
        //Debug.Log("后续开启法师职业,敬请期待！");
        //return;

        UI_SetObj_Sclect.SetActive(false);

        Obj_ShowText_ZhanShi.SetActive(false);
        Obj_ShowText_MoFaShi.SetActive(false);
        Obj_ShowText_LieRen_1.SetActive(false);
        Obj_ShowText_LieRen_2.SetActive(false);

        if (sclectOcctype == "1")
        {
            TeShuStatus = true;
            TeShuStatusOne = false;
        }

        if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
        {
            CreateRose("2");
            //UI_SetObj_Sclect.SetActive(true);
        }
        else
        {
            Debug.Log("账户下已经拥有角色无法选择角色！");
        }



    }


    public void btn_createLieRen()
    {

        UI_SetObj_Sclect.SetActive(false);

        Obj_ShowText_ZhanShi.SetActive(false);
        Obj_ShowText_MoFaShi.SetActive(false);
        Obj_ShowText_LieRen_1.SetActive(false);
        Obj_ShowText_LieRen_2.SetActive(false);

        if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == "")
        {
            CreateRose("3");
        }
        else
        {
            Debug.Log("账户下已经拥有角色无法选择角色！");
        }
    }




    //创建角色
    //创建类型（1,战士  2,法师 3 猎人）
    public void CreateRose(string createType,bool firstGame = false) {

        //Debug.Log("createType = " + createType);
        /*
        if (sclectOcctype == createType)
        {

            return;
        }
        */


        if (!firstGame)
        {
            //如果当前选中的职业和已经选的职业相同,则不触发任何操作
            if (createType == sclectOcctype)
            {
                Debug.Log("选择相同职业");

                if (sclectOcctype != "3")
                {
                    btn_createLieRen();
                }
                else {
                    UI_SetObj_Sclect.SetActive(true);
                }

                return;
            }
        }

        //如果当前移动则点击无效
        
        if (MoveStatus) {
            return;
        }

        if (ReturnMoveStatus) {
            return;
        }
        
        //开启移动状态
        MoveStatus = true;

        //更改进入游戏的角色
        switch (createType)
        {
            case "1":
                Game_PublicClassVar.Get_wwwSet.RoseID = "10001";
                break;
            case "2":
                Game_PublicClassVar.Get_wwwSet.RoseID = "10001";
                break;
            case "3":
                Game_PublicClassVar.Get_wwwSet.RoseID = "10001";
                break;
        }

        //删除中心特效
        if (effectSclectLoopObj != null) {
            Destroy(effectSclectLoopObj);
        }

        /*
        //职业往回跑为0时,表示没有职业需要王辉跑
        if (returnOcctype == "0")
        {
            //职业往回跑
            returnOcctype = sclectOcctype;
            //return;
        }
        */

        //取消职业往回跑
        returnOcctype = sclectOcctype;
        if (sclectOcctype != "0")
        {
            ReturnMoveStatus = true;
        }

        //更新当前选中角色
        sclectOcctype = createType;



    }

    //获取当前角色数据
    private string getRoseData(string occ) {
        //显示UI
        string roseID = "10001";
        switch (occ) { 
            case "1":
                roseID = "10001";
            break;
            case "2":
                roseID = "10002";
                break;
        }

        string roseName = "";
        string roseLv = "";
        string firstGame = "";

        try
        {
            //此处加密以后不能读取此数据想一下怎么处理此处
            string set_XmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/" + roseID + "/";
            roseName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Name", "ID", roseID, set_XmlPath + "GameConfig.xml");
            roseLv = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("Lv", "ID", roseID, set_XmlPath + "GameConfig.xml");
            firstGame = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("FirstGame", "ID", roseID, set_XmlPath + "GameConfig.xml");
            
        }
        catch {
            Debug.Log("报错了");
            roseName = "角色数据错误,点击进入游戏修复！";
        }

        if (firstGame != "0")
        {
            string value = "Lv." + roseLv + "  " + roseName;
            return value;
        }
        else {
            string value = returnNoRoseStr;
            return value;
        }
    }

    public void updateNameShow(bool ifHideStatus = false)
    {
        //更新名称显示
        //UI_NameObj_ZhanShi.GetComponent<Text>().text = getRoseData("1");
        //UI_NameObj_MoFaShi.GetComponent<Text>().text = getRoseData("2");

        //Debug.Log("更新名称显示");

        if (!ifHideStatus)
        {
            switch (sclectOcctype)
            {
                case "0":
                    UI_SetObj_MoFaShi.SetActive(true);
                    UI_SetObj_ZhanShi.SetActive(true);
                    UI_SetObj_Sclect.SetActive(false);

                    break;

                case "1":
                    UI_SetObj_MoFaShi.SetActive(true);
                    UI_SetObj_ZhanShi.SetActive(false);
                    if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
                        UI_SetObj_Sclect.SetActive(true);
                    }

                    Obj_ShowText_LieRen_1.SetActive(true);
                    Obj_ShowText_LieRen_2.SetActive(false);

                    break;

                case "2":
                    UI_SetObj_MoFaShi.SetActive(false);
                    UI_SetObj_ZhanShi.SetActive(true);
                    if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
                    {
                        UI_SetObj_Sclect.SetActive(true);
                    }

                    Obj_ShowText_LieRen_1.SetActive(false);
                    Obj_ShowText_LieRen_2.SetActive(true);
                    break;

                case "3":
                    UI_SetObj_MoFaShi.SetActive(true);
                    UI_SetObj_ZhanShi.SetActive(true);
                    if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
                    {
                        UI_SetObj_Sclect.SetActive(true);
                    }

                    Obj_ShowText_LieRen_1.SetActive(false);
                    Obj_ShowText_LieRen_2.SetActive(false);
                    break;
            }


        }
        else
        {
            UI_SetObj_MoFaShi.SetActive(false);
            UI_SetObj_ZhanShi.SetActive(false);
            if (!Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                UI_SetObj_Sclect.SetActive(false);
            }

            UI_SetObj_Sclect.SetActive(true);
            UI_SetObj_SclectShowName.SetActive(true);
        }



        /*
        //修正物体在屏幕中的位置
        vec3_CreateZhanShi = Camera.main.WorldToViewportPoint(Obj_EffectSclectZhanShi.transform.position);
        Debug.Log("vec3_CreateZhanShi = " + vec3_CreateZhanShi);
        vec3_CreateZhanShi = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_CreateZhanShi);
        Debug.Log("vec3_CreateZhanShi = " + vec3_CreateZhanShi);
        Btn_CreateZhanShi.transform.position = vec3_CreateZhanShi;

        //修正物体在屏幕中的位置
        vec3_CreateMoFaShi = Camera.main.WorldToViewportPoint(Obj_EffectSclectFaShi.transform.position);
        Debug.Log("vec3_CreateMoFaShi = " + vec3_CreateMoFaShi);
        vec3_CreateMoFaShi = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(vec3_CreateZhanShi);
        Debug.Log("vec3_CreateMoFaShi = " + vec3_CreateMoFaShi);
        Btn_CreateMoFaShi.transform.position = vec3_CreateMoFaShi;
        */
    }




    //显示角色列表
    public void ShowCreateRoseList()
    {

        //清空
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_CreateListObjSet);

        //显示角色列表
        string createXmlPath = Application.persistentDataPath + "/GameData/"+ "Xml" +"/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        //Debug.Log("createIDListStr = " + createIDListStr);
        //显示当前选择
        Game_PublicClassVar.Get_wwwSet.NowSelectFileName = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateNowID", "ID", "1", createXmlPath + "GameCreate.xml");

        string[] createIDList = createIDListStr.Split(';');
        int createAddRose = 0;
        if (createIDList[0] != "")
        {
            //循环显示
            for (int i = 0; i < createIDList.Length; i++)
            {
                string[] createRoseIDList = createIDList[i].Split(',');
                string saveFileName = createRoseIDList[0];
                string saveFileRoseID = createRoseIDList[1];

                GameObject obj = (GameObject)Instantiate(Obj_CreateListObj);
                obj.transform.SetParent(Obj_CreateListObjSet.transform);
                obj.GetComponent<UI_CreateRoseListObj>().RoseFileName = saveFileName;
                obj.GetComponent<UI_CreateRoseListObj>().roseID = saveFileRoseID;
                obj.GetComponent<UI_CreateRoseListObj>().Obj_CreatePar = this.gameObject;
                obj.GetComponent<UI_CreateRoseListObj>().updateShow();
                obj.transform.localScale = new Vector3(1, 1, 1);
                createAddRose = createAddRose + 1;
                Obj_CreateRoseListObj[i] = obj;

                //显示选中状态
                if (Game_PublicClassVar.Get_wwwSet.NowSelectFileName == saveFileName)
                {
                    obj.GetComponent<UI_CreateRoseListObj>().SelectStatus = true;
                    obj.GetComponent<UI_CreateRoseListObj>().Btn_Select();     //进入界面显示选择项
                    //Debug.Log("更新界面选择!!!");
                }
            }
        }

        int maxCreateNum = 5;
        int createNullListNum = maxCreateNum - createAddRose;
        if (createNullListNum >= 1)
        {
            for (int i = createAddRose; i < maxCreateNum; i++)
            {
                GameObject obj = (GameObject)Instantiate(Obj_CreateListObj);
                obj.transform.SetParent(Obj_CreateListObjSet.transform);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.GetComponent<UI_CreateRoseListObj>().RoseFileName = "";
                obj.GetComponent<UI_CreateRoseListObj>().roseID = "";
                obj.GetComponent<UI_CreateRoseListObj>().Obj_CreatePar = this.gameObject;
                obj.GetComponent<UI_CreateRoseListObj>().updateShow();

                Obj_CreateRoseListObj[i] = obj;
            }
        }

        //如果都为空,显示第一个为选中状态
        if (createNullListNum >= maxCreateNum)
        {
            Obj_CreateRoseListObj[0].GetComponent<UI_CreateRoseListObj>().SelectStatus = true;
            Obj_CreateRoseListObj[0].GetComponent<UI_CreateRoseListObj>().Btn_Select();

        }

    }


    //更新选择
    public void UpdateSelect(GameObject selectObj)
    {

        for (int i = 0; i < Obj_CreateRoseListObj.Length; i++)
        {
            if (Obj_CreateRoseListObj[i] != null)
            {
                if (selectObj == Obj_CreateRoseListObj[i])
                {
                    Obj_CreateRoseListObj[i].GetComponent<UI_CreateRoseListObj>().SelectStatus = true;
                }
                else
                {
                    Obj_CreateRoseListObj[i].GetComponent<UI_CreateRoseListObj>().SelectStatus = false;
                }
            }
        }

    }


    public void Btn_DeleteRose()
    {

        GameObject uiCommonHint = (GameObject)Instantiate(Obj_DeleteRoseHint);
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_25");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint, DeletRose, null);
        uiCommonHint.transform.SetParent(this.gameObject.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);

    }


    //删除数据
    public void DeletRose()
    {
        //显示列表
        string createXmlPath = Application.persistentDataPath + "/GameData/Xml/Set_Xml/";
        string createIDListStr = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("CreateIDList", "ID", "1", createXmlPath + "GameCreate.xml");
        string deleteFileID = "";
        string[] createIDList = createIDListStr.Split(';');
        string firstID = "";
        string writeStr = "";
        if (createIDList[0] != "")
        {
            //循环显示
            for (int i = 0; i < createIDList.Length; i++)
            {
                string[] createRoseIDList = createIDList[i].Split(',');
                string saveFileName = createRoseIDList[0];
                string saveFileRoseID = createRoseIDList[1];

                if (saveFileName == Game_PublicClassVar.Get_wwwSet.NowSelectFileName)
                {

                    deleteFileID = saveFileName;

                    //删除文件夹
                    string fileStr = createXmlPath + saveFileName;
                    if (Directory.Exists(fileStr))
                    {
                        Directory.Delete(fileStr, true);
                    }

                    fileStr = createXmlPath + saveFileName + "_beifen";
                    if (Directory.Exists(fileStr))
                    {
                        Directory.Delete(fileStr, true);
                    }

                    /*
                    
                    if (File.Exists(fileStr))
                    {
                        //如果有子文件删除文件
                        File.Delete(fileStr);
                        Console.WriteLine(fileStr);
                    }
                    else
                    {
                        //循环递归删除子文件夹
                        DeleteDir(fileStr);
                    }
                    */
                }
                else
                {
                    writeStr = writeStr + createIDList[i] + ";";
                    if (firstID == "")
                    {
                        firstID = saveFileName;
                    }
                }
            }
        }
        if (writeStr != "")
        {
            writeStr = writeStr.Substring(0, writeStr.Length - 1);
        }

        //存储
        Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateIDList", writeStr, "ID", "1", createXmlPath + "GameCreate.xml");
        Game_PublicClassVar.Get_xmlScript.Xml_SetDate("CreateNowID", firstID, "ID", "1", createXmlPath + "GameCreate.xml");

        //重新显示
        ShowCreateRoseList();

        //重新存储验证文件值
        if (deleteFileID != "") {
            PlayerPrefs.SetString("YanZhengFileStr_" + deleteFileID, "");
            PlayerPrefs.SetString("iosPay_" + deleteFileID, "");
            string ChangeKey = PlayerPrefs.GetString("ChangeKey_" + deleteFileID);
            if (ChangeKey == "change")
            {
                PlayerPrefs.SetString("ChangeKey_" + deleteFileID, "");
            }
            PlayerPrefs.Save();
        }
    }

    //展示进入最新服务器的名称
    public void ShowCreateEnterServerName(){

        if (Game_PublicClassVar.gameServerObj.NowServerName != "" && Game_PublicClassVar.gameServerObj.NowServerName != null)
        {
            Obj_CreatRoseEneterServerName.GetComponent<Text>().text = Game_PublicClassVar.gameServerObj.NowServerName;
        }
        else {
            Obj_CreatRoseEneterServerName.GetComponent<Text>().text = "";
        }
        
    }

    //随机姓名
    public void Btn_RandomName() {

        string randomNameStr = "";
        if (Game_PublicClassVar.gameSettingLanguge.ranNameNum >= 2) {
            int xingXuHaoMax = Game_PublicClassVar.gameSettingLanguge.randomName_xing.Length - 1;
            int nameXuHaoMax = Game_PublicClassVar.gameSettingLanguge.randomName_name.Length - 1;
            int xingXuHao = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, xingXuHaoMax);
            int nameXuHao = Game_PublicClassVar.Get_function_Rose.ReturnRamdomValue_Int(0, xingXuHaoMax);
            randomNameStr = Game_PublicClassVar.gameSettingLanguge.randomName_xing[xingXuHao] + Game_PublicClassVar.gameSettingLanguge.randomName_name[nameXuHao];
        }

        if (randomNameStr != "") {
            UI_InputName.GetComponent<InputField>().text = randomNameStr;
        }

    }
}
