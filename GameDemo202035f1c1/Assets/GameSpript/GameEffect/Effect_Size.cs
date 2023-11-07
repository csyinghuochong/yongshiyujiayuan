using UnityEngine;
using System.Collections;

public class Effect_Size : MonoBehaviour {

    public bool EffectChangeStatus;
    public float EffectChangeTime;          //模型改变时间
    public float EffectChangeSize;          //模型改变大小
    public string EffectType;               //默认只有放大    1表示放大加缩小
    private float effectChangeSize_Minute;
    private float effectTimeSum;
    private float effectChangeSize_Now;     //模型改变大小

	// Use this for initialization
	void Start () {
        effectChangeSize_Now = this.transform.localScale.x;
        effectChangeSize_Minute = (EffectChangeSize - effectChangeSize_Now) / EffectChangeTime;
        EffectChangeStatus = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (EffectChangeStatus)
        {
            effectTimeSum = effectTimeSum + Time.deltaTime;
            if (effectTimeSum < EffectChangeTime)
            {
                float nowSize = effectChangeSize_Now + (EffectChangeSize - effectChangeSize_Now) * (effectTimeSum / EffectChangeTime);
                this.transform.localScale = new Vector3(nowSize, nowSize, nowSize);
            }
            else {
                if (EffectType == "1")
                {
                    //Debug.Log("重置为0");
                    EffectChangeStatus = false;
                    effectTimeSum = 0;
                }
            }
        }
        else {
            effectTimeSum = effectTimeSum + Time.deltaTime;
            if (effectTimeSum < EffectChangeTime)
            {
                float nowSize = EffectChangeSize - (EffectChangeSize - effectChangeSize_Now) * (effectTimeSum / EffectChangeTime);
                this.transform.localScale = new Vector3(nowSize, nowSize, nowSize);
            }
            else
            {
                if (EffectType == "1")
                {
                    //Debug.Log("重置为1");
                    EffectChangeStatus = true;
                    effectTimeSum = 0;
                }
            }
        }
	}
}
