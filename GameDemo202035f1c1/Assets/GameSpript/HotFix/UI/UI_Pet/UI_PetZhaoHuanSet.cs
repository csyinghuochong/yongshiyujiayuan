using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PetZhaoHuanSet : MonoBehaviour {

    public GameObject Obj_UIPetZhaoHuanShow;
    public GameObject UIPetZhaoHuanShowSet;
    public GameObject UI_PetZhuanHuanShowDi;

    // Use this for initialization
    void Start () {

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //界面偏移
        float pianyiValue = 500 * (1 - this.gameObject.transform.localScale.x);
        if (pianyiValue >= 150) {
            pianyiValue = 150;
        }
        if (pianyiValue <= 0) {
            pianyiValue = 0;
        }

        this.gameObject.transform.localPosition = new Vector3(this.gameObject.transform.localPosition.x + pianyiValue, this.gameObject.transform.localPosition.y, this.gameObject.transform.localPosition.z);

        //显示宠物列表
        int showPetNum = 0;
        for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RosePetMaxNum; i++)
        {
            string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
            if (petID != "0")
            {
                showPetNum = showPetNum + 1;
                float value = (float)showPetNum / 4.0f;
                int higtNum = (int)(value);
                if (value > higtNum) {
                    higtNum = higtNum + 1;
                }

                //Debug.Log("showPetNum = " + showPetNum + ";higtNum = " + higtNum);
                //int higtNum = System.Math.Floor(System.Convert.ToDouble(showPetNum / 4));        //向下取整

                //设置底图高度
                UI_PetZhuanHuanShowDi.GetComponent<RectTransform>().sizeDelta = new Vector2(450, higtNum * 150);

                //显示宠物
                GameObject petShow = (GameObject)Instantiate(Obj_UIPetZhaoHuanShow);
                petShow.transform.SetParent(UIPetZhaoHuanShowSet.transform);
                petShow.GetComponent<UI_PetZhaoHuanShow>().NowPetID = i.ToString();
                petShow.transform.localScale = new Vector3(1, 1, 1);
            }
        }

        if (showPetNum == 0) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_298");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前没有宠物！");
            Destroy(this.gameObject);
        }

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
