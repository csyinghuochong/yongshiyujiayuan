using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RoseHp : MonoBehaviour {

    private Transform RoseBoneSet;
    public GameObject Obj_TaskRun;
    private Rose_Status rose_Status;

    //称谓
    public GameObject Obj_ChengWeiSet;
    public GameObject Obj_ChengWei;
    public ObscuredString ChengWeiID;                            //称谓ID
    public ObscuredString Rose_ChengWei;                        //角色称谓
    public bool RoseChengWeiStatus;                     //更新称谓状态
    public GameObject Obj_ChengWei_SP1;                 //称谓装饰1
    public GameObject Obj_ChengWei_SP2;                 //称谓装饰2
    public GameObject Obj_ChengWeiNameSet;
    public GameObject Obj_ChengWei_Img;
    public GameObject ObJ_RoseName;
    public GameObject Obj_Lan_Value;
    public GameObject Obj_NengLiangSet;
    public GameObject Obj_NengLiang_Value;
    public GameObject Obj_RoseName;
    public GameObject Obj_RoseLv;
    public GameObject Obj_PaiImg;

    private Vector3 RoseHeadZuoQiPosition;

    // Use this for initialization
    void Start () {
        RoseBoneSet = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Head.transform;
        rose_Status = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>();
        //显示当前成
        string nowChengHaoID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NowChengHaoID", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (nowChengHaoID != "")
        {
            Game_PublicClassVar.Get_function_Rose.ChengHao_Use(nowChengHaoID);
        }
        else {
            Obj_ChengWeiSet.SetActive(false);
        }

        //根据职业显示魔法值
        Obj_Lan_Value.SetActive(false);   //默认不显示
        Obj_NengLiangSet.SetActive(false);

        //战士不显示魔法
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1") {
            Obj_Lan_Value.SetActive(false);
        }
        //法师显示SP
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "2")
        {
            Obj_Lan_Value.SetActive(true);
        }
        //猎人显示能量
        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "3")
        {
            Obj_NengLiangSet.SetActive(true);
        }

        //更新阵营
        UpdateZhenYing();

        UpdatePosition();
    }
	
	// Update is called once per frame
	void Update () {

        //显示UI,并对其相应的属性修正
        UpdatePosition();
        Vector3 Hp_show_position = Camera.main.WorldToViewportPoint(RoseHeadZuoQiPosition);
        Hp_show_position = Game_PublicClassVar.Get_function_UI.RetrunScreenV2(Hp_show_position);

        //血条位置修正（根据分辨率的变化而变化）
        this.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(Hp_show_position.x, Hp_show_position.y + 20.0f, 0);

        if (rose_Status.TaskRunStatus == "2") {
            Obj_TaskRun.SetActive(true);
        }else{
            Obj_TaskRun.SetActive(false);
        }

        if (RoseChengWeiStatus)
        {
            RoseChengWeiStatus = false;
			if (Rose_ChengWei != ""&&Rose_ChengWei != "0")
            {
                Obj_ChengWeiSet.SetActive(true);
                
                //获取文字长度
                //1个字是9的长度
                Obj_ChengWei_SP1.transform.localPosition = new Vector3(-9 * Rose_ChengWei.Length - 15, Obj_ChengWei_SP1.transform.localPosition.y, Obj_ChengWei_SP1.transform.localPosition.z);
                Obj_ChengWei_SP2.transform.localPosition = new Vector3(9 * Rose_ChengWei.Length + 15, Obj_ChengWei_SP1.transform.localPosition.y, Obj_ChengWei_SP1.transform.localPosition.z);


                //设置颜色
                string colorStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ColorStr", "ID", ChengWeiID, "ChengHao_Template");
                if (colorStr != "" && colorStr != "0" && colorStr != null)
                {
                   
                    Rose_ChengWei = "<color=#" + colorStr + ">" + Rose_ChengWei + "</color>";
                }

                Obj_ChengWei.GetComponent<Text>().text = Rose_ChengWei;

            }
            else {
                Obj_ChengWeiSet.SetActive(false);
            }
        }
    }

    //更新坐标位置
    public void UpdatePosition() {

        if (RoseBoneSet == null) {
            RoseBoneSet = Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Bone>().Bone_Head.transform;
            //RoseHeadZuoQiPosition = RoseBoneSet.position;
        }

        if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().ZuoQiStatus)
        {
            RoseHeadZuoQiPosition = new Vector3(RoseBoneSet.position.x, RoseBoneSet.position.y + 1.0f, RoseBoneSet.position.z);
        }
        else {
            RoseHeadZuoQiPosition = new Vector3(RoseBoneSet.position.x, RoseBoneSet.position.y, RoseBoneSet.position.z);
        }

    }

    //更新阵营
    public void UpdateZhenYing() {

        //显示派系
        string zhenying = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ZhenYing", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        Obj_PaiImg.SetActive(false);
        if (zhenying == "" || zhenying == "0" || zhenying == null)
        {
            Obj_PaiImg.SetActive(false);
        }
        else
        {
            if (zhenying == "1")
            {
                object obj = Resources.Load("GameUI/Img/UI_Image_319", typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                Obj_PaiImg.GetComponent<Image>().sprite = itemIcon;
                Obj_PaiImg.SetActive(true);
            }

            if (zhenying == "2")
            {
                object obj = Resources.Load("GameUI/Img/UI_Image_318", typeof(Sprite));
                Sprite itemIcon = obj as Sprite;
                Obj_PaiImg.GetComponent<Image>().sprite = itemIcon;
                Obj_PaiImg.SetActive(true);
            }

            //战士修正位置
            if (Game_PublicClassVar.Get_game_PositionVar.Obj_Rose.GetComponent<Rose_Status>().RoseOcc == "1")
            {
                Obj_PaiImg.transform.localPosition = new Vector3(Obj_PaiImg.transform.localPosition.x, 0, Obj_PaiImg.transform.localPosition.z);
            }

        }

    }


}
