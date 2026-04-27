using UnityEngine;
using System.Collections;

public class UI_YiJianShuShou : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //出售绿装
    public void Btn_SellLvZhuang() 
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("3");
    }

    //出售绿色材料
    public void Btn_SellLvCaiLiao()
    {

        GameObject Obj_XiLianHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        string langStrHint_1 = "是否出售全部背包内的绿色品质的材料";
        //Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1 + hintSkillName + langStrHint_2, Btn_EquipXiLian_Item, null);
        Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint(langStrHint_1, null, SellLvCaiLiao, "洗炼提示", "取消", "确定", null);
        //Obj_XiLianHint.GetComponent<UI_CommonHint>().Btn_CommonHint("在此装备上发现了隐藏技能:" + hintSkillName + "！\n提示:如果再次洗炼隐藏技能有可能消失！", Btn_EquipXiLian_Item, null);

        Obj_XiLianHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        Obj_XiLianHint.transform.localPosition = Vector3.zero;
        Obj_XiLianHint.transform.localScale = new Vector3(1, 1, 1);

    }

    public void SellLvCaiLiao()
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("2");
    }

        //出售制作书
        public void Btn_SellLvZhiZuoShu()
    {
        Game_PublicClassVar.Get_function_Rose.SellBagYiJianItemToMoney("1");
    }

    public void closeUI() {
        Destroy(this.gameObject);
    }
}
