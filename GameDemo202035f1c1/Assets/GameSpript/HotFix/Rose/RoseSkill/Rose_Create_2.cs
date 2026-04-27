using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//召唤怪物技能,召唤时属性取玩家的当前属性
public class Rose_Create_2 : MonoBehaviour
{

	private GameObject SkillEffect;     //技能特效
	//private GameObject skillEffect;   //脚本中实例化的特效ID
    public float SkillTime;             //技能特效播放时间
    private float delayTime;            //延迟时间
    private float delayTimeSum;         //延迟时间累计
    private int zhaohuanNum;            //召唤数量
    private bool zhaoHuanStatus;        //召唤状态
    private int zhaoHuanTime;           //召唤时间
    private Dictionary<int, GameObject> dicCreateList = new Dictionary<int, GameObject>();

    //绑点专用
    private Game_PositionVar game_PositionVar;
    private Object monsterObj;

	// Use this for initialization
	void Start () {

        //获取绑点
        /*
        game_PositionVar = Game_PublicClassVar.Get_game_PositionVar;
        Rose_Proprety rose_Proprety = game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>();
        
        //获取治疗值
        startAddHpValue = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("DamgeValue","ID",this.GetComponent<SkillObjBase>().SkillID,"Skill_Template"));

        Game_PublicClassVar.Get_function_MonsterSkill.MonsterAddHp(this.gameObject.transform.parent.gameObject, this.GetComponent<SkillObjBase>().SkillID);
        */
        //获取特效名称
        //string effectName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");


        //Debug.Log("召唤技能ID：" + this.GetComponent<SkillObjBase>().SkillID);

        //播放召唤特效
        string EffectNameStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        if (EffectNameStr != "" && EffectNameStr != "0")
        {
            GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + EffectNameStr, typeof(GameObject));
            GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);

            //SkillObject_p.transform.position = monsterObj.transform.position;
            SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
            SkillObject_p.transform.position = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
        }



        //获取技能时间
        //SkillTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillLiveTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        SkillTime = this.GetComponent<SkillObjBase>().SkillLiveTime;
        delayTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillDelayTime", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template"));
        
        //播放施法动作
        //________Game_PublicClassVar.Get_game_PositionVar.Rose_IfSkill = true;

	}
	
	// Update is called once per frame
	void Update () {

        //延迟召唤
        delayTimeSum = delayTimeSum + Time.deltaTime;
        if (delayTimeSum > delayTime&& zhaoHuanStatus==false) {
            createMonster();
            zhaoHuanStatus = true;
        }

        if (zhaoHuanStatus) {
            zhaoHuanTime = zhaoHuanTime + 1;
            if (zhaoHuanTime >= 2) {

                
                //属性,确保下一帧在执行
                if (dicCreateList != null) {
                    foreach (GameObject nowObj in dicCreateList.Values)
                    {
                        if (nowObj != null)
                        {
                            /*
                            //更新属性
                            nowObj.GetComponent<AI_Property>().base_Act = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Act;
                            nowObj.GetComponent<AI_Property>().base_HpMax = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Hp;
                            nowObj.GetComponent<AI_Property>().base_Def = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Def;
                            nowObj.GetComponent<AI_Property>().base_Adf = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Adf;
                            nowObj.GetComponent<AI_Property>().AI_Lv = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Lv;
                            */
                            nowObj.GetComponent<AI_Property>().UpdataAIPropertyStatus = true;
                        }
                    }
                }
                
                //注销脚本
                Destroy(this.gameObject);
                zhaoHuanStatus = false;
            }

        }

	}

    void createMonster()
    {
        //Debug.Log("触发召唤RoseCreate_1");
        //获取召唤列表
        string gameObjectParameter = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
        string ifXiangTong = gameObjectParameter.Split('|')[0];
        //gameObjectParameter = "1001,1";
        string[] createList = gameObjectParameter.Split('|')[1].Split(';');
        for (int i = 0; i <= createList.Length - 1; i++)
        {

            string createID = createList[i].Split(',')[0];
            int createNum = int.Parse(createList[i].Split(',')[1]);
            float addProValue = float.Parse(createList[i].Split(',')[2]);
            float addProHpValue = float.Parse(createList[i].Split(',')[3]);

            for (int x = 0; x <= createNum - 1; x++)
            {
                //获取怪物并设置位置
                GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet;
                string createName = createID;

                //判定当前宠物是否已经召唤
                if (ifXiangTong == "0") {

                    for (int y = 0; y < monsterSetObj.transform.childCount; y++)
                    {

                        GameObject go = monsterSetObj.transform.GetChild(y).gameObject;
                        string name = go.name.Replace("(Clone)", "");
                        if (name == createName)
                        {
                            if (go.GetComponent<AI_Property>() != null) {
                                if (go.GetComponent<AI_Property>().AI_Hp > 0) {
                                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_220");
                                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                                    //Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("你已经召唤此怪物！");
                                    return;
                                }
                            }
                        }

                        //特殊处理（熊升级的时候替换）
                        /*
                        string tianFuNowLv = Game_PublicClassVar.Get_function_UI.RetrunTianFuNowLv("318101");
                        if (tianFuNowLv == "-1"&& this.GetComponent<SkillObjBase>().SkillID.Contains("6001201"))
                        {
                            Debug.Log("111111111111111");
                            if (name == "70009301" || name == "70009302" || name == "70009303" || name == "70009304" || name == "70009305")
                            {
                                if (createName == "70009301" || createName == "70009302" || createName == "70009303" || createName == "70009304" || createName == "70009305") {
                                    //Debug.Log("召唤杀死怪物  name = " + name + "createName = " + createName);
                                    go.GetComponent<AI_Property>().AI_Hp = 0;
                                }
                            }
                        }
                        */

                        if (this.GetComponent<SkillObjBase>().SkillID.Contains("6001201"))
                        {
                            Debug.Log("111111111111111");
                            if (name == "70009301" || name == "70009302" || name == "70009303" || name == "70009304" || name == "70009305")
                            {
                                if (createName == "70009301" || createName == "70009302" || createName == "70009303" || createName == "70009304" || createName == "70009305")
                                {
                                    //Debug.Log("召唤杀死怪物  name = " + name + "createName = " + createName);
                                    go.GetComponent<AI_Property>().AI_Hp = 0;
                                }
                            }
                        }
                    }
                }

                //Debug.Log("createName = " + createName);
                //角色属性
                GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + createName, typeof(GameObject)));
                monsterObj.transform.SetParent(monsterSetObj.transform);
                Vector3 Vec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                Vec3 = new Vector3(Vec3.x + Random.value, Vec3.y + 0.5f, Vec3.z + Random.value);
                //Debug.Log("召唤AAAAVec3 = " + Vec3);

                //如果是被动技能召唤,则创建于当前的目标点（表现形式好看）
                string skillType = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SkillType", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (skillType == "2") {
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget != null) {
                        Vec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().Obj_ActTarget.transform.position;
                        //Debug.Log("召唤BBBBVec3 = " + Vec3);
                        //Vec3 = Vector3.zero;
                    }
                }

                monsterObj.transform.position = Vec3;
                monsterObj.SetActive(false);
                monsterObj.SetActive(true);
                Game_PublicClassVar.Get_function_Rose.addRosePetPositionObj(monsterObj);
                monsterObj.GetComponent<AIPet>().PetType = "1";
                monsterObj.GetComponent<AIPet>().AILiveTime = SkillTime;
                monsterObj.GetComponent<AIPet>().PositionObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
                monsterObj.GetComponent<AIPet>().IfSkillCreatePet_ProTeShu = true;

                //设置属性比例
                monsterObj.GetComponent<AI_Property>().AI_SummonPropertyPro = addProValue * (1 + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SummonAIPropertyAddPro);
                monsterObj.GetComponent<AI_Property>().AI_SummonPropertyHpPro = addProHpValue * (1 + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SummonAIPropertyAddPro + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SummonAIHpPropertyAddPro);
                monsterObj.GetComponent<AI_Property>().AI_SummonPropertyActPro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SummonAIActPropertyAddPro;
                monsterObj.GetComponent<AI_Property>().AI_SummonPropertyDefPro = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_SummonAIDefPropertyAddPro;
                //monsterObj.GetComponent<AI_Property>().AI_Hp = monsterObj.GetComponent<AI_Property>().AI_HpMax;
                zhaohuanNum = zhaohuanNum + 1;
                dicCreateList.Add(zhaohuanNum, monsterObj);

                //播放召唤特效
                string EffectNameStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HitEffectName", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template");
                if (EffectNameStr == "" || EffectNameStr == "0") {
                    EffectNameStr = "Eff_Skill_ZhaoHuan_3";
                }

                GameObject SkillObj = (GameObject)Resources.Load("Effect/Skill/" + EffectNameStr, typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);

                //SkillObject_p.transform.position = monsterObj.transform.position;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
                SkillObject_p.transform.SetParent(monsterObj.transform);
                SkillObject_p.transform.localPosition = new Vector3(0, 0, 0);

                Destroy(SkillObject_p, 2);

            }
        }
    }


    /*
    void createMonster() {

        //获取召唤列表
        string[] createList = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("GameObjectParameter", "ID", this.GetComponent<SkillObjBase>().SkillID, "Skill_Template").Split(';');
        for (int i = 0; i <= createList.Length - 1; i++)
        {
            string createID = createList[i].Split(',')[0];
            int createNum = int.Parse(createList[i].Split(',')[1]);
            for (int x = 0; x <= createNum - 1; x++)
            {
                //获取怪物并设置位置
                GameObject monsterSetObj = Game_PublicClassVar.Get_game_PositionVar.Obj_RoseCreatePetSet;
                GameObject monsterObj = MonoBehaviour.Instantiate((GameObject)Resources.Load("PetSet/" + "PetObj_1", typeof(GameObject)));
                monsterObj.transform.SetParent(monsterSetObj.transform);
                Vector3 Vec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                Vec3 = new Vector3(Vec3.x + Random.value, Vec3.y + 0.5f, Vec3.z + Random.value);
                //Vector3 CreateVec3 = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.transform.position;
                monsterObj.transform.position = Vec3;
                monsterObj.SetActive(false);
                monsterObj.SetActive(true);
                Game_PublicClassVar.Get_function_Rose.addRosePetPositionObj(monsterObj);
                zhaohuanNum = zhaohuanNum + 1;
                dicCreateList.Add(zhaohuanNum, monsterObj);

                //播放传送特效
                GameObject SkillObj = (GameObject)Resources.Load("Effect/Rose/" + "Rose_MoveScene", typeof(GameObject));
                GameObject SkillObject_p = (GameObject)Instantiate(SkillObj);
                GameObject effectObj = (GameObject)Instantiate((GameObject)Resources.Load("Effect/Skill/Rose_CriAct"));
                SkillObject_p.transform.position = Vec3;
                SkillObject_p.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
    */
}
