using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_NPCName : MonoBehaviour {

    public string NpcID;
    public bool HeadSpeakStatus;
    public float HeadSpeakShowTime;             //头顶说话展示的时间
    private float headSpeakShowTimeSum;
    private bool headSpeakShowOnce; 
    public string HeadSpeakText;            //说话的文本
    public GameObject Obj_HeadSpeak;
    public GameObject Obj_HeadSpeakText;
    public GameObject Obj_TaskGet;
    public GameObject Obj_TaskComplete;
    public GameObject Obj_NpcName;
    public GameObject Obj_HeadSpeakImg;
    public GameObject Obj_Npc;
    public GameObject Obj_MakeHint;

	// Use this for initialization
	void Start () {
        headSpeakShowTimeSum = 0;
        HeadSpeakShowTime = 3;
        HeadSpeakText = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NpcHeadSpeakText", "ID", NpcID, "Npc_Template");
        Obj_HeadSpeakText.GetComponent<Text>().text = HeadSpeakText;

        //设置说话框大小
        int hang = (int)(HeadSpeakText.Length / 14);
        Obj_HeadSpeakText.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 10.0f + hang * 30.0f);
        Obj_HeadSpeakImg.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 40 + hang * 20.0f);
        Obj_HeadSpeak.transform.localPosition = new Vector3(Obj_HeadSpeak.transform.localPosition.x, hang * 10.0f, 0);
	}
	
	// Update is called once per frame
	void Update () {

        if (HeadSpeakStatus) {
            //Debug.Log("进来了");
            headSpeakShowOnce = true;
            HeadSpeakStatus = false;
            //显示头顶说话
            //Obj_HeadSpeakText.GetComponent<Text>().text = HeadSpeakText;
            if (HeadSpeakText != "")
            {
                Obj_HeadSpeak.SetActive(true);    
            }
        }

        if (headSpeakShowOnce) {

            headSpeakShowTimeSum = headSpeakShowTimeSum + Time.deltaTime;

            if (headSpeakShowTimeSum >= HeadSpeakShowTime)
            {
                //Debug.Log("出去了");
                Obj_HeadSpeak.SetActive(false);       //隐藏头顶说话
                HeadSpeakStatus = false;
                headSpeakShowOnce = false;
            }
        }

        if (Obj_Npc==null)
        {
            Destroy(this.gameObject);
        }
	}
}
