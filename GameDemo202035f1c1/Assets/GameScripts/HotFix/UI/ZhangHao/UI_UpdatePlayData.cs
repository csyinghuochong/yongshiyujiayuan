using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class UI_UpdatePlayData : MonoBehaviour {

    private string zhangHaoIDStr;
    public GameObject ZhangHaoID;
    public GameObject ZhangHaoMiMa;

    //修改密码
    public GameObject XiuGaiObj;
    public GameObject ZhangHaoMiMa_ID;
    public GameObject ZhangHaoMiMa_YuanShi;
    public GameObject ZhangHaoMiMa_XiuGai_1;
    public GameObject ZhangHaoMiMa_XiuGai_2;

	// Use this for initialization
	void Start () {
        zhangHaoIDStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        ZhangHaoID.GetComponent<Text>().text = zhangHaoIDStr;
        ZhangHaoMiMa_ID.GetComponent<Text>().text = zhangHaoIDStr;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //上传服务器
    public void Btn_UpdatePlayData() {

        string zhangHaoMiMa = ZhangHaoMiMa.GetComponent<InputField>().text;
        string mima = ZhangHaoMiMa.GetComponent<InputField>().text;

        bool chineseStatus = ContainChinese(mima);
        
        if (chineseStatus) {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请不要输入中文字符！");
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_127");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }

        if (zhangHaoMiMa != "")
        {
            //string[] saveList = new string[] { zhangHaoIDStr, zhangHaoMiMa,""};
            string[] saveList = new string[] { mima, "1","预留设备号位置","7" };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001006, saveList);
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_128");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("账号不能为空！");
        }
    }

    //修改密码
    public void Btn_XiuGaiMiMa() {

        string zhangHaoID = zhangHaoIDStr;
        string mima_yuanShi = ZhangHaoMiMa_YuanShi.GetComponent<InputField>().text;
        string mima_XiuGai_1 = ZhangHaoMiMa_XiuGai_1.GetComponent<InputField>().text;
        string mima_XiuGai_2 = ZhangHaoMiMa_XiuGai_2.GetComponent<InputField>().text;

        //发送修改数据
        if (mima_XiuGai_1 == mima_XiuGai_2)
        {
            //判断是否有中文字符
            bool chineseStatus = ContainChinese(mima_XiuGai_1);
            if (chineseStatus)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_129");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请不要输入中文字符！");
                return;
            }

            string[] mimaList = new string[] { zhangHaoID, mima_yuanShi, mima_XiuGai_1 };
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001009, mimaList);
        }
        else {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_130");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请检查！两次确认密码不一致！");
        }
    }

    //修改密码打开
    public void Btn_XiuGaiOpen(){
        XiuGaiObj.SetActive(true);
    }

    //修改密码关闭
    public void Btn_XiuGaiClose() {
        XiuGaiObj.SetActive(false);
    }

    //界面关闭
    public void Btn_Close()
    {
        Destroy(this.gameObject);
    }

    //判定是否中文
    static bool ContainChinese(string input)
    {
        string pattern = "[\u4e00-\u9fbb]";
        return Regex.IsMatch(input, pattern);
    }
}
