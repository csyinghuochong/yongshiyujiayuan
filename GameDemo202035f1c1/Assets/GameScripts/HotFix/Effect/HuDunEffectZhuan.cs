using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuDunEffectZhuan : MonoBehaviour
{
    float jiaodu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        jiaodu = jiaodu + Time.deltaTime * 60;
        if (jiaodu >= 360) {
            jiaodu = 0;
        }
        this.transform.localRotation = Quaternion.Euler(0, jiaodu, 0);
    }
}
