using UnityEngine;
using System.Collections;

public class MapCameraAI : MonoBehaviour
{

    public GameObject FlowObj;  //跟随移动的物体
    //private float cushionSpeed;     //缓冲移动速度
    //private RoseStatus roseObj;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.localPosition = new Vector3(FlowObj.transform.position.x, 35.0f, FlowObj.transform.position.z);

    }
}
