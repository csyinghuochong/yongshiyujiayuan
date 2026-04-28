using UnityEngine;
using System.Collections;
using System.Data;
using System.Xml;
using System.IO;
using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Security.Cryptography;

//本表为缓存Xml数据的表,在游戏初始化的时候将全部表缓存
public class RobotTest : MonoBehaviour
{

	private TcpClient tc;
	private string ipAddress;
	private int port;
	private NetworkStream ns;
	private bool ServerLinkStatus = false;
	private bool ServerLinkStopSendStatus = false;

	// Use this for initialization
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	public void Init(string address, int port)
	{
		this.ipAddress = address;
		this.port = port;

		IPAddress[] serveIp = Dns.GetHostAddresses(ipAddress);
		IPEndPoint ipend = new IPEndPoint(serveIp[0], port);
		tc = new TcpClient();
		tc.NoDelay = true;
		tc.Connect(ipend);

		//创建数据流
		ns = tc.GetStream();
		ServerLinkStatus = true;
		ServerLinkStopSendStatus = false;



	}

	public void SendToServerBuf(int xuhaoID, object sendStr)
	{
		if (!ServerLinkStatus)
			return;

		if (ServerLinkStopSendStatus)
			return;

		NetworkStream ms =tc.GetStream();

		switch (xuhaoID)
		{
			//客户端发送的第一个连接请求
			case 10000001:
				Pro_ComStr_1 comStr_1 = new Pro_ComStr_1() { str_1 = sendStr.ToString() };
				ProtoBuf.Serializer.Serialize<Pro_ComStr_1>(ms, comStr_1);
				break;
			//心跳包
			case 10000002:
				break;
		}
		NetModel item = new NetModel()
		{
			ID = 1
		};
		ProtoBuf.Serializer.Serialize<NetModel>(ms, item);

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
		try
		{
			while (true)
			{
				sendNum = sendNum + 1;
				if (ns != null)
				{
					if (sendSize > 1024)
					{
						ns.Write(sendByte, sendMin, 1024);
						sendSize = sendSize - 1024;
						sendMin = sendMin + 1024;
					}
					else
					{
						ns.Write(sendByte, sendMin, sendSize);
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
	}


	public void CloseClient()
	{ 
	}

}
