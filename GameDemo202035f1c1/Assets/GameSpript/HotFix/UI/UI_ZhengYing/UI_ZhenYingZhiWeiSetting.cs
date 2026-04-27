using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ZhenYingZhiWeiSetting : MonoBehaviour
{

    public string ZhiWeiXuHaoID;
    public string PlayerZhiWeiName;

    private string roseOcc;
    private string PlayerName;

    public GameObject Obj_PlayerName;
    public GameObject Obj_PlayerHeadIcon;
    public GameObject Obj_PlayerZhiWeiName;

    public GameObject Obj_ZhenYingPlayerXuanZe;

    public Pro_ZhenYingRoseData ProZhenYingRoseData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Init() {

        //显示职位
        PlayerZhiWeiName = Game_PublicClassVar.Get_function_UI.ReturnZhiWeiName(ZhiWeiXuHaoID);

        if (ProZhenYingRoseData != null)
        {
            roseOcc = ProZhenYingRoseData.RoseOcc;
            PlayerName = ProZhenYingRoseData.RoseName;
        }

        if (roseOcc != "" && roseOcc != "0"&& roseOcc != null)
        {
            //显示头像
            Game_PublicClassVar.Get_function_UI.ShowPlayerHeadIcon(roseOcc, Obj_PlayerHeadIcon);
        }
        else {
            PlayerName = "点击选择玩家";
        }

        Obj_PlayerName.GetComponent<Text>().text = PlayerName;
        Obj_PlayerZhiWeiName.GetComponent<Text>().text = PlayerZhiWeiName;
    }

    //设置职位
    public void Btn_SaveZhiWei() {

        GameObject nowObj = (GameObject)Instantiate(Obj_ZhenYingPlayerXuanZe);
        nowObj.transform.SetParent(Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().Obj_ZhenYing_Setting.transform);
        nowObj.transform.localScale = new Vector3(1, 1, 1);
        nowObj.transform.localPosition = new Vector3(0, 0, 0);
        nowObj.GetComponent<UI_ZhenYingPlayerXuanZe>().ZhengYingSpace = ZhiWeiXuHaoID;

    }

}
