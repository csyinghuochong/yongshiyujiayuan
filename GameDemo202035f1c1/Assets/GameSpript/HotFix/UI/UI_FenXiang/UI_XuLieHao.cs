using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_XuLieHao : MonoBehaviour {

    public GameObject Obj_WeiXinSet;
    public GameObject Obj_QQSet;

    public GameObject Obj_Btn_WeiXin;
    public GameObject Obj_Btn_QQ;

    public GameObject Obj_XuLieHaoID;

    public GameObject QQErWeiMa;                     //qq群二维码
    public GameObject Obj_QQID;
    // Use this for initialization
    void Start () {



        Btn_WeiXin();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //打开微信
    public void Btn_WeiXin() {

        Obj_WeiXinSet.SetActive(true);
        Obj_QQSet.SetActive(false);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_23_2", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_WeiXin.GetComponent<Image>().sprite = img;

        //显示按钮
        obj = Resources.Load("GameUI/" + "Btn/Btn_23_1", typeof(Sprite));
        img = obj as Sprite;
        Obj_Btn_QQ.GetComponent<Image>().sprite = img;

    }

    //打开QQ
    public void Btn_QQ() {

        Obj_WeiXinSet.SetActive(false);
        Obj_QQSet.SetActive(true);

        //显示按钮
        object obj = Resources.Load("GameUI/" + "Btn/Btn_23_1", typeof(Sprite));
        Sprite img = obj as Sprite;
        Obj_Btn_WeiXin.GetComponent<Image>().sprite = img;

        //显示按钮
        obj = Resources.Load("GameUI/" + "Btn/Btn_23_2", typeof(Sprite));
        img = obj as Sprite;
        Obj_Btn_QQ.GetComponent<Image>().sprite = img;

        //GetQQErWeiMa(Game_PublicClassVar.Get_wwwSet.QQErWeiMaStr);
        this.StartCoroutine(GetQQErWeiMa(Game_PublicClassVar.Get_wwwSet.QQErWeiMaStr));
        Obj_QQID.GetComponent<Text>().text = Game_PublicClassVar.Get_wwwSet.QQqunID;
    }

    public void Btn_LingQuXuLieHao() {

        string xulieHaoID = Obj_XuLieHaoID.GetComponent<InputField>().text;
        //Debug.Log("输入值:" + xulieHaoID);
        if (xulieHaoID == "")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_413");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("请输入序列号!");
            return;
        }
        if (Game_PublicClassVar.Get_function_Rose.ifGetXuLieHao(xulieHaoID))
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_414");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameHint("序列号已被领取!");
            return;
        }

        switch (xulieHaoID)
        {
            //微信的
            case "weijing666":
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(4))
                {

                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("1", 50000);          //5万金币
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验魔盒5个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010096", 1);       //洗练石袋子
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10015001", 1);       //英勇装备宝盒

                    //更新UI显示状态
                    //Btn_XueLieHaoSet();
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";
                }
                else {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留至少4个背包位置");
                }

                break;

            //qq的
            case "woaizuozhe":

                langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(4))
                {

                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示   10010107

                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016", 1);       //藏宝图1张
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010041", 5);       //经验魔盒5个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010083", 1);       //宠灵露1个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010111", 5);       //召唤狗铃铛

                    //更新UI显示状态
                    //Btn_XueLieHaoSet();
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";
                }
                else
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留至少4个背包位置");
                }
                break;


            //补发坐骑
            case "zuoqi":

                string nowRMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (nowRMBPayValue != "" && nowRMBPayValue != null) {

                    float payValue = float.Parse(nowRMBPayValue);
                    if (payValue >= 198)
                    {
                        //获取
                        if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(1))
                        {
                            string nowZuoQiPiFuSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                            if (nowZuoQiPiFuSet.Contains("10007") == false)
                            {
                                //写入序列号ID
                                Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                                Game_PublicClassVar.Get_function_Rose.SendRewardToBag("12000007", 1);       //圣光战熊
                            }
                            else
                            {
                                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经获得了圣光战熊,请勿重复领取");
                            }
                        }
                        else
                        {
                            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留至少1个背包位置");
                        }
                    }
                    else {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未达获得领取权限");
                    }

                }

                break;

            case "jiayuanchengjiu":

                //激活坐骑
                string nowZuoQiPiFuSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                if (nowZuoQiPiFuSetStr != "" && nowZuoQiPiFuSetStr != "0" && nowZuoQiPiFuSetStr != null) {

                    string[] pifuList = nowZuoQiPiFuSetStr.Split(';');
                    for (int i = 0; i < pifuList.Length; i++) {
                        if (pifuList[i]!=""&& pifuList[i] != "0"& pifuList[i] != null) {
                            //写入坐骑成就
                            Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("221", pifuList[i], "1");
                        }
                    }
                }

                //激活家园等级
                //写入成就
                string nowPastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string nowLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", nowPastureLv, "PastureUpLv_Template");
                
                for (int i = 1; i <= int.Parse(nowLv); i++) {
                    Game_PublicClassVar.Get_function_Task.ChengJiu_WriteValue("220", "0", i.ToString());
                }

                //写入序列号ID
                Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);

                break;


            //微信的
            case "woaibibi":
                //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_415");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你领取哔哔专属福利成功,奖励已发往背包,请点击查看！");
                //Game_PublicClassVar.Get_function_UI.GameHint("领取组织福利成功,奖励已发往背包,请点击查看！");
                //检测背包格子
                if (Game_PublicClassVar.Get_function_Rose.IfBagNullNum(4))
                {

                    //写入序列号ID
                    Game_PublicClassVar.Get_function_Rose.WriteXuLieHao(xulieHaoID);
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";        //清空显示

                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10010035", 1);       //活力恢复药剂1个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000024", 2);       //复活石2个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000011", 1);       //小型宝石袋1个
                    Game_PublicClassVar.Get_function_Rose.SendRewardToBag("10000016", 1);       //藏宝图1张

                    //更新UI显示状态
                    //Btn_XueLieHaoSet();
                    Obj_XuLieHaoID.GetComponent<InputField>().text = "";
                }
                else
                {
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请预留至少4个背包位置");
                }

                break;

            default:

                Pro_ComStr_4 comStr_4 = new Pro_ComStr_4();
                comStr_4.str_1 = xulieHaoID;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001055, comStr_4);

                break;
        }


        //超级链接


    }

    public void Btn_AddQQ() {

        Application.OpenURL(Game_PublicClassVar.Get_wwwSet.QQLnkStr);
    }

    //IEnumerator
    public IEnumerator GetQQErWeiMa(string linkStr)
    {

        //请求WWW
        WWW www = new WWW(linkStr);
        yield return www;
        if (www != null && string.IsNullOrEmpty(www.error))
        {
            //获取Texture
            Texture2D texture = www.texture;
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            QQErWeiMa.GetComponent<Image>().sprite = sprite;
            Debug.Log("下载完成:" + linkStr);
        }
    }

}
