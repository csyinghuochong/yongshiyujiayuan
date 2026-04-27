using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GameGirdHintSingle : MonoBehaviour
{

	public string HintText;
    public string HintColorValue;
	public GameObject Obj_HintText;
	private float SizeTime;             //字放大时间
	private float DamgeFlyTimeSum;      //伤害时间
    public bool HintStatus;
    private float DestroyTime;          //销毁时间
    private float DestroyTimeSum;       //销毁时间累加

	void Start () {
		transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-15.0f, transform.localPosition.z);
		SizeTime = 0.0f;

        if (HintColorValue == "" || HintColorValue == null)
        {
            Obj_HintText.GetComponent<Text>().text = HintText;      //<color=#ff0000ff>  (攻击力不足)</color>
        }
        else {
            Obj_HintText.GetComponent<Text>().text = "<color=#" + HintColorValue + ">" + HintText + "</color>";      //<color=#ff0000ff>  (攻击力不足)</color>
        }

        DestroyTime = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {

        if (HintStatus) {

            DamgeFlyTimeSum = DamgeFlyTimeSum + Time.deltaTime;
            if (DamgeFlyTimeSum < 0.1f)
            {
                transform.localScale = new Vector3(1f, 1f, 1);
                if (DamgeFlyTimeSum < 0.03f)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }

            //Game_PublicClassVar.Get_function_UI.ReturnScreen_Y(4.0f)
            float flyspeed = 4.0f * (30.0f / (float)(Application.targetFrameRate));
            if (flyspeed >= 4.0f) {
                flyspeed = 4.0f;
            }
            if (flyspeed <= 1) {
                flyspeed = 1;
            }

            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + flyspeed, transform.localPosition.z);

            DestroyTimeSum = DestroyTimeSum + Time.deltaTime;
            if (DestroyTimeSum >= DestroyTime) {
                //销魂自身
                Destroy(this.gameObject);
            }
        }	
	}
}
