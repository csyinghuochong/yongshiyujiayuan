using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class GamePayLinkServer : MonoBehaviour {

    public bool LinkPayServerStatus;        //是否连接交易服务器   
    private float linkStartTime;

    //服务器连接相关
    Socket tcpSocket;
    public string serverIP;
    public int serverPort;
    private EndPoint remoteEndPoint;

    //接收消息 发送消息的线程 
    Thread receiveThread, sendThread;

    //心跳包计时
    private float xintiaoTimeSum;
    private float xintiaoDuanKaiSum;

    //按钮计时（1秒钟只能点一次付费按钮）
    private float payBtnTimeSum;
    public bool PayingStatus;

    //缓存网络消息的队列
    Queue<string> netMsg = new Queue<string>();

    //微信支付参数
    public string appid;
    public string mch_id;
    public string prepayid;
    public string noncestr;
    public string timestamp;
    public string packageValue;
    public string sign;

    private string QudaoPayStr = "QudaoPay";

    //付费组件
#if UNITY_ANDROID
    AliComponent aliComponent;
    WeChatComponent weChatComponent;
    QudaoPayComponent qudaoPayComponent;


#endif
    // Use this for initialization
    void Start () {


#if UNITY_ANDROID
        //注册阿里组件的事件:同步回调通知 
        if (aliComponent == null)
        {
            aliComponent = this.GetComponent<AliComponent>();
            aliComponent.aliPayCallBack += AliPayCallback;
        }

        //注册微信组件的事件:同步回调通知 
        if (weChatComponent == null)
        {
            weChatComponent = this.GetComponent<WeChatComponent>();
            weChatComponent.weChatPayCallback += WeChatPayCallback;
        }

        //渠道充值组件
        if (qudaoPayComponent == null)
        {
            qudaoPayComponent = this.gameObject.AddComponent<QudaoPayComponent>();
            qudaoPayComponent.qudaoPayCallback += QudaoPayCallback;
        }
#endif

        StartLinkPayServer();   //连接付费服务器
        linkStartTime = 1;

        //Game_PublicClassVar.Get_game_PositionVar.OnPayResult("");
    }
	
	// Update is called once per frame
	void Update () {
        try
        {
            //处理网络消息 根据不同的协议 调用不同的消息
            if (netMsg.Count > 0)
            {
                string msg = netMsg.Dequeue();

                string[] msgStrList = msg.Split(';');
                for (int i = 0; i < msgStrList.Length; i++)
                {
                    if (msgStrList[i] != "")
                    {
                        string[] msgList = msgStrList[i].Split(',');
                        switch (msgList[0])
                        {
                            //心跳
                            case "xintiao":
                                xintiaoDuanKaiSum = 0;
                                break;

                            //发起支付的时候调用
                            case "AliPay":
                                Debug.Log("AliPay");
                                //调起sdk中的支付方法
                                AliPay(msgList);
                                break;

                            //微信支付
                            case "WeChatPay":
                                Debug.Log("WeChatPay");
                                //addHint("微信支付已经收到服务器生成字符向SDK发送数据:" + msg);
                                WechatPay(msgList);
                                break;

                            case "QudaoPay":
                                Debug.Log("QudaoPay回调用");
                                QudaoPay( msgList );
                                break;
                            //发送道具的时候调用
                            case "SendPay":
                                Debug.Log("收到Send收到Send收到Send收到Send");
                                //执行对应充值
                                string payStr = msgStrList[i].Substring(8, msgStrList[i].Length - 8);
                                //替换字符,因为在发送数据时；为发送信息间隔符
                                payStr = payStr.Replace('@', ';');
                                Debug.Log("payStr = " + payStr);

                                //跨线程调用(跨线程调用的值不能传出)
                                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                                {
                                    //Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
                                    Game_PublicClassVar.Get_game_PositionVar.OnPayResultReturn(payStr);
                                    //Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                                }));

                                string sendStr = msgStrList[i].Substring(8, msgStrList[i].Length - 8);
                                string[] sendStrList = sendStr.Split('@');

                                //消息协议 SendProps,会话ID/订单号，道具ID,数量...(道具实体)
                                string sendMsg = "GetPay"  + "," + sendStrList[3];
                                SendToServer(sendMsg);

                                break;
                            //发送道具的时候调用
                            case "IOSPay":
                                Debug.Log("收到IOSPay..." + msgStrList[i]);
                                //执行对应充值
                                payStr = msgStrList[i].Substring(7, msgStrList[i].Length - 7);
                                //替换字符,因为在发送数据时；为发送信息间隔符
                                payStr = payStr.Replace('@', ';');

                                //跨线程调用(跨线程调用的值不能传出)
                                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                                {
                                    //Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
                                    Debug.Log("执行payStr..." + payStr);
                                    Game_PublicClassVar.Get_game_PositionVar.OnPayResultReturnIOS(payStr);
                                    //Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                                }));

                                //消息协议 SendProps,会话ID/订单号，道具ID,数量...(道具实体)
                                sendStrList = payStr.Split(';');
                                string sendIOSMsg = "GetPay" + "," + sendStrList[3];
                                Debug.Log("sendIOSMsg..." + sendIOSMsg);
                                SendToServer(sendIOSMsg);

                                break;

                            //发送道具的时候调用
                            case "IOSPayDel":
                                Debug.Log("收到IOSPay..." + msgStrList[i]);
                                //执行对应充值
                                payStr = msgStrList[i].Substring(10, msgStrList[i].Length - 10);

                                //跨线程调用(跨线程调用的值不能传出)
                                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                                {
                                    //Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
                                    Debug.Log("执行payDelStr..." + payStr);
                                    Game_PublicClassVar.Get_game_PositionVar.DeleteIosPay(payStr);
                                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().IOSBuyHintText.GetComponent<Text>().text = "有未兑付的订单正在查询..";
                                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_RmbStore.GetComponent<UI_RmbStore>().IOSBuyHint.SetActive(false);
                                    //Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                                }));


                                break;


                            //发送道具的时候调用
                            case "GooglePay":
                                Debug.Log("收到Send收到Send收到Send收到Send");
                                //执行对应充值
                                payStr = msgStrList[i].Substring(10, msgStrList[i].Length - 10);
                                //替换字符,因为在发送数据时；为发送信息间隔符
                                payStr = payStr.Replace('@', ';');
                                string[] payList = payStr.Split(',');
                                Debug.Log("收到消息payStr = " + payStr);

                                //跨线程调用(跨线程调用的值不能传出)
                                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                                {

                                    string payValue = "0";
                                    switch (payList[0]) {
                                        case "paytest3":
                                            payValue = "6";
                                            break;
                                        case "pay_1":
                                            payValue = "6";
                                            break;
                                        case "pay_2":
                                            payValue = "30";
                                            break;
                                        case "pay_3":
                                            payValue = "50";
                                            break;
                                        case "pay_4":
                                            payValue = "98";
                                            break;
                                        case "pay_5":
                                            payValue = "198";
                                            break;
                                        case "pay_6":
                                            payValue = "298";
                                            break;
                                        case "pay_7":
                                            payValue = "488";
                                            break;
                                        case "pay_8":
                                            payValue = "648";
                                            break;
                                    }

                                    Game_PublicClassVar.Get_game_PositionVar.PayStr = "1;支付成功";
                                    Game_PublicClassVar.Get_game_PositionVar.PayDingDanIDNow = payList[0];
                                    Game_PublicClassVar.Get_function_Rose.GamePay(payValue, payList[0]);

                                    //Debug.Log("kkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkkk");
                                    //Game_PublicClassVar.Get_game_PositionVar.OnPayResultReturn(payStr);
                                    //Debug.Log("mmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmmm");
                                }));

                                
                                //sendStr = msgStrList[i].Substring(8, msgStrList[i].Length - 8);
                                //string[] sendStrList = sendStr.Split('@');

                                //消息协议 SendProps,会话ID/订单号，道具ID,数量...(道具实体)
                                sendMsg = "GetPay" + "," + payList[1];
                                SendToServer(sendMsg);
                                

                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }
        catch (Exception ex) {
            Debug.Log("读取支付服务器信息错误：" + ex);
        }

        //10秒内没有得到服务器响应自动连接服务器
        xintiaoDuanKaiSum = xintiaoDuanKaiSum + Time.deltaTime;
        if (xintiaoDuanKaiSum >= 10)
        {
            xintiaoDuanKaiSum = 0;
            ClosePayServerLink();
            Debug.Log("服务器断开连接");
            StartLinkPayServer();
            Debug.Log("正在尝试连接服务器");
        }


        //Debug.Log("tcpSocket = " + tcpSocket.Connected);
        //如果服务器连接失败,每秒执一次,尝试连接服务器
        if (tcpSocket != null)
        {
            if (tcpSocket.Connected == false)
            {
                //更新服务器连接状态
                if (LinkPayServerStatus == true)
                {
                    ClosePayServerLink();
                }
                //尝试再次连接
                linkStartTime = linkStartTime + Time.deltaTime;
                if (linkStartTime >= 1)
                {
                    linkStartTime = 0;
                    StartLinkPayServer();
                }
            }
        }
        else {
            //链接支付
            ClosePayServerLink();
        }


        //每5秒自动向服务器发送一条信息
        if (LinkPayServerStatus) {
            xintiaoTimeSum = xintiaoTimeSum + Time.deltaTime;
            if (xintiaoTimeSum >= 1) {
                xintiaoTimeSum = 0;
                SendToServer("xintiao");
            }
        }

        //1秒钟只能点一次付费按钮
        if (PayingStatus) {
            payBtnTimeSum = payBtnTimeSum + Time.deltaTime;
            if (payBtnTimeSum >= 1)
            {
                PayingStatus = false;
            }
        }
    }


    //开始连接充值服务器
    public void StartLinkPayServer() {
        try
        {
            //初始化端口数据
            //serverIP = "127.0.0.1";

            if (Game_PublicClassVar.Get_wwwSet.IfGooglePay)
            {
                //国外服务器
                serverIP = "47.88.26.105";
                serverPort = 30001;
            }
            else
            {
                //国内服务器
                //serverIP = "gameserver_weijing2.weijinggame.com";
                serverIP = "molongbanhao.weijinggame.com";
                serverPort = 30001;
            }


            IPAddress[] serveIp = Dns.GetHostAddresses(serverIP);
            //Debug.Log("serveIp = " + serveIp[0]);

            //初始化连接服务器
            tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            remoteEndPoint = new IPEndPoint(serveIp[0], serverPort);
            //建立连接
            tcpSocket.Connect(remoteEndPoint);
            //确认连接至服务器上
            LinkPayServerStatus = true;
            tcpSocket.NoDelay = true;

            //接收服务器消息（创建线程接受服务端的消息）
            receiveThread = new Thread(Receivemsg);
            receiveThread.Start();
            receiveThread.IsBackground = true;          //线程后台执行
        }
        catch(Exception ex) {
            Debug.Log("连接服务器出错EX = " + ex);
        }
    }


    /// <summary>接收到服务器的消息</summary>
    private void Receivemsg()
    {
        //存储网络消息的容器
        byte[] data = new byte[1024];
        //这里是开启了一个线程一直重复循环接受服务器发给自己的数据
        int errNum = 0;
        while (true)
        {
            try
            {
                //将接收到的消息进行解码 byte[]->string
                if (tcpSocket != null && remoteEndPoint!=null) {
                    int length = tcpSocket.ReceiveFrom(data, ref remoteEndPoint);
                    if (length > 0)
                    {
                        string message = Encoding.UTF8.GetString(data, 0, length);
                        netMsg.Enqueue(message);
                        //Debug.Log("收到的服务器消息是:" + message);
                    }
                    else
                    {
                        Debug.Log("支付服务器断开连接");
                    }

                    errNum = 0;
                }
            }
            catch (Exception ex) {
                Debug.Log("读取数据报错 ex = " + ex);
                //连续报错100次,跳出循环
                errNum = errNum + 1;
                if (errNum >= 100) {
                    break;
                }
            }
        }
    }

    /// <summary>发送消息到服务器</summary>
    public void SendToServer(string str)
    {
        //Debug.Log("发送数据 str = " + str);
        string sendMsg = str + ";";
        //重新创建一个线程发送消息,发送完毕后关闭线程
        sendThread = new Thread(() => {
            try
            {
                if (tcpSocket != null) {
                    byte[] data = Encoding.UTF8.GetBytes(sendMsg);      //封装发送数据 转化为byte[]字节数组
                    tcpSocket.SendTo(data, remoteEndPoint);             //发送到服务器
                }
            }
            catch (Exception ex) {
                Debug.Log("发送数据报错: ex = " + ex);
            }

            sendThread.Abort();//中止线程

        });
        sendThread.Start();
    }


    //按钮调动支付宝支付,发送服务端消息
    public void Btn_AliPay(string payValue)
    {

        if (PayingStatus)
        {
            Debug.Log("点击付费按钮太快,请稍后再试！");
            return;
        }
        PayingStatus = true;

        //检测连接是否可用
        if (tcpSocket.Connected==false && tcpSocket==null)
        {
            Debug.Log("检测到关闭连接,尝试重新连接");
            ClosePayServerLink();
        }

        //判定服务器是否连接
        if (!LinkPayServerStatus)
        {
            //重新发送连接请求
            StartLinkPayServer();
        }

        if (LinkPayServerStatus)
        {
            string goodsID = payValue;                  //暂定价格
            RequestPayment("AliPay", goodsID, 1);       //发送服务器支付请求
        }
        else {
            Debug.Log("请再次点击按钮！");
        }
    }



    //调用阿里支付
    public void AliPay(string[] parameter)
    {
#if UNITY_ANDROID

        Debug.Log("执行支付宝支付 parameter[1] = " + parameter[1]);
        this.GetComponent<UI_RmbStore>().clickPayBtnStatus = true;
        aliComponent.AliPay(parameter[1]);
#endif
    }


    //支付宝同步调用的回调时间（返回字符串在SDK中定义）
    public void AliPayCallback(string result)
    {
        if (result == "支付成功")
        {
            Debug.Log("支付宝支付成功");
        }
        else
        {
            Debug.Log("支付宝支付失败");
        }
    }

    public void QudaoPayCallback(string result)
    {
        Debug.Log("QudaoPayCallback");
    }


    public void Btn_QudaoPay(string payValue)
    {
        if (PayingStatus)
        {
            Debug.Log("点击付费按钮太快,请稍后再试！");
            return;
        }

        PayingStatus = true;

        //检测连接是否可用
        if (tcpSocket.Connected == false && tcpSocket == null)
        {
            Debug.Log("检测到关闭连接,尝试重新连接");
            ClosePayServerLink();
        }

        //判定服务器是否连接
        if (!LinkPayServerStatus)
        {
            //重新发送连接请求
            StartLinkPayServer();
        }

        if (LinkPayServerStatus)
        {
            Debug.Log("发送渠道支付请求~");
            string goodsID = payValue;                  //暂定价格
            RequestPayment(QudaoPayStr, goodsID, 1);    //发送服务器支付请求
        }
        else
        {
            Debug.Log("请再次点击按钮！");
        }
    }

    //按钮调动支付宝支付,发送服务端消息
    public void Btn_WeiXinPay(string payValue)
    {

        if (PayingStatus)
        {
            Debug.Log("点击付费按钮太快,请稍后再试！");
            return;
        }

        PayingStatus = true;

        //检测连接是否可用
        if (tcpSocket.Connected == false && tcpSocket == null)
        {
            Debug.Log("检测到关闭连接,尝试重新连接");
            ClosePayServerLink();
        }

        //判定服务器是否连接
        if (!LinkPayServerStatus)
        {
            //重新发送连接请求
            StartLinkPayServer();
        }

        if (LinkPayServerStatus)
        {
            Debug.Log("发送微信支付请求~");
            string goodsID = payValue;                  //暂定价格
            RequestPayment("WeChatPay", goodsID, 1);    //发送服务器支付请求
        }
        else
        {
            Debug.Log("请再次点击按钮！");
        }
    }


    //将参数传递给安卓层，拉去渠道支付
    public void QudaoPay(string[] parameter)
    {
#if UNITY_ANDROID
        EventHandle.SendPay(parameter);
#endif
    }


    /// <summary> 将参数传递给安卓层 调起微信支付 </summary>
    public void WechatPay(string[] parameter)
    {
        this.GetComponent<UI_RmbStore>().clickPayBtnStatus = true;
#if UNITY_ANDROID
        try
        {
            Debug.Log("获取到微信支付参数:" + parameter[1].ToString());
            appid = parameter[1];
            mch_id = parameter[2];
            prepayid = parameter[3];
            noncestr = parameter[4];
            timestamp = parameter[5];
            packageValue = parameter[6];
            sign = parameter[7];

            weChatComponent.WeChatPay(appid, mch_id, prepayid, noncestr, timestamp, sign);
        }
        catch
        {
            Debug.Log("微信支付接收数据异常");
        }
#endif
    }


    //微信同步回调
    /// <summary> 微信支付回调 0成功 -1失败 -2取消 </summary>
    public void WeChatPayCallback(int state)
    {
        string result = "";
        switch (state)
        {
            case 0:
                result = "成功";
                break;
            case 1:
                result = "失败";
                break;
            case 2:
                result = "取消";
                break;
            default:
                break;
        }
    }

    //查询订单
    public void QueDingDan() {
        if (LinkPayServerStatus) {
            string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string sendMsg = "ChaXun" + "," + zhanghaoID;
            SendToServer(sendMsg);
        }
    }


    /// <summary> 第一步:向服务器请求调起支付SDK所需要的参数 </summary>
    // 此处为点击按钮发送,玩家点击按钮向服务器发送的支付数据,服务器用来缓存当前订单进行比对
    public void RequestPayment(string msgType, string goodsID, int buyCount)
    {
        //与服务器请求的时候,至少包含3个参数,甚至更多
        //支付的类型、购买的物品ID、购买的物品数量
        //获取账号ID
        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        SendToServer(msgType + "," + goodsID + "," + buyCount + "," + zhanghaoID);
    }


    //关闭服务器
    public void ClosePayServerLink()
    {
        LinkPayServerStatus = false;

        //关闭服务器连接
        if (tcpSocket != null)
        {
            tcpSocket.Close();
        }
        if (sendThread != null)
        {
            sendThread.Abort();
        }
        if (receiveThread != null)
        {
            receiveThread.Abort();
        }
    }

    /// <summary> 当应用程序正常退出的时候 释放网络连接和中止发送线程 </summary>
    private void OnApplicationQuit()
    {
        ClosePayServerLink();
    }

    //此界面在注销的时候调用关闭所有相关线程
    private void OnDestroy()
    {
        Debug.Log("注销界面Close");
        ClosePayServerLink();
    }

    //检测服务端是否连接
    private bool SocketIsOnline(Socket c)
    {
        try
        {
            if (c != null)
            {
                return !((c.Poll(1000, SelectMode.SelectRead) && (c.Available == 0)) || !c.Connected);
            }
            else
            {
                //没有客户端表示客户端没有了服务端连接
                return false;
            }
        }
        catch
        {
            //SeverHint(c + "客户端检测连接错误！");
            return false;
        }
    }

}
