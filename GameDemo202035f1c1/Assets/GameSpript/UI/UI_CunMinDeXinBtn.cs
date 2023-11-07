using UnityEngine;
using System.Collections;

public class UI_CunMinDeXinBtn : MonoBehaviour {

    public bool EventStatus;
    public bool EventUpdaDataStatus;
    private string eventID;
    private float CunMinSaveSum;
    private float eventTime;
    public float eventTimeSum;
    private float specialEventTime;
    private GameObject obj_CunMinDeXin;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //村民的信
        //CunMinEmailSum = CunMinEmailSum + Time.deltaTime;
        if (EventStatus)
        {
            CunMinSaveSum = CunMinSaveSum + Time.deltaTime;
            //每1秒记录一次事件
            if (CunMinSaveSum > 1.0f)
            {
                //eventTimeSum = eventTimeSum + CunMinSaveSum;
                specialEventTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventOpenTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                specialEventTime = specialEventTime + CunMinSaveSum;
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventOpenTime", specialEventTime.ToString(), "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

                //Debug.Log("specialEventTime = " + specialEventTime);
                //Debug.Log("eventTimeSum = " + eventTimeSum);
                if (specialEventTime >= eventTime)
                {
                    //隐藏按钮
                    EventStatus = false;
                    this.gameObject.SetActive(false);
                    //eventTimeSum = 0;
                    specialEventTime = 0;

                    Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("SpecialEventOpenTime", "0", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                    Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
                }
                CunMinSaveSum = 0;
            }
        }
        else {
            //隐藏按钮
            //eventTimeSum = 0;
            CunMinSaveSum = 0;
            this.gameObject.SetActive(false);
        }

        //根据事件等级
        if (EventUpdaDataStatus)
        {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();

            string[] specialEvent = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEvent", "RoseLv", roseLv.ToString(), "RoseExp_Template").Split(';');
            if (specialEvent[0] != "" && specialEvent[0] != "0")
            {
                bool eventStatus = false;
                int eventSum = 0;
                do{
                    //随机一个事件
                    float eventLength = specialEvent.Length - 0.01f;        //加0.99是确保最后一个数也能随机得到
                    int eventList = (int)(eventLength * Random.value);
                    
                    Debug.Log("eventList = " + eventList);
                    eventID = specialEvent[eventList];

                    float enevtPro = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventPro", "ID", eventID, "SpecialEvent_Template"));
                    if (Random.value <= enevtPro)
                    {
                        eventTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventTime", "ID", eventID, "SpecialEvent_Template"));
                        eventStatus = true;
                        //获取触发等级
                        Debug.Log("eventID = " + eventID);
                        int eventLv = int.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("EventLv", "ID", eventID, "SpecialEvent_Template"));
                        if (roseLv< eventLv) {
                            eventStatus = false;
                        }

                    }
                    eventSum = eventSum + 1;
                    if (eventSum >= 11) {
                        Debug.Log("eventSum = " + eventSum);
                        eventStatus = true;
                        eventID = specialEvent[0];
                    }
                } while (!eventStatus);
            }
            EventUpdaDataStatus = false;
        }
	}

    public void Btn_Open() {
        if (EventStatus)
        {
            if (obj_CunMinDeXin == null) {
                obj_CunMinDeXin = (GameObject)Instantiate(Game_PublicClassVar.Get_game_PositionVar.Obj_UICunMinDeXin);
                obj_CunMinDeXin.GetComponent<UI_CunMinDeXin>().CunMinDeXinID = eventID;
                //float specialEventTime = float.Parse(Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpecialEventOpenTime", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData"));
                obj_CunMinDeXin.GetComponent<UI_CunMinDeXin>().TimeShowStr = eventTime - specialEventTime;
                obj_CunMinDeXin.transform.SetParent(Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.transform);
                obj_CunMinDeXin.transform.localScale = new Vector3(1, 1, 1);
                obj_CunMinDeXin.transform.localPosition = Vector3.zero;
            }
        }
    }
}
