using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_RanSeSet : MonoBehaviour {

	public GameObject Obj_RanSeList;
	public GameObject Obj_RanSeListSet_PuTong;
	public GameObject Obj_RanSeListSet_TeShu;
	public ObscuredString NowSelectRanSeID;

	public GameObject Obj_Show_NeedItemSet;
	public GameObject Obj_Show_DesSet;
    public GameObject Obj_Show_JiHuoSet;
    public GameObject Obj_Show_GaiBianSet;

    public GameObject Obj_RanSeDes;
    public GameObject Obj_RanSeItemID;
	public GameObject Obj_RanSeNum;

    public GameObject Obj_RanSeBtn_Hair;
    public GameObject Obj_RanSeBtn_Body;

    private ObscuredString RanSeIDSet_Body;
    private ObscuredString RanSeIDSet_Hair;
    private ObscuredString ranSeIDSet;

    private ObscuredString NowSelectID_Body;
    private ObscuredString NowSelectID_Hair;

    // Use this for initialization
    void Start () {

        //字段  Name Type YanSeIcon NeedItemNum Des     10000033  染色果实

        //界面适配
        Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(this.gameObject);

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RanSeListSet_PuTong);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RanSeListSet_TeShu);

        RanSeIDSet_Body = "10001,10002,10003,20001,20002";
        RanSeIDSet_Hair = "30001,30002,30003,40001,40002";

        //默认显示身体
        Btn_Body();

        //打开摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipRoseSet.SetActive(true);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


    public void Init() {

        //清理
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RanSeListSet_PuTong);
        Game_PublicClassVar.Get_function_UI.DestoryTargetObj(Obj_RanSeListSet_TeShu);

        string[] ranseIDList = ranSeIDSet.ToString().Split(',');
        //string dropChance = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Name", "ID", ranseIDList[0], "RanSe_Template");
        for (int i = 0; i < ranseIDList.Length; i++)
        {
            GameObject obj = (GameObject)Instantiate(Obj_RanSeList);

            string type = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Type", "ID", ranseIDList[i], "RanSe_Template");
            if (type == "1")
            {
                obj.transform.SetParent(Obj_RanSeListSet_PuTong.transform);
            }
            if (type == "2")
            {
                obj.transform.SetParent(Obj_RanSeListSet_TeShu.transform);
            }
            obj.transform.localScale = new Vector3(1, 1, 1);
            obj.GetComponent<UI_RanSeList>().RanSeID = ranseIDList[i];
            obj.GetComponent<UI_RanSeList>().Init();
            obj.GetComponent<UI_RanSeList>().Obj_Par = this.gameObject;
        }

        //默认现实第一个
        NowSelectRanSeID = ranseIDList[0];
        string nowPosi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Posi", "ID", NowSelectRanSeID, "RanSe_Template");

        if (nowPosi == "1" && NowSelectID_Body!="" && NowSelectID_Body != null)
        {
            NowSelectRanSeID = NowSelectID_Body ;
        }

        if (nowPosi == "2" && NowSelectID_Hair != "" && NowSelectID_Hair != null)
        {
            NowSelectRanSeID = NowSelectID_Hair;
        }

        SelectRanSe();

        //初始化写入初始ID
        string roseYanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (roseYanSeIDSet.Contains("10001") == false|| roseYanSeIDSet.Contains("30001") == false)
        {
            if (roseYanSeIDSet == "" || roseYanSeIDSet == "0" || roseYanSeIDSet == null)
            {
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanSeIDSet", "10001;30001", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }
            else
            {
                roseYanSeIDSet = roseYanSeIDSet + ";" + "10001" + ";" + "30001";
                Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("YanSeIDSet", roseYanSeIDSet, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
                Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");
            }
        }
    }

    //切换头部
    public void Btn_Hair() {

        Obj_RanSeBtn_Hair.SetActive(true);
        Obj_RanSeBtn_Body.SetActive(false);

        ranSeIDSet = RanSeIDSet_Hair;      //默认显示身体
        Init();     //初始化

    }

    //切换身体
    public void Btn_Body() {

        Obj_RanSeBtn_Hair.SetActive(false);
        Obj_RanSeBtn_Body.SetActive(true);

        ranSeIDSet = RanSeIDSet_Body;      //默认显示身体
        Init();     //初始化

    }


    //显示选中
	public void SelectRanSe() {

		//描述
		string des = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Des", "ID", NowSelectRanSeID, "RanSe_Template");
		string needNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum", "ID", NowSelectRanSeID, "RanSe_Template");

		//
		Obj_RanSeDes.GetComponent<Text>().text = des;

        string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID", "ID", NowSelectRanSeID, "RanSe_Template");
        int nowItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(needItemID);
		Obj_RanSeNum.GetComponent<Text>().text = nowItemNum + "/" + needNum;

        //显示图片
        string ItemIcon = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemIcon", "ID", needItemID, "Item_Template");
        //职业处理
        //string ItemQuality = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("ItemQuality", "ID", ItemID, "Item_Template");
        object obj = Resources.Load("ItemIcon/" + ItemIcon, typeof(Sprite));
        Sprite itemIcon = obj as Sprite;
        Obj_RanSeItemID.GetComponent<Image>().sprite = itemIcon;

        string roseYanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        if (roseYanSeIDSet.Contains(NowSelectRanSeID) == false)
        {
            Obj_Show_JiHuoSet.SetActive(true);
            Obj_Show_GaiBianSet.SetActive(false);

            string needItemNum = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum", "ID", NowSelectRanSeID, "RanSe_Template");

            if (needItemNum == "0" || needItemNum == "")
            {
                Obj_Show_NeedItemSet.SetActive(false);
                Obj_Show_DesSet.SetActive(true);
            }
            else
            {
                Obj_Show_NeedItemSet.SetActive(true);
                Obj_Show_DesSet.SetActive(false);
            }
        }
        else {
            Obj_Show_JiHuoSet.SetActive(false);
            Obj_Show_GaiBianSet.SetActive(true);
        }

        //显示当前选择的外形
        string nowPosi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Posi", "ID", NowSelectRanSeID, "RanSe_Template");
        string nowYanSe = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeID", "ID", NowSelectRanSeID, "RanSe_Template");
        Game_PublicClassVar.Get_function_Rose.RoseModelChange(nowYanSe, NowSelectRanSeID,true, nowPosi);

        if (nowPosi == "1") {
            NowSelectID_Body = NowSelectRanSeID;
        }

        if (nowPosi == "2"){
            NowSelectID_Hair = NowSelectRanSeID;
        }

    }

    //改变外形
    public void Btn_ChangeModel() {

		string roseYanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		if (roseYanSeIDSet.Contains(NowSelectRanSeID) == false)
		{
			Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前未拥有此颜色!");
			//return;
		}

        string nowPosi = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("Posi", "ID", NowSelectRanSeID, "RanSe_Template");

        if (nowPosi == "1" )
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowYanSeID", NowSelectRanSeID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

        if (nowPosi == "2")
        {
            Game_PublicClassVar.Get_function_DataSet.DataSet_WriteData("NowYanSeHairID", NowSelectRanSeID, "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
        }

		Game_PublicClassVar.Get_function_DataSet.DataSet_SetXml("RoseData");

        //刷新显示
        Game_PublicClassVar.Get_function_Rose.RoseModelChangeValue();

    }

    //激活按钮
    public void Btn_JiHuo() {

		string roseYanSeIDSet = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("YanSeIDSet", "ID", Game_PublicClassVar.Get_wwwSet.RoseID, "RoseData");
		if (roseYanSeIDSet.Contains(NowSelectRanSeID)) {
			Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("当前已拥有此颜色!");
			return;
        }

        string needItemID = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemID", "ID", NowSelectRanSeID, "RanSe_Template");

        if (needItemID == "" || needItemID == "0" || needItemID == null) {
            return;
        }

        int nowItemNum = Game_PublicClassVar.Get_function_Rose.ReturnBagItemNum(needItemID);
		string needNumStr = Game_PublicClassVar.Get_function_DataSet.DataSet_ReadData("NeedItemNum", "ID", NowSelectRanSeID, "RanSe_Template");
        if (needNumStr == "" || needNumStr == "0"|| needNumStr == null) {
            return;
        }

		if (nowItemNum >= int.Parse(needNumStr)) {
			Game_PublicClassVar.Get_function_Rose.CostBagItem(needItemID, int.Parse(needNumStr));
			Game_PublicClassVar.Get_function_Rose.RoseYanSeAdd(NowSelectRanSeID);
            Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("恭喜你已成功激活!");
            //刷新显示
            SelectRanSe();
        }

    }


    public void Btn_Close()
    {
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {

        Game_PublicClassVar.Get_function_Rose.RoseModelChangeValue();

        //关闭摄像机
        Game_PublicClassVar.Get_game_PositionVar.Obj_RoseModelSheXiangJi.transform.parent.GetComponent<CameraModel>().Obj_EquipRoseSet.SetActive(false);

    }

}
