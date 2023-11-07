using UnityEngine;
using System.Collections;

public class Rose_SelectTarget : MonoBehaviour {

    public GameObject Obj_SelectEffect;
    public float SelectEffectSize;

	// Use this for initialization
	void Start () {
        Obj_SelectEffect.GetComponent<ParticleSystem>().startSize = SelectEffectSize*3.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
