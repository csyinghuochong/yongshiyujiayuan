using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_RosePastureBag : MonoBehaviour {

    public GameObject Obj_BagSet;

	// Use this for initialization
	void Start () {

        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePastureBag_Status = true;
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_RosePastureBag = this.gameObject;
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_BagSet);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().RosePastureBag_Status = false;
        Game_PublicClassVar.Get_game_PositionVar.Obj_UI_FunctionOpen.GetComponent<UI_FunctionOpen>().Obj_RosePastureBag = null;
    }


}
