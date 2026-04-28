using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PastureKuangShowTitle : MonoBehaviour
{

    public GameObject Obj_KuangShow;
    public string KuangSpaceID;
    public GameObject[] Obj_PetHeadIcon;
    public bool WeiKaiFaStatus;
    public GameObject Obj_PetSet;
    public GameObject Obj_LvHintSet;
    public GameObject Obj_LvHintDes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj != null)
        {
            if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().ShowKuangTitleStatus)
            {
                Init();
                //Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().ShowKuangTitleStatus = false;
            }
        }
    }

    private void LateUpdate()
    {
        if (Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().ShowKuangTitleStatus)
        {
            Game_PublicClassVar.Get_game_PositionVar.NowPastureSetObj.GetComponent<PastureSet>().ShowKuangTitleStatus = false;
        }
    }

    public void Init() {

        //展示宠物头像
        if (WeiKaiFaStatus == false)
        {
            Obj_LvHintSet.SetActive(false);

            string nowPastureKuangSetStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PastureKuangSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RosePastureData");

            string[] nowPastureKuangSetStrList = nowPastureKuangSetStr.Split('@');
            //for (int i = 0; i < nowPastureKuangSetStrList.Length; i++)
            //{

            //Debug.Log("nowPastureKuangSetStrList.Length = " + nowPastureKuangSetStrList.Length + " KuangSpaceID = " + KuangSpaceID + "name = " + this.gameObject.name);
            string[] nowPastureKuangList = nowPastureKuangSetStrList[int.Parse(KuangSpaceID) - 1].Split(';');

            string kuangName = Game_PublicClassVar.Get_function_Pasture.PastureKuang_GetTypeName(nowPastureKuangList[0]);
            //Debug.Log("kuangName = " + kuangName + "nowPastureKuangSetStrList[0] = " + nowPastureKuangSetStrList[0]);
            Obj_KuangShow.GetComponent<Text>().text = kuangName;

            if (nowPastureKuangList.Length >= 3)
            {
                //设置驻守
                string[] petList = nowPastureKuangList[1].Split(',');
                for (int y = 0; y < petList.Length; y++)
                {
                    //显示宠物头像
                    if (petList[y] != "" && petList[y] != "0" && petList[y] != null)
                    {

                        //获取宠物名称
                        string petName = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetName", "ID", petList[y], "RosePet");

                        //获取宠物等级
                        string petLv = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetLv", "ID", petList[y], "RosePet");

                        //获取宠物头像Icon
                        string petID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("PetID", "ID", petList[y], "RosePet");
                        string petHeadIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("HeadIcon", "ID", petID, "Pet_Template");

                        //显示宠物信息
                        //Obj_PetName[y].GetComponent<Text>().text = petName;

                        //显示底图
                        Object obj = Resources.Load("PetHeadIcon/" + petHeadIcon, typeof(Sprite));
                        Sprite img = obj as Sprite;
                        Obj_PetHeadIcon[y].GetComponent<Image>().sprite = img;
                    }
                }
            }
        }
        else {
            Obj_KuangShow.GetComponent<Text>().text = "未开发的矿脉";
            Obj_PetSet.SetActive(false);
            Obj_LvHintSet.SetActive(true);

            string lvHintStr = "";
            if (KuangSpaceID == "2") {
                lvHintStr = "等级达到35级开启";
            }

            if (KuangSpaceID == "3")
            {
                lvHintStr = "等级达到45级开启";
            }
            //Debug.Log("lvHintStr = " + lvHintStr);
            Obj_LvHintDes.GetComponent<Text>().text = lvHintStr;
        }
    }

    private void OnDestroy()
    {
        //Debug.Log("我被注销啦...");
    }
}
