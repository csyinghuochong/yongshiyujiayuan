using UnityEngine;
using System.Collections;

public class UI_Close : MonoBehaviour {

    public GameObject CloseUI;  //要关闭的UI

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ClossUI() {

        //注销UI
        Destroy(CloseUI);
        Game_PublicClassVar.Get_function_UI.PlaySource("10002","1");
        //播放音效
        /*
        Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.GetComponent<UI_GameSource>().UI_CloseUI.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.SourceSize;
        Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.GetComponent<UI_GameSource>().UI_CloseUI.GetComponent<AudioSource>().Play();
        */
    }

    //隐藏当前UI
    public void ClossUI_Hide()
    {
        //注销UI
        CloseUI.SetActive(false);
    }

    //建筑UI用的
    public void CloseUI_Hide() { 

        //播放音效
        Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.GetComponent<UI_GameSource>().UI_CloseUI.GetComponent<AudioSource>().volume = Game_PublicClassVar.Get_game_PositionVar.SourceSize;
        Game_PublicClassVar.Get_game_PositionVar.Obj_GameSourceSet.GetComponent<UI_GameSource>().UI_CloseUI.GetComponent<AudioSource>().Play();
        CloseUI.SetActive(false);
        Game_PublicClassVar.Get_game_PositionVar.OBJ_UI_Set.GetComponent<UI_Set>().Obj_BuildingMainUISet.GetComponent<BuildingMainUI>().ifHideMainUI = false;
    }
}
