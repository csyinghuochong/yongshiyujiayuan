using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class YongHuYinSi : MonoBehaviour
{

    public GameObject TextYinSi_0;

    void Awake()
    {
        UnityEngine.Debug.Log("TextYinSi Awake");
        TextYinSi_0.SetActive(false);
        UILoginHelper.ShowTextList(this.TextYinSi_0);
    }

    void Start()
    {
        UnityEngine.Debug.Log("TextYinSi Start");
       
    }
}