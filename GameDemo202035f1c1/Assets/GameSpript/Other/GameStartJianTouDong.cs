using UnityEngine;
using System.Collections;

public class GameStartJianTouDong : MonoBehaviour {
    private float DongTimeSum;
    public GameObject DongObj;
    private bool move_Left;     //左
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        DongTimeSum = DongTimeSum + Time.deltaTime * 2;
        if (DongTimeSum >= 1)
        {
            DongTimeSum = 0;
        }
        DongObj.transform.localPosition = new Vector3(DongTimeSum * 40.0f, 0, 0);

        /*
        if (!move_Left)
        {
            DongTimeSum = DongTimeSum + Time.deltaTime*2;
            if (DongTimeSum >= 1)
            {
                //DongTimeSum = 1;
                move_Left = true;
            }

        }
        else {
            DongTimeSum = DongTimeSum - Time.deltaTime*2;
            if (DongTimeSum <= 0)
            {
                //DongTimeSum = 0;
                move_Left = false;
            }

        }
        DongObj.transform.localPosition = new Vector3(DongTimeSum * 40.0f, 0, 0);
         */
	}
}
