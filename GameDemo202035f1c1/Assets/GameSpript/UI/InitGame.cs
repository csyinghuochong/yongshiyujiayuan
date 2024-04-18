using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{


    public GameObject Obj_HuaWeiYinSi;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("InitGame.Start");

        //打开隐私协议
        if (PlayerPrefs.GetString(YinSi.PlayerPrefsYinSi) != YinSi.YinSiValue)
        {
            Debug.Log("InitGame.YinSi false");
            Obj_HuaWeiYinSi.SetActive(true);
        }
        else
        {
            Debug.Log("InitGame.YinSi true");
            Obj_HuaWeiYinSi.GetComponent<YinSi>().onRequestPermissionsResult("1_1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
