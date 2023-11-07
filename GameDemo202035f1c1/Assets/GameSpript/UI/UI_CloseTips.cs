using UnityEngine;
using System.Collections;

public class UI_CloseTips : MonoBehaviour {

    public GameObject TipsParent;   //父级Tips
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //父级为空时关闭此UI
        if (TipsParent == null) {
            Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
        }
	}

    public void CloseTipsUI() {
        //this.gameObject.SetActive(false);
        Game_PublicClassVar.Get_function_UI.DestoryTipsUI();
    }
}
