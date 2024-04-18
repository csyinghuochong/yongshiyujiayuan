using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitGame : MonoBehaviour
{


    public GameObject Obj_HuaWeiYinSi;


    // Start is called before the first frame update
    void Start()
    {
        //打开隐私协议
        if (PlayerPrefs.GetString(YinSi.PlayerPrefsYinSi) != YinSi.YinSiValue)
        {
            Obj_HuaWeiYinSi.SetActive(true);
        }
        else
        {
            Obj_HuaWeiYinSi.GetComponent<YinSi>().onRequestPermissionsResult("1_1");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
