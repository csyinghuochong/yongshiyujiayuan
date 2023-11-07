using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaiGuaJianCe : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //退出
    public void Exit() {
        Debug.Log("检测到加速,退出游戏");
        //Application.Quit();
        Game_PublicClassVar.Get_wwwSet.ExitGame();
    }
}
