using UnityEngine;
using System.Collections;

public class YanChiCreateEffect : MonoBehaviour {

    public GameObject effectObjPrent;
    public GameObject effectObj;
    public float YanchiTime;
    private float yanchiTimeSum;
    private bool showStatus;    //展示状态

	// Use this for initialization
	void Start () {
        effectObjPrent.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () {
        if (!showStatus) {
            yanchiTimeSum = yanchiTimeSum + Time.deltaTime;
            if (yanchiTimeSum >= YanchiTime)
            {
                effectObjPrent.SetActive(true);
                showStatus = true;

                GameObject obj = (GameObject)Instantiate(effectObj, effectObjPrent.transform);
                obj.transform.SetParent(effectObjPrent.transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
            }
        }

	}
}
