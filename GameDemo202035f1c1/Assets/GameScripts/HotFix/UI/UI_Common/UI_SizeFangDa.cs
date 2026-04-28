using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UI_SizeFangDa : MonoBehaviour {
    private float size = 1;
    public GameObject Obj_Img;
    private float fangdaValue;
	// Use this for initialization
	void Start () {
        fangdaValue = 1.5f;

    }
	
	// Update is called once per frame
	void Update () {
        size = size + Time.deltaTime * 1.5f;
        this.gameObject.transform.localScale = new Vector3(size, size, size);

        float toumingValue = (fangdaValue - size)/(fangdaValue-1.1f);
        if (toumingValue < 0) {
            toumingValue = 0;
        }
        Obj_Img.GetComponent<Image>().color = new Color(1, 1, 1, toumingValue);
        if (size >= fangdaValue) {
            Destroy(this.gameObject);
        }
    }
}
