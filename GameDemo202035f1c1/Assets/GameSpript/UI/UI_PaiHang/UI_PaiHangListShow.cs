using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PaiHangListShow : MonoBehaviour {

    public bool IfNullStatus;

    public string rosePaiHangValue;
    public string roseName;
    public string roseLv;
    public string roseOcc;              //1表示战士  2:表示法师
    public string roseEquipStr;         
    public string roseEquipHideStr;
    public string roseShiLiValue;
    public string roseNowYanSeID;
    public string roseNowNowYanSeHairID;


    public string[] roseEquipIDList;
    public Dictionary<string, string> roseEquipIDDic = new Dictionary<string, string>();            //装备ID,隐藏属性ID
    public Dictionary<string, string> roseEquipHideDic = new Dictionary<string, string>();

    public GameObject Obj_RosePaiHangValue;
    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_RoseOccImg;
    public GameObject Obj_RoseShiLiValue;
    public GameObject Obj_RoseChaKanBtn;
    public GameObject Obj_RosePaiHangDiImg;
    public GameObject[] Obj_RosePaiHangSanShowList;

    public GameObject Obj_PaiHangRoseEquipShow;
    private GameObject obj_PaiHangRoseEquipShow;

	public GameObject Obj_PaiHangRosePet;

	//服务传输相关数据
	public string Server_PetDataStr;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		
	}

    //显示玩家信息
    public void ShowPlayListData() {



        if (IfNullStatus == true) {

            Obj_RoseName.GetComponent<Text>().text = "暂无玩家";
            Obj_RoseLv.GetComponent<Text>().text = "Lv.0";
            //Obj_RosePaiHangValue.GetComponent<Text>().text = "0";
            Obj_RoseShiLiValue.GetComponent<Text>().text = "0";
            Obj_RoseChaKanBtn.SetActive(false);
            return;
        }

        Obj_RoseName.GetComponent<Text>().text = roseName;
        Obj_RoseLv.GetComponent<Text>().text = "Lv." + roseLv;
        Obj_RoseShiLiValue.GetComponent<Text>().text = roseShiLiValue;

        //显示底图
        if (int.Parse(rosePaiHangValue) % 2 == 0) {
            Obj_RosePaiHangDiImg.GetComponent<Image>().color = new Color(0.86f,0.83f,0.71f);
        }

        //排名显示
        Obj_RosePaiHangValue.GetComponent<Text>().text =  rosePaiHangValue;
        if (rosePaiHangValue == "1") {
            Obj_RosePaiHangSanShowList[0].SetActive(true);
            Obj_RosePaiHangValue.SetActive(false);
        }
        if (rosePaiHangValue == "2")
        {
            Obj_RosePaiHangSanShowList[1].SetActive(true);
            Obj_RosePaiHangValue.SetActive(false);
        }
        if (rosePaiHangValue == "3")
        {
            Obj_RosePaiHangSanShowList[2].SetActive(true);
            Obj_RosePaiHangValue.SetActive(false);
        }


        //补充头像显示


        EquipHideID();
    }

    //将玩家的隐藏属性加载到Dic中方便调用
    public void EquipHideID() {

        //装备列表
        if (roseEquipStr != ""&& roseEquipStr!=null) {
            roseEquipIDList = roseEquipStr.Split('|');
            //Debug.Log("roseEquipStr = " + roseEquipStr);
            /*
            //装备和隐藏属性ID
            for (int i = 0; i < roseEquipIDList.Length; i++)
            {
                string[] hideID = roseEquipIDList[i].Split(',');
                //hideID[0] 为0表示没有装备
                if (hideID[0] != "0")
                {
                    roseEquipIDDic.Add(hideID[0], hideID[1]);
                }
            }
            */
        }


        //隐藏属性ID和隐藏值
        if (roseEquipHideStr != ""&& roseEquipHideStr != null) {
            string[] hideList = roseEquipHideStr.Split(']');
            //Debug.Log("roseEquipHideStr = " + roseEquipHideStr);
            for (int i = 0; i < hideList.Length; i++)
            {
                //Debug.Log("hideList[i] = " + hideList[i]);
                string[] hideIDPro = hideList[i].Split('[');
                //hideIDPro[0] 为0表示没有装备
                if (hideIDPro[0] != "0")
                {
                    roseEquipHideDic.Add(hideIDPro[0], hideIDPro[1]);
                }
            }
        }
    }

	//展示装备
    public void Btn_ShowEquip() {

        if (obj_PaiHangRoseEquipShow != null)
        {
            Destroy(obj_PaiHangRoseEquipShow);
        }
        else {
            Debug.Log("打开装备界面");
            obj_PaiHangRoseEquipShow = (GameObject)Instantiate(Obj_PaiHangRoseEquipShow);
            obj_PaiHangRoseEquipShow.transform.SetParent(Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.transform);
            obj_PaiHangRoseEquipShow.transform.localPosition = new Vector3(40,0,0);
            obj_PaiHangRoseEquipShow.transform.localScale = new Vector3(1, 1, 1);
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseLv = roseLv;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseName = roseName;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseOcc = roseOcc;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipIDList = roseEquipIDList;
            //obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipIDDic = roseEquipIDDic;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().roseEquipHideDic = roseEquipHideDic;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().nowYanSeID = roseNowYanSeID;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().nowNowYanSeHairID = roseNowNowYanSeHairID;
            obj_PaiHangRoseEquipShow.GetComponent<UI_PaiHangRoseEquipShow>().ShowEquip();
        }
    }

	//展示宠物信息
	public void Btn_ShowPet(){

		string[] petIDSetList = Server_PetDataStr.Split('|');
		GameObject showPetobj = (GameObject)Instantiate(Obj_PaiHangRosePet);
		showPetobj.transform.SetParent(Game_PublicClassVar.Get_gameServerObj.Obj_PaiHang.transform);
		showPetobj.GetComponent<UI_PaiHangShowPet>().SerVer_PetDataList = petIDSetList;
        showPetobj.GetComponent<UI_PaiHangShowPet>().roseEquipHideDic = roseEquipHideDic;
        showPetobj.GetComponent<UI_PaiHangShowPet> ().Inti();
		showPetobj.transform.localScale = new Vector3(1, 1, 1);
		showPetobj.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
		showPetobj.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        


	}
}
