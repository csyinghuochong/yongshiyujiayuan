using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rose_SkillRange : MonoBehaviour {
    private GameObject RoseObj;
	// Use this for initialization
	void Start () {
        RoseObj = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position = RoseObj.transform.position;
    }
}
