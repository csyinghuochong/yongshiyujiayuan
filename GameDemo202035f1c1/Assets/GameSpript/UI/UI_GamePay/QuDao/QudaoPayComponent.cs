using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml;
using UnityEngine;


class QudaoPayComponent : MonoBehaviour
{

    public delegate void QudaoPayCCallback(string state);

    public QudaoPayCCallback qudaoPayCallback;

    public void QudaoPay(string state)
    {
        Debug.Log("QudaoPay: " + state );
    }
    
    public void QudaoPayCallback(string result)
    {
        qudaoPayCallback(result);
        //告诉服务器已经支付 等待返回结果
        //再监听结果 进行发放奖励 实际上都是独立的
        if (result == "支付成功")
        {
            Debug.Log("渠道支付成功");
        }
        else
        {
            Debug.Log("渠道支付失败");
        }
    }

}