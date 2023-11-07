using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TestXinXi : MonoBehaviour {

    public GameObject Obj_DrawCall;
    public GameObject Obj_Batch;
    public GameObject Obj_Fps;
    public GameObject Obj_Test_1;
    public GameObject Obj_Test_2;

    public float fpsMeasuringDelta = 2.0f;
    public int TargetFrame = 30;

    private float timePassed;
    private int m_FrameCount = 0;
    private float m_FPS = 0.0f;

    

    // Use this for initialization
    void Start () {
        timePassed = 0.0f;
        Application.targetFrameRate = TargetFrame;
    }

    // Update is called once per frame
    void Update()
    {

        //Obj_DrawCall.GetComponent<Text>().text = "DrawCalls = " + UnityEditor.UnityStats.drawCalls;
        //Obj_Batch.GetComponent<Text>().text = "Batch = " + UnityEditor.UnityStats.batches;

        m_FrameCount = m_FrameCount + 1;
        timePassed = timePassed + Time.deltaTime;

        if (timePassed > fpsMeasuringDelta)
        {
            m_FPS = m_FrameCount / timePassed;

            timePassed = 0.0f;
            m_FrameCount = 0;
        }

        Obj_Fps.GetComponent<Text>().text = "Fps = " + m_FPS;

        //读取npcshujv
        /*
        string test1 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("SpeakText", "ID", "20001001", "Npc_Template");
        string test2 = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ShopValue", "ID", "22000008", "Npc_Template");
        Obj_Test_1.GetComponent<Text>().text = test1;
        Obj_Test_2.GetComponent<Text>().text = test2;

        byte[] dateByte = Game_PublicClassVar.Get_wwwSet.DataSetToByte(Game_PublicClassVar.Get_wwwSet.DataSetXml);
        MD5 md5 = MD5.Create();
        byte[] encryptionBytes = md5.ComputeHash(dateByte);
        string EncryptionStr = Convert.ToBase64String(encryptionBytes);

        string test3 = "BiDui_1 =" + Game_PublicClassVar.Get_wwwSet.DataSetXml_BiDui_1_michi + "; BiDui_1 =" + EncryptionStr;
        Obj_Test_2.GetComponent<Text>().text = test2 + test3;
        
        Debug.Log("md5TestStr = " + EncryptionStr);
        */

        Debug.Log("时间：" + Time.deltaTime);
    }

    //隐藏场景
    public void CloseScence() {
        
        GameObject sceneSet = GameObject.Find("SceneSet");
        if (sceneSet.active)
        {
            sceneSet.SetActive(false);
        }
        else {
            sceneSet.SetActive(true);
        }

    }

    //隐藏怪物
    public void CloseMonster()
    {

        GameObject sceneSet = GameObject.Find("Monster");
        if (sceneSet.active)
        {
            sceneSet.SetActive(false);
        }
        else
        {
            sceneSet.SetActive(true);
        }

    }

    public void Close() {
        this.gameObject.SetActive(false);
    }
}
