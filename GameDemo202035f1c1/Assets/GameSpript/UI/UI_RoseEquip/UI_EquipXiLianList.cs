using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weijing;
using System;

public class UI_EquipXiLianList : MonoBehaviour {

	// Use this for initialization

	public GameObject mContainer;
	public GameObject mXilianItem;
	private XiLianResult selectProperty;
	private Action<XiLianResult> callBack;
	private List<XiLianResult> mXilianResults;
	private UI_EquipXiLianItem selectItem;


	void Start () {


		//打开通用UI
		Game_PublicClassVar.Get_function_UI.AddUI_CommonHuoBiSet(this.gameObject, "101");

	}

	// Update is called once per frame
	void Update () {
		
	}

	private void ShowXilianList()
	{
		for( int i = 0; i < this.mXilianResults.Count; i++ )
        {
			GameObject xilianResultItem = (GameObject)MonoBehaviour.Instantiate(this.mXilianItem);
			xilianResultItem.transform.SetParent(this.mContainer.transform);
			xilianResultItem.transform.localPosition = Vector3.zero;
			xilianResultItem.transform.localScale = new Vector3(1, 1, 1);
			xilianResultItem.GetComponent<UI_EquipXiLianItem>().InitData(this.mXilianResults[i], this.OnClickCallBack);
		}
	}

	private void OnClickCallBack(UI_EquipXiLianItem result)
	{
		if (selectItem)
			selectItem.OnSelect( false );
		selectItem = result;
		selectItem.OnSelect( true );
	}

	public void OnClickCloseButton()
	{
		GameObject.Destroy( this.gameObject );
	}

	public void OnClickConfirmButton()
	{
		if (!selectItem)
			return;
		this.callBack( this.selectItem.currentResult);
		GameObject.Destroy(this.gameObject);
	}

	public void InitData(List<XiLianResult> xiLianResults, Action<XiLianResult> handle)
	{
		mXilianResults = xiLianResults;
		callBack = handle;
		if (this.gameObject == null)
			return;
		this.ShowXilianList();
	}

}
