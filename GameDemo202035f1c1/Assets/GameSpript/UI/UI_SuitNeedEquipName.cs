using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_SuitNeedEquipName : MonoBehaviour {

    public string SuitID;                   //套装ID
    public string SuitShowDirection;        //展示方向          1:表示在左侧  2：表示在右侧
    public GameObject Obj_SuitName;         //套装名称
    public GameObject Obj_SuitNameSet;      //套装名称
    public GameObject Obj_SuitEquipName;    //套装名称
    private float showPosition_Y;       //展示的Y轴位置
    public GameObject Obj_SuitSign_Left;     //套装标记图标
    public GameObject Obj_SuitSign_Right;     //套装标记图标

	// Use this for initialization
	void Start () {
        //SuitID = "1001";
        switch (SuitShowDirection) { 
            case "1":
                Debug.Log("显示1");
                Obj_SuitSign_Left.SetActive(false);
                Obj_SuitSign_Right.SetActive(true);
                break;

            case "2":
                Debug.Log("显示2");
                Obj_SuitSign_Left.SetActive(true);
                Obj_SuitSign_Right.SetActive(false);
                break;
        }
        
        showPosition_Y = 70.0f;
        string needEquipIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipID", "ID", SuitID, "EquipSuit_Template");
        string needEquipNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedEquipNum", "ID", SuitID, "EquipSuit_Template");
        string[] needEquipID = needEquipIDStr.Split(';');
        string[] needEquipNum = needEquipNumStr.Split(';');

        //获取套装名称
        string equipSuitName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", SuitID, "EquipSuit_Template");
        //获取套装ID
        string[] suitPropertyIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SuitPropertyID", "ID", SuitID, "EquipSuit_Template").Split(';');
        //获取自身套装数量
        int equipSuitNum = Game_PublicClassVar.Get_function_Rose.returnEquipSuitNum(needEquipID, needEquipNum);
        //显示套装名称及拥有数量
        Obj_SuitName.GetComponent<Text>().text = equipSuitName + "(" + equipSuitNum + "/" + needEquipID.Length + ")";
        

        //逐个显示
        for (int i = 0; i <= needEquipID.Length - 1; i++) {

            //获取当前装备的数量
            int equipNowNum = Game_PublicClassVar.Get_function_Rose.IfWearEquipID(needEquipID[i]);
            for (int y = 0; y < int.Parse(needEquipNum[i]); y++) {
                //实例化
                GameObject obj_suitName = (GameObject)Instantiate(Obj_SuitEquipName);
                obj_suitName.transform.SetParent(Obj_SuitNameSet.transform);
                obj_suitName.transform.localScale = new Vector3(1, 1, 1);
                obj_suitName.transform.localPosition = new Vector3(0, showPosition_Y, 0);
                showPosition_Y = showPosition_Y - 25.0f;
                //获取装备名称
                string suitNeedItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", needEquipID[i], "Item_Template");
                
                //判定显示的颜色
                if (equipNowNum >= int.Parse(needEquipNum[i]))
                {
                    //显示绿色
                    obj_suitName.GetComponent<Text>().color = Color.green;
                    obj_suitName.GetComponent<Text>().text = suitNeedItemName + "(已穿戴)";
                }
                else {
                    //确保2个装备 第一个显示绿色  第二个显示灰色
                    int yyy = y + 1;
                    if (yyy >= equipNowNum)
                    {
                        //显示绿色
                        obj_suitName.GetComponent<Text>().color = Color.white;
                        obj_suitName.GetComponent<Text>().text = suitNeedItemName + "(未穿戴)";
                    }
                    //显示灰色(默认)
                    //obj_suitName.GetComponent<Text>().color = new Color();
                }
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
	    


	}

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
