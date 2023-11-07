using UnityEngine;
using System.Collections;

public class YanChiEffect : MonoBehaviour {

    public GameObject effectObj;
    public float YanchiTime;
    private float yanchiTimeSum;
    private bool showStatus;    //展示状态

	// Use this for initialization
	void Start () {
        effectObj.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!showStatus) {
            yanchiTimeSum = yanchiTimeSum + Time.deltaTime;
            if (yanchiTimeSum >= YanchiTime)
            {
                effectObj.SetActive(true);
                showStatus = true;
            }
        }

	}
}
