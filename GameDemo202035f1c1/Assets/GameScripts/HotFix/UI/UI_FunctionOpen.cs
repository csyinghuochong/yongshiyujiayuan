using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;
using System.Collections.Generic;

public class UI_FunctionOpen : MonoBehaviour {

	public Transform UISet;                 //UI生成的绑点
	
	public bool RoseBag_Status;             //角色背包打开状态
	public GameObject Obj_roseBag;          //实例化的背包
    public GameObject Obj_Bag;              //背包

	public bool RoseEquip_Status;             //角色装备打开状态
	public GameObject Obj_roseEquip;          //实例化的装备
	public GameObject Obj_Equip;              //装备

	public bool RoseTask_Status;             //角色任务打开状态
	public GameObject Obj_roseTask;          //实例化的任务
	public GameObject Obj_Task;              //任务

    public bool RosePet_Status;
    public GameObject Obj_rosePet;
    public GameObject Obj_Pet;

	public bool RoseSkill_Status;             //角色任务打开状态
	public GameObject Obj_roseSkill;          //实例化的任务
	public GameObject Obj_Skill;              //任务

	public bool MakeItem_Status;              //角色任务打开状态
	public GameObject Obj_RoseMakeItem;       //实例化的任务
	public GameObject Obj_MakeItem;           //任务

    public bool RoseRmbStore_Status;          //人民币商城
    public GameObject Obj_RomStore;
    
    public bool RoseFenXiang_Status;            //分享界面
    public GameObject Obj_roseFenXiang;
    public GameObject Obj_FenXiang;

    public bool RoseHuoDongDaTing_Status;       //活动大厅状态
    public GameObject Obj_HuoDongDaTing;        //活动大厅
    public GameObject obj_RoseHuoDongDaTing;   //活动大厅实例化

    public bool RoseMap_Status;                 //小地图打开状态
    public GameObject Obj_Map;                  //小地图源
    private GameObject obj_Map;                 //小地图实例化

    private GameObject obj_GameSetting;         //游戏设置
    public GameObject UI_ZhuLingSet;            //注灵

    public GameObject ObjGameSetting {
        get { return obj_GameSetting; }
        set { obj_GameSetting = value; }
    }

    public GameObject Obj_Pet_ZhaoHuanSet;      //快捷召唤宠物
    public GameObject obj_Pet_ZhaoHuanSet;      //快捷召唤（实力化）
    public GameObject Obj_Pet_ZhaoHuanCDImg;    //召唤CD图片
    public GameObject Obj_Pet_ZhaoHuanCDText;   //召唤

    //攻击键
    private bool FightBtnStatus;                //攻击按钮状态
    private float FightBtnSum;                  //攻击事件累计

    //翻滚
    public bool FanGunCDStatus;                 //翻滚技能CD状态
    public ObscuredFloat FanGunCDSum;                   //翻滚CD计时
    public bool FanGunOnceStatus;               //单次翻滚状态
    public ObscuredFloat FanGunOneCDSum;                //翻滚累计一次时间
    public ObscuredInt FanGunNum;                       //翻滚累计次数
    public GameObject Obj_FanGunCDImg;          //翻滚CD图片
    public GameObject Obj_FanGunCDText;         //翻滚

    //治疗
    public bool PetZhiLiaoCDStatus;                 //翻滚技能CD状态
    public ObscuredFloat PetZhiLiaoCDSum;                   //翻滚CD计时
    public GameObject Obj_PetZhiLiaoCDImg;          //翻滚CD图片
    public GameObject Obj_PetZhiLiaoCDText;         //翻滚

    //其余单独的Obj
    public GameObject Obj_EquipXiLian;      //洗炼界面
    public GameObject Obj_HuiShouItem;		//回收界面
    public GameObject Obj_GiveNPC;          //给予NPC界面

    //其余变量
    private ObscuredFloat buZhuoCD = 2;         //2秒捕捉一次
    private ObscuredFloat buzhuoCDSum;
    private bool buzhuoCDStatus;

    //成就
    public bool RoseChengJiu_Status;                    //角色任务打开状态
    public GameObject Obj_roseChengJiu;            //实例化的任务
    public GameObject Obj_ChengJiu;                //任务

    private ObscuredFloat tanSuoCD = 0;     //暂时不开启探索CD,开始时修改此值即可
    private ObscuredFloat tanSuoCDSum;
    private bool tanSuoCDStatus;
    public Vector3 TansuoRosePosiVec3;
    public bool TanSuoStatus;
    public ObscuredFloat TanSuoLodingTimeSum;
    private ObscuredFloat TanSuoLodingTime = 5;
    //public GameObject Obj_TanSuoLoding;
    private GameObject tanSuoLodingObj;
    private GameObject tansuoSourceObj;

    //拍卖行
    public bool RosePaiMaiHang_Status;		
	public GameObject Obj_PaiMaiHang;
	public GameObject Obj_rosePaiMaiHang;

	public bool RosePaiHangBang_Status;             //角色任务打开状态
	public GameObject Obj_rosePaiHangBang;          //实例化的任务
	public GameObject Obj_PaiHangBang;              //任务

    //家园仓库
    public bool RosePastureBag_Status;
    public GameObject Obj_RosePastureBag;

	//战区活动
	public bool RoseZhanQuHuoDong_Status;		
	public GameObject Obj_ZhanQuHuoDong;
	public GameObject Obj_roseZhanQuHuoDong;

    //每日活跃
    public bool RoseDayTask_Status;
    public GameObject Obj_DayTask;
    public GameObject Obj_roseDayTask;

    //每日世界等级
    public bool WorldLv_Status;
    public GameObject Obj_WorldLv;
    public GameObject Obj_roseWorldLv;

    //技能取消释法状态
    public GameObject Obj_MainSkillCancleBtn;
	public bool MainSkillCancleStatus;
    private bool mainSkillCancleStatusNext = false;
    public ObscuredInt MainSkillCancleStatusTrue_Sum;   //累计到下一帧执行

    private bool ClearnActListStatus;
    private float ClearnActListTime;
    private bool clearnActStatus;

    //玩家查看信息
    public GameObject Obj_PlayerEquipShow;
	public GameObject Obj_PlayerPetShow;

    //攻击反馈UI特效
    public GameObject Obj_AcTUIEffect;
    public GameObject Obj_ActUISet;

    //对话Npc
    public GameObject Obj_JianCeNpc;                    //检测Npc

    //挖宝
    public GameObject Obj_WabaoObj;

    //阵营
    public bool RoseZhenYing_Status;
    public GameObject Obj_ZhenYing;
    public GameObject Obj_roseZhenYing;

    //红包
    public bool RoseHongBao_Status;
    public GameObject Obj_HongBao;
    public GameObject Obj_roseHongBao;

    //狩猎
    public bool RoseShouLie_Status;
    public GameObject Obj_ShouLie;
    public GameObject Obj_roseShouLie;

    //魔塔
    public bool RoseTower_Status;
    public GameObject Obj_Tower;
    public GameObject Obj_roseTower;

    //伤害试炼副本
    public GameObject Obj_ShangHaiShiLian;

    //离线界面
    public GameObject Obj_LiXianShouYi;


    //红包
    public bool ShiMing_Status;
    public GameObject Obj_ShiMing;
    public GameObject Obj_roseShiMing;

    public bool ttt;

    // Use this for initialization

    private Dictionary<string, string> mPrefabPath;
    private Dictionary<string, GameObject> mPrefabGameObject;

    private Rose_Status roseStatus;

    public GameObject Btn_ZhengYing;


    void Start () {

        //显示功能区域按钮是否隐藏
        if (Game_PublicClassVar.Get_wwwSet.IfHindMainBtn)
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_LeftBtnSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else
        {
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_LeftBtnSet.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

        //初始化
        roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        InitPrefabPath();

        //隐藏显示
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 52)
        {
            Btn_ZhengYing.SetActive(false);
        }

    }

	// Update is called once per frame
	void Update () {

        if (buzhuoCDStatus) {
            buzhuoCDSum = buzhuoCDSum + Time.deltaTime;
            if (buzhuoCDSum >= buZhuoCD) {
                buzhuoCDSum = 0;
                buzhuoCDStatus = false;
            }
        }

        if (tanSuoCDStatus) {
            tanSuoCDSum = tanSuoCDSum + Time.deltaTime;
            if (tanSuoCDSum >= tanSuoCD) {
                tanSuoCDSum = 0;
                tanSuoCDStatus = false;
            }
        }

        //翻滚计CD
        if (FanGunCDStatus) {
            FanGunCDSum = FanGunCDSum + Time.deltaTime;
            Obj_FanGunCDImg.GetComponent<Image>().fillAmount = 1 - (FanGunCDSum / 10);
            int fangunTime = (int)(10.0f - FanGunCDSum);
            Obj_FanGunCDText.GetComponent<Text>().text = fangunTime.ToString();

            if (FanGunCDSum >= 10) {
                FanGunCDStatus = false;
                FanGunCDSum = 0;
                FanGunNum = 0;
                Obj_FanGunCDImg.SetActive(false);
                Obj_FanGunCDText.SetActive(false);
            }
        }

        //单次翻滚CD
        if (FanGunOnceStatus) {
            FanGunOneCDSum = FanGunOneCDSum + Time.deltaTime;
            if (FanGunOneCDSum>=10) {
                FanGunOnceStatus = false;
                FanGunOneCDSum = 0;
                FanGunNum = 0;
            }
        }

        //探索状态
        if (TanSuoStatus) {

            TanSuoLodingTimeSum = TanSuoLodingTimeSum + Time.deltaTime;
            //显示当前打开的进度值
            TanSuoLodingTimeSum = TanSuoLodingTimeSum + Time.deltaTime;
            float value = TanSuoLodingTimeSum / TanSuoLodingTime;
            if (tanSuoLodingObj != null)
            {
                tanSuoLodingObj.transform.Find("Img_GetherPro").GetComponent<Image>().fillAmount = value;
            }

            if (TanSuoLodingTimeSum >= TanSuoLodingTime)
            {
                TanSuoStatus = false;
                TanSuoLodingTimeSum = 0;
                Debug.Log("销毁探索...");
                Destroy(tanSuoLodingObj);
                RoseTanSuo();
            }
            else {
                //如果开启的过程中触发移动，则开启失败
                bool roseStopTakeStatus = false;
                if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseStatus != "1")
                {
                    roseStopTakeStatus = true;
                }
                if (roseStopTakeStatus) {
                    TanSuoStatus = false;
                    TanSuoLodingTimeSum = 0;
                    Debug.Log("正在拾取道具,探索销毁...");
                    Destroy(tanSuoLodingObj);
                    if (tansuoSourceObj != null) {
                        Destroy(tansuoSourceObj);
                    }
                }
            }
        }

        //宠物治疗CD
        if (PetZhiLiaoCDStatus)
        {
            PetZhiLiaoCDSum = PetZhiLiaoCDSum + Time.deltaTime;
            if (PetZhiLiaoCDSum >= 12)
            {
                PetZhiLiaoCDStatus = false;
                PetZhiLiaoCDSum = 0;
                Obj_PetZhiLiaoCDImg.SetActive(false);
                Obj_PetZhiLiaoCDText.SetActive(false);
            }

            Obj_PetZhiLiaoCDImg.GetComponent<Image>().fillAmount = 1 - (PetZhiLiaoCDSum / 12);
            int petZhiLiaoTime = (int)(12.0f - PetZhiLiaoCDSum);
            Obj_PetZhiLiaoCDText.GetComponent<Text>().text = petZhiLiaoTime.ToString();
        }

        if (FightBtnStatus) {
            FightBtnSum = FightBtnSum + Time.deltaTime;
            if (FightBtnSum>=0.8f) {
                FightBtnSum = 0;
                FightBtnStatus = false;
            }
        }


        //技能取消释放时进行累加操作,这样延迟判定技能是否取消,要不技能不会取消释放，在PC端可以，在手机端就不可以
        if (mainSkillCancleStatusNext) {
            MainSkillCancleStatusTrue_Sum = MainSkillCancleStatusTrue_Sum + 1;
            if (MainSkillCancleStatusTrue_Sum >= 2) {
                MainSkillCancleStatusTrue_Sum = 0;
                mainSkillCancleStatusNext = false;
                MainSkillCancleStatus = false;
            }
        }


        if (ClearnActListStatus) {
            ClearnActListTime = ClearnActListTime + Time.deltaTime;
            if (ClearnActListTime >= 5) {
                ClearnActListTime = 0;
                ClearnActListStatus = false;
                //清理选中
                Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                foreach (GameObject obj in roseStatus.AutomaticMonsterObjList.Keys)
                {
                    roseStatus.AutomaticMonsterObjList[obj].ifXuanZhong = false;
                }
            }
        }


        if (ttt) {
            ttt = false;
            //Btn_ShengCunDaZuoZhan_Enter();
        }


    }

    //打开角色背包
    public void OpenBag() {
		//当前背包是否打开
		if (!RoseBag_Status)
		{
			//载入背包UI
			RoseBag_Status = true;
			Obj_roseBag = (GameObject)FunctionInstantiate(Obj_Bag, "Obj_Bag");
			Obj_roseBag.transform.SetParent(UISet);
			Obj_roseBag.transform.localScale = new Vector3(1,1,1);
			Obj_roseBag.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(340f, 5, 0);
            playUISource_Open();    //播放音效
		}
		else {
			//注销背包UI
			RoseBag_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseBag);
		}
    }

	public void OpenRoseEquip(){
		//当前角色装备是否打开
		if (!RoseEquip_Status)
		{
			//载入背包UI
			RoseEquip_Status = true;
			Obj_roseEquip = (GameObject)FunctionInstantiate(Obj_Equip, "Obj_Equip");
			Obj_roseEquip.transform.SetParent(UISet);
			Obj_roseEquip.transform.localScale = new Vector3(1,1,1);
			//Obj_roseEquip.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_roseEquip.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_roseEquip.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            playUISource_Open();    //播放音效
            //显示怪物模型
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(true);
		}
		else {
			//注销背包UI
			RoseEquip_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseEquip);
            Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.SetActive(false);
		}
	}

	public void OpenRoseTask(){
		//当前角色装备是否打开
		if (!RoseTask_Status)
		{
			//载入背包UI
			RoseTask_Status = true;
			Obj_roseTask = (GameObject)FunctionInstantiate(Obj_Task, "Obj_Task");
			Obj_roseTask.transform.SetParent(UISet);
			Obj_roseTask.transform.localScale = new Vector3(1,1,1);
			Obj_roseTask.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_roseTask.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_roseTask.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            playUISource_Open();    //播放音效
		}
		else {
			//注销背包UI
			RoseTask_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseTask);
		}
	}

	public void OpenRoseSkill(){

		//当前角色装备是否打开
		if (!RoseSkill_Status)
		{
			//载入背包UI
			RoseSkill_Status = true;
			Obj_roseSkill = (GameObject)FunctionInstantiate(Obj_Skill, "Obj_Skill");
			Obj_roseSkill.transform.SetParent(UISet);
			Obj_roseSkill.transform.localScale = new Vector3(1,1,1);
			//Obj_roseSkill.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_roseSkill.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_roseSkill.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            playUISource_Open();    //播放音效
            //取消新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseSkill(false);
        }
		else {
			//注销背包UI
			RoseSkill_Status = false;
            playUISource_Close();   //播放音效
			Destroy(Obj_roseSkill);
		}
	}

	//打开制造道具
	public void OpenMakeItem(){

		//当前角色装备是否打开
		if (!MakeItem_Status)
		{
			//载入背包UI
			MakeItem_Status = true;
			Obj_RoseMakeItem = (GameObject)FunctionInstantiate(Obj_MakeItem, "Obj_MakeItem");
			Obj_RoseMakeItem.transform.SetParent(UISet);
			Obj_RoseMakeItem.transform.localScale = new Vector3(1,1,1);
			//Obj_RoseMakeItem.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_RoseMakeItem.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_RoseMakeItem.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
			playUISource_Open();    //播放音效
            //取消新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseMake(false);
        }
		else {
			//注销背包UI
			MakeItem_Status = false;
			playUISource_Close();   //播放音效
			Destroy(Obj_RoseMakeItem);
		}
	}


    //打开角色宠物
    public void OpenPet()
    {

        //判断玩家等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv <= 7)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_49");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先将玩家等级提升至8级开启宠物功能！");
            return;
        }

        //当前背包是否打开
        if (!RosePet_Status)
        {
            //载入背包UI
            RosePet_Status = true;
            Obj_rosePet = (GameObject)FunctionInstantiate(Obj_Pet, "Obj_Pet");
            Obj_rosePet.transform.SetParent(UISet);
            Obj_rosePet.transform.localScale = new Vector3(1, 1, 1);
            Obj_rosePet.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_rosePet.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            playUISource_Open();    //播放音效
                                    //取消新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RosePet(false);
        }
        else
        {
            //注销背包UI
            RosePet_Status = false;
            playUISource_Close();   //播放音效
            Destroy(Obj_rosePet);
        }
    }


    //打开成就
    public void OpenRoseChengJiu()
    {
        //当前角色装备是否打开
        if (!RoseChengJiu_Status)
        {
            //载入背包UI
            RoseChengJiu_Status = true;
            Obj_roseChengJiu = (GameObject)FunctionInstantiate(Obj_ChengJiu, "Obj_ChengJiu");
            Obj_roseChengJiu.transform.SetParent(UISet);
            Obj_roseChengJiu.transform.localScale = new Vector3(1, 1, 1);
            //Obj_roseSkill.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            Obj_roseChengJiu.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
            Obj_roseChengJiu.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            //注销背包UI
            RoseChengJiu_Status = false;
            playUISource_Close();   //播放音效
            Destroy(Obj_roseChengJiu);
        }
    }


    //自动战斗
    public void AutomaticGuaJi()
    {

        //AutomaticFight();
        //return;
        //实例化一个特效

        //角色状态
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        //执行攻击
        AutomaticFight();

        if (roseStatus.Obj_ActTarget == null)
        {
            //触发拾取
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_FunctionOpen.GetComponent<UI_FunctionOpen>().AutomaticTake();
        }

        bool ifReturn = false;
        //判断目标是否为空
        if (roseStatus.Obj_ActTarget == null)
        {
            ifReturn = true;
        }
        else {
            //判断目标是否较远
            float dis = Vector3.Distance(roseStatus.Obj_ActTarget.transform.position, roseStatus.gameObject.transform.position);
            if (dis >= 25)
            {
                ifReturn = true;
            }
        }

        if (ifReturn)
        {
            //获取是否在初始点
            float dis = Vector3.Distance(roseStatus.AutomaticGuaJiStatusInitPosi, roseStatus.gameObject.transform.position);
            if (dis >= 1)
            {
                //返回初始挂机点
                roseStatus.Move_Target_Position = roseStatus.AutomaticGuaJiStatusInitPosi;
                roseStatus.Move_Target_Status = true;
            }
            else {
                Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有怪物,等待刷新或选择新的挂机点！");
            }
        }

        /*
        if (roseStatus.YaoGanStopMoveTime > 0)
        {
            return;
        }

        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //获取当前怪物的最近距离
        float actDis = 10.0f;

        //如果是法师变为20
        if (roseStatus.RoseOcc == "2"|| roseStatus.RoseOcc == "3")
        {
            actDis = 30.0f;
        }

        if (roseStatus.NextAutomaticMonsterDis <= actDis)
        {
            //获取角色当前状态
            if (roseStatus.RoseStatus != "3")
            {
                //获取自动战斗是否开启
                if (roseStatus.ifAutomatic == false)
                {
                    roseStatus.ifAutomatic = true;
                }
            }
        }
        else
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有怪物！");
        }

        */
    }

    //攻击目标
    public void Btn_ActTarget() {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        GameObject obj_Rose = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;

        float dis = 0;
        GameObject nowObj = null;
        foreach (GameObject obj in roseStatus.AutomaticMonsterObjList.Keys) {
            if (obj != null) {
                float nowDis = Vector3.Distance(obj_Rose.transform.position, obj.transform.position);
                if (dis == 0)
                {
                    dis = nowDis + 1;
                }

                if (dis > nowDis && obj != roseStatus.Obj_ActTarget && roseStatus.AutomaticMonsterObjList[obj].ifXuanZhong == false)
                {
                    nowObj = obj;
                    dis = nowDis;
                }
            }
        }

        if (nowObj != null)
        {
            roseStatus.Obj_ActTarget = nowObj;
            roseStatus.AutomaticMonsterObjList[nowObj].ifXuanZhong = true;

            //对选定的目标显示选中特效
            roseStatus.Obj_ActTarget.GetComponent<AI_1>().AI_Selected_Status = true;
            //开启更新怪物选中状态
            Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus = true;

            ClearnActListStatus = true;

            //展示BOSS条
            if (nowObj.GetComponent<AI_1>() != null)
            {
                string ifBoss = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfBoss", "ID", nowObj.GetComponent<AI_1>().AI_ID.ToString(), "Monster_Template");
                if (ifBoss == "1")
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().UpdataMonster = true;
                }
                else
                {
                    Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_BossHp.gameObject.GetComponent<UI_BossHp>().CloseStatus = true;
                }
            }
        }
        else {
            foreach (GameObject obj in roseStatus.AutomaticMonsterObjList.Keys)
            {
                roseStatus.AutomaticMonsterObjList[obj].ifXuanZhong = false;
            }

            if (roseStatus.AutomaticMonsterObjList.Count>=1) {
                if (clearnActStatus == false) {
                    clearnActStatus = true;
                    Btn_ActTarget();
                }
            }
        }

        clearnActStatus = false;
    }

    //自动战斗
    public void AutomaticFight(bool speakNpc = true) {


        //攻击反馈
        GameObject actEffect = (GameObject)FunctionInstantiate(Obj_AcTUIEffect, "Obj_AcTUIEffect");
        actEffect.transform.SetParent(Obj_ActUISet.transform);
        actEffect.transform.localPosition = Vector3.zero;
        actEffect.transform.localScale = new Vector3(1, 1, 1);

        //Debug.LogError("我按下了攻击！");
        //1秒只能戳1次攻击键
        if (FightBtnStatus) {
            //Debug.Log("1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!1秒戳一次!");
            return;
        }

        FightBtnStatus = true;

        //角色状态
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        if (roseStatus.YaoGanStopMoveTime > 0)
        {
            return;
        }

        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //获取当前怪物的最近距离
        float actDis = 15.0f;

        //如果是法师变为20
        if (roseStatus.RoseOcc == "2"|| roseStatus.RoseOcc == "3")
        {
            actDis = 15.0f;
        }

        //获取当前怪物的最近距离
        if (roseStatus.NextAutomaticMonsterDis <= actDis)
        {
            //获取角色当前状态
            if (roseStatus.RoseStatus != "3")
            {
                //获取自动战斗是否开启
                if (roseStatus.ifAutomatic == false)
                {
                    roseStatus.ifAutomatic = true;
                }
            }

            //下坐骑
            if (roseStatus.ZuoQiStatus) {
                roseStatus.ZuoQiStatus = false;
            }
        }
        else
        {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有怪物！");
            //检测一下附近是否有Npc,如果有移动至Npc处
            if (speakNpc) {
                AutomaticNpc();
            }

            /*
            Debug.Log("攻击攻击");
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().roseAnimatorOpen("3");
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseActActionStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseActActionTime = 1f;
            */
        }
    }


    //按下攻击
    public void AutomaticFight_Down()
    {

        //显示攻击范围
        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().CreateActRangEffect();

        AutomaticFight(true);
    }

    //松开攻击目标
    public void AutomaticFight_Up() {
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillRangeEffect != null) {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillRangeEffect.SetActive(false);
        }
    }

    //自动选取攻击目标（暂不处理,需要做一个攻击列表,目前感觉比较烦,如果后期需求在在做处理）
    public void AutomaticTarget(){
	
	}

    //宠物治疗(治疗当前出战宠物)
    public void Pet_ZhiLiao() {

        //判断当前是否有宠物出战
        if (Game_PublicClassVar.Get_function_AI.Pet_ReturnChuZhanObj()==null) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_53");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此技能为宠物治疗！当前暂无宠物出战！");
            return;
        }

        //判定翻滚CD
        if (PetZhiLiaoCDStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_54");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能冷却中…");
            return;
        }

        PetZhiLiaoCDStatus = true;
        Obj_PetZhiLiaoCDImg.SetActive(true);
        Obj_PetZhiLiaoCDText.SetActive(true);

        string useSkillID = "60000206";
        Debug.Log("useSkillID = " + useSkillID);

        if (useSkillID != "0")
        {
            GameObject skill_0 = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainFunctionUI.transform.Find("UI_MainRoseSkill_0").gameObject;
            skill_0.GetComponent<MainUI_SkillGrid>().SkillID = useSkillID;
            skill_0.GetComponent<MainUI_SkillGrid>().UseSkillID = useSkillID;
            skill_0.GetComponent<MainUI_SkillGrid>().updataSkill();
            skill_0.GetComponent<MainUI_SkillGrid>().cleckbutton();
        }


    }

    //对话Npc
    public void AutomaticNpc(bool IfJianCeTask = false) {

        GameObject objJianCe = (GameObject)FunctionInstantiate(Obj_JianCeNpc, "Obj_JianCeNpc");
        objJianCe.transform.localScale = new Vector3(1, 1, 1);
        objJianCe.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        objJianCe.GetComponent<JianCeNpc>().JianCeType = 9;
        objJianCe.GetComponent<JianCeNpc>().IfJianCeTask = IfJianCeTask;
    }

    //采集场景道具
    public void AutomaticGetScenceItem()
    {
        GameObject objJianCe = (GameObject)FunctionInstantiate(Obj_JianCeNpc, "Obj_JianCeNpc");
        objJianCe.transform.localScale = new Vector3(1, 1, 1);
        objJianCe.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        objJianCe.GetComponent<JianCeNpc>().JianCeType = 13;
    }

    //自动拾取
    public void AutomaticTake() {

        //角色状态
        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return;
            }
        }

        //获取当前怪物的最近距离
        if (roseStatus.NextAutomaticDropDis <= 6.0f && roseStatus.NextAutomaticDropObj != null)
        {
            //获取自动战斗是否开启
            if (roseStatus.IfAutomaticTake == false)
            {
                roseStatus.IfAutomaticTake = true;
                roseStatus.shiquNullNum = 0;
                //取消新手引导
                Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_ShiQu(false);
            }
        }
        else
        {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("附近没有可拾取的道具！");
            //拾取道具
            AutomaticGetScenceItem();
        }
    }

    //自动翻滚
    public void FanGunBtn() {

        //判定翻滚CD
        if (FanGunCDStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_54");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能冷却中…");
            return;
        }

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>() == null)
        {

            float fanGunValue = 15;
            string addStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue("FanGun", "7");
            if (addStr == "") {
                addStr = "0";
            }
            fanGunValue = fanGunValue + int.Parse(addStr);

            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.AddComponent<RoseSkill_FanGun_1>();
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().FanGunSpeedValue = fanGunValue;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().SkillTime = 0.5f;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<RoseSkill_FanGun_1>().MoveStatus = true;
            //播放动作
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().roseAnimator.Play("FanGun");
            //播放音效
            //播放音效
            Game_PublicClassVar.Get_function_UI.PlaySource("chong", "4");
            //获取技能特效名称
            string acteffectName = "Eff_Skill_ChongFeng_2";
            if (acteffectName != "" && acteffectName != "0")
            {
                //实例化技能特效
                GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + acteffectName, typeof(GameObject));
                GameObject effect = (GameObject)Instantiate(SkillEffect);
                effect.SetActive(false);
                //string positionName = "Center";
                effect.transform.parent = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform;
                effect.transform.localPosition = Vector3.zero;
                effect.transform.localRotation = Quaternion.Euler(Vector3.zero);
                effect.SetActive(true);
                //2秒后自动销毁特效
                Destroy(effect, 2.0f);
            }

            //设置翻滚CD
            FanGunNum = FanGunNum + 1;
            if (FanGunOnceStatus) {
                //累计翻滚3次进入翻滚冷却状态
                if (FanGunNum >= 3)
                {
                    FanGunCDStatus = true;
                    FanGunCDSum = 0;

                    Obj_FanGunCDImg.SetActive(true);
                    Obj_FanGunCDText.SetActive(true);
                }
            }

            FanGunOnceStatus = true;
            FanGunOneCDSum = 0;

            /*
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillAnimationStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().SkillAnimationTime = 0;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseNowUseSkillAnimationName = "FanGun";
            */
        }
        else {
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能CD中");
        }
    }


    //捕捉
    public void BuZhuoBtn() {

        //判断玩家等级
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv <= 7) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_69");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先将玩家等级提升至8级开启捕捉宠物功能！");
            return;
        }

        //探索状态无法捕捉
        if (TanSuoStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_70");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("探索中无法捕捉！");
            return;
        }

        //获取目标是否为空
        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;
        if (actObj == null)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_71");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先选择要捕捉的目标！");
            return;
        }

        //获取目标是否为BOSS
        string monsterType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MonsterType", "ID", actObj.GetComponent<AI_Property>().AI_ID, "Monster_Template");
        if (monsterType == "3")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_72");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("首领BOSS无法捕捉！");
            return;
        }

        //判断目标是否死亡
        if (actObj.GetComponent<AI_Property>().AI_Hp <= 0 || actObj.GetComponent<AI_1>().ai_IfDeath) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_73");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("目标已经死亡,无法捕捉！");
            return;
        }

        //判定当前捕捉CD
        if (buzhuoCDStatus) {

            string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_74");
            string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_75");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + (int)(buZhuoCD - buzhuoCDSum) + langStrHint_2);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前捕捉冷却时间请" + (int)(buZhuoCD - buzhuoCDSum) + "秒再试...");
            return;
        }

        bool ifZhuaPetStatus = Game_PublicClassVar.Get_function_AI.Pet_JianCeZhuaPet(actObj);
        if (ifZhuaPetStatus == false) {
            return;
        }


        string costItemID = "10010085";
        int buzhuoTiLi = 3;
        string ItemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", costItemID, "Item_Template");
        int costItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(costItemID);

        //判定体力值是否足够,如果足够就扣除
        int roseTili = Game_PublicClassVar.Get_function_Rose.GetRoseTili();
        if (roseTili < buzhuoTiLi)
        {
            //如果体力值不足就扣除道具
            if (costItemNum <= 0)
            {
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_76");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_77");
                string langStrHint_3 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_78");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + buzhuoTiLi + langStrHint_2 + ItemName + langStrHint_3);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("体力不足" + buzhuoTiLi + "点无法捕捉！或可以使用道具：" + ItemName + "代替!");
                return;
            }
            else {
                //扣除一个道具
                Game_PublicClassVar.Get_function_Rose.CostBagItem(costItemID, 1);
                string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_79");
                string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_80");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + ItemName + langStrHint_2);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front ("消耗捕捉宠物道具：" + ItemName + "1个");
            }
        }
        else {
            //扣除体力
            Game_PublicClassVar.Get_function_Rose.CostRoseTiLi(buzhuoTiLi);
        }

        //目标抓捕超过3次会消耗道具才能进行抓捕
        /*
        if (actObj.GetComponent<AI_1>().AI_BuZhuoNum>=3)
        {
            //是否消耗对应的捕捉道具
            if (costItemNum == 0)
            {
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("捕捉超过3次,需要背包内有" + ItemName + "才可以继续捕捉宠物！");
                return;
            }

            
            //10%概率消耗一个道具
            if (Random.value <= 1)
            {
                if (!Game_PublicClassVar.Get_function_Rose.CostBagItem(costItemID, 1))
                {

                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("道具:" + ItemName + "不足！");
                    return;
                }
            }
           
        }
        */

        actObj.GetComponent<AI_1>().AI_BuZhuoNum = actObj.GetComponent<AI_1>().AI_BuZhuoNum + 1;

        //播放捕捉特效
        GameObject SkillEffect = (GameObject)Resources.Load("Effect/Skill/" + "Eff_BuZhuo", typeof(GameObject));
        if (SkillEffect!=null) {
            GameObject effect = (GameObject)Instantiate(SkillEffect);
            effect.SetActive(false);
            effect.transform.parent = actObj.transform;
            effect.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            effect.SetActive(true);
        }


        //冷却CD
        buzhuoCDStatus = true;
        buzhuoCDSum = 0;

        //开启捕捉
        bool ifZhua = Game_PublicClassVar.Get_function_AI.Pet_ZhuaPet(actObj,0.25f);
        if (ifZhua)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_89");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你!捕捉宠物成功");
            //新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RosePet();

            foreach (GameObject nowObj in Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList) {
                if (nowObj == actObj) {
                    Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightMonsterActList.Remove(nowObj);
                    break;
                }
            }
        }

        //取消新手引导
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_ZhuaPu(false);
    }

    //探索
    public void TanSuoBtn() {

        Debug.Log("我点击了探索");

        //判定当前是否在采集道具
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseGetherItemStatus) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_90");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("拾取道具时,不能进行探索！");
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Move_Target_Status = false;
            return;
        }

        //tanSuoLodingObj = (GameObject)Instantiate(Obj_TanSuoLoding);
        //实例化打开UI
        if (tanSuoLodingObj != null) {
            Destroy(tanSuoLodingObj);
        }
        tanSuoLodingObj = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UIGetherItem);
        string langStr = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalization("探索寻找物品中");
        tanSuoLodingObj.transform.Find("Lab_OpenText").transform.GetComponent<Text>().text = langStr + "……";
        tanSuoLodingObj.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_GetherItemSet.transform);
        tanSuoLodingObj.transform.localPosition = new Vector3(0, 0, 0);
        tanSuoLodingObj.transform.localScale = new Vector3(1, 1, 1);
        TanSuoStatus = true;
        TanSuoLodingTimeSum = 0;
        //播放开启音效
        tansuoSourceObj = Game_PublicClassVar.Get_function_UI.PlaySource("20006", "2", TanSuoLodingTime);

        //写入活跃任务
        Game_PublicClassVar.Get_function_Country.UpdataTaskValue("1", "11", "1");

    }

    private void RoseTanSuo() {

        if (tanSuoCDStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_94");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("刚刚已经搜索过了,休息一下在探索要不会累的鸭！");
            return;
        }

        //判定当前是否有移动
        float dis = Vector3.Distance(Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position, TansuoRosePosiVec3);
        if (dis <= 20)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_95");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此地已经搜索过了,请换个地方再次搜索！");
            return;
        }

        bool tansuoStatus = Game_PublicClassVar.Get_function_Rose.CostHuoLi(5);
        if (!tansuoStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_96");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("活力不足,无法搜索！活力值会随着在线时间而增长！");
            return;
        }

        Debug.Log("tanSuoCDStatus = " + tanSuoCDStatus);

        if (!tanSuoCDStatus)
        {
            tanSuoCDStatus = true;

            //寻找附近有没有必得的点
            GameObject tansuoObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_SceneTanSuo;
            if (tansuoObj != null)
            {
                //检测背包是否有剩余位置
                int nullBagNum = Game_PublicClassVar.Get_function_Rose.BagNullNum();
                if (nullBagNum >= 1)
                {
                    tansuoObj.GetComponent<Scence_TanSuo>().RoseTanSuo();
                }
                else
                {
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_97");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameHint("叮咣叮咣,好像发现了什么！但是你的背包好像已经装不下了!");
                }
                return;
            }

            //随机获得道具
            if (Random.value <= 0.5f)
            {
                string dropID = "50080101";
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                if (roseLv >= 1)
                {
                    dropID = "50080101";
                }
                if (roseLv >= 18)
                {
                    dropID = "50080201";
                }
                if (roseLv >= 30)
                {
                    dropID = "50080301";
                }
                if (roseLv >= 40)
                {
                    dropID = "50080401";
                }
                if (roseLv >= 50)
                {
                    dropID = "50080501";
                }

                string dropStr = Game_PublicClassVar.Get_function_AI.ReturnDropItem(dropID);
                //Debug.Log("dropStr = " + dropStr);
                //获取掉落
                string rewardItemID = dropStr.Split(',')[0];
                string rewardItemNum = dropStr.Split(',')[1];

                //发送奖励
                if (Game_PublicClassVar.Get_function_Rose.SendRewardToBag(rewardItemID, int.Parse(rewardItemNum), "1", 0, "0", true, "4"))
                {
                    string itemName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemName", "ID", rewardItemID, "Item_Template");
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_98");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint + itemName + "*" + rewardItemNum);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你意外找到散落在外面的道具:" + itemName + "*" + rewardItemNum);
                }

            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_99");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("搜索完毕！这附近好像并没有什么东西！");
            }

            TansuoRosePosiVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        }
    }


    //召唤出战宠物
    public void BtnZhaoHuan() {

        //判定出战
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhaoHuanCDStatus) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_100！");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("召唤冷却中！");
            return;
        }

        if (obj_Pet_ZhaoHuanSet == null) {
            //召唤
            obj_Pet_ZhaoHuanSet = (GameObject)FunctionInstantiate(Obj_Pet_ZhaoHuanSet, "Obj_Pet_ZhaoHuanSet");
            obj_Pet_ZhaoHuanSet.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            obj_Pet_ZhaoHuanSet.transform.position = new Vector3(this.gameObject.transform.position.x + 190, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
            obj_Pet_ZhaoHuanSet.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            Destroy(obj_Pet_ZhaoHuanSet);
        }
    }

    public void GuaJiBtn() {

        Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();

        if (roseStatus.AutomaticGuaJiStatus)
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_111");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("关闭自动挂机！");
            roseStatus.AutomaticGuaJiStatus = false;
        }
        else {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_112");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("开启自动挂机！");
            roseStatus.AutomaticGuaJiStatus = true;
            //设置自己的坐标点
            roseStatus.AutomaticGuaJiStatusInitPosi = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;

        }

    }

    //打开商城
    public void Open_RmbStore()
    {
        if (RoseRmbStore_Status)
        {
            Obj_RomStore.SetActive(false);
            RoseRmbStore_Status = false;
        }
        else {
            Obj_RomStore.SetActive(true);
            RoseRmbStore_Status = true;
        }
    }

    //打开商城分享
    public void Open_FenXiang()
    {
        Debug.Log("我点击了分享按钮");
        if (!RoseFenXiang_Status)
        {
            //载入背包UI
            RoseFenXiang_Status = true;
            Obj_roseFenXiang = (GameObject)FunctionInstantiate(Obj_FenXiang, "Obj_FenXiang");
            Debug.Log("我点击了分享按钮1111");
            Obj_roseFenXiang.transform.SetParent(UISet);
            Obj_roseFenXiang.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseFenXiang.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
            //Obj_FenXiang.SetActive(false);
            //RoseFenXiang_Status = false;
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseFenXiang);
            RoseFenXiang_Status = false;
        }
    }

    //打开活动大厅
    public void Open_HuoDongDaTing() {
        if (!RoseHuoDongDaTing_Status)
        {
            //载入背包UI
            RoseHuoDongDaTing_Status = true;
            obj_RoseHuoDongDaTing = (GameObject)FunctionInstantiate(Obj_HuoDongDaTing, "Obj_HuoDongDaTing");
            Debug.Log("我点击了分享按钮1111");
            obj_RoseHuoDongDaTing.transform.SetParent(UISet);
            obj_RoseHuoDongDaTing.transform.localScale = new Vector3(1, 1, 1);
            obj_RoseHuoDongDaTing.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else {
            playUISource_Close();   //播放音效
            Destroy(obj_RoseHuoDongDaTing);
            RoseHuoDongDaTing_Status = false;
        }
    }

    //隐藏功能按钮
    public void HindFunctionBtn() {

        if (!Game_PublicClassVar.Get_wwwSet.IfHindMainBtn)
        {
            Game_PublicClassVar.Get_wwwSet.IfHindMainBtn = true;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_LeftBtnSet.SetActive(false);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuo.transform.Find("Img_ShouSuo").GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else {
            Game_PublicClassVar.Get_wwwSet.IfHindMainBtn = false;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_MainUI_LeftBtnSet.SetActive(true);
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuoImg.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            //Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_ShouSuo.transform.Find("Img_ShouSuo").GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }

    }

    //打开地图
    public void Open_Map()
    {
        if (!RoseMap_Status)
        {
            //载入地图UI
            RoseMap_Status = true;
            obj_Map = (GameObject)FunctionInstantiate(Obj_Map, "Obj_Map");
            obj_Map.transform.SetParent(UISet);
            obj_Map.transform.localScale = new Vector3(1, 1, 1);
            obj_Map.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效

            //取消新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseMap(false);
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(obj_Map);
            RoseMap_Status = false;
        }
    }

    //游戏设置
    public void OpenGameSetting()
    {
        if (obj_GameSetting == null)
        {
            obj_GameSetting = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_GameSetting);
            obj_GameSetting.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
            obj_GameSetting.transform.localPosition = Vector3.zero;
            obj_GameSetting.transform.localScale = new Vector3(1, 1, 1);
            //取消新手引导
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_GameYinDaoSet.GetComponent<UI_GameYinDao>().ShowYinDao_RoseSetting(false);
        }
    }

	//游戏拍卖行
	public void OpenGamePaiMaiHang()
	{

        if (Game_PublicClassVar.Get_gameLinkServerObj.ServerLinkStatus == false) {
            //网络问题无法打开拍卖行
            Game_PublicClassVar.Get_function_UI.GameHint("网络异常,暂时无法打开拍卖行");
            return;
        }

		if (!RosePaiMaiHang_Status)
		{
			//载入地图UI
			RosePaiMaiHang_Status = true;
			Obj_rosePaiMaiHang = (GameObject)FunctionInstantiate(Obj_PaiMaiHang, "Obj_PaiMaiHang");
			Obj_rosePaiMaiHang.transform.SetParent(UISet);
			Obj_rosePaiMaiHang.transform.localScale = new Vector3(1, 1, 1);
			Obj_rosePaiMaiHang.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
			playUISource_Open();    //播放音效
		}
		else
		{
			playUISource_Close();   //播放音效
			Destroy(Obj_rosePaiMaiHang);
			RosePaiMaiHang_Status = false;
		}
	}

	//排行榜
	public void OpenPaiHangBang(){

        //查看等级
        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 12) {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_454");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            return;
        }

		//当前角色装备是否打开
		if (!RosePaiHangBang_Status)
		{

            if (Game_PublicClassVar.Get_game_PositionVar.GameDanJiStatus == 1) {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_114");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你关闭了联网功能！请在游戏设置内开启才能使用此功能!");
                return;
            }
			//载入背包UI
			RosePaiHangBang_Status = true;
			Obj_rosePaiHangBang = (GameObject)FunctionInstantiate(Obj_PaiHangBang, "Obj_PaiHangBang");
			Obj_rosePaiHangBang.transform.SetParent(UISet);
			Obj_rosePaiHangBang.transform.localScale = new Vector3(1,1,1);
			Obj_rosePaiHangBang.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
			Obj_rosePaiHangBang.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
			Obj_rosePaiHangBang.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
			playUISource_Open();    //播放音效
		}
		else {
			//注销背包UI
			RosePaiHangBang_Status = false;
			playUISource_Close();   //播放音效
			Destroy(Obj_rosePaiHangBang);
		}
	}


	//游戏战区活动
	public void OpenZhanQuHuoDong()
	{

        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv <= 8) {

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_115");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("9级可以开启战区活动功能!");
            return;
        }

		if (!RoseZhanQuHuoDong_Status)
		{
			//载入地图UI
			RoseZhanQuHuoDong_Status = true;
			Obj_roseZhanQuHuoDong = (GameObject)FunctionInstantiate(Obj_ZhanQuHuoDong, "Obj_ZhanQuHuoDong");
			Obj_roseZhanQuHuoDong.transform.SetParent(UISet);
			Obj_roseZhanQuHuoDong.transform.localScale = new Vector3(1, 1, 1);
			Obj_roseZhanQuHuoDong.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
			playUISource_Open();    //播放音效
		}
		else
		{
			playUISource_Close();   //播放音效
			Destroy(Obj_roseZhanQuHuoDong);
			RoseZhanQuHuoDong_Status = false;
		}
	}

    //每日活跃
    public void OpenErveryDayTask() {

        if (!RoseDayTask_Status)
        {
            RoseDayTask_Status = true;
            Obj_roseDayTask = (GameObject)FunctionInstantiate(Obj_DayTask, "Obj_DayTask");
            Obj_roseDayTask.transform.SetParent(UISet);
            Obj_roseDayTask.transform.localPosition = Vector3.zero;
            Obj_roseDayTask.transform.localScale = new Vector3(1, 1, 1);
        }
        else {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseDayTask);
            RoseDayTask_Status = false;
        }
    }

    //打开世界等级
    public void OpenWorldLv() {

        if (!WorldLv_Status)
        {
            WorldLv_Status = true;
            Obj_roseWorldLv = (GameObject)FunctionInstantiate(Obj_WorldLv, "Obj_WorldLv");
            Obj_roseWorldLv.transform.SetParent(UISet);
            Obj_roseWorldLv.transform.localPosition = Vector3.zero;
            Obj_roseWorldLv.transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseWorldLv);
            WorldLv_Status = false;
        }

    }


	//取消释放技能
	public void RoseSkill_Cancle (){
		//Debug.Log ("取消技能释法");
        string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_122");
        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("取消技能释法");
		MainSkillCancleStatus = true;

	}

	//取消释放技能
	public void RoseSkill_Cancle_False (){
		//Debug.Log ("取消技能释法False:"+ Input.mousePosition);
        //判定是否在指定区域
        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("取消技能释法False:" + Input.mousePosition);
        //MainSkillCancleStatus = false;
        mainSkillCancleStatusNext = true;

        /*
        Vector2 yaoganVec2 = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_MainSkillCancleBtn.transform.position;
        float chaVec2Value = Vector2.Distance(Input.mousePosition, yaoganVec2);
        if (chaVec2Value >= 100)
        {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("取消技能释法False");
            MainSkillCancleStatus = false;
        }
        */
	}


    //红包
    public void OpenGameHongBao()
    {
        if (!RoseHongBao_Status)
        {
            //载入地图UI
            RoseHongBao_Status = true;
            Obj_roseHongBao = (GameObject)FunctionInstantiate(Obj_HongBao, "Obj_HongBao");
            Obj_roseHongBao.transform.SetParent(UISet);
            Obj_roseHongBao.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseHongBao.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseHongBao);
            RoseHongBao_Status = false;
        }
    }

    //狩猎活动
    public void OpenGameShouLie()
    {
        if (!RoseShouLie_Status)
        {
            //载入地图UI
            RoseShouLie_Status = true;
            Obj_roseShouLie = (GameObject)FunctionInstantiate(Obj_ShouLie, "Obj_ShouLie");
            Obj_roseShouLie.transform.SetParent(UISet);
            Obj_roseShouLie.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseShouLie.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseShouLie);
            RoseShouLie_Status = false;
        }
    }

    //打开爬塔
    public void OpenGameTower()
    {
        if (!RoseTower_Status)
        {
            //载入地图UI
            RoseTower_Status = true;
            Obj_roseTower = (GameObject)FunctionInstantiate(Obj_Tower, "Obj_Tower");
            Obj_roseTower.transform.SetParent(UISet);
            Obj_roseTower.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseTower.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseTower);
            RoseTower_Status = false;
        }
    }

    //打开阵营
    public void OpenGameZhenYing()
    {

        if (Game_PublicClassVar.Get_function_Rose.GetRoseLv() < 52) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("52级开启阵营系统!");
            return;
        }

        if (!RoseZhenYing_Status)
        {
            //载入地图UI
            RoseZhenYing_Status = true;
            Obj_roseZhenYing = (GameObject)FunctionInstantiate(Obj_ZhenYing, "Obj_ZhenYing");
            Obj_roseZhenYing.transform.SetParent(UISet);
            Obj_roseZhenYing.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseZhenYing.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseZhenYing);
            RoseZhenYing_Status = false;
        }
    }

    //实名认证
    public void Btn_ShiMingReward() {

        if (EventHandle.IsHuiWeiChannel()) {
            return;
        }

        if (!ShiMing_Status)
        {
            //载入地图UI
            ShiMing_Status = true;
            Obj_roseShiMing = (GameObject)FunctionInstantiate(Obj_ShiMing, "Obj_ShiMing");
            //Obj_roseShiMing = (GameObject)Instantiate(Obj_ShiMing);
            Obj_roseShiMing.transform.SetParent(UISet);
            Obj_roseShiMing.transform.localScale = new Vector3(1, 1, 1);
            Obj_roseShiMing.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, 0, 0);
            playUISource_Open();    //播放音效
        }
        else
        {
            playUISource_Close();   //播放音效
            Destroy(Obj_roseShiMing);
            ShiMing_Status = false;
        }
        

    }

    //上下马
    public void BtnZuoQiUpDown() {

        //检测当前坐骑是否开启
        string nowZuoQiID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZuoQiPiFuSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiID == "" || nowZuoQiID == "0")
        {
            return;
        }

        //检测坐骑外观
        string nowZuoQiSHowID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowZuoQiShowID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");
        if (nowZuoQiSHowID == "" || nowZuoQiSHowID == "0"|| nowZuoQiSHowID == null) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("请先到家园里设定要出战的坐骑..");
            return;
        }

        //判定当前是否在战斗状态
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseFightStatus == true) {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("战斗状态中无法上下坐骑");
            return;
        }

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiZiDongStatus = false;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().UpdateZuoQiBuffStatus = true;
        }
        else {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiZiDongStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuffListShowSet.GetComponent<UI_BuffShowListSet>().UpdateZuoQiBuffStatus = true;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ZuoQiShowModel.GetComponent<ZuoQiShowModel>().PlayStartEffect();
        }

    }


    //打开
    void playUISource_Open() {
        Game_PublicClassVar.Get_function_UI.PlaySource("10001", "1");
    }

    //关闭
    void playUISource_Close()
    {
        Game_PublicClassVar.Get_function_UI.PlaySource("10002", "1");
    }

    //清除当前打开的功能
    public void ClearnOpenUI() {

        //关闭背包
        if (Obj_roseEquip!=null) {
            OpenRoseEquip();
        }

        //关闭任务
        if (Obj_roseTask != null)
        {
            OpenRoseTask();
        }

        //关闭技能
        if (Obj_roseSkill != null)
        {
            OpenRoseSkill();
        }

        //关闭成就
        if (Obj_roseChengJiu != null)
        {
            OpenRoseChengJiu();
        }

        //关闭宠物
        if (Obj_rosePet != null)
        {
            OpenPet();
        }

        //关闭制造
        if (Obj_RoseMakeItem != null)
        {
            OpenMakeItem();
        }

        //排行榜
        if (Obj_rosePaiHangBang != null) {
            OpenPaiHangBang();
        }

        
        //拍卖行 
        if (Obj_rosePaiMaiHang != null)
        {
            OpenGamePaiMaiHang();
        }
        
    }



	//测试进入活动地图
	public void EnterTestMap(){
        
	}

    public GameObject FunctionInstantiate( GameObject go, string path )
    {
        if (go != null)
        {
            return GameObject.Instantiate<GameObject>(go);
        }

        mPrefabGameObject.TryGetValue(path, out go);
        if (go != null)
        {
            return GameObject.Instantiate<GameObject>(go);
        }

        go = Resources.Load<GameObject>(mPrefabPath[path]);
        mPrefabGameObject.Add(path, go);
        return GameObject.Instantiate<GameObject>(go);
    }

    //初始化预制件路径
    private void InitPrefabPath()
    {
        mPrefabPath = new Dictionary<string, string>();
        mPrefabGameObject = new Dictionary<string, GameObject>();
        mPrefabPath.Add("Obj_Bag", "UGUI/UISet/Bag/Rose_UIBag");
        mPrefabPath.Add("Obj_Equip", "UGUI/UISet/RoseEquip/RoseEquip");
        mPrefabPath.Add("Obj_Task", "UGUI/UISet/Task/Rose_TaskList");
        mPrefabPath.Add("Obj_Pet", "UGUI/UISet/Pet/RosePet");
        mPrefabPath.Add("Obj_Skill", "UGUI/UISet/Skill/UI_RoseSkill");
        mPrefabPath.Add("Obj_MakeItem", "UGUI/UISet/RoseEquip/UI_EquipMake");
        mPrefabPath.Add("Obj_FenXiang", "UGUI/UISet/FenXiang/UI_FenXiang");
        mPrefabPath.Add("Obj_HuoDongDaTing", "UGUI/UISet/Other/UI_HuoDongDaTing");
        mPrefabPath.Add("Obj_Map", "UGUI/UISet/Map/UI_Map");
        mPrefabPath.Add("Obj_Pet_ZhaoHuanSet", "UGUI/UISet/Pet/UI_Pet_ZhaoHuanSet");
        mPrefabPath.Add("Obj_ChengJiu", "UGUI/UISet/ChengJiu/Rose_ChengJiuSet");
        mPrefabPath.Add("Obj_PaiMaiHang", "UGUI/UISet/PaiMaiHang/Rose_PaiMaiHangSet");
        mPrefabPath.Add("Obj_PaiHangBang", "UGUI/UISet/PaiHang/UI_PaiHang");
        mPrefabPath.Add("Obj_ZhanQuHuoDong", "UGUI/UISet/ZhanQuHuoDong/UI_ZhanQuHuoDong");
        mPrefabPath.Add("Obj_DayTask", "UGUI/UISet/RoseDay/UI_DayTask");
        mPrefabPath.Add("Obj_WorldLv", "UGUI/UISet/WorldLv/UI_WorldLvSet");
        mPrefabPath.Add("Obj_PlayerEquipShow", "UGUI/UISet/PaiHang/UI_PaiHangRoseEquip");
        mPrefabPath.Add("Obj_PlayerPetShow", "UGUI/UISet/PaiHang/UI_PaiHangShow_RosePet");
        mPrefabPath.Add("Obj_AcTUIEffect", "UGUI/UISet/Fight/UI_FightHintEffect");
        mPrefabPath.Add("Obj_JianCeNpc", "3DModel/RoseModel/RoseJianCeNpc");
        mPrefabPath.Add("Obj_ZhenYing", "UGUI/UISet/ZhenYing/UI_ZhenYingSet");
        mPrefabPath.Add("Obj_HongBao", "UGUI/UISet/HongBao/UI_HongBaoSet");
        mPrefabPath.Add("Obj_ShouLie", "UGUI/UISet/HuoDong/UI_HuoDong_ShouLie");
        mPrefabPath.Add("Obj_Tower", "UGUI/UISet/HuoDong/UI_HuoDong_Tower_Reward");
        mPrefabPath.Add("Obj_ShangHaiShiLian", "UGUI/UISet/FuBen/UI_FuBen_ShangHaiRewadSet");
        mPrefabPath.Add("Obj_ShiMing", "UGUI/UISet/ShiMing/UI_ShiMingRewardSet");
        mPrefabPath.Add("Obj_LiXianShouYi", "UGUI/UISet/LiXian/UI_LiXianShouYi");
        mPrefabPath.Add("UI_ZhuLingSet", "UGUI/UISet/ZhuLing/UI_ZhuLingSet");
        
    }
}
