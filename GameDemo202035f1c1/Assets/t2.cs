using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class t2 : MonoBehaviour {

    public GameObject timeObj;
    public GameObject Obj_ActMin;
    public GameObject Obj_ActMin_JiaMiKey;
    public GameObject Obj_ActMin_Key;
    public GameObject Obj_Key;
    public GameObject Obj_Inted;
    public GameObject Obj_FakeActive;

    private float time;

    public bool test;

    public ObscuredInt aaa;

    // Use this for initialization
    void Start () {
        aaa = 1;
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("aaa = " + aaa);



        //
        /*
        if (Game_PublicClassVar.Get_wwwSet.DataUpdataStatus) {
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>() != null) {
                Obj_ActMin.GetComponent<Text>().text = "Rose_ActMin:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.ToString();
                Obj_ActMin_JiaMiKey.GetComponent<Text>().text = "hiddenValue:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.hiddenValue.ToString();
                Obj_ActMin_Key.GetComponent<Text>().text = "fakeValue:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.fakeValue.ToString();
                Obj_Key.GetComponent<Text>().text = "currentCryptoKey:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.currentCryptoKey.ToString();
                Obj_Inted.GetComponent<Text>().text = "inited:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.inited.ToString() + "  APP:" + Application.version;
                Obj_FakeActive.GetComponent<Text>().text = "fakeValueActive:" + Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.fakeValueActive.ToString();
                time = time + Time.deltaTime;
                timeObj.GetComponent<Text>().text = time.ToString();
            }
        }


        if (test) {
            test = false;
            Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ActMin.hiddenValue = 8888888;
        }
        */

        if (test)
        {
            test = false;
            Debug.Log("aaa = " + aaa);
        }
    }
}
