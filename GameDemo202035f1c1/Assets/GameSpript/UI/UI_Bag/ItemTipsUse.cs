using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ItemTipsUse : MonoBehaviour {

    public string ItemID;           //道具ID
    public GameObject Obj_ItemName;
    public string UseSkillID;

	// Use this for initialization
	void Start () {
	    //获取道具名称
       /*
        ItemID = this.GetComponent<UI_EquipTips>
        if (ItemID != "") {
            string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", ItemID, "Item_Template");
            Obj_ItemName.GetComponent<Text>().text = itemName;
        }*/
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //使用道具按钮
    public void UseItem(){
        Debug.Log("我使用了道具");

        GameObject skill_0 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainFunctionUI.transform.Find("UI_MainRoseSkill_0").gameObject;
        skill_0.GetComponent<MainUI_SkillGrid>().SkillID = ItemID;
        skill_0.GetComponent<MainUI_SkillGrid>().UseSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", ItemID, "Item_Template");
        skill_0.GetComponent<MainUI_SkillGrid>().updataSkill();
        skill_0.GetComponent<MainUI_SkillGrid>().cleckbutton();
        CloseUI();
    }

    //丢弃道具按钮
    public void ThrowItem() {
        Debug.Log("我丢弃了道具");

        Game_PublicClassVar.Get_function_Rose.CostBagItem(ItemID, 1);
        CloseUI();
    }

    public void CloseUI() {

        Destroy(this.gameObject);

    }

}
