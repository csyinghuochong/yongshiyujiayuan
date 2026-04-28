using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

//此脚本主要用来显示主界面的快捷技能信息
public class MainUI_SkillGrid : MonoBehaviour
{
    
    //public string FunctionKeyName;                //功能开关名称，储存功能
    public ObscuredString SkillID;                          //当前格子的技能或使用道具ID
    public ObscuredString UseSkillID;                       //触发的技能ID
    public string SkillSpace;                       //填写1-8，对应配置表中的1-8的技能
    //public GameObject roseSkillSet;                 //技能绑点
    public GameObject Obj_SkillCD;                  //技能冷却CD绑点
    public GameObject Text_SkillCD;                 //技能冷却时间显示
    public GameObject Img_SkillIcon;                //技能图标显示
    //public GameObject MouseMoveSkillIcon;           //技能移动实例化的图标
    //public GameObject MainUISkillTips;              //主界面技能Tips
    public GameObject SkillItemNum;                 //道具数量显示
    public GameObject SkillSingImg;                 //技能吟唱选中显示
    public GameObject SkillPublicCD;                //技能公共CD
    public GameObject SkillCDEffect;                //技能冷却CD完毕后显示的UI特效
    public bool IfSkillMove;                        //技能图标是否产生移动
    public bool ItemCostStatus;                     //道具消耗状态,开启表示要消耗一个当前道具并更新             
    private ObscuredInt itemnNum;

    public ObscuredBool skillCDStatus;                      //技能冷却状态
    private ObscuredFloat skillCDTime;                      //技能冷却时间
    private ObscuredFloat skillCDTime_Now;                  //技能当前剩余冷却时间
    private ObscuredFloat skillPublicCDTime_Now;            //公共技能冷却时间
    private ObscuredBool skillPublicCDStatus;               //公共技能冷却状态,开启为当前技能处于公共技能CD中
    public ObscuredBool skillCDSelfStatus;                  //技能自身冷却状态
    private ObscuredFloat skillCDSelfTime;                  //技能自身冷却时间
    private ObscuredBool ifShowCDTime;                      //是否显示冷却的数字时间
    private ObscuredString skillCDType;                     //技能CD类型 0：表示自身CD  1：表示公共CD
    private ObscuredFloat skillCDSelfTimeSum;               //技能自身冷却时间累加值
    

    private bool clickBtnKeyStatus;                 //点击技能Icon处罚的按键状态
    private float Keytime;                          //按键时间
    private string skillIconID;                     //技能图标ID
    private Sprite skillIcon;                       //技能图标
    private GameObject mouseMoveSkillIcon;          //技能移动实例化的图标
    private bool skillIconDragStatus;               //技能拖拽结束状态开启
    private float SkillExchangeTime;                //用作拖拽技能延迟0.1秒后清空数据（解决拖拽技能因为实例化图标不能触发鼠标进入事件）
    private bool skillTipsStatus;                   //技能Tips开启
    private GameObject skillTipsObj;                //技能Tips的Obj
    private Game_PositionVar game_PositionVar;
    public bool MouseEnterStatus;                   //鼠标进入图标状态       
    public Rose_Status roseStatus;                  //
    private string ifSkillRange;

    //多点触碰检测
    private Vector2 DianJi_Vec2;        //点击的底坐标
    private Vector2 DianJiKuan_Vec2;    //点击的大小
    private Rect DianJiRect;            //点击区域

    //技能两次点击间隔
    private bool dianjiStatus;
    private float dianJiTimeSum;

    private int useMP;
    private bool ifUseShowStatus;

    private bool exitStatus;

    private Transform Btn_SkillStart;
    private Vector2 mSkillIndicator;
    private GameObject Obj_RoseModel;

    private bool isDrag;

    // Use this for initialization
    void Start () {
        mSkillIndicator = Vector2.zero;
        //角色
        roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        Obj_RoseModel = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel;

        skillCDStatus = false;
        //SkillSpace = "1";   //测试
        //更新图标
        updataSkill();
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        
        float x = this.GetComponent<RectTransform>().anchoredPosition.x;
        float y = this.GetComponent<RectTransform>().anchoredPosition.y;
        //Debug.Log("x = " + x);
        x = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(x);
        y = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(y);
        DianJi_Vec2 = new Vector2(x, y);
        x = Game_PublicClassVar.Get_function_UI.ReturnScreen_X(80);
        y = Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(80);
        DianJiKuan_Vec2 = new Vector2(x, y);
        //Debug.Log("格子" + SkillSpace + ":" + x + "," + y + "||" + "大小" + this.GetComponent<RectTransform>().localScale.x + "||" + "高宽" + this.GetComponent<RectTransform>().sizeDelta + "||触碰区域" + DianJiKuan_Vec2);
        DianJiRect = new Rect(DianJi_Vec2.x, DianJi_Vec2.y, DianJiKuan_Vec2.x, DianJiKuan_Vec2.y);
        //Debug.Log("区域：" + DianJiRect);

        Btn_SkillStart = transform.Find("Btn_SkillStart");
        this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect = false;
        isDrag = false;
    }
	
	// Update is called once per frame
	void Update () {

        //检测多个触点
        GetTouchPosition(DianJiRect);

        //判定是否更新图标
        if (game_PositionVar.UpdataMainSkillUI)
        {
            //更新图标
            updataSkill();
        }

        //判定当前技能是否处于公共CD中
        if (game_PositionVar.Rose_PublicSkillCDStatus)
        {
            //if (SkillSpace != "0") {
                if (UseSkillID != "" && UseSkillID != "0" && UseSkillID != null) {          //这里会导致没有技能的栏位不会有公共CD显示,如果取消这里,那么技能被动触发第一次效果就会不触发,因为默认第一次技能为空
                    string ifPublicSkillCD = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfPublicSkillCD", "ID", UseSkillID, "Skill_Template");
                    if (ifPublicSkillCD == "0")
                    {
                        skillCDStatus = true;       //开启技能冷却状态
                        skillCDTime = Game_PublicClassVar.Get_game_PositionVar.Rose_PublicSkillCDTime;
                        ifShowCDTime = false;
                        skillCDType = "1";
                    }
                }
            //}
        }
        else {
            //Debug.Log("skillCDStatus1111111111111111");
            //skillCDStatus = false;      //设置自身技能冷却为false,如果下面技能自身的冷却存在的话,此值还是会为true
        }

        //判定技能是否处于自身的冷却CD中
        if (skillCDSelfStatus)
        {
            //设置冷却时间为技能的CD
            skillCDTime = skillCDSelfTime;
            skillCDStatus = true;
            ifShowCDTime = true;
            skillCDType = "0";
            skillCDSelfTimeSum = skillCDSelfTimeSum + Time.deltaTime;
            //Debug.Log("CD1 : " + skillCDSelfTimeSum);
            //判定冷却时间结束后清空自身冷却CD相关值
            if (skillCDSelfTimeSum >= skillCDTime) {
                skillCDSelfTimeSum = 0;
                skillCDSelfStatus = false;
                //ifShowCDTime = false;
                //Debug.Log("冷却结束");
            }
        }

        //判定自身是否处于冷却CD中
        if (skillCDStatus)
        {
            //skillCDTime = skillCDSelfTime;
            updataSkillCDTime(skillCDTime,ifShowCDTime,skillCDType);
        }

        if (ItemCostStatus) {
            ItemCostStatus = false;
            //判定是否为道具
            //根据首字母判定是装备还是技能
            
            string idFirst = SkillID.ToString().Substring(0, 1);
            if (idFirst == "1") {
                //消耗一个道具
                Game_PublicClassVar.Get_function_Rose.CostBagItem(SkillID, 1);
                //更新显示
                updataSkill();
            }
        }

        //设置两次点击按钮的间隔时间
        if (dianjiStatus) {
            dianJiTimeSum = dianJiTimeSum + Time.deltaTime;
            if (dianJiTimeSum >= 0.5f) {
                dianjiStatus = false;
                dianJiTimeSum = 0.0f;
            }
        }

        //判定能量值是否足够  
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue < useMP)
        {
            if (ifUseShowStatus) {
                object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                Material huiMaterial = huiObj as Material;
                Img_SkillIcon.GetComponent<Image>().material = huiMaterial;
                ifUseShowStatus = false;
            }

        }
        else {
            if (ifUseShowStatus == false)
            {
                Img_SkillIcon.GetComponent<Image>().material = null;
                ifUseShowStatus = true;
            }
        }

        if (this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect)
        {
            this.GetComponent<RoseSkill_Sing_1>().OnSkillDragUpdate();
        }
    }


    void LateUpdate() {

    }

    //更新技能图标
    public void updataSkill()
    {

        useMP = 0;
        ifUseShowStatus = true;

        //更新技能
        string lastSkillID = SkillID;       //获取上一次的ID,方便开启时优化
        if (SkillSpace != "0") { 
            SkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("MainSkillUI_" + SkillSpace, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseConfig");
            //Debug.Log("主界面更新技能：" + SkillID);
        }
        if (SkillID == "")
        {
            Img_SkillIcon.SetActive(false);
            SkillItemNum.SetActive(false);      //显示道具数量
        }
        else {
            Img_SkillIcon.SetActive(true);
            string idFirst = SkillID.ToString().Substring(0, 1);
            switch (idFirst)
            {

                //道具图标
                case "1":
                    if (lastSkillID != SkillID) {

                        UseSkillID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", SkillID, "Item_Template");
                        string skillValue = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillID", "ID", SkillID, "Item_Template");
                        skillCDSelfTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", skillValue, "Skill_Template"));
                        //修改ICON （在做技能交换位置时,此处会修正）
                        skillIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", SkillID, "Item_Template");
                        object obj = Resources.Load("ItemIcon/" + skillIconID, typeof(Sprite));
                        skillIcon = obj as Sprite;
                        Img_SkillIcon.GetComponent<Image>().sprite = skillIcon;

                    }

                    //显示道具数量
                    itemnNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(SkillID);
                    //itemnNum = 10;
                    SkillItemNum.GetComponent<Text>().text = itemnNum.ToString();
                    SkillItemNum.SetActive(true);      //显示道具数量
                    if (itemnNum <= 0)
                    {
                        object huiObj = (Material)Resources.Load("Effect/UI_Effect/Sharde/UI_Hui", typeof(Material));
                        Material huiMaterial = huiObj as Material;
                        Img_SkillIcon.GetComponent<Image>().material = huiMaterial;
                    }
                    else {
                        Img_SkillIcon.GetComponent<Image>().material = null;
                    }

                    break;

                //技能图标
                case "6":
                    if (lastSkillID != SkillID) {
                        UseSkillID = SkillID;
                        skillCDSelfTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", SkillID, "Skill_Template"));
                        skillIconID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillIcon", "ID", SkillID, "Skill_Template");
                        object obj = Resources.Load("SkillIcon/" + skillIconID, typeof(Sprite));
                        skillIcon = obj as Sprite;
                        Img_SkillIcon.GetComponent<Image>().sprite = skillIcon;
                        SkillItemNum.SetActive(false);      //不显示道具数量
                        Img_SkillIcon.GetComponent<Image>().material = null;
                    }
                    break;
            }
        }


        //特殊技能栏是没有冷却CD的
        if (SkillSpace == "0") {
            skillCDSelfTime = 0;
        }


        useMP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillUseMP", "ID", UseSkillID, "Skill_Template"));

    }

    //更新技能冷却时间（参数1 冷却时间  参数2 是否显示数字倒计时）
    private void updataSkillCDTime(float skillCDTime,bool ifShowNum,string showType) {

        //更新技能冷却UI的显示
        switch (showType) { 
            //表示自身CD
            case "0":
                //skillCDTime_Now = skillCDTime_Now + Time.deltaTime;
                skillCDTime_Now = skillCDSelfTimeSum;
                //Debug.Log("CD2 : "+skillCDTime_Now);
            break;
            //表示公共CD
            case "1":
                skillCDTime_Now = Game_PublicClassVar.Get_game_PositionVar.Rose_PublicSkillCDTimeSum;
            break;
        }
        
        //当技能处于CD中
        if (skillCDTime_Now <= skillCDTime && skillCDTime_Now != 0)
        {
            //获取当前技能冷却的累计时间
            float value = 1 - skillCDTime_Now / skillCDTime;

            if (value <= Time.deltaTime*2)
            {
                value = 0.0f;
                
                //技能冷却结束显示的特效
                //SkillCDEffect.active = true;
            }

            //更新UI值
            Obj_SkillCD.GetComponent<Image>().fillAmount = value;

            //是否显示冷却时间（数字倒计时）
            if (ifShowNum)
            {
                Text_SkillCD.SetActive(true);
                int skilltime = (int)skillCDTime - (int)skillCDTime_Now;
                Text_SkillCD.GetComponent<Text>().text = skilltime.ToString();
            }
            else {
                Text_SkillCD.SetActive(false);
            }
        }
        else
        {
            //初始化冷却CD的值
            //Debug.Log("skillCDStatus222222222222222222");
            skillCDStatus = false;
            skillCDTime_Now = 0.0f;
            Text_SkillCD.SetActive(false);
        }
    }


    //当多个触电点击到技能ICON的区域
    public bool GetTouchPosition(Rect rect) {

        //死亡状态不能触发按钮
        if (roseStatus != null)
        {
            if (roseStatus.RoseDeathStatus)
            {
                return false;
            }
        }

        //技能移动状态开启,返回false
        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            return false;
        }
        /*
        int inputNum = Input.touchCount;

        //Debug.Log("我的手指数量：" + inputNum);
        for (int i = 0; i <= inputNum - 1; i++)
        {
            //检测触碰区域在右边平且触碰点大于等于2时
            if (Input.GetTouch(i).position.x > Screen.width / 2 && inputNum>=2)
            {
                //Game_PublicClassVar.Get_function_UI.GameHint("我点击了右边区域" + Input.GetTouch(i).position.x + ";" + Input.GetTouch(i).position.y);

                if (rect.Contains(Input.GetTouch(i).position)) {
                    //Game_PublicClassVar.Get_function_UI.GameHint("我触发了点击区域" + SkillSpace);
                    //触发技能
                    cleckbutton();
                    this.GetComponent<RoseSkill_Sing_1>().ChuMoFingerId = i;
                }
            }
        }
        */
        return true;
    }

    public void OnPointUp()
    {
        if (!this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect)
            return;

        this.GetComponent<RoseSkill_Sing_1>().OnSkillEndDrag(isDrag);
    }

    public void cleckbutton()
    {
        isDrag = false;
        //Debug.Log("触发技能：" + SkillID);
        //特殊技能栏是没有冷却CD的
        if (SkillSpace != "0")
        {
            //技能触发冷却不触发任何技能
            if (skillCDStatus)
            {
                //Debug.Log("技能冷区中：" + SkillID);
                return;
            }

            //判断距离上次点击的时间
            if (dianjiStatus)
            {
                //Debug.Log("点击时间：" + SkillID);
                return;
            }

            //两次点击技能按钮的间隔时间
            if (!dianjiStatus)
            {
                
                dianjiStatus = true;
            }
            else
            {
                //Debug.Log("两次点击间隔：" + SkillID);
                return;
            }

            //判定当前是否在选择施法状态
            /*
            if (this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect == true) {
                return;
            }
            */
        }
        else {
            //此处特殊处理,要不猎人第二次技能没有buff  法师可以无限召唤骷髅 点完额外召唤的天赋
            if (UseSkillID == "60032121" || UseSkillID == "60032122"|| UseSkillID == "60032142" || UseSkillID == "60032152" || UseSkillID == "60012071" || UseSkillID == "60012072" || UseSkillID == "60012073" || UseSkillID == "60012074" || UseSkillID == "60012075" || UseSkillID == "60032171") {
                skillCDStatus = false;
            }

            //处理法师召唤
            if (UseSkillID == "60030119" || UseSkillID == "60030151" || UseSkillID == "60030152" || UseSkillID == "60030153" || UseSkillID == "60030154" || UseSkillID == "60030155"|| UseSkillID == "60030115"  || UseSkillID == "60030116") {
                skillCDStatus = false;
            }
        }

        //Debug.Log("触发技能22222：" + SkillID);

        //判断自己是否眩晕
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().XuanYunStatus && UseSkillID !=  "60030001") {
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("眩晕状态无法释放技能...");
            //Debug.Log("眩晕状态无法释放技能：" + SkillID);
            return;
        }

        //指定一些地图不能使用技能
        if (Application.loadedLevelName == "100001")
        {
            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_9");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("此地图不能使用技能！");
            return;
        }

        //魔法师需要消耗MP
        int nowSkillUseMP = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillUseMP", "ID", UseSkillID, "Skill_Template"));
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_LanValue < nowSkillUseMP) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Occupation == "2")
            {

                //提示魔法值不足
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_458");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);

            }

            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Occupation == "3")
            {

                //提示能量值不足
                //string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_458");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("能量值不足");

            }
            return;
        }
        //Debug.Log("触发技能33333：" + SkillID);

        MouseEnterStatus = true;            //设置鼠标进入状态

        //判定当前格子是否有技能,没有技能则不执行任何操作
        if (SkillID == "" || SkillID == "0")
        {
            //Debug.Log("判定当前格子是否有技能：" + SkillID);
            return;
        }

        //判定是否为道具,如果是道具数量不足则无反应
        string idFirst = SkillID.ToString().Substring(0, 1);
        if(idFirst=="1"){
            //判定道具数量是否小于1,是的直接跳出本次方法
            if(itemnNum<1){
                //Debug.Log("判定道具数量是否小于1：" + SkillID);
                return ;
            }
        }

        //检测当前是否处于技能吟唱状态
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseSingStopStatus = true;

            string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_10");
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
            //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能施法状态中断！");
        }

        //判定技能是否为选定目标释放的
        string damgeRangeType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRangeType", "ID", UseSkillID, "Skill_Template");
        string damgeRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRange", "ID", UseSkillID, "Skill_Template");
        GameObject actObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget;

        //目标立即释放
        if (damgeRangeType == "3") {

            //如果当前没有目标会自动设置一个攻击目标
            if (actObj == null) {

                Rose_Status roseStatus = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
                if (roseStatus.Obj_ActTarget == null && roseStatus.NextAutomaticMonsterObj != null) {
                    if (roseStatus.NextAutomaticMonsterDis <= 12) {
                        roseStatus.Obj_ActTarget = roseStatus.NextAutomaticMonsterObj;
                        actObj = roseStatus.Obj_ActTarget;
                        actObj.GetComponent<AI_1>().AI_Selected_Status = true;
                        //开启更新怪物选中状态
                        Game_PublicClassVar.Get_game_PositionVar.UpdataSelectedStatus = true;
                    }
                }
            }

            //判定当前是否指定了目标
            if (actObj == null) {

                //获取与当前目标的距离,超过距离施法失败
                /*
                float disValue = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeRange", "ID", UseSkillID, "Skill_Template"));
                if (Vector3.Distance(actObj.transform.position, Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position) >= disValue)
                {
                    //Debug.Log("立即释放技能,超过攻击范围,释放失败！");
                    return;         //直接跳出方法,后面不执行
                }
                */

                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_11");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你需要选择攻击目标");
                return;
            }
        }

        //Debug.Log("触发技能44444：" + SkillID);

        //目标立即释放,有施法距离
        if (damgeRangeType == "4")
        {
            if (actObj != null)
            {
                float dis = Vector3.Distance(game_PositionVar.Obj_Rose.transform.position, actObj.transform.position);
                if (dis > float.Parse(damgeRange))
                {

                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_12");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你需要离的更近一点");
                    return;
                }
            }
            else
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_296");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你需要选择攻击目标");
                return;
            }
        }

        //Debug.Log("释放技能111111111:" + SkillID);
        if (!skillCDStatus) {

            //Debug.Log("释放技能成功:" + SkillID);

            skillCDStatus = true;
            mSkillIndicator = Vector2.zero;

            //执行技能

            this.GetComponent<RoseSkill_Sing_1>().enabled = true;

            
            //获取当前技能是否需要吟唱
            ifSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", UseSkillID, "Skill_Template");
            this.GetComponent<RoseSkill_Sing_1>().SelectSkillRangeType = ifSkillRange;

            //立即释放
            if (ifSkillRange == "0") {
                this.GetComponent<RoseSkill_Sing_1>().OnIfSkillSelect (false );
            }

            //圆圈指示
            if (ifSkillRange == "1") {
                this.GetComponent<RoseSkill_Sing_1>().OnIfSkillSelect(true);
            }

            //直线箭头指示
            if (ifSkillRange == "2")
            {
                InitZhiShiTarget();
                this.GetComponent<RoseSkill_Sing_1>().OnIfSkillSelect(true);
                roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().Show_SkillDir();
                roseStatus.Obj_RoseSkillZhiShi.SetActive(true);
            }

            //60°指示
            if (ifSkillRange == "3")
            {
                InitZhiShiTarget();
                this.GetComponent<RoseSkill_Sing_1>().OnIfSkillSelect(true);
                roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().Show_SkillArea60r();
                roseStatus.Obj_RoseSkillZhiShi.SetActive(true);

            }

            //120°指示
            if (ifSkillRange == "4")
            {
                InitZhiShiTarget();
                this.GetComponent<RoseSkill_Sing_1>().OnIfSkillSelect(true);
                roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().Show_SkillArea120r();
                roseStatus.Obj_RoseSkillZhiShi.SetActive(true);
            }

            //设置触摸ID
            this.GetComponent<RoseSkill_Sing_1>().ChuMoFingerId = Input.touchCount-1;


            //根据主角的专注属性,重新获取技能冷却CD
            if (Random.value < Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ZhuanZhuPro)
            {
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_17");
                Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("技能触发专注!");
                skillCDSelfTime = 0;
            }
            else {
                skillCDSelfTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillCD", "ID", UseSkillID, "Skill_Template"));

                //技能附加值,减少冷却CD
                int skillAddValue_skillCD = 0;
                string skillAddValueStr = Game_PublicClassVar.Get_function_Skill.GetSkillAddValue(UseSkillID, "4");
                if (skillAddValueStr != "" && skillAddValueStr != "0")
                {
                    string[] skillAddValueList = skillAddValueStr.Split(',');
                    for (int y = 0; y < skillAddValueList.Length; y++)
                    {
                        skillAddValue_skillCD = skillAddValue_skillCD + int.Parse(skillAddValueList[y]);
                    }
                }

                skillCDSelfTime = skillCDSelfTime - skillAddValue_skillCD;
                if (skillCDSelfTime <= 1)
                {
                    skillCDSelfTime = 1;
                }
            }
        }
    }

    private void InitZhiShiTarget() {

        //技能指示器在未移动的时候一直表示玩家的正前方
        Vector3 SelectRangeEffect;
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget)
        {
            SelectRangeEffect = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.transform.TransformPoint(0, 0, 0);
        }
        else
        {
            SelectRangeEffect = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_RoseModel.transform.TransformPoint(0, 0, 1);  //此代码表示技能指示为最前方
        }

        roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().InitSkillTarget(new Vector3(SelectRangeEffect.x, SelectRangeEffect.y, SelectRangeEffect.z ));
        roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().UseSkillDir(roseStatus.Obj_RoseSkillZhiShi.GetComponent<RoseSkillZhiShi>().SkillTarget);

    }

    //Icon拖拽（开始）
    public void StarDrag()
    {
        /*
        Debug.Log("开始拖拽"+SkillID);
        Game_PublicClassVar.Get_game_PositionVar.UI_SkillMove_Initial = this.gameObject;
        Game_PublicClassVar.Get_game_PositionVar.UI_SkillMoveStatus = true;

        //实例化图标
        mouseMoveSkillIcon = (GameObject)Instantiate(MouseMoveSkillIcon);
        mouseMoveSkillIcon.transform.parent = this.transform.parent.transform;
        //显示图标
        skillIconID = Game_PublicClassVar.Get_xmlScript.Xml_GetDate("SkillIcon", "ID", SkillID, Game_PublicClassVar.Get_game_PositionVar.Xml_Path + "Skill_Template.xml");
        object obj = Resources.Load("SkillIcon/" + skillIconID, typeof(Sprite));
        skillIcon = obj as Sprite;
        mouseMoveSkillIcon.transform.Find("SkillIcon").GetComponent<Image>().sprite = skillIcon;
        //图标位置
        mouseMoveSkillIcon.transform.position = Input.mousePosition;
        mouseMoveSkillIcon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        */
    }

    //Icon拖拽（结束）
    public void EndDrag()
    {
        if (!this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect)
            return;

        this.GetComponent<RoseSkill_Sing_1>().OnSkillEndDrag(true);

        /*
        Debug.Log("结束拖拽" + SkillID);
        if (Game_PublicClassVar.Get_game_PositionVar.UI_SkillMoveStatus)
        {

            if (Game_PublicClassVar.Get_game_PositionVar.UI_SkillMove_End != null)
            {

                //更新图标
                Game_PublicClassVar.Get_game_PositionVar.Rose_MoveSkillIconStatus = true;

                //执行交换
                Game_PublicClassVar.Get_function_UI.UI_SkillIconMove();

            }
            else {
                //清空交换数据
                //Game_PublicClassVar.Get_game_PositionVar.UI_SkillMove_Initial = null;
                //Game_PublicClassVar.Get_game_PositionVar.UI_SkillMoveStatus = false;
            }

            skillIconDragStatus = true;

        }

        //销毁移动图标
        Destroy(mouseMoveSkillIcon);
        */
    }

    //鼠标进入Icon（结束）
    public void Enter()
    {

        if (Game_PublicClassVar.Get_game_PositionVar.SkillMoveStatus) {
            Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = SkillSpace;
            //Debug.Log("进来了");

            if (skillCDStatus) {
                Game_PublicClassVar.Get_game_PositionVar.SkillMoveValue_End = "";
                string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_343");
                Game_PublicClassVar.Get_function_UI.GameHint(langStrHint);
                //Game_PublicClassVar.Get_function_UI.GameHint("请等待冷却时间结束");
                return;
            }

        }

        //如果移动装备开关打开放入对应的装备
        if (Game_PublicClassVar.Get_game_PositionVar.ItemMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveType_End = "";
            Game_PublicClassVar.Get_game_PositionVar.ItemMoveValue_End = "";
            //触发移动
            //Game_PublicClassVar.Get_function_UI.UI_ItemMouseMove();
        }
    }

    //鼠标移出
    public void Exit() {

        //Debug.Log("鼠标移出"+SkillID);
        //MouseEnterStatus = false;           //设置鼠标离开状态
        exitStatus = true;
        /*
        if (Game_PublicClassVar.Get_game_PositionVar.UI_SkillMoveStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.UI_SkillMove_End = null;
        }

        //注销技能Tips
        Destroy(skillTipsObj);
        */
    }

    public Vector2 GetSkillIndicator()
    {
        return mSkillIndicator;
    }

    public void drag(BaseEventData eventData)
    {
        isDrag = true;
        if (!this.GetComponent<RoseSkill_Sing_1>().IfSkillSelect)
            return;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Btn_SkillStart.GetComponent<RectTransform>(),
            (eventData as PointerEventData).position, null, out mSkillIndicator);
        //float length = mSkillOffset.sqrMagnitude;

        if (exitStatus)
        {
            if (MouseEnterStatus)
            {
                //Debug.Log ("sssss1111");
                MouseEnterStatus = false;
            }
            else
            {
                //Debug.Log ("ssssss2222");
            }
            exitStatus = false;
        }

        if (ifSkillRange=="") {
            ifSkillRange = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("IfSelectSkillRange", "ID", UseSkillID, "Skill_Template");
        }


    }

}
