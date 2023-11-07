using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_MainUIRoseExp : MonoBehaviour {
	
	public bool UpdataRoseExp;
	public GameObject Obj_RoseExpValue;
	public GameObject Obj_RoseExpPro;

	// Use this for initialization
	void Start () {
		UpdataRoseExp = true;
	}
	
	// Update is called once per frame
	void Update () {
		//更新当前经验
		if (UpdataRoseExp) {

			//获取当前经验和总经验
			long nowExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_ExpNow;
			long sumExp = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Proprety>().Rose_Exp;
            if (sumExp != 0) {
                float value = (float)nowExp / (float)sumExp;
                Obj_RoseExpPro.GetComponent<Image>().fillAmount = value;
                Obj_RoseExpValue.GetComponent<Text>().text = nowExp.ToString() + "/" + sumExp.ToString();
                UpdataRoseExp = false;
            }

            //Debug.Log("显示当前经验 nowExp = " + nowExp + "sumExp = " + sumExp);
		}
	
	}
}
