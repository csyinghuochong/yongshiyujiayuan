using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using CodeStage.AntiCheat.ObscuredTypes;

public class UI_EquipXiLianItem : MonoBehaviour {


    public GameObject selectStatus;
    public GameObject container;
    public GameObject propertyText;
    public XiLianResult currentResult;
    public Action<UI_EquipXiLianItem> selectXiLianHandle;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	 
	public void InitData(XiLianResult result, Action<UI_EquipXiLianItem> callback)
	{
        currentResult = result;
        selectXiLianHandle = callback;

        List<ObscuredString> propertyList = this.GeneratePropertyList(result);
        if (propertyList.Count == 0)
        {
            propertyList.Add(result.hideProListStr);
        }

        for (int i = 0; i < propertyList.Count; i++)
        {
            GameObject textItem = MonoBehaviour.Instantiate(this.propertyText);

            if (propertyList[i].ToString().Contains("隐藏")) {
                textItem.GetComponent<Text>().color = new Color(1,0.31f,0);
            }
            //Debug.Log("propertyList[i].ToString() = " + propertyList[i].ToString());
            if (propertyList[i].ToString().Contains(",") == false)
            {
                textItem.GetComponent<Text>().text = propertyList[i];
                textItem.transform.SetParent(this.container.transform);
                textItem.transform.localPosition = Vector3.zero;
                textItem.transform.localScale = new Vector3(1, 1, 1);
            }
            else {
                textItem.GetComponent<Text>().text = "无";
                textItem.transform.SetParent(this.container.transform);
                textItem.transform.localPosition = Vector3.zero;
                textItem.transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    public void OnClickButton()
    {
        this.selectXiLianHandle(this);
    }

    public void OnSelect(bool value)
    {
        selectStatus.SetActive(value);
    }

    // "2,7;4,4"  
    private List<ObscuredString> GeneratePropertyList(XiLianResult result)
	{
        //Debug.Log("result = " + result.hideProListStr);
        string ItemID = result.equipItemId;
        string hidePropertyStr = result.hideProListStr;

        List<ObscuredString> propertyList = new List<ObscuredString>();
        string[] hideProperty = hidePropertyStr.Split(';');
        for (int i = 0; i <= hideProperty.Length - 1; i++)
        {

            string[] hidePropertyList = hideProperty[i].Split(',');
            if (hidePropertyList.Length >= 2)
            {
                string hidePropertyType = hideProperty[i].Split(',')[0];
                string hidePropertyValue = hideProperty[i].Split(',')[1];

                switch (hidePropertyType)
                {
                    //血量上限
                    case "1":
                        propertyList.Add("血量+"+ hidePropertyValue);
                        break;
                    //物理攻击最大值
                    case "2":
                        propertyList.Add("攻击+" + hidePropertyValue);
                        break;
                    case "3":
                        //物理防御最大值
                        propertyList.Add("物防+" + hidePropertyValue);
                        break;
                    //魔法防御最大值
                    case "4":
                        propertyList.Add("魔防+" + hidePropertyValue);
                        break;
                    //法术
                    case "5":
                        propertyList.Add("技能伤害+" + hidePropertyValue);
                        break;
                        
                }
            }

            //获取和显示隐藏属性
            string proStr = Game_PublicClassVar.Get_function_UI.ShowHintPro(hideProperty[i], 1);
            if (proStr != "" && proStr != "0")
            {
                if (proStr.Split(',').Length == 1) {
                    propertyList.Add(proStr);
                }
                else
                {
                    propertyList.Add("无");
                }
            }
            //获取和显示隐藏技能
            proStr = Game_PublicClassVar.Get_function_UI.ShowHintSkill(hideProperty[i]);
            if (proStr != "" && proStr != "0")
            {
                if (proStr.Split(',').Length == 1)
                {
                    propertyList.Add(proStr);
                }
                else {
                    propertyList.Add("无");
                }
            }
            
        }

        return propertyList;
	}

}
