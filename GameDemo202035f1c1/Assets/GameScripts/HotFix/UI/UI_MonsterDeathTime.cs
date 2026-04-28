using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_MonsterDeathTime : MonoBehaviour {
    public GameObject Obj_DeathTime;
    public ObscuredFloat DeathTime;
    public ObscuredFloat DeathRebirthTimeSum;
    private ObscuredFloat deathTimeSum;
    public ObscuredString deathMonsterName;
    //离线时间
    public ObscuredFloat OffLineTime;
    private ObscuredFloat offLineTimeSum;
    public GameObject Obj_OffLineTime;
    public GameObject Obj_MonsterObj;

    public bool wordTimeOnceStatus;

	// Use this for initialization
	void Start () {
        Obj_DeathTime.GetComponent<Text>().text = "BOSS重生时间：" + (int)DeathTime + "秒";
        Obj_OffLineTime.GetComponent<Text>().text = "BOSS离线重生时间：" + (int)OffLineTime + "秒";
        //如果初始化的时候读取到了时间时间,则不在更新离线时间
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus) {
            wordTimeOnceStatus = true;
        }
        
        if (DeathTime + 1>= OffLineTime) {
            DeathRebirthTimeSum = 0;
        }

    }
	
	// Update is called once per frame
	void Update () {
        deathTimeSum = deathTimeSum + Time.deltaTime;
        offLineTimeSum = offLineTimeSum + Time.deltaTime;
        if (deathTimeSum>=1.0f)
        {
            DeathTime = DeathTime - deathTimeSum;
            OffLineTime = OffLineTime - offLineTimeSum;
            if ((int)DeathTime <= 0)
            {
                Obj_DeathTime.GetComponent<Text>().text = "BOSS将立即刷新";
                Obj_OffLineTime.GetComponent<Text>().text = "BOSS将立即刷新";
            }
            else {
                //展示在线时间
                int hourTime = (int)(DeathTime - DeathRebirthTimeSum) / 3600;
                int remainTime = (int)(DeathTime - DeathRebirthTimeSum) % 3600;
                int minuteTime = remainTime / 60;
                remainTime = remainTime % 60;

                string shouType = "0";
                string showStr = "";
                if (minuteTime != 0){
                    shouType = "1";
                }

                if (hourTime != 0) {
                    shouType = "2";
                }

                switch (shouType) { 
                    case "0":
                        showStr = "BOSS重生时间：" + remainTime + "秒";
                        break;

                    case "1":
                        showStr = "BOSS重生时间：" + minuteTime + "分" + remainTime + "秒";
                        break;

                    case "2":
                        showStr = "BOSS重生时间：" + hourTime + "时" + minuteTime + "分" + remainTime + "秒";
                        break;
                
                }
                //Debug.Log("时间:" + hourTime + "时" + minuteTime + "分" + remainTime + "秒");
                Obj_DeathTime.GetComponent<Text>().text = showStr;

                //展示离线时间
                int offLine_hourTime = (int)OffLineTime / 3600;
                int offLine_remainTime = (int)OffLineTime % 3600;
                int offLine_minuteTime = offLine_remainTime / 60;
                offLine_remainTime = offLine_remainTime % 60;

                string offLine_shouType = "0";
                string offLine_showStr = "";
                if (offLine_minuteTime != 0)
                {
                    offLine_shouType = "1";
                }

                if (offLine_hourTime != 0)
                {
                    offLine_shouType = "2";
                }

                switch (offLine_shouType)
                {
                    case "0":
                        offLine_showStr = "BOSS离线重生时间：" + offLine_remainTime + "秒";
                        break;

                    case "1":
                        offLine_showStr = "BOSS离线重生时间：" + offLine_minuteTime + "分" + offLine_remainTime + "秒";
                        break;

                    case "2":
                        offLine_showStr = "BOSS离线重生时间：" + offLine_hourTime + "时" + offLine_minuteTime + "分" + offLine_remainTime + "秒";
                        break;

                }
                //Debug.Log("时间:" + hourTime + "时" + minuteTime + "分" + remainTime + "秒");
                Obj_OffLineTime.GetComponent<Text>().text = offLine_showStr;
            }
            deathTimeSum = 0;
            offLineTimeSum = 0;

            //如果离线时间小于在线时间,则将再现时间等于离线时间
            if (OffLineTime < DeathTime) {
                DeathTime = OffLineTime;
                /*
                string monsterOnlyID = Obj_MonsterObj.GetComponent<AI_1>().AI_ID_Only;
                float deathTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(monsterOnlyID);
                //更新离线时间
                if (Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime - Obj_MonsterObj.GetComponent<AI_1>().monsterReirthTimeSum < deathTime)
                {
                    Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime = deathTime;
                    Debug.Log("ZZZZZZZZZZZZZZZ");
                }
                */
            }

            if (OffLineTime <= 0) {
                
                //Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime = OffLineTime;   //离线时间为0时立即复活次怪物
                Obj_MonsterObj.GetComponent<AI_1>().monsterReirthTimeSum = Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime;   //离线时间为0时,立即复活次怪物

            }
        }

        //获取世界时间时更新怪物复活时间
        if (Game_PublicClassVar.Get_wwwSet.WorldTimeStatus)
        {
            if (!wordTimeOnceStatus) {
                wordTimeOnceStatus = true;
                updataMonsterDeathTime();
            }
        }

        if (Game_PublicClassVar.Get_wwwSet.UpdateMonsterDeathTimeStatus) {
            Game_PublicClassVar.Get_wwwSet.UpdateMonsterDeathTimeStatus = false;
            updataMonsterDeathTime();
        }


	}

    private void OnDestroy()
    {
        //Debug.Log("销毁OnDestroy");
    }

    //更新怪物复活时间
    void updataMonsterDeathTime() {
        string monsterOnlyID = Obj_MonsterObj.GetComponent<AI_1>().AI_ID_Only;
        float deathTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathTime(monsterOnlyID);
        //DeathTime = deathTime;
        //更新离线时间
        
        if (Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime - Obj_MonsterObj.GetComponent<AI_1>().monsterReirthTimeSum < deathTime) {
            //Debug.Log("更新怪物时间：" + deathTime);
            Obj_MonsterObj.GetComponent<AI_1>().monsterRebirthTime = deathTime;
            //Obj_MonsterObj.GetComponent<AI_1>().monsterReirthTimeSum = 0;
            //Debug.Log("离线时间比在线时间低,在线时间更新为离线时间");
            DeathTime = deathTime;
        }
        

        deathTime = Game_PublicClassVar.Get_function_AI.GetMonsterDeathOffLineTime(monsterOnlyID);
        OffLineTime = deathTime;
    }

}
