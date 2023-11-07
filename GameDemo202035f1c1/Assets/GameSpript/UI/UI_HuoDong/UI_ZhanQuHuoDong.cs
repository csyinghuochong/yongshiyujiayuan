using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhanQuHuoDong : MonoBehaviour {

	public GameObject Obj_HuoDongRewardLv;
	public GameObject Obj_HuoDongRewardShiLi;
    public GameObject Obj_HuoDongQianDao;
    public GameObject Obj_HuoDongLingPai;

    //按钮区域
    public GameObject Obj_Btn_ZhanQuRewardLv;
	public GameObject Obj_Btn_ZhanQuRewardShiLi;

    public GameObject Obj_Btn_ZhanQuRewardLv_Img;
    public GameObject Obj_Btn_ZhanQuRewardShiLi_Img;
    public GameObject Obj_Btn_ZhanQuRewardLv_Text;
    public GameObject Obj_Btn_ZhanQuRewardShiLi_Text;
    public GameObject Obj_Btn_ZhanQuQianDao_Img;
    public GameObject Obj_Btn_ZhanQuQianDao_Text;
    public GameObject Obj_Btn_ZhanQuLingPai_Img;
    public GameObject Obj_Btn_ZhanQuLingPai_Text;
    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing = this.gameObject;
		//初始化显示等级奖励
		ClearnShow();
		Btn_RewardLv ();

	}
	
	// Update is called once per frame
	void Update () {


	}

	//等级奖励
	public void Btn_RewardLv(){
	
		ClearnShow();
        Obj_HuoDongRewardLv.SetActive(true);
        Obj_HuoDongRewardLv.GetComponent<UI_ZhanQu_RewardLv>().Init();
        Btn_XuanZhongShow(Obj_Btn_ZhanQuRewardLv_Img, Obj_Btn_ZhanQuRewardLv_Text);

    }

	//实力奖励
	public void Btn_RewardShiLi(){
	
		ClearnShow();
        Obj_HuoDongRewardShiLi.SetActive(true);
        Obj_HuoDongRewardShiLi.GetComponent<UI_ZhanQu_RewardShiLi>().Init();
        Btn_XuanZhongShow(Obj_Btn_ZhanQuRewardShiLi_Img, Obj_Btn_ZhanQuRewardShiLi_Text);
    }

    //签到系统
    public void Btn_QianDao() {
        ClearnShow();
        Obj_HuoDongQianDao.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhanQuQianDao_Img, Obj_Btn_ZhanQuQianDao_Text);
    }

    //令牌系统
    public void Btn_LingPai() {
        ClearnShow();
        Obj_HuoDongLingPai.SetActive(true);
        Btn_XuanZhongShow(Obj_Btn_ZhanQuLingPai_Img, Obj_Btn_ZhanQuLingPai_Text);
    }

	public void ClearnShow(){
		
		Obj_HuoDongRewardLv.SetActive(false);
		Obj_HuoDongRewardShiLi.SetActive (false);
        Obj_HuoDongQianDao.SetActive(false);
        Obj_HuoDongLingPai.SetActive(false);
        Btn_ChongZhi();
    
    }

	//关闭战区UI
	public void CloseUI(){
		Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen> ().OpenZhanQuHuoDong ();
		//Destroy (this.gameObject);
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
        Obj_Btn_ZhanQuRewardLv_Img.GetComponent<Image>().sprite = img; ;
        Obj_Btn_ZhanQuRewardShiLi_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhanQuQianDao_Img.GetComponent<Image>().sprite = img;
        Obj_Btn_ZhanQuLingPai_Img.GetComponent<Image>().sprite = img;
        //重置字体
        Obj_Btn_ZhanQuRewardLv_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhanQuRewardShiLi_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhanQuQianDao_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
        Obj_Btn_ZhanQuLingPai_Text.GetComponent<Text>().color = new Color(0.53f, 0.32f, 0.26f);
    }
}
