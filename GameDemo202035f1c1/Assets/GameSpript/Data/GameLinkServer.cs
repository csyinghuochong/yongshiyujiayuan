using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System;
using System.Linq;
using System.IO;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Net.NetworkInformation;
using UnityEngine.Networking;

public class GameLinkServer : MonoBehaviour {


    //private TcpListener tcpListener;                      //声明一个监听事件
    private ObscuredString ipAddress;                       //服务器IP  
    private ObscuredInt port;                               //服务器端口
    private Thread mainThread;
    private TcpClient tc;
    private NetworkStream ns;

    public ObscuredBool ServerLinkStatus;                   //服务器连接状态
    public ObscuredString ServerLinkTimeStamp;              //服务器第一次连接后反馈的时间戳
    public ObscuredBool ServerLinkStopSendStatus;

    private ObscuredBool SendIDYanZhengStatus;              //当前ID为8位数不断发送请求新ID
    private ObscuredFloat SendIDYanZhengTime;

    public ObscuredBool SendDataStatus;
    private ObscuredString zhanghaoID;
    private ObscuredString gameVerSionStr;                  //游戏版本号
    public ObscuredString GameGongGaoStr;                   //游戏公告内容

    public GameObject Obj_GameUpdate;                       //游戏更新UI

    //状态数据
    public bool SendPlayDataStatus;                             //发送玩家信息
    public bool WeiYiIDStatus;                                  //获取当前ID是否为唯一状态
    public bool IfEnterGameStatus;                              //是否可以进入游戏

    //public delegate void DelegateCommonString(string str);    // 定义一个委托
    //private DelegateCommonString commonDelStr;

    //服务器第一
    public ObscuredString ServerPaiHangChengHao;

    //红包总额
    public ObscuredBool HongBaoStatus;
    public ObscuredInt HongBaoSumValue;
    public ObscuredFloat HongBaoCostTime;

    //宠物天梯
    public Pro_PetListData PetTianTi_ProPetListData;

    //掠夺状态
    public ObscuredBool LveDuoStatus;
    public ObscuredInt LveDuoGoldNum;
    public ObscuredInt LveDuoKuangSpace;
    public ObscuredString LvDuoKuangZhangHaoID;
    public ObscuredString LvDuoKuangType;
    public ObscuredString LvDuoKuangPetSpaceStr;

    //活动相关
    public ObscuredBool HuoDong_BaoXiang_Status;            //红包活动
    public ObscuredBool HuoDong_ShouLie_Status;             //狩猎活动
    public ObscuredBool HuoDong_Tower_Status;               //魔塔活动
    public float HuoDong_Tower_EndShowTime;                 //魔塔显示剩余时间（用来给那些玩家发送奖励）
    public ObscuredInt HuoDong_ShouLie_SendNum;
    private ObscuredInt HuoDong_ShouLie_SendNumMax = 10;
    public ObscuredFloat HuoDong_ShouLie_Time;              //狩猎活动时间
    public float HuoDong_ShouLie_SendTime;

    //分享相关
    public string FenXiang_Title;
    public string FenXiang_Text;
    public string FenXiang_ImageUrl;
    public string FenXiang_ClickUrl;

    //更新游戏
    public string UpdateGame_Android;
    public string UpdateGame_Ios;

    //是否发送消息
    public bool IfSendGetItemData;          //是否发送获得道具消息
    public bool IfSendErrorData;            //是否发送报错日志

    //邮件提示
    public bool MailHintStatus;

    //是否封号
    public ObscuredBool IfFengHaoStatus;

    //动态密尺
    public ObscuredString EncryptKey_DongTai = "/9}G";
    //public ObscuredString EncryptKey_DongTai_Save = "/8}G";     //后期需要添加此处的备用密匙

    //延迟
    public ObscuredBool YanChiStatus;
    public ObscuredFloat YanChiValue;
    public ObscuredBool SendYanChiStatus;

    //强制实名制
    public ObscuredBool QiangZhiShiMingStatus;

    //提示相关
    public bool HintMsgStatus_Exit;
    public string HintMsgText_Exit;
    public bool HintMsgStatus;
    public string HintMsgText;

    public bool HintMsgStatus_StartGame;
    public string HintMsgText_StartGame;

    public bool setXmlErrorWriteYanZhengStatus;
    public bool serverFileYanZhengStatus;

    private string nowShebeiStr;

    public bool FirstLinkStatus;

    //获取Ip
    public bool GetIpStatus;
    public string GetIpStr;

    // Use this for initialization
    void Start () {

        //Control.CheckForIllegalCrossThreadCalls = false;
        //调用主线程
        mainThread = new Thread(MainThread);
        mainThread.Start();

        WeiYiIDStatus = false;

        gameVerSionStr = Application.version;

        nowShebeiStr = SystemInfo.deviceUniqueIdentifier;


        GetWANIp(delegate (string iip)
        {
            GetIpStatus = true;
            iip = iip.Replace("/n", "");
            GetIpStr = iip;
        });
    }
	
	// Update is called once per frame
	void Update () {

        if (gameVerSionStr == "") {
            gameVerSionStr = Application.version;
        }

        //测试一些发送的信息
        if (SendDataStatus) {

            SendDataStatus = false;
            //SendToServerBuf(10001007, "1");

            //进入EnterGame后发送
            //获取实力值
            int shiliValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue;
            //Debug.Log("shiliValue = " + shiliValue);
            zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
            string[] sendStrList = new string[] { zhanghaoID, shiliValue.ToString(), "", "" };
            //SendToServerBuf(10001030, sendStrList);
            SendToServerBuf(10001031, "");
        }

        if (!SendPlayDataStatus) {
            if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus)
            {
                Debug.Log("发送玩家信息成功！！！");
                SendPlayDataStatus = true;

                //发送玩家基本数据
                SendToServerBuf(10001003, "");

                //获取唯一ID
                zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (zhanghaoID.Length <= 8)
                {
                    Debug.Log("发送获取唯一ID发送获取唯一ID发送获取唯一ID发送获取唯一ID");
                    //SendToServerBuf(10001008, "");
                    WeiYiIDStatus = false;
                    SendIDYanZhengStatus = true;
                    SendToServerBuf(10001008, "");
                }
                else {
                    //获取当前已经确定了唯一的ID,开始上传玩家数据
                    WeiYiIDStatus = true;
                }
            }
        }

        if (SendIDYanZhengStatus) {

            SendIDYanZhengTime = SendIDYanZhengTime + Time.deltaTime;

            //如果没有效验到 每10效验一次
            if (SendIDYanZhengTime >= 10) {
                SendIDYanZhengTime = 0;

                //获取唯一ID
                zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (zhanghaoID.Length <= 8)
                {
                    SendToServerBuf(10001008, "");
                }
                else {
                    SendIDYanZhengStatus = false;
                }
            }
        }

        //获取唯一ID后发送   
        if (WeiYiIDStatus) {

            WeiYiIDStatus = false;

            //获取唯一ID
            zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

            //每次进入游戏上传一次玩家数据 (验证完服务器数据后开启下载)
            string[] saveList = new string[] { "", "2", "预留设备号位置","1" };
            SendToServerBuf(10001006, saveList);

            //验证设备唯一表示符号
            //SendToServerBuf(10001010, "");

            string pingtaiStr = "";
#if UNITY_ANDROID
            pingtaiStr = "1";
#endif
#if UNITY_IPHONE
            pingtaiStr = "2";
#endif


            //平台
            string pingtaiTypeStr = "0";
            if (Game_PublicClassVar.Get_wwwSet.GetComponent<GetSignature>() != null)
            {
                pingtaiTypeStr = Game_PublicClassVar.Get_wwwSet.GetComponent<GetSignature>().ChannelId;
            }
            //发送版本号
            Pro_ComStr_3 proComStr_3 = new Pro_ComStr_3 {str_1= zhanghaoID, str_2= Application.version, str_3 = pingtaiStr, str_4 = pingtaiTypeStr};
            SendToServerBuf(10001014, proComStr_3);

            //发送线程变量
            SendToServerBuf(20000001, zhanghaoID);

            //获取线程变量
            SendToServerBuf(20000003, "");

            //发送设备ID
            SendToServerBuf(10001020, "");

            //服务器信息请求
            //Debug.Log("发送服务器请求");
            SendToServerBuf(10000105, zhanghaoID);

            //发送验证
            SendToServerBuf(10000108, "");

            //请求世界等级
            SendToServerBuf(10001302, "");

            //请求今天第一称号
            SendToServerBuf(10001036, "");

            //请求文件验证记录
            string shebeiID = SystemInfo.deviceUniqueIdentifier;
            SendToServerBuf(10001600, shebeiID);

            //发送当前语言
            SendToServerBuf(10001021, "");

            //发送记录值
            Game_PublicClassVar.Get_function_Rose.SendGameHuoBiJiLu();

            //发送MD5
            Pro_ComStr_4 com_4 = new Pro_ComStr_4();
            com_4.str_1 = Game_PublicClassVar.Get_wwwSet.GameMD5Str;
            com_4.str_2 = zhanghaoID;
            SendToServerBuf(10001023, com_4);

            //发送设备信息
            SendToServerBuf(10001022, Game_PublicClassVar.Get_wwwSet.PlayerSheBeiData);

            //获取当前是否发送记录日志
            SendToServerBuf(10000113, zhanghaoID);

            //发送伤害记录
            SendToServerBuf(10002101, "");

            //发送obj退出次数记录
            SendToServerBuf(10002102, "");

            //发送自身IP
            if (GetIpStatus) {
                com_4 = new Pro_ComStr_4();
                com_4.str_1 = zhanghaoID;
                com_4.str_2 = GetIpStr;
                SendToServerBuf(10002401, com_4);
            }

            CheckAndroid();


        }

        if (YanChiStatus) {
            YanChiValue = YanChiValue + Time.deltaTime;
        }

        if (SendYanChiStatus) {
            SendYanChiStatus = false;
            SendToServerBuf(10000003, "");
        }

        if (HongBaoCostTime > 0) {
            HongBaoCostTime = HongBaoCostTime - Time.deltaTime;
        }

        //狩猎活动
        if (HuoDong_ShouLie_Status)
        {
            if (HuoDong_ShouLie_Time > 0)
            {
                HuoDong_ShouLie_Time = HuoDong_ShouLie_Time - Time.deltaTime;
            }
            else {
                HuoDong_ShouLie_Status = false;
            }

            //最后10秒改为1个1传
            float HuoDong_ShouLie_SendTimeMax = 10;
            if (HuoDong_ShouLie_Time <= 10) {
                HuoDong_ShouLie_SendNumMax = 1;
                HuoDong_ShouLie_SendTimeMax = 1;
            }
            //测试 
            //HuoDong_ShouLie_SendNumMax = 1;
            //根据击杀人数发送
            HuoDong_ShouLie_SendTime = HuoDong_ShouLie_SendTime + Time.deltaTime;

            //每10秒发送一次数据
            if (HuoDong_ShouLie_SendTime >= HuoDong_ShouLie_SendTimeMax) {
                HuoDong_ShouLie_SendTime = 0;

                //每次超过10个才发送
                if (HuoDong_ShouLie_SendNum >= HuoDong_ShouLie_SendNumMax)
                {
                    //发送击杀数据
                    Pro_ComStr_4 shouLieCom = new Pro_ComStr_4();
                    shouLieCom.str_1 = HuoDong_ShouLie_SendNum.ToString();
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001901, shouLieCom);

                    //增加本地记录
                    Game_PublicClassVar.Get_function_Rose.RoseAddShouLieNum(HuoDong_ShouLie_SendNum);

                    HuoDong_ShouLie_SendNum = 0;
                }
            }
        }

        //查询封号
        if (IfFengHaoStatus)
        {
            Game_PublicClassVar.Get_wwwSet.IfFengHaoStatus = true;
        }

        if (HuoDong_Tower_EndShowTime > 0) {
            HuoDong_Tower_EndShowTime = HuoDong_Tower_EndShowTime - Time.deltaTime;
        }


        if (HintMsgStatus_Exit) {

            if (Application.loadedLevelName != "StartGame") {

                HintMsgStatus_Exit = false;

                //弹出提示
                GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(HintMsgText_Exit, Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "系统提示", "确定", "取消", Game_PublicClassVar.Get_wwwSet.ExitGame);
                //DontDestroyOnLoad(uiCommonHint);
                if (GameObject.Find("Canvas/GameGongGaoSet") != null)
                {
                    uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                    uiCommonHint.transform.localPosition = Vector3.zero;
                    uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }
        }


        if (HintMsgStatus)
        {
            if (Application.loadedLevelName != "StartGame")
            {

                HintMsgStatus = false;

                //弹出提示
                GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
                uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(HintMsgText, null, null, "系统提示", "确定", "取消", null);
                //DontDestroyOnLoad(uiCommonHint);
                if (GameObject.Find("Canvas/GameGongGaoSet") != null)
                {
                    uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                    uiCommonHint.transform.localPosition = Vector3.zero;
                    uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                }
            }
        }


        if (HintMsgStatus_StartGame)
        {
            HintMsgStatus_StartGame = false;

            //弹出提示
            GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
            uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(HintMsgText_StartGame, null, null, "系统提示", "确定", "取消", null);
            //DontDestroyOnLoad(uiCommonHint);
            if (GameObject.Find("Canvas/GameGongGaoSet") != null)
            {
                uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                uiCommonHint.transform.localPosition = Vector3.zero;
                uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            }
        }

    }

    void OnDestroy() {
        CloseServer();
    }


    //主线程无线循环
    private void MainThread()
    {

        //根据调试模式对应不同的端口
        /*
        if (Game_PublicClassVar.Get_wwwSet.TiaoShiStatus)
        {
            ipAddress = "192.168.1.14";
            //ipAddress = "gameserver_weijing1.weijinggame.com";
            port = 17960;
        }
        else {
            ipAddress = "gameserver_weijing2.weijinggame.com";
            port = 15006;
        }
        */

        if (Game_PublicClassVar.Get_wwwSet.IfGooglePay)
        {
            //国外服务器
            ipAddress = "47.88.26.105";
            port = 15001;
        }
        else
        {
            //国内服务器
            ipAddress = "gameserver_weijing2.weijinggame.com";
            port = 15006;
            //port = 15006;
        }

        IPAddress[] serveIp = Dns.GetHostAddresses(ipAddress);
        //Debug.Log("serveIp = " + serveIp[0]);

        //构建一个端口(ip,端口)
        IPEndPoint ipend = new IPEndPoint(serveIp[0], port);
        //链接服务器
        tc = new TcpClient();
        //tc.SendTimeout = 1000;
        //tc.ReceiveTimeout = 1000;
        tc.NoDelay = true;

        while (true) {
            //Debug.Log("开始触发链接！");
            try
            {
                tc.Connect(ipend);
                //Debug.Log("链接成功！");
                //创建数据流
                ns = tc.GetStream();
                //Debug.Log("链接服务器和通讯流数据成功！");
                ServerLinkStatus = true;
                ServerLinkStopSendStatus = false;
            }

            catch (Exception ex)
            {
                Debug.Log("服务器连接失败！EX = " + ex);
            }

            //发送申请链接协议
            SendToServerBuf(10000001, "");

            //声明一个空数组
            byte[] btyArr = new byte[1024];
            int btLen = 0;
            List<byte> readByteList = new List<byte>();
            while (true)
            {
                //接受服务端传送过来的数据&& ns.CanRead 
                if (ns != null)
                {
                    int shuliangRow = 0;
                    int readDataSize = 0;
                    bool readDataStatus = false;
                    //Debug.Log("循环数据处理时间11111：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    try
                    {
                        int while_2 = 0;
                        //循环读取数据
                        do
                        {
                            while_2 = while_2 + 1;
                            if (while_2 >= 100)
                            {
                                Debug.Log("while_2 = " + while_2 + ";Thread.CurrentThread.Name = " + Thread.CurrentThread.Name + "readByteList.Count=" + readByteList.Count + ";readDataSize=" + readDataSize + " + Thread.CurrentThread.Name = " + Thread.CurrentThread.Name);
                            }

                            if (while_2 >= 1000) {
                                //读取完数据后清理数据
                                readByteList.Clear();
                                readDataSize = 0;
                                shuliangRow = 0;
                                readDataStatus = false;
                                continue;       //跳过本次循环
                            }

                            //同步读取
                            btyArr = new byte[1024];
                            //Debug.Log("循环数据处理时间22222：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                            try
                            {
                                btLen = ns.Read(btyArr, 0, btyArr.Length);
                                //Debug.Log("循环数据处理时间33333：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff") + "btLen = " + btLen);
                            }
                            catch (Exception ex)
                            {
                            }
                            if (btLen >= 1)
                            {
                                //把读取到的数据转换成赋值到新的数组中
                                byte[] addBty = btyArr.Skip(0).Take(btLen).ToArray();
                                //将得到的数组进行累加
                                readByteList.AddRange(addBty.ToArray());
                                //readByteList.AddRange(btyArr);
                                shuliangRow = shuliangRow + 1;
                                //第一次收到包先获取要读取的字节大小
                                if (shuliangRow == 1)
                                {
                                    //写了一个方法,这个方法能获取当前读取包的大小
                                    readDataSize = GetBaoTiSize(btyArr, 0);
                                    if (readDataSize == -1)
                                    {
                                        Console.WriteLine("第一次读取数据报错！btyArr.leng = " + btyArr.Length);
                                        break;
                                    }
                                    //Debug.Log("获取当前包体大小readDataSize = " + readDataSize);
                                }
                                //Debug.Log("循环数据处理时间44444：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                                if (readByteList.Count >= readDataSize)
                                {
                                    readDataStatus = true;

                                    while (true)
                                    {
                                        //如果接受数据和包体总长度一样直接返回
                                        if (readByteList.Count == readDataSize)
                                        {
                                            break;
                                        }

                                        /*
                                        if (readDataSize >= readByteList.Count) {
                                            break;
                                        }
                                        */

                                        //如果接受数据和包体不一致,则检测是否为粘包
                                        //if (readDataSize >= readByteList.Count) {
                                        //Debug.Log("readDataSize = " + readDataSize);
                                        if (readByteList[readDataSize] != 0)
                                        {
                                            if (readByteList[readDataSize] == 10)
                                            {
                                                //获取数据包长度
                                                int addLength = GetBaoTiSize(readByteList.ToArray(), readDataSize);
                                                if (addLength == -1)
                                                {
                                                    int chaValue = readByteList.Count - readDataSize;
                                                    if (chaValue < 6)   //此处等于6是因为 固定开头2位数 包体大小4位数，相加为6位数  GetBaoTiSize只需要6位数获取包体大小
                                                    {
                                                        //表示粘包最后末位显示完的下个包的数据
                                                        /*
                                                        //记录本次剩余的和数据
                                                        nianbaoShengyu = new byte[chaValue];
                                                        nianbaoShengyu = readByteList.Skip(readDataSize).Take(chaValue).ToArray();
                                                        nianBaoShengYuStatus = true;
                                                        */
                                                        //接着上层读取新的数据
                                                        Console.WriteLine("循环读取数据末位<10 = " + btyArr.Length);
                                                        readDataStatus = false;
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        Console.WriteLine("循环读取数据报错 = " + btyArr.Length);
                                                        //没有数据跳出循环
                                                        //readDataStatus = true;
                                                        break;
                                                    }
                                                }



                                                //获取剩余包体的长度
                                                int chang = readByteList.Count - readDataSize;
                                                //叠加当前总包体的长度
                                                readDataSize = readDataSize + addLength;
                                                if (chang < addLength)
                                                {

                                                    //接着上层读取新的数据
                                                    readDataStatus = false;
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                //没有数据跳出循环
                                                readDataStatus = true;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            //没有数据跳出循环
                                            readDataStatus = true;
                                            break;
                                        }
                                        //}
                                    }
                                }
                            }
                            else {
                                readDataStatus = true;
                            }
                        } while (!readDataStatus);       //备用,判断是否有数据需要读取 ns.DataAvailable
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("btLen = " + btLen + ";ex = " + ex);
                        //读取完数据后清理数据
                        readByteList.Clear();
                        readDataSize = 0;
                        shuliangRow = 0;
                        readDataStatus = false;
                        continue;       //跳过本次循环
                    }
                    //Debug.Log("循环数据处理时间55555：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    //将数据赋值给新的字符
                    byte[] readBtyeArr = readByteList.ToArray();
                    //读取完数据后清理数据
                    readByteList.Clear();

                    if (btLen < 1)
                    {
                        Debug.Log("服务端断开了！");
                        ServerLinkStatus = false;
                        break;
                    }
                    else
                    {
                        ServerToClientBuf(readBtyeArr);
                        /*
                        //.Replace("\0", "");一定要加 该字符串不能再后缀加任何字符串,坑了我1晚上时间（Replace为旧的字符串替换新的字符串,所以为空即可,此操作可以生成一个全新的字符串,参数为“\0”的说明：由于\0在字符串中代表字符串截止，因此，c#在处理字符串时，当遇到\0,就会截止对字符串的操作。）
                        string dataStr = Encoding.Default.GetString(btyArr).Replace("\0", "");
                        Debug.Log("dataStr = " + dataStr);
                        DoThing(dataStr);
                        */
                        //清空数据
                        btyArr = new byte[1024];
                    }
                    //Debug.Log("循环数据处理时间66666：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    if (!TcpIsOnline(tc))
                    {
                        Debug.Log("服务端断开了！");
                        ServerLinkStatus = false;
                        break;
                    }

                }
                else {
                    break;
                }

                btLen = 0;

                //将进程暂时挂起1000毫秒,也就是1秒执行一次无线循环
                //Thread.Sleep(1000);
                //Debug.Log("循环结束");

            }
            //Debug.Log("掉线！！！！");
            //跳出循环后开启断线重连（每秒触发一次断线重连）
            Thread.Sleep(1000);
            tc = new TcpClient();
        }
    }


    //检测服务端是否连接
    private static bool TcpIsOnline(TcpClient c)
    {
        return !((c.Client.Poll(1000, SelectMode.SelectRead) && (c.Client.Available == 0)) || !c.Client.Connected);
    }


    /*
    //根据接受到的服务端的请求处理不同的数据
    private void DoThing(string dataStr)
    {

        //如果接受数据链协议类型都不满足协议头的长度则表示本条接受数据无效
        if (dataStr.Length < 6)
        {
            Debug.Log("dataStr.Length长度不足");
            return;
        }

        string[] dataStrList = dataStr.Split(';');
        for (int i = 0; i < dataStrList.Length; i++)
        {
            string nowDataStr = dataStrList[i].Replace("\0", "");
            //Debug.Log("dataStrList_" + i + " = " + dataStrList[i]);
            //获取协议类型
            string DataTypeStr = "";
            if (nowDataStr.Length >= 6)
            {
                DataTypeStr = nowDataStr.Substring(0, 6);
            }
            //Debug.Log("DataTypeStr = " + DataTypeStr);
            //根据不同协议类型执行不同操作
            switch (DataTypeStr)
            {
                //获取服务器第一次返回的信息
                case "100000":
                    ServerLinkStatus = true;

                    //根据不同平台获取不同的版本号
#if UNITY_ANDROID
                    sendToServerData("100003;");
#endif
#if UNITY_IPHONE
                    sendToServerData("100004");
#endif

                    //获取时间戳
                    sendToServerData("100001;");
                    //获取QQ群
                    sendToServerData("100005;");


                    break;

                //获取时间戳
                case "100001":
                    string timeStamp = dataStr.Substring(6, dataStrList[i].Length - 6);
                    ServerLinkTimeStamp = timeStamp;
                    Debug.Log("ServerLinkTimeStamp = " + ServerLinkTimeStamp);


                    //服务器发送关闭请求(因为服务器只是用时间戳,所以获取时间戳后直接关闭服务器连接,减轻服务器压力) 此功能暂时屏蔽,现在由服务器完成
                    //sendToServerData("109999");
                    break;

                //获取心跳包
                case "100002":
                    string getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                    //Debug.Log("接受到服务器的测试数据 = " + getServerStr);
                    sendToServerData("100002123;");
                    break;

                //获取当前最新安卓版本号
                case "100003":
                    //Debug.Log("进来啦进来啦进来啦进来啦进来啦进来啦进来啦");
                    //Debug.Log("dataStr = " + dataStr);
                    getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                    Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = getServerStr;
                    break;

                //获取当前最新IOS版本号
                case "100004":
                    getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                    Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = getServerStr;
                    break;

                //获取最新的QQ群号码
                case "100005":
                    Debug.Log("读取到QQ群号码");
                    getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                    Game_PublicClassVar.Get_wwwSet.QQqunID = getServerStr;
                    break;

                //游戏封号/解封
                case "100006":
                    Game_PublicClassVar.Get_wwwSet.IfFengHaoStatus = true;
                    //设置账号封的状态
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", "1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    break;

                //游戏解封
                case "100007":
                    //设置账号封的状态
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", "", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                    break;

                //接受发送的广播
                case "110001":
                    //if (this.gameObject.scene.name != "StarGame") {
                    getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                        Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr = Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr + "10001," + getServerStr + ";";
                        Game_PublicClassVar.Get_GameServerMessage.ServerMessageStatus = true;
                    //}
                    break;

                //接受发送的道具
                case "110002":
                    //if (this.gameObject.scene.name != "StarGame") {
                    getServerStr = dataStr.Substring(6, dataStrList[i].Length - 6);
                    Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr = Game_PublicClassVar.Get_GameServerMessage.ServerMessageStr + "10002," + getServerStr + ";";
                    Game_PublicClassVar.Get_GameServerMessage.ServerMessageStatus = true;
                    //}
                    break;

                //获取玩家名称和信息
                case "110003":        
                    string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string shopDes = zhanghaoID + ";" + roseName + ";" + roseLv + ";" + GoldNum + ";" + RMB + ";" + RMBPayValue + ";" + Application.unityVersion;
                    sendToServerData("110003" + shopDes + ";");
                    Debug.Log("shopDes = " + shopDes);
                    break;

                //关闭服务器的最后一条消息
                case "109999":
                    Debug.Log("关闭服务器连接");
                    break;
            }
        }

        //执行关闭
        //Debug.Log("执行关闭Server");
    }
    */

    //发送信息
    public void sendToServerData(string dataStr) {
        
        //判定是否连接服务器
        //Debug.Log("dataStr = " + dataStr + ";ns = " + ns);
        //发送信息
        byte[] bt = Encoding.Default.GetBytes(dataStr);
        ns.Write(bt, 0, bt.Length);

    }

    //根据接受到的服务端的请求处理不同的数据
    private void ServerToDoThing(int xuhaoID, byte[] serverbyte)
    {
        MemoryStream ms = new MemoryStream();

        //读取客户端传来的消息,将其变成流,方便后期变成字符串
        if (serverbyte.Length != 0)
        {
            ms.Write(serverbyte, 0, serverbyte.Length);
            ms.Position = 0;
        }

        switch (xuhaoID)
        {
            //客户端发送的第一个连接请求
            case 10000001:

                //根据不同平台获取不同的版本号
#if UNITY_ANDROID
                SendToServerBuf(10000102,"");
#endif
#if UNITY_IPHONE
                SendToServerBuf(10000103,"");
#endif
#if UNITY_EDITOR
                SendToServerBuf(10000102, "");
#endif
                //获取时间戳
                SendToServerBuf(10000101,"");

                //获取QQ群
                SendToServerBuf(10000104,"");
                
                //ServerLinkStatus = true;

                //重置服务器连接信息
                SendPlayDataStatus = false;

                //请求公告信息
                SendToServerBuf(10000106, "");

                //是否获取错误日志
                SendToServerBuf(10000114, nowShebeiStr);

                //请求分享数据
                SendToServerBuf(10000117, "");

                //请求更新数据
                SendToServerBuf(10000118, "");

                //请求获取动态密匙
                SendToServerBuf(10000152, "");

                //请求更新服务器列表
                //SendToServerBuf(10000120, "");
                //SendToServerBuf(10000121, "");

                Debug.Log("请求公告信息");
                string BanBenStr = "25";
                
                //发送版本ID
#if UNITY_ANDROID
                switch (Application.platform)
                {

                    case RuntimePlatform.WindowsEditor:
                        Debug.Log("发送验证安卓版本");
                        
                        Pro_ComStr_2 proComStr_2 = new Pro_ComStr_2 { str_1 = "1", str_2 = BanBenStr,str_3 = nowShebeiStr };            //1表示安卓 2表示IOS
                        SendToServerBuf(10000109, proComStr_2);
                        //print("Windows");
                        break;

                    case RuntimePlatform.Android:

                        //Debug.Log("发送验证安卓版本" + UnityEditor.PlayerSettings.Android.bundleVersionCode.ToString());
                        //proComStr_2 = new Pro_ComStr_2 { str_1 = "1", str_2 = UnityEditor.PlayerSettings.Android.bundleVersionCode.ToString() };            //1表示安卓 2表示IOS
                        proComStr_2 = new Pro_ComStr_2 { str_1 = "1", str_2 = BanBenStr, str_3 = nowShebeiStr };            //1表示安卓 2表示IOS
                        SendToServerBuf(10000109, proComStr_2);

                        break;

                }
#endif
                // IOS
#if UNITY_IPHONE
                Pro_ComStr_2 proComStr_2 = new Pro_ComStr_2 { str_1 = "2", str_2 = BanBenStr, str_3 = nowShebeiStr };                        //1表示安卓 2表示IOS
                SendToServerBuf(10000109, proComStr_2);
#endif

                break;

            //心跳包
            case 10000002:
                //返回心跳包
                SendToServerBuf(xuhaoID, "");
                //delegate void DelegateCommon();   // 定义一个委托
                //delegate aa = Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("心跳包心跳包");
                //Game_PublicClassVar.Get_GameServerMessage.BtnWeiTuoTest(Game_PublicClassVar.Get_GameServerMessage.TestWeiTuoHint);
                
                break;

            //发送测试延迟消息
            case 10000003:
                //Debug.Log("100000003时间22222：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                Debug.Log("延迟 =" + (int)(YanChiValue * 1000));
                //Debug.Log("100000003时间33333：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                YanChiStatus = false;
                YanChiValue = 0;
                break;


            //强制关闭游戏
            case 10000080:

                //pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //弹出提示
                    /*
                    GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("数据异常!断开与服务器连接...", Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame,"系统提示","确定","取消", Game_PublicClassVar.Get_wwwSet.ExitGame);

                    if (GameObject.Find("Canvas/GameGongGaoSet") != null) {
                        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                        uiCommonHint.transform.localPosition = Vector3.zero;
                        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    }
                    */
                    
                    HintMsgStatus_Exit = true;
                    HintMsgText_Exit = "数据异常!断开与服务器连接...";
                    
                    //强制退出游戏
                    Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                    Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;

                    //Application.Quit();
                }));

                break;


            //强制关闭游戏
            case 10000081:

                Pro_ComStr_1 pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    /*
                    Debug.Log("提示1111");
                    //弹出提示
                    GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(pro_ComStr_1.str_1, Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "系统提示", "确定", "取消", Game_PublicClassVar.Get_wwwSet.ExitGame);
                    //DontDestroyOnLoad(uiCommonHint.transform.parent.gameObject);        
                    if (GameObject.Find("Canvas/GameGongGaoSet") != null)
                    {
                        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                        uiCommonHint.transform.localPosition = Vector3.zero;
                        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    }
                    */
                    Debug.Log("pro_ComStr_1.str_1 = " + pro_ComStr_1.str_1);
                    HintMsgStatus_Exit = true;
                    HintMsgText_Exit = pro_ComStr_1.str_1;


                    //强制退出游戏
                    Game_PublicClassVar.Get_wwwSet.forceExitGameStatus = true;
                    Game_PublicClassVar.Get_wwwSet.forceExitGameTimeSum = 0;
                    //Application.Quit();
                }));

                break;


            //弹出提示
            case 10000082:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    /*
                    //弹出提示
                    GameObject uiCommonHint = (GameObject)Instantiate(Game_PublicClassVar.Get_wwwSet.Obj_CommonHintHint);
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhintText_19");
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint(pro_ComStr_1.str_1, Game_PublicClassVar.Get_wwwSet.ExitGame, Game_PublicClassVar.Get_wwwSet.ExitGame, "系统提示", "确定", "取消", Game_PublicClassVar.Get_wwwSet.ExitGame);

                    if (GameObject.Find("Canvas/GameGongGaoSet") != null)
                    {
                        uiCommonHint.transform.SetParent(GameObject.Find("Canvas/GameGongGaoSet").transform);
                        uiCommonHint.transform.localPosition = Vector3.zero;
                        uiCommonHint.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                    }
                    */

                    HintMsgStatus = true;
                    HintMsgText = pro_ComStr_1.str_1;

                }));

                break;


            //弹出提示（Start界面弹出提示）
            case 10000083:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    HintMsgStatus_StartGame = true;
                    HintMsgText_StartGame = pro_ComStr_1.str_1;

                }));

                break;

            //是否认证实名
            case 10000091:

                //pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    QiangZhiShiMingStatus = true;
                }));

                break;

            //发送到客户端关闭请求
            case 10000099:
                Debug.Log("关闭服务器连接");
                break;


            //********************初始化请求************************
            //请求时间戳（初始进入游戏的）
            case 10000101:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                ServerLinkTimeStamp = pro_ComStr_1.str_1;
                Debug.Log("获得时间戳:" + ServerLinkTimeStamp);
                Game_PublicClassVar.Get_wwwSet.EnterGameTimeStamp = ServerLinkTimeStamp;

                //设置时间
                if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus == false) {
                    Game_PublicClassVar.Get_wwwSet.DataTime = Game_PublicClassVar.Get_wwwSet.GetTime(ServerLinkTimeStamp);
                    Game_PublicClassVar.Get_wwwSet.enterGameTimeStamp = ServerLinkTimeStamp;
                    //Debug.Log((int)DataTime.Year + "年" + (int)DataTime.Month + "月" + (int)DataTime.Day + "日" + (int)DataTime.Hour + "时" + (int)DataTime.Minute + "分" + (int)DataTime.Second + "秒");//输出时间
                    Game_PublicClassVar.Get_wwwSet.dayUpdataTime = 86400 - ((int)Game_PublicClassVar.Get_wwwSet.DataTime.Hour * 3600 + (int)Game_PublicClassVar.Get_wwwSet.DataTime.Minute * 60 + (int)Game_PublicClassVar.Get_wwwSet.DataTime.Second);
                    if ((int)Game_PublicClassVar.Get_wwwSet.DataTime.Year > 2015)
                    {
                        Game_PublicClassVar.Get_wwwSet.dayUpdataOne = true;
                        Game_PublicClassVar.Get_wwwSet.WorldTimeStatus = true;
                    }
                }

                break;

            //获取当前最新安卓版本号
            case 10000102:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = pro_ComStr_1.str_1;
                break;

            //获取当前最新IOS号
            case 10000103:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                Game_PublicClassVar.Get_wwwSet.GameServerVersionStr = pro_ComStr_1.str_1;
                break;

            //请求QQ群号
            case 10000104:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                Game_PublicClassVar.Get_wwwSet.QQqunID = pro_ComStr_1.str_1;
                break;

            //请求QQ群其他讯息
            case 100001041:
                Pro_ComStr_4 pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                Game_PublicClassVar.Get_wwwSet.QQLnkStr = pro_ComStr_4.str_1;   //QQ群超链接
                Game_PublicClassVar.Get_wwwSet.QQErWeiMaStr = pro_ComStr_4.str_2;   //QQ群超链接
                //Game_PublicClassVar.Get_wwwSet.QQLnkStr = pro_ComStr_4.str_2;   //QQ群二维码超链接
                break;

            //服务器信息
            case 10000105:

                //Debug.Log("接受服务器反馈消息！！！100001051000010510000105");
                Pro_ComStr_3 pro_ComStr_3 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_3>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_wwwSet.ServerID = pro_ComStr_3.str_1;
                    Game_PublicClassVar.Get_wwwSet.ServerName = pro_ComStr_3.str_2;
                    Game_PublicClassVar.Get_wwwSet.ServerStart = pro_ComStr_3.str_3;

                    //Debug.Log("当前服务器名称:" + Game_PublicClassVar.Get_wwwSet.ServerName);
                    Game_PublicClassVar.Get_function_Rose.SaveGameConfig_Rose(Game_PublicClassVar.Get_wwwSet.RoseID, "ServerName", Game_PublicClassVar.Get_wwwSet.ServerName);
                }));
                break;

            //公告
            case 10000106:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (EventHandle.IsHuiWeiChannel() == false)
                    {
                        if (Application.loadedLevelName == "StartGame")
                        {
                            GameGongGaoStr = pro_ComStr_1.str_1;
                            Game_PublicClassVar.Get_wwwSet.Show_GameGongGao(pro_ComStr_1.str_1);
                        }
                    }
                }));
                break;

            //是否可以进入游戏
            case 10000107:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                IfEnterGameStatus = true;
                break;

            //请求验证文件
            case 10000108:
                //发送验证
                SendToServerBuf(10000108, "");
                break;

            //强制玩家更新游戏包
            case 10000109:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_wwwSet.Show_GameUpdate(pro_ComStr_4.str_1, pro_ComStr_4.str_2);

                }));
                break;

            //是否发送获取道具信息
            case 10000113:
                //pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    IfSendGetItemData = true;
                }));
                break;

            //是否发送获取报错信息
            case 10000114:
                //pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    IfSendErrorData = true;
                }));
                break;





            //请求分享数据
            case 10000117:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    FenXiang_Title = pro_ComStr_4.str_1;
                    FenXiang_Text = pro_ComStr_4.str_2;
                    FenXiang_ImageUrl = pro_ComStr_4.str_3;
                    FenXiang_ClickUrl = pro_ComStr_4.str_4;

                }));

                break;

            //请求跟新游戏数据
            case 10000118:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    UpdateGame_Android = pro_ComStr_4.str_1;
                    UpdateGame_Ios = pro_ComStr_4.str_2;
                }));

                break;

            //请求跟新游戏数据
            case 10000119:

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    this.gameObject.GetComponent<GameServerObj>().OpenLinkStatus = true;
                }));
                break;


            //请求服务器列表数据
            case 10000120:

                Pro_ServerListDataSet pro_ServerListDataSet = ProtoBuf.Serializer.Deserialize<Pro_ServerListDataSet>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    this.GetComponent<GameServerObj>().ProServerListDataSet = pro_ServerListDataSet;
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_ServerListShow != null) {
                        Game_PublicClassVar.Get_gameServerObj.Obj_ServerListShow.GetComponent<UI_ServerListShowSet>().Init();

                        /*
                        for (int i = 0; i < Game_PublicClassVar.Get_gameServerObj.Obj_BtnServerList.transform.childCount; i++)
                        {
                            GameObject go = Game_PublicClassVar.Get_gameServerObj.Obj_BtnServerList.transform.GetChild(i).gameObject;
                            go.SetActive(true);
                            go.AddComponent<UI_MailHint>();
                        }
                        */
                    }
                }));

                break;


            //请求当前最新服务器数据
            case 10000121:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    this.GetComponent<GameServerObj>().NowServerName = pro_ComStr_4.str_1;
                    if (Game_PublicClassVar.gameServerObj.Obj_ServerEnterName != null) {
                        Game_PublicClassVar.gameServerObj.Obj_ServerEnterName.GetComponent<Text>().text = pro_ComStr_4.str_1;
                    }

                }));

                break;

            //是否发送获取报错信息
            case 10000122:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_1.str_1 == "1") {
                        Game_PublicClassVar.Get_wwwSet.RootIfExitGame = true;
                    }
                }));
                break;

            //失去焦点直接退出
            case 10000123:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_1.str_1 == "1")
                    {
                        Game_PublicClassVar.Get_wwwSet.RootShiQuExitGame = true;
                    }
                }));
                break;

            //验证身份证
            case 10000131:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng != null) {

                        if (pro_ComStr_4.str_1 == "1" || pro_ComStr_4.str_1 == "2")
                        {
                            //验证通过
                            Debug.Log("服务器身份验证通过!");
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintText.GetComponent<Text>().text = "服务器身份验证通过!";
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintSet.SetActive(true);
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintSetText.GetComponent<Text>().text = "服务器身份验证通过!";
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_ShenFenRenZhengSucessHintSet.SetActive(true);
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintText.SetActive(false);
                            Game_PublicClassVar.Get_wwwSet.IfYouKeStatus = false;
                            //Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus = false;

                        }
                        /*
                        if (pro_ComStr_4.str_1 == "2")
                        {
                            //ZI
                            Debug.Log("重复的身份信息!");
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintText.GetComponent<Text>().text = "已经验证身份信息,请不要重复验证!";
                        }
                        */
                        if (pro_ComStr_4.str_1 == "3")
                        {
                            //验证通过
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintText.GetComponent<Text>().text = "服务器验证失败:你的身份信息已经被注册!";
                        }

                        if (pro_ComStr_4.str_1 == "4")
                        {
                            //验证通过
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().Obj_HintText.GetComponent<Text>().text = "服务器验证失败:你的身份信息已经被注册!";
                        }
                    }

                }));
                break;



            //验证身份证
            case 10000132:
                Pro_PlayerYanZheng proPlayerYanZheng = ProtoBuf.Serializer.Deserialize<Pro_PlayerYanZheng>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    //初始化认证自身是否绑定身份证
                    string name = proPlayerYanZheng.Name;
                    string shenfenID = proPlayerYanZheng.ShenFenID;

                    if (name != "" && name != null && shenfenID != "" && shenfenID != null)
                    {
                        Debug.Log("FangChenMi_Name服务器身份验证通过!");
                        PlayerPrefs.SetString("FangChenMi_Name", proPlayerYanZheng.Name);
                        PlayerPrefs.SetString("FangChenMi_ID", proPlayerYanZheng.ShenFenID);
                        PlayerPrefs.Save();

                        if (Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng != null)
                        {
                            //if(PlayerPrefs.GetInt("FangChenMi_Year")
                            //Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().YanZhengShenFenIDReturn(true);

                            //验证通过
                            Debug.Log("服务器身份验证通过!");
                            Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().WriteYanZheng();
                            //更新年龄
                            Game_PublicClassVar.Get_function_Rose.FangChenMiYear();
                        }

                        //Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus = false;
                    }

                }));
                break;



            //验证身份证
            case 10000133:

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ShenFenBangDing", "1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }));

                break;


            //找回角色列表
            case 10000141:

                Pro_FindRoseList proFindRoseList = ProtoBuf.Serializer.Deserialize<Pro_FindRoseList>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    if (Game_PublicClassVar.Get_gameServerObj.Obj_FindRose != null) {
                        Game_PublicClassVar.Get_gameServerObj.Obj_FindRose.GetComponent<UI_FindRose>().ProFindRoseList = proFindRoseList;
                        Game_PublicClassVar.Get_gameServerObj.Obj_FindRose.GetComponent<UI_FindRose>().Init();
                    }

                }));

                break;


            //重置玩家效验文件错误
            case 10000151:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_1.str_1 == SystemInfo.deviceUniqueIdentifier) {
                        Game_PublicClassVar.Get_wwwSet.ClearnFileYanZheng();
                    }
                }));

                break;


            //更改邮件密匙
            case 10000152:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_4.str_1 != ""&& pro_ComStr_4.str_1 != "0" && pro_ComStr_4.str_1 != null)
                    {
                        EncryptKey_DongTai = pro_ComStr_4.str_1;
                    }
                }));

                break;



            //重置玩家效验文件错误
            case 10000153:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Debug.Log("pro_ComStr_1.str_1 = " + pro_ComStr_1.str_1 + "SystemInfo.deviceUniqueIdentifier = " + SystemInfo.deviceUniqueIdentifier);
                    if (pro_ComStr_1.str_1 == SystemInfo.deviceUniqueIdentifier)
                    {
                        PlayerPrefs.SetString("YanZhengFileStr_" + Game_PublicClassVar.Get_wwwSet.NowSelectFileName, "999");
                        Game_PublicClassVar.Get_wwwSet.ClearnFileYanZheng();
                    }
                }));

                break;



            //******************通用请求****************************
            //发送广播
            case 10001001:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Application.loadedLevelName != "StartGame")
                    {
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(pro_ComStr_1.str_1);
                    }
                    
                }));
                break;

            //发送道具
            case 10001002:
                /*
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //发送奖励
                    string[] ServerMessageStrList = pro_ComStr_1.str_1.Split(';');
                    for (int i = 0; i < ServerMessageStrList.Length; i++)
                    {
                        if (ServerMessageStrList[i] != "")
                        {
                            string[] serverList = ServerMessageStrList[i].Split(',');
                            string sendItem = serverList[0];
                            int sendItemNum = int.Parse(serverList[1]);
                            //发送奖励
                            Game_PublicClassVar.Get_function_Rose.SendRewardToBag(sendItem, sendItemNum ,"0",0,"0",true,"7");
                        }
                    }
                }));
                */
                break;

            //获取玩家名称和信息
            case 10001003:
                
                break;

            //解封
            case 10001004:
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_wwwSet.IfFengHaoStatus = false;
                IfFengHaoStatus = false;
                break;

            //封号
            case 10001005:
                Debug.Log("接受到封号请求,开始封号");
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", "1","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                Game_PublicClassVar.Get_wwwSet.IfFengHaoStatus = true;
                IfFengHaoStatus = true;
                break;

            //发送角色全部信息
            case 10001006:

                SendToServerBuf(xuhaoID,"");

                break;

            //接受角色全部信息
            case 10001007:
                Debug.Log("接受玩家的全部数据!" + "serverbyte = " + serverbyte.Length);
                Pro_PlayAllData proPlay = ProtoBuf.Serializer.Deserialize<Pro_PlayAllData>(ms);
                Debug.Log("接受玩家的全部数据! 等级" + proPlay.Write_Lv);

                //获取当前玩家的职业
                string roseOcc = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Debug.Log("roseOcc = " + roseOcc + "proPlay.Write_RoseOccupation = " + proPlay.Write_RoseOccupation);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //职业数据一直在保存账号
                    if (roseOcc == proPlay.Write_RoseOccupation)
                    {
                        Game_PublicClassVar.Get_function_Rose.SavePlayAllData(proPlay);
                    }
                    else {
                        string occStr = "";
                        switch (proPlay.Write_RoseOccupation) { 
                            case "1":
                                occStr = "战士";
                                break;

                            case "2":
                                occStr = "魔法师";
                                break;

                            case "3":
                                occStr = "狩猎者";
                                break;
                        }

                        //发送解除账号锁定
                        Debug.Log("下载数据失败！职业不匹配！");
                        SendToServerBuf(10001013, proPlay.Write_ZhangHaoID);

                        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_101");
                        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_102");
                        string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_103");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + occStr + langStrHint_2 + occStr + langStrHint_3);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你要下载的账号" + occStr + ",请创建一个" + occStr + "角色在下载此数据");
                    }
                }));
                break;


            //强制下载角色数据
            case 100010071:

                Game_PublicClassVar.Get_wwwSet.QiangZhiDownRose = true;
                Game_PublicClassVar.Get_wwwSet.QiangZhiDownData();

                break;

            //根据返回值设置是否允许下载
            case 100010072:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc != null) {
                        if (pro_ComStr_4.str_1 == "1")
                        {

                            if (pro_ComStr_4.str_2 == Game_PublicClassVar.Get_wwwSet.FindRoseZhangHaoID)
                            {
                                Game_PublicClassVar.Get_wwwSet.FindRoseStatus = true;
                                Game_PublicClassVar.Get_wwwSet.NowSelectFileName = "";
                                //根据下载职业默认创建
                                switch (pro_ComStr_4.str_3)
                                {
                                    //创建战士
                                    case "1":
                                        Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Obj_CreateRoseSet.GetComponent<UI_CreateRose>().btn_createZhanShi();
                                        break;
                                    //创建法师
                                    case "2":
                                        Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Obj_CreateRoseSet.GetComponent<UI_CreateRose>().btn_createFaShi();
                                        break;
                                    //创建猎人
                                    case "3":
                                        Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Obj_CreateRoseSet.GetComponent<UI_CreateRose>().btn_createLieRen();
                                        break;
                                }

                                Game_PublicClassVar.Get_gameServerObj.Obj_UI_StartGameFunc.GetComponent<UI_StartGameFunc>().Obj_CreateRoseBtnSet.GetComponent<UI_StartGame>().Btn_CreateRose();

                                //弹出提示
                                Game_PublicClassVar.gameLinkServer.HintMsgStatus_StartGame = true;
                                Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "当前正在进行数据下载中,请不要进行任何操作,下载完成后请自己重新启动游戏即可!";

                            }
                        }
                        else {
                            //弹出提示
                            Game_PublicClassVar.gameLinkServer.HintMsgStatus_StartGame = true;
                            Game_PublicClassVar.gameLinkServer.HintMsgText_StartGame = "服务器不允许您下载此角色!";
                        }
                    }

                    Game_PublicClassVar.Get_wwwSet.StartFindRoseStatus = false;


                }));
                break;

            //存储账号ID
            case 10001008:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                Debug.Log("存储了新的账号ID : " + pro_ComStr_1.str_1);
                string nowZhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                if (nowZhangHaoID.Length <= 10) {
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("ZhangHaoID", pro_ComStr_1.str_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                }
                //获取当前已经确定了唯一的ID,开始上传玩家数据
                if (WeiYiIDStatus == false) {
                    WeiYiIDStatus = true;
                }
                SendIDYanZhengStatus = false;

                break;


            //接受修改密码信息
            case 10001009:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                switch (pro_ComStr_1.str_1) { 
                    case "1":
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("密码修改成功！");
                        Debug.Log("密码修改成功！");
                        break;
                    case "2":
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("密码修改失败！");
                        Debug.Log("密码修改失败，请确认密码是否正确！");
                        break;
                }
                break;


            //验证设备唯一ID
            case 10001010:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                if (pro_ComStr_1.str_1 == "1") {

                    //请求文件验证记录
                    //SendToServerBuf(10001600, "");
                    //Debug.Log("设备验证正常");
                }
                if (pro_ComStr_1.str_1 == "2")
                {
                    //Debug.Log("设备验证异常,封号处理！");
                    Game_PublicClassVar.Get_wwwSet.IfFengHaoStatus = true;
                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("BeiYong_9", "2","ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseConfig");
                }
                break;


            //接受服务端读取的单个数据
            case 10001011:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //Debug.Log("接受到服务器的单个数据 :" + pro_ComStr_1.str_1);
                break;


            //请求版本号
            case 10001014:
                string pingtaiStr = "";
#if UNITY_ANDROID
                pingtaiStr = "1";
#endif
#if UNITY_IPHONE
                pingtaiStr = "2";
#endif

                //平台
                string pingtaiTypeStr = "0";
                if (Game_PublicClassVar.Get_wwwSet.GetComponent<GetSignature>() != null) {
                    pingtaiTypeStr = Game_PublicClassVar.Get_wwwSet.GetComponent<GetSignature>().ChannelId;
                }

                Pro_ComStr_3 proComStr_3 = new Pro_ComStr_3 { str_1 = zhanghaoID, str_2 = gameVerSionStr, str_3 = pingtaiStr,str_4 = pingtaiTypeStr };
                SendToServerBuf(10001014, proComStr_3);
                break;
                

            //滚屏消息
            case 10001015:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //pro_ComStr_1.str_1 = Game_PublicClassVar.Get_xmlScript.Decrypt(pro_ComStr_1.str_1);
                    if (Application.loadedLevelName != "StartGame")
                    {
                        Game_PublicClassVar.Get_gameServerObj.GetComponent<GameServerObj>().Obj_GuangBoList.GetComponent<UI_GuangBoList>().AddGuangBo(pro_ComStr_1.str_1);
                    }
                    
                }));

                break;


            //接受客户端消息,发送给给全服广播
            case 10001019:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    pro_ComStr_1.str_1 = Game_PublicClassVar.Get_xmlScript.Decrypt(pro_ComStr_1.str_1);
                    if (Game_PublicClassVar.Get_gameServerObj.GetComponent<GameServerObj>().Obj_GuangBoList != null) {
                        Game_PublicClassVar.Get_gameServerObj.GetComponent<GameServerObj>().Obj_GuangBoList.GetComponent<UI_GuangBoList>().AddGuangBo(pro_ComStr_1.str_1);
                    }
                    
                }));

                break;



            //接受客户端消息,检查安卓
            case 10001026:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    pro_ComStr_4.str_1 = Game_PublicClassVar.Get_xmlScript.Decrypt(pro_ComStr_4.str_1);
                    Game_PublicClassVar.Get_wwwSet.CheckApkNameStrAdd = pro_ComStr_4.str_1;
                    if (!EventHandle.IsHuiWeiChannel())
                    {
                        Game_PublicClassVar.Get_getSignature.excuteCheckAction();
                    }
                }));
                break;
            //再次发送战斗力
            case 10001029:

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //发送链接请求
                    int shiliValue = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ShiLiValue;
                    //Debug.Log("shiliValue = " + shiliValue);
                    string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    string sendShiLi = Game_PublicClassVar.Get_xmlScript.Encrypt(shiliValue.ToString());

                    string[] sendStrList = new string[] { zhanghaoID, sendShiLi.ToString(), "", "" };
                    Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001029, sendStrList);
                }));

                break;


            //排行榜
            case 10001031:
                //Debug.Log("接受排行榜数据"+ DateTime.Now.ToString());
                Pro_PaiHang pro_PaiHang = ProtoBuf.Serializer.Deserialize<Pro_PaiHang>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    //Debug.Log("接受排行榜数据2222" + DateTime.Now.ToString());
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang != null) {
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().pro_ProList = pro_PaiHang;
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().ReceiveDataStatus = true;
                    }

                }));

                break;


            //排行榜名次
            case 10001032:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang != null)
                    {
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("我的排名");
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_PaiHangRankValue.GetComponent<Text>().text = langStr + ":" + pro_ComStr_1.str_1;
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_RewardEveryDayPaiHang_RankValue.GetComponent<Text>().text = langStr + ":" + pro_ComStr_1.str_1;
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_HeQuPaiHang_RankValue.GetComponent<Text>().text = langStr + ":" + pro_ComStr_1.str_1;
                    }

                    //30名以内实时传输数据
                    int rankValue = int.Parse(pro_ComStr_1.str_1);
                    if (rankValue <= 30) {
                        //每次进入游戏上传一次玩家数据
                        //string[] saveList = new string[] { "", "2", "预留设备号位置" };
                        //SendToServerBuf(10001006, saveList);
                    }
                }));

                break;

            //获取开区信息
            case 10001033:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang != null)
                    {
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_TimeNewStr.GetComponent<Text>().text = pro_ComStr_1.str_1;
                    }
                }));

                break;


            //排行榜名次(职业)
            case 10001034:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //10名以内实时传输数据
                    int rankValue = int.Parse(pro_ComStr_1.str_1);
                    if (rankValue <= 10)
                    {
                        //每次进入游戏上传一次玩家数据
                        //string[] saveList = new string[] { "", "2", "预留设备号位置" };
                        //SendToServerBuf(10001006, saveList);
                    }
                }));

                break;

            //排行榜（大秘境）
            case 10001035:
                //Debug.Log("接受排行榜大秘境数据" + DateTime.Now.ToString());
                Pro_PaiHang_DaMiJing pro_PaiHang_DaMiJing = ProtoBuf.Serializer.Deserialize<Pro_PaiHang_DaMiJing>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang != null)
                    {
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().pro_PaiHang_DaMiJing = pro_PaiHang_DaMiJing;
                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().UpdateDaMiJingStatus = true;
                    }

                }));

                break;

            //排行榜 第一名 称号
            case 10001036:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    ServerPaiHangChengHao = pro_ComStr_1.str_1;
                    if (ServerPaiHangChengHao == "" || ServerPaiHangChengHao == null || ServerPaiHangChengHao == "0")
                    {
                        //判断当前是否为天下第一的称号，如果是则取消佩戴
                        string nowChengHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowChengHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        string chengHaoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (chengHaoIDSet.Contains(nowChengHaoID) == false)
                        {
                            if (Application.loadedLevelName != "StartGame")
                            {
                                Game_PublicClassVar.Get_function_Rose.ChengHao_Null();
                            }
                            else {
                                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowChengHaoID","", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                            }
                        }
                    }
                    else {
                        if (Application.loadedLevelName != "StartGame")
                        {
                            Game_PublicClassVar.Get_function_Rose.ChengHao_Use(pro_ComStr_1.str_1);
                        }
                        else
                        {
                            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowChengHaoID", pro_ComStr_1.str_1, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                            Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                        }
                    }
                }));

                break;


            //排行榜类型显示
            case 10001037:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang != null)
                    {
                        //Debug.Log("排行类型:" + pro_ComStr_1.str_1);
                        string[] strList = pro_ComStr_1.str_1.Split(';');
                        string type = "";
                        string startTime = "";
                        string hequTime = "";
                        if (strList.Length >=3) {
                            type = strList[0];
                            startTime = strList[1];
                            hequTime = strList[2];
                        }

                        Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().ShowRewardRank(type, startTime, hequTime);
                    }
                }));
                break;

            //接受邮件数据
            case 10001040:
                //Debug.Log("接受邮件奖励数据");
                Pro_Mail pro_Mail = ProtoBuf.Serializer.Deserialize<Pro_Mail>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_Mail != null)
                    {
                        Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().pro_Mail = pro_Mail;
                        Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().MailUpdateStatus = true;
                    }
                }));

                break;

            //接受邮件数据(动态)

            case 100010401:
                //Debug.Log("接受邮件奖励数据");
                
                pro_Mail = ProtoBuf.Serializer.Deserialize<Pro_Mail>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环处理排行
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_Mail != null)
                    {
                        Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().pro_Mail = pro_Mail;
                        Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().MailUpdateStatus = true;
                    }
                }));

                break;


            //发送邮件奖励
            case 10001041:
                Debug.Log("接受邮件数据");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().SelectMailID == pro_ComStr_1.str_1) {
                        Game_PublicClassVar.Get_gameServerObj.Obj_Mail.GetComponent<UI_Mail>().SendMailReward();
                    }
                }));

                break;


            //发送邮件奖励
            case 10001042:
                //Debug.Log("接受邮件数据");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_1.str_1 == "" || pro_ComStr_1.str_1 == "0" || pro_ComStr_1.str_1 == null)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MailHint.SetActive(false);
                    }
                    else {
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MailHint.SetActive(true);
                        MailHintStatus = true;
                    }

                }));

                break;

            //显示奖励人数
            case 10001060:

                //Debug.Log("显示奖励人数");
                pro_ComStr_3 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_3>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    switch (pro_ComStr_3.str_1) {
                        //等级显示
                        case "1":
                            if (pro_ComStr_3.str_2 != null && pro_ComStr_3.str_2 != "")
                            {
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardLv.SetActive(true);
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardLv.GetComponent<UI_ZhanQu_RewardLv>().ZhanQuRewardNumStr = pro_ComStr_3.str_2;
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardLv.GetComponent<UI_ZhanQu_RewardLv>().UpdateStatus = true;
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardLv.GetComponent<UI_ZhanQu_RewardLv>().ZhanQuRewardOnlyID = pro_ComStr_3.str_3;
								Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_ZhanQuHuoDong>().Obj_Btn_ZhanQuRewardLv.SetActive(true);
                            }
                            else {
								Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_ZhanQuHuoDong>().Obj_Btn_ZhanQuRewardLv.SetActive(false);
                            }

                            break;

                        //战斗力显示
                        case "2":
                            if (pro_ComStr_3.str_2 != null && pro_ComStr_3.str_2 != "")
                            {
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardShiLi.SetActive(true);
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardShiLi.GetComponent<UI_ZhanQu_RewardShiLi>().ZhanQuRewardNumStr = pro_ComStr_3.str_2;
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardShiLi.GetComponent<UI_ZhanQu_RewardShiLi>().UpdateStatus = true;
                                Game_PublicClassVar.Get_gameServerObj.Obj_ZhanQu_RewardShiLi.GetComponent<UI_ZhanQu_RewardShiLi>().ZhanQuRewardOnlyID = pro_ComStr_3.str_3;
								Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_ZhanQuHuoDong>().Obj_Btn_ZhanQuRewardShiLi.SetActive(true);
                            }
                            else {
								Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_ZhanQuHuoDong>().Obj_Btn_ZhanQuRewardShiLi.SetActive(false);
                            }
                            break;
                    }
                    
                }));

                break;

            //发送战区奖励数据
            case 10001061:
                //Debug.Log("发送战区奖励数据");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                }));

                break;



            //验证是否打开战区相关按钮
            case 10001062:
                //Debug.Log("发送战区奖励数据");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_1.str_1 != "" && pro_ComStr_1.str_1 != null)
                    {
                        string[] strList = pro_ComStr_1.str_1.Split(';');
                        if (strList.Length >= 2)
                        {
                            switch (strList[0])
                            {
                                case "1":
                                    if (strList[1] == "1")
                                    {
                                        Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Obj_Btn_ZhanQuRewardLv.SetActive(true);
                                    }
                                    else
                                    {
                                        Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Obj_Btn_ZhanQuRewardLv.SetActive(false);
                                    }
                                    break;
                                case "2":
                                    if (strList[1] == "1")
                                    {
                                        Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Obj_Btn_ZhanQuRewardShiLi.SetActive(true);
                                    }
                                    else
                                    {
                                        Game_PublicClassVar.Get_gameServerObj.Obj_HuoDongDaTing.GetComponent<UI_HuoDongDaTing>().Obj_Btn_ZhanQuRewardShiLi.SetActive(false);
                                    }
                                    break;
                            }
                        }
                    }
                }));

                break;
            


			//发送拍卖行信息
			case 10001071:
				//Debug.Log("发送战区奖励数据");
				//Pro_PaiMaiDataList pro_PaiMaiDataList = ProtoBuf.Serializer.Deserialize<Pro_PaiMaiDataList>(ms);

				//跨线程调用(跨线程调用的值不能传出)
				MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
					{
                        //循环解密
                        //Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().PaimaiItemShow = pro_PaiMaiDataList.PaiMai_DataList;
						//Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowPaiMaiHangUI();
					}));

				break;

			//拍卖行玩家自身出售信息
			case 10001074:
				/*
                //Debug.Log("发送战区奖励数据");
				Pro_PaiMai_PlayerSellList pro_PaiMai_PlayerSellList = ProtoBuf.Serializer.Deserialize<Pro_PaiMai_PlayerSellList>(ms);

				//跨线程调用(跨线程调用的值不能传出)
				MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
					{
					Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ChuShouItemShow = pro_PaiMai_PlayerSellList.Pro_PaiMai_PlayerSell_List;
					//Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Rose_PaiMaiHangList_Update = true;
					Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowSaleItemList();
					}));
				*/
				break;

			//获取兑换数据
			case 10001076:
				//Debug.Log("发送战区奖励数据");
				Pro_ComStr_2 pro_ComStr_2 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_2>(ms);
                if (pro_ComStr_2.str_1 == ""|| pro_ComStr_2.str_1 == null) {
                    pro_ComStr_2.str_1 = "0";
                }
                if (pro_ComStr_2.str_2 == "" || pro_ComStr_2.str_2 == null)
                {
                    pro_ComStr_2.str_2 = "0";
                }
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
					{
					Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().DuiHuanRmbValuePro = int.Parse(pro_ComStr_2.str_1);
					Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().DuiHuanRmbValuePro_Last = int.Parse(pro_ComStr_2.str_2);
					Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowDuiHuanNum();
					}));

				break;


            //是否可以出售道具
            case 10001077:
                //Debug.Log("发送战区奖励数据");
                Pro_PaiMaiSellData pro_PaiMaiSellData = ProtoBuf.Serializer.Deserialize<Pro_PaiMaiSellData>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //弹出上架提示框
                    if (pro_PaiMaiSellData.GoldValue != "" && pro_PaiMaiSellData.GoldValue != "-1" && pro_PaiMaiSellData.GoldValue != "0")
                    {
                        //清理其他显示列表,只保留一个
                        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Game_PublicClassVar.gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Obj_ChuShouItemShowSet);

                        GameObject saleObj = (GameObject)Instantiate(Game_PublicClassVar.gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Obj_ChuShouItemShow);
                        saleObj.transform.SetParent(Game_PublicClassVar.gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Obj_ChuShouItemShowSet.transform);
                        saleObj.transform.localPosition = Vector3.zero;
                        saleObj.GetComponent<UI_PaiMaiChuShouShow>().BagSpace = pro_PaiMaiSellData.BagSpaceNum;
                        saleObj.GetComponent<UI_PaiMaiChuShouShow>().Sale_ItemID = pro_PaiMaiSellData.PaiMaiItemID;
                        saleObj.GetComponent<UI_PaiMaiChuShouShow>().sale_ItemNum = int.Parse(pro_PaiMaiSellData.PaiMaiItemNum);
                        saleObj.GetComponent<UI_PaiMaiChuShouShow>().Sale_Gold = (int)(int.Parse(pro_PaiMaiSellData.GoldValue) * int.Parse(pro_PaiMaiSellData.PaiMaiItemNum));
                        saleObj.GetComponent<UI_PaiMaiChuShouShow>().ShowSale();
                    }
                    else {
                        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_121");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前物品无法在拍卖行出售!");
                    }
                }));

                break;


            //发送拍卖行信息（解密）
            case 10001081:
                //Debug.Log("接受拍卖行发过来的数据...接受拍卖行发过来的数据...接受拍卖行发过来的数据...接受拍卖行发过来的数据...");
                Pro_PaiMaiDataList pro_PaiMaiDataList = ProtoBuf.Serializer.Deserialize<Pro_PaiMaiDataList>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //循环解密
                    Dictionary<string, Pro_PaiMaiDataAdd> PaimaiItemShow = new Dictionary<string, Pro_PaiMaiDataAdd>();

                    //Debug.Log("拍卖行数据:" + pro_PaiMaiDataList.PaiMai_DataList.Count);
                    foreach (string listID in pro_PaiMaiDataList.PaiMai_DataList.Keys)
                    {
                        Pro_PaiMaiData now_PaiMaiData = new Pro_PaiMaiData();
                        if (pro_PaiMaiDataList.PaiMai_DataList.ContainsKey(listID)) {
                            now_PaiMaiData = pro_PaiMaiDataList.PaiMai_DataList[listID];
                            Pro_PaiMaiDataAdd pro_PaiMaiData = new Pro_PaiMaiDataAdd();
                            pro_PaiMaiData.PaiMaiID = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiID);
                            pro_PaiMaiData.PaiMaiType = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiType);
                            pro_PaiMaiData.PaiMaiItemID = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiItemID);
                            pro_PaiMaiData.PaiMaiItemNum = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiItemNum);
                            pro_PaiMaiData.GoldType = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.GoldType);
                            pro_PaiMaiData.GoldValue = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.GoldValue);
                            pro_PaiMaiData.GoldBianHuaValue = now_PaiMaiData.GoldBianHuaValue;
                            pro_PaiMaiData.GoldBianHuaValueStr = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.GoldBianHuaValueStr);
                            PaimaiItemShow.Add(listID, pro_PaiMaiData);
                        }
                    }
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().PaimaiItemShow = PaimaiItemShow;
                    //Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Rose_PaiMaiHangList_Update = true;
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowPaiMaiHangUI();
                }));

                break;


            //拍卖行玩家自身出售信息(加密)
            case 10001084:
                //Debug.Log("发送战区奖励数据");
                Pro_PaiMai_PlayerSellList pro_PaiMai_PlayerSellList = ProtoBuf.Serializer.Deserialize<Pro_PaiMai_PlayerSellList>(ms);


                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    //循环解密
                    Dictionary<string, Pro_PaiMai_PlayerSellAdd> Pro_PaiMai_PlayerSell_List = new Dictionary<string, Pro_PaiMai_PlayerSellAdd>();
                    //Debug.Log("拍卖行数据:" + pro_PaiMaiDataList.PaiMai_DataList.Count);
                    foreach (string listID in pro_PaiMai_PlayerSellList.Pro_PaiMai_PlayerSell_List.Keys)
                    {
                        Pro_PaiMai_PlayerSell now_PaiMaiData = new Pro_PaiMai_PlayerSell();
                        if (pro_PaiMai_PlayerSellList.Pro_PaiMai_PlayerSell_List.ContainsKey(listID))
                        {
                            now_PaiMaiData = pro_PaiMai_PlayerSellList.Pro_PaiMai_PlayerSell_List[listID];
                            Pro_PaiMai_PlayerSellAdd pro_PaiMaiData = new Pro_PaiMai_PlayerSellAdd();
                            pro_PaiMaiData.GoldType = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.GoldType);
                            pro_PaiMaiData.GoldValue = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.GoldValue);
                            pro_PaiMaiData.PaiMaiItemID = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiItemID);
                            pro_PaiMaiData.PaiMaiItemNum = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.PaiMaiItemNum);
                            pro_PaiMaiData.SellID = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.SellID);
                            pro_PaiMaiData.SellTime = Game_PublicClassVar.Get_xmlScript.Decrypt(now_PaiMaiData.SellTime);
                            Pro_PaiMai_PlayerSell_List.Add(listID, pro_PaiMaiData);
                        }
                    }

                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ChuShouItemShow = Pro_PaiMai_PlayerSell_List;
                    //Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().Rose_PaiMaiHangList_Update = true;
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowSaleItemList();
                }));

                break;


            //获取兑换数据
            case 10001086:

                //Debug.Log("发送战区奖励数据");
                pro_ComStr_2 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_2>(ms);

                string str_1 = Game_PublicClassVar.Get_xmlScript.Decrypt(pro_ComStr_2.str_1);
                string str_2 = Game_PublicClassVar.Get_xmlScript.Decrypt(pro_ComStr_2.str_2);

                if (str_1 == "" || str_1 == null)
                {
                    str_1 = "0";
                }
                if (str_2 == "" || str_2 == null)
                {
                    str_2 = "0";
                }
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().DuiHuanRmbValuePro = int.Parse(str_1);
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().DuiHuanRmbValuePro_Last = int.Parse(str_2);
                    Game_PublicClassVar.Get_gameServerObj.Obj_PaiMaiHang.GetComponent<UI_PaiMaiHangSet>().ShowDuiHuanNum();
                }));

                break;


            //玩家信息装备
            case 10001091:
				Pro_PlayerEquipDataList pro_PlayerEquipDataList = ProtoBuf.Serializer.Deserialize<Pro_PlayerEquipDataList>(ms);
				//跨线程调用(跨线程调用的值不能传出)
				MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
				{
					Debug.Log("打开装备界面");
                 
                    UI_FunctionOpen f_open = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
                    GameObject obj_PaiHangRoseEquipShow = f_open.FunctionInstantiate(f_open.Obj_PlayerEquipShow, "Obj_PlayerEquipShow");
                    obj_PaiHangRoseEquipShow.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
					obj_PaiHangRoseEquipShow.transform.localPosition = new Vector3(40,0,0);
					obj_PaiHangRoseEquipShow.transform.localScale = new Vector3(1, 1, 1);
					obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseLv = pro_PlayerEquipDataList.str_4;
					obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseName = pro_PlayerEquipDataList.str_3;
					obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseOcc = pro_PlayerEquipDataList.str_6;
					obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipIDList = pro_PlayerEquipDataList.str_1.Split('|');
					//obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipIDDic = roseEquipIDDic;

					//隐藏属性ID和隐藏值
					Dictionary<string, string> roseEquipHideDic = new Dictionary<string, string>();
					string roseEquipHideStr = pro_PlayerEquipDataList.str_2;
					if (roseEquipHideStr != ""&& roseEquipHideStr != null) {
						string[] hideList = roseEquipHideStr.Split(']');
						//Debug.Log("roseEquipHideStr = " + roseEquipHideStr);
						for (int i = 0; i < hideList.Length; i++)
						{
							//Debug.Log("hideList[i] = " + hideList[i]);
							string[] hideIDPro = hideList[i].Split('[');
							//hideIDPro[0] 为0表示没有装备
							if (hideIDPro[0] != "0")
							{
								roseEquipHideDic.Add(hideIDPro[0], hideIDPro[1]);
							}
						}
					}

					obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipHideDic = roseEquipHideDic;
                    obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().nowYanSeID = pro_PlayerEquipDataList.NowYanSeID;
                    obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().nowNowYanSeHairID = pro_PlayerEquipDataList.NowNowYanSeHairID;
                    obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().ShowEquip();

				}));

				break;

			//玩家信息宠物
			case 10001092:
				pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
				//跨线程调用(跨线程调用的值不能传出)
				MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
				{
					string[] petIDSetList = pro_ComStr_1.str_1.Split('|');

                    UI_FunctionOpen f_open = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
                    GameObject showPetobj = f_open.FunctionInstantiate(f_open.Obj_PlayerPetShow, "Obj_PlayerPetShow");
                    showPetobj.transform.SetParent (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
					showPetobj.GetComponent<UI_PaiHangShowPet>().SerVer_PetDataList = petIDSetList;
					showPetobj.GetComponent<UI_PaiHangShowPet>().Inti();
					showPetobj.transform.localScale = new Vector3(1, 1, 1);
					showPetobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
					showPetobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
				}));

				break;


            //玩家信息宠物
            case 10001093:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    string[] petIDSetList = pro_ComStr_4.str_1.Split('|');

                    UI_FunctionOpen f_open = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
                    GameObject showPetobj = f_open.FunctionInstantiate(f_open.Obj_PlayerPetShow, "Obj_PlayerPetShow");

                    showPetobj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    showPetobj.GetComponent<UI_PaiHangShowPet>().SerVer_PetDataList = petIDSetList;
                    showPetobj.GetComponent<UI_PaiHangShowPet>().roseEquipHideStr = pro_ComStr_4.str_2;
                    showPetobj.GetComponent<UI_PaiHangShowPet>().Inti();
                    showPetobj.transform.localScale = new Vector3(1, 1, 1);
                    showPetobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
                    showPetobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
                }));

                break;

            //首杀数据显示
            case 10001101:
			Pro_ShouShaNameList pro_ShouShaNameList = ProtoBuf.Serializer.Deserialize<Pro_ShouShaNameList>(ms);
			    //跨线程调用(跨线程调用的值不能传出)
			    MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
				{
					Game_PublicClassVar.Get_gameServerObj.Obj_ChengJiu.GetComponent<Rose_ChengJiuSet>().pro_ShouShaNameList = pro_ShouShaNameList;
					Game_PublicClassVar.Get_gameServerObj.Obj_ChengJiu.GetComponent<Rose_ChengJiuSet>().ShowShouShaName();
				}));
				break;

            //清空每日登陆数据
            case 10001111:
                //Pro_ShouShaNameList pro_ShouShaNameList = ProtoBuf.Serializer.Deserialize<Pro_ShouShaNameList>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //if (Application.loadedLevelName != "StartGame") {
                        Game_PublicClassVar.Get_wwwSet.DayUpdataStatus = true;
                    //}
                }));
                break;


            //世界等级数据
            /*
            case 10001301:
                
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_4.str_1 != null&& pro_ComStr_4.str_1 != "") {
                        int worldLv = int.Parse(pro_ComStr_4.str_1);
                        string worldPlayName = pro_ComStr_4.str_2;
                        int worldPlayLv = int.Parse(pro_ComStr_4.str_3);

                        Game_PublicClassVar.Get_wwwSet.WorldLv = worldLv;
                        Game_PublicClassVar.Get_wwwSet.WorldPlayerLv = worldPlayLv;
                        Game_PublicClassVar.Get_wwwSet.WorldPlayerName = worldPlayName;

                        //获取等级经验加成
                        Game_PublicClassVar.Get_function_Rose.GetWorldLvExpPro();

                        //如果界面当前为打开状态
                        if (Game_PublicClassVar.Get_gameServerObj.Obj_WorldLv != null)
                        {
                            //更新界面
                            Game_PublicClassVar.Get_gameServerObj.Obj_WorldLv.GetComponent<UI_WorldLvSet>().Init();
                        }
                    }
  
                }));
                
                break;
            */

            //世界等级数据
            case 10001302:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_4.str_1 != null && pro_ComStr_4.str_1 != "")
                    {
                        int worldLv = int.Parse(Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(pro_ComStr_4.str_1));
                        string worldPlayName = Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(pro_ComStr_4.str_2);
                        int worldPlayLv = int.Parse(Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(pro_ComStr_4.str_3));
                        string zhanghaoIDStr = Game_PublicClassVar.Get_xmlScript.Decrypt_DongTai(pro_ComStr_4.str_4);
                        string nowzhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                        if (nowzhanghaoID == zhanghaoIDStr)
                        {
                            Game_PublicClassVar.Get_wwwSet.WorldLv = worldLv;
                            Game_PublicClassVar.Get_wwwSet.WorldPlayerLv = worldPlayLv;
                            Game_PublicClassVar.Get_wwwSet.WorldPlayerName = worldPlayName;

                            //获取等级经验加成
                            Game_PublicClassVar.Get_function_Rose.GetWorldLvExpPro();

                            //如果界面当前为打开状态
                            if (Game_PublicClassVar.Get_gameServerObj.Obj_WorldLv != null)
                            {
                                //更新界面
                                Game_PublicClassVar.Get_gameServerObj.Obj_WorldLv.GetComponent<UI_WorldLvSet>().Init();
                            }
                        }
                    }
                }));
                break;


            //抢夺红包
            case 10001400:

                Pro_ComStr_4 pro_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //设置红包总的额度
                    HongBaoSumValue = int.Parse(pro_4.str_1);
                    HongBaoCostTime = float.Parse(pro_4.str_2);
                    HongBaoStatus = true;
                    if (Application.loadedLevelName != "StartGame") {
                        //控制红包选项显示和隐藏
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnHongBao.SetActive(true);
                    }
                }));

                break;


            //玩家请求抢红包其数据
            case 10001401:
                Pro_ComInt_1 proComInt_1 = ProtoBuf.Serializer.Deserialize<Pro_ComInt_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.gameServerObj.Obj_HongBao.GetComponent<UI_HongBaoSet>().SelfHongBaoValue = proComInt_1.int_1;
                    Game_PublicClassVar.gameServerObj.Obj_HongBao.GetComponent<UI_HongBaoSet>().LingQuReturn();
                }));
                break;



            //玩家请求抢红包其他玩家数据
            case 10001402:
                Pro_HongBaoListData proHongBaoListData = ProtoBuf.Serializer.Deserialize<Pro_HongBaoListData>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.gameServerObj.Obj_HongBao.GetComponent<UI_HongBaoSet>().proHongBaoListData = proHongBaoListData;
                    Game_PublicClassVar.gameServerObj.Obj_HongBao.GetComponent<UI_HongBaoSet>().Return_ShowPlayerLingQuData();
                }));
                break;

            //红包活动取消,隐藏所有红包图标
            case 10001403:
                //Pro_HongBaoListData proHongBaoListData = ProtoBuf.Serializer.Deserialize<Pro_HongBaoListData>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //控制红包选项显示和隐藏
                    if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null)
                    {
                        HongBaoStatus = false;
                        if (Application.loadedLevelName != "StartGame")
                        {
                            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BtnHongBao.SetActive(false);
                        }
                    }
                }));
                break;


            //玩家接受服务器返回的宠物天梯挑战信息
            case 10001502:

                Pro_RankPetListData proRankPetListData = ProtoBuf.Serializer.Deserialize<Pro_RankPetListData>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_PetTianTi != null) {
                        Game_PublicClassVar.gameServerObj.Obj_PetTianTi.GetComponent<UI_PetTianTiSet>().proRankListData = proRankPetListData;
                        Game_PublicClassVar.gameServerObj.Obj_PetTianTi.GetComponent<UI_PetTianTiSet>().Init();
                    }
                }));
                break;

            //获取自身排名信息
            case 10001504:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_PetTianTi != null) {
                        Game_PublicClassVar.gameServerObj.Obj_PetTianTi.GetComponent<UI_PetTianTiSet>().SelfRank = pro_ComStr_1.str_1;
                        Game_PublicClassVar.gameServerObj.Obj_PetTianTi.GetComponent<UI_PetTianTiSet>().ShowMyRank();
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("我的排名");
                        Game_PublicClassVar.gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_RewardPetPaiHang_RankValue.GetComponent<Text>().text = langStr + ":" + pro_ComStr_1.str_1;
                    }

                    if (Game_PublicClassVar.gameServerObj.Obj_PaiHang != null) {
                        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("我的排名");
                        Game_PublicClassVar.gameServerObj.Obj_PaiHang.GetComponent<UI_PaiHang>().Obj_HeQuPetPaiHang_RankValue.GetComponent<Text>().text = langStr + ":" + pro_ComStr_1.str_1;
                    }

                }));
                break;

            //获取服务器文件验证
            case 10001600:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //写入文件
                    Game_PublicClassVar.Get_wwwSet.GetFileServerYanZheng(pro_ComStr_1.str_1);

                }));

                break;


            //接受服务器的验证码
            case 10001601:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //写入文件
                    Game_PublicClassVar.Get_wwwSet.WriteFileServerYanZheng(pro_ComStr_1.str_1);

                }));
                break;

            //服务器允许传送数据
            case 10001603:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_wwwSet.UpdatePlayerDataToServer = true;

                }));
                break;

            //宝箱活动开启
            case 10001801:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang != null) {
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().HuoDongCostTime = float.Parse(pro_ComStr_4.str_1);
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().IfEndBaoXiangActive = false;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().BaoXiangActiveStatus = true;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().Init();
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().Obj_ShiQuBtn.SetActive(true);
                    }
                }));
                break;

            //宝箱活动关闭
            case 10001802:

                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang != null){
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().BaoXiangActiveStatus = false;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().IfEndBaoXiangActive = true;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_BaoXiang.GetComponent<UI_HuoDong_BaoXiang>().HuoDongCostTime = 0;
                    }
                }));
                break;


            //狩猎开启
            case 10001900:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //控制红包选项显示和隐藏
                    if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null && Application.loadedLevelName!="StartGame") {
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HuoDongBtn_ShouLie.SetActive(true);
                        HuoDong_ShouLie_Time = float.Parse(pro_ComStr_4.str_1);
                        HuoDong_ShouLie_Status = true;
                        HuoDong_ShouLie_SendNumMax = 10;
                    }

                }));
                break;


            //获取狩猎数据
            case 10001902:

                HuoDong_ShouLie huoDong_ShouLie = ProtoBuf.Serializer.Deserialize<HuoDong_ShouLie>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie != null)
                    {
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().ShouLieStr = huoDong_ShouLie.HuoDong_ShouLieRankData;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().ShouLieDataStr = huoDong_ShouLie.HuoDong_ShouLieRewardData;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().HuoDongTime = huoDong_ShouLie.HuoDong_Time;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().KillNum = huoDong_ShouLie.HuoDong_KillNum;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().RankValue = huoDong_ShouLie.HuoDong_SelfRankStr;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().UpdateDataTime = huoDong_ShouLie.HuoDong_UpdateTime;
                        Game_PublicClassVar.gameServerObj.Obj_HuoDong_ShouLie.GetComponent<UI_HuoDong_ShouLie>().Init();
                    }
                }));
                break;

            //狩猎结束
            case 10001904:
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //控制红包选项显示和隐藏
                    //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HuoDongBtn_ShouLie.SetActive(true);
                    Game_PublicClassVar.gameLinkServer.HuoDong_ShouLie_Status = false;
                    HuoDong_ShouLie_SendNumMax = 10;
                    if (Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set != null && Application.loadedLevelName != "StartGame")
                    {
                        //延迟显示领取奖励
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().HuoDong_ShowLie_YanChiShowStatus = true;
                        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().HuoDong_ShowLie_RewardTime = 900;
                        Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_YanChiShowStatus = true;
                        Game_PublicClassVar.Get_game_PositionVar.HuoDong_ShowLie_RewardTime = 900;
                    }
                }));
                break;


            //狩猎结束
            case 10002001:

                Pro_JiaoHuData pro_JiaoHuData = ProtoBuf.Serializer.Deserialize<Pro_JiaoHuData>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    for (int i = 0; i < pro_JiaoHuData.JiaoHuData.Count;i++) {
                        Game_PublicClassVar.Get_function_Pasture.PastureCreateJiaoHuData(pro_JiaoHuData);
                    }
                }));
                break;


            //玩家矿场交互数据
            case 10002012:

                proRankPetListData = ProtoBuf.Serializer.Deserialize<Pro_RankPetListData>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.gameServerObj.Obj_KuangSet != null)
                    {
                        Game_PublicClassVar.gameServerObj.Obj_KuangSet.GetComponent<UI_PastureKuangSet>().proRankListData = proRankPetListData;
                        Game_PublicClassVar.gameServerObj.Obj_KuangSet.GetComponent<UI_PastureKuangSet>().Init();
                    }
                }));

                break;


            //狩猎开启
            case 10002014:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //控制红包选项显示和隐藏
                    if (Application.loadedLevelName != "StartGame")
                    {
                        //写入掠夺数据
                        if (pro_ComStr_4.str_1 == null|| pro_ComStr_4.str_1 == ""|| pro_ComStr_4.str_1 == "0") {
                            pro_ComStr_4.str_1 = "0@0@0";
                        }
                        Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("KuangLvDuoData", pro_ComStr_4.str_1.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                        Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");
                    }

                }));
                break;

            //魔塔活动开启
            case 10002201:

                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("TowerCengTime", "900", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                    //Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseDayReward");

                    //活动按钮显示
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HuoDongBtn_Tower.SetActive(true);
                    HuoDong_Tower_Status = true;

                }));

                break;

            //魔塔活动关闭
            case 10002202:

                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //活动按钮关闭
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_HuoDongBtn_Tower.SetActive(false);
                    HuoDong_Tower_Status = false;
                    HuoDong_Tower_EndShowTime = 1200;       //额外留出20分钟用来领取奖励

                }));

                break;

            //阵营实力
            case 10002301:

                Pro_ZhenYingDataList proZhenYingDataList = ProtoBuf.Serializer.Deserialize<Pro_ZhenYingDataList>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //活动按钮显示
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null) {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingDataList = proZhenYingDataList;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().Btn_ShiLi();
                    }
                }));

                break;


            //阵营实力
            case 10002304:

                Pro_ZhenYingXuanZeDataList proZhenYingXuanZeDataList = ProtoBuf.Serializer.Deserialize<Pro_ZhenYingXuanZeDataList>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //活动按钮显示
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingXuanZeDataList = proZhenYingXuanZeDataList;
                    }
                }));

                break;


            //阵营实力
            case 10002305:

                proZhenYingXuanZeDataList = ProtoBuf.Serializer.Deserialize<Pro_ZhenYingXuanZeDataList>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //活动按钮显示
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ProZhenYingZhiWeiDataList = proZhenYingXuanZeDataList;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().Obj_ZhenYing_Setting.GetComponent<UI_ZhenYingSetting>().Init();
                    }
                }));

                break;


            //阵营实力
            case 10002306:

                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //活动按钮显示
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().SelfGuanZhi = pro_ComStr_4.str_1;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_ZhenYingSet.GetComponent<UI_ZhenYingSet>().ShowSetting();
                    }
                }));

                break;


            //接受移动数据
            case 20000002:
                //Debug.Log("接受移动时间00000：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                //Debug.Log("接受移动数据"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                Pro_Fight_Move pro_Fight_Move = ProtoBuf.Serializer.Deserialize<Pro_Fight_Move>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    //Debug.Log("接受移动时间11111：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1 != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().pro_Fight_Move = pro_Fight_Move;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().PlayerAllMove();
                        //Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().MoveStatus = true;
                    }

                }));

                break;
           
            //接受自身线程ID
            case 20000003:
                //Debug.Log("发送战区奖励数据");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    Game_PublicClassVar.Get_wwwSet.ServerRoseThreadID = pro_ComStr_1.str_1;
                }));

                break;

            //地图进入玩家线程
            case 20000101:
                //Debug.Log("接受000:2000100120001001200010012000100120001001");
                Pro_Fight_CreatePlayer pro_Fight_CreatePlayer = ProtoBuf.Serializer.Deserialize<Pro_Fight_CreatePlayer>(ms);
                //Debug.Log(pro_Fight_CreatePlayer.PlayerName);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //Debug.Log("接受移动时间11111：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    //Debug.Log("地图来人啦啦啦啦啦");
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        //Debug.Log("地图来人啦啦啦啦啦222222");
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_Add(pro_Fight_CreatePlayer.ThreadID, pro_Fight_CreatePlayer);
                    }
                }));

                break;

            //地图离开玩家线程
            case 20000102:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //Debug.Log("检测到玩家离开地图线程");
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //创建角色
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        //Debug.Log("检测到玩家离开地图线程22222");
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_Delete(pro_ComStr_1.str_1);
                    }

                }));
                break;


            //接受移动数据
            case 20000103:
                //Debug.Log("接受移动时间00000：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                //Debug.Log("接受移动数据"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                MapThread_PlayerMoveList mapThread_PlayerMoveList = ProtoBuf.Serializer.Deserialize<MapThread_PlayerMoveList>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().mapThread_PlayerMoveList = mapThread_PlayerMoveList;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerAllMove();
                    }
                }));

                break;


            //地图进入玩家线程
            case 20000201:
                //Debug.Log("接受000:2000100120001001200010012000100120001001");
                pro_Fight_CreatePlayer = ProtoBuf.Serializer.Deserialize<Pro_Fight_CreatePlayer>(ms);
                //Debug.Log(pro_Fight_CreatePlayer.PlayerName);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //Debug.Log("接受移动时间11111：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                    //Debug.Log("地图来人啦啦啦啦啦");
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        //Debug.Log("地图来人啦啦啦啦啦222222");
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_Add(pro_Fight_CreatePlayer.ThreadID, pro_Fight_CreatePlayer);
                    }
                }));

                break;

            //地图离开玩家线程
            case 20000202:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //Debug.Log("检测到玩家离开地图线程");
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //创建角色
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        //Debug.Log("检测到玩家离开地图线程22222");
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_Delete(pro_ComStr_1.str_1);
                    }
                }));
                break;


            //接受移动数据
            case 20000203:

                mapThread_PlayerMoveList = ProtoBuf.Serializer.Deserialize<MapThread_PlayerMoveList>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().mapThread_PlayerMoveList = mapThread_PlayerMoveList;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerAllMove();
                    }
                }));

                break;

            //接受移动数据
            case 20000204:
                //Debug.Log("接受移动时间00000：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                //Debug.Log("接受移动数据"+ DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                MapThread_PlayerDataChange mapThread_PlayerDataChange = ProtoBuf.Serializer.Deserialize<MapThread_PlayerDataChange>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    /*
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                    {
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().mapThread_PlayerMoveList = mapThread_PlayerMoveList;
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerAllMove();
                    }
                    */

                    if (mapThread_PlayerDataChange.PlayerID != "" && mapThread_PlayerDataChange.PlayerID != null && mapThread_PlayerDataChange.PlayerID != "0")
                    {
                        if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj != null)
                        {
                            if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerSet.ContainsKey(mapThread_PlayerDataChange.PlayerID))
                            {

                                GameObject palyerObj = Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().PlayerSet[mapThread_PlayerDataChange.PlayerID];
                                if (palyerObj != null)
                                {
                                    switch (mapThread_PlayerDataChange.ChangType)
                                    {

                                        //修改武器
                                        case "1":
                                            Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_ShowWeaponEquipID(palyerObj, mapThread_PlayerDataChange.ChangValue);
                                            break;

                                        //修改宠物
                                        case "2":
                                            string[] petDataList = mapThread_PlayerDataChange.ChangValue.Split(',');            //0 ID  1 名称 2 等级
                                            if (petDataList.Length >= 3)
                                            {
                                                Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_ShowPetID(palyerObj, petDataList[0], petDataList[1], petDataList[2]);
                                            }
                                            break;

                                        //称号
                                        case "3":
                                            Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().PlayerSetObj.GetComponent<GamePlayerSet>().Player_ShowChengHao(palyerObj.GetComponent<Player_Status>().Obj_HpNameUI, mapThread_PlayerDataChange.ChangValue);
                                            break;

                                    }
                                }
                            }
                        }
                    }
                }));

                break;


            //地图进入玩家线程（HuoDong_1）
            case 20001001:
                //Debug.Log("接受000:2000100120001001200010012000100120001001");
                pro_Fight_CreatePlayer = ProtoBuf.Serializer.Deserialize<Pro_Fight_CreatePlayer>(ms);
                //Debug.Log(pro_Fight_CreatePlayer.PlayerName);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //创建角色
                    Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().Player_Add(pro_Fight_CreatePlayer.ThreadID, pro_Fight_CreatePlayer);
                }));

                break;


            //地图离开玩家线程（HuoDong_1）
            case 20001002:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //创建角色
                    Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().Player_Delete(pro_ComStr_1.str_1);
                }));
                break;


            //是否可以进入地图
            case 20001004:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //1表示可以进入
                if (pro_ComStr_1.str_1 == "1") {
                    //跨线程调用(跨线程调用的值不能传出)
                    MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                    {
                        //进入活动地图
                        Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_MapEnterUI_HuoDong_1.GetComponent<UI_HuoDongEnterList>().EnterScence();
                    }));
                }

                break;



            //创建宝箱
            case 20001011:
                Pro_HuoDong1_CreateChest pro_HuoDong1_CreateChest = ProtoBuf.Serializer.Deserialize<Pro_HuoDong1_CreateChest>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //创建宝箱
                    Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().Chest_Create(pro_HuoDong1_CreateChest);
                }));
                break;

            //开启宝箱
            case 20001012:
                //Debug.Log("200010122000101220001012");
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {

                    //触发掉落
                    Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().Chest_Open(pro_ComStr_1.str_1);

                }));
                break;

            //其他宝箱被开启(接受到后将这个宝箱关闭)
            case 20001013:
                pro_ComStr_1 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_1>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    //触发掉落
                    Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>().Chest_Delte(pro_ComStr_1.str_1);

                }));
                break;

            //重置场景内的宝箱和玩家
            case 20001015:

                Pro_HuoDong1_UpdateData pro_HuoDong1_UpdateData = ProtoBuf.Serializer.Deserialize<Pro_HuoDong1_UpdateData>(ms);

                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1 != null) {

                        ServerHuoDong_1 huodong_1 = Game_PublicClassVar.Get_gameLinkServerObj.GetComponent<GameServerObj>().Obj_Map_HuoDong_1.GetComponent<ServerHuoDong_1>();

                        //清空多余的宝箱
                        for (int i = 0; i < huodong_1.ChestSet.Count; i++)
                        {
                            string chestID = huodong_1.ChestSet.ElementAt(i).Key;
                            if (huodong_1.ChestSet.ContainsKey(chestID) == false)
                            {
                                huodong_1.Chest_Delte(chestID);
                            }
                        }

                        //创建没有的宝箱
                        for (int i = 0; i < pro_HuoDong1_UpdateData.HuoDong_1_ChestList.Count; i++)
                        {
                            string chestID = pro_HuoDong1_UpdateData.HuoDong_1_ChestList.ElementAt(i).Key;
                            pro_HuoDong1_CreateChest = pro_HuoDong1_UpdateData.HuoDong_1_ChestList[chestID];

                            if (huodong_1.ChestSet.ContainsKey(pro_HuoDong1_CreateChest.ChestID.ToString())==false) {
                                //表示宝箱不存在,生成宝箱
                                huodong_1.Chest_Create(pro_HuoDong1_CreateChest);
                            }
                        }

                        //清空地图内的所有玩家
                        huodong_1.Player_DeleteAll();

                        //暂时不处理玩家,可能会有假人,但是真人一定会存在,以为移动的时候如果数据没有会重新生成
                        //检测玩家
                        for (int i = 0; i < pro_HuoDong1_UpdateData.HuoDong_1_PlayList.Count; i++) {
                            string playerID = pro_HuoDong1_UpdateData.HuoDong_1_PlayList.ElementAt(i).Key;
                            pro_Fight_CreatePlayer = pro_HuoDong1_UpdateData.HuoDong_1_PlayList[playerID];

                            if (huodong_1.PlayerSet.ContainsKey(pro_Fight_CreatePlayer.ThreadID) == false)
                            {
                                //表示玩家不存在,生存玩家数据
                                huodong_1.Player_Add(playerID, pro_Fight_CreatePlayer);
                            }
                        }
                            
                    }

                }));

                break;

            case 30000001:
                pro_ComStr_3 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_3>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    if (pro_ComStr_3.str_1 == "0")
                    {
                        Debug.Log("认证成功！");

                        Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().YanZhengShenFenIDReturn(true);

                    }
                    else
                    {
                        Debug.Log("认证失败！");
                        Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng.GetComponent<FangChenMi>().YanZhengShenFenIDReturn(false);
                    }
                }));
                break;

            //查询订单
            case 30000011:
                pro_ComStr_4 = ProtoBuf.Serializer.Deserialize<Pro_ComStr_4>(ms);
                //跨线程调用(跨线程调用的值不能传出)
                MainTaskProcessor.AppendOneAction(new TaskUnit(() =>
                {
                    GameObject uiCommonHint = (GameObject)MonoBehaviour.Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICommonHint);
                    //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("comhint_11");
                    uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("亲爱的玩家,您上次有未领取的订单,请进入充值界面自动领取!", Game_PublicClassVar.function_UI.GoToRmbStore, null);
                    //uiCommonHint.GetComponent<UI_CommonHint>().Btn_CommonHint("是否前往充值界面？", GoToRmbStore, null);
                    uiCommonHint.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                    uiCommonHint.transform.localPosition = Vector3.zero;
                    uiCommonHint.transform.localScale = new Vector3(1, 1, 1);
                    return;
                }));
                break;

        }
    }

    public void SendToServerBuf(ObscuredInt xuhaoID, object sendStr)
    {
        //未连接网络不发送任何数据
        if (!ServerLinkStatus) {
            Debug.Log("未连接网络,发送数据失败!");
            return;
        }

        if (ServerLinkStopSendStatus) {
            return;
        }

        //Debug.Log("xuhaoID = " + xuhaoID);
        MemoryStream ms = new MemoryStream();

        switch (xuhaoID)
        {
            //客户端发送的第一个连接请求
            case 10000001:
                Pro_ComStr_1 comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //心跳包
            case 10000002:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //延迟
            case 10000003:
                YanChiStatus = true;
                YanChiValue = 0;
                //Debug.Log("100000003时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                break;

            //加速时间效验
            case 10000004:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //当前时间戳效验
            case 10000005:
                Pro_ComStr_4 proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送到客户端关闭请求
            case 10000099:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;


            //********************初始化请求************************
            //请求时间戳
            case 10000101:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //获取当前最新安卓版本号
            case 10000102:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //获取当前最新IOS号
            case 10000103:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求QQ群号
            case 10000104:
                //Debug.Log("申请显示QQ群");
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求服务器信息
            case 10000105:
                //Debug.Log("请求服务器信息");
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求公告
            case 10000106:
                //Debug.Log("请求公告请求公告请求公告");
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //上传文件数据
            case 10000108:
                proComStr_4 = new Pro_ComStr_4() { str_1 = Game_PublicClassVar.Get_wwwSet.gameCount, str_2 = Game_PublicClassVar.Get_wwwSet.fileSize, str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送本地版本
            case 10000109:
                Pro_ComStr_2 proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

            //发送错误日志
            case 10000110:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送Obs错误日志
            case 100001101:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送重置次数
            case 10000111:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送错误日志
            case 10000112:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //请求是否发送获取道具信息
            case 10000113:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求是否发送本地错误数据
            case 10000114:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //服务器消息揭露
            case 10000115:
                Pro_GameMsg proGameMsg = (Pro_GameMsg)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_GameMsg>(ms, proGameMsg);
                break;

            //服务器记录消息
            case 10000116:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求分享数据
            case 10000117:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求更新数据
            case 10000118:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求更新服务器列表
            case 10000120:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求更新最新服务器数据
            case 10000121:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送身份信息
            case 10000131:
                Pro_PlayerYanZheng proPlayerYanZheng = (Pro_PlayerYanZheng)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PlayerYanZheng>(ms, proPlayerYanZheng);
                break;

            //发送身份信息
            case 10000132:
                Pro_ComStr_3 proComStr_3 = (Pro_ComStr_3)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //发送身份信息
            case 10000133:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送找回身份信息
            case 10000141:
                proPlayerYanZheng = (Pro_PlayerYanZheng)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PlayerYanZheng>(ms, proPlayerYanZheng);
                break;

            //发送请求
            case 10000152:
                proComStr_4 = new Pro_ComStr_4();
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送效验失败数据
            case 10000153:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //******************通用请求****************************
            //获取玩家名称和信息
            case 10001003:
                string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string roseLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string shopDes = "账号:" + zhanghaoID + "," + "名称:" + roseName + "," + "等级:" + roseLv + "," + "金币:" + GoldNum + "," + "钻石:" + RMB + "," + "付费:" + RMBPayValue;
                //Debug.Log("获取玩家名称和信息:" + sendStr);
                comStr_1 = new Pro_ComStr_1() { str_1 = shopDes };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;


            //发送角色全部信息
            case 10001006:

                //读取RoseData
                string Rose_ID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_Lv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_Name = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_ZhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_RMB = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMB", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_RMBPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RMBPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_MaoXianJiaExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaoXianJiaExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_YueKa = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKa", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_YueKaDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YueKaDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_TiLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TiLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_MaxTiLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MaxTiLi", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_SP = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SP", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_RoseExpNow = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_RoseHpNow = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseHpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_RoseOccupation = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseOccupation", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_StoryStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StoryStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_NowMapName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_NowMapPositionName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowMapPositionName", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_OffGameTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("OffGameTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_AchievementTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_AchievementTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AchievementTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_CompleteTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CompleteTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_LearnSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_EquipSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_EquipSuitSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSuitSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_LearnSkillIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_EquipSkillIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipSkillIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_SceneItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SceneItemID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_PVEChapter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PVEChapter", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_SpecialEventTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_SpecialEventOpenTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventOpenTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

				string Rose_HuoLi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HuoLi",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_SkillSP = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillSP",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ShiMenTaskNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenTaskNum",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ShiMenNextTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiMenNextTaskID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_LearnTianFuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianFuID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_LearnTianSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LearnTianSkillID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_LingPaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LingPaiRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_TanSuoListID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("TanSuoListID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_Proficiency_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_1",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_Proficiency_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_2",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_Proficiency_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Proficiency_3",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ProficiencyID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_1",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ProficiencyID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_2",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ProficiencyID_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_3",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_QiTianDengLuStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiTianDengLuStatus",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_QiTianDengLu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiTianDengLu",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_LvLiBao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("LvLiBao",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_StroeHouseMaxNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("StroeHouseMaxNum",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_PetAddMaxNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetAddMaxNum",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_DaMiJingLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLv",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_DaMiJingLvKillTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingLvKillTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_DaMiJingRewardLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJingRewardLv",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ShiLianTaskNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskNum",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ShiLianTaskStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianTaskStatus",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ShiLianNextTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShiLianNextTaskID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_ChengHaoIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengHaoIDSet",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_NowChengHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowChengHaoID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_JingLingIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingIDSet",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
				string Rose_JingLingEquipID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JingLingEquipID",  "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_XiLianDaShiShuLian = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiShuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_XiLianDaShiIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("XiLianDaShiIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_PetXiuLian = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetXiuLian", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_FuBen_ShangHaiHightID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiHightID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_FuBen_ShangHaiRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_FuBen_ShangHaiLvRewardSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_ShangHaiLvRewardSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_JueXingExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_JueXingJiHuoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("JueXingJiHuoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_YanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_NowYanSeID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowYanSeID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_NowYanSeHairID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowYanSeHairID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                string Rose_ShengXiaoSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShengXiaoSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_ZhenYing = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string Rose_ProficiencyID_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ProficiencyID_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");

                string Rose_ZhuLingIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhuLingIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                

                //读取RoseConfig
                string Rose_MainUITaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainUITaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_RoseEquipHideNumID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseEquipHideNumID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_DengLuReward = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuReward", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_DengLuDayStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DengLuDayStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_ZhiChiZuoZheID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhiChiZuoZheID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_DeathMonsterID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DeathMonsterID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_FirstEnterGame = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FirstEnterGame", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_5 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_6 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_7 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_7", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_8 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_8", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_9 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_9", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string Rose_BeiYong_10 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BeiYong_10", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                //读取RoseDayReward
                string Rose_CountryExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_CountryHonor = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryHonor", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_CountryLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("CountryLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_Day_ExpNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_Day_ExpTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_ExpTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_Day_GoldNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_Day_GoldTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_GoldTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_ChouKaTime_One = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_One", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_ChouKaTime_Ten = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChouKaTime_Ten", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_Day_FuBenNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Day_FuBenNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_FenXiang_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_FenXiang_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_FenXiang_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FenXiang_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_DayTaskID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_DayTaskValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_DayTaskHuoYueValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskHuoYueValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_DayTaskCommonHuoYueRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayTaskCommonHuoYueRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                string Rose_MeiRiLiBao = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiLiBao", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
				string Rose_MeiRiCangBaoNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
				string Rose_MeiRiCangBaoTrueChestNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MeiRiCangBaoTrueChestNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
				string Rose_FuBen_1_DayNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FuBen_1_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
				string Rose_DaMiJing_DayNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DaMiJing_DayNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                string Rose_DayPayValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DayPayValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_QianDaoNum_Com = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_QianDaoNum_Pay = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_QianDaoNum_Com_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Com_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_QianDaoNum_Pay_Day = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QianDaoNum_Pay_Day", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
                string Rose_FengYinCeng = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("FengYinCeng", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");

                Dictionary<string, string>  Rose_AllHideIDList = new Dictionary<string, string>();

                //读取背包
                int bagItemNum = Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum;
                string[] Rose_BagItemIDList = new string[bagItemNum];
                string[] Rose_BagItemNumList = new string[bagItemNum];
                string[] Rose_BagItemHideList = new string[bagItemNum];
				string[] Rose_BagItemParList = new string[bagItemNum];
				string[] Rose_BagGemIDList = new string[bagItemNum];
				string[] Rose_BagGemHoleList = new string[bagItemNum];
				
                for (int i = 1; i <= Game_PublicClassVar.Get_game_PositionVar.RoseBagMaxNum; i++) {
                    string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID",i.ToString(), "RoseBag");
                    string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseBag");
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseBag");
					string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID",i.ToString(), "RoseBag");
					string gemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseBag");
					string gemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseBag");

                    Rose_BagItemIDList[i - 1] = itemID;
                    Rose_BagItemNumList[i - 1] = itemNum;
                    Rose_BagItemHideList[i - 1] = hideID;
					Rose_BagItemParList[i - 1] = itemPar;
					Rose_BagGemIDList[i - 1] = gemID;
					Rose_BagGemHoleList[i - 1] = gemHole;

                    //记录隐藏属性
                    if (hideID != "" && hideID != "0") {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(hideID) == false)
                        {
                            Rose_AllHideIDList.Add(hideID, hideStr);
                        }
                        else {
                            Debug.Log("隐藏属性具有相同键值,请检查...  id:" + hideID);
                        }
                    }
                }

                //读取仓库
                string[] Rose_StoreHouseItemIDList = new string[210];
				string[] Rose_StoreHouseItemNumList = new string[210];
				string[] Rose_StoreHouseItemHideList = new string[210];
				string[] Rose_StoreHouseItemParList = new string[210];
				string[] Rose_StoreHouseGemIDList = new string[210];
				string[] Rose_StoreHouseGemHoleList = new string[210];


			    for (int i = 1; i <= 210; i++)
                {
                    string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemID", "ID", i.ToString(), "RoseStoreHouse");
                    string itemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemNum", "ID", i.ToString(), "RoseStoreHouse");
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseStoreHouse");
					string itemPar = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemPar", "ID", i.ToString(), "RoseStoreHouse");
					string gemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseStoreHouse");
					string gemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseStoreHouse");
					
                    Rose_StoreHouseItemIDList[i - 1] = itemID;
                    Rose_StoreHouseItemNumList[i - 1] = itemNum;
                    Rose_StoreHouseItemHideList[i - 1] = hideID;
					Rose_StoreHouseItemParList [i - 1] = itemPar;
					Rose_StoreHouseGemIDList [i - 1] = gemID;
					Rose_StoreHouseGemHoleList [i - 1] = gemHole;
                    //记录隐藏属性
                    if (hideID != "" && hideID != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                        //Debug.Log("hideID = "+ hideID + ";hideStr = " + hideStr);
                        if (Rose_AllHideIDList.ContainsKey(hideID)==false) {
                            Rose_AllHideIDList.Add(hideID, hideStr);
                        }
                    }
                }

                string[] Rose_EquipItemIDList = new string[13];
                string[] Rose_EquipHideIDList = new string[13];
				string[] Rose_EquipGemHoleList = new string[13];
				string[] Rose_EquipGemIDList = new string[13];
				string[] Rose_EquipQiangHuaIDList = new string[13];
                //读取角色自身
                for (int i = 1; i <= 13; i++)
                {
                    string itemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipItemID", "ID", i.ToString(), "RoseEquip");
                    string hideID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HideID", "ID", i.ToString(), "RoseEquip");
					string gemHole = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemHole", "ID", i.ToString(), "RoseEquip");
					string gemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GemID", "ID", i.ToString(), "RoseEquip");
					string qiangHuaID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("QiangHuaID", "ID", i.ToString(), "RoseEquip");
					
                    Rose_EquipItemIDList[i - 1] = itemID;
                    Rose_EquipHideIDList[i - 1] = hideID;
					Rose_EquipGemHoleList [i - 1] = gemHole;
					Rose_EquipGemIDList [i - 1] = gemID;
					Rose_EquipQiangHuaIDList [i - 1] = qiangHuaID;
					
                    //记录隐藏属性
                    if (hideID != "" && hideID != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", hideID, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(hideID)==false) {
                            Rose_AllHideIDList.Add(hideID, hideStr);
                        }
                    }
                }
				
				//读取宠物
				string[] Rose_PetStatusList = new string[12];
				string[] Rose_PetIDList = new string[12];
				string[] Rose_PetLvList = new string[12];
				string[] Rose_PetNowHpList = new string[12];
				string[] Rose_PetMaxHpList = new string[12];
				string[] Rose_PetExpList = new string[12];
				string[] Rose_PetNameList = new string[12];
				string[] Rose_IfBabyList = new string[12];
				string[] Rose_AddPropretyNumList = new string[12];
				string[] Rose_AddPropretyValueList = new string[12];
				string[] Rose_PetPingFenList = new string[12];
				string[] Rose_ZiZhi_HpList = new string[12];
				string[] Rose_ZiZhi_ActList = new string[12];
				string[] Rose_ZiZhi_MageActList = new string[12];
				string[] Rose_ZiZhi_DefList = new string[12];
				string[] Rose_ZiZhi_AdfList = new string[12];
				string[] Rose_ZiZhi_ActSpeedList = new string[12];
				string[] Rose_ZiZhi_ChengZhangList = new string[12];
				string[] Rose_PetSkillList = new string[12];

                string[] Rose_PetEquipID_1 = new string[12];
                string[] Rose_PetEquipIDHide_1 = new string[12];
                string[] Rose_PetEquipID_2 = new string[12];
                string[] Rose_PetEquipIDHide_2 = new string[12];
                string[] Rose_PetEquipID_3 = new string[12];
                string[] Rose_PetEquipIDHide_3 = new string[12];
                string[] Rose_PetEquipID_4 = new string[12];
                string[] Rose_PetEquipIDHide_4 = new string[12];

                for (int i = 1; i<=12; i++){
					
					//string iD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ID", "ID", i.ToString(), "RosePet");
					string petStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetStatus", "ID", i.ToString(), "RosePet");
					string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", i.ToString(), "RosePet");
					string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", i.ToString(), "RosePet");
					string petExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetExp", "ID", i.ToString(), "RosePet");
					string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", i.ToString(), "RosePet");
					string ifBaby = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBaby", "ID", i.ToString(), "RosePet");
					string addPropretyNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyNum", "ID", i.ToString(), "RosePet");
					string addPropretyValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("AddPropretyValue", "ID",i.ToString(), "RosePet");
					string petPingFen = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetPingFen", "ID", i.ToString(), "RosePet");
					string ziZhi_Hp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Hp", "ID", i.ToString(), "RosePet");
					string ziZhi_Act = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Act", "ID", i.ToString(), "RosePet");
					string ziZhi_MageAct = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_MageAct", "ID", i.ToString(), "RosePet");
					string ziZhi_Def = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Def", "ID", i.ToString(), "RosePet");
					string ziZhi_Adf = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_Adf", "ID", i.ToString(), "RosePet");
					string ziZhi_ActSpeed = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ActSpeed", "ID", i.ToString(), "RosePet");
					string ziZhi_ChengZhang = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZiZhi_ChengZhang", "ID", i.ToString(), "RosePet");
					string petSkill = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetSkill", "ID", i.ToString(), "RosePet");
					string petNowHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetNowHp", "ID", i.ToString(), "RosePet");
					string petMaxHp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetMaxHp", "ID", i.ToString(), "RosePet");


                    string petEquipID_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_1", "ID", i.ToString(), "RosePet");
                    string petEquipIDHide_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_1", "ID", i.ToString(), "RosePet");
                    string petEquipID_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_2", "ID", i.ToString(), "RosePet");
                    string petEquipIDHide_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_2", "ID", i.ToString(), "RosePet");
                    string petEquipID_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_3", "ID", i.ToString(), "RosePet");
                    string petEquipIDHide_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_3", "ID", i.ToString(), "RosePet");
                    string petEquipID_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipID_4", "ID", i.ToString(), "RosePet");
                    string petEquipIDHide_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EquipIDHide_4", "ID", i.ToString(), "RosePet");


                    Rose_PetStatusList[i - 1] = petStatus;
					Rose_PetIDList[i - 1] = petID;
					Rose_PetLvList[i - 1] = petLv;
					Rose_PetNowHpList[i - 1] = petNowHp;
					Rose_PetMaxHpList[i - 1] = petMaxHp;
					Rose_PetExpList[i - 1] = petExp;
					Rose_PetNameList[i - 1] = petName;
					Rose_IfBabyList[i - 1] = ifBaby;
					Rose_AddPropretyNumList[i - 1] = addPropretyNum;
					Rose_AddPropretyValueList[i - 1] = addPropretyValue;
					Rose_PetPingFenList[i - 1] = petPingFen;
					Rose_ZiZhi_HpList[i - 1] = ziZhi_Hp;
					Rose_ZiZhi_ActList[i - 1] = ziZhi_Act;
					Rose_ZiZhi_MageActList[i - 1] = ziZhi_MageAct;
					Rose_ZiZhi_DefList[i - 1] = ziZhi_Def;
					Rose_ZiZhi_AdfList[i - 1] = ziZhi_Adf;
					Rose_ZiZhi_ActSpeedList[i - 1] = ziZhi_ActSpeed;
					Rose_ZiZhi_ChengZhangList[i - 1] = ziZhi_ChengZhang;
					Rose_PetSkillList[i - 1] = petSkill;

                    Rose_PetEquipID_1[i - 1] = petEquipID_1;
                    Rose_PetEquipIDHide_1[i - 1] = petEquipIDHide_1;
                    Rose_PetEquipID_2[i - 1] = petEquipID_2;
                    Rose_PetEquipIDHide_2[i - 1] = petEquipIDHide_2;
                    Rose_PetEquipID_3[i - 1] = petEquipID_3;
                    Rose_PetEquipIDHide_3[i - 1] = petEquipIDHide_3;
                    Rose_PetEquipID_4[i - 1] = petEquipID_4;
                    Rose_PetEquipIDHide_4[i - 1] = petEquipIDHide_4;

                    //记录宠物隐藏属性
                    if (petEquipIDHide_1 != "" && petEquipIDHide_1 != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", petEquipIDHide_1, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(petEquipIDHide_1) == false)
                        {
                            Rose_AllHideIDList.Add(petEquipIDHide_1, hideStr);
                        }
                        else
                        {
                            Debug.Log("隐藏属性具有相同键值,请检查...  id:" + petEquipIDHide_1);
                        }
                    }

                    if (petEquipIDHide_2 != "" && petEquipIDHide_2 != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", petEquipIDHide_2, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(petEquipIDHide_2) == false)
                        {
                            Rose_AllHideIDList.Add(petEquipIDHide_2, hideStr);
                        }
                        else
                        {
                            Debug.Log("隐藏属性具有相同键值,请检查...  id:" + petEquipIDHide_2);
                        }
                    }

                    if (petEquipIDHide_3 != "" && petEquipIDHide_3 != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", petEquipIDHide_3, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(petEquipIDHide_3) == false)
                        {
                            Rose_AllHideIDList.Add(petEquipIDHide_3, hideStr);
                        }
                        else
                        {
                            Debug.Log("隐藏属性具有相同键值,请检查...  id:" + petEquipIDHide_3);
                        }
                    }


                    if (petEquipIDHide_4 != "" && petEquipIDHide_4 != "0")
                    {
                        string hideStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PrepeotyList", "ID", petEquipIDHide_4, "RoseEquipHideProperty");
                        if (Rose_AllHideIDList.ContainsKey(petEquipIDHide_4) == false)
                        {
                            Rose_AllHideIDList.Add(petEquipIDHide_4, hideStr);
                        }
                        else
                        {
                            Debug.Log("隐藏属性具有相同键值,请检查...  id:" + petEquipIDHide_4);
                        }
                    }

                }

				
				

				//读取成就
				string Rose_ComChengJiuID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ComChengJiuRewardID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ComChengJiuRewardID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_5 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_5", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_6 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_6", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_7 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_7", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_101 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_101", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_102 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_102", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_103 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_103", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_104 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_104", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_105 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_105", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_106 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_106", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_107 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_107", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_108 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_108", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_109 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_109", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_110 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_110", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_111 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_111", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_201 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_201", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_202 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_202", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_203 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_203", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_204 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_204", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_205 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_205", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_206 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_206", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_207 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_207", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_208 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_208", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_209 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_209", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_210 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_210", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_211 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_211", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
				string Rose_ChengJiu_212 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_212", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");

                string Rose_ChengJiu_220 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_220", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_221 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_221", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_222 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_222", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_223 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_223", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_224 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_224", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_225 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_225", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_226 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_226", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_227 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_227", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_228 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_228", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_229 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_229", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");
                string Rose_ChengJiu_250 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ChengJiu_250", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseChengJiu");

                //读取牧场相关
                string Rose_PastureLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_PastureExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_PastureGold = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureGold", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiPiFuSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiZiZhi_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiZiZhi_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiZiZhi_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiZiZhi_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiZiZhi_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiLv_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiExp_1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_1", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiLv_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiExp_2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_2", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiLv_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiExp_3 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_3", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiLv_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiLv_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQi_NengLiExp_4 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQi_NengLiExp_4", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_NowZuoQiShowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiBaoShiDu = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoShiDu", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiBaoStatus = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiBaoStatus", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiJieDuanLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiJieDuanLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_ZuoQiXianJiExp = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiXianJiExp", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
                string Rose_PastureDuiHuanID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureDuiHuanID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

                string[] sendList = (string[])(sendStr);
                //额外附加
                //Debug.Log("sendList[0].ToString() = " + sendList[0].ToString());

                Pro_PlayAllData playData = new Pro_PlayAllData()
                {
                    //RoseData相关
                    Write_ID = Rose_ID,
                    Write_Lv = Rose_Lv,
                    Write_Name = Rose_Name,
                    Write_ZhangHaoID = Rose_ZhangHaoID,
                    Write_GoldNum = Rose_GoldNum,
                    Write_RMB = Rose_RMB,
                    Write_RMBPayValue = Rose_RMBPayValue,
                    Write_MaoXianJiaExp = Rose_MaoXianJiaExp,
                    Write_YueKa = Rose_YueKa,
                    Write_YueKaDayStatus = Rose_YueKaDayStatus,
                    Write_TiLi = Rose_TiLi,
                    Write_MaxTiLi = Rose_MaxTiLi,
                    Write_SP = Rose_SP,
                    Write_RoseExpNow = Rose_RoseExpNow,
                    Write_RoseHpNow = Rose_RoseHpNow,
                    Write_RoseOccupation = Rose_RoseOccupation,
                    Write_StoryStatus = Rose_StoryStatus,
                    Write_NowMapName = Rose_NowMapName,
                    Write_NowMapPositionName = Rose_NowMapPositionName,
                    Write_OffGameTime = Rose_OffGameTime,
                    Write_AchievementTaskID = Rose_AchievementTaskID,
                    Write_AchievementTaskValue = Rose_AchievementTaskValue,
                    Write_CompleteTaskID = Rose_CompleteTaskID,
                    Write_LearnSkillID = Rose_LearnSkillID,
                    Write_LingPaiRewardID = Rose_LingPaiRewardID,
                    Write_EquipSkillID = Rose_EquipSkillID,
                    Write_EquipSuitSkillID = Rose_EquipSuitSkillID,
                    Write_LearnSkillIDSet = Rose_LearnSkillIDSet,
                    Write_EquipSkillIDSet = Rose_EquipSkillIDSet,
                    Write_SceneItemID = Rose_SceneItemID,
                    Write_PVEChapter = Rose_PVEChapter,
                    Write_SpecialEventTime = Rose_SpecialEventTime,
                    Write_SpecialEventOpenTime = Rose_SpecialEventOpenTime,
                    Write_HuoLi = Rose_HuoLi,
                    Write_SkillSP = Rose_SkillSP,
                    Write_ShiMenTaskNum = Rose_ShiMenTaskNum,
                    Write_ShiMenNextTaskID = Rose_ShiMenNextTaskID,
                    Write_LearnTianFuID = Rose_LearnTianFuID,
                    Write_LearnTianSkillID = Rose_LearnTianSkillID,
                    Write_TanSuoListID = Rose_TanSuoListID,
                    Write_Proficiency_1 = Rose_Proficiency_1,
                    Write_Proficiency_2 = Rose_Proficiency_2,
                    Write_Proficiency_3 = Rose_Proficiency_3,
                    Write_ProficiencyID_1 = Rose_ProficiencyID_1,
                    Write_ProficiencyID_2 = Rose_ProficiencyID_2,
                    Write_ProficiencyID_3 = Rose_ProficiencyID_3,
                    Write_QiTianDengLuStatus = Rose_QiTianDengLuStatus,
                    Write_QiTianDengLu = Rose_QiTianDengLu,
                    Write_LvLiBao = Rose_LvLiBao,
                    Write_StroeHouseMaxNum = Rose_StroeHouseMaxNum,
                    Write_PetAddMaxNum = Rose_PetAddMaxNum,
                    Write_DaMiJingLv = Rose_DaMiJingLv,
                    Write_DaMiJingLvKillTime = Rose_DaMiJingLvKillTime,
                    Write_DaMiJingRewardLv = Rose_DaMiJingRewardLv,
                    Write_ShiLianTaskNum = Rose_ShiLianTaskStatus,
                    Write_ShiLianNextTaskID = Rose_ShiLianNextTaskID,
                    Write_ChengHaoIDSet = Rose_ChengHaoIDSet,
                    Write_NowChengHaoID = Rose_NowChengHaoID,
                    Write_JingLingIDSet = Rose_JingLingIDSet,
                    Write_JingLingEquipID = Rose_JingLingEquipID,
                    Write_XiLianDaShiShuLian = Rose_XiLianDaShiShuLian,
                    Write_XiLianDaShiIDSet = Rose_XiLianDaShiIDSet,
                    Write_PetXiuLian = Rose_PetXiuLian,

                    Write_FuBen_ShangHaiHightID = Rose_FuBen_ShangHaiHightID,
                    Write_FuBen_ShangHaiRewardID = Rose_FuBen_ShangHaiRewardID,
                    Write_FuBen_ShangHaiLvRewardSet = Rose_FuBen_ShangHaiLvRewardSet,
                    Write_JueXingExp = Rose_JueXingExp,
                    Write_JueXingJiHuoID = Rose_JueXingJiHuoID,
                    Write_YanSeIDSet = Rose_YanSeIDSet,
                    Write_NowYanSeID = Rose_NowYanSeID,
                    Write_NowYanSeHairID = Rose_NowYanSeHairID,
                    Write_ShengXiaoSet = Rose_ShengXiaoSet,
                    Write_ZhenYing = Rose_ZhenYing,
                    Write_ProficiencyID_4 = Rose_ProficiencyID_4,
                    Write_ZhuLingIDSet = Rose_ZhuLingIDSet,

                    //RoseConfig相关
                    Write_MainUITaskID = Rose_MainUITaskID,
                    Write_RoseEquipHideNumID = Rose_RoseEquipHideNumID,
                    Write_DengLuReward = Rose_DengLuReward,
                    Write_DengLuDayStatus = Rose_DengLuDayStatus,
                    Write_ZhiChiZuoZheID = Rose_ZhiChiZuoZheID,
                    Write_DeathMonsterID = Rose_DeathMonsterID,
                    Write_FirstEnterGame = Rose_FirstEnterGame,
                    Write_BeiYong_2 = Rose_BeiYong_2,
                    Write_BeiYong_5 = Rose_BeiYong_5,
                    Write_BeiYong_6 = Rose_BeiYong_6,
                    Write_BeiYong_7 = Rose_BeiYong_7,
                    Write_BeiYong_8 = Rose_BeiYong_8,
                    Write_BeiYong_9 = Rose_BeiYong_9,

                    //RoseDayReward相关
                    Write_CountryExp = Rose_CountryExp,
                    Write_CountryHonor = Rose_CountryHonor,
                    Write_CountryLv = Rose_CountryLv,
                    Write_Day_ExpNum = Rose_Day_ExpNum,
                    Write_Day_ExpTime = Rose_Day_ExpTime,
                    Write_Day_GoldNum = Rose_Day_GoldNum,
                    Write_Day_GoldTime = Rose_Day_GoldTime,
                    Write_ChouKaTime_One = Rose_ChouKaTime_One,
                    Write_ChouKaTime_Ten = Rose_ChouKaTime_Ten,
                    Write_Day_FuBenNum = Rose_Day_FuBenNum,
                    Write_FenXiang_1 = Rose_FenXiang_1,
                    Write_FenXiang_3 = Rose_FenXiang_3,
                    Write_FenXiang_2 = Rose_FenXiang_2,
                    Write_DayTaskID = Rose_DayTaskID,
                    Write_DayTaskValue = Rose_DayTaskValue,
                    Write_DayTaskHuoYueValue = Rose_DayTaskHuoYueValue,
                    Write_DayTaskCommonHuoYueRewardID = Rose_DayTaskCommonHuoYueRewardID,

                    Write_MeiRiLiBao = Rose_MeiRiLiBao,
                    Write_MeiRiCangBaoNum = Rose_MeiRiCangBaoNum,
                    Write_MeiRiCangBaoTrueChestNum = Rose_MeiRiCangBaoTrueChestNum,
                    Write_FuBen_1_DayNum = Rose_FuBen_1_DayNum,
                    Write_DaMiJing_DayNum = Rose_DaMiJing_DayNum,

                    Write_DayPayValue = Rose_DayPayValue,
                    Write_QianDaoNum_Com = Rose_QianDaoNum_Com,
                    Write_QianDaoNum_Pay = Rose_QianDaoNum_Pay,
                    Write_QianDaoNum_Com_Day = Rose_QianDaoNum_Com_Day,
                    Write_QianDaoNum_Pay_Day = Rose_QianDaoNum_Pay_Day,
                    Write_FengYinCeng = Rose_FengYinCeng,

                    //RoseBag背包
                    Write_BagItemIDList = Rose_BagItemIDList,
                    Write_BagItemNumList = Rose_BagItemNumList,
                    Write_BagItemHideList = Rose_BagItemHideList,
                    Write_BagItemParList = Rose_BagItemParList,
                    Write_BagGemIDList = Rose_BagGemIDList,
                    Write_BagGemHoleList = Rose_BagGemHoleList,

                    //RoseHouseStore内容************
                    Write_StoreHouseItemIDList = Rose_StoreHouseItemIDList,
                    Write_StoreHouseItemNumList = Rose_StoreHouseItemNumList,
                    Write_StoreHouseItemHideList = Rose_StoreHouseItemHideList,
                    Write_StoreHouseItemParList = Rose_StoreHouseItemParList,
                    Write_StoreHouseGemIDList = Rose_StoreHouseGemIDList,
                    Write_StoreHouseGemHoleList = Rose_StoreHouseGemHoleList,

                    //RoseEquip内容************
                    Write_EquipItemIDList = Rose_EquipItemIDList,
                    Write_EquipHideIDList = Rose_EquipHideIDList,
                    Write_EquipGemHoleList = Rose_EquipGemHoleList,
                    Write_EquipGemIDList = Rose_EquipGemIDList,
                    Write_EquipQiangHuaIDList = Rose_EquipQiangHuaIDList,

                    //*********RoseEquipHideProperty内容************
                    Write_AllHideIDList = Rose_AllHideIDList,

                    //额外的信息
                    Write_ZhangHaoMiMa = sendList[0].ToString(),
                    Write_UpdateType = sendList[1].ToString(),
                    Write_BeiYong_10 = Rose_BeiYong_10,

                    //宠物相关
                    Write_PetStatus_List = Rose_PetStatusList,
                    Write_PetID_List = Rose_PetIDList,
                    Write_PetLv_List = Rose_PetLvList,
                    Write_PetNowHp_List = Rose_PetNowHpList,
                    Write_PetMaxHp_List = Rose_PetMaxHpList,
                    Write_PetExp_List = Rose_PetExpList,
                    Write_PetName_List = Rose_PetNameList,
                    Write_IfBaby_List = Rose_IfBabyList,
                    Write_AddPropretyNum_List = Rose_AddPropretyNumList,
                    Write_AddPropretyValue_List = Rose_AddPropretyValueList,
                    Write_PetPingFen_List = Rose_PetPingFenList,
                    Write_ZiZhi_Hp_List = Rose_ZiZhi_HpList,
                    Write_ZiZhi_Act_List = Rose_ZiZhi_ActList,
                    Write_ZiZhi_MageAct_List = Rose_ZiZhi_MageActList,
                    Write_ZiZhi_Def_List = Rose_ZiZhi_DefList,
                    Write_ZiZhi_Adf_List = Rose_ZiZhi_AdfList,
                    Write_ZiZhi_ActSpeed_List = Rose_ZiZhi_ActSpeedList,
                    Write_ZiZhi_ChengZhang_List = Rose_ZiZhi_ChengZhangList,
                    Write_PetSkill_List = Rose_PetSkillList,

                    Write_PetEquipID_1 = Rose_PetEquipID_1,
                    Write_PetEquipIDHide_1 = Rose_PetEquipIDHide_1,
                    Write_PetEquipID_2 = Rose_PetEquipID_2,
                    Write_PetEquipIDHide_2 = Rose_PetEquipIDHide_2,
                    Write_PetEquipID_3 = Rose_PetEquipID_3,
                    Write_PetEquipIDHide_3 = Rose_PetEquipIDHide_3,
                    Write_PetEquipID_4 = Rose_PetEquipID_4,
                    Write_PetEquipIDHide_4 = Rose_PetEquipIDHide_4,

                    //成就相关
                    Write_ChengJiuID = Rose_ComChengJiuID,
                    Write_ChengJiuRewardID = Rose_ComChengJiuRewardID,
                    Write_ChengJiu_1 = Rose_ChengJiu_1,
                    Write_ChengJiu_2 = Rose_ChengJiu_2,
                    Write_ChengJiu_3 = Rose_ChengJiu_3,
                    Write_ChengJiu_4 = Rose_ChengJiu_4,
                    Write_ChengJiu_5 = Rose_ChengJiu_5,
                    Write_ChengJiu_6 = Rose_ChengJiu_6,
                    Write_ChengJiu_7 = Rose_ChengJiu_7,
                    Write_ChengJiu_101 = Rose_ChengJiu_101,
                    Write_ChengJiu_102 = Rose_ChengJiu_102,
                    Write_ChengJiu_103 = Rose_ChengJiu_103,
                    Write_ChengJiu_104 = Rose_ChengJiu_104,
                    Write_ChengJiu_105 = Rose_ChengJiu_105,
                    Write_ChengJiu_106 = Rose_ChengJiu_106,
                    Write_ChengJiu_107 = Rose_ChengJiu_107,
                    Write_ChengJiu_108 = Rose_ChengJiu_108,
                    Write_ChengJiu_109 = Rose_ChengJiu_109,
                    Write_ChengJiu_110 = Rose_ChengJiu_110,
                    Write_ChengJiu_111 = Rose_ChengJiu_111,
                    Write_ChengJiu_201 = Rose_ChengJiu_201,
                    Write_ChengJiu_202 = Rose_ChengJiu_202,
                    Write_ChengJiu_203 = Rose_ChengJiu_203,
                    Write_ChengJiu_204 = Rose_ChengJiu_204,
                    Write_ChengJiu_205 = Rose_ChengJiu_205,
                    Write_ChengJiu_206 = Rose_ChengJiu_206,
                    Write_ChengJiu_207 = Rose_ChengJiu_207,
                    Write_ChengJiu_208 = Rose_ChengJiu_208,
                    Write_ChengJiu_209 = Rose_ChengJiu_209,
                    Write_ChengJiu_210 = Rose_ChengJiu_210,
                    Write_ChengJiu_211 = Rose_ChengJiu_211,
                    Write_ChengJiu_212 = Rose_ChengJiu_212,
                    Write_ChengJiu_220 = Rose_ChengJiu_220,
                    Write_ChengJiu_221 = Rose_ChengJiu_221,
                    Write_ChengJiu_222 = Rose_ChengJiu_222,
                    Write_ChengJiu_223 = Rose_ChengJiu_223,
                    Write_ChengJiu_224 = Rose_ChengJiu_224,
                    Write_ChengJiu_225 = Rose_ChengJiu_225,
                    Write_ChengJiu_226 = Rose_ChengJiu_226,
                    Write_ChengJiu_227 = Rose_ChengJiu_227,
                    Write_ChengJiu_228 = Rose_ChengJiu_228,
                    Write_ChengJiu_229 = Rose_ChengJiu_229,
                    Write_ChengJiu_250 = Rose_ChengJiu_250,


                    //牧场相关
                    Write_PastureLv = Rose_PastureLv,
                    Write_PastureExp = Rose_PastureExp,
                    Write_PastureGold = Rose_PastureGold,
                    Write_ZuoQiLv = Rose_ZuoQiLv,
                    Write_ZuoQiPiFuSet = Rose_ZuoQiPiFuSet,
                    Write_ZuoQiZiZhi_1 = Rose_ZuoQiZiZhi_1,
                    Write_ZuoQiZiZhi_2 = Rose_ZuoQiZiZhi_2,
                    Write_ZuoQiZiZhi_3 = Rose_ZuoQiZiZhi_3,
                    Write_ZuoQiZiZhi_4 = Rose_ZuoQiZiZhi_4,
                    Write_ZuoQi_NengLiLv_1 = Rose_ZuoQi_NengLiLv_1,
                    Write_ZuoQi_NengLiExp_1 = Rose_ZuoQi_NengLiExp_1,
                    Write_ZuoQi_NengLiLv_2 = Rose_ZuoQi_NengLiLv_2,
                    Write_ZuoQi_NengLiExp_2 = Rose_ZuoQi_NengLiExp_2,
                    Write_ZuoQi_NengLiLv_3 = Rose_ZuoQi_NengLiLv_3,
                    Write_ZuoQi_NengLiExp_3 = Rose_ZuoQi_NengLiExp_3,
                    Write_ZuoQi_NengLiLv_4 = Rose_ZuoQi_NengLiLv_4,
                    Write_ZuoQi_NengLiExp_4 = Rose_ZuoQi_NengLiExp_4,
                    Write_NowZuoQiShowID = Rose_NowZuoQiShowID,
                    Write_ZuoQiBaoShiDu = Rose_ZuoQiBaoShiDu,
                    Write_ZuoQiBaoStatus = Rose_ZuoQiBaoStatus,
                    Write_ZuoQiJieDuanLv = Rose_ZuoQiJieDuanLv,
                    Write_ZuoQiXianJiExp = Rose_ZuoQiXianJiExp,
                    Write_PastureDuiHuanID = Rose_PastureDuiHuanID,

                    //其他
                    Player_UpdateType = sendList[3]

                };

                //单独写入IP
                if (GetIpStatus) {
                    playData.Write_PlayerIP = GetIpStr;
                }

                ProtoBuf.Serializer.Serialize<Pro_PlayAllData>(ms, playData);

                break;

            //获取角色全部信息
            case 10001007:
                string[] saveList = (string[])(sendStr);
                proComStr_3 = new Pro_ComStr_3() { str_1 = saveList[0], str_2 = saveList[1], str_3 = saveList[2] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //获取角色全部信息
            case 100010071:
                string[] nowsaveList = (string[])(sendStr);
                proComStr_3 = new Pro_ComStr_3() { str_1 = nowsaveList[0], str_2 = nowsaveList[1], str_3 = nowsaveList[2] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //查询当前是否可以下载
            case 100010072:
                Pro_FindRoseListDataDown ProFindRoseListDataDown = (Pro_FindRoseListDataDown)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_FindRoseListDataDown>(ms, ProFindRoseListDataDown);
                break;

            //存储账号ID
            case 10001008:
                string saveZhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhanghaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送修改账号密码
            case 10001009:
                string[] zhanghaoList = (string[])sendStr;
                proComStr_3 = new Pro_ComStr_3() { str_1 = zhanghaoList[0], str_2 = zhanghaoList[1], str_3 = zhanghaoList[2] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //验证账号的唯一设备表示ID
            case 10001010:
                string zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhanghaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                string shebeiID = SystemInfo.deviceUniqueIdentifier;
                proComStr_3 = new Pro_ComStr_3() { str_1 = zhangHaoID, str_2 = shebeiID, str_3 = "预留" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //接受服务端读取的单个数据
            case 10001011:
                string[] readDataStrList = (string[])sendStr;
                proComStr_3 = new Pro_ComStr_3() { str_1 = readDataStrList[0], str_2 = readDataStrList[1], str_3 = readDataStrList[2]};
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //向发送服务端存储的单个数据
            case 10001012:
                string[] sendDataStrList = (string[])sendStr;
                proComStr_4 = new Pro_ComStr_4() { str_1 = sendDataStrList[0], str_2 = sendDataStrList[1], str_3 = sendDataStrList[2], str_4 = sendDataStrList[3] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //清除账号下载记录
            case 10001013:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //清除账号下载记录
            case 10001014:
                Pro_ComStr_3 comStr_3 = (Pro_ComStr_3)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, comStr_3);
                break;

            //服务器消息揭露
            case 10001016:
                proGameMsg = (Pro_GameMsg)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_GameMsg>(ms, proGameMsg);
                break;

            //服务器记录消息
            case 10001017:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //接受客户端消息,发送给给全服广播(废弃,客户端不存在不加密的广播)
            /*
            case 10001018:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;
            */
            //接受客户端消息,发送给给全服广播
            case 10001019:
                string strJiaMi = Game_PublicClassVar.Get_xmlScript.Encrypt(sendStr.ToString());
                comStr_1 = new Pro_ComStr_1() { str_1 = strJiaMi };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            case 100010191:
                Pro_ComStr_4 comStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, comStr_4);
                break;

            //发送给服务端设备ID
            case 10001020:
                shebeiID = SystemInfo.deviceUniqueIdentifier;
                zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                proComStr_4 = new Pro_ComStr_4() { str_1 = shebeiID,str_2 = zhanghaoID, str_3 = "",str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送给当前语言类型
            case 10001021:
                shebeiID = SystemInfo.deviceUniqueIdentifier;
                string typeStr = PlayerPrefs.GetString("GameLanguageType");
                proComStr_4 = new Pro_ComStr_4() { str_1 = typeStr, str_2 = "", str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送给当前设备属性
            case 10001022:
                Pro_SheBeiData pro_SheBeiData = (Pro_SheBeiData)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_SheBeiData>(ms, pro_SheBeiData);
                break;

            //发送给当前语言类型
            case 10001023:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送给当前语言类型
            case 10001024:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送给当前语言类型
            case 10001025:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;
            case 10001027:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;
            case 10001028:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;
            //向服务端发送自己的战斗力
            case 10001029:
                readDataStrList = (string[])sendStr;
                proComStr_4 = new Pro_ComStr_4() { str_1 = readDataStrList[0], str_2 = readDataStrList[1], str_3 = readDataStrList[2], str_4 = readDataStrList[3] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //向服务端发送自己的战斗力
            case 10001030:
                readDataStrList = (string[])sendStr;
                proComStr_4 = new Pro_ComStr_4() { str_1 = readDataStrList[0], str_2 = readDataStrList[1], str_3 = readDataStrList[2], str_4 = readDataStrList[3] };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;
            
            //向服务端发送自己的战斗力
            case 10001031:
                readDataStrList = (string[])sendStr;
                proComStr_4 = new Pro_ComStr_4() { str_1 = readDataStrList[0], str_2 = readDataStrList[1], str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;


            //排行榜名次
            case 10001032:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //排行榜开区时间
            case 10001033:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //排行榜名次（职业）
            case 10001034:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //排行榜名次（大秘境）
            case 10001035:
                readDataStrList = (string[])sendStr;
                proComStr_4 = new Pro_ComStr_4() { str_1 = readDataStrList[0], str_2 = readDataStrList[1], str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //请求谁是天下第一
            case 10001036:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求显示排行榜
            case 10001037:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送邮件列表请求
            case 10001040:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送邮件列表请求_动态
            case 100010401:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送领取邮件请求
            case 10001041:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送领取邮件请求
            case 10001042:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送领取邮件请求
            case 10001043:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送领取邮件请求验证完成
            case 10001044:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送自身的交易信息
            case 10001050:
                Pro_PayList pro_PayList = (Pro_PayList)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PayList>(ms, pro_PayList);
                Debug.Log("发送支付信息成功");
                break;

            //发送序列号
            case 10001055:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送数据
            case 10001060:
                zhangHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhanghaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                proComStr_2 = new Pro_ComStr_2() { str_1 = zhangHaoID, str_2 = sendStr.ToString()};
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

            //发送数据
            case 10001061:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //验证战区奖励按钮状态
            case 10001062:
                proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

			//发送拍卖行信息
			case 10001071:
				proComStr_2 = (Pro_ComStr_2)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
				break;

			//发送拍卖行信息
			case 10001072:
				Pro_PaiMai_Buy pro_PaiMai_Buy = (Pro_PaiMai_Buy)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_PaiMai_Buy>(ms, pro_PaiMai_Buy);
				break;

			//发送拍卖行信息
			case 10001073:
				Pro_PaiMai_Sell pro_PaiMai_Sell = (Pro_PaiMai_Sell)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_PaiMai_Sell>(ms, pro_PaiMai_Sell);
				break;

			//拍卖行玩家出售列表
			case 10001074:
				comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
				ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
				break;

			//拍卖行信息
			case 10001075:
				proComStr_2 = (Pro_ComStr_2)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
				break;

			//获取兑换金额
			case 10001076:
				comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
				ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
				break;
                
             //获取出售金币数据
            case 10001077:
                Pro_PaiMaiSellData pro_PaiMaiSellData = (Pro_PaiMaiSellData)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PaiMaiSellData>(ms, pro_PaiMaiSellData);
                break;

            //拍卖行信息
            case 10001078:
                proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

            //发送拍卖行信息（加密）
            case 10001079:
                pro_PaiMai_Sell = (Pro_PaiMai_Sell)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PaiMai_Sell>(ms, pro_PaiMai_Sell);
                break;

            //发送拍卖行信息（加密）
            case 10001081:
                proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;
            //发送拍卖行信息（加密）
            case 10001082:
                pro_PaiMai_Buy = (Pro_PaiMai_Buy)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PaiMai_Buy>(ms, pro_PaiMai_Buy);
                break;

            //发送拍卖行信息（加密）
            case 10001083:
                pro_PaiMai_Sell = (Pro_PaiMai_Sell)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PaiMai_Sell>(ms, pro_PaiMai_Sell);
                break;

            //拍卖行玩家出售列表（加密）
            case 10001084:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //拍卖行信息（加密）
            case 10001085:
                proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

            //获取兑换金额（加密）
            case 10001086:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //获取出售金币数据（加密）
            case 10001087:
                pro_PaiMaiSellData = (Pro_PaiMaiSellData)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PaiMaiSellData>(ms, pro_PaiMaiSellData);
                break;


            //玩家显示装备（加密）
            case 10001091:
				comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
				ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
				break;

			//玩家显示宠物
			case 10001092:
				comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
				ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
				break;

            //玩家显示宠物
            case 10001093:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家显示首杀数据
            case 10001101:
				proComStr_2 = (Pro_ComStr_2)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                Debug.Log("数据发送！");
				break;

			//玩家添加首杀数据
			case 10001102:
				proComStr_3 = (Pro_ComStr_3)(sendStr);
				ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
				break;

            //检测玩家数据
            case 10001201:
                proComStr_3 = (Pro_ComStr_3)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_3>(ms, proComStr_3);
                break;

            //请求最高世界等级
            case 10001301:
                comStr_1 = new Pro_ComStr_1() { str_1 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;


            //玩家抢红包数据
            case 10001401:
                comStr_1 = new Pro_ComStr_1() { str_1 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家请求抢红包其他玩家数据
            case 10001402:
                comStr_1 = new Pro_ComStr_1() { str_1 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家添加或更新宠物战队信息
            case 10001501:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //玩家获取当前挑战信息
            case 10001502:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家获取当前挑战信息
            case 10001503:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //获取文件验证码
            case 10001600:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //获取文件验证码
            case 10001601:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //文件验证码录入完毕
            case 10001602:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送数据记录
            case 10001701:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送数据记录
            case 10001702:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //请求当前宝箱活动是否开启
            case 10001801:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //狩猎开始
            case 10001900:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家发送猎杀数据
            case 10001901:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //获取狩猎数据
            case 10001902:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //狩猎奖励
            case 10001903:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //获取交互数据
            case 10002001:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //发送牧场矿数据
            case 10002011:
                //发送服务器记录
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //玩家获取当前矿场宠物数据
            case 10002012:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //掠夺数据
            case 10002013:
                //发送服务器记录
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //伤害记录
            case 10002101:

                string nowRoseActMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseActMaxValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
                string nowPetActMaxValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetActMaxValue", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                //发送服务器记录
                proComStr_4 = new Pro_ComStr_4 { str_1 = nowRoseActMaxValue+";"+ nowPetActMaxValue, str_2 = "", str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //Obs强制退出次数
            case 10002102:

                string ObsExitNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ObsExitNum", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");

                //发送服务器记录
                proComStr_4 = new Pro_ComStr_4 { str_1 = ObsExitNumStr, str_2 = "", str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //是否开启魔塔活动
            case 10002201:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求阵营数据
            case 10002301:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //请求阵营数据
            case 10002302:
                proComStr_4 = new Pro_ComStr_4 { str_1 = sendStr.ToString(), str_2 = "", str_3 = "", str_4 = "" };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //阵营选择官职
            case 10002303:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //阵营玩家列表
            case 10002304:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //获取玩家真实Ip
            case 10002401:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //发送线程变量
            case 20000001:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //玩家移动(接受玩家移动数据)
            case 20000002:
                Pro_Fight_MoveList proFight_Move = (Pro_Fight_MoveList)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_Fight_MoveList>(ms, proFight_Move);
                //Debug.Log("测试移动时间开始：" + System.DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                break;

            //玩家进入活动地图
            case 20000101:
                MapThread_EnterMapData mapThread_EnterMapData = (MapThread_EnterMapData)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_EnterMapData>(ms, mapThread_EnterMapData);
                break;

            //玩家退出活动地图
            case 20000102:
                mapThread_EnterMapData = (MapThread_EnterMapData)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_EnterMapData>(ms, mapThread_EnterMapData);
                break;

            //发送移动消息
            case 20000103:
                MapThread_PlayerMove mapThread_PlayerMove = (MapThread_PlayerMove)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_PlayerMove>(ms, mapThread_PlayerMove);
                break;

            //玩家进入活动地图
            case 20000201:
                mapThread_EnterMapData = (MapThread_EnterMapData)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_EnterMapData>(ms, mapThread_EnterMapData);
                break;

            //玩家退出活动地图
            case 20000202:
                mapThread_EnterMapData = (MapThread_EnterMapData)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_EnterMapData>(ms, mapThread_EnterMapData);
                break;

            //发送移动消息
            case 20000203:
                mapThread_PlayerMove = (MapThread_PlayerMove)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_PlayerMove>(ms, mapThread_PlayerMove);
                break;

            //发送改变自身状态的消息
            case 20000204:
                MapThread_PlayerDataChange mapThread_PlayerDataChange = (MapThread_PlayerDataChange)(sendStr);
                ProtoBuf.Serializer.Serialize<MapThread_PlayerDataChange>(ms, mapThread_PlayerDataChange);
                break;
                

            //地图添加玩家线程（HuoDong_1）
            case 20001001:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //地图离开玩家线程（HuoDong_1）
            case 20001002:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //重新请求地图内玩家信息
            case 20001003:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //是否可以进入地图
            case 20001004:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;


            //开启宝箱
            case 20001012:
                proComStr_2 = (Pro_ComStr_2)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_2>(ms, proComStr_2);
                break;

            //重新请求地图内宝箱和玩家数据
            case 20001014:
                comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
                ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
                break;

            //实名认证
            case 30000001:
                Pro_PlayerYanZheng comStr_yanzheng =  (Pro_PlayerYanZheng)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_PlayerYanZheng>(ms, comStr_yanzheng);
                break;

            //查询订单
            case 30000011:
                proComStr_4 = (Pro_ComStr_4)(sendStr);
                ProtoBuf.Serializer.Serialize<Pro_ComStr_4>(ms, proComStr_4);
                break;

            //测试
            case 100001:
                NetModel item = new NetModel() { ID = 1, Commit = "He123",Message="TTTTT" };
                ProtoBuf.Serializer.Serialize<NetModel>(ms, item);
                break;

            //测试信息
            case 100002:
                NetModel model = ProtoBuf.Serializer.Deserialize<NetModel>(ms);
                Console.WriteLine("反序列化成功:" + model.Message.ToString());
                break;
        }

        //构建发送的数据
        byte[] result = new byte[ms.Length];
        ms.Position = 0;
        ms.Read(result, 0, result.Length);

        //构建数据包
        byte[] sendByte = new byte[result.Length + 10];

        //Byte[0][1]:包体起始位置
        byte[] send_1 = BitConverter.GetBytes(10);
        send_1.CopyTo(sendByte, 0);
        //Debug.Log(BitConverter.ToInt16(send_1, 0) + "开始发送111：" + sendByte[0] + "," + sendByte[1]);

        //Byte[2][3]:包体总长度
        byte[] send_2 = new byte[4];
        send_2 = BitConverter.GetBytes(result.Length);
        send_2.CopyTo(sendByte, 2);
        //Debug.Log(BitConverter.ToInt16(send_2,0) + "开始发送222：" + sendByte[0] + "," + sendByte[1] + "," + sendByte[2] + "," + sendByte[3]);

        //Byte[4][5][6][7]  序号ID
        byte[] send_3 = new byte[4];
        send_3 = BitConverter.GetBytes(xuhaoID);
        send_3.CopyTo(sendByte, 6);
        //Debug.Log(BitConverter.ToInt32(send_3,0) + "开始发送333：" + sendByte[0] + "," + sendByte[1] + "," + sendByte[2] + "," + sendByte[3] + "," + sendByte[4] + "," + sendByte[5] + "," + sendByte[6] + "," + sendByte[7]);

        //实际内容
        result.CopyTo(sendByte, 10);

        //发送内容
        int sendMin = 0;
        int sendNum = 0;
        int sendSize = sendByte.Length;
        try {
            while (true)
            {
                sendNum = sendNum + 1;
                if (ns != null) {
                    if (sendSize > 1024)
                    {
                        ns.Write(sendByte, sendMin, 1024);
                        sendSize = sendSize - 1024;
                        sendMin = sendMin + 1024;
                    }
                    else
                    {
                        ns.Write(sendByte, sendMin, sendSize);

                        //Debug.Log("100000003时间222：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss fff"));
                        break;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //断线重连后,可能会在写数据时报错,这里为了不让其因为报错退出线程,在此处加入判断
            Debug.Log("EX = " + ex);
        }

        /*
        //粘包发送内容
        if (sendByte.Length > 1024) {
            ns.Write(sendByte, 0, sendByte.Length);
        }
        */
        //ns.Write(sendByte, 0, sendByte.Length);

        //Debug.Log("发送序号:" + xuhaoID + "sendByte = " + sendByte.Length + ";sendNum = " + sendNum);

        /*
        //发送粘包测试
        byte[] sendByteTo = new byte[sendByte.Length*2];
        sendByte.CopyTo(sendByteTo, 0);
        sendByte.CopyTo(sendByteTo, sendByte.Length);
        //发送内容
        ns.Write(sendByteTo, 0, sendByteTo.Length);
        */
    }

    public void ServerToClientBuf(byte[] btyArr)
    {
        int MaxXiaoXiSum = 0;
        //拆包(考虑到粘包的问题,所以用了循环)
        while (true)
        {

            //最多处理10个粘包
            MaxXiaoXiSum = MaxXiaoXiSum + 1;
            if (MaxXiaoXiSum >= 100)
            {
                break;
            }
            try
            {
                //数据起始位置（此处一定是ToInt16,需要和客户端对应,ToInt32需要4个字符）
                byte[] btyArr_1 = btyArr.Skip(0).Take(2).ToArray();
                int btyInt_1 = BitConverter.ToInt16(btyArr_1, 0);

                //包体大小
                byte[] btyArr_2 = btyArr.Skip(2).Take(4).ToArray();
                int btyInt_2 = BitConverter.ToInt32(btyArr_2, 0);
                //byte[] send_3 = new byte[4];
                //btyArr_2.CopyTo(send_3, 0);
                //int btyInt_2 = BitConverter.ToInt32(send_3, 0);

                //ID编码
                byte[] btyArr_3 = btyArr.Skip(6).Take(4).ToArray();
                int btyInt_3 = BitConverter.ToInt32(btyArr_3, 0);
                //反序列化对象
                byte[] bufByte = btyArr.Skip(btyInt_1).Take(btyInt_2).ToArray();

                //发送协议号,处理事件
                //Debug.Log("协议号:" + btyInt_3);
                ServerToDoThing(btyInt_3, bufByte);

                //截取下一个字节
                btyArr = btyArr.Skip(btyInt_1 + btyInt_2).Take(btyArr.Length - btyInt_2).ToArray();
                if (btyArr.Length >= 10)
                {
                    //判定是否需要执行
                    if (btyArr[0] != btyArr_1[0] || btyArr[1] != btyArr_1[1])
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            catch (Exception ex)
            {
                Debug.Log("数据错误!ex = " + ex);
                break;
            }
        }
    }


    private void sendDataTest() {
        Debug.Log("测试发送数据");
        //发送信息
        byte[] bt = Encoding.Default.GetBytes("100001我想要个朋友222！");
        ns.Write(bt, 0, bt.Length);
        Debug.Log("测试发送数据成功");
    }

    //获取数据包体长度
    private int GetBaoTiSize(byte[] btyArr, int qishiNum)
    {
        try
        {
            if (btyArr.Length >= 1024)
            {
                //Debug.Log("1111");
            }

            byte[] btyArr_1 = btyArr.Skip(qishiNum).Take(2).ToArray();
            int btyInt_1 = BitConverter.ToInt16(btyArr_1, 0);
            //Console.WriteLine("btyInt_1 = " + btyInt_1);

            //包体大小
            byte[] btyArr_2 = btyArr.Skip(qishiNum + 2).Take(4).ToArray();
            int btyInt_2 = BitConverter.ToInt32(btyArr_2, 0);

            //byte[] send_3 = new byte[4];
            //btyArr_2.CopyTo(send_3, 0);
            //int btyInt_2 = BitConverter.ToInt32(send_3, 0);
            //Console.WriteLine("btyInt_2 = " + btyInt_2);
            int readDataSize = btyInt_1 + btyInt_2;
            return readDataSize;
        }
        catch (Exception ex)
        {
            Console.WriteLine("读取数据字节出错！ex = " + ex + "qishiNum = " + qishiNum + ";btyArr = " + btyArr.Length + ";ThreadID =" + Thread.CurrentThread.Name);
        }
        return -1;
    }

    //关闭服务器
    public void CloseServer() {

        //biaDebug.Log("关闭链接服务器及相关数据流");

        //关闭数据流
        if (ns != null)
        {
            ns.Close();
        }

        //关闭tcp
        if (tc != null)
        {
            tc.Close();
        }

        //关闭线程
        if (mainThread != null && mainThread.IsAlive)
        {
            mainThread.Abort();
        }
    
    }

    public void CheckAndroid() {

        string zhanghaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhangHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        //发送是否越狱
        Pro_ComStr_4 com_4 = new Pro_ComStr_4();
        com_4.str_1 = Game_PublicClassVar.Get_wwwSet.IfRootStatus.ToString();
        com_4.str_2 = zhanghaoID;
        SendToServerBuf(10001027, com_4);

        //检测
        com_4 = new Pro_ComStr_4();
        com_4.str_1 = Game_PublicClassVar.Get_wwwSet.CheckApkNameStr;
        com_4.str_2 = zhanghaoID;
        SendToServerBuf(10001028, com_4);

        Game_PublicClassVar.Get_getSignature.JianCeNum = 0;

        if (Game_PublicClassVar.Get_wwwSet.IfRootStatus.ToString().Contains("1"))
        {
            //PlayerPrefs.SetString("RootStatus", "1");
            Game_PublicClassVar.Get_wwwSet.GameRootStatus = true;
        }
        else {
            //PlayerPrefs.SetString("RootStatus", "0");
            Game_PublicClassVar.Get_wwwSet.GameRootStatus = false;
        }
    }


    private string GetIP()
    {

        NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
        foreach (NetworkInterface adater in adapters)
        {
            if (adater.Supports(NetworkInterfaceComponent.IPv4))
            {
                UnicastIPAddressInformationCollection UniCast = adater.GetIPProperties().UnicastAddresses;
                if (UniCast.Count > 0)
                {
                    foreach (UnicastIPAddressInformation uni in UniCast)
                    {
                        if (uni.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            //Debug.Log(uni.Address.ToString());
                            return uni.Address.ToString();
                        }
                    }
                }
            }
        }
        return null;
    }

    //获取本地IP
    public static string GetLocalIP()
    {
        try
        {
            IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress item in IpEntry.AddressList)
            {
                //AddressFamily.InterNetwork  ipv4
                //AddressFamily.InterNetworkV6 ipv6
                if (item.AddressFamily == AddressFamily.InterNetwork)
                {
                    return item.ToString();
                }
            }
            return "";
        }
        catch { return ""; }
    }

    //获取公网IP
    public void GetWANIp(Action<string> finish)
    {
        string url = "http://icanhazip.com/";

        StartCoroutine(GetWANIp(url, finish));
    }

    public IEnumerator GetWANIp(string url, Action<string> finish)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            if (finish != null) finish(uwr.downloadHandler.text);
        }
    }

}
