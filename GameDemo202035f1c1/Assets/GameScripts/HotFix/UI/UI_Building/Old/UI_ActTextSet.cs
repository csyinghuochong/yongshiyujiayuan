using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_ActTextSet : MonoBehaviour {

    public GameObject Obj_TextShow;
    public string TextShow;

	// Use this for initialization
	void Start () {
        //TextShow = "敌人向你发动攻击，要塞士兵受伤0人，死亡0人";
        Obj_TextShow.GetComponent<Text>().text = TextShow;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
