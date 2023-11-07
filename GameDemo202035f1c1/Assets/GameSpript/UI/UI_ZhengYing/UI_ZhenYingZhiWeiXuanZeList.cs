using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhenYingZhiWeiXuanZeList : MonoBehaviour
{

    public string ZhengYingSpace;
    public Pro_ZhenYingRoseData ProZhenYingRoseData;

    public GameObject Obj_ZhengYingHeadIcon;
    public GameObject Obj_ZhengYingName;
    public GameObject Obj_ZhengYingShiLiValue;
    public GameObject Obj_Par;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    //初始化显示
    public void Init() {

        Obj_ZhengYingName.GetComponent<Text>().text = ProZhenYingRoseData.RoseName;
        Obj_ZhengYingShiLiValue.GetComponent<Text>().text = ProZhenYingRoseData.RoseShiLiValue.ToString();
        Game_PublicClassVar.Get_function_UI.ShowPlayerHeadIcon(ProZhenYingRoseData.RoseOcc, Obj_ZhengYingHeadIcon);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Btn_XuanZhong() {

        Pro_ComStr_4 com4 = new Pro_ComStr_4();
        com4.str_1 = ProZhenYingRoseData.RoseZhangHaoID;
        com4.str_2 = ZhengYingSpace;
        Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002303, com4);

        Obj_Par.GetComponent<UI_ZhenYingPlayerXuanZe>().Btn_Close();

    }
}
