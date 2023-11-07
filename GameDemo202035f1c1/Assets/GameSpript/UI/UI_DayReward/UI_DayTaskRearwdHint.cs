using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DayTaskRearwdHint : MonoBehaviour {

    public GameObject Obj_DayTaskRewardTitleHint;
    public GameObject Obj_DayTaskRewardHint;
    public GameObject Obj_ImgDi;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //关闭界面
    public void CloseUI() {
        Destroy(this.gameObject);
    }
}
