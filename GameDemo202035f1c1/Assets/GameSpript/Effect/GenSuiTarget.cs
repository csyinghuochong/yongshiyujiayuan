using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenSuiTarget : MonoBehaviour
{
    public GameObject Obj_Par;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Obj_Par==null) {
            Obj_Par = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose;
        }

        if (Obj_Par != null)
        {
            this.transform.position = Obj_Par.transform.position;
        }
    }
}
