using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_ZhenYingSet : MonoBehaviour {

    //按钮区域
    public GameObject Obj_Btn_ZhenYingShiLi;
    public GameObject Obj_Btn_ZhenYingReward;
    public GameObject Obj_Btn_ZhenYingXuanZe;
    public GameObject Obj_ZhenYing_ShouLie;
    public GameObject Obj_ZhenYing_FuLi;
    public GameObject Obj_ZhenYing_Store;
    public GameObject Obj_ZhenYing_Setting;

    public GameObject Obj_Btn_ZhenYingShiLi_Img;
    public GameObject Obj_Btn_ZhenYingReward_Img;
    public GameObject Obj_Btn_ZhenYingXuanZe_Img;
    public GameObject Obj_Btn_ZhenYingShiLi_Text;
    public GameObject Obj_Btn_ZhenYingReward_Text;
    public GameObject Obj_Btn_ZhenYingXuanZe_Text;

    public GameObject Obj_Btn_ZhenYingShouLie_Img;
    public GameObject Obj_Btn_ZhenYingFuLi_Img;
    public GameObject Obj_Btn_ZhenYingStore_Img;
    public GameObject Obj_Btn_ZhenYingShouLie_Text;
    public GameObject Obj_Btn_ZhenYingFuLi_Text;
    public GameObject Obj_Btn_ZhenYingStore_Text;

    public GameObject Obj_Btn_ZhenYingSeeting_Img;
    public GameObject Obj_Btn_ZhenYingSeeting_Text;

    public GameObject Obj_GameSettingBtn;

    public ObscuredString SelfGuanZhi;

    public Pro_ZhenYingDataList ProZhenYingDataList;
    public Pro_ZhenYingXuanZeDataList ProZhenYingXuanZeDataList;
    public Pro_ZhenYingXuanZeDataList ProZhenYingZhiWeiDataList;
    // Use this for initialization
    void Start () {

        //请求数据
        Game_PublicClassVar.Get_gameServerObj.Obj_ZhenYingSet = this.gameObject;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002301, "");

        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Btn_XuanZe();
        }
        else {
            Btn_ShiLi();
        }

        //获取当前职位
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002306, "");

    }
	
	// Update is called once per frame
	void Update () {
		
	}


    //关闭UI
    public void CloseUI()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().OpenGameZhenYing();
    }


    //实力
    public void Btn_ShiLi()
    {
        ClearnShow();
        Obj_Btn_ZhenYingShiLi.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingShiLi_Img, Obj_Btn_ZhenYingShiLi_Text);
        Obj_Btn_ZhenYingShiLi.GetComponent<UI_ZhenYingShiLiShowSet>().ProZhenYingDataList = ProZhenYingDataList;
        Obj_Btn_ZhenYingShiLi.GetComponent<UI_ZhenYingShiLiShowSet>().UpdateData();
    }

    //奖励
    public void Btn_Reward()
    {
        ClearnShow();
        Obj_Btn_ZhenYingReward.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingReward_Img, Obj_Btn_ZhenYingReward_Text);
    }

    //选择
    public void Btn_XuanZe()
    {
        ClearnShow();
        Obj_Btn_ZhenYingXuanZe.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingXuanZe_Img, Obj_Btn_ZhenYingXuanZe_Text);
    }

    //狩猎
    public void Btn_ShouLie()
    {
        ClearnShow();
        Obj_ZhenYing_ShouLie.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingShouLie_Img, Obj_Btn_ZhenYingShouLie_Text);
    }

    //福利
    public void Btn_FuLi()
    {
        ClearnShow();
        Obj_ZhenYing_FuLi.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingFuLi_Img, Obj_Btn_ZhenYingFuLi_Text);
    }

    //商店
    public void Btn_Store()
    {
        ClearnShow();
        Obj_ZhenYing_Store.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingStore_Img, Obj_Btn_ZhenYingStore_Text);
    }

    //商店
    public void Btn_Setting()
    {
        ClearnShow();
        Obj_ZhenYing_Setting.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhenYingSeeting_Img, Obj_Btn_ZhenYingSeeting_Text);
    }


    //清理
    public void ClearnShow()
    {
        Obj_Btn_ZhenYingShiLi.SetActive(false);
        Obj_Btn_ZhenYingReward.SetActive(false);
        Obj_Btn_ZhenYingXuanZe.SetActive(false);
        Obj_ZhenYing_ShouLie.SetActive(false);
        Obj_ZhenYing_FuLi.SetActive(false);
        Obj_ZhenYing_Store.SetActive(false);
        Obj_ZhenYing_Setting.SetActive(false);
        Btn_ChongZhi();
    }


    //选中按钮展示
    public void Btn_XuanZhongShow(GameObject obj, GameObject obj_Text)
    {
        //显示按钮
        object imgObj = Resources.Load("GameUI/" + "Btn/Btn_57_1", typeof(Sprite));
        Sprite img = imgObj as Sprite;
        //重置按钮状态
        obj.GetComponent<Image>().sprite = img;
        obj_Text.GetComponent<Text>().color = new Color(0.93f, 0.85f, 0.68f);
    }

    //重置按钮状态
    public void Btn_ChongZhi()
    {
        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_57_2", typeof(Sprite));
        Sprite img = obj as Sprite;

        //重置按钮状态
        Obj_Btn_ZhenYingShiLi_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhenYingReward_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhenYingXuanZe_Img.GetComponent<Image>().sprite = img;

        //重置字体
        Obj_Btn_ZhenYingShiLi_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhenYingReward_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhenYingXuanZe_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);

        //重置按钮状态
        Obj_Btn_ZhenYingShouLie_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhenYingFuLi_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhenYingStore_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhenYingSeeting_Img.GetComponent<Image>().sprite = img;

        //重置字体
        Obj_Btn_ZhenYingShouLie_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhenYingFuLi_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhenYingStore_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhenYingSeeting_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);

    }

    public void ShowSetting() {

        if (SelfGuanZhi == "1")
        {
            Obj_GameSettingBtn.SetActive(true);
        }
        else {
            Obj_GameSettingBtn.SetActive(false);
        }

    }

}
