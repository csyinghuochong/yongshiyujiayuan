using System;
using ProtoBuf;
using System.IO;
using UnityEngine;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;

public class ProBuf
{
    public ProBuf()
	{
	}

    public void chushi() { 
        /*
        //创建序列化
        NetModel item = new NetModel() { ID = 1, Commit = "He", Message = "GOGOGO" };
        //序列化对象
        byte[] temp = Serialize(item);
        Console.WriteLine("temp的序列化长度为:" + temp.Length);
        //反序列化对象
        NetModel result = Deserialize(temp);
        Console.WriteLine("接受到消息:" + result.Message);
        */
    }

    //将消息二进制
    public static byte[] Serialize(NetModel model)
    {
        try
        {
            //开启流转换二进制
            using (MemoryStream ms = new MemoryStream())
            {
                //工具转换(将传入的类放入到流中)
                ProtoBuf.Serializer.Serialize<NetModel>(ms, model);
                //新建二进制
                byte[] result = new byte[ms.Length];
                //将流的位置设为0
                ms.Position = 0;
                //将流中的内容读取到二进制中
                ms.Read(result, 0, result.Length);
                return result;
            }
        }
        catch (Exception ex) {
            Console.WriteLine("序列化失败:" + ex.ToString());
            return null;
        }
    }

    //将消息反序列化
    public NetModel Deserialize(byte[] msg) {
        Debug.Log("ServerToClientBuf4444444444444444444");
        try
        {
            //将二进制转换成类值
            using (MemoryStream ms = new MemoryStream())
            {

                //写将二进制写入流数据
                ms.Write(msg, 0, msg.Length);
                //设置流起点
                ms.Position = 0;
                //使用工具反序列化,转变成对应类
                NetModel result = new NetModel() { ID = 1, Commit = "He123", Message = "ccccccccc" };
                Debug.Log("result:" + result.Message.ToString());
                result = ProtoBuf.Serializer.Deserialize<NetModel>(ms);
                Debug.Log("反序列化成功:" + result.Message.ToString());
                return result;
            }
        }
        catch (Exception ex) {
            Debug.Log("反序列化失败:" + ex.ToString());
            return null;
        }
    }

}

//新建一个序列化类
[ProtoContract]
public class NetModel
{

    //添加属性,数字表示下标
    [ProtoMember(1)]
    public int ID;
    [ProtoMember(2)]
    public string Commit;
    [ProtoMember(3)]
    public string Message;

}

//新建一个序列化类
[ProtoContract]
public class Pro_ComStr_1
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;
}

//新建一个序列化类
[ProtoContract]
public class Pro_ComStr_2
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;
    [ProtoMember(2)]
    public string str_2;
    [ProtoMember(3)]
    public string str_3;
}

//新建一个序列化类
[ProtoContract]
public class Pro_ComStr_3
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;
    [ProtoMember(2)]
    public string str_2;
    [ProtoMember(3)]
    public string str_3;
    [ProtoMember(4)]
    public string str_4;
}


//新建一个序列化类
[ProtoContract]
public class Pro_ComStr_4
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;
    [ProtoMember(2)]
    public string str_2;
    [ProtoMember(3)]
    public string str_3;
    [ProtoMember(4)]
    public string str_4;
}

//新建一个序列化类
[ProtoContract]
public class Pro_ComInt_1
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public int int_1;
}

//新建一个序列化类
[ProtoContract]
public class Pro_PaiHang
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_PaiHangStrList> PaiHangData = new Dictionary<string, Pro_PaiHangStrList>();
    [ProtoMember(2)]
    public DateTime ShowTimeMin;
    [ProtoMember(3)]
    public DateTime ShowTimeMax;
    [ProtoMember(4)]
    public string ShowServerName;   //服务器名称
    [ProtoMember(5)]
    public string ShowType;         //展示的类型  0：全部  1：战士排行  2：法师排行
    [ProtoMember(6)]
    public string UpdateTime;       //刷新时间
}

//新建一个序列化类
[ProtoContract]
public struct Pro_PaiHangStrList
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;            //装备ID,
    [ProtoMember(2)]
    public string str_2;            //隐藏属性ID
    [ProtoMember(3)]
    public string str_3;            //名称
    [ProtoMember(4)]
    public string str_4;            //等级
    [ProtoMember(5)]
    public string str_5;            //实力值
    [ProtoMember(6)]
    public string str_6;            //职业
	[ProtoMember(7)]
	public string str_7;            //宠物
    [ProtoMember(8)]
    public string NowYanSeID;        //当前颜色ID
    [ProtoMember(9)]
    public string NowNowYanSeHairID;        //当前颜色ID
}


//新建一个序列化类
[ProtoContract]
public class Pro_PaiHang_DaMiJing
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_PaiHangDaMiJingStrList> PaiHangData = new Dictionary<string, Pro_PaiHangDaMiJingStrList>();
    [ProtoMember(2)]
    public DateTime ShowTimeMin;
    [ProtoMember(3)]
    public DateTime ShowTimeMax;
    [ProtoMember(4)]
    public string ShowServerName;   //服务器名称
    [ProtoMember(5)]
    public string ShowType;         //展示的类型  0：全部  1：战士排行  2：法师排行

}

//新建一个序列化类
[ProtoContract]
public struct Pro_PaiHangDaMiJingStrList
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string str_1;            //装备ID
    [ProtoMember(2)]
    public string str_2;            //隐藏属性ID
    [ProtoMember(3)]
    public string str_3;            //名称
    [ProtoMember(4)]
    public string str_4;            //等级
    [ProtoMember(5)]
    public string str_5;            //实力值
    [ProtoMember(6)]
    public string str_6;            //职业
    [ProtoMember(7)]
    public string str_7;            //宠物
    [ProtoMember(8)]
    public string str_8;            //层数
    [ProtoMember(9)]
    public string str_9;            //时间
}

//新建一个序列化类  玩家装备查看
[ProtoContract]
public struct Pro_PlayerEquipDataList
{
	//添加属性,数字表示下标
	[ProtoMember(1)]
	public string str_1;            //装备ID,
	[ProtoMember(2)]
	public string str_2;            //隐藏属性ID
	[ProtoMember(3)]
	public string str_3;            //名称
	[ProtoMember(4)]
	public string str_4;            //等级
	[ProtoMember(5)]
	public string str_5;            //实力值
	[ProtoMember(6)]
	public string str_6;            //职业
    [ProtoMember(7)]
    public string NowYanSeID;        //当前颜色ID
    [ProtoMember(8)]
    public string NowNowYanSeHairID;        //当前颜色ID
}

//新建一个序列化类
[ProtoContract]
public class Pro_Mail
{
	[ProtoMember(1)]
	public Dictionary<string, Pro_MailStrList> MailData = new Dictionary<string, Pro_MailStrList>();
	//public Dictionary<string,string> HideStr = new Dictionary<string,string> ();
}

//新建一个序列化类
[ProtoContract]
public struct Pro_MailStrList
{
	//添加属性,数字表示下标
	[ProtoMember(1)]
	public string str_1;            //邮件名称
	[ProtoMember(2)]
	public string str_2;            //邮件内容
	[ProtoMember(3)]
	public string str_3;            //邮件奖励
    [ProtoMember(4)]
    public string str_4;            //邮件时间
}


//新建一个序列化类
/*
[ProtoContract]
public class Pro_PaiMai
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_PaiMaiStrList> PaiMaiData = new Dictionary<string, Pro_PaiMaiStrList>();
}


//拍卖行道具的结构体
[ProtoContract]
public struct Pro_PaiMaiStrList
{

    [ProtoMember(1)]
    public string id;                  //拍卖道具ID
    [ProtoMember(2)]
    public string serverID;            //服务器ID
    [ProtoMember(3)]
    public string createTime;          //拍卖道具创建时间
    [ProtoMember(4)]
    public string paiMaiType;          //拍卖道具类型
    [ProtoMember(5)]
    public string itemID;              //拍卖道具ID
    [ProtoMember(6)]
    public string itemNum;             //拍卖道具数量
    [ProtoMember(7)]
    public string itemHideStr;         //拍卖道具隐藏属性
    [ProtoMember(8)]
    public string salePrice;           //出售价格
    [ProtoMember(9)]
    public string salePlayID;          //出售者的玩家id

}
*/

[ProtoContract]
public struct Pro_PayList
{
    [ProtoMember(1)]
    public string zhanghaoID;           //玩家账号ID
    [ProtoMember(2)]
    public string pay_PingTai;          //支付平台
    [ProtoMember(3)]
    public string pay_DingDan;          //订单号
    [ProtoMember(4)]
    public string pay_JiaGe;            //价格
    [ProtoMember(5)]
    public string pay_Status;           //支付成功/失败
    [ProtoMember(6)]
    public string pay_Des;              //交易信息
}


//拍卖行购买信息
[ProtoContract]
public class Pro_PaiMai_Buy
{
	[ProtoMember(1)]
	public string ZhangHaoID;            //购买金币值
	[ProtoMember(2)]
	public string PaiMaiItemID;         //拍买道具ID
	[ProtoMember(3)]
	public string PaiMaiItemNum;        //拍买道具数量
	[ProtoMember(4)]
	public string GoldType;             //购买金币类型
	[ProtoMember(5)]
	public string GoldValue;            //购买金币值
}


//拍卖行购买信息
[ProtoContract]
public class Pro_PaiMai_Sell
{
	[ProtoMember(1)]
	public string ZhangHaoID;               //购买金币值
	[ProtoMember(2)]
	public string PaiMaiItemID;             //拍买道具ID
	[ProtoMember(3)]
	public string PaiMaiItemNum;            //拍买道具数量
	[ProtoMember(4)]
	public string GoldType;                 //出售金币类型
	[ProtoMember(5)]
	public string GoldValue;                //出售金币值
    [ProtoMember(6)]
    public string SendTimeStr;              //出售时间
}


//拍卖行购买信息
[ProtoContract]
public class Pro_PaiMai_PlayerSellList
{
	[ProtoMember(1)]
	public Dictionary<string, Pro_PaiMai_PlayerSell> Pro_PaiMai_PlayerSell_List = new Dictionary<string, Pro_PaiMai_PlayerSell>();
}

//拍卖行购买信息
[ProtoContract]
public class Pro_PaiMai_PlayerSell
{
	[ProtoMember(1)]
	public string PaiMaiItemID;         //拍买道具ID
	[ProtoMember(2)]
	public string PaiMaiItemNum;        //拍买道具数量
	[ProtoMember(3)]
	public string GoldType;             //出售金币类型
	[ProtoMember(4)]
	public string GoldValue;            //出售金币值
	[ProtoMember(5)]
	public string SellTime;             //出售时间
	[ProtoMember(6)]
	public string SellID;             	//出售ID
}

//新建一个序列化类
[ProtoContract]
public class Pro_Fight_Move
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_Fight_MoveList> Fight_MoveList = new Dictionary<string, Pro_Fight_MoveList>();
}

[ProtoContract]
//移动坐标
public class Pro_Fight_MoveList
{
    [ProtoMember(1)]
    public int Move_X;
    [ProtoMember(2)]
    public int Move_Y;
    [ProtoMember(3)]
    public int Move_Z;
    [ProtoMember(4)]
    public string MapName;
}

//创建地图角色
[ProtoContract]
public class Pro_Fight_CreatePlayer
{
    [ProtoMember(1)]
    public int Move_X;
    [ProtoMember(2)]
    public int Move_Y;
    [ProtoMember(3)]
    public int Move_Z;
    [ProtoMember(4)]
    public string MapName;
    [ProtoMember(5)]
    public string OccType;
    [ProtoMember(6)]
    public string PlayerName;
    [ProtoMember(7)]
    public string PlayerLv;
    [ProtoMember(8)]
    public string ThreadID;
    [ProtoMember(9)]
    public string ZhangHaoID;
    [ProtoMember(10)]
    public string ChengHaoID;       //称号ID
    [ProtoMember(11)]
    public string PetData;          //宠物信息
    [ProtoMember(12)]
    public string EquipData;        //装备信息
    [ProtoMember(13)]
    public string ZuoQiID;        //坐骑ID
    [ProtoMember(14)]
    public string NowYanSeID;        //当前颜色ID
    [ProtoMember(15)]
    public string NowNowYanSeHairID;        //当前颜色ID
    [ProtoMember(16)]
    public string NowZhenYing;        //当前阵营

}


//活动创建宝箱
[ProtoContract]
public class Pro_HuoDong1_CreateChest
{
    [ProtoMember(1)]
    public int Posi_X;
    [ProtoMember(2)]
    public int Posi_Y;
    [ProtoMember(3)]
    public int Posi_Z;
    [ProtoMember(4)]
    public string ChestType;
    [ProtoMember(5)]
    public int ChestID;
    [ProtoMember(6)]
    public float ChestTime;
}

//新建一个序列化类
[ProtoContract]
public class Pro_HuoDong1_UpdateData
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_HuoDong1_CreateChest> HuoDong_1_ChestList = new Dictionary<string, Pro_HuoDong1_CreateChest>();
    [ProtoMember(2)]
    public Dictionary<string, Pro_Fight_CreatePlayer> HuoDong_1_PlayList = new Dictionary<string, Pro_Fight_CreatePlayer>();
}


//新建一个序列化类
[ProtoContract]
public class Pro_PaiMaiDataList
{
	[ProtoMember(1)]
	public Dictionary<string, Pro_PaiMaiData>PaiMai_DataList = new Dictionary<string, Pro_PaiMaiData>();
}

//新建一个拍卖行类
[ProtoContract]
public class Pro_PaiMaiData{

    [ProtoMember(1)]
    public string PaiMaiID;             //拍买ID
    [ProtoMember(2)]
    public string PaiMaiType;           //拍买类型
    [ProtoMember(3)]
    public string PaiMaiItemID;         //拍买道具ID
    [ProtoMember(4)]
    public string PaiMaiItemNum;        //拍买道具数量
    [ProtoMember(5)]
    public string GoldType;             //购买金币类型
    [ProtoMember(6)]
    public string GoldValue;            //购买金币值
    [ProtoMember(7)]
    public float GoldBianHuaValue;      //购买价格浮动值
    [ProtoMember(8)]
    public string GoldBianHuaValueStr;      //购买价格浮动值
}

//新建一个拍卖行类
[ProtoContract]
public class Pro_PaiMaiSellData
{

    [ProtoMember(1)]
    public string ZhangHaoID;           //账号id
    [ProtoMember(2)]
    public string PaiMaiType;           //拍买类型
    [ProtoMember(3)]
    public string PaiMaiItemID;         //拍买道具ID
    [ProtoMember(4)]
    public string PaiMaiItemNum;        //拍买道具数量
    [ProtoMember(5)]
    public string GoldType;             //购买金币类型
    [ProtoMember(6)]
    public string GoldValue;            //购买金币值
    [ProtoMember(7)]
    public float GoldBianHuaValue;      //购买价格浮动值
    [ProtoMember(8)]
    public int BagSpaceNum;          //背包格子位置
}


//地图玩家数据集合
[ProtoContract]
public class MapThread_Data
{
    [ProtoMember(1)]
    public Dictionary<string, MapThread_PlayerData> mapThread_PlayerData = new Dictionary<string, MapThread_PlayerData>();       //玩家数据集合(玩家唯一ID)
    [ProtoMember(2)]
    public Dictionary<string, MapThread_MonsterData> mapThread_MonsterData = new Dictionary<string, MapThread_MonsterData>();    //怪物集合（怪物唯一ID）
}

[ProtoContract]
public class MapThread_MonsterData
{
    [ProtoMember(1)]
    public string MonsterOnlyID;            //怪物唯一ID,用于识别怪物
    [ProtoMember(2)]
    public string MonsterID;                //怪物ID,用于读取怪物数据
}


//玩家数据发生改变
[ProtoContract]
public class MapThread_PlayerDataChange
{
    [ProtoMember(1)]
    public string ChangType;
    [ProtoMember(2)]
    public string ChangValue;
    [ProtoMember(3)]
    public string MapName;
    [ProtoMember(4)]
    public string PlayerID;
}


//玩家数据汇总
[ProtoContract]
public class MapThread_PlayerData
{
    public string PlayerID;
    public MapThread_PlayerMove mapThread_PlayerMove;         //玩家移动相关数据
    public MapThread_PlayerSkill mapThread_PlayerSkill;       //玩家技能相关数据
}

//新建一个序列化类
[ProtoContract]
public class MapThread_PlayerMoveList
{
    [ProtoMember(1)]
    public Dictionary<string, MapThread_PlayerMove> mapThread_PlayerMove = new Dictionary<string, MapThread_PlayerMove>();
}

//玩家移动坐标
[ProtoContract]
public class MapThread_PlayerMove
{

    [ProtoMember(1)]
    public bool MoveStatus;
    [ProtoMember(2)]
    public int Move_X;
    [ProtoMember(3)]
    public int Move_Y;
    [ProtoMember(4)]
    public int Move_Z;
    [ProtoMember(5)]
    public int MoveDirection;       //方向
    [ProtoMember(6)]
    public string MapName;
}

//新建一个序列化类
[ProtoContract]
public class MapThread_PlayerSkillList
{
    [ProtoMember(1)]
    public Dictionary<string, MapThread_PlayerSkill> mapThread_PlayerSkill = new Dictionary<string, MapThread_PlayerSkill>();
}

//玩家地图使用技能
[ProtoContract]
public class MapThread_PlayerSkill
{
    //移动
    [ProtoMember(1)]
    public string MapName;
    [ProtoMember(2)]
    public int PlayerID;
    [ProtoMember(3)]
    public int PLayer_X;
    [ProtoMember(4)]
    public int PLayer_Y;
    [ProtoMember(5)]
    public int PLayer_Z;
    [ProtoMember(6)]
    public string UseSkillID;
    [ProtoMember(7)]
    public int Skill_X;
    [ProtoMember(8)]
    public int Skill_Y;
    [ProtoMember(9)]
    public int Skill_Z;
    [ProtoMember(10)]
    public string Skill_ID;
}

//进入地图使用技能
[ProtoContract]
public class MapThread_EnterMapData
{
    //移动
    [ProtoMember(1)]
    public string MapName;
    [ProtoMember(2)]
    public string ZhangHaoID;
    [ProtoMember(3)]
    public int Position_X;
    [ProtoMember(4)]
    public int Position_Y;
    [ProtoMember(5)]
    public int Position_Z;
    [ProtoMember(6)]
    public int MoveDirection;       //方向
}


//首杀列表
[ProtoContract]
public class Pro_ShouShaNameList{

	[ProtoMember(1)]
	public string PlayerName_1;
	[ProtoMember(2)]
	public string ShouShaTime_1;
	[ProtoMember(3)]
	public string PlayerName_2;
	[ProtoMember(4)]
	public string ShouShaTime_2;
	[ProtoMember(5)]
	public string PlayerName_3;
	[ProtoMember(6)]
	public string ShouShaTime_3;
    [ProtoMember(7)]
    public string ShouShaZhangHaoID_1;
    [ProtoMember(8)]
    public string ShouShaZhangHaoID_2;
    [ProtoMember(9)]
    public string ShouShaZhangHaoID_3;
}


//新建一个序列化类
[ProtoContract]
public class Pro_GameMsg
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string Des;
    [ProtoMember(2)]
    public string Type;
    [ProtoMember(3)]
    public string TypeSon;
    [ProtoMember(4)]
    public string TargetID;
    [ProtoMember(5)]
    public string TargetNum;
}


//新建一个序列化类
[ProtoContract]
public class Pro_HongBaoListData
{
    [ProtoMember(1)]
    public Dictionary<string, int> HongBaoData = new Dictionary<string, int>();

}


//宠物排行榜相关
[ProtoContract]
public class Pro_RankPetListData
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_PetListData> PetRankData = new Dictionary<string, Pro_PetListData>();
}


//宠物排行榜相关
[ProtoContract]
public class Pro_PetListData
{
    [ProtoMember(1)]
    public string PetPlayerName;
    [ProtoMember(2)]
    public string PetData;
    [ProtoMember(3)]
    public string PetTeamName;
    [ProtoMember(4)]
    public string RankVlaue;
    [ProtoMember(5)]
    public string RankPetID;
    [ProtoMember(6)]
    public string PetXiuLian;
    [ProtoMember(7)]
    public string EuqipHideStr;
}


//活动狩猎
[ProtoContract]
public class HuoDong_ShouLie
{
    [ProtoMember(1)]
    public string HuoDong_ShouLieRankData;      //狩猎排行
    [ProtoMember(2)]
    public string HuoDong_ShouLieRewardData;    //狩猎奖励
    [ProtoMember(3)]
    public int HuoDong_Time;                    //活动时间
    [ProtoMember(4)]
    public string HuoDong_KillNum;              //活动击杀人数
    [ProtoMember(5)]
    public string HuoDong_SelfRankStr;          //获取自身排名
    [ProtoMember(6)]
    public int HuoDong_UpdateTime;              //获取刷新时间
}


//交互数据相关
[ProtoContract]
public class Pro_JiaoHuData
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_JiaoHuListData> JiaoHuData = new Dictionary<string, Pro_JiaoHuListData>();
}

//交互数据
[ProtoContract]
public class Pro_JiaoHuListData
{
    [ProtoMember(1)]
    public string Name;             //玩家名字
    [ProtoMember(2)]
    public string Occ;              //玩家职业
    [ProtoMember(3)]
    public string PastureLv;        //玩家农场等级
}


//服务器列表
[ProtoContract]
public class Pro_ServerListDataSet
{
    [ProtoMember(1)]
    public Dictionary<string, Pro_ServerListData> ProServerListData = new Dictionary<string, Pro_ServerListData>();              //服务器名字

}

[ProtoContract]
public class Pro_ServerListData
{
    [ProtoMember(1)]
    public string ServerName;               //服务器名字
    [ProtoMember(2)]
    public string ServerStatus;             //玩家名字
    [ProtoMember(3)]
    public string ServerTime;               //服务器时间
}


//玩家设备信息
[ProtoContract]
public class Pro_SheBeiData
{
    [ProtoMember(1)]
    public string SheBei_deviceModel;                   //设备型号
    [ProtoMember(2)]
    public string SheBei_deviceName;                    //设备名称
    [ProtoMember(3)]
    public string SheBei_deviceType;                    //设备型号
    [ProtoMember(4)]
    public string SheBei_deviceUniqueIdentifier;        //设备唯一标识符
    [ProtoMember(5)]
    public string SheBei_graphicsDeviceID;              //显卡ID
    [ProtoMember(6)]
    public string SheBei_graphicsDeviceName;            //显卡名称
    [ProtoMember(7)]
    public string SheBei_graphicsDeviceType;            //显卡类型
    [ProtoMember(8)]
    public string SheBei_graphicsDeviceVendor;          //显卡供应商
    [ProtoMember(9)]
    public string SheBei_graphicsDeviceVendorID;        //显卡供应商ID
    [ProtoMember(10)]
    public string SheBei_graphicsDeviceVersion;         //显卡版本号
    [ProtoMember(11)]
    public string SheBei_graphicsMemorySize;            //显存大小（单位：MB）
    [ProtoMember(12)]
    public string SheBei_systemMemorySize;              //系统内存大小（单位：MB）
    [ProtoMember(13)]
    public string SheBei_operatingSystem;               //操作系统
    [ProtoMember(14)]
    public List<string> SheBei_MacDiZhi;                //Mac地址
    [ProtoMember(15)]
    public string SheBei_imei0;                         //IMEI0
    [ProtoMember(16)]
    public string SheBei_imei1;                         //IMEI1
    [ProtoMember(17)]
    public string SheBei_meid;                          //MEID
}


//验证身份证
[ProtoContract]
public class Pro_PlayerYanZheng
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string SheBeiID;
    [ProtoMember(2)]
    public string Name;
    [ProtoMember(3)]
    public string ShenFenID;
}


//玩家数据列表
[ProtoContract]
public class Pro_FindRoseList
{
    [ProtoMember(1)]
    public List<Pro_FindRoseListData> ProFindRoseListData = new List<Pro_FindRoseListData>();
}


//玩家数据列表
[ProtoContract]
public class Pro_FindRoseListData
{
    [ProtoMember(1)]
    public string Name;             //玩家名字
    [ProtoMember(2)]
    public string Occ;              //玩家职业
    [ProtoMember(3)]
    public string ServerName;        //服务器名称
    [ProtoMember(4)]
    public string Lv;               //等级
    [ProtoMember(5)]
    public string ZhangHaoID;        //账号ID
    [ProtoMember(6)]
    public string ZhangHaoPassword;        //账号密码
}


//验证身份证
[ProtoContract]
public class Pro_FindRoseListDataDown
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public string SheBeiID;
    [ProtoMember(2)]
    public string Name;
    [ProtoMember(3)]
    public string ShenFenID;
    [ProtoMember(4)]
    public string ZhangHaoID;
    [ProtoMember(5)]
    public string GameVsion;
}


//阵营数据列表
[ProtoContract]
public class Pro_ZhenYingDataList
{
    [ProtoMember(1)]
    public Dictionary<int,Pro_ZhenYingRoseData> ProZhenYingRoseData_Zheng = new Dictionary<int, Pro_ZhenYingRoseData>();
    [ProtoMember(2)]
    public Dictionary<int, Pro_ZhenYingRoseData> ProZhenYingRoseData_Xie = new Dictionary<int, Pro_ZhenYingRoseData>();
    [ProtoMember(3)]
    public long ZhenYingNum_Zheng;
    [ProtoMember(4)]
    public long ZhenYingNum_Xie;
}

//阵营身份信息
[ProtoContract]
public class Pro_ZhenYingRoseData
{
    //添加属性,数字表示下标
    [ProtoMember(1)]
    public int RankNum;
    [ProtoMember(2)]
    public string RoseName;
    [ProtoMember(3)]
    public string RoseOcc;
    [ProtoMember(4)]
    public int RoseShiLiValue;
    [ProtoMember(5)]
    public string RoseZhangHaoID;
    [ProtoMember(6)]
    public string RoseZhenYingGuanZhi;
}


//阵营数据列表
[ProtoContract]
public class Pro_ZhenYingXuanZeDataList
{
    [ProtoMember(1)]
    public Dictionary<int, Pro_ZhenYingRoseData> ProZhenYingRoseData = new Dictionary<int, Pro_ZhenYingRoseData>();
}


//玩家全部信息序列化
[ProtoContract]
public class Pro_PlayAllData
{

    //*********RoseData内容************
    [ProtoMember(1)]
	public string Write_ID;
	[ProtoMember(2)]
	public string Write_Lv;
	[ProtoMember(3)]
	public string Write_Name;
	[ProtoMember(4)]
	public string Write_ZhangHaoID;
	[ProtoMember(5)]
	public string Write_GoldNum;
	[ProtoMember(6)]
	public string Write_RMB;
	[ProtoMember(7)]
	public string Write_RMBPayValue;
    [ProtoMember(162)]
    public string Write_MaoXianJiaExp;
    [ProtoMember(8)]
	public string Write_YueKa;
	[ProtoMember(9)]
	public string Write_YueKaDayStatus;
	[ProtoMember(10)]
	public string Write_TiLi;
	[ProtoMember(11)]
	public string Write_MaxTiLi;
	[ProtoMember(12)]
	public string Write_SP;
	[ProtoMember(13)]
	public string Write_RoseExpNow;
	[ProtoMember(14)]
	public string Write_RoseHpNow;
	[ProtoMember(15)]
	public string Write_RoseOccupation;
	[ProtoMember(16)]
	public string Write_StoryStatus;
	[ProtoMember(17)]
	public string Write_NowMapName;
	[ProtoMember(18)]
	public string Write_NowMapPositionName;
	[ProtoMember(19)]
	public string Write_OffGameTime;
	[ProtoMember(20)]
	public string Write_AchievementTaskID;
	[ProtoMember(21)]
	public string Write_AchievementTaskValue;
	[ProtoMember(22)]
	public string Write_CompleteTaskID;
	[ProtoMember(23)]
	public string Write_LearnSkillID;
	[ProtoMember(24)]
	public string Write_EquipSkillID;
	[ProtoMember(25)]
	public string Write_EquipSuitSkillID;
	[ProtoMember(26)]
	public string Write_LearnSkillIDSet;
	[ProtoMember(27)]
	public string Write_EquipSkillIDSet;
	[ProtoMember(28)]
	public string Write_SceneItemID;
	[ProtoMember(29)]
	public string Write_PVEChapter;
	[ProtoMember(30)]
	public string Write_SpecialEventTime;
	[ProtoMember(31)]
	public string Write_SpecialEventOpenTime;
	[ProtoMember(32)]
	public string Write_HuoLi;
	[ProtoMember(33)]
	public string Write_SkillSP;
	[ProtoMember(34)]
	public string Write_ShiMenTaskNum;
	[ProtoMember(35)]
	public string Write_ShiMenNextTaskID;
	[ProtoMember(36)]
	public string Write_LearnTianFuID;
	[ProtoMember(37)]
	public string Write_TanSuoListID;
	[ProtoMember(38)]
	public string Write_Proficiency_1;
	[ProtoMember(39)]
	public string Write_Proficiency_2;
	[ProtoMember(40)]
	public string Write_Proficiency_3;
	[ProtoMember(41)]
	public string Write_ProficiencyID_1;
	[ProtoMember(42)]
	public string Write_ProficiencyID_2;
	[ProtoMember(43)]
	public string Write_ProficiencyID_3;
	[ProtoMember(44)]
	public string Write_QiTianDengLuStatus;
	[ProtoMember(45)]
	public string Write_QiTianDengLu;
	[ProtoMember(46)]
	public string Write_LvLiBao;
	[ProtoMember(47)]
	public string Write_StroeHouseMaxNum;
	[ProtoMember(48)]
	public string Write_PetAddMaxNum;
	[ProtoMember(49)]
	public string Write_DaMiJingLv;
    [ProtoMember(161)]
    public string Write_DaMiJingLvKillTime;
    [ProtoMember(50)]
	public string Write_DaMiJingRewardLv;
	[ProtoMember(51)]
	public string Write_ShiLianTaskNum;
	[ProtoMember(52)]
	public string Write_ShiLianTaskStatus;
	[ProtoMember(53)]
	public string Write_ShiLianNextTaskID;
	[ProtoMember(54)]
	public string Write_ChengHaoIDSet;
	[ProtoMember(55)]
	public string Write_NowChengHaoID;
	[ProtoMember(56)]
	public string Write_JingLingIDSet;
	[ProtoMember(57)]
	public string Write_JingLingEquipID;
    [ProtoMember(163)]
    public string Write_XiLianDaShiShuLian;
    [ProtoMember(164)]
    public string Write_XiLianDaShiIDSet;
    [ProtoMember(165)]
    public string Write_LearnTianSkillID;
    [ProtoMember(173)]
    public string Write_LingPaiRewardID;
    [ProtoMember(174)]
    public string Write_PetXiuLian;
    [ProtoMember(200)]
    public string Write_FuBen_ShangHaiHightID;
    [ProtoMember(201)]
    public string Write_FuBen_ShangHaiRewardID;
    [ProtoMember(202)]
    public string Write_FuBen_ShangHaiLvRewardSet;
    [ProtoMember(203)]
    public string Write_JueXingExp;
    [ProtoMember(204)]
    public string Write_JueXingJiHuoID;
    [ProtoMember(205)]
    public string Write_YanSeIDSet;
    [ProtoMember(206)]
    public string Write_NowYanSeID;
    [ProtoMember(207)]
    public string Write_NowYanSeHairID;
    [ProtoMember(214)]
    public string Write_ShengXiaoSet;
    [ProtoMember(215)]
    public string Write_ZhenYing;
    [ProtoMember(216)]
    public string Write_ProficiencyID_4;
    [ProtoMember(236)]
    public string Write_ZhuLingIDSet;
    [ProtoMember(237)]
    public string Write_PlayerIP;

    //*********RoseConfig内容************
    [ProtoMember(58)]
	public string Write_MainUITaskID;
	[ProtoMember(59)]
	public string Write_RoseEquipHideNumID;
	[ProtoMember(60)]
	public string Write_DengLuReward;
	[ProtoMember(61)]
	public string Write_DengLuDayStatus;
	[ProtoMember(62)]
	public string Write_ZhiChiZuoZheID;
	[ProtoMember(63)]
	public string Write_DeathMonsterID;
	[ProtoMember(64)]
	public string Write_FirstEnterGame;
	[ProtoMember(65)]
	public string Write_BeiYong_2;
	[ProtoMember(66)]
	public string Write_BeiYong_5;
	[ProtoMember(67)]
	public string Write_BeiYong_6;
	[ProtoMember(68)]
	public string Write_BeiYong_7;
	[ProtoMember(69)]
	public string Write_BeiYong_8;
	[ProtoMember(70)]
	public string Write_BeiYong_9;

	//*********RoseDayReward内容***********
	[ProtoMember(71)]
	public string Write_CountryExp;
	[ProtoMember(72)]
	public string Write_CountryHonor;
	[ProtoMember(73)]
	public string Write_CountryLv;
	[ProtoMember(74)]
	public string Write_Day_ExpNum;
	[ProtoMember(75)]
	public string Write_Day_ExpTime;
	[ProtoMember(76)]
	public string Write_Day_GoldNum;
	[ProtoMember(77)]
	public string Write_Day_GoldTime;
	[ProtoMember(78)]
	public string Write_ChouKaTime_One;
	[ProtoMember(79)]
	public string Write_ChouKaTime_Ten;
	[ProtoMember(80)]
	public string Write_Day_FuBenNum;
	[ProtoMember(81)]
	public string Write_FenXiang_1;
	[ProtoMember(82)]
	public string Write_FenXiang_3;
	[ProtoMember(83)]
	public string Write_FenXiang_2;
	[ProtoMember(84)]
	public string Write_DayTaskID;
	[ProtoMember(85)]
	public string Write_DayTaskValue;
    [ProtoMember(166)]
    public string Write_DayTaskHuoYueValue;
    [ProtoMember(167)]
    public string Write_DayTaskCommonHuoYueRewardID;
    [ProtoMember(86)]
	public string Write_MeiRiLiBao;
	[ProtoMember(87)]
	public string Write_MeiRiCangBaoNum;
	[ProtoMember(88)]
	public string Write_MeiRiCangBaoTrueChestNum;
	[ProtoMember(89)]
	public string Write_FuBen_1_DayNum;
    [ProtoMember(90)]
    public string Write_DaMiJing_DayNum;
    [ProtoMember(168)]
    public string Write_DayPayValue;
    [ProtoMember(169)]
    public string Write_QianDaoNum_Com;
    [ProtoMember(170)]
    public string Write_QianDaoNum_Pay;
    [ProtoMember(171)]
    public string Write_QianDaoNum_Com_Day;
    [ProtoMember(172)]
    public string Write_QianDaoNum_Pay_Day;
    [ProtoMember(208)]
    public string Write_FengYinCeng;
    


    //*********RoseBag内容************
    [ProtoMember(91)]
	public string[] Write_BagItemIDList;
	[ProtoMember(92)]
	public string[] Write_BagItemNumList;
	[ProtoMember(93)]
	public string[] Write_BagItemHideList;
	[ProtoMember(94)]
	public string[] Write_BagItemParList;
	[ProtoMember(95)]
	public string[] Write_BagGemHoleList;
	[ProtoMember(96)]
	public string[] Write_BagGemIDList;

	//*********RoseHouseStore内容************
	[ProtoMember(97)]
	public string[] Write_StoreHouseItemIDList;
	[ProtoMember(98)]
	public string[] Write_StoreHouseItemNumList;
	[ProtoMember(99)]
	public string[] Write_StoreHouseItemHideList;
	[ProtoMember(100)]
	public string[] Write_StoreHouseItemParList;
	[ProtoMember(101)]
	public string[] Write_StoreHouseGemHoleList;
	[ProtoMember(102)]
	public string[] Write_StoreHouseGemIDList;

	//*********RoseEquip内容************
	[ProtoMember(103)]
	public string[] Write_EquipItemIDList;
	[ProtoMember(104)]
	public string[] Write_EquipHideIDList;
	[ProtoMember(105)]
	public string[] Write_EquipGemHoleList;
	[ProtoMember(106)]
	public string[] Write_EquipGemIDList;
	[ProtoMember(107)]
	public string[] Write_EquipQiangHuaIDList;

	//*********RoseEquipHideProperty内容************
	[ProtoMember(108)]
	public Dictionary<string, string> Write_AllHideIDList = new Dictionary<string, string>();

	//新增密码
	[ProtoMember(109)]
	public string Write_ZhangHaoMiMa;
	[ProtoMember(110)]
	public string Write_UpdateType;     // 1 表示为需要密码更新   2 表示为自动上传（不需要密码）
	[ProtoMember(111)]
	public string Write_BeiYong_10;

	//宠物列表
	[ProtoMember(112)]
	public string[] Write_PetStatus_List;
	[ProtoMember(113)]
	public string[] Write_PetID_List;
	[ProtoMember(114)]
	public string[] Write_PetLv_List;
	[ProtoMember(115)]
	public string[] Write_PetNowHp_List;
	[ProtoMember(116)]
	public string[] Write_PetMaxHp_List;
	[ProtoMember(117)]
	public string[] Write_PetExp_List;
	[ProtoMember(118)]
	public string[] Write_PetName_List;
	[ProtoMember(119)]
	public string[] Write_IfBaby_List;
	[ProtoMember(120)]
	public string[] Write_AddPropretyNum_List;
	[ProtoMember(121)]
	public string[] Write_AddPropretyValue_List;
	[ProtoMember(122)]
	public string[] Write_PetPingFen_List;
	[ProtoMember(123)]
	public string[] Write_ZiZhi_Hp_List;
	[ProtoMember(124)]
	public string[] Write_ZiZhi_Act_List;
	[ProtoMember(125)]
	public string[] Write_ZiZhi_MageAct_List;
	[ProtoMember(126)]
	public string[] Write_ZiZhi_Def_List;
	[ProtoMember(127)]
	public string[] Write_ZiZhi_Adf_List;
	[ProtoMember(128)]
	public string[] Write_ZiZhi_ActSpeed_List;
	[ProtoMember(129)]
	public string[] Write_ZiZhi_ChengZhang_List;
	[ProtoMember(160)]
	public string[] Write_PetSkill_List;

    [ProtoMember(217)]
    public string[] Write_PetEquipID_1;
    [ProtoMember(218)]
    public string[] Write_PetEquipIDHide_1;
    [ProtoMember(219)]
    public string[] Write_PetEquipID_2;
    [ProtoMember(220)]
    public string[] Write_PetEquipIDHide_2;
    [ProtoMember(221)]
    public string[] Write_PetEquipID_3;
    [ProtoMember(222)]
    public string[] Write_PetEquipIDHide_3;
    [ProtoMember(223)]
    public string[] Write_PetEquipID_4;
    [ProtoMember(224)]
    public string[] Write_PetEquipIDHide_4;

    //成就相关
    [ProtoMember(130)]
	public string Write_ChengJiuID;
	[ProtoMember(131)]
	public string Write_ChengJiuRewardID;
	[ProtoMember(132)]
	public string Write_ChengJiu_1;
	[ProtoMember(133)]
	public string Write_ChengJiu_2;
	[ProtoMember(134)]
	public string Write_ChengJiu_3;
	[ProtoMember(135)]
	public string Write_ChengJiu_4;
	[ProtoMember(136)]
	public string Write_ChengJiu_5;
	[ProtoMember(137)]
	public string Write_ChengJiu_101;
	[ProtoMember(138)]
	public string Write_ChengJiu_102;
	[ProtoMember(139)]
	public string Write_ChengJiu_103;
	[ProtoMember(140)]
	public string Write_ChengJiu_104;
	[ProtoMember(141)]
	public string Write_ChengJiu_105;
	[ProtoMember(142)]
	public string Write_ChengJiu_106;
	[ProtoMember(143)]
	public string Write_ChengJiu_107;
	[ProtoMember(144)]
	public string Write_ChengJiu_108;
	[ProtoMember(145)]
	public string Write_ChengJiu_109;
	[ProtoMember(146)]
	public string Write_ChengJiu_110;
	[ProtoMember(147)]
	public string Write_ChengJiu_111;
	[ProtoMember(148)]
	public string Write_ChengJiu_201;
	[ProtoMember(149)]
	public string Write_ChengJiu_202;
	[ProtoMember(150)]
	public string Write_ChengJiu_203;
	[ProtoMember(151)]
	public string Write_ChengJiu_204;
	[ProtoMember(152)]
	public string Write_ChengJiu_205;
	[ProtoMember(153)]
	public string Write_ChengJiu_206;
	[ProtoMember(154)]
	public string Write_ChengJiu_207;
	[ProtoMember(155)]
	public string Write_ChengJiu_208;
	[ProtoMember(156)]
	public string Write_ChengJiu_209;
	[ProtoMember(157)]
	public string Write_ChengJiu_210;
	[ProtoMember(158)]
	public string Write_ChengJiu_211;
	[ProtoMember(159)]
	public string Write_ChengJiu_212;
    [ProtoMember(225)]
    public string Write_ChengJiu_220;
    [ProtoMember(226)]
    public string Write_ChengJiu_221;
    [ProtoMember(227)]
    public string Write_ChengJiu_222;
    [ProtoMember(228)]
    public string Write_ChengJiu_223;
    [ProtoMember(229)]
    public string Write_ChengJiu_224;
    [ProtoMember(230)]
    public string Write_ChengJiu_225;
    [ProtoMember(231)]
    public string Write_ChengJiu_226;
    [ProtoMember(232)]
    public string Write_ChengJiu_227;
    [ProtoMember(233)]
    public string Write_ChengJiu_228;
    [ProtoMember(234)]
    public string Write_ChengJiu_229;
    [ProtoMember(235)]
    public string Write_ChengJiu_250;

    [ProtoMember(210)]
    public string Write_GetGoldSum;
    [ProtoMember(211)]
    public string Write_GetZuanShiSum;
    [ProtoMember(212)]
    public string Write_CostGoldSum;
    [ProtoMember(213)]
    public string Write_CostZuanShiSum;

    [ProtoMember(175)]
    public string Write_ChengJiu_6;
    [ProtoMember(176)]
    public string Write_ChengJiu_7;
    [ProtoMember(177)]
    public string Write_PastureLv;
    [ProtoMember(178)]
    public string Write_PastureExp;
    [ProtoMember(179)]
    public string Write_PastureGold;
    [ProtoMember(180)]
    public string Write_ZuoQiLv;
    [ProtoMember(181)]
    public string Write_ZuoQiPiFuSet;
    [ProtoMember(182)]
    public string Write_ZuoQiZiZhi_1;
    [ProtoMember(183)]
    public string Write_ZuoQiZiZhi_2;
    [ProtoMember(184)]
    public string Write_ZuoQiZiZhi_3;
    [ProtoMember(185)]
    public string Write_ZuoQiZiZhi_4;
    [ProtoMember(186)]
    public string Write_ZuoQi_NengLiLv_1;
    [ProtoMember(187)]
    public string Write_ZuoQi_NengLiExp_1;
    [ProtoMember(188)]
    public string Write_ZuoQi_NengLiLv_2;
    [ProtoMember(189)]
    public string Write_ZuoQi_NengLiExp_2;
    [ProtoMember(190)]
    public string Write_ZuoQi_NengLiLv_3;
    [ProtoMember(191)]
    public string Write_ZuoQi_NengLiExp_3;
    [ProtoMember(192)]
    public string Write_ZuoQi_NengLiLv_4;
    [ProtoMember(193)]
    public string Write_ZuoQi_NengLiExp_4;
    [ProtoMember(194)]
    public string Write_NowZuoQiShowID;
    [ProtoMember(195)]
    public string Write_ZuoQiBaoShiDu;
    [ProtoMember(196)]
    public string Write_ZuoQiBaoStatus;
    [ProtoMember(197)]
    public string Write_ZuoQiJieDuanLv;
    [ProtoMember(198)]
    public string Write_ZuoQiXianJiExp;
    [ProtoMember(199)]
    public string Write_PastureDuiHuanID;

    [ProtoMember(209)]
    public string Player_UpdateType;    //1.默认上传 2.抽卡 3.宠物洗炼上传 4.装备洗炼 5.宠物战队上传！ 6.等级上传 7.手动上传

}

