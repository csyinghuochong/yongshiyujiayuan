using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_EnterGameDataUpdata : MonoBehaviour {

    public GameObject Obj_UpdataDataText;        //要塞战斗结果Obj
    public GameObject Btn_EnterGame;

    public GameObject Obj_EnterProImg;          //进入游戏图标进度
    public GameObject Obj_EnterProText;         //进入游戏文字

    private int nowValue;
    private int ValueMax;

    private float addProSum;

	// Use this for initialization
	void Start () {
        //Obj_UpdataDataText.GetComponent<Text>().text = "游戏初始数据加载中……";
        //Btn_EnterGame.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        /*
	    //获取当前进度
        ValueMax = Game_PublicClassVar.Get_wwwSet.updataNumSum;
        nowValue = Game_PublicClassVar.Get_wwwSet.updataNum;
        Obj_UpdataDataText.GetComponent<Text>().text = "游戏初始数据加载进度：" + nowValue + "/" + ValueMax;

        //设置当前进度
        addProSum = addProSum + Time.deltaTime;
        if (addProSum >= 3) {
            addProSum = 3.0f;
        }

        float proValue = addProSum / 3 + ValueMax / nowValue * 0.2f;

        //加载完配置文件后显示进入游戏按钮
        if (ValueMax >= nowValue)
        {
            proValue = 1;
            Btn_EnterGame.SetActive(true);
            this.gameObject.SetActive(false);
        }
        //设置进度
        Obj_EnterProImg.GetComponent<Image>().fillAmount = proValue;
        Obj_EnterProText.GetComponent<Text>().text = (int)(proValue*100)+"%";
        */
	}
}
