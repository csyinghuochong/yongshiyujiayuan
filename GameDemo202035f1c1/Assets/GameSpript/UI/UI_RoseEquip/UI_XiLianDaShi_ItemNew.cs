using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UI_XiLianDaShi_ItemNew : MonoBehaviour
{

    public GameObject Obj_Title;
    public Text Text_attribute;
    public Text Text_gailvup;
    public Text Text_needvalue;
    public GameObject Obj_JiHuoHintSet;

    // Start is called before the first frame update
    void Start()
    {

    }

    public static string get_uft8(string unicodeString)
    {
        UTF8Encoding utf8 = new UTF8Encoding();
        Byte[] encodedBytes = utf8.GetBytes(unicodeString);
        String decodedString = utf8.GetString(encodedBytes);
        return decodedString;
    }

    public void UpdateData(string id, int total_value)
    {
        string Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", id, "EquipXiLianDaShi_Template");
        string NeedXiLianValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedXiLianValue", "ID", id, "EquipXiLianDaShi_Template");
        string Des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", id, "EquipXiLianDaShi_Template");
        string SkillDes = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDes", "ID", id, "EquipXiLianDaShi_Template");
        Obj_Title.GetComponent<Text>().text = Name;
        Text_attribute.text = Des;
        Text_gailvup.text = SkillDes;
        Text_gailvup.text = "获得" + Name + "洗炼之力,获得极品概率提升!";
        //string desc = "需要洗练熟练度 {0}/{1}";
        //string.(desc, NeedXiLianValue, total_value);
        Text_needvalue.text = "需要洗练熟练度 " + NeedXiLianValue + "/" + total_value;

        int nowNum = Game_PublicClassVar.Get_function_Rose.ReturnXiLianNum();
        if (NeedXiLianValue == "" || NeedXiLianValue == null)
        {
            NeedXiLianValue = "0";
        }

        if (nowNum >= int.Parse(NeedXiLianValue))
        {
            Obj_JiHuoHintSet.SetActive(true);
        }
        else
        {
            Obj_JiHuoHintSet.SetActive(false);
            Text_needvalue.color = Color.red;
        }

    }
}
