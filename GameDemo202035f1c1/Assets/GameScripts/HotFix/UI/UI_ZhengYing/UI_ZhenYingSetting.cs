using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ZhenYingSetting : MonoBehaviour
{

    private string guanzhiSpaceStr;

    public GameObject Obj_GuanZhiPar;
    public GameObject Obj_GuanZhi;

    // Start is called before the first frame update
    void Start()
    {
        guanzhiSpaceStr = "2;11;12;13;21;22;23;24;31;32;33;34;35;36";
        //Init();

        //如果自己是领袖就显示
        if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().SelfGuanZhi == "1") {
            Pro_ComStr_4 com4 = new Pro_ComStr_4();
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002304, com4);
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10002305, com4);
        }

    }

    public void Init() {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_GuanZhiPar);

        string[] guanzhiList = guanzhiSpaceStr.Split(';');
        for (int i = 0; i < guanzhiList.Length; i++) {

            GameObject obj = (GameObject)Instantiate(Obj_GuanZhi);
            obj.transform.SetParent(Obj_GuanZhiPar.transform);
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_ZhenYingZhiWeiSetting>().ZhiWeiXuHaoID = guanzhiList[i];

            //判断是否有人在此职位
            if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingZhiWeiDataList.ProZhenYingRoseData.ContainsKey(int.Parse(guanzhiList[i]))) {
                obj.GetComponent<UI_ZhenYingZhiWeiSetting>().ProZhenYingRoseData = Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingZhiWeiDataList.ProZhenYingRoseData[int.Parse(guanzhiList[i])];
            }

            obj.GetComponent<UI_ZhenYingZhiWeiSetting>().Init();

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
