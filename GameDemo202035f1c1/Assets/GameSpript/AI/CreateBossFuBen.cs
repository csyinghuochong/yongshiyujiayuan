using UnityEngine;
using System.Collections;

public class CreateBossFuBen : MonoBehaviour {

    public GameObject[] MonsterBossListSet;
    private int CreateBossXuHaoValue;
    //public Transform MonsterPosition;
    private GameObject monsterNow;

    public Transform monsterCreatePosition;
    public bool CreateBossStatus;
    private float CreateBossTime;

    private int CreateBossNum;

    private int roseLv;
    private int BaseAct;
    private int BaseHp;

    private bool endFuBenStatus;
	// Use this for initialization
	void Start () {

        roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        //roseLv = 65;

        //当前创建怪物的序列ID
        if (roseLv >= 1)
        {
            CreateBossXuHaoValue = 5;
        }

        if (roseLv >= 18)
        {
            CreateBossXuHaoValue = 9;
        }

        if (roseLv >= 30)
        {
            CreateBossXuHaoValue = 13;
        }

        if (roseLv >= 40)
        {
            CreateBossXuHaoValue = 17;
        }

        if (roseLv >= 50)
        {
            CreateBossXuHaoValue = 22;
        }


        CreateBossNum = 0;
        CreateBoss();
	}
	
	// Update is called once per frame
	void Update () {

        //检测当前BOSS是否死亡
        if (!endFuBenStatus)
        {
            if (CreateBossTime <= 0) {
                if (monsterNow != null)
                {
                    if (monsterNow.GetComponent<AI_1>().AI_Status == "5")
                    {
                       //设置一定时间刷新第二个BOSS
                        CreateBossTime = 5.0f;

                        string langStrHint_1 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_203");
                        string langStrHint_2 = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_204");
                        Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint_1 + CreateBossTime + langStrHint_2);
                        //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你击杀副本BOSS成功!" + CreateBossTime + "秒后刷新下一只BOSS,请做好准备！");
                        //设置Boss
                        int KillBoss = PlayerPrefs.GetInt("FuBenKillBossNum");
                        KillBoss = KillBoss + 1;
                        PlayerPrefs.SetInt("FuBenKillBossNum", KillBoss);

                        //boss掉落
                        bool dropStatus = false;
                        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

                        //1级掉落
                        if (!dropStatus) {
                            if (roseLv >= 1)
                            {
                                switch (CreateBossNum)
                                {
                                    case 1:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50041012", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 2:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50041013", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 3:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50041014", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        //Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070201", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 4:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50041015", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070101", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 5:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50041015", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070101", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;
                                }

                                dropStatus = true;
                            }
                        }

                        //20级掉落
                        if (!dropStatus)
                        {
                            if (roseLv >= 18)
                            {
                                switch (CreateBossNum)
                                {
                                    case 1:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50002301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 2:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50002401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 3:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50002401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        //Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070201", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 4:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50002501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070201", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 5:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50002501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070201", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;
                                }

                                dropStatus = true;
                            }
                        }

                        //30级掉落
                        if (!dropStatus)
                        {
                            if (roseLv >= 30)
                            {
                                switch (CreateBossNum)
                                {
                                    case 1:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50003301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 2:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50003401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 3:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50003401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        //Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 4:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50003501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 5:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50003501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;
                                }
                                dropStatus = true;
                            }
                        }

                        //40级掉落
                        if (!dropStatus)
                        {
                            if (roseLv >= 40)
                            {
                                switch (CreateBossNum)
                                {
                                    case 1:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50004301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 2:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50004401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 3:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50004401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        //Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 4:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50004501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 5:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50004501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;
                                }
                                dropStatus = true;
                            }
                        }


                        //50级掉落
                        if (!dropStatus)
                        {
                            if (roseLv >= 50)
                            {
                                switch (CreateBossNum)
                                {
                                    case 1:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50005301", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 2:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50005401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 3:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50005401", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        //Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 4:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50005501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;

                                    case 5:
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50005501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        Game_PublicClassVar.Get_function_AI.DropIDToDropItem("50070501", monsterNow.transform.position, monsterNow.GetComponent<AI_Property>().AI_ID);
                                        break;
                                }
                                dropStatus = true;
                            }
                        }
                    }
                }
            }

            if (CreateBossTime > 0) {
                CreateBossTime = CreateBossTime - Time.deltaTime;
                if (CreateBossTime <= 0) {
                    CreateBossTime = 0;
                    CreateBossStatus = true;
                }
            }

            if (CreateBossStatus) {
                CreateBossStatus = false;
                CreateBoss();
            }

            //当前BOSS怪物属性更新完成开始刷新其当前属性
            if (monsterNow.GetComponent<AI_1>().AI_UpdateProStatus) {

                monsterNow.GetComponent<AI_1>().AI_UpdateProStatus = false;

                //设置当前BOSS属性
                switch (CreateBossNum)
                {
                    case 1:
                        BaseAct = (int)(BaseAct * 0.8f);
                        BaseHp = (int)(BaseHp * 1f);
                        break;

                    case 2:

                        BaseAct = (int)(BaseAct * 1.5f);
                        BaseHp = (int)(BaseHp * 1.5f);
                    
                        break;

                    case 3:

                        BaseAct = (int)(BaseAct * 2);
                        BaseHp = (int)(BaseHp * 2.5f);

                        break;

                    case 4:

                        BaseAct = (int)(BaseAct * 3);
                        BaseHp = (int)(BaseHp * 4f);

                        break;

                    case 5:

                        BaseAct = (int)(BaseAct * 3);
                        BaseHp = (int)(BaseHp * 6);

                        break;
                }

                //Debug.Log("BaseAct = " + BaseAct);
                monsterNow.GetComponent<AI_Property>().AI_Act = BaseAct;
                monsterNow.GetComponent<AI_Property>().AI_HpMax = BaseHp;
                monsterNow.GetComponent<AI_Property>().AI_Hp = BaseHp;
                

                //清空其唯一刷新id
                monsterNow.GetComponent<AI_1>().AI_ID_Only = "0";
                //设置当前怪物的掉落失效
                monsterNow.GetComponent<AI_1>().dropStatus = true;
                //设置其当前刷新时间很长
                monsterNow.GetComponent<AI_1>().monsterDestoryTime = 100000;
                monsterNow.GetComponent<AI_1>().monsterRebirthTime = 999999;
                //设置脱战距离
                monsterNow.GetComponent<AI_1>().ai_chaseRange = 100;

                //设置怪物当前的等级
                int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
                monsterNow.GetComponent<AI_Property>().AI_Lv = roseLv;

                //

            }
        }
	}

    //创建一个怪物
    public void CreateBoss() {

        //判定当前是否已经击杀完BOSS;
        if (CreateBossNum >= 5) {

            endFuBenStatus = true;
            //Game_PublicClassVar.Get_function_UI.GameGirdHint("实力证明！恭喜你今天已经完成5连斩的击杀！");
            return;
        }

        //获取刷新怪物的序号
        int bossXuHaoID = (int)(Random.value * CreateBossXuHaoValue-0.0001f);
        //Debug.Log("bossXuHaoID = " + bossXuHaoID);

        //四舍五入
        if (bossXuHaoID >= CreateBossXuHaoValue)
        {
            bossXuHaoID = bossXuHaoID - 1;
        }
        if (bossXuHaoID < 0) {
            bossXuHaoID = 0;
        }

        //Debug.Log("CreateBossXuHaoValue = " + CreateBossXuHaoValue);
        //刷新怪物
        monsterNow = (GameObject)Instantiate(MonsterBossListSet[bossXuHaoID]);
        //设置怪物的父级和坐标点
        monsterNow.transform.SetParent(monsterCreatePosition);
        monsterNow.transform.localPosition = new Vector3(0,1,0);
        //CreateVec3,new Quaternion(0,0,0,0));
        monsterNow.SetActive(false);
        monsterNow.SetActive(true);


        CreateBossNum = CreateBossNum + 1;
        //Debug.Log("当前创建怪物数量：" + CreateBossNum);


        //设置当前攻击和血量

        BaseAct = 164;
        BaseHp = 7000;

        if (roseLv >= 20)
        {

            BaseAct = 110;
            BaseHp = 7000;
        }

        if (roseLv >= 30)
        {
            BaseAct = 150;
            BaseHp = 12000;
        }

        if (roseLv >= 40)
        {
            BaseAct = 180;
            BaseHp = 20000;
        }

        if (roseLv >= 50)
        {
            BaseAct = 240;
            BaseHp = 35000;
        }

        /*
        for (int i = 0; i <= CreateBossXuHaoValue; i++)
        {



        }
        */
    }
}
