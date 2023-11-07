using UnityEngine;
using System.Collections;

//此脚本存放游戏内固定的全局变量值
public class Game_AllValue : MonoBehaviour
{

    public string Get_XmlPath = Application.dataPath + "/GameData/Xml/Get_Xml/";        //只读Xml的位置
    public string Set_XmlPath = Application.dataPath + "/GameData/Xml/Set_Xml/";        //可读可存Xml的位置

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
