using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaveServerPlayData : MonoBehaviour
{
    public GameObject SaveZhangHaoObj;
    public GameObject SaveMiMaObj;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //保存玩家数据至服务器
    public void Btn_SavePlayData(){

        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
        //string jieshaoStr = "1.玩家需要在限定的时间内击杀小怪收集秘境值,当秘境值达到500点后即可召唤秘境领主BOSS.\n2.击杀秘境BOSS后则挑战当前秘境层级成功,并激活下一层级大秘境,怪物实力随着秘境层级越高！\n3.大秘境内均会掉落秘境碎片可以在隔壁的同学处兑换奖励！\n4.大秘境成功退出地图后可以连续挑战,如果挑战失败只能等明日再来！";
        string jieshaoStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_20");
        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(jieshaoStr, SavePlayData, null);
        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
        uiCommonHint.transform.localPosition = Vector3.zero;
        uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
    }

    public void SavePlayData() {
        string zhanghao = SaveZhangHaoObj.GetComponent<InputField>().text;
        string mima = SaveMiMaObj.GetComponent<InputField>().text;
        if (zhanghao != "")
        {

            if (mima != "")
            {
                //上传数据
                Debug.Log("密码：" + mima);
                string shebeiStr = SystemInfo.deviceUniqueIdentifier;
                string[] saveList = new string[] { zhanghao, mima, shebeiStr };
                //string[] saveList = new string[] { zhangHaoIDStr, zhangHaoMiMa, "" };
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001007, saveList);

            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_133");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("密码不能为空！");
            }
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_134");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("账号不能为空！");
        }
    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
