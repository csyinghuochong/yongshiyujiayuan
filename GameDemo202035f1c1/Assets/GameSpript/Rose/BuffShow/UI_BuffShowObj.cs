using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffShowObj : MonoBehaviour {

	public string BuffID;
	public GameObject Obj_BuffIcon;
	public GameObject Obj_BuffTimeShow;

	public GameObject Obj_BuffTipsObj;
	private GameObject buffTipsObj;

    //爆率提示
    public string BaolvValue;

	// Use this for initialization
	void Start () {



	}
	
	// Update is called once per frame
	void Update () {



	}

	//Buff显示
	public void BuffIDShow(){
        //显示buff
        if (BuffID != "" && BuffID != "0") {
            string buffIDIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffIcon", "ID", BuffID, "SkillBuff_Template");

            //显示底图
            Object obj = Resources.Load("SkillIcon/BuffIcon/" + buffIDIcon, typeof(Sprite));
            Sprite img = obj as Sprite;
            Obj_BuffIcon.GetComponent<Image>().sprite = img;
        }
	}

	//点击Buff显示
	public void Btn_BuffShow(){

		//获取buff描述
		string buffDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffDescribe", "ID", BuffID, "SkillBuff_Template");
		if (buffTipsObj != null) {
			Destroy (buffTipsObj);
		}

		buffTipsObj = (GameObject)Instantiate(Obj_BuffTipsObj);
		buffTipsObj.GetComponent<UI_SkillBuffTips>().SkillBuffID = BuffID;
		Transform ObjTipsTra = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set> ().Obj_UI_TipsSet.transform;
		buffTipsObj.transform.SetParent(ObjTipsTra);
		buffTipsObj.transform.localScale = new Vector3 (1, 1, 1);
		buffTipsObj.transform.position = new Vector3(this.gameObject.transform.position.x+150,this.gameObject.transform.position.y-100,this.gameObject.transform.position.z);
	}

}
