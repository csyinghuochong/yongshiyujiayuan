using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.Net.Http;

public class UI_RmbStore : MonoBehaviour {

    public ObscuredBool buyStatus;              //当开启支付时打开此状态
    public ObscuredBool buyQueryStatus;        //当开启支付查询时打开此状态
    //private string payValue;                  //支付额度
    private ObscuredFloat rmbPayValue;          //当前支付额度
    private ObscuredInt rmbToZuanShiValue;      //人民币兑换钻石数量
    private ObscuredString buyStatusStr;
    private ObscuredString payStatusType;       //支付返回状态类型值
    private ObscuredString payStatusStr;        //支付返回字符串
    private ObscuredString payStr;              //支付返回值
    private ObscuredString payType;             //支付类型  1:表示支付宝   2：表示微信
    public ObscuredBool clickPayBtnStatus;      //点击支付按钮后开启此状态,保证当前只有一个支付调用

    public ObscuredBool IfGooglePlay;

    public GameObject payText;
    public GameObject Obj_RoseZuanShiValue;
    public GameObject Obj_ImgZhiFuBao;
    public GameObject Obj_ImgWeiXin;
    public GameObject Obj_Select_WeiXin;
    public GameObject Obj_Select_ZhiFuBao;
    public GameObject Obj_ChongZhiHint;
    public GameObject Obj_SelectSet;
    public GameObject Obj_PayHint;

    public ObscuredBool IOSBuyStatus;
    public GameObject IOSBuyHint;
    public GameObject IOSBuyHintText;

    public float HuaWeiChaXunSum;
    public bool HuaWeiChaXunStatus;

    public bool IOSInitStatus =false;

#if UNITY_IPHONE
    /*
    [DllImport("__Internal")]
    private static extern void InitIAPManager();//初始化

    [DllImport("__Internal")]
    private static extern bool IsProductAvailable();//判断是否可以购买

    [DllImport("__Internal")]
    private static extern void RequstProductInfo(string s);//获取商品信息

    [DllImport("__Internal")]
    private static extern void BuyProduct(string s);//购买商品
    */
#endif

    // Use this for initialization
    void Start () {
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore = this.gameObject;

#if UNITY_IPHONE

        //初始化
        //InitIAPManager();
        Obj_Select_WeiXin.SetActive(false);
        Obj_Select_ZhiFuBao.SetActive(false);
        //隐藏安卓支付选项
        Obj_SelectSet.SetActive(false);

#endif

        int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
        Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString()+"钻";

        //获取当前支付方式
        payType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PayType", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        if (payType == "" || payType == "0") {
            payType = "1";
        }

        seclectPayType(payType);

        //将服务器连接绑定值此处

        //显示群号
        if (Game_PublicClassVar.Get_wwwSet.QQqunID != "" && Game_PublicClassVar.Get_wwwSet.QQqunID != "0") {
            if (EventHandle.IsHuiWeiChannel()==false)
            {
                Obj_ChongZhiHint.GetComponent<Text>().text = "如果仍未到账,请加玩家群：" + Game_PublicClassVar.Get_wwwSet.QQqunID + "  找管理解决即可！";
            }
        }

        //如果当前为英文版本，则切换为谷歌支付模式
        if(Game_PublicClassVar.Get_wwwSet.IfGooglePay){
            IfGooglePlay = true;
        }

        if (IfGooglePlay || EventHandle.IsHuiWeiChannel()) {
            Obj_Select_WeiXin.SetActive(false);
            Obj_Select_ZhiFuBao.SetActive(false);
            //隐藏安卓支付选项
            Obj_SelectSet.SetActive(false);
            Obj_PayHint.SetActive(false);
        }

        //华为打开默认查询一次
        if (EventHandle.IsHuiWeiChannel()) {
            HuaWeiChaXunStatus = true;
            //Btn_ChaXun();
        }
        

        IOSBuyHint.SetActive(false);

#if UNITY_IPHONE
        //YanZhengIosPay();
#endif
    }

    // Update is called once per frame
    void Update () {

        if (HuaWeiChaXunStatus) {
            HuaWeiChaXunSum = HuaWeiChaXunSum + Time.deltaTime;
            if (HuaWeiChaXunSum >= 1.5f) {
                HuaWeiChaXunStatus = false;
                Btn_ChaXun();
            }
        }


        if (Game_PublicClassVar.Get_game_PositionVar.PayStatusIOS) {
            Game_PublicClassVar.Get_game_PositionVar.PayStatusIOS = false;

            //ios发送
            try
            {
#if UNITY_IPHONE
#endif
                string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                //参数：协议号,账号ID,平台ID,交易金额,交易状态,IOS查询Base64位码            重要备注：第五个参数 1 表示原来自己写的ios内购  2表示用的unity自带的iap  因为 1和2在服务器的解析方式参数名不相同
                string sendStr = "IOSPay," + zhanghaoID + "," + "3" + "," + rmbPayValue.ToString() + "," + "2" + "," + Game_PublicClassVar.Get_game_PositionVar.PayIOS_Str;
				Debug.Log("sendStr = " + sendStr);
				this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);

                IOSBuyHintText.GetComponent<Text>().text = "正在支付效验订单..";
                IOSBuyHint.SetActive(true);

            }
            catch(Exception ex)
            {
                Debug.LogError("IOS支付发送信息报错:" + ex);
            }

        }


        //Debug.Log("rmbToZuanShiValue = " + rmbToZuanShiValue);
        buyStatus = Game_PublicClassVar.Get_game_PositionVar.PayStatus;
        if (buyStatus) {
            payStr = "支付状态开启:" + Game_PublicClassVar.Get_game_PositionVar.PayStr;
            payStatusType = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[0];
            payStatusStr = Game_PublicClassVar.Get_game_PositionVar.PayStr.ToString().Split(';')[1];
            switch (payStatusType)
            { 
                //支付状态
                case "0":
                    payStr = "支付中……" + payStatusStr;
                    break;

                //支付成功
                case "1":
                    //删除充值记录
                    //删除充值记录
                    Game_PublicClassVar.Get_function_Rose.DeletePayID(Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow);
                    payStr = rmbPayValue + "支付成功！" + payStatusStr;
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);

                    //累计当前充值额度,发送指定钻石奖励
                    //Game_PublicClassVar.Get_function_Rose.AddRMB(rmbToZuanShiValue);
                    //Game_PublicClassVar.Get_function_Rose.AddPayValue(rmbPayValue);
                    //int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
                    //Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString()+"钻";

                    //清理支付值
                    //float youmengRmbValue = rmbPayValue;
                    rmbPayValue = 0;
                    ClearnPayValue();       //清理支付值

                    /*
                    //发送友盟支付信息
                    try
                    {
                        GA.Pay(youmengRmbValue, GA.PaySource.Source10, rmbToZuanShiValue);
                    }
                    catch
                    {
                        Debug.Log("充值报错！");
                    }
                    */
                    rmbToZuanShiValue = 0;
                    
                    break;

                //支付失败
                case "2":
                    payStr = "支付失败！" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    rmbToZuanShiValue = 0;
                    rmbPayValue = 0;

                    //返回App时,会出现纵向显示
                    if (Screen.orientation != ScreenOrientation.LandscapeLeft&& Screen.orientation != ScreenOrientation.LandscapeRight) {
                        Screen.orientation = ScreenOrientation.LandscapeLeft;
                    }    

#if UNITY_ANDROID

                    //QueryDingDanStatus();   //支付失败调用一次检查
                    if (payType == "1") {
                        //防止支付宝第一次调用错误
                        int ZhiFuBaoOpenNum = PlayerPrefs.GetInt("ZhiFuBaoOpenNum");
                        if (ZhiFuBaoOpenNum == 0)
                        {
                            PlayerPrefs.SetInt("ZhiFuBaoOpenNum", 1);
                            if (Game_PublicClassVar.Get_game_PositionVar.PayValueNow != "")
                            {
                                Btn_BuyZuanShi(Game_PublicClassVar.Get_game_PositionVar.PayValueNow);
                            }
                        }
                    }

                    /*
                    if (payStatusStr.IndexOf("disable") != -1) {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("微信支付暂时关闭,请使用支付宝支付!!!");
                    }
                    */

                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
#endif
                    break;

                //其他原因
                case "3":
                    payStr = "支付未知原因！" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
                    break;

                default :
                    payStr = "支付default" + payStatusStr;
                    ClearnPayValue();       //清理支付值
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
                    break;

                Debug.Log("payStr: " + payStr);
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(payStr);
            }
        }

        payText.GetComponent<Text>().text = payStr;
        /*
        buyQueryStatus = Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus;
        if (buyQueryStatus) {
            payStr = "查询状态：" + Game_PublicClassVar.Get_game_PositionVar.PayStrQueryStatus;
        }
        payText.GetComponent<Text>().text = payStr;
        */

        //更新钻石显示
        if (Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus) {
            Game_PublicClassVar.Get_game_PositionVar.PayQueryStatus = false;
            int zuanShi = Game_PublicClassVar.Get_function_Rose.GetRoseRMB();
            Obj_RoseZuanShiValue.GetComponent<Text>().text = zuanShi.ToString() + "钻";
        }
	}
    
    //清理支付信息
    public void ClearnPayValue() {

        Debug.Log("清理支付信息！");
        clickPayBtnStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.PayStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.PayStr = "";
        Game_PublicClassVar.Get_game_PositionVar.PayIOS_Str = "";
        //Game_PublicClassVar.Get_game_PositionVar.PayIOSYanZhengStr = "";
        buyStatus = false;
        IOSBuyStatus = false;
        IOSBuyHintText.GetComponent<Text>().text = "";
        IOSBuyHint.SetActive(false);
        buyQueryStatus = false;

    }

    //根据支付额度返回应获取的额度
    private int ReturnPayValue(string payValue){
        int returnZuanShiValue = 0;
        switch (payValue) {
            /*
            case "0.03":
                  returnZuanShiValue = 6000;
                  Debug.Log("匹配到了");
                  break;
        
          
              case "6":
                  returnZuanShiValue = 5000;
                  break;
              */

            //安卓付费额度
#if UNITY_ANDROID
             
            //新付费
            case "6":
                returnZuanShiValue = 600;
                break;
            case "30":
                returnZuanShiValue = 3300;
                break;
            case "50":
                returnZuanShiValue = 6000;
                break;
            case "98":
                returnZuanShiValue = 11000;
                break;
            case "198":
                returnZuanShiValue = 22688;
                break;
            case "298":
                returnZuanShiValue = 34688;
                break;
            case "488":
                returnZuanShiValue = 57688;
                break;
            case "648":
                returnZuanShiValue = 77688;
                break;


#endif

                //ios付费额度
#if UNITY_IPHONE
            //新付费
            case "6":
                returnZuanShiValue = 600;
                break;
            case "30":
                returnZuanShiValue = 3300;
                break;
            case "50":
                returnZuanShiValue = 6000;
                break;
            case "98":
                returnZuanShiValue = 11000;
                break;
            case "198":
                returnZuanShiValue = 22688;
                break;
            case "298":
                returnZuanShiValue = 34688;
                break;
            case "488":
                returnZuanShiValue = 57688;
                break;
            case "648":
                returnZuanShiValue = 77688;
                break;
#endif
        }
        return returnZuanShiValue;
    }


    private void Btn_ClickQudaoBuy(string rmbValue)
    {
        rmbPayValue = float.Parse(rmbValue);
        //其他验证逻辑
        this.GetComponent<GamePayLinkServer>().Btn_QudaoPay(rmbPayValue.ToString());
    }

    public void Btn_BuyZuanShi(string rmbValue)
    {
        if (EventHandle.IsQudaoPackage())
        {
            Btn_ClickQudaoBuy(rmbValue);
            return;
        }

        Debug.Log("我点击了支付按钮[非渠道包！]:" + rmbValue);
        if(clickPayBtnStatus){
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_30");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付开启中,请稍等……！");
            return;
        }

        //游客不能充值
        if (Game_PublicClassVar.Get_wwwSet.IfYouKeStatus && Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus)
        {
            GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
            uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家相关规定,未实名用户和8岁以下未成年人无法进行游戏充值。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "确认", "前往实名");
            uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            uiCommonHint.transform.localPosition = Vector3.zero;
            uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            return;
        }
        

        //判断是否是未成年支付
        if (Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus)
        {
            if (Game_PublicClassVar.Get_gameLinkServerObj.QiangZhiShiMingStatus)
            {
                int nowPay = PlayerPrefs.GetInt("FangChenMi_Pay");          //获取当前时间
                bool hintStatus = false;

                //单次额度不得超过50
                if (rmbValue != "" && rmbValue != null)
                {
                    if (float.Parse(rmbValue) > 50) {
                        hintStatus = true;
                    }

                    //累计额度不得超过200
                    if (nowPay >= 400)
                    {
                        hintStatus = true;
                    }

                    if (hintStatus)
                    {
                        GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint_2);
                        uiCommonHint.GetComponent<UI_CommonHint>().Obj_HintText.GetComponent<Text>().fontSize = 20;
                        uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您好！\n根据国家相关规定,未实名或未成年用户单次充值不能超过50,累计充值不能超过400元。", null, Game_PublicClassVar.Get_wwwSet.OpenFangChenMiYanZheng, "温馨提示", "确认", "确认");
                        uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                        uiCommonHint.transform.localPosition = Vector3.zero;
                        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);

                        return;
                    }
                }
            }
        }

        //clickPayBtnStatus = true;
        //判定当前是否在支付状态,如果是,则取消本次支付
        if (buyStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_31");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付繁忙,请稍后支付,如长时间无反应请重启游戏应用！");
            return;
        }
        else
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_32");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付转接中,请不要关闭界面……");
        }

#if UNITY_IPHONE
        if (IOSBuyStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_32");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("支付转接中,请不要关闭界面……");
            return;
        }

        if (buyQueryStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请等待查询结束后在进行支付...");
            return;
        }

        IOSBuyStatus = true;

        IOSBuyHintText.GetComponent<Text>().text = "正在发送支付请求,请稍等..";
        IOSBuyHint.SetActive(true);

#endif

        string shopName = "感谢您的赞助";     //最多6个字
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string shopDes = "赞助角色-" + roseName + "ID-" + zhanghaoID + " 等级-" + roseLv + " 金币-" + GoldNum + " 钻石-" + RMB + "已赞助-" + RMBPayValue + "安卓支付";

        //英语设置订单的金额存储
        Game_PublicClassVar.Get_game_PositionVar.PayValueNow = rmbValue;

        //根据选择调用不同平台的支付接口(暂时调用支付宝)
        rmbPayValue = float.Parse(rmbValue);
        Debug.Log("rmbPayValue = " + rmbPayValue);
        rmbToZuanShiValue = ReturnPayValue(rmbValue);

        //ios付费
#if UNITY_IPHONE
        //BuyProduct(rmbPayValue + "SG");
        Game_PublicClassVar.Get_wwwSet.gameObject.GetComponent<ShopList>().BuyProductID("iospay" + rmbPayValue);
#endif


        //安卓付费
#if UNITY_ANDROID

        //是否为谷歌支付
        if (IfGooglePlay)
        {
            string googlePayStr = "pay_1";
            switch (rmbValue) {

                case "6":
                    googlePayStr = "pay_1";
                    break;

                case "30":
                    googlePayStr = "pay_2";
                    break;

                case "50":
                    googlePayStr = "pay_3";
                    break;

                case "98":
                    googlePayStr = "pay_4";
                    break;

                case "198":
                    googlePayStr = "pay_5";
                    break;

                case "298":
                    googlePayStr = "pay_6";
                    break;

                case "488":
                    googlePayStr = "pay_7";
                    break;

                case "648":
                    googlePayStr = "pay_8";
                    break;
            }

            this.GetComponent<GooglePay>().BuyProduct(googlePayStr);

            return;
        }



        if (this.GetComponent<GamePayLinkServer>().LinkPayServerStatus)
        {

            //调用服务器支付
            //调用本地支付
            switch (payType)
            {
                //支付宝
                case "1":
                    this.GetComponent<GamePayLinkServer>().Btn_AliPay(rmbPayValue.ToString());
                    break;
                //微信
                case "2":
                    this.GetComponent<GamePayLinkServer>().Btn_WeiXinPay(rmbPayValue.ToString());
                    break;
            }

        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_34");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("未连接支付服务器,请确认本地网络或加游戏群联系管理员！");

            /*
            //调用本地支付
            switch (payType)
            {
                //支付宝
                case "1":
                    Buy_ZhiFuBao(shopName, shopDes, rmbPayValue);
                    break;
                //微信
                case "2":
                    Buy_WeiXin(shopName, shopDes, rmbPayValue);
                    break;
            }

            //去掉订单的最前面10个
            string dingdanStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            string[] dingdanIDList = dingdanStatus.Split(';');
            string saveDingDan = "";
            if (dingdanIDList.Length >= 10)
            {
                for (int i = 0; i <= dingdanIDList.Length - 1; i++)
                {
                    if (i >= 5)
                    {
                        if (saveDingDan == "")
                        {
                            saveDingDan = saveDingDan + dingdanIDList[i];
                        }
                        else
                        {
                            saveDingDan = saveDingDan + ";" + dingdanIDList[i];
                        }
                    }
                }

                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_1", saveDingDan, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            }
            */
        }
#endif

        //ReturnPayValue("a");
        //Buy_WeiXin(shopName, shopDes, ddd);
        //int getPayValue = 0;    //重置支付金额
    }
    
    //支付宝支付
    public void Buy_ZhiFuBao(string shopName,string shopDes, float value)
    {
#if UNITY_ANDROID
        
        Debug.Log("付费" + value + "元");
        payStr = "调用支付方法";
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //activity.Call("Pay_ZhiFuBao", shopName, shopDes, value);         //调用支付方法
        string payValueStr = value.ToString();
        activity.Call("Pay_ZhiFuBao", payValueStr, shopDes);                     //调用支付方法
        
#endif
    }


    //微信支付
    public string Buy_WeiXin(string shopName, string shopDes,float value)
    {
#if UNITY_ANDROID
        
        Debug.Log("Value = " + value);
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
        //activity.Call("Pay_WeiXin", shopName, shopDes, value);        //调用支付方法
        string payValueStr = value.ToString();
        activity.Call("Pay_WeiXin", payValueStr);                       //调用支付方法
        
#endif
        return buyStatusStr;

    }

    //选择
    public void BtnSeclect_ZhiFuBao() {
        payType = "1";
        seclectPayType(payType);
            //Obj_WeiXinChaJianHint.SetActive(false);
    }

    //选择
    public void BtnSeclect_WeiXin() {
        payType = "2";
        seclectPayType(payType);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示:微信支付需要安装安全支付插件！");
        //Obj_WeiXinChaJianHint.SetActive(true);

    }

    void seclectPayType(string payTypeStr)
    {

#if UNITY_ANDROID
        switch (payTypeStr)
        { 
            //支付宝
            case "1":
                Obj_ImgZhiFuBao.SetActive(true);
                Obj_ImgWeiXin.SetActive(false);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PayType", payTypeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            break;

            //微信
            case "2":
                Obj_ImgZhiFuBao.SetActive(false);
                Obj_ImgWeiXin.SetActive(true);
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("PayType", payTypeStr, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
            break;
        }
#endif
    }
    /*
    //调用查询按钮
    public void QueryDingDanStatus() {

#if UNITY_ANDROID
        string dingdanStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
        //string dingdanStatus = "62918c959364ba417c60313592d48444;469ee4e36115f83824674bd6885efdbe;63c01e3f9532e0e095193ac80ce0e4fe;4cf61e34c23a9d1b2bdad5c939b01038";
        string[] dingdanIDList = dingdanStatus.Split(';');
        for (int i = 0; i <= dingdanIDList.Length - 1; i++) {
            //发送查询信息
            if (dingdanIDList[i] != "0" && dingdanIDList[i] != "") {
                AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject activity = jc.GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("Query", dingdanIDList[i].Split(',')[0]);         //调用支付方法
            }
        }
#endif
    }
    */
    
    public void OnPayResultReturn(string str)
    {
        Debug.Log("调用了支付返回值str = " + str);
        payStr = "调用付费返回值";
        //payStr = str;
        if (str != "") {
            payStatusType = str.Split(';')[0];
            payStatusStr = str.Split(';')[1];
        }

        buyStatus = true;   //开启支付状态
    }


    public void Btn_ChaXun() {
        if (!buyQueryStatus)
        {
            this.GetComponent<GamePayLinkServer>().QueDingDan();
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前正在查询中，查询时间较长, 如需再次查询，请关闭界面重新点击查询...");
        }

#if UNITY_IPHONE

        if (IOSBuyStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("购买中,请不要点击查询相关功能...");
        }

        if (buyQueryStatus) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前正在查询中，查询时间较长, 如需再次查询，请关闭界面重新点击查询...");
        }
        else {
            //string nowIOSPayStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IOSPayStr", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            string nowIOSPayStr = PlayerPrefs.GetString("iosPayDingDan");
            Debug.Log("查询查询查询查询查询查询查询nowIOSPayStr = " + nowIOSPayStr);

            if (nowIOSPayStr != "" && nowIOSPayStr != null && nowIOSPayStr != "0")
            {
                string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string[] nowIOSPayStrList = nowIOSPayStr.Split(';');
                for (int i = 0; i < nowIOSPayStrList.Length; i++)
                {
                    if (nowIOSPayStrList[i] != "" && nowIOSPayStrList[i] != null && nowIOSPayStrList[i] != "0")
                    {

                        //参数：协议号,账号ID,平台ID,交易金额,交易状态,IOS查询Base64位码
                        string sendStr = "IOSPay," + zhanghaoID + "," + "3" + "," + rmbPayValue.ToString() + "," + "2" + "," + nowIOSPayStrList[i];
                        Debug.Log("查询查询查询查询查询查询查询sendStr = " + sendStr);
                        this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);

                        buyQueryStatus = true;

                        IOSBuyHintText.GetComponent<Text>().text = "有未兑付的订单正在查询..";
                        IOSBuyHint.SetActive(true);
                    }
                }

                //累计查询次数
                string nowChaXunNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChaXunNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Debug.Log("当前查询次数:" + nowChaXunNum);
                if (nowChaXunNum == "" || nowChaXunNum == null) {
                    int writeInt = int.Parse(nowChaXunNum)+1;
                    //查询超过10次 清空存储
                    if (writeInt >= 10) {
                        //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("IOSPayStr", "","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                        PlayerPrefs.SetString("iosPayDingDan","");
                        PlayerPrefs.Save();
                        writeInt = 0;
                    }

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ChaXunNum", writeInt.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }

            }
            else
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你当前未有需要查询的订单");
            }
        }


#endif


        /*

        //参数：协议号,账号ID,平台ID,交易金额,交易状态,IOS查询Base64位码
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        string sendStr = "IOSPay," + zhanghaoID + "," + "3" + "," + rmbPayValue.ToString() + "," + "1" + "," + "ewoJInNpZ25hdHVyZSIgPSAiQXdKUlNwSS9KMm1OV2l3bndIeU80Z1pLNEpvYm9LbHJwamNqaDBMZ3VtYjdKSVRlTXk3UWtIU081NHZnT2dMUW9qdlYrdzJtSHZEZjFCZXJvS0p4M0FzUmJLaXo3UExReXBWcnI4VUw3T2gzZmZVZE1ZaFRQY29SYkhNbTQ4SE41VGhkaFl6RTIxLzFTcnVHQ2ltVkRDN3BHb002cEpFbVRtU3RPTlhuSzJ3dDF4cjhRZWhsYTZIUlljUGcrbVlIaHdsbFVPTkJBYlJ0NEhCUEE1aW1wRHJ6bU4xM3BFUHlXOWhpamxhaHVETEZFMTZTMzVhSW5aNnk1d3BGNElnQTAzWmZUd1JnejJaQ3NybittM0FldzM2bitlcWlqK1djWU13RVkxbHNTQ001bW5UZTdyT2RBWkVCdjBJU1dicVc2ai8rTGRveHhOYkRNUHo3dnI2TDJjNEFBQVdBTUlJRmZEQ0NCR1NnQXdJQkFnSUlEdXRYaCtlZUNZMHdEUVlKS29aSWh2Y05BUUVGQlFBd2daWXhDekFKQmdOVkJBWVRBbFZUTVJNd0VRWURWUVFLREFwQmNIQnNaU0JKYm1NdU1Td3dLZ1lEVlFRTERDTkJjSEJzWlNCWGIzSnNaSGRwWkdVZ1JHVjJaV3h2Y0dWeUlGSmxiR0YwYVc5dWN6RkVNRUlHQTFVRUF3dzdRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTWdRMlZ5ZEdsbWFXTmhkR2x2YmlCQmRYUm9iM0pwZEhrd0hoY05NVFV4TVRFek1ESXhOVEE1V2hjTk1qTXdNakEzTWpFME9EUTNXakNCaVRFM01EVUdBMVVFQXd3dVRXRmpJRUZ3Y0NCVGRHOXlaU0JoYm1RZ2FWUjFibVZ6SUZOMGIzSmxJRkpsWTJWcGNIUWdVMmxuYm1sdVp6RXNNQ29HQTFVRUN3d2pRWEJ3YkdVZ1YyOXliR1IzYVdSbElFUmxkbVZzYjNCbGNpQlNaV3hoZEdsdmJuTXhFekFSQmdOVkJBb01Da0Z3Y0d4bElFbHVZeTR4Q3pBSkJnTlZCQVlUQWxWVE1JSUJJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBUThBTUlJQkNnS0NBUUVBcGMrQi9TV2lnVnZXaCswajJqTWNqdUlqd0tYRUpzczl4cC9zU2cxVmh2K2tBdGVYeWpsVWJYMS9zbFFZbmNRc1VuR09aSHVDem9tNlNkWUk1YlNJY2M4L1cwWXV4c1FkdUFPcFdLSUVQaUY0MWR1MzBJNFNqWU5NV3lwb041UEM4cjBleE5LaERFcFlVcXNTNCszZEg1Z1ZrRFV0d3N3U3lvMUlnZmRZZUZScjZJd3hOaDlLQmd4SFZQTTNrTGl5a29sOVg2U0ZTdUhBbk9DNnBMdUNsMlAwSzVQQi9UNXZ5c0gxUEttUFVockFKUXAyRHQ3K21mNy93bXYxVzE2c2MxRkpDRmFKekVPUXpJNkJBdENnbDdaY3NhRnBhWWVRRUdnbUpqbTRIUkJ6c0FwZHhYUFEzM1k3MkMzWmlCN2o3QWZQNG83UTAvb21WWUh2NGdOSkl3SURBUUFCbzRJQjF6Q0NBZE13UHdZSUt3WUJCUVVIQVFFRU16QXhNQzhHQ0NzR0FRVUZCekFCaGlOb2RIUndPaTh2YjJOemNDNWhjSEJzWlM1amIyMHZiMk56Y0RBekxYZDNaSEl3TkRBZEJnTlZIUTRFRmdRVWthU2MvTVIydDUrZ2l2Uk45WTgyWGUwckJJVXdEQVlEVlIwVEFRSC9CQUl3QURBZkJnTlZIU01FR0RBV2dCU0lKeGNKcWJZWVlJdnM2N3IyUjFuRlVsU2p0ekNDQVI0R0ExVWRJQVNDQVJVd2dnRVJNSUlCRFFZS0tvWklodmRqWkFVR0FUQ0IvakNCd3dZSUt3WUJCUVVIQWdJd2diWU1nYk5TWld4cFlXNWpaU0J2YmlCMGFHbHpJR05sY25ScFptbGpZWFJsSUdKNUlHRnVlU0J3WVhKMGVTQmhjM04xYldWeklHRmpZMlZ3ZEdGdVkyVWdiMllnZEdobElIUm9aVzRnWVhCd2JHbGpZV0pzWlNCemRHRnVaR0Z5WkNCMFpYSnRjeUJoYm1RZ1kyOXVaR2wwYVc5dWN5QnZaaUIxYzJVc0lHTmxjblJwWm1sallYUmxJSEJ2YkdsamVTQmhibVFnWTJWeWRHbG1hV05oZEdsdmJpQndjbUZqZEdsalpTQnpkR0YwWlcxbGJuUnpMakEyQmdnckJnRUZCUWNDQVJZcWFIUjBjRG92TDNkM2R5NWhjSEJzWlM1amIyMHZZMlZ5ZEdsbWFXTmhkR1ZoZFhSb2IzSnBkSGt2TUE0R0ExVWREd0VCL3dRRUF3SUhnREFRQmdvcWhraUc5Mk5rQmdzQkJBSUZBREFOQmdrcWhraUc5dzBCQVFVRkFBT0NBUUVBRGFZYjB5NDk0MXNyQjI1Q2xtelQ2SXhETUlKZjRGelJqYjY5RDcwYS9DV1MyNHlGdzRCWjMrUGkxeTRGRkt3TjI3YTQvdncxTG56THJSZHJqbjhmNUhlNXNXZVZ0Qk5lcGhtR2R2aGFJSlhuWTR3UGMvem83Y1lmcnBuNFpVaGNvT0FvT3NBUU55MjVvQVE1SDNPNXlBWDk4dDUvR2lvcWJpc0IvS0FnWE5ucmZTZW1NL2oxbU9DK1JOdXhUR2Y4YmdwUHllSUdxTktYODZlT2ExR2lXb1IxWmRFV0JHTGp3Vi8xQ0tuUGFObVNBTW5CakxQNGpRQmt1bGhnd0h5dmozWEthYmxiS3RZZGFHNllRdlZNcHpjWm04dzdISG9aUS9PamJiOUlZQVlNTnBJcjdONFl0UkhhTFNQUWp2eWdhWndYRzU2QWV6bEhSVEJoTDhjVHFBPT0iOwoJInB1cmNoYXNlLWluZm8iID0gImV3b0pJbTl5YVdkcGJtRnNMWEIxY21Ob1lYTmxMV1JoZEdVdGNITjBJaUE5SUNJeU1ESXdMVEV4TFRJeElEQXlPalEyT2pNNElFRnRaWEpwWTJFdlRHOXpYMEZ1WjJWc1pYTWlPd29KSW5WdWFYRjFaUzFwWkdWdWRHbG1hV1Z5SWlBOUlDSmpaV1ZqWmpreVpqWTVZakptWW1ReE1qUmxNemsyTlRjeU4ySXpPV001T0RNNFlqazJNekkzSWpzS0NTSnZjbWxuYVc1aGJDMTBjbUZ1YzJGamRHbHZiaTFwWkNJZ1BTQWlNVEF3TURBd01EYzBORFk1TlRBeE1TSTdDZ2tpWW5aeWN5SWdQU0FpTVNJN0Nna2lkSEpoYm5OaFkzUnBiMjR0YVdRaUlEMGdJakV3TURBd01EQTNORFEyT1RVd01URWlPd29KSW5GMVlXNTBhWFI1SWlBOUlDSXhJanNLQ1NKdmNtbG5hVzVoYkMxd2RYSmphR0Z6WlMxa1lYUmxMVzF6SWlBOUlDSXhOakExT1RVMU5UazRNVEExSWpzS0NTSjFibWx4ZFdVdGRtVnVaRzl5TFdsa1pXNTBhV1pwWlhJaUlEMGdJakE1UVRBMVJUVTJMVVZDTWpRdE5EQXhOaTFCTkRrMUxUbEVNREZCTlRVMk5Ea3lRU0k3Q2draWNISnZaSFZqZEMxcFpDSWdQU0FpTmxOSElqc0tDU0pwZEdWdExXbGtJaUE5SUNJeE5URXdNamd4TXpZNUlqc0tDU0oyWlhKemFXOXVMV1Y0ZEdWeWJtRnNMV2xrWlc1MGFXWnBaWElpSUQwZ0lqQWlPd29KSW1sekxXbHVMV2x1ZEhKdkxXOW1abVZ5TFhCbGNtbHZaQ0lnUFNBaVptRnNjMlVpT3dvSkluQjFjbU5vWVhObExXUmhkR1V0YlhNaUlEMGdJakUyTURVNU5UVTFPVGd4TURVaU93b0pJbkIxY21Ob1lYTmxMV1JoZEdVaUlEMGdJakl3TWpBdE1URXRNakVnTVRBNk5EWTZNemdnUlhSakwwZE5WQ0k3Q2draWFYTXRkSEpwWVd3dGNHVnlhVzlrSWlBOUlDSm1ZV3h6WlNJN0Nna2liM0pwWjJsdVlXd3RjSFZ5WTJoaGMyVXRaR0YwWlNJZ1BTQWlNakF5TUMweE1TMHlNU0F4TURvME5qb3pPQ0JGZEdNdlIwMVVJanNLQ1NKaWFXUWlJRDBnSW1OdmJTNW5kV0Z1WjNscGJtY3VkMlZwYW1sdVp6SWlPd29KSW5CMWNtTm9ZWE5sTFdSaGRHVXRjSE4wSWlBOUlDSXlNREl3TFRFeExUSXhJREF5T2pRMk9qTTRJRUZ0WlhKcFkyRXZURzl6WDBGdVoyVnNaWE1pT3dwOSI7CgkiZW52aXJvbm1lbnQiID0gIlNhbmRib3giOwoJInBvZCIgPSAiMTAwIjsKCSJzaWduaW5nLXN0YXR1cyIgPSAiMCI7Cn0=";
        Debug.Log("sendStr = " + sendStr);
        this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);

        buyQueryStatus = true;
        */



    }


    public void Btn_Close() {

        if (clickPayBtnStatus == true)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_35");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("提示:你已充值,请耐心等待支付返回结果,不要关闭此界面和退出游戏！");
        }
        else {
            buyStatus = false;
            payStr = "清空";
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
            ClearnPayValue();

            //卸载支付服务器组件
            this.GetComponent<GamePayLinkServer>().ClosePayServerLink();
        }
    }

    public void Btn_Close1111()
    {
        //buyStatus = false;
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().Open_RmbStore();
    }

    public void TestGoogle() {

        /*
        string sendStr = "GooglePay," + "10015847140701581126" + "," + "4" + "," + "0" + "," + "1" + "," + "paytest3" + "," + "bchgfaepajggedohhhphgeai.AO-J1OxkpYNiy0vkYDNFsnW1OAccTAQC_LjM34DyFc5_2Y87Izth2Wc66qyivwXSvOMLKkti2WHb0Y2_rjjW1au9fTlZ6wfy7EZ1GKfhJh4fKJT14h8e2YYrdxORT_dxPOwVJtMWUZ9l";
        Debug.Log("sendStr = " + sendStr);
        this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);
        */

        //this.GetComponent<GooglePay>().BuyProduct("paytest3");
    }

    //验证IOS订单
    public void YanZhengIosPay() {

        ObscuredString iosPayData = PlayerPrefs.GetString("iosPay_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName);
        if (iosPayData != "" && iosPayData != "0" && iosPayData != null)
        {
            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            //参数：协议号,账号ID,平台ID,交易金额,交易状态,IOS查询Base64位码
            string sendStr = "IOSPay," + zhanghaoID + "," + "3" + "," + rmbPayValue.ToString() + "," + "1" + "," + Game_PublicClassVar.Get_game_PositionVar.PayIOS_Str;
            Debug.Log("sendStr = " + sendStr);
            this.GetComponent<GamePayLinkServer>().SendToServer(sendStr);

            IOSBuyHintText.GetComponent<Text>().text = "有未兑付的订单正在查询..";
            IOSBuyHint.SetActive(true);

            PlayerPrefs.SetString("iosPay_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName,"");
            PlayerPrefs.Save();
        }

    }
}
