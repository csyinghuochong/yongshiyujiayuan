using System.Collections;
using System.Collections.Generic;
using TapTap.Login;
using UnityEngine;
using UnityEngine.UI;

public class UI_StartGameFunc : MonoBehaviour {

    public GameObject Obj_GongGaoSet;
    private GameObject GongGaoSetObj;
    public GameObject Obj_ServerListShowSet;
    public GameObject Obj_BtnServerList;
    public GameObject Obj_RenZhengSet;
    public GameObject Obj_IphoneSet;
    public GameObject Obj_FindRoseSet;
    public GameObject FindRoseSetObj;
    public GameObject Obj_CreateRoseSet;
    public GameObject Obj_CreateRoseBtnSet;
    public GameObject Obj_UISet;
    public GameObject Obj_ShowBanHao;
    public GameObject Obj_HuaWeiYinSi;
    public GameObject Obj_FangChenMiBtn;
    public GameObject Obj_FangChenMiBtn2;
    public GameObject Obj_BtnFindBtn;
    public GameObject Obj_YinSiSet;
    public GameObject Obj_QuanXianHint;
    public GameObject ZhuXiaoZhangHaoObj;           //注销账号
    // Use this for initialization
    void Start () {

        Game_PublicClassVar.Get_gameServerObj.Obj_BtnServerList = Obj_BtnServerList;
        Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc = this.gameObject;

        //初始化认证自身是否绑定身份证
        /*
        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");
        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {

        }
        else
        {
            Btn_OpenRenZheng();
        }
        */

        //更新华为
        if (EventHandle.IsHuiWeiChannel())
        {
            Obj_ShowBanHao.SetActive(true);
            Obj_FangChenMiBtn.SetActive(false);
            Obj_FangChenMiBtn2.SetActive(false);
            Obj_BtnFindBtn.SetActive(false);
        }

        //打开隐私协议
        if (PlayerPrefs.GetString(YinSi.PlayerPrefsYinSi) != YinSi.YinSiValue)
        {
            Obj_HuaWeiYinSi.SetActive(true);
        }
        else
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            GameObject.Find("WWW_Set/TapTapSdk").GetComponent<TapTapSdkHelper>().TapInit_1();
#endif
        }

        if (Application.platform == RuntimePlatform.Android)
        {
            if (ZhuXiaoZhangHaoObj != null)
            {
                ZhuXiaoZhangHaoObj.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		


	}

    //展示公告
    public void Btn_ShowGongGao() {

        GongGaoSetObj = (GameObject)Instantiate(Obj_GongGaoSet);
        GongGaoSetObj.transform.SetParent(this.transform.parent);
        GongGaoSetObj.transform.localScale = new Vector3(1, 1, 1);
        GongGaoSetObj.transform.localPosition = new Vector3(0, 0, 0);
        GongGaoSetObj.GetComponent<UI_GameGongGao>().Obj_HintText.GetComponent<Text>().text = Game_PublicClassVar.Get_gameLinkServerObj.GameGongGaoStr;
        GongGaoSetObj.GetComponent<UI_GameGongGao>().Obj_HintTitleName.GetComponent<Text>().text = "游戏公告";

    }

    //展示服务器列表
    public void Btn_ShowServerList() {
        Obj_ServerListShowSet.SetActive(true);
    }

    //打开认证
    public void Btn_OpenRenZheng() {

        Debug.Log("打开认证...");

        GongGaoSetObj = (GameObject)Instantiate(Obj_RenZhengSet);
        GongGaoSetObj.transform.SetParent(Obj_UISet.transform);
        GongGaoSetObj.transform.localScale = new Vector3(1, 1, 1);
        GongGaoSetObj.transform.localPosition = new Vector3(0, 0, 0);

    }

    //打开认证
    public void Btn_OpenRenZheng_2()
    {
        Debug.Log("打开TapTap认证...");
        int timestart = (int)Time.time;
        string userIdentifier = SystemInfo.deviceUniqueIdentifier + timestart;

        GameSDKManager.Instance.AntiAddictionHandler = this.OnAntiAddictionHandler;
        GameSDKManager.Instance.RealNameAuther(userIdentifier);
    }

    //实名认证回调
    // code == 500;   // 玩家未受到限制，正常进入游戏
    // code == 1000;  // 退出防沉迷认证及检查，当开发者调用 Exit 接口时或用户认证信息无效时触发，游戏应返回到登录页
    // code == 1001;  // 用户点击切换账号，游戏应返回到登录页
    // code == 1030;  // 用户当前时间无法进行游戏，此时用户只能退出游戏或切换账号
    // code == 1050;  // 用户无可玩时长，此时用户只能退出游戏或切换账号
    // code == 1100;  // 当前用户因触发应用设置的年龄限制无法进入游戏
    // code == 1200;  // 数据请求失败，游戏需检查当前设置的应用信息是否正确及判断当前网络连接是否正常
    // code == 9002;  // 实名过程中点击了关闭实名窗，游戏可重新开始防沉迷认证
    public void OnAntiAddictionHandler(int code, string errormsg)
    {
        if (code == 1050)
        {
            Debug.Log("用户无可玩时长，此时用户只能退出游戏或切换账号");
            return;
        }

        if (code != 500)
        {
            Debug.Log("实名认证失败");
            return;
        }

        //获取年龄
        GameSDKManager.Instance.GetAgeRange();
        //获取剩余游戏时长
        GameSDKManager.Instance.GetRemainingTime(); 
    }

    //打开验证
    public void Btn_OpenShouJi() {

    }

    //找回角色
    public void Btn_OpenFindRose() {

        if (FindRoseSetObj != null) {
            Destroy(FindRoseSetObj);
        }

        FindRoseSetObj = (GameObject)Instantiate(Obj_FindRoseSet);
        FindRoseSetObj.transform.SetParent(Obj_UISet.transform);
        FindRoseSetObj.transform.localScale = new Vector3(1, 1, 1);
        FindRoseSetObj.transform.localPosition = new Vector3(0, 0, 0);

    }

    //隐私关闭
    public void Btn_CloseYinSi() {
        Obj_YinSiSet.SetActive(false);
    }

    //隐私关闭
    public void Btn_OpenYinSi()
    {
        Obj_YinSiSet.SetActive(true);
    }

    //请求权限
    public void QingQiuQuanXianShow()
    {
        Obj_QuanXianHint.SetActive(true);
    }

    //请求权限
    public void Btn_QingQiuQuanXian() {
        Btn_CloseQuanXianHint();
        //申请权限
        Game_PublicClassVar.Get_getSignature.QuDaoRequestPermissions();
    }

    public void Btn_CloseQuanXianHint() {

        Obj_QuanXianHint.SetActive(false);

    }
}
