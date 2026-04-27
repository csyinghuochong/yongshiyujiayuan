using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

//移动UI的专用脚本
public class UI_Move : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public GameObject MoveObj;

    //背包移动开关
    public bool BagMoveStatus;
    private Vector3 firstmousePosition;
    private bool firstmouseStatus;

    private Vector3 moustPosition_Now;
    private Vector3 moustPosition_Last;
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //判定背包移动是否开启
        if (BagMoveStatus == true)
        {

            //获取第一次按下时鼠标的位置
            if (firstmouseStatus == true)
            {

                firstmousePosition = Input.mousePosition;
                firstmouseStatus = false;

                //获取鼠标位置
                moustPosition_Now = Input.mousePosition;
                moustPosition_Last = Input.mousePosition;

            }

            //设置UI位置
            moustPosition_Now = Input.mousePosition;
            MoveObj.transform.localPosition = MoveObj.transform.localPosition + (moustPosition_Now - moustPosition_Last) * 1;
            moustPosition_Last = Input.mousePosition;
        }
        else
        {
            firstmouseStatus = true;
        }

	}

    //该按钮处于鼠标按下状态
    public void OnDrag(PointerEventData eventData)
    {
        BagMoveStatus = true;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    //该按钮出去鼠标松开状态
    public void OnEndDrag(PointerEventData eventData)
    {
        BagMoveStatus = false;
    }
}
