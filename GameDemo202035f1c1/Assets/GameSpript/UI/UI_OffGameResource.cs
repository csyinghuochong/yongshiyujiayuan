using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_OffGameResource : MonoBehaviour {

    public GameObject Obj_BuildingGold;         //建筑金币
    public GameObject Obj_Food;	                //粮食
    public GameObject Obj_Wood;	                //木材
    public GameObject Obj_Stone;	            //石头
    public GameObject Obj_Iron;	                //钢铁
    public GameObject Obj_RoseExp;              //角色经验

    public int BuildingGold;            //建筑金币
    public int Food;	                //粮食
    public int Wood;	                //木材
    public int Stone;	                //石头
    public int Iron;	                //钢铁
    public int RoseExp;                 //角色经验


    public GameObject Obj_CountryExp;
    public GameObject Obj_CountryHonor;
    public int CountryExp;
    public int CountryHonor;


	// Use this for initialization
	void Start () {
        //更新资源显示
        /*
        updateResourceValue(BuildingGold, Obj_BuildingGold);
        updateResourceValue(Food, Obj_Food);
        updateResourceValue(Wood, Obj_Wood);
        updateResourceValue(Stone, Obj_Stone);
        updateResourceValue(Iron, Obj_Iron);
        updateResourceValue(RoseExp, Obj_RoseExp);
        */

        updateResourceValue(CountryExp, Obj_CountryExp);
        updateResourceValue(CountryHonor, Obj_CountryHonor);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void updateResourceValue(int resourceValue, GameObject showObj){

        if (resourceValue > 10000)
        {
            float value = (float)(resourceValue / 10000.0f);
            showObj.GetComponent<Text>().text = value.ToString("F1") + "W";
        }
        else {
            showObj.GetComponent<Text>().text = resourceValue.ToString();
        }

    }

    public void Btn_Close() {
        Destroy(this.gameObject);
    }
}
