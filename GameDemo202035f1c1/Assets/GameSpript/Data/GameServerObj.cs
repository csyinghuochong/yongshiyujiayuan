using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameServerObj : MonoBehaviour {

    public GameObject Obj_PaiHang;
	public GameObject Obj_Mail;                     
    public GameObject Obj_ZhanQu_RewardLv;          //战区等级奖励
    public GameObject Obj_ZhanQu_RewardShiLi;       //战区实力值奖励
    public GameObject Obj_HuoDongDaTing;
    public GameObject Obj_Map_HuoDong_1;            //活动1地图
    public GameObject Obj_MapEnterUI_HuoDong_1;     //活动1进入UI
    public GameObject Obj_GuangBoList;              //广播列表
	public GameObject Obj_ChengJiu;
    public GameObject Obj_WorldLv;                  //世界等级
    public GameObject Obj_HongBao;                  //红包
    public GameObject Obj_PetTianTi;                //宠物天梯
    public GameObject Obj_HuoDong_BaoXiang;         //宝箱活动
    public GameObject Obj_HuoDong_ShouLie;          //狩猎活动
    public GameObject Obj_JiaoHuListSet;            //交互列表
    public GameObject Obj_ServerListShow;           //服务器列表
    public GameObject Obj_ServerEnterName;          //服务器进入名称
    public GameObject Obj_BtnServerList;            //服务器按钮
    public GameObject Obj_ShenFenYanZheng;          //身份验证
    public GameObject Obj_FindRose;                 //角色找回
    public GameObject Obj_UI_StartGameFunc;         //创角界面总控
    public GameObject Obj_ZhenYingSet;              //阵营相关
    public GameObject Obj_KuangSet;                   //宠物天梯

    public Dictionary<int, BaoXiangData> HuoDong_BaoXiang_ChestSet = new Dictionary<int, BaoXiangData>();

    //服务器相关
    public Pro_ServerListDataSet ProServerListDataSet;          //服务器列表消息
    public string NowServerName;                                //当前最新的服务器
    //拍卖行
    public GameObject Obj_PaiMaiHang;
    //玩家集合
    public GameObject PlayerSetObj;

    //
    public bool OpenLinkStatus;                     //JianCe类型
    public float JianCeTimeSum;
    private float jianCeSec;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
            JianCeTimeSum = JianCeTimeSum + Time.deltaTime;
            jianCeSec = jianCeSec + Time.deltaTime;
            if (JianCeTimeSum < 300)
            {
                if (jianCeSec >= 5)
                {
                    jianCeSec = 0;
                    if (Random.value <= 0.1f)
                    {
                        if (OpenLinkStatus == false)
                        {
                            Application.Quit();
                        }
                    }
                }
            }
        }

	}
}
