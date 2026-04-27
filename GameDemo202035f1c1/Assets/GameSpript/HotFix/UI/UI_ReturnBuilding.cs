using UnityEngine;
using System.Collections;

public class UI_ReturnBuilding : MonoBehaviour
{

    public GameObject ReturnBuildingObj;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Btn_ReturnBuilding() {
        ReturnBuildingObj.GetComponent<UI_StartGame>().Btn_RetuenBuilding();
        Btn_CloseUI();
    }

    //关闭
    public void Btn_CloseUI()
    {
        Destroy(this.gameObject);
    }

}
