using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_JingLingShow : MonoBehaviour {

    public string JingLingID;
    public GameObject Obj_JingLingName;
    public GameObject Obj_JingLingDes;
    public GameObject Obj_JingLingJiHuo;
    public GameObject Obj_JingLingImgShow;
    public GameObject Obj_JingLingIcon;
    public GameObject Obj_JingLingBtn;
    public GameObject Obj_JingLingType;
    private Rose_ChengJiuSet roseChengJiuList;
    // Use this for initialization
    void Start () {
        roseChengJiuList = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_roseChengJiu.GetComponent<Rose_ChengJiuSet>();
    }
	
	// Update is called once per frame
	void Update () {
        if (roseChengJiuList.UpdateXuanZhongJingLingStatus)
        {
            JingLingShow();
        }
    }

    //展示精灵
    public void JingLingShow() {

        string name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", JingLingID, "Spirit_Template");
        string des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", JingLingID, "Spirit_Template");
        string model = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ModelID", "ID", JingLingID, "Spirit_Template");
        string icon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", JingLingID, "Spirit_Template");

        Obj_JingLingName.GetComponent<Text>().text = name;
        Obj_JingLingDes.GetComponent<Text>().text = des;

        //显示Icon
        object obj = Resources.Load("JingLingIcon/" + icon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_JingLingIcon.GetComponent<Image>().sprite = itemIcon;

        //显示精灵主动/被动
        string mainSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", JingLingID, "Spirit_Template");
        if (mainSkill != "" && mainSkill != "0")
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("主动精灵");
            Obj_JingLingType.GetComponent<Text>().text = "(" + langStr + ")";
        }
        else
        {
            string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("被动精灵");
            Obj_JingLingType.GetComponent<Text>().text = "("+ langStr + ")";
        }

        //获取当前精灵是否激活
        if (Game_PublicClassVar.Get_function_Rose.JingLing_JianCe(JingLingID))
        {
            //是否显示可装备按钮
            if (mainSkill != "" && mainSkill != "0")
            {
                Obj_JingLingBtn.SetActive(true);
            }
            else
            {
                Obj_JingLingBtn.SetActive(true);
            }

            string jingLingEquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            if (jingLingEquipIDStr != "")
            {
                string[] jingLingEquipIDList = jingLingEquipIDStr.Split(';');
                for (int i = 0; i < jingLingEquipIDList.Length; i++)
                {
                    if (jingLingEquipIDList[i] == JingLingID)
                    {
                        string nowMainSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", jingLingEquipIDList[i], "Spirit_Template");
                        if (nowMainSkill != "" && nowMainSkill != "0")
                        {
                            Obj_JingLingJiHuo.SetActive(true);
                            Debug.Log("激活激活激活！");
                        }
                        else
                        {
                            Obj_JingLingJiHuo.SetActive(false);
                        }
                    }
                    else {
                        Obj_JingLingJiHuo.SetActive(false);
                    }
                }
            }

        }
        else {
            //未激活
            Obj_JingLingJiHuo.SetActive(false);
            Obj_JingLingBtn.SetActive(false);
            //灰化Icon
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_JingLingIcon.GetComponent<Image>().material = huiMaterial;
            Obj_JingLingName.GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1);
            Obj_JingLingDes.GetComponent<Text>().color = new Color(0.55f, 0.55f, 0.55f, 1);
        }
    }

    //装备精灵
    public void Btn_EquipJingLing() {

        string oldJingLingID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

        if (Game_PublicClassVar.Get_function_Rose.JingLing_Equip(JingLingID)) {
            roseChengJiuList.UpdateXuanZhongJingLingStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().UpdataPassiveSkillStatus = true;

            //获取原来buffID注销对应buff
            if (oldJingLingID != "" && oldJingLingID != "0" && oldJingLingID != null) {

                string jingLingSkillStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkill", "ID", oldJingLingID, "Spirit_Template");
                string buffID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffID", "ID", jingLingSkillStr, "Skill_Template");
                string buffType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffType", "ID", buffID, "SkillBuff_Template");
                /*
                if (buffType == "1") {
                    Buff_1[] buff_1_List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_1>();
                    for (int i = 0; i < buff_1_List.Length; i++)
                    {
                        if (buff_1_List[i].BuffID == buffID)
                        {
                            Destroy(buff_1_List[i]);
                        }
                    }
                }
                */
                if (buffType == "4")
                {
                    Buff_4[] buff_4_List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
                    for (int i = 0; i < buff_4_List.Length; i++) {
                        if (buff_4_List[i].BuffID == buffID) {
                            Destroy(buff_4_List[i]);
                        }
                    }
                }


                //查询快捷按钮是否有老的技能ID
                Game_PublicClassVar.Get_function_Skill.UpdataMainUISkillID(jingLingSkillStr,"");

            }

        }
    }
}
