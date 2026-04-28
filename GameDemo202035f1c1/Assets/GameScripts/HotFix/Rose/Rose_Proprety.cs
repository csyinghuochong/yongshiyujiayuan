using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class Rose_Proprety : MonoBehaviour {

    //定义角色属性，方便其他脚本调用
    public ObscuredString Rose_Name;                            //名称
    public ObscuredString Rose_Occupation;                      //职业
    public ObscuredInt Rose_Lv;                                 //角色等级
    public ObscuredBool Rose_GetExp;                            //角色每获得一次经验打开此开关
    public ObscuredLong Rose_ExpNow;                     //角色当前等级经验
    public ObscuredLong Rose_Exp;                        //角色当前升级需要经验
    public ObscuredInt Rose_Hp;                         //角色拥有血量
    public ObscuredInt Rose_HpNow;                      //角色当前血量               
    public ObscuredInt Rose_ActMin;                     //角色最小攻击
    public ObscuredInt Rose_ActMax;                     //角色最大攻击
    public ObscuredInt Rose_MagActMin;                  //角色最小魔攻
    public ObscuredInt Rose_MagActMax;                  //角色最大魔攻
    public ObscuredInt Rose_DefMin;                     //角色最小物防
    public ObscuredInt Rose_DefMax;                     //角色最大物防
    public ObscuredInt Rose_AdfMin;                     //角色最小魔防
    public ObscuredInt Rose_AdfMax;                     //角色最大魔防
    public ObscuredFloat Rose_HealHpValuePro;           //角色恢复最大生命的比例
    public ObscuredInt Rose_HealHpValue;                //角色恢复的固定值
    public ObscuredFloat Rose_HealHpPro;                //角色恢复的百分比加成固定值
    public ObscuredFloat Rose_HealHpFightPro;           //角色战斗时恢复额外恢复血量的百分比
    public ObscuredFloat Rose_HealHpTime;               //角色2次恢复生命的时间
    private ObscuredFloat rose_HealHpTimeSum;
    public ObscuredInt Rose_LanValue;                       //魔法是蓝量
    public ObscuredInt Rose_LanValueMax;                    //魔法是蓝量上限
    public ObscuredInt Rose_GeDangValue;                    //格挡值
    public ObscuredFloat Rose_ZhongJiPro;                   //重击概率
    public ObscuredInt Rose_ZhongJiValue;                   //重击附加伤害值
    public ObscuredInt Rose_GuDingValue;                    //每次普通攻击附加的伤害值
    public ObscuredInt Rose_HuShiDefValue;                  //忽视目标防御值                       
    public ObscuredInt Rose_HuShiAdfValue;                  //忽视目标魔防值
    public ObscuredFloat Rose_HuShiDefValuePro;             //忽视目标防御值_百分比                       
    public ObscuredFloat Rose_HuShiAdfValuePro;             //忽视目标魔防值_百分比
    public ObscuredFloat Rose_XiXuePro;                     //吸血百分比

    public ObscuredInt Rose_CriRating;                     //暴击等级
    public ObscuredInt Rose_ResilienceRating;              //韧性等级
    public ObscuredInt Rose_HitRating;                     //命中等级
    public ObscuredInt Rose_DodgeRating;                   //闪避等级
    public ObscuredFloat Rose_MagicRebound;                //法术反击值
    public ObscuredFloat Rose_ActRebound;                  //攻击反击
    public ObscuredFloat Rose_Resistance_1;                //光抗性
    public ObscuredFloat Rose_Resistance_2;                //暗抗性
    public ObscuredFloat Rose_Resistance_3;                //火抗性
    public ObscuredFloat Rose_Resistance_4;                //水抗性
    public ObscuredFloat Rose_Resistance_5;                //电抗性
    public ObscuredFloat Rose_RaceResistance_1;            //野兽攻击抗性
    public ObscuredFloat Rose_RaceResistance_2;            //人物攻击抗性
    public ObscuredFloat Rose_RaceResistance_3;            //恶魔攻击抗性
	public ObscuredFloat Rose_RaceDamge_1;            		//攻击野兽加成
	public ObscuredFloat Rose_RaceDamge_2;            		//攻击人物加成
	public ObscuredFloat Rose_RaceDamge_3;            		//攻击恶魔加成
    public ObscuredInt Rose_Exp_AddValue;                   //经验加成固定值
    public ObscuredFloat Rose_Exp_AddPro;                   //经验加成百分比
    public ObscuredInt Rose_Gold_AddValue;                  //金币加成固定值
    public ObscuredFloat Rose_Gold_AddPro;                  //金币加成百分比

    public ObscuredFloat Rose_Blessing_AddPro;          	//洗炼极品掉落
    public ObscuredFloat Rose_HidePro_AddPro;           	//隐藏属性出现概率
    public ObscuredFloat Rose_GemHole_AddPro;           	//装备上的宝石槽位出现概率
    //public ObscuredInt Rose_XilianLucky_AddValue;         //洗炼幸运值,洗炼幸运值越高出好的属性概率越高,出幸运属性越高

    public ObscuredInt Rose_YaoJiShuLian_Value;             //药剂类熟练度
    public ObscuredInt Rose_DuanZuaoShuLian_Value;          //锻造类熟练度
    private Rose_Status rose_Status;

    public ObscuredInt Rose_Act;                        //角色攻击
    public ObscuredInt Rose_MagAct;                     //角色魔法攻击
    public ObscuredInt Rose_Def;                        //角色防御
    public ObscuredInt Rose_Adf;                        //角色魔防
    public ObscuredFloat Rose_Cri;                      //角色暴击
    public ObscuredFloat Rose_Res;                      //韧性值
    public ObscuredFloat Rose_Hit;                      //角色命中
    public ObscuredFloat Rose_Dodge;                    //角色闪避
    public ObscuredFloat Rose_DefAdd;                   //角色物理免伤
    public ObscuredFloat Rose_AdfAdd;                   //角色魔法免伤
    public ObscuredFloat Rose_DamgeSub;                 //角色免伤值
    public ObscuredFloat Rose_DamgeAdd;                 //角色伤害加成
    public ObscuredFloat Rose_Lucky;                    //角色幸运值
    public ObscuredFloat Rose_MoveSpeed;                //角色移动速度
    public ObscuredFloat Rose_Boss_ActAdd;                          //Boss普通攻击加成
    public ObscuredFloat Rose_Boss_SkillAdd;                        //Boss技能攻击加成
    public ObscuredFloat Rose_Boss_ActHitCost;                      //受到Boss普通攻击减免
    public ObscuredFloat Rose_Boss_SkillHitCost;                    //受到Boss技能攻击减免
    public ObscuredFloat Rose_PetActAdd;                            //宠物攻击加成
    public ObscuredFloat Rose_PetActHitCost;                        //宠物受伤减免
    public ObscuredFloat Rose_SkillCDTimePro;                       //技能冷却时间缩减
    public ObscuredFloat Rose_BuffTimeAddPro;                       //自身buff效果延长
    public ObscuredFloat Rose_DeBuffTimeCostPro;                    //Debuff时间缩短
    public ObscuredFloat Rose_DodgeAddHpPro;                        //闪避恢复血量
    public ObscuredFloat Rose_SingTimePro;                          //吟唱时间缩减

    //被动技能附加属性
    public ObscuredFloat Rose_FuHuoPro;						//复活概率
    public ObscuredFloat Rose_ActWuShi;						//攻击无视防御
    public ObscuredFloat Rose_ShenNong;						//神农
    public ObscuredFloat Rose_DropExtra;					//额外掉落
    public ObscuredFloat Rose_WeiZhuang;					//伪装  +增大发现范围   -缩小范围
    public ObscuredFloat Rose_ZaiNanValue;					//灾难
    public ObscuredFloat Rose_ShiXuePro;					//嗜血概率
    public ObscuredFloat Rose_AITuoZhanDisValue;			//怪物脱战距离
    public ObscuredFloat Rose_ZhuanZhuPro;					//专注概率
    public ObscuredFloat Rose_BiZhongPro;                   //必中概率
    public ObscuredFloat Rose_YaoJiCirPro;                  //生产药剂暴击概率
    public ObscuredFloat Rose_BuZhuoPro;                    //捕捉怪物概率
    public ObscuredFloat Rose_ChouHenValue;                    //仇恨值

    public ObscuredFloat Rose_LanValueMaxAdd;                       //魔法量附加
    public ObscuredFloat Rose_SummonAIPropertyAddPro;               //召唤生物属性加成
    public ObscuredFloat Rose_HuDunValueAddPro;                     //护盾属性附加
    public ObscuredFloat Rose_ActAddPro;                            //普通攻击技能加成

    public ObscuredFloat Rose_SummonAIHpPropertyAddPro;             //召唤生物血量属性加成
    public ObscuredFloat Rose_SummonAIActPropertyAddPro;            //召唤生物攻击属性加成
    public ObscuredFloat Rose_SummonAIDefPropertyAddPro;            //召唤生物防御属性加成

    //基础值(未加buff之前的属性总值)
    public ObscuredInt Rose_Hp_PropertySum;                         //角色拥有血量             
    public ObscuredInt Rose_ActMin_PropertySum;                     //角色最小攻击
    public ObscuredInt Rose_ActMax_PropertySum;                     //角色最大攻击
    public ObscuredInt Rose_MagActMin_PropertySum;                  //角色最小魔攻
    public ObscuredInt Rose_MagActMax_PropertySum;                  //角色最大魔攻
    public ObscuredInt Rose_DefMin_PropertySum;                     //角色最小物防
    public ObscuredInt Rose_DefMax_PropertySum;                     //角色最大物防
    public ObscuredInt Rose_AdfMin_PropertySum;                     //角色最小魔防
    public ObscuredInt Rose_AdfMax_PropertySum;                     //角色最大魔防
    public ObscuredFloat Rose_HealHpValuePro_PropertySum;           //角色恢复最大生命的比例
    public ObscuredInt Rose_HealHpValue_PropertySum;                //角色恢复的固定值
    public ObscuredFloat Rose_HealHpPro_PropertySum;                //角色恢复的百分比加成固定值
    public ObscuredFloat Rose_HealHpFightPro_PropertySum;           //角色战斗时恢复额外恢复血量的百分比
    public ObscuredFloat Rose_HealHpTime_PropertySum;               //角色2次恢复生命的时间
    private ObscuredFloat rose_HealHpTimeSum_PropertySum;

    public ObscuredInt Rose_GeDangValue_PropertySum;                    //格挡值
    public ObscuredFloat Rose_ZhongJiPro_PropertySum;                   //重击概率
    public ObscuredInt Rose_ZhongJiValue_PropertySum;                   //重击附加伤害值
    public ObscuredInt Rose_GuDingValue_PropertySum;                    //每次普通攻击附加的伤害值
    public ObscuredInt Rose_HuShiDefValue_PropertySum;                  //忽视目标防御值                       
    public ObscuredInt Rose_HuShiAdfValue_PropertySum;                  //忽视目标魔防值
    public ObscuredFloat Rose_HuShiDefValuePro_PropertySum;             //忽视目标防御值_百分比                       
    public ObscuredFloat Rose_HuShiAdfValuePro_PropertySum;             //忽视目标魔防值_百分比
    public ObscuredFloat Rose_XiXuePro_PropertySum;                     //吸血百分比

    public ObscuredInt Rose_CriRating_PropertySum;                     //暴击等级
    public ObscuredInt Rose_ResilienceRating_PropertySum;              //韧性等级
    public ObscuredInt Rose_HitRating_PropertySum;                     //命中等级
    public ObscuredInt Rose_DodgeRating_PropertySum;                   //闪避等级
    public ObscuredFloat Rose_MagicRebound_PropertySum;                //法术反击值
    public ObscuredFloat Rose_ActRebound_PropertySum;                  //攻击反击
    public ObscuredFloat Rose_Resistance_1_PropertySum;                //光抗性
    public ObscuredFloat Rose_Resistance_2_PropertySum;                //暗抗性
    public ObscuredFloat Rose_Resistance_3_PropertySum;                //火抗性
    public ObscuredFloat Rose_Resistance_4_PropertySum;                //水抗性
    public ObscuredFloat Rose_Resistance_5_PropertySum;                //电抗性
    public ObscuredFloat Rose_RaceResistance_1_PropertySum;            //野兽攻击抗性
    public ObscuredFloat Rose_RaceResistance_2_PropertySum;            //人物攻击抗性
    public ObscuredFloat Rose_RaceResistance_3_PropertySum;            //恶魔攻击抗性
    public ObscuredFloat Rose_RaceDamge_1_PropertySum;                  //攻击野兽加成
    public ObscuredFloat Rose_RaceDamge_2_PropertySum;                  //攻击人物加成
    public ObscuredFloat Rose_RaceDamge_3_PropertySum;            		//攻击恶魔加成
    public ObscuredInt Rose_Exp_AddValue_PropertySum;                   //经验加成固定值
    public ObscuredFloat Rose_Exp_AddPro_PropertySum;                   //经验加成百分比
    public ObscuredInt Rose_Gold_AddValue_PropertySum;                  //金币加成固定值
    public ObscuredFloat Rose_Gold_AddPro_PropertySum;                  //金币加成百分比


    public ObscuredFloat Rose_Blessing_AddPro_PropertySum;          	//洗炼极品掉落
    public ObscuredFloat Rose_HidePro_AddPro_PropertySum;           	//隐藏属性出现概率
    public ObscuredFloat Rose_GemHole_AddPro_PropertySum;           	//装备上的宝石槽位出现概率
    //public ObscuredInt Rose_XilianLucky_AddValue_PropertySum;         //洗炼幸运值,洗炼幸运值越高出好的属性概率越高,出幸运属性越高

    public ObscuredInt Rose_YaoJiShuLian_Value_PropertySum;             //药剂类熟练度
    public ObscuredInt Rose_DuanZuaoShuLian_Value_PropertySum;          //锻造类熟练度


    public ObscuredFloat Rose_Cri_PropertySum;                      //角色暴击
    public ObscuredFloat Rose_Res_PropertySum;                      //韧性值
    public ObscuredFloat Rose_Hit_PropertySum;                      //角色命中
    public ObscuredFloat Rose_Dodge_PropertySum;                    //角色闪避
    public ObscuredFloat Rose_DefAdd_PropertySum;                   //角色物理免伤
    public ObscuredFloat Rose_AdfAdd_PropertySum;                   //角色魔法免伤
    public ObscuredFloat Rose_DamgeSub_PropertySum;                 //角色免伤值
    public ObscuredFloat Rose_DamgeAdd_PropertySum;                 //角色伤害加成
    public ObscuredFloat Rose_Lucky_PropertySum;                    //角色幸运值
    public ObscuredFloat Rose_MoveSpeed_PropertySum;                //角色移动速度

    public ObscuredFloat Rose_Boss_ActAdd_PropertySum;                          //Boss普通攻击加成
    public ObscuredFloat Rose_Boss_SkillAdd_PropertySum;                        //Boss技能攻击加成
    public ObscuredFloat Rose_Boss_ActHitCost_PropertySum;                      //受到Boss普通攻击减免
    public ObscuredFloat Rose_Boss_SkillHitCost_PropertySum;                    //受到Boss技能攻击减免
    public ObscuredFloat Rose_PetActAdd_PropertySum;                            //宠物攻击加成
    public ObscuredFloat Rose_PetActHitCost_PropertySum;                        //宠物受伤减免
    public ObscuredFloat Rose_SkillCDTimePro_PropertySum;                       //技能冷却时间缩减
    public ObscuredFloat Rose_BuffTimeAddPro_PropertySum;                       //自身buff效果延长
    public ObscuredFloat Rose_DeBuffTimeCostPro_PropertySum;                    //Debuff时间缩短
    public ObscuredFloat Rose_DodgeAddHpPro_PropertySum;                        //闪避恢复血量

    public ObscuredFloat Rose_LanValueMaxAdd_PropertySum;                       //魔法量附加
    public ObscuredFloat Rose_SummonAIPropertyAddPro_PropertySum;               //召唤生物属性加成
    public ObscuredFloat Rose_HuDunValueAddPro_PropertySum;                     //护盾属性附加
    public ObscuredFloat Rose_ActAddPro_PropertySum;                            //普通攻击技能加成
    public ObscuredFloat Rose_SummonAIHpPropertyAddPro_PropertySum;             //召唤生物血量属性加成
    public ObscuredFloat Rose_SummonAIActPropertyAddPro_PropertySum;            //召唤生物攻击属性加成
    public ObscuredFloat Rose_SummonAIDefPropertyAddPro_PropertySum;            //召唤生物防御属性加成

    private GameObject Obj_UI_RoseHp;                       //角色Hp的UI
    public bool updataOnly;                                 //更新标题显示
    private ObscuredFloat updatePrppertyValue;

    public ObscuredBool HuDunStatus;                    //护盾状态
    public ObscuredInt HuDunValue;                      //护盾值
    public ObscuredFloat HuDunTime;                     //护盾时间
    public ObscuredFloat HudunTimeSum;                  //护盾时间累计
    public GameObject HuDunEffect;                      //护盾特效

    //血量上限计算系数
    public ObscuredInt Rose_HpAdd_1;                //加法，在公式的内测
    public ObscuredInt Rose_HpAdd_2;                //加法，在公式的最外侧
    public ObscuredFloat Rose_HpMul_1;              //乘法，在公式的外侧

    //最小攻击计算系数
    public ObscuredInt Rose_ActMinAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_ActMinAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_ActMinMul_1;     //乘法，在公式的外侧

    //最大攻击计算系数
    public ObscuredInt Rose_ActMaxAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_ActMaxAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_ActMaxMul_1;     //乘法，在公式的外侧

    //最小攻击计算系数
    public ObscuredInt Rose_MagActMinAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_MagActMinAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_MagActMinMul_1;     //乘法，在公式的外侧

    //最大攻击计算系数
    public ObscuredInt Rose_MagActMaxAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_MagActMaxAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_MagActMaxMul_1;     //乘法，在公式的外侧

    //最小物防计算系数
    public ObscuredInt Rose_DefMinAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_DefMinAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_DefMinMul_1;     //乘法，在公式的外侧

    //最大物防计算系数
    public ObscuredInt Rose_DefMaxAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_DefMaxAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_DefMaxMul_1;     //乘法，在公式的外侧

    //最小魔防计算系数
    public ObscuredInt Rose_AdfMinAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_AdfMinAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_AdfMinMul_1;     //乘法，在公式的外侧

    //最大魔防计算系数
    public ObscuredInt Rose_AdfMaxAdd_1;       //加法，在公式的内测
    public ObscuredInt Rose_AdfMaxAdd_2;       //加法，在公式的最外侧
    public ObscuredFloat Rose_AdfMaxMul_1;     //乘法，在公式的外侧

    //暴击
    public ObscuredFloat Rose_CriMul_1;        //乘法

    //命中
    public ObscuredFloat Rose_HitMul_1;        //乘法

    //闪避
    public ObscuredFloat Rose_DodgeMul_1;        //乘法
    
    //韧性
    public ObscuredFloat Rose_ResilienceRatingMul_1;    //乘法

    //物理免疫
    public ObscuredFloat Rose_DefMul_1;        //乘法

    //魔法免疫
    public ObscuredFloat Rose_AdfMul_1;        //乘法

    //伤害减免
    public ObscuredFloat Rose_DamgeSubtractMul_1;    //乘法

    //伤害减免
    public ObscuredFloat Rose_DamgeAddMul_1;    //乘法

    //移动速度计算系数
    public ObscuredInt Rose_MoveSpeedAdd_1;     //加法，在公式的内测
    public ObscuredInt Rose_MoveSpeedAdd_2;     //加法，在公式的最外侧
    public ObscuredFloat Rose_MoveSpeedMul_1;   //乘法，在公式的外侧
    public ObscuredFloat Rose_MoveSpeedYaoGan;  //摇杆控制的系数
    public ObscuredFloat rose_LastMoveSpeed;    //摇杆控制的基础速度

    public ObscuredInt Rose_HealHpValue_Add;                    //角色恢复的固定值
    public ObscuredFloat Rose_HealHpPro_Add;                    //角色恢复的百分比加成固定值
    public ObscuredFloat Rose_HealHpFightPro_Add;               //角色战斗时恢复额外恢复血量的百分比

    public ObscuredInt Rose_Luck_Add;                           //幸运值
    public ObscuredInt Rose_GeDangValue_Add;                    //格挡值
    public ObscuredFloat Rose_ZhongJiPro_Add;                   //重击概率
    public ObscuredInt Rose_ZhongJiValue_Add;                   //重击附加伤害值
    public ObscuredInt Rose_GuDingValue_Add;                    //每次普通攻击附加的伤害值
    public ObscuredInt Rose_HuShiDefValue_Add;                  //忽视目标防御值                       
    public ObscuredInt Rose_HuShiAdfValue_Add;                  //忽视目标魔防值
    public ObscuredFloat Rose_HuShiDefValuePro_Add;             //忽视目标防御值_百分比                       
    public ObscuredFloat Rose_HuShiAdfValuePro_Add;             //忽视目标魔防值_百分比
    public ObscuredFloat Rose_XiXuePro_Add;                     //吸血百分比

    public ObscuredInt Rose_CriRating_Add;                      //暴击等级
    public ObscuredInt Rose_ResilienceRating_Add;               //韧性等级
    public ObscuredInt Rose_HitRating_Add;                      //命中等级
    public ObscuredInt Rose_DodgeRating_Add;                    //闪避等级
    public ObscuredFloat Rose_MagicRebound_Add;                 //法术反击值
    public ObscuredFloat Rose_ActRebound_Add;                   //攻击反击
    public ObscuredFloat Rose_Resistance_1_Add;                 //光抗性
    public ObscuredFloat Rose_Resistance_2_Add;                 //暗抗性
    public ObscuredFloat Rose_Resistance_3_Add;                 //火抗性
    public ObscuredFloat Rose_Resistance_4_Add;                 //水抗性
    public ObscuredFloat Rose_Resistance_5_Add;                 //电抗性
    public ObscuredFloat Rose_RaceResistance_1_Add;             //野兽攻击抗性
    public ObscuredFloat Rose_RaceResistance_2_Add;             //人物攻击抗性
    public ObscuredFloat Rose_RaceResistance_3_Add;             //恶魔攻击抗性
    public ObscuredFloat Rose_RaceDamge_1_Add;            	    //攻击野兽加成
    public ObscuredFloat Rose_RaceDamge_2_Add;            		//攻击人物加成
    public ObscuredFloat Rose_RaceDamge_3_Add;                  //攻击恶魔加成
    public ObscuredFloat Rose_Boss_ActAdd_Add;            	    //Boss普通攻击加成
    public ObscuredFloat Rose_Boss_SkillAdd_Add;            	//Boss技能攻击加成
    public ObscuredFloat Rose_Boss_ActHitCost_Add;              //受到Boss普通攻击减免
    public ObscuredFloat Rose_Boss_SkillHitCost_Add;            //受到Boss技能攻击减免
    public ObscuredFloat Rose_PetActAdd_Add;                    //宠物攻击加成
    public ObscuredFloat Rose_PetActHitCost_Add;                //宠物受伤减免
    public ObscuredFloat Rose_SkillCDTimePro_Add;                       //技能冷却时间缩减
    public ObscuredFloat Rose_BuffTimeAddPro_Add;                       //自身buff效果延长
    public ObscuredFloat Rose_DeBuffTimeCostPro_Add;                    //Debuff时间缩短
    public ObscuredFloat Rose_DodgeAddHpPro_Add;                        //闪避恢复血量
    public ObscuredFloat Rose_SingTimePro_Add;                          //吟唱时间缩减

    public ObscuredInt Rose_Exp_AddValue_Add;                   //经验加成固定值
    public ObscuredFloat Rose_Exp_AddPro_Add;                   //经验加成百分比
    public ObscuredInt Rose_Gold_AddValue_Add;                  //金币加成固定值
    public ObscuredFloat Rose_Gold_AddPro_Add;                  //金币加成百分比
    public ObscuredFloat Rose_Blessing_AddPro_Add;          	//洗炼极品掉落
    public ObscuredFloat Rose_HidePro_AddPro_Add;           	//隐藏属性出现概率
    public ObscuredFloat Rose_GemHole_AddPro_Add;           	//装备上的宝石槽位出现概率
    public ObscuredInt Rose_YaoJiShuLian_Value_Add;             //药剂类熟练度
    public ObscuredInt Rose_DuanZuaoShuLian_Value_Add;          //锻造类熟练度

    public ObscuredFloat Rose_FuHuoPro_Add;			            //复活概率
    public ObscuredFloat Rose_ActWuShi_Add;			            //攻击无视防御
    public ObscuredFloat Rose_ShenNong_Add;			            //神农
    public ObscuredFloat Rose_DropExtra_Add;		            //额外掉落
    public ObscuredFloat Rose_WeiZhuang_Add;		            //伪装  +增大发现范围   -缩小范围
    public ObscuredFloat Rose_ZaiNanValue_Add;		            //灾难
    public ObscuredFloat Rose_ShiXuePro_Add;		            //嗜血概率
    public ObscuredFloat Rose_AITuoZhanDisValue_Add;			//怪物脱战距离
    public ObscuredFloat Rose_ZhuanZhuPro_Add;					//专注概率

    public ObscuredFloat Rose_BiZhongPro_Add;                   //必中概率
    public ObscuredFloat Rose_YaoJiCirPro_Add;                  //生产药剂暴击概率
    public ObscuredFloat Rose_PuZhuoPro_Add;                    //捕捉宠物概率
    public ObscuredFloat Rose_ChouHenValue_Add;                    //仇恨值


    //public ObscuredInt zuobiInt;
    private ObscuredBool JianCeShuXingStatus;                   //检测属性异常

    //特殊系数
    public ObscuredInt Rose_MijingValue;                        //秘境值
    public ObscuredFloat Rose_ZhaoHuanCDTime;                   //召唤宠物冷却时间
    public ObscuredFloat Rose_ZhaoHuanCDSumTime;                //召唤宠物冷却时间
    public ObscuredBool Rose_ZhaoHuanCDStatus;                  //召唤宠物冷却状态
    private UI_FunctionOpen ui_FunctionOpen;

    //特殊技能加成类
    public ObscuredInt Skill_LuanShe_AddNum;                            //乱射附加次数

	//战力
	public ObscuredInt Rose_ShiLiValue;

    //其他
    private GameObject hpShowObj;
    private GameObject lanShowObj;
    private GameObject nengliangShowObj;
    private ObscuredFloat secSum;



    // Use this for initialization
    void Start () {

        //初始化值
        secSum = 0;
        updatePrppertyValue = 0;
        //Rose_ZhaoHuanCDTime = 0;
        Rose_ZhaoHuanCDSumTime = 0;

        Rose_HpNow = 10;    //防止从主城进入关卡直接触发死亡
        Obj_UI_RoseHp = Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_UI_RoseHp;

		Rose_GetExp = true;
		//初始化角色当前属性
		Game_PublicClassVar.Get_game_PositionVar.UpdataRoseProperty = true;

        //初始化回血数据
        Rose_HealHpValuePro = 0.01f;
        Rose_HealHpValue = 1;
        Rose_HealHpTime = 5.0f;     //N秒回一次血量

        rose_Status = this.GetComponent<Rose_Status>();
        Rose_MoveSpeedYaoGan = 1;

        //初始化蓝量为100
        Rose_LanValue = 0;
        //法师
        if (Rose_Occupation == "2") {
            Rose_LanValueMax = 100;
            Rose_LanValue = 100;
        }
        //猎人
        if (Rose_Occupation == "3")
        {
            Rose_LanValueMax = 5;
            Rose_LanValue = 0;
        }


        //Game_PublicClassVar.function_UI.SendObsErr("HAHAHAHAHA");
        //ObscuredInt.sendError("HAHAHAHAHA123123123");
    }
	
	// Update is called once per frame
	void Update () {

        //更新角色生命值UI
        float hpPro = float.Parse(Rose_HpNow.ToString()) / float.Parse(Rose_Hp.ToString());
        if (hpShowObj == null) {
            hpShowObj = Obj_UI_RoseHp.transform.Find("Img_Value").gameObject;
        }

        hpShowObj.GetComponent<Image>().fillAmount = hpPro;

        //更新魔法值
        float lanPro = float.Parse(Rose_LanValue.ToString()) / float.Parse(Rose_LanValueMax.ToString());
        if (lanShowObj == null)
        {
            //法师 
            lanShowObj = Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_Lan_Value.gameObject;
        }

        if (nengliangShowObj == null)
        {
            //猎人
            nengliangShowObj = Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_NengLiang_Value;
        }


        lanShowObj.GetComponent<Image>().fillAmount = lanPro;
        nengliangShowObj.GetComponent<Image>().fillAmount = lanPro;

        //获取当前角色血量
        if (Rose_HpNow <= 0)
        {
            //获取概率是否复活
            float suijiValue = Random.value;

            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus == false)
            {
                if (suijiValue <= Rose_FuHuoPro)
                {
                    //触发复活
                    Rose_HpNow = Rose_Hp;
                    string langStrHint = Game_PublicClassVar.Get_gameSettingLanguge.LoadLocalizationHint("hint_396");
                    Game_PublicClassVar.Get_function_UI.GameGirdHint_Front(langStrHint);
                    Debug.Log("触发重生！！！" + Rose_FuHuoPro + "Rose_HpNow = " + Rose_HpNow + "Rose_Hp = " + Rose_Hp + "suijiValue = " + suijiValue);
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                    //Game_PublicClassVar.Get_function_UI.GameHint("触发重生！！！");
                    //后期需要重制当前所有的增益减益效果

                }
                else
                {
                    //设置死亡状态
                    if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus == false)
                    {
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatus = true;
                        Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseDeathStatusOnce = false;
                    }
                }
            }
        }

        //更新当前经验
        if (Rose_GetExp) {
			Rose_GetExp = false;
			Rose_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            Rose_Exp = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseUpExp", "RoseLv", Rose_Lv.ToString(), "RoseExp_Template"));
            Rose_ExpNow = long.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseExpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        }

        //放在开始出会和开始的缓存有冲突
        if (!updataOnly)
        {
            //显示角色血量
            Rose_HpNow = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RoseHpNow", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
            if (Rose_HpNow <= 0) {
                Rose_HpNow = 1;
            }
            this.GetComponent<Rose_Status>().rose_LastHp = Rose_HpNow;

            //显示角色名称
            ShowName();

            updataOnly = true;
        }

        updatePrppertyValue = updatePrppertyValue + Time.deltaTime;

        if (updatePrppertyValue >= 0.2f)
        {
            updatePrppertyValue = 0;

            //获取实际攻击
            Rose_Act = (int)((Rose_ActMax - Rose_ActMin) * Random.value) + Rose_ActMin;
            if (LuckActMax())
            {
                Rose_Act = Rose_ActMax;
            }

            //获取实际攻击
            //Rose_Act = (int)((Rose_ActMax - Rose_ActMin) * Random.value) + Rose_ActMin;
            //获取实际魔攻
            Rose_MagAct = (int)((Rose_MagActMax - Rose_MagActMin) * Random.value) + Rose_MagActMin;
            //获取实际防御
            Rose_Def = (int)((Rose_DefMax - Rose_DefMin) * Random.value) + Rose_DefMin;
            //获取实际魔防
            Rose_Adf = (int)((Rose_AdfMax - Rose_AdfMin) * Random.value) + Rose_AdfMin;

            //嗜血
            ObscuredFloat pro_1 = 1;
            ObscuredFloat pro_2 = 2;

            Rose_Act = (int)(Rose_Act * (pro_1 + Rose_ShiXuePro));
			Rose_Def = (int)(Rose_Def * (pro_1 - Rose_ShiXuePro * pro_2));
			Rose_Adf = (int)(Rose_Adf * (pro_1 - Rose_ShiXuePro * pro_2));

            if (rose_Status.RoseStatus != "8")
            {
                //正常回血
                rose_HealHpTimeSum = rose_HealHpTimeSum + Time.deltaTime;
                if (rose_HealHpTimeSum >= Rose_HealHpTime)
                {
                    //Debug.Log("回血");
                    ObscuredInt healValue = (int)(Rose_Hp * Rose_HealHpValuePro + Rose_HealHpValue * (pro_1 + Rose_HealHpPro));
                    //判定角色是否在战斗状态
                    if (this.GetComponent<Rose_Status>().RoseFightStatus) {
                        //战斗回血加成
                        healValue = (int)(healValue * (pro_1 + Rose_HealHpFightPro));
                    }
                    Game_PublicClassVar.Get_function_Rose.addRoseHp(healValue);
                    rose_HealHpTimeSum = 0;
                }
            }
        }
        //护盾开启
        if (HuDunStatus) {
            HudunTimeSum = HudunTimeSum + Time.deltaTime;
            //时间到取消护盾
            if (HudunTimeSum > HuDunTime) {
                HuDunStatus = false;
            }
            //护盾值降低取消护盾
            if (HuDunValue <= 0) {
                Debug.Log("伤害抵消,护盾");
                HuDunStatus = false;
            }

            //注销护盾
            if (!HuDunStatus) {
                HudunTimeSum = 0;
                if (HuDunEffect != null)
                {
                    Destroy(HuDunEffect);  //删除护盾特效
                }
            }
        }
        
        //更新摇杆速度
        if (rose_Status.YaoGan_Status)
        {
            //摇杆靠近中心时速度降低
            Rose_MoveSpeed = rose_LastMoveSpeed * Rose_MoveSpeedYaoGan;
        }
        else
        {
            Rose_MoveSpeed = rose_LastMoveSpeed * 1;
        }


        if (Rose_ZhaoHuanCDStatus) {

            if (ui_FunctionOpen == null) {
                ui_FunctionOpen = Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>();
            }

            Rose_ZhaoHuanCDSumTime = Rose_ZhaoHuanCDSumTime + Time.deltaTime;
            if (Rose_ZhaoHuanCDSumTime >= Rose_ZhaoHuanCDTime)
            {
                //冷却时间结束
                ClearnPetZhaoHuanTime();
            }
            else {
                if (this.GetComponent<Rose_Status>().RoseFightFirstStatus)
                {
                    float shengyuTime = Rose_ZhaoHuanCDTime - Rose_ZhaoHuanCDSumTime;
                    ui_FunctionOpen.Obj_Pet_ZhaoHuanCDImg.SetActive(true);
                    ui_FunctionOpen.Obj_Pet_ZhaoHuanCDImg.GetComponent<Image>().fillAmount = shengyuTime / Rose_ZhaoHuanCDTime;
                    int shengyuTime_Int = (int)(shengyuTime);
                    ui_FunctionOpen.Obj_Pet_ZhaoHuanCDText.SetActive(true);
                    ui_FunctionOpen.Obj_Pet_ZhaoHuanCDText.GetComponent<Text>().text = shengyuTime_Int.ToString();
                }
                else {
                    //冷却时间结束
                    ClearnPetZhaoHuanTime();
                }
            }
        }

        //检测属性异常
        /*
        if (!JianCeShuXingStatus) {
            if (Rose_Act >= 100000)
            {
                JianCeShuXingStatus = true;
                Pro_ComStr_3 comstr_3 = new Pro_ComStr_3();
                comstr_3.str_1 = "3";
                comstr_3.str_2 = Rose_Act.ToString();
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10001201, comstr_3);
            }
        }
        */

        //每秒触发一次
        secSum = secSum + Time.deltaTime;
        if (secSum>=1) {
            secSum = 0;
            if (Rose_Occupation == "2") {
                //每秒恢复10点魔法
                int addMp = (int)(Rose_LanValueMax / 10);
                if (addMp < 10) {
                    addMp = 10;
                }
                Game_PublicClassVar.Get_function_Rose.RoseLanAdd(addMp);
            }
        }
	}

    public void ShowName() {
        //显示角色名称
        string roseName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Rose_Lv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Lv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
        //Obj_UI_RoseHp.transform.Find("Lab_RoseName").GetComponent<Text>().text = "Lv." + Rose_Lv + "  " + roseName;
        Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_RoseName.GetComponent<Text>().text = roseName;
        Obj_UI_RoseHp.GetComponent<UI_RoseHp>().Obj_RoseLv.GetComponent<Text>().text = Rose_Lv.ToString();
        updataOnly = true;
    }

    //清除召唤宠物冷却时间
    public void ClearnPetZhaoHuanTime() {

        Rose_ZhaoHuanCDStatus = false;
        ui_FunctionOpen.Obj_Pet_ZhaoHuanCDImg.SetActive(false);
        ui_FunctionOpen.Obj_Pet_ZhaoHuanCDText.SetActive(false);
        Rose_ZhaoHuanCDTime = 0;
        Rose_ZhaoHuanCDSumTime = 0;
    }

    //幸运值换算最大攻击概率
    public bool LuckActMax()
    {
        ObscuredInt luckValue = (int)(Rose_Lucky);
        ObscuredFloat luckProValue = 0;

        switch (luckValue)
        {

            case 0:
                luckProValue = 0f;
                break;
            case 1:
                luckProValue = 0.025f;
                break;
            case 2:
                luckProValue = 0.05f;
                break;
            case 3:
                luckProValue = 0.1f;
                break;
            case 4:
                luckProValue = 0.15f;
                break;
            case 5:
                luckProValue = 0.2f;
                break;
            case 6:
                luckProValue = 0.3f;
                break;
            case 7:
                luckProValue = 0.4f;
                break;
            case 8:
                luckProValue = 0.5f;
                break;
            case 9:
                luckProValue = 1.0f;
                break;
        }

        if (luckValue >= 9)
        {
            luckProValue = 1.0f;
            return true;
        }

        if (Random.value >= luckProValue)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //换算当前暴击,抗暴,命中,闪避属性
    public void PropretyValueToPro(){

        /*
    public ObscuredInt Rose_CriRating;                     //暴击等级
    public ObscuredInt Rose_ResilienceRating;              //韧性等级
    public ObscuredInt Rose_HitRating;                     //命中等级
    public ObscuredInt Rose_DodgeRating;                   //闪避等级
        */





    }

}
