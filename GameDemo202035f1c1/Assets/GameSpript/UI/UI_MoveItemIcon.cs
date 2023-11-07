using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MoveItemIcon : MonoBehaviour {

    public GameObject Obj_ItemIcon;
    public Sprite itemIconSprite;

	// Use this for initialization
	void Start () {

        Obj_ItemIcon.GetComponent<Image>().sprite = itemIconSprite;
	
	}
	
	// Update is called once per frame
	void Update () {

        //跟着当前鼠标移动
        //Debug.Log(Input.mousePosition);
        //this.transform.localPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0);
        this.transform.localPosition = Game_PublicClassVar.Get_function_UI.UIMoveIconPosition(Input.mousePosition.x, Input.mousePosition.y);
	}
}
