using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GameYinDao : MonoBehaviour {

    //public GameObject Obj_YinDaoSet;
    public GameObject Obj_YinDaoSet_MaoXian;
    public GameObject Obj_YinDaoSet_ShiQu;
    public GameObject Obj_YinDaoSet_ZhuaPu;
    public GameObject Obj_YinDaoSet_RosePet;
    public GameObject Obj_YinDaoSet_RoseSkill;
    public GameObject Obj_YinDaoSet_RoseMake;
    public GameObject Obj_YinDaoSet_RoseSetting;
    public GameObject Obj_YinDaoSet_RoseMap;

    //状态 防止二次引导
    private bool StopYinDao;            //超过15级 不进行任何引导
    private bool ShiQu_YinDaoStatus;
    private bool RosePet_YinDaoStatus;
    
    // Use this for initialization
    void Start () {

        //界面适配
        //Game_PublicClassVar.Get_function_UI.UIFitResolutionRatio(Obj_YinDaoSet);

        Obj_YinDaoSet_MaoXian.SetActive(false);
        Obj_YinDaoSet_ShiQu.SetActive(false);
        Obj_YinDaoSet_ZhuaPu.SetActive(false);
        Obj_YinDaoSet_RosePet.SetActive(false);
        Obj_YinDaoSet_RoseSkill.SetActive(false);
        Obj_YinDaoSet_RoseMake.SetActive(false);
        Obj_YinDaoSet_RoseSetting.SetActive(false);
        Obj_YinDaoSet_RoseMap.SetActive(false);
        ShowYinDao();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    //一些通用的引导
    public void ShowYinDao() {

        //展示开始冒险  1级显示
        int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
        if (roseLv == 1) {
            if (Application.loadedLevelName == "EnterGame") {
                Obj_YinDaoSet_MaoXian.SetActive(true);
            }
        }

        if (roseLv >= 15) {
            StopYinDao = true;
        }
    }


    //展示拾取掉落
    public void ShowYinDao_ShiQu(bool ifShow = true) {

        if (StopYinDao) {
            return;
        }

        if (ifShow)
        {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            if (roseLv <= 2)
            {
                Obj_YinDaoSet_ShiQu.SetActive(true);
            }
        }
        else {
            Obj_YinDaoSet_ShiQu.SetActive(false);
        }
    }

    //抓捕
    public void ShowYinDao_ZhuaPu(bool ifShow = true) {

        if (StopYinDao)
        {
            return;
        }

        if (ifShow)
        {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            //8级展示宠物抓捕
            if (roseLv == 8)
            {
                //获取当前宠物数量是否为0
                if (Game_PublicClassVar.Get_function_AI.Pet_ReturnRoseListFirst() == "0") {
                    Obj_YinDaoSet_ZhuaPu.SetActive(true);
                }
            }
        }
        else {
            Obj_YinDaoSet_ZhuaPu.SetActive(false);
        }
    }


    //宠物
    public void ShowYinDao_RosePet(bool ifShow = true)
    {

        if (StopYinDao)
        {
            return;
        }

        if (ifShow)
        {
            int roseLv = Game_PublicClassVar.Get_function_Rose.GetRoseLv();
            //8级展示宠物抓捕
            if (roseLv <= 8)
            {
                //每次打开游戏只引导一次
                if (!RosePet_YinDaoStatus) {
                    RosePet_YinDaoStatus = true;
                    Obj_YinDaoSet_RosePet.SetActive(true);
                }
            }
        }
        else
        {
            Obj_YinDaoSet_RosePet.SetActive(false);
        }
    }


    //技能
    public void ShowYinDao_RoseSkill(bool ifShow = true)
    {
        if (ifShow)
        {

            Obj_YinDaoSet_RoseSkill.SetActive(true);
        }
        else
        {
            Obj_YinDaoSet_RoseSkill.SetActive(false);
        }
    }

    //制作
    public void ShowYinDao_RoseMake(bool ifShow = true)
    {
        if (ifShow)
        {

            Obj_YinDaoSet_RoseMake.SetActive(true);
        }
        else
        {
            Obj_YinDaoSet_RoseMake.SetActive(false);
        }
    }

    //设置
    public void ShowYinDao_RoseSetting(bool ifShow = true)
    {
        if (ifShow)
        {

            Obj_YinDaoSet_RoseSetting.SetActive(true);
        }
        else
        {
            Obj_YinDaoSet_RoseSetting.SetActive(false);
        }
    }

    //地图
    public void ShowYinDao_RoseMap(bool ifShow = true)
    {
        if (ifShow)
        {

            Obj_YinDaoSet_RoseMap.SetActive(true);
        }
        else
        {
            Obj_YinDaoSet_RoseMap.SetActive(false);
        }
    }

}
