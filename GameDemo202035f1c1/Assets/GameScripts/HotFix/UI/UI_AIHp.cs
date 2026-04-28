using UnityEngine;
using System.Collections;

public class UI_AIHp : MonoBehaviour {

    public GameObject Obj_aiName;
    public GameObject Obj_aiLv;
    public GameObject Obj_aiImgValue;
    public GameObject Obj_aiRoseName;
    public GameObject Obj_AI;
    public AI_1 Obj_ai_1;
    public AIPet Obj_aiPet;

	// Use this for initialization
	void Start () {

        if(Obj_AI.GetComponents<AI_1>().Length >= 1){
            Obj_ai_1 = Obj_AI.GetComponent<AI_1>();
        }
        if (Obj_AI.GetComponents<AIPet>().Length >= 1)
        {
            Obj_aiPet = Obj_AI.GetComponent<AIPet>();
        }
        
    }
	
	// Update is called once per frame
	void Update () {

        if (Obj_ai_1 != null){

            if (Obj_ai_1.ai_IfDeath)
            {
                Destroy(this.gameObject);
            }

        }

        if (Obj_AI == null)
        {
            Destroy(this.gameObject);
        }
        else {
            if (Obj_aiPet != null) {
                if (Obj_aiPet.UI_Hp != this.gameObject) {
                    Destroy(this.gameObject);
                }
            }
        }



    }
}
