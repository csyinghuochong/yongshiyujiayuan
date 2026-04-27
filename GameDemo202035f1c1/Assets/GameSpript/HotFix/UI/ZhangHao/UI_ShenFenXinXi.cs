using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShenFenXinXi : MonoBehaviour {

    public GameObject Obj_Name;
    public GameObject Obj_ShenFenID;

    // Use this for initialization
    void Start () {

        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");
        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {
            //不需要进行防沉迷验证,已经验证成功
            string xinghaoStr = "";
            for (int i = 0; i < name.Length - 1; i++)
            {
                xinghaoStr = xinghaoStr + "*";
            }

            Obj_Name.GetComponent<InputField>().text = name.Substring(0, 1) + xinghaoStr;
            Obj_ShenFenID.GetComponent<InputField>().text = shenfenID.Substring(0, 2) + "****************";
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
