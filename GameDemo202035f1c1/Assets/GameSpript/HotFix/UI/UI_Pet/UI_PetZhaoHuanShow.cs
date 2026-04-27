using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PetZhaoHuanShow : MonoBehaviour {

    public string NowPetID;
    public GameObject Obj_PetHeadIcon;
    public GameObject Obj_PetHeadLv;
    public GameObject Obj_PetName;
    public GameObject Obj_PetChuZhan;
    public GameObject Obj_PetZhaoHuanCD_Img;
    public GameObject Obj_PetZhaoHuanCD_Text;
    public GameObject Obj_PetHpValuePro;

    private float maxHp;
    private float secUpdateSum;

    // Use this for initialization
    void Start () {

        //显示宠物信息
        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", NowPetID, "RosePet");
        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", NowPetID, "RosePet");
        string petTemplateID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", NowPetID, "RosePet");
        string headIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petTemplateID, "Pet_Template");
        Obj_PetName.GetComponent<Text>().text = petName;
        Obj_PetHeadLv.GetComponent<Text>().text = "Lv." + petLv;
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowPetID, "RosePet");
        //显示宠物头像
        object obj = Resources.Load("PetHeadIcon/" + headIcon, typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_PetHeadIcon.GetComponent<Image>().sprite = img;

        if (petStatus == "1")
        {
            Obj_PetChuZhan.SetActive(true);
        }
        else {
            Obj_PetChuZhan.SetActive(false);
        }

        //获取最大血量
        maxHp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetMaxHp", "ID", NowPetID, "RosePet"));
        secUpdateSum = 1;
    }
	
	// Update is called once per frame
	void Update () {

        secUpdateSum = secUpdateSum + Time.deltaTime;
        if (secUpdateSum >= 1) {
            secUpdateSum = 0;
            float CostTime = Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanCD(NowPetID);
            int CostTime_Int = (int)(CostTime);
            if (CostTime_Int > 0)
            {
                Obj_PetZhaoHuanCD_Text.GetComponent<Text>().text = CostTime_Int.ToString();
                Obj_PetZhaoHuanCD_Img.GetComponent<Image>().fillAmount = CostTime / 300.0f;
                Obj_PetZhaoHuanCD_Img.SetActive(true);

            }
            else {
                Obj_PetZhaoHuanCD_Img.SetActive(false);
            }

            //显示生命值
            float nowHp = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetNowHp", "ID", NowPetID, "RosePet"));
            if (maxHp != 0)
            {
                //判定角色是否处于冷却状态
                float value = 1;

                if (nowHp <= 0) {
                    if (Game_PublicClassVar.Get_function_AI.Pet_GetZhaoHuanCD(NowPetID) == 0)
                    {
                        value = 1;
                    }
                    else {
                        value = nowHp / maxHp;
                    }
                }
                else{
                    value = nowHp / maxHp;
                }

                Obj_PetHpValuePro.GetComponent<Image>().fillAmount = value;
            }
            else
            {
                //表示该宠物为未出战过
                Obj_PetHpValuePro.GetComponent<Image>().fillAmount = 1;
            }
        }

	}

    public void Btn_ZhaoHuan() {

        //Debug.Log("我点击了出战按钮");

        //判定点击的是否是当前已经召唤的
        string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", NowPetID, "RosePet");
        if (petStatus == "1")
        {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_147");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("收回召唤宠物成功！");
            //收回宠物
            Game_PublicClassVar.Get_function_Rose.RosePetDelete(int.Parse(NowPetID));
            //关闭界面
            Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_Pet_ZhaoHuanSet);
            return;
        }
        else {
            //召唤默认出战的宠物
            //Debug.Log("Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.Length = " + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.Length);
            for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RosePetObj.Length; i++)
            {
                string nowpetStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", i.ToString(), "RosePet");
                if (nowpetStatus == "1")
                {
                    //撤回当前宠物
                    //Debug.Log("撤回了当前出战 i = " + i.ToString());
                    Game_PublicClassVar.Get_function_Rose.RosePetDelete(int.Parse(i.ToString()));
                    break;
                }
            }

            //判定当前栏位是否为空
            if (Game_PublicClassVar.Get_function_AI.Pet_ZhaoHuan(NowPetID))
            {
                //关闭界面
                Destroy(Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().obj_Pet_ZhaoHuanSet);
            }
        }
    }
}
