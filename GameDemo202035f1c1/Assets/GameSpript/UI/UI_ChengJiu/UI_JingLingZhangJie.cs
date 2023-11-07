using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_JingLingZhangJie : MonoBehaviour {

    public string ZhangJieID;
    public GameObject Obj_ZhangJieName;
    public GameObject Obj_ZhangJieXuanZhong;
    private Rose_ChengJiuSet roseChengJiuList;
    // Use this for initialization
    void Start () {
        roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
        Obj_ZhangJieName.GetComponent<Text>().color = new Color(0.92f, 0.84f, 0.67f);
    }
	
	// Update is called once per frame
	void Update () {
        if (roseChengJiuList.UpdateXuanZhongJingLingStatus) {
            if (roseChengJiuList.Obj_XuanZhongJingLing != this.gameObject)
            {
                Obj_ZhangJieXuanZhong.SetActive(false);
                Obj_ZhangJieName.GetComponent<Text>().color = new Color(0.92f, 0.84f, 0.67f);
            }
            else {
                Obj_ZhangJieXuanZhong.SetActive(true);
                Obj_ZhangJieName.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
            }
        }
	}

    public void ShowZhangJieName() {
        //Debug.Log("展示章节ID");
        string zhangJieName = "";
        switch (ZhangJieID) {

            //第一章
            case "1":
                zhangJieName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第一章");
                break;

            //第二章
            case "2":
                zhangJieName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第二章");
                break;

            //第三章
            case "3":
                zhangJieName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第三章");
                break;

            //第四章
            case "4":
                zhangJieName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第四章");
                break;

            //第五章
            case "5":
                zhangJieName = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("第五章");
                break;
        }

        Obj_ZhangJieName.GetComponent<Text>().text = zhangJieName;

    }

    public void Btn_ZhangJie() {

        Debug.Log("点击章节ID + " + ZhangJieID);
        roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
        roseChengJiuList.Obj_XuanZhongJingLing = this.gameObject;
        roseChengJiuList.UpdateXuanZhongJingLingStatus = true;
        roseChengJiuList.JingLingShow(ZhangJieID);

    }
}
