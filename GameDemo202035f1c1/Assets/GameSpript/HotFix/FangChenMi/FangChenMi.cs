using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FangChenMi : MonoBehaviour {
    public GameObject Obj_Name;
    public GameObject Obj_ShenFenID;
    public GameObject Obj_HintSet;
    public GameObject Obj_HintSetText;
    public GameObject Obj_HintText;
    public GameObject Obj_ShenFenRenZhengSucessHintSet;
    private string roseName;
    private string roseShenFenID;

    // Use this for initialization
    void Start () {

        /*
        PlayerPrefs.SetInt("FangChenMi_Type", 0);
        PlayerPrefs.SetString("FangChenMi_Name", "");
        PlayerPrefs.SetString("FangChenMi_ID", "");
        */

        Game_PublicClassVar.Get_gameServerObj.Obj_ShenFenYanZheng = this.gameObject;

        //初始化认证自身是否绑定身份证
        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        Debug.Log("startname = " + name + " shenfenID = " + shenfenID);

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {

            //不需要进行防沉迷验证,已经验证成功
            WriteYanZheng();
            Debug.Log("startname111 = ");
        }
        else {
            Debug.Log("startname22221 = ");
            Obj_ShenFenRenZhengSucessHintSet.SetActive(false);
            //请求验证当前是否
            Pro_ComStr_3 proComStr_3 = new Pro_ComStr_3();
            proComStr_3.str_1 = SystemInfo.deviceUniqueIdentifier;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000132, proComStr_3);
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void WriteYanZheng() {

        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        //Debug.Log("name = " + name + " shenfenID = " + shenfenID);

        //不需要进行防沉迷验证,已经验证成功
        string xinghaoStr = "";
        for (int i = 0; i < name.Length - 1; i++)
        {
            xinghaoStr = xinghaoStr + "*";
        }

        Obj_Name.GetComponent<InputField>().text = name.Substring(0, 1) + xinghaoStr;
        Obj_ShenFenID.GetComponent<InputField>().text = shenfenID.Substring(0, 2) + "****************";
        Obj_ShenFenRenZhengSucessHintSet.SetActive(true);

    }


    public void YanZheng_New()
    {
        roseName = Obj_Name.GetComponent<InputField>().text;
        roseShenFenID = Obj_ShenFenID.GetComponent<InputField>().text;

        //验证名称
        if (roseName == "" || roseShenFenID == "")
        {
            Debug.Log("名字为空!");
            return;
        }

        YanZheng2 yanzhen = new YanZheng2();
        yanzhen.YanZhengShenFen(roseName, roseShenFenID);
    }

    public void YanZheng() {

        /*
        return;

        //初始化认证自身是否绑定身份证
        string name = PlayerPrefs.GetString("FangChenMi_Name");
        string shenfenID = PlayerPrefs.GetString("FangChenMi_ID");

        if (name != "" && name != null && shenfenID != "" && shenfenID != null)
        {
            //Obj_HintText.GetComponent<Text>().text = "认证身份成功！";
            //测试
            //PlayerPrefs.SetString("FangChenMi_Name", "");
            //PlayerPrefs.SetString("FangChenMi_ID", "");
            //PlayerPrefs.Save();

            Obj_ShenFenRenZhengSucessHintSet.SetActive(false);

            Pro_PlayerYanZheng proPlayerYanZheng = new Pro_PlayerYanZheng();
            proPlayerYanZheng.SheBeiID = SystemInfo.deviceUniqueIdentifier;
            proPlayerYanZheng.Name = name;
            proPlayerYanZheng.ShenFenID = shenfenID;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000131, proPlayerYanZheng);

            return;
        }
        

        Obj_HintText.GetComponent<Text>().text = "";

        roseName = Obj_Name.GetComponent<InputField>().text;
        roseShenFenID = Obj_ShenFenID.GetComponent<InputField>().text;
        */
        //验证名称
        //bool yanZhengName = false;

        roseName = Obj_Name.GetComponent<InputField>().text;
        roseShenFenID = Obj_ShenFenID.GetComponent<InputField>().text;

        if (roseName == "") {
            Debug.Log("名字为空!");
            return;
        }

        
        int yanzhengNum = PlayerPrefs.GetInt("FangChenMi_YanZhengNum");
        if (yanzhengNum >= 100) {
            Obj_HintText.GetComponent<Text>().text = "当天验证次数超过上限,请明天再来！";
            return;
        }
        

        yanzhengNum = yanzhengNum + 1;
        PlayerPrefs.SetInt("FangChenMi_YanZhengNum", yanzhengNum);
        PlayerPrefs.Save();

        YanZheng_New();

        /*
        //CheckIDCard(roseShenFenID)
        //验证身份证号
        if (this.GetComponent<YanZheng2>().YanZhengShenFen(roseName, roseShenFenID)) {

            Debug.Log("身份验证通过！");
            //PlayerPrefs.SetInt("FangChenMi_Type", 1);
            PlayerPrefs.SetString("FangChenMi_Name", roseName);
            PlayerPrefs.SetString("FangChenMi_ID", roseShenFenID);
            PlayerPrefs.Save();

            //获取身份证年龄
            int year = int.Parse(roseShenFenID.Substring(6, 4));
            int month = int.Parse(roseShenFenID.Substring(10, 2));
            int day = int.Parse(roseShenFenID.Substring(12, 2));
            //身份证为15的开启验证
            if (roseShenFenID.Length == 15) {
                year = 18;
            }

            DateTime t1 = new DateTime(year, month, day);
            DateTime t2 = DateTime.Now;

            if (Game_PublicClassVar.Get_wwwSet.DataTime != null)
            {
                t2 = Game_PublicClassVar.Get_wwwSet.DataTime;
            }

            TimeSpan chaSpan = t2 - t1;
            int chayear = (int)(chaSpan.Days / 365);
            Debug.Log("当前年龄:" + chayear);
            PlayerPrefs.SetInt("FangChenMi_Year", chayear);


            Pro_PlayerYanZheng proPlayerYanZheng = new Pro_PlayerYanZheng();
            proPlayerYanZheng.SheBeiID = SystemInfo.deviceUniqueIdentifier;
            proPlayerYanZheng.Name = roseName;
            proPlayerYanZheng.ShenFenID = roseShenFenID;
            Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000131, proPlayerYanZheng);

            //Obj_HintSet.SetActive(false);
            //CloseUI();

            //防沉迷认证
            int nowYearNum = PlayerPrefs.GetInt("FangChenMi_Year");
            if (nowYearNum <= 17)
            {
                Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus = true;
            }

            //表示游客登录
            string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");
            if (shenfenIDStr.Length <= 5)
            {
                Game_PublicClassVar.Get_wwwSet.IfYouKeStatus = true;
            }
        }
        else
        {

            Obj_HintSet.SetActive(false);

            Debug.Log("身份证错误!");
            Obj_HintText.GetComponent<Text>().text = "身份证信息验证失败,请重新确认！";
        }

        */
    }

    public void YanZhengShenFenIDReturn(bool status) {

        if (roseName != "" && roseName != null && roseShenFenID != "" && roseShenFenID != null) {

            if (status)
            {
                Debug.Log("身份验证通过！");
                //PlayerPrefs.SetInt("FangChenMi_Type", 1);
                PlayerPrefs.SetString("FangChenMi_Name", roseName);
                PlayerPrefs.SetString("FangChenMi_ID", roseShenFenID);
                //PlayerPrefs.SetString("FangChenMi_ID", roseShenFenID);
                PlayerPrefs.Save();

                //获取身份证年龄
                int year = int.Parse(roseShenFenID.Substring(6, 4));
                int month = int.Parse(roseShenFenID.Substring(10, 2));
                int day = int.Parse(roseShenFenID.Substring(12, 2));

                //身份证为15的开启验证
                if (roseShenFenID.Length == 15)
                {
                    year = 18;
                }

                DateTime t1 = new DateTime(year, month, day);
                DateTime t2 = DateTime.Now;

                if (Game_PublicClassVar.Get_wwwSet.DataTime != null)
                {
                    t2 = Game_PublicClassVar.Get_wwwSet.DataTime;
                }

                TimeSpan chaSpan = t2 - t1;
                int chayear = (int)(chaSpan.Days / 365);
                //Debug.Log("当前年龄:" + chayear);
                PlayerPrefs.SetInt("FangChenMi_Year", chayear);
                
                Pro_PlayerYanZheng proPlayerYanZheng = new Pro_PlayerYanZheng();
                proPlayerYanZheng.SheBeiID = SystemInfo.deviceUniqueIdentifier;
                proPlayerYanZheng.Name = roseName;
                proPlayerYanZheng.ShenFenID = roseShenFenID;
                Game_PublicClassVar.Get_gameLinkServerObj.SendToServerBuf(10000131, proPlayerYanZheng);

                //Obj_HintSet.SetActive(false);
                //CloseUI();

                //防沉迷认证
                int nowYearNum = PlayerPrefs.GetInt("FangChenMi_Year");
                if (nowYearNum <= 17)
                {
                    Game_PublicClassVar.Get_wwwSet.IfWeiChengNianStatus = true;
                }
                else {
                    Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus = false;
                    Game_PublicClassVar.Get_wwwSet.IfYouKeStatus = false;
                }

                //表示游客登录
                string shenfenIDStr = PlayerPrefs.GetString("FangChenMi_ID");
                if (shenfenIDStr.Length <= 5)
                {
                    Game_PublicClassVar.Get_wwwSet.IfYouKeStatus = true;
                }

                Obj_ShenFenRenZhengSucessHintSet.SetActive(true);

            }
            else
            {

                Obj_HintSet.SetActive(false);
                Obj_ShenFenRenZhengSucessHintSet.SetActive(false);
                Debug.Log("身份证错误!");
                Obj_HintText.SetActive(true);
                Obj_HintText.GetComponent<Text>().text = "身份证信息验证失败,请重新确认！";
            }
        }
    }

    public void CloseUI() {

        //如何当前已经提示 取消实名制就退出游戏
        if (Game_PublicClassVar.Get_wwwSet.ShiMingHintStatus) {
            //Game_PublicClassVar.Get_wwwSet.ExitGame();
            return;
        }

        this.gameObject.SetActive(false);

    }

    public bool CheckIDCard(string Id)
    {
        if (Id.Length == 18)
        {
            bool check = CheckIDCard18(Id);
            return check;
        }
        else if (Id.Length == 15)
        {
            bool check = CheckIDCard15(Id);
            return check;
        }
        else
        {
            return false;
        }
    }

    public bool CheckIDCard18(string Id)
    {
        long n = 0;
        if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }

        string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证
        }

        string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
        string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
        char[] Ai = Id.Remove(17).ToCharArray();
        int sum = 0;
        for (int i = 0; i < 17; i++)
        {
            sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
        }
        int y = -1;
        DivRem(sum, 11, out y);
        if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
        {
            return false;//校验码验证
        }
        return true;//符合GB11643-1999标准
    }

    public int DivRem(int a, int b, out int result)
    {
        result = a % b;
        return (a / b);
    }

    public bool CheckIDCard15(string Id)
    {
        long n = 0;
        if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
        {
            return false;//数字验证
        }
        string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
        if (address.IndexOf(Id.Remove(2)) == -1)
        {
            return false;//省份验证
        }
        string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");
        DateTime time = new DateTime();
        if (DateTime.TryParse(birth, out time) == false)
        {
            return false;//生日验证
        }
        return true;//符合15位身份证标准
    }
}
