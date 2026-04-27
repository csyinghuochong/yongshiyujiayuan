using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RewardGird : MonoBehaviour {

    public string RewardType;
    public string RewardTypeValue;
    public GameObject Obj_RewardType;
    public GameObject Obj_RewardTypeValue;


	// Use this for initialization
	void Start () {

        //显示资源图标
        string iconPath = Game_PublicClassVar.Get_function_UI.ResourceTypeReturnIconPath(RewardType);
        Debug.Log("iconPath = " + iconPath);
        object obj = Resources.Load(iconPath, typeof(Sprite));
        Sprite itemQuality = obj as Sprite;
        Obj_RewardType.GetComponent<Image>().sprite = itemQuality;

        //显示资源数量
        Obj_RewardTypeValue.GetComponent<Text>().text = RewardTypeValue;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
