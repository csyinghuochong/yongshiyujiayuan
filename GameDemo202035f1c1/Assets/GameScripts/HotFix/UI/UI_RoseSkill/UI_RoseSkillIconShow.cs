using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseSkillIconShow : MonoBehaviour {

    public string SpaceID;
    public GameObject Img_SkillIcon;
    private GameObject moveIconObj;
    public Sprite IconObjDi;

	// Use this for initialization
	void Start () {
        upDataSkillIcon();
	}
	
	// Update is called once per frame
	void Update () {
        if (Game_PublicClassVar.Get_game_PositionVar.UpdataMainSkillUI) {
            upDataSkillIcon();
        }
	}

    //更新技能图标
    void upDataSkillIcon() {
        //Debug.Log("格子-" + SpaceID + "更新");
        string skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + SpaceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (skillID != "" && skillID != "0")
        {
            //Img_SkillIcon.SetActive(true);
            string lastID = skillID.Substring(0, 1);
            switch (lastID)
            {
                case "1":

                    //string skillValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", skillID, "Item_Template");
                    //修改ICON （在做技能交换位置时,此处会修正）
                    string skillIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", skillID, "Item_Template");
                    object obj = Resources.Load("ItemIcon/" + skillIconID, typeof(Sprite));
                    Sprite skillIconSp = obj as Sprite;
                    Img_SkillIcon.GetComponent<Image>().sprite = skillIconSp;

                    break;

                case "6":

                    string skillIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", skillID, "Skill_Template");
                    obj = Resources.Load("SkillIcon/" + skillIcon, typeof(Sprite));
                    skillIconSp = obj as Sprite;
                    Img_SkillIcon.GetComponent<Image>().sprite = skillIconSp;

                    break;
            }

        }
        else {

            Img_SkillIcon.GetComponent<Image>().sprite = IconObjDi;

        }
    }

    public void Btn_Click() {
        //Debug.Log("我点击了技能图标");
    }

    public void Enter()
    {
        //Debug.Log("鼠标进入"+SpaceID);
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = SpaceID;
            //Debug.Log("进来了");
            /*
            if (skillCDStatus)
            {
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
                Game_PublicClassVar.Get_function_UI.GameHint("请等待冷却时间结束");
                return;
            }
            */
        }

        
        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            //触发移动
            //Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
        }
        
    }

    //鼠标移出
    public void Exit()
    {
        //ebug.Log("鼠标退出"+SpaceID);
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
    }

    //拖动技能
    public void MouseDrag_Start()
    {
        /*
        //获取技能是否为主动技能
        bool dragStatus = false;
        string skillType = "";
        if (SkillAddID != "" && SkillAddID != "0")
        {
            skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", SkillAddID, "Skill_Template");
        }
        else
        {
            skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", SkillID, "Skill_Template");
        }

        if (skillType == "1")
        {
            dragStatus = true;

            //循环获取快捷栏内的技能检测是否有CD

        }
        */
        
        //开启技能拖拽状态
        //实例化技能图标
        string skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + SpaceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (skillID.Substring(0, 1) == "1")
        {
            //表示为装备
            string skillIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", skillID, "Item_Template");
            if (skillIcon != "0")
            {
                moveIconObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UIMoveIcon);
                //显示Icon
                object obj = Resources.Load("ItemIcon/" + skillIcon, typeof(Sprite));
                Sprite skillIconSpr = obj as Sprite;
                moveIconObj.GetComponent<UI_MoveItemIcon>().itemIconSprite = skillIconSpr;      //传入图标精灵
                moveIconObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
                moveIconObj.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else { 
            //表示为技能
            string skillIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", skillID, "Skill_Template");
            if (skillIcon != "0")
            {
                moveIconObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.OBJ_UIMoveIcon);
                //显示Icon
                object obj = Resources.Load("SkillIcon/" + skillIcon, typeof(Sprite));
                Sprite skillIconSpr = obj as Sprite;
                moveIconObj.GetComponent<UI_MoveItemIcon>().itemIconSprite = skillIconSpr;      //传入图标精灵
                moveIconObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_TipsSet.transform);
                moveIconObj.transform.localScale = new Vector3(1, 1, 1);
            }

        }


        //开启技能移动
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus = true;
        Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_Initial = skillID;

    }

    //结束拖拽技能
    public void MouseDrag_End()
    {
        //执行技能拖拽

        string skillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + SpaceID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (skillID.Substring(0, 1) == "1")
            Game_PublicClassVar.Get_function_UI.UI_MoveToMainSkill("0");
        else
            Game_PublicClassVar.Get_function_UI.UI_MoveToMainSkill("1");

        //注销移动的Obj
        if (moveIconObj != null)
        {
            Destroy(moveIconObj);
        }

    }
}
