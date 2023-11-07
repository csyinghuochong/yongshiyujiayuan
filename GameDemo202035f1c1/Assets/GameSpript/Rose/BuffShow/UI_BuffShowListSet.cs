using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BuffShowListSet : MonoBehaviour {

	public Dictionary<string, GameObject> BuffIDObjSet = new Dictionary<string, GameObject>();

	public GameObject Obj_BuffShowSet;
	public GameObject Obj_BuffShow;

	public float time_ShuaXinSum;

    public bool UpdateZuoQiBuffStatus;
    public bool ttt;
	// Use this for initialization
	void Start () {
        ShowBaoLvBuff();
        UpdateZuoQiBuffStatus = true;
    }
	
	// Update is called once per frame
	void Update () {

		//刷新Buff显示
		time_ShuaXinSum = Time.deltaTime + time_ShuaXinSum;
		if (time_ShuaXinSum >= 1) {
			time_ShuaXinSum = 0;
			Buff_ShuaXinTime();
		}

        if (ttt) {
            ttt = false;
            DeleteBaoLvBuff();
        }

        if (UpdateZuoQiBuffStatus) {
            UpdateZuoQiBuffStatus = false;
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus)
            {
                AddBuff("96001001");
            }
            else {
                DelBuff("96001001");
            }
        }

	}

    //根据爆率值显示不同buffid
    public void ShowBaoLvBuff() {

        string rankBaoLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string showBuffID = getBaoLvID(rankBaoLv);
 

        if (showBuffID != "") {
            AddBuff(showBuffID);
        }
    }


    //根据爆率值删除不同buffid
    public void DeleteBaoLvBuff()
    {
        string rankBaoLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("RankBaoLv", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseDayReward");
        string showBuffID = getBaoLvID(rankBaoLv);


        if (showBuffID != "")
        {
            DelBuff(showBuffID);
        }

    }

    private string getBaoLvID(string type) {
        string showBuffID = "";
        switch (type)
        {
            /*
            case "1":
                showBuffID = "96000001";
                break;

            case ".5":
                showBuffID = "96000002";
                break;

            case ".4":
                showBuffID = "96000003";
                break;

            case ".3":
                showBuffID = "96000004";
                break;

            case ".2":
                showBuffID = "96000005";
                break;

            case ".15":
                showBuffID = "96000006";
                break;

            case ".1":
                showBuffID = "96000007";
                break;

            case ".05":
                showBuffID = "96000008";
                break;

            case ".03":
                showBuffID = "96000009";
                break;

            case ".01":
                showBuffID = "96000010";
                break;
                */

            case "":
                break;

            case "0":
                break;
            default:
                showBuffID = "96000010";
                break;
        }

        return showBuffID;
    }

	//添加Buff显示
	public void AddBuff(string buffID){

		GameObject buffShowObj = (GameObject)Instantiate(Obj_BuffShow);
		buffShowObj.transform.SetParent(Obj_BuffShowSet.transform);
        buffShowObj.transform.localScale = new Vector3(1,1,1);

        //集合放置参数
        buffShowObj.transform.GetComponent<UI_BuffShowObj>().BuffID = buffID;
		buffShowObj.transform.GetComponent<UI_BuffShowObj>().BuffIDShow();

		//添加buffID集合
		if(BuffIDObjSet.ContainsKey(buffID)){
			DelBuff(buffID);
		}

		BuffIDObjSet.Add(buffID, buffShowObj);

	}

	//删除Buff
	public void DelBuff(string buffID){

		//添加buffID集合
		if(BuffIDObjSet.ContainsKey(buffID)){
			Destroy(BuffIDObjSet[buffID]);
			BuffIDObjSet.Remove (buffID);
		}
        //BuffIDObjSet[buffID].GetComponent<UI_BuffShowObj>().Obj_BuffTimeShow.GetComponent<Text>().text = "N/A";
    }

	//刷新
	public void Buff_ShuaXinTime(){

		foreach (string buffID in BuffIDObjSet.Keys) {
            string skillCDTime = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("BuffTime", "ID", buffID, "SkillBuff_Template");
            //显示buff剩余时间
            int buffRemainTime = 0;

            //buff1列表
            Buff_1[] buff1List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_1>();
            if (buff1List.Length >= 1)
            {
                for (int i = 0; i < buff1List.Length; i++)
                {
                    if (buff1List[i].BuffID == buffID)
                    {

                        buffRemainTime = (int)(float.Parse(skillCDTime) - buff1List[i].buffTimeSum);
                    }
                }
            }

            //buff4列表
            Buff_4[] buff4List = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponents<Buff_4>();
			if (buff4List.Length >= 1) {
				for (int i = 0; i < buff4List.Length; i++) {
					if (buff4List[i].BuffID == buffID) {
						
						buffRemainTime =(int)(float.Parse(skillCDTime) - buff4List[i].buffTimeSum);
					}
				}
			}

			string showStr = "";
			if (buffRemainTime >= 60) {
				buffRemainTime = (int)(buffRemainTime / 60);
				showStr = buffRemainTime.ToString () + "时";
			} else {
				showStr = buffRemainTime.ToString()+"秒";
			}

			//buff显示
			if(BuffIDObjSet[buffID]!=null){
                if (skillCDTime != "999"&& skillCDTime != "99999" && skillCDTime != "86400")
                {
                    BuffIDObjSet[buffID].GetComponent<UI_BuffShowObj>().Obj_BuffTimeShow.GetComponent<Text>().text = showStr;
                }
                else {
                    if (skillCDTime == "999") {
                        BuffIDObjSet[buffID].GetComponent<UI_BuffShowObj>().Obj_BuffTimeShow.GetComponent<Text>().text = "";
                    }

                    if (skillCDTime == "99999")
                    {
                        BuffIDObjSet[buffID].GetComponent<UI_BuffShowObj>().Obj_BuffTimeShow.GetComponent<Text>().text = "";
                    }

                    if (skillCDTime == "86400")
                    {
                        BuffIDObjSet[buffID].GetComponent<UI_BuffShowObj>().Obj_BuffTimeShow.GetComponent<Text>().text = "1天";
                    }
                }
			}
		}
	}
}
