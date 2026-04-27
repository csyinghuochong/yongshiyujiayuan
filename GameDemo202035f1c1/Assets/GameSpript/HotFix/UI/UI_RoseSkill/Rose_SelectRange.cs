using UnityEngine;
using System.Collections;

public class Rose_SelectRange : MonoBehaviour {

    public float RangeSize;
    public GameObject Obj_LightEffect_1;
    public GameObject Obj_LightEffect_2;
    public GameObject Obj_LightEffect_3;
    private float timeSum;

	// Use this for initialization
	void Start () {
        Obj_LightEffect_1.GetComponent<Projector>().fieldOfView = 20 * RangeSize;
        Obj_LightEffect_2.GetComponent<Projector>().fieldOfView = 20 * RangeSize;
        Obj_LightEffect_3.GetComponent<Projector>().fieldOfView = 20 * RangeSize;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        timeSum = timeSum + Time.deltaTime;
        //超过10秒直接注销
        if (timeSum >= 10) {
            Destroy(this.gameObject);
        }
        */
    }
}
