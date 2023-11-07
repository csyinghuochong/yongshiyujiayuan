using System.Collections;
using System.Collections.Generic;
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
        if ( string.IsNullOrEmpty(PlayerPrefs.GetString(YinSi.PlayerPrefsYinSi)))
        {
            Obj_HuaWeiYinSi.SetActive(true);
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
