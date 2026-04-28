using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_RoseGetItemHint : MonoBehaviour {

	public string UpdataHintText;
	public bool UpdataHint;
	public GameObject Obj_HintText;
	private string hintText;
	private int hintNum;
	private int hintPosition;
	private int deleteTime;
	//private float hintPosition;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		if (UpdataHint) {
			UpdataHint = false;

			hintNum = hintNum + 1;
			hintPosition = hintPosition +1;
			GameObject obj_hintText = (GameObject)Instantiate(Obj_HintText);
			obj_hintText.transform.SetParent(this.transform);
			obj_hintText.transform.localScale=new Vector3(1,1,1);
			obj_hintText.GetComponent<Text>().text = UpdataHintText;
			obj_hintText.transform.localPosition = new Vector3(35,46-hintPosition*16,0);
			UpdataHintText = "";
			if(hintNum>5){
				this.transform.localPosition = new Vector3(this.transform.localPosition.x,this.transform.localPosition.y+16.0f,this.transform.localPosition.z);
			}

			//超过10个清理不显示的UI
			if(hintNum>=10){
				hintNum = 5;
				//循环阐述子控件
				for(int i = 0;i<5;i++)
				{
					GameObject go = this.transform.GetChild(i).gameObject;
					Destroy(go);
				}
			}
		}
	}
}
