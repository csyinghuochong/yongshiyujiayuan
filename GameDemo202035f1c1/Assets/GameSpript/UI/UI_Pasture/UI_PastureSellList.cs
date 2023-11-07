using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_PastureSellList : MonoBehaviour {

    public ObscuredString PastureID;
    private ObscuredInt PastureBuyGold;
    private ObscuredInt pasPeopleNum;

    public GameObject Obj_PastureName;
    public GameObject Obj_PastureBuyGold;
    public GameObject Obj_PastureDanName;
    public GameObject Obj_PastureRenKou;
    public GameObject Obj_PastureJieSuo;
    public GameObject Obj_PastureBuyBtn;
    public GameObject Obj_PastureHeadIcon;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Init()
    {
        //Debug.Log("初始化...");

        //显示信息
        string PastureName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", PastureID, "Pasture_Template");
        Obj_PastureName.GetComponent<Text>().text = PastureName;
        //显示购买金币
        PastureBuyGold = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuyGold", "ID", PastureID, "Pasture_Template"));
        Obj_PastureBuyGold.GetComponent<Text>().text = PastureBuyGold.ToString();
        //显示牧场名称
        string pasGetItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GetItemID", "ID", PastureID, "Pasture_Template");
        string pasGetItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", pasGetItemID, "SceneItem_Template");
        Obj_PastureDanName.GetComponent<Text>().text = pasGetItemName;
        //显示人口
        pasPeopleNum = int.Parse( Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PeopleNum", "ID", PastureID, "Pasture_Template"));
        Obj_PastureRenKou.GetComponent<Text>().text = pasPeopleNum.ToString();

        //显示需要达到多少级解锁
        string nowPastureLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowPastureLvStr, "PastureUpLv_Template");
        string needLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", PastureID, "Pasture_Template");
        if (int.Parse(nowPastureLv) >= int.Parse(needLv))
        {
            Obj_PastureJieSuo.SetActive(false);
            Obj_PastureBuyBtn.SetActive(true);
        }
        else {
            Obj_PastureBuyBtn.SetActive(false);
            Obj_PastureJieSuo.GetComponent<Text>().text = "需要牧场等级达到" + needLv + "级解锁";

            //头像显示灰色
            object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
            Material huiMaterial = huiObj as Material;
            Obj_PastureHeadIcon.GetComponent<Image>().material = huiMaterial;
        }
        //显示头像
        string PastureIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Icon", "ID", PastureID, "Pasture_Template");
        object obj = Resources.Load("HeadIcon/PastureIcon/" + PastureIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_PastureHeadIcon.GetComponent<Image>().sprite = itemIcon;
    }

    //购买牧场动物
    public void BuyPasture() {

        //判断牧场等级是否足够
        string nowPastureLvStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowPastureLvStr, "PastureUpLv_Template");
        string needLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", PastureID, "Pasture_Template");
        if (int.Parse(nowPastureLv) < int.Parse(needLv))
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("牧场等级不足");
            return;
        }

        //判定资金是否足够
        int nowpasstureGold = Game_PublicClassVar.Get_function_Rose.GetRosePastureGold();
        if (nowpasstureGold >= PastureBuyGold) {

            //判定人口上限
            if (Game_PublicClassVar.Get_function_Pasture.IfPasstureRenKou(pasPeopleNum))
            {
                //扣除资金
                Game_PublicClassVar.Get_function_Rose.CostReward("5", PastureBuyGold.ToString());
                //创建动物
                Game_PublicClassVar.Get_function_Pasture.CreatePastureAI(PastureID);

                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买成功！你的农场又多了一位小伙伴！");

                //Debug.Log("购买成功牧场动物！");
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("农场人口已满！");
            }

        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("农场资金不足！");
        }

    }

}
