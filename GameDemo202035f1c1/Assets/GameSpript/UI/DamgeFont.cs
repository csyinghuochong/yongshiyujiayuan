using UnityEngine;
using System.Collections;

public class DamgeFont : MonoBehaviour {

	//public int DamgeValue;
    private float SizeTime;         //字放大时间
    private float DamgeFlyTimeSum;       //伤害时间
	// Use this for initialization
	void Start () {

        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-15.0f, transform.localPosition.z);

        SizeTime = 0.1f;

	}
	
	// Update is called once per frame
	void Update () {

        DamgeFlyTimeSum = DamgeFlyTimeSum + Time.deltaTime;
        if (DamgeFlyTimeSum < 0.1f)
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
            if (DamgeFlyTimeSum < 0.03f) {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else {
            transform.localScale = new Vector3(1,1,1);
        }
        transform.localPosition =new Vector3(transform.localPosition.x, transform.localPosition.y + 2f,transform.localPosition.z);
		Destroy (this.gameObject,0.3f);
	
	}
}
