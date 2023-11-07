using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillAreaHelper : MonoBehaviour 
{
    public GameObject prefab;
    public float angle = 60;
    public Material mtl;
    public Transform left;
    public Transform right;
	void Start () 
    {

	}
	
	void Update () 
    {


        float a = Mathf.PI * this.angle / 180;
        mtl.SetFloat("_Angle", a);
        left.localEulerAngles = Vector3.up * angle ;
        right.localEulerAngles = -Vector3.up * angle;
	}

}
