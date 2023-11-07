using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ChengHaoXuanZeList : MonoBehaviour
{

    public string ChengHaoID;
    public GameObject Obj_FuJiObj;
    public GameObject Obj_ChengHaoName;
    public GameObject Obj_ChengHaoDes;
    public bool UpdateStatus;
    public GameObject Obj_UseChengHao_True;     //使用称号
    public GameObject Obj_UseChengHao_False;    //取消使用称号

    // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (UpdateStatus) {
            UpdateStatus = false;
            showPetListProperty();
        }
	}

    //展示宠物列表信息
    void showPetListProperty() {

        //获取宠物名称
        //string Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", PetOnlyID, "RosePet");
        string Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ChengHaoID, "ChengHao_Template");
        string Des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", ChengHaoID, "ChengHao_Template");

        //显示宠物信息
        Obj_ChengHaoName.GetComponent<Text>().text = Name;
        Obj_ChengHaoDes.GetComponent<Text>().text = Des;

        //获取是否是当前的称号
        string nowChengHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowChengHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowChengHaoID == ChengHaoID && nowChengHaoID !="")
        {
            Obj_UseChengHao_False.SetActive(true);
            Obj_UseChengHao_True.SetActive(false);
        }
        else {
            Obj_UseChengHao_True.SetActive(true);
            Obj_UseChengHao_False.SetActive(false);
        }
    }


    //点击选中按钮
    public void Click_XuanZhong() {

        Debug.Log("点击选中" + ChengHaoID);
        Game_PublicClassVar.Get_function_Rose.ChengHao_Use(ChengHaoID);
        Destroy(Obj_FuJiObj);
    }

    public void Click_QuXiaoChengHao() {
        Game_PublicClassVar.Get_function_Rose.ChengHao_Null();
        Destroy(Obj_FuJiObj);
    }

    //关闭界面
    public void Click_CloseUI() {
        Destroy(this.gameObject);
    }

}
