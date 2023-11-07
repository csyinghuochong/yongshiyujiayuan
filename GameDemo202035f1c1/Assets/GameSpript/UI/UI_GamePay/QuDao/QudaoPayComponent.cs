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

}