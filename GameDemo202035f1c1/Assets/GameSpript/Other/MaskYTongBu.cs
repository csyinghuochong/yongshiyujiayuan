using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskYTongBu : MonoBehaviour {

	public GameObject Obj_TongBu;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (Obj_TongBu != null)
		{
			this.transform.localPosition = new Vector3(this.transform.localPosition.x,Obj_TongBu.transform.localPosition.y, this.transform.localPosition.z) ;
		}
	}
}
